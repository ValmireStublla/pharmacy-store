using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pharmacy_app.Areas.Admin.Models;
using Pharmacy_app.Areas.Admin.ViewModels;
using Pharmacy_app.Data;
using Pharmacy_app.Models;
using System.Diagnostics;

namespace Pharmacy_app.Controllers
{
    
    public class HomeController : Controller
    {

        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context; 

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;  

        }


        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
        public IActionResult About()
        {
            return View();
        }
        public async Task<IActionResult> Pharmacists()
        {
            var pharmacists = await _context.Pharmacists.ToListAsync();

            if (pharmacists != null)
            {
                return View(pharmacists);
            }
            else
            {
                return Problem("Entity set 'ApplicationDbContext.Pharmacists' is null.");
            }
        }

        public async Task<IActionResult> DetailsPharmacist(Guid? id)
        {
            if (id == null || _context.Pharmacists == null)
            {
                return NotFound();
            }
            var pharmacist = await _context.Pharmacists
                .FirstOrDefaultAsync(p => p.Id == id);
            if (pharmacist == null)
            {
                return NotFound();
            }

            return View(pharmacist);
        }

        public async Task<IActionResult> DetailsMedicament(Guid? id)
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

        public async Task<IActionResult> Faqs()
        {
            var faqs = await _context.Faq.ToListAsync();

            if (faqs != null)
            {
                return View(faqs);
            }
            else
            {
                return Problem("Entity set 'ApplicationDbContext.Faqs' is null.");
            }
        }

        public async Task<IActionResult> Products()
        {
            return _context.Medicament != null ?
                        View(await _context.Medicament.ToListAsync()) :
                        Problem("Entity set 'ApplicationDbContext.Medicament'  is null.");
        }




        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}