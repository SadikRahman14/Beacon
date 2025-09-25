using Beacon.Data;
using Beacon.Models;
using Beacon.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Beacon.Controllers
{
    [AllowAnonymous] // NEW: allow everyone to see the home page
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AppDbContext _context; // NEW

        public HomeController(ILogger<HomeController> logger, AppDbContext context) // NEW: inject db
        {
            _logger = logger;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            const int take = 6;

            var alertRows = await _context.AlertPosts
                .AsNoTracking()
                .Include(a => a.Admin)
                .OrderByDescending(a => a.CreatedAt)
                .Take(take)
                .Select(a => new
                {
                    a.AlertId,
                    a.Type,
                    a.Content,
                    a.Location,
                    ImageUrl = a.AlertImageUrl,
                    a.CreatedAt,
                    AdminFirst = a.Admin!.FirstName,
                    AdminLast = a.Admin!.LastName,
                    AdminEmail = a.Admin!.Email,
                    AdminAvatar = a.Admin!.ProfileImageUrl
                })
                .ToListAsync();

            var alerts = alertRows.Select(a =>
                new HomeIndexViewModel.AlertItem(
                    a.AlertId,
                    a.Type,
                    a.Content,
                    a.Location,
                    a.ImageUrl,
                    a.CreatedAt,
                    string.IsNullOrWhiteSpace(a.AdminFirst) && string.IsNullOrWhiteSpace(a.AdminLast)
                        ? (a.AdminEmail ?? "Administrator")
                        : $"{a.AdminFirst} {a.AdminLast}".Trim(),
                    a.AdminAvatar
                )
            ).ToList();

            var updateRows = await _context.DevUpdates
                .AsNoTracking()
                .Include(d => d.Admin)
                .OrderByDescending(d => d.CreatedAt)
                .Take(take)
                .Select(d => new
                {
                    d.DevUpdateId,
                    d.Type,
                    d.Content,
                    d.Location,
                    d.ImageUrl,
                    d.CreatedAt,
                    AdminFirst = d.Admin!.FirstName,
                    AdminLast = d.Admin!.LastName,
                    AdminEmail = d.Admin!.Email,
                    AdminAvatar = d.Admin!.ProfileImageUrl
                })
                .ToListAsync();

            var updates = updateRows.Select(d =>
                new HomeIndexViewModel.UpdateItem(
                    d.DevUpdateId,
                    d.Type,
                    d.Content,
                    d.Location,
                    d.ImageUrl,
                    d.CreatedAt,
                    string.IsNullOrWhiteSpace(d.AdminFirst) && string.IsNullOrWhiteSpace(d.AdminLast)
                        ? (d.AdminEmail ?? "Administrator")
                        : $"{d.AdminFirst} {d.AdminLast}".Trim(),
                    d.AdminAvatar
                )
            ).ToList();

            var vm = new HomeIndexViewModel { Alerts = alerts, Updates = updates };
            return View(vm);
        }


        public IActionResult Privacy() => View();

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
