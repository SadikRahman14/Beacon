using Beacon.Data;
using Beacon.Models;
using Beacon.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Beacon.Controllers
{
    [Authorize]
    public class AlertPostsController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<User> _userManager;

        public AlertPostsController(AppDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
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
        // Details (public)
        // =======================
        [AllowAnonymous]
        public async Task<IActionResult> Details(string id)
        {
            if (id == null) return NotFound();

            var alert = await _context.AlertPosts
                .Include(a => a.Admin)
                .FirstOrDefaultAsync(a => a.AlertId == id);

            if (alert == null) return NotFound();

            return View(alert);
        }

        // Create (Admin only)
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

            // Optional: validate file
            string? savedUrl = null;
            if (vm.AlertImage is { Length: > 0 })
            {
                // size limit (2MB example)
                const long maxBytes = 2 * 1024 * 1024;
                if (vm.AlertImage.Length > maxBytes)
                {
                    ModelState.AddModelError(nameof(vm.AlertImage), "Image must be ≤ 2MB.");
                    return View(vm);
                }

                // allowlist extensions & content types
                var ext = Path.GetExtension(vm.AlertImage.FileName).ToLowerInvariant();
                var okExt = new[] { ".jpg", ".jpeg", ".png", ".webp" };
                var okTypes = new[] { "image/jpeg", "image/png", "image/webp" };
                if (!okExt.Contains(ext) || !okTypes.Contains(vm.AlertImage.ContentType))
                {
                    ModelState.AddModelError(nameof(vm.AlertImage), "Only JPG, PNG, or WebP images are allowed.");
                    return View(vm);
                }

                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "alerts");
                Directory.CreateDirectory(uploadsFolder);

                var fileName = $"{Guid.NewGuid()}{ext}";
                var filePath = Path.Combine(uploadsFolder, fileName);
                try
                {
                    using var stream = new FileStream(filePath, FileMode.Create);
                    await vm.AlertImage.CopyToAsync(stream);
                    savedUrl = $"/uploads/alerts/{fileName}";
                }
                catch
                {
                    ModelState.AddModelError(nameof(vm.AlertImage), "Failed to save the image. Try again.");
                    return View(vm);
                }
            }

            var entity = new AlertPost
            {
                AdminId = user.Id,
                Type = vm.Type.ToString(), // if enum
                                           // OR: Type   = vm.Type,           // if string
                Content = vm.Content,
                Location = vm.Location,
                AlertImageUrl = savedUrl,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _context.AlertPosts.Add(entity);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }


        // =======================
        // Edit (Admin only)
        // =======================
        [Authorize(Roles = "admin")]
        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null) return NotFound();

            var alert = await _context.AlertPosts.FindAsync(id);
            if (alert == null) return NotFound();

            return View(alert);
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, AlertPost model, IFormFile? AlertImage)
        {
            if (id != model.AlertId) return NotFound();
            if (!ModelState.IsValid) return View(model);

            var alert = await _context.AlertPosts.FindAsync(id);
            if (alert == null) return NotFound();

            alert.Type = model.Type;
            alert.Content = model.Content;
            alert.Location = model.Location;
            alert.UpdatedAt = DateTime.UtcNow;

            if (AlertImage != null && AlertImage.Length > 0)
            {
                var fileName = $"{Guid.NewGuid()}{Path.GetExtension(AlertImage.FileName)}";
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "alerts");
                Directory.CreateDirectory(uploadsFolder);

                var filePath = Path.Combine(uploadsFolder, fileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await AlertImage.CopyToAsync(stream);
                }

                alert.AlertImageUrl = $"/uploads/alerts/{fileName}";
            }

            _context.Update(alert);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
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
                _context.AlertPosts.Remove(alert);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
