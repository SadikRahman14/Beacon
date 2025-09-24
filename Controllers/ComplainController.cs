using Beacon.Data;
using Beacon.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;  
using System.Diagnostics;
using System.Security.Claims;

namespace Beacon.Controllers
{
    [Authorize]
    public class ComplainController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public ComplainController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        [HttpGet]
        public IActionResult CreateComplain()
        {
            return View(new Complain());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateComplain(Complain model)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            Debug.WriteLine($"DEBUG: Logged-in UserId = {userId}");

            if (string.IsNullOrEmpty(userId))
            {
                TempData["Error"] = "You must be logged in to submit a complaint.";
                return RedirectToAction("Login", "Account");
            }

            model.UserId = userId;
            model.ComplaintImageUrl = string.Empty;

            ModelState.Remove(nameof(model.UserId));
            ModelState.Remove(nameof(model.User));
            ModelState.Remove(nameof(model.ComplaintImageUrl));

            LogFormValues(model);

            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Please fix the validation errors.";
                LogModelStateErrors();
                return View(model);
            }

            if (model.ImageFiles != null && model.ImageFiles.Count > 0)
            {
                var uploadFolder = Path.Combine(_env.WebRootPath, "uploads/complaints");
                if (!Directory.Exists(uploadFolder))
                    Directory.CreateDirectory(uploadFolder);

                var uploadedPaths = new List<string>();
                foreach (var file in model.ImageFiles)
                {
                    if (file.Length > 0)
                    {
                        var fileName = $"{Guid.NewGuid()}{System.IO.Path.GetExtension(file.FileName)}";
                        var filePath = System.IO.Path.Combine(uploadFolder, fileName);
                        using var stream = new System.IO.FileStream(filePath, System.IO.FileMode.Create);
                        await file.CopyToAsync(stream);

                        uploadedPaths.Add($"/uploads/complaints/{fileName}");
                    }
                }

                model.ComplaintImageUrl = string.Join(',', uploadedPaths);
            }

            model.CreatedAt = DateTime.UtcNow;
            model.UpdatedAt = DateTime.UtcNow;

            _context.Complains.Add(model);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Complaint submitted successfully.";
            return RedirectToAction(nameof(CreateComplain));
        }

        [HttpGet]
        public IActionResult AllComplains()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // Include User and exclude complaints from logged-in user
            var complains = _context.Complains
                .Include(c => c.User)
                .Where(c => c.UserId != userId)
                .OrderByDescending(c => c.CreatedAt)
                .ToList();

            return View(complains);
        }

        #region Debug Helpers
        private void LogFormValues(Complain model)
        {
            Debug.WriteLine("DEBUG: Form values received:");
            Debug.WriteLine($"Type: {model.Type}");
            Debug.WriteLine($"Content: {model.Content}");
            Debug.WriteLine($"Location: {model.Location}");
            Debug.WriteLine($"ComplaintImageUrl: {model.ComplaintImageUrl}");
            Debug.WriteLine($"ImageFiles count: {model.ImageFiles?.Count ?? 0}");
        }

        private void LogModelStateErrors()
        {
            if (!ModelState.IsValid)
            {
                Debug.WriteLine("DEBUG: ModelState errors:");
                foreach (var key in ModelState.Keys)
                {
                    var state = ModelState[key];
                    foreach (var error in state.Errors)
                    {
                        Debug.WriteLine($"{key}: {error.ErrorMessage}");
                    }
                }
            }
        }
        #endregion
    }
}
