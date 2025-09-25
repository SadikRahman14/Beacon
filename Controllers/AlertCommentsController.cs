using Beacon.Data;
using Beacon.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Beacon.Controllers
{
    [Authorize] // must be signed in to post/delete; listing is anonymous below
    public class AlertCommentsController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<User> _userManager;

        public AlertCommentsController(AppDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: /AlertComments/List?alertId=...
        [AllowAnonymous]
        public async Task<IActionResult> List(string alertId)
        {
            if (string.IsNullOrWhiteSpace(alertId)) return BadRequest();

            var comments = await _context.AlertComments
                .Where(c => c.AlertId == alertId)
                .Include(c => c.Author)
                .OrderByDescending(c => c.CreatedAt)
                .ToListAsync();

            ViewBag.AlertId = alertId;
            return PartialView("~/Views/Shared/Components/AlertComments/_CommentsList.cshtml", comments);
        }

        // POST: /AlertComments/Create
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(string alertId, string content)
        {
            if (string.IsNullOrWhiteSpace(alertId))
                return BadRequest("Missing alert id.");

            if (string.IsNullOrWhiteSpace(content))
                return BadRequest("Please write a comment.");

            // Ensure alert exists
            var alertExists = await _context.AlertPosts.AnyAsync(a => a.AlertId == alertId);
            if (!alertExists) return NotFound("Alert not found.");

            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Unauthorized();

            var entity = new AlertComment
            {
                AlertId = alertId,
                UserId = user.Id,
                Content = content.Trim(),
                CreatedAt = DateTime.UtcNow
            };
            _context.AlertComments.Add(entity);
            await _context.SaveChangesAsync();

            // Return the re-rendered list partial (handy for AJAX)
            return await List(alertId);
        }

        // POST: /AlertComments/Delete/{id}
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(string id)
        {
            if (string.IsNullOrWhiteSpace(id)) return BadRequest();

            var comment = await _context.AlertComments
                .Include(c => c.Author)
                .FirstOrDefaultAsync(c => c.CommentId == id);

            if (comment == null) return NotFound();

            var currentUserId = _userManager.GetUserId(User);
            var isAdmin = User.IsInRole("admin");

            if (!isAdmin && comment.UserId != currentUserId)
                return Forbid(); // only the author or an admin can delete

            var alertId = comment.AlertId; // capture before remove
            _context.AlertComments.Remove(comment);
            await _context.SaveChangesAsync();

            // Return updated list partial
            return await List(alertId);
        }
    }
}
