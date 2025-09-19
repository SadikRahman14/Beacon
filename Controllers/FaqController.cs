using Beacon.Data;
using Beacon.Models;
using Beacon.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Beacon.Controllers
{
    public class FaqsController : Controller
    {
        private readonly AppDbContext _context;

        public FaqsController(AppDbContext context)
        {
            _context = context;
        }

        // =============== Public: List FAQs ===============
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            var faqs = await _context.Faqs
                .OrderByDescending(f => f.CreatedAt)
                .ToListAsync();
            return View(faqs);
        }

        // =============== Admin: Create ===============
        [Authorize(Roles = "admin")]
        [HttpGet]
        public IActionResult Create() => View(new FaqCreateViewModel());

        [Authorize(Roles = "admin")]
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(FaqCreateViewModel vm)
        {
            if (!ModelState.IsValid) return View(vm);

            var entity = new Faq
            {
                Question = vm.Question,
                Answer = vm.Answer,
                CreatedAt = DateTime.UtcNow
            };

            _context.Faqs.Add(entity);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // =============== Admin: Edit ===============
        [Authorize(Roles = "admin")]
        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null) return NotFound();
            var faq = await _context.Faqs.FindAsync(id);
            if (faq == null) return NotFound();

            var vm = new FaqEditViewModel
            {
                FaqId = faq.FaqId,
                Question = faq.Question,
                Answer = faq.Answer
            };
            return View(vm);
        }

        [Authorize(Roles = "admin")]
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, FaqEditViewModel vm)
        {
            if (id != vm.FaqId) return BadRequest();
            if (!ModelState.IsValid) return View(vm);

            var faq = await _context.Faqs.FindAsync(id);
            if (faq == null) return NotFound();

            faq.Question = vm.Question;
            faq.Answer = vm.Answer;

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // =============== Admin: Delete ===============
        [Authorize(Roles = "admin")]
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(string id)
        {
            var faq = await _context.Faqs.FindAsync(id);
            if (faq != null)
            {
                _context.Faqs.Remove(faq);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
