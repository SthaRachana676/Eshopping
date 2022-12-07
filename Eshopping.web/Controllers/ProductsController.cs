using Eshopping.web.Controllers.Infrastructure;
using Eshopping.web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eshopping.web.Controllers
{
    [Authorize]
    public class ProductsController : Controller
    {
        private readonly EshoppingCartContext context;

        public ProductsController(EshoppingCartContext context)
        {
            this.context = context;
        }
        //get/products/
        public async Task<IActionResult> Index(int p = 1)
        {
            int pageSize = 6;
            var products = context.products.OrderByDescending(x => x.Id)
                .Skip((p - 1) * pageSize)
                .Take(pageSize);


            ViewBag.PageNumber = p;
            ViewBag.PageRange = pageSize;
            ViewBag.TotalPages = (int)Math.Ceiling((decimal)context.products.Count() / pageSize);
            return View(await products.ToListAsync());
        }
        public async Task<IActionResult> ProductsByCategory(string categorySlug,int p = 1)
        {
            Category category =await context.Categories.Where(x => x.Slug == categorySlug).FirstOrDefaultAsync();
            if (category == null) return  RedirectToAction("Index");
            
            int pageSize = 6;
            var products = context.products.OrderByDescending(x => x.Id)
                .Where(x => x.CategoryId == category.Id)
                .Skip((p - 1) * pageSize)
                .Take(pageSize);


            ViewBag.PageNumber = p;
            ViewBag.PageRange = pageSize;
            ViewBag.TotalPages = (int)Math.Ceiling((decimal)context.products.Where(x => x.CategoryId == category.Id).Count() / pageSize);
            ViewBag.CategoryName = category.Name;
            ViewBag.CategorySlug = categorySlug;
            return View(await products.ToListAsync());
        }
    }
}
