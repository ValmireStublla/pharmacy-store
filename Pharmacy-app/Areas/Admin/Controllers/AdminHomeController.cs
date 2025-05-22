using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient.Server;

namespace Pharmacy_app.Areas.Admin.Controllers
{
    //Tregon qe ky controller gjendet ne Area admin
    [Area("Admin")]
    //Tregon qe useri duhet te jete i kyqyr dhe te kete rolin admin ne menyre qe te kete access
    [Authorize(Roles = "Admin")]
    public class AdminHomeController : Controller
    {
        private readonly RoleManager<IdentityRole> rolemanager;
        public AdminHomeController(RoleManager<IdentityRole> _roleManager)
        {
            rolemanager = _roleManager;
        }
        public IActionResult Index()
        {
            return View();
        }

       


        //emer nuk egziston e krijon ne tabele
        [HttpPost]
        public async Task<IActionResult> CreateRole(string roleName)
        {

            if (!await rolemanager.RoleExistsAsync(roleName))
            {

                await rolemanager.CreateAsync(new IdentityRole(roleName));
            }

            return View();
        }
    }
}
