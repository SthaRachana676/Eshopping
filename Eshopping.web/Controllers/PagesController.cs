using Eshopping.web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eshopping.web.Controllers.Infrastructure
{
    public class PagesController : Controller
    {
        private readonly EshoppingCartContext context;

        public PagesController(EshoppingCartContext context)
        {
            this.context = context;
        }

        //GET/or/slug
        public async Task<IActionResult> Page(string slug)
        {
            if(slug == null)
            {
                return View(await context.pages.Where(x => x.Slug == "home").FirstOrDefaultAsync());

            }

            Page page = await context.pages.Where(x => x.Slug == slug).FirstOrDefaultAsync();

            if(page == null)
            {
                return NotFound();
            }
            return View(page);
        }
    }
}
