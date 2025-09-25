using Beacon.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Beacon.ViewComponents
{
    public class AlertCommentsViewComponent : ViewComponent
    {
        private readonly AppDbContext _context;
        public AlertCommentsViewComponent(AppDbContext context) { _context = context; }

        public async Task<IViewComponentResult> InvokeAsync(string alertId)
        {
            var comments = await _context.AlertComments
                .Where(c => c.AlertId == alertId)
                .Include(c => c.Author)
                .OrderByDescending(c => c.CreatedAt)
                .ToListAsync();

            ViewBag.AlertId = alertId;
            return View(comments); // Views/Shared/Components/AlertComments/Default.cshtml
        }
    }
}
