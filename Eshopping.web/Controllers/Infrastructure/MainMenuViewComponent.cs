using Eshopping.web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eshopping.web.Controllers.Infrastructure
{

    public class MainMenuViewComponent : ViewComponent
    {
        private readonly EshoppingCartContext context;

        public MainMenuViewComponent(EshoppingCartContext context)
        {
            this.context = context;
        }
         
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var pages = await GetPagesAsync();
            return View(pages);
        }

        private Task<List<Page>> GetPagesAsync()
        {
            return context.pages.OrderBy(x => x.Sorting).ToListAsync();
        }
    }
}
