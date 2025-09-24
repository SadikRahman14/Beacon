using System.Text;
using Beacon.Data;
using Beacon.Models;
using Microsoft.EntityFrameworkCore;

namespace Beacon.Data.Seeders
{
    public static class LocationCountsSeeder
    {
        // Set to true after first successful run to avoid reseeding
        private static bool _done = false;

        public static async Task SeedAsync(IServiceProvider services, string csvPath, ILogger? logger = null)
        {
            if (_done) return;

            using var scope = services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            if (!File.Exists(csvPath))
            {
                logger?.LogWarning("CSV not found at {Path}", csvPath);
                return;
            }

            int addedLoc = 0, upserted = 0, total = 0;

            using var stream = File.OpenRead(csvPath);
            using var reader = new StreamReader(stream, Encoding.UTF8, detectEncodingFromByteOrderMarks: true);

            var header = await reader.ReadLineAsync();
            if (header is null) return;

            var cols = header.Split(',').Select(c => c.Trim()).ToArray();
            int idxName = Array.FindIndex(cols, c => c.Equals("NameEn", StringComparison.OrdinalIgnoreCase));
            int idxAlerts = Array.FindIndex(cols, c => c.Equals("Alerts", StringComparison.OrdinalIgnoreCase));
            int idxComplaints = Array.FindIndex(cols, c => c.Equals("Complaints", StringComparison.OrdinalIgnoreCase));
            int idxDev = Array.FindIndex(cols, c => c.Equals("DevUpdates", StringComparison.OrdinalIgnoreCase));
            if (idxName < 0 || idxAlerts < 0 || idxComplaints < 0 || idxDev < 0)
                throw new InvalidOperationException("CSV must have headers: NameEn, Alerts, Complaints, DevUpdates");

            string? line;
            while ((line = await reader.ReadLineAsync()) != null)
            {
                total++;
                if (string.IsNullOrWhiteSpace(line)) continue;

                var parts = line.Split(',');
                if (parts.Length <= Math.Max(idxName, Math.Max(idxAlerts, Math.Max(idxComplaints, idxDev)))) continue;

                var name = parts[idxName].Trim();
                if (string.IsNullOrWhiteSpace(name)) continue;

                int alerts = int.TryParse(parts[idxAlerts].Trim(), out var a) ? Math.Max(0, a) : 0;
                int complaints = int.TryParse(parts[idxComplaints].Trim(), out var c) ? Math.Max(0, c) : 0;
                int dev = int.TryParse(parts[idxDev].Trim(), out var d) ? Math.Max(0, d) : 0;

                // ensure CanonicalLocation
                var loc = await db.CanonicalLocations.FirstOrDefaultAsync(x => x.NameEn.ToLower() == name.ToLower());
                if (loc == null)
                {
                    loc = new CanonicalLocation { NameEn = name };
                    db.CanonicalLocations.Add(loc);
                    await db.SaveChangesAsync(); // get Id
                    addedLoc++;
                }

                // upsert seed stats
                var seed = await db.LocationSeedStats.FirstOrDefaultAsync(s => s.CanonicalLocationId == loc.Id);
                if (seed == null)
                {
                    seed = new LocationSeedStats
                    {
                        CanonicalLocationId = loc.Id,
                        Alerts = alerts,
                        Complaints = complaints,
                        DevUpdates = dev
                    };
                    db.LocationSeedStats.Add(seed);
                }
                else
                {
                    seed.Alerts = alerts;
                    seed.Complaints = complaints;
                    seed.DevUpdates = dev;
                    db.LocationSeedStats.Update(seed);
                }
                upserted++;
            }

            await db.SaveChangesAsync();
            logger?.LogInformation("Seed counts done. Locations added: {Added}, upserted: {Upserted}, lines: {Total}", addedLoc, upserted, total);
            _done = true;
        }
    }
}
