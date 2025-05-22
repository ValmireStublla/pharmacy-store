using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
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
    public class PharmacistsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PharmacistsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Admin/Pharmacists
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

            var pharmacists = from p in _context.Pharmacists
                              select p;

            if (!String.IsNullOrEmpty(searchString))
            {
                pharmacists = pharmacists.Where(p => p.Name.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "name_desc":
                    pharmacists = pharmacists.OrderByDescending(p => p.Name);
                    break;
                case "Date":
                    pharmacists = pharmacists.OrderBy(p => p.YearsOfExperience); // Assuming YearsOfExperience is relevant for sorting
                    break;
                default:
                    pharmacists = pharmacists.OrderBy(p => p.Degree);
                    break;
            }

            int pageSize = 3;
            int pageNumberValue = pageNumber ?? 1; // Use pageNumber ?? 1 to ensure pageNumberValue is not nullable
            return View(await PaginatedList<Pharmacists>.CreateAsync(pharmacists.AsNoTracking(), pageNumberValue, pageSize));
        }

        // GET: Admin/Pharmacists/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null || _context.Pharmacists == null)
            {
                return NotFound();
            }

            var pharmacists = await _context.Pharmacists
                .FirstOrDefaultAsync(m => m.Id == id);
            if (pharmacists == null)
            {
                return NotFound();
            }

            return View(pharmacists);
        }

        // GET: Admin/Pharmacists/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/Pharmacists/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PharmacistsViewModel pharmacist)
        {
            if (ModelState.IsValid)
            {
                byte[] imagebytes = null;

                if (pharmacist.ProfilePicture.Length > 0)
                {
                    using (var stream = new MemoryStream())
                    {
                        await pharmacist.ProfilePicture.CopyToAsync(stream);
                        imagebytes = stream.ToArray();
                    }
                }

                Pharmacists createPharmacist = new Pharmacists
                {

                    Name = pharmacist.Name,
                    Degree = pharmacist.Degree,
                    ProfilePicture = imagebytes
                };


                _context.Add(createPharmacist);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(pharmacist);
        }

        // GET: Admin/Pharmacists/Edit/5
        public async Task<IActionResult> Edit(Guid id, PharmacistsViewModel pharmacists)
        {
            if (id != pharmacists.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var dbPharmacist = await _context.Pharmacists.FindAsync(id);

                    if (dbPharmacist == null)
                    {
                        return NotFound();
                    }

                    // Update properties of dbPharmacist
                    dbPharmacist.Name = pharmacists.Name;
                    dbPharmacist.Degree = pharmacists.Degree;

                    // Handle ProfilePicture if needed
                    if (pharmacists.ProfilePicture != null && pharmacists.ProfilePicture.Length > 0)
                    {
                        using (var stream = new MemoryStream())
                        {
                            await pharmacists.ProfilePicture.CopyToAsync(stream);
                            dbPharmacist.ProfilePicture = stream.ToArray();
                        }
                    }

                    _context.Update(dbPharmacist);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PharmacistsExists(pharmacists.Id))
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
            return View(pharmacists);
        }
        // GET: Admin/Pharmacists/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null || _context.Pharmacists == null)
            {
                return NotFound();
            }

            var pharmacists = await _context.Pharmacists
                .FirstOrDefaultAsync(m => m.Id == id);
            if (pharmacists == null)
            {
                return NotFound();
            }

            return View(pharmacists);
        }

        // POST: Admin/Pharmacists/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (_context.Pharmacists == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Pharmacists'  is null.");
            }
            var pharmacists = await _context.Pharmacists.FindAsync(id);
            if (pharmacists != null)
            {
                _context.Pharmacists.Remove(pharmacists);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PharmacistsExists(Guid id)
        {
          return (_context.Pharmacists?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
