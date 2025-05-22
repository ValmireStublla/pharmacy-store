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
using Pharmacy_app.Areas.Admin.ViewModels;
using Pharmacy_app.Data;

namespace Pharmacy_app.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class MedicamentsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MedicamentsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Admin/Medicaments
        public async Task<IActionResult> Index(string sortOrder,
                                                 string currentFilter,
                                                 string searchString,
                                                 int? pageNumber)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["DateSortParm"] = sortOrder == "Date" ? "date_desc" : "Date";

            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            var medicaments = from m in _context.Medicament
                              select m;
            if (!String.IsNullOrEmpty(searchString))
            {
                medicaments = medicaments.Where(m => m.Title.Contains(searchString));
            }
            switch (sortOrder)
            {
                case "name_desc":
                    medicaments = medicaments.OrderByDescending(s => s.Price);
                    break;
                case "Date":
                    medicaments = medicaments.OrderBy(s => s.DataSkadences);
                    break;
                default:
                    medicaments = medicaments.OrderBy(s => s.Title);
                    break;
            }
            int pageSize = 3;
            return View(await PaginatedList<Medicament>.CreateAsync(medicaments.AsNoTracking(), pageNumber ?? 1, pageSize));
        }

        // GET: Admin/Medicaments/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null || _context.Medicament == null)
            {
                return NotFound();
            }
            var medicament = await _context.Medicament
                .FirstOrDefaultAsync(m => m.ID == id);
            if (medicament == null)
            {
                return NotFound();
            }

            return View(medicament);
        }

        // GET: Admin/Medicaments/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/Medicaments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MedicamentViewModel medicament)
        {
            if (ModelState.IsValid)
            {
                byte[] imagebytes = null;

                if (medicament.Photo.Length > 0)
                {
                    using (var stream = new MemoryStream())
                    {
                        await medicament.Photo.CopyToAsync(stream);
                        imagebytes = stream.ToArray();
                    }
                }

                Medicament createProduct = new Medicament
                {

                    Title = medicament.Title,
                    DataSkadences = DateTime.Now,
                    Price = medicament.Price,
                    Description = medicament.Description,
                    Photo = imagebytes,

                };


                _context.Add(createProduct);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(medicament);
        }


        // GET: Admin/Medicaments/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var medicament = await _context.Medicament.FindAsync(id);
            if (medicament == null)
            {
                return NotFound();
            }

            MedicamentViewModel model = new MedicamentViewModel()
            {
                ID = medicament.ID,
                Title = medicament.Title,
                Description = medicament.Description,
                Price = medicament.Price,
                DataSkadences = medicament.DataSkadences
            };
            return View(model);
        }

        // POST: Admin/Medicaments/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, MedicamentViewModel medicament)
        {
            if (id != medicament.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var dbMedicament = await _context.Medicament.FindAsync(id);

                    if (dbMedicament == null)
                    {
                        return NotFound();
                    }

                    byte[] imagebytes = null;

                    if (medicament.Photo != null && medicament.Photo.Length > 0)
                    {
                        using (var stream = new MemoryStream())
                        {
                            await medicament.Photo.CopyToAsync(stream);
                            imagebytes = stream.ToArray();
                        }
                    }

                    dbMedicament.Photo = imagebytes;
                    dbMedicament.Title = medicament.Title;
                    dbMedicament.Price = medicament.Price;
                    dbMedicament.Description = medicament.Description;

                    _context.Update(dbMedicament);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MedicamentExists(medicament.ID))
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
            return View(medicament);
        }


        // GET: Admin/Medicaments/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null || _context.Medicament == null)
            {
                return NotFound();
            }

            var medicament = await _context.Medicament
                .FirstOrDefaultAsync(m => m.ID == id);
            if (medicament == null)
            {
                return NotFound();
            }

            return View(medicament);
        }

        // POST: Admin/Medicaments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (_context.Medicament == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Medicament'  is null.");
            }
            var medicament = await _context.Medicament.FindAsync(id);
            if (medicament != null)
            {
                _context.Medicament.Remove(medicament);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MedicamentExists(Guid id)
        {
          return (_context.Medicament?.Any(e => e.ID == id)).GetValueOrDefault();
        }
    }
}
