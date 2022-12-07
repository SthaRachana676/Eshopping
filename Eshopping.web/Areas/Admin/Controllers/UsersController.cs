using Eshopping.web.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eshopping.web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class UsersController : Controller
    {
        private readonly UserManager<AppUser> userManager;

        public UsersController(UserManager<AppUser> userManager)
        {
            this.userManager = userManager;
            

        }
        public IActionResult Index()
        {
            return View(userManager.Users);
        }
    }
}
