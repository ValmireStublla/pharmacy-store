using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Pharmacy_app.Areas.Admin.Models;
using Pharmacy_app.Data;

namespace Pharmacy_app.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class FaqsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public FaqsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Admin/Faqs
        public async Task<IActionResult> Index(string sortOrder,
                                              string currentFilter,
                                              string searchString,
                                              int? pageNumber)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";

            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            var medicaments = from m in _context.Faq
                              select m;
            if (!String.IsNullOrEmpty(searchString))
            {
                medicaments = medicaments.Where(m => m.Question.Contains(searchString));
            }
            switch (sortOrder)
            {
                case "name_desc":
                    medicaments = medicaments.OrderByDescending(s => s.Question);
                    break;
                default:
                    medicaments = medicaments.OrderBy(s => s.Question);
                    break;
            }
            int pageSize = 3;
            return View(await PaginatedList<Faq>.CreateAsync(medicaments.AsNoTracking(), pageNumber ?? 1, pageSize));
        }



        // GET: Admin/Faqs/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null || _context.Faq == null)
            {
                return NotFound();
            }

            var faq = await _context.Faq
                .FirstOrDefaultAsync(m => m.ID == id);
            if (faq == null)
            {
                return NotFound();
            }

            return View(faq);
        }

        // GET: Admin/Faqs/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/Faqs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Question,Answer")] Faq faq)
        {
            if (ModelState.IsValid)
            {
                faq.ID = Guid.NewGuid();
                _context.Add(faq);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(faq);
        }

        // GET: Admin/Faqs/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null || _context.Faq == null)
            {
                return NotFound();
            }

            var faq = await _context.Faq.FindAsync(id);
            if (faq == null)
            {
                return NotFound();
            }
            return View(faq);
        }

        // POST: Admin/Faqs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("ID,Question,Answer")] Faq faq)
        {
            if (id != faq.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(faq);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FaqExists(faq.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(faq);
        }

        // GET: Admin/Faqs/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null || _context.Faq == null)
            {
                return NotFound();
            }

            var faq = await _context.Faq
                .FirstOrDefaultAsync(m => m.ID == id);
            if (faq == null)
            {
                return NotFound();
            }

            return View(faq);
        }

        // POST: Admin/Faqs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (_context.Faq == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Faq'  is null.");
            }
            var faq = await _context.Faq.FindAsync(id);
            if (faq != null)
            {
                _context.Faq.Remove(faq);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FaqExists(Guid id)
        {
          return (_context.Faq?.Any(e => e.ID == id)).GetValueOrDefault();
        }
    }
}
