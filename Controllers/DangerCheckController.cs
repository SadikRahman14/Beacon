using System;
using System.Linq;
using System.Threading.Tasks;
using Beacon.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Beacon.Controllers
{
    [AllowAnonymous]
    public class DangerCheckController : Controller
    {
        private readonly AppDbContext _db;

        public DangerCheckController(AppDbContext db)
        {
            _db = db;
        }

        // Page with the dropdown of locations
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var locations = await _db.CanonicalLocations
                .OrderBy(x => x.NameEn)
                .Select(x => new { x.Id, x.NameEn })
                .ToListAsync();

            ViewBag.Locations = locations;
            return View();
        }

        // Very small scoring API: /api/danger/score?locationId=123
        [HttpGet("/api/danger/score")]
        public async Task<IActionResult> GetScore([FromQuery] int locationId)
        {
            var loc = await _db.CanonicalLocations.FindAsync(locationId);
            if (loc == null) return NotFound(new { error = "Location not found." });

            var name = loc.NameEn.Trim();
            var nameLower = name.ToLower();
            var recentCutoff = DateTime.UtcNow.AddDays(-30);

            // 1) Seed counts (from CSV)
            var seed = await _db.LocationSeedStats
                .AsNoTracking()
                .FirstOrDefaultAsync(s => s.CanonicalLocationId == loc.Id);

            int seedAlerts = seed?.Alerts ?? 0;
            int seedComplaints = seed?.Complaints ?? 0;
            int seedDevUpdates = seed?.DevUpdates ?? 0;

            // 2) Live counts (currently AlertPosts only)
            int liveAllAlerts = await _db.AlertPosts
                .AsNoTracking()
                .Where(a => a.Location != null && a.Location.Trim().ToLower() == nameLower)
                .CountAsync();

            int liveRecentAlerts = await _db.AlertPosts
                .AsNoTracking()
                .Where(a => a.CreatedAt >= recentCutoff &&
                            a.Location != null && a.Location.Trim().ToLower() == nameLower)
                .CountAsync();

            // 3) Totals (seed + live)
            int lifetimeAlerts = seedAlerts + liveAllAlerts;
            int recentAlerts = liveRecentAlerts; // seed is treated as all-time; recency only from live

            // 4) Simple, explainable score:
            //    Score = 3*RecentAlerts + (LifetimeAlerts - RecentAlerts) + 2*Complaints - 1*DevUpdates
            int lifetimeMinusRecent = Math.Max(0, lifetimeAlerts - recentAlerts);
            int score = (recentAlerts * 3) + lifetimeMinusRecent + (seedComplaints * 2) - seedDevUpdates;

            string level = score <= 3 ? "Low" : score <= 7 ? "Medium" : "High";

            return Ok(new
            {
                location = loc.NameEn,
                level,
                score,
                recent = new { alerts = recentAlerts, days = 30 },
                lifetime = new { alerts = lifetimeAlerts, complaints = seedComplaints, devUpdates = seedDevUpdates },
                explanation = $"Seed+Live → Alerts: {lifetimeAlerts} (recent {recentAlerts}); Complaints: {seedComplaints}; Mitigations: {seedDevUpdates}."
            });
        }


        [HttpGet("/DangerCheck/Probe")]
            public async Task<IActionResult> Probe([FromQuery] string name)
            {
                if (string.IsNullOrWhiteSpace(name))
                    return BadRequest(new { error = "Pass ?name=Motijheel" });

                var loc = await _db.CanonicalLocations
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.NameEn.ToLower() == name.Trim().ToLower());

                if (loc == null)
                    return NotFound(new { error = "Canonical location not found", name });

                var seed = await _db.LocationSeedStats
                    .AsNoTracking()
                    .FirstOrDefaultAsync(s => s.CanonicalLocationId == loc.Id);

                // Also show a couple of live AlertPosts that match exactly, to verify string matching
                var exactMatches = await _db.AlertPosts
                    .AsNoTracking()
                    .Where(a => a.Location != null && a.Location.Trim().ToLower() == loc.NameEn.ToLower())
                    .OrderByDescending(a => a.CreatedAt)
                    .Select(a => new { a.AlertId, a.Location, a.CreatedAt })
                    .Take(3)
                    .ToListAsync();

                return Ok(new
                {
                    canonical = new { loc.Id, loc.NameEn },
                    seed = new
                    {
                        exists = seed != null,
                        seed?.Alerts,
                        seed?.Complaints,
                        seed?.DevUpdates
                    },
                    liveSample = exactMatches
                });
            }
        }
}
