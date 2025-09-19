using Beacon.Data;
using Beacon.Models;
using Beacon.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IO;

namespace Beacon.Controllers
{
    [Authorize]
    public class AlertPostsController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly IWebHostEnvironment _env;

        public AlertPostsController(AppDbContext context, UserManager<User> userManager, IWebHostEnvironment env)
        {
            _context = context;
            _userManager = userManager;
            _env = env;
        }

        // =======================
        // List all alerts (public)
        // =======================
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            var alerts = await _context.AlertPosts
                .Include(a => a.Admin)
                .OrderByDescending(a => a.CreatedAt)
                .ToListAsync();

            return View(alerts);
        }

        // =======================
        // Details (current: auth)
        // =======================
        [Authorize]
        public async Task<IActionResult> Details(string id)
        {
            if (id == null) return NotFound();

            var alert = await _context.AlertPosts
                .Include(a => a.Admin)
                .FirstOrDefaultAsync(a => a.AlertId == id);

            if (alert == null) return NotFound();

            return View(alert);
        }

        // =======================
        // Create (Admin only)
        // =======================
        [Authorize(Roles = "admin")]
        [HttpGet]
        public IActionResult Create() => View(new AlertPostCreateViewModel());

        [Authorize(Roles = "admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AlertPostCreateViewModel vm)
        {
            if (!ModelState.IsValid) return View(vm);

            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Unauthorized();

            string? savedUrl = null;

            if (vm.AlertImage is { Length: > 0 })
            {
                const long maxBytes = 2 * 1024 * 1024;
                if (vm.AlertImage.Length > maxBytes)
                {
                    ModelState.AddModelError(nameof(vm.AlertImage), "Image must be ≤ 2MB.");
                    return View(vm);
                }

                var ext = Path.GetExtension(vm.AlertImage.FileName).ToLowerInvariant();
                var okExt = new[] { ".jpg", ".jpeg", ".png", ".webp" };
                var okTypes = new[] { "image/jpeg", "image/png", "image/webp" };
                if (!okExt.Contains(ext) || !okTypes.Contains(vm.AlertImage.ContentType))
                {
                    ModelState.AddModelError(nameof(vm.AlertImage), "Only JPG, PNG, or WebP images are allowed.");
                    return View(vm);
                }

                var uploadsFolder = Path.Combine(_env.WebRootPath, "uploads", "alerts");
                Directory.CreateDirectory(uploadsFolder);

                var fileName = $"{Guid.NewGuid():N}{ext}";
                var filePath = Path.Combine(uploadsFolder, fileName);

                await using (var stream = System.IO.File.Create(filePath))
                    await vm.AlertImage.CopyToAsync(stream);

                var baseUrl = $"{Request.Scheme}://{Request.Host}";
                savedUrl = $"{baseUrl}/uploads/alerts/{fileName}";
            }

            var entity = new AlertPost
            {
                AdminId = user.Id,
                Type = vm.Type.ToString(),   // enum -> string
                Content = vm.Content,
                Location = vm.Location,
                AlertImageUrl = savedUrl,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _context.AlertPosts.Add(entity);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Details), new { id = entity.AlertId });
        }

        // =======================
        // Edit (Admin only)
        // =======================
        [Authorize(Roles = "admin")]
        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            if (string.IsNullOrWhiteSpace(id)) return NotFound();

            var alert = await _context.AlertPosts.AsNoTracking().FirstOrDefaultAsync(a => a.AlertId == id);
            if (alert == null) return NotFound();

            var vm = new AlertPostEditViewModel
            {
                AlertId = alert.AlertId,
                Type = Enum.TryParse<AlertType>(alert.Type, ignoreCase: true, out var at) ? at : AlertType.Announcement,
                Content = alert.Content,
                Location = alert.Location,
                ExistingImageUrl = alert.AlertImageUrl
            };

            return View(vm);
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, AlertPostEditViewModel vm)
        {
            if (string.IsNullOrWhiteSpace(id)) return NotFound();
            if (!ModelState.IsValid) return View(vm);
            if (!string.Equals(id, vm.AlertId, StringComparison.Ordinal))
                return BadRequest("Route id and form id mismatch.");

            var alert = await _context.AlertPosts.FirstOrDefaultAsync(a => a.AlertId == id);
            if (alert == null) return NotFound();

            // Update scalar fields
            alert.Type = vm.Type.ToString();
            alert.Content = vm.Content;
            alert.Location = vm.Location;

            // Handle image: new upload wins over RemoveImage
            if (vm.AlertImage is { Length: > 0 })
            {
                const long maxBytes = 2 * 1024 * 1024;
                if (vm.AlertImage.Length > maxBytes)
                {
                    vm.ExistingImageUrl = alert.AlertImageUrl; // keep for redisplay
                    ModelState.AddModelError(nameof(vm.AlertImage), "Image must be ≤ 2MB.");
                    return View(vm);
                }

                var ext = Path.GetExtension(vm.AlertImage.FileName).ToLowerInvariant();
                var okExt = new[] { ".jpg", ".jpeg", ".png", ".webp" };
                var okTypes = new[] { "image/jpeg", "image/png", "image/webp" };
                if (!okExt.Contains(ext) || !okTypes.Contains(vm.AlertImage.ContentType))
                {
                    vm.ExistingImageUrl = alert.AlertImageUrl;
                    ModelState.AddModelError(nameof(vm.AlertImage), "Only JPG, PNG, or WebP images are allowed.");
                    return View(vm);
                }

                var uploadsFolder = Path.Combine(_env.WebRootPath, "uploads", "alerts");
                Directory.CreateDirectory(uploadsFolder);

                var fileName = $"{Guid.NewGuid():N}{ext}";
                var filePath = Path.Combine(uploadsFolder, fileName);

                await using (var stream = System.IO.File.Create(filePath))
                    await vm.AlertImage.CopyToAsync(stream);

                var baseUrl = $"{Request.Scheme}://{Request.Host}";
                var newUrl = $"{baseUrl}/uploads/alerts/{fileName}";

                // delete old file if any
                if (!string.IsNullOrEmpty(alert.AlertImageUrl))
                    TryDeletePhysicalFile(alert.AlertImageUrl);

                alert.AlertImageUrl = newUrl;
            }
            else if (vm.RemoveImage)
            {
                if (!string.IsNullOrEmpty(alert.AlertImageUrl))
                    TryDeletePhysicalFile(alert.AlertImageUrl);

                alert.AlertImageUrl = null;
            }
            // else: keep existing image

            alert.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Details), new { id = alert.AlertId });
        }

        // =======================
        // Delete (Admin only)
        // =======================
        [Authorize(Roles = "admin")]
        [HttpGet]
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null) return NotFound();

            var alert = await _context.AlertPosts
                .Include(a => a.Admin)
                .FirstOrDefaultAsync(a => a.AlertId == id);

            if (alert == null) return NotFound();

            return View(alert);
        }

        [Authorize(Roles = "admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var alert = await _context.AlertPosts.FindAsync(id);
            if (alert != null)
            {
                if (!string.IsNullOrEmpty(alert.AlertImageUrl))
                    TryDeletePhysicalFile(alert.AlertImageUrl);

                _context.AlertPosts.Remove(alert);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        // =======================
        // Helpers
        // =======================
        private void TryDeletePhysicalFile(string urlOrPath)
        {
            try
            {
                string relativePath;
                if (Uri.TryCreate(urlOrPath, UriKind.Absolute, out var uri))
                {
                    // absolute URL -> /uploads/alerts/xyz.jpg
                    relativePath = uri.AbsolutePath.TrimStart('/');
                }
                else
                {
                    // already relative
                    relativePath = urlOrPath.TrimStart('/');
                }

                var safeRel = relativePath.Replace('/', Path.DirectorySeparatorChar);
                var full = Path.Combine(_env.WebRootPath, safeRel);

                if (System.IO.File.Exists(full))
                    System.IO.File.Delete(full);
            }
            catch
            {
                // swallow on purpose
            }
        }
    }
}
