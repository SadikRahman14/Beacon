using Beacon.Data;
using Beacon.Models;
using Beacon.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Beacon.Controllers
{
    [Authorize]
    public class DevUpdatesController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly IWebHostEnvironment _env;

        public DevUpdatesController(AppDbContext context, UserManager<User> userManager, IWebHostEnvironment env)
        {
            _context = context;
            _userManager = userManager;
            _env = env;
        }

        // =======================
        // Index (public)
        // =======================
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            var list = await _context.DevUpdates
                .Include(d => d.Admin)
                .OrderByDescending(d => d.CreatedAt)
                .ToListAsync();
            return View(list);
        }

        // =======================
        // Details (public)
        // =======================
        [AllowAnonymous]
        public async Task<IActionResult> Details(string id)
        {
            if (string.IsNullOrWhiteSpace(id)) return NotFound();
            var item = await _context.DevUpdates
                .Include(d => d.Admin)
                .FirstOrDefaultAsync(d => d.DevUpdateId == id);
            if (item == null) return NotFound();
            return View(item);
        }

        // =======================
        // Create (Admin only)
        // =======================
        [Authorize(Roles = "admin")]
        [HttpGet]
        public IActionResult Create() => View(new DevUpdateCreateViewModel());

        [Authorize(Roles = "admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(DevUpdateCreateViewModel vm)
        {
            if (!ModelState.IsValid) return View(vm);

            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Unauthorized();

            var entity = new DevUpdate
            {
                AdminId = user.Id,
                Type = vm.Type.Trim(),
                Content = vm.Content.Trim(),
                Location = vm.Location.Trim(),
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            if (vm.Image != null)
            {
                var imgResult = await SaveImageAsync(vm.Image);
                if (!imgResult.ok)
                {
                    ModelState.AddModelError("Image", imgResult.error!);
                    return View(vm);
                }
                entity.ImageUrl = imgResult.url;
            }

            _context.DevUpdates.Add(entity);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Details), new { id = entity.DevUpdateId });
        }

        // =======================
        // Edit (Admin only)
        // =======================
        [Authorize(Roles = "admin")]
        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            if (string.IsNullOrWhiteSpace(id)) return NotFound();
            var entity = await _context.DevUpdates.FindAsync(id);
            if (entity == null) return NotFound();

            var vm = new DevUpdateEditViewModel
            {
                DevUpdateId = entity.DevUpdateId,
                Type = entity.Type,
                Content = entity.Content,
                Location = entity.Location,
                ExistingImageUrl = entity.ImageUrl
            };
            return View(vm);
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(DevUpdateEditViewModel vm)
        {
            if (!ModelState.IsValid) return View(vm);
            var entity = await _context.DevUpdates.FindAsync(vm.DevUpdateId);
            if (entity == null) return NotFound();

            entity.Type = vm.Type.Trim();
            entity.Content = vm.Content.Trim();
            entity.Location = vm.Location.Trim();
            entity.UpdatedAt = DateTime.UtcNow;

            if (vm.RemoveImage && !string.IsNullOrEmpty(entity.ImageUrl))
            {
                DeletePhysicalFileIfExists(entity.ImageUrl);
                entity.ImageUrl = null;
            }

            if (vm.Image != null)
            {
                var imgResult = await SaveImageAsync(vm.Image);
                if (!imgResult.ok)
                {
                    ModelState.AddModelError("Image", imgResult.error!);
                    return View(vm);
                }
                // delete old
                if (!string.IsNullOrEmpty(entity.ImageUrl))
                    DeletePhysicalFileIfExists(entity.ImageUrl);
                entity.ImageUrl = imgResult.url;
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Details), new { id = entity.DevUpdateId });
        }

        // =======================
        // Delete (Admin only)
        // =======================
        [Authorize(Roles = "admin")]
        [HttpGet]
        public async Task<IActionResult> Delete(string id)
        {
            if (string.IsNullOrWhiteSpace(id)) return NotFound();
            var item = await _context.DevUpdates.FirstOrDefaultAsync(d => d.DevUpdateId == id);
            if (item == null) return NotFound();
            return View(item);
        }

        [Authorize(Roles = "admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var entity = await _context.DevUpdates.FindAsync(id);
            if (entity != null)
            {
                if (!string.IsNullOrEmpty(entity.ImageUrl))
                    DeletePhysicalFileIfExists(entity.ImageUrl);
                _context.DevUpdates.Remove(entity);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        // =======================
        // Helpers
        // =======================
        private async Task<(bool ok, string? url, string? error)> SaveImageAsync(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return (false, null, "No file provided");

            if (file.Length > 2 * 1024 * 1024)
                return (false, null, "Image must be ≤ 2 MB");

            var ext = Path.GetExtension(file.FileName).ToLowerInvariant();
            var allowed = new[] { ".jpg", ".jpeg", ".png", ".webp" };
            if (!allowed.Contains(ext))
                return (false, null, "Allowed types: jpg, jpeg, png, webp");

            var folder = Path.Combine(_env.WebRootPath, "uploads", "devupdates");
            Directory.CreateDirectory(folder);
            var fileName = $"{Guid.NewGuid():N}{ext}";
            var absPath = Path.Combine(folder, fileName);
            using (var stream = System.IO.File.Create(absPath))
            {
                await file.CopyToAsync(stream);
            }
            var url = $"/uploads/devupdates/{fileName}";
            return (true, url, null);
        }

        private void DeletePhysicalFileIfExists(string relative)
        {
            try
            {
                var safeRel = relative.Replace('/', Path.DirectorySeparatorChar).TrimStart('~');
                var full = Path.Combine(_env.WebRootPath, safeRel);
                if (System.IO.File.Exists(full)) System.IO.File.Delete(full);
            }
            catch { /* ignore */ }
        }
    }
}