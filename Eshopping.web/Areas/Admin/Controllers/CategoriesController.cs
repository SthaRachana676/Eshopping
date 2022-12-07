using Eshopping.web.Controllers.Infrastructure;
using Eshopping.web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eshopping.web.Areas.Admin.Controllers
{
    [Area("Admin")]

    public class CategoriesController : Controller
    {
        private readonly EshoppingCartContext context;

        public CategoriesController(EshoppingCartContext context)
        {
            this.context = context;
        }
        //get/admin/categories/
        public async Task<IActionResult> Index()
        {

            return View(await context.Categories.OrderBy(x => x.Sorting).ToListAsync());
        }
        
        //get/admin/categories/create

        public IActionResult Create() => View();
        //post/admin/pages/create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Category category)
        {
            if (ModelState.IsValid)
            {
                category.Slug = category.Name.ToLower().Replace(" ", "|");
                category.Sorting = 100;
                var slug = await context.Categories.FirstOrDefaultAsync(x => x.Slug == category.Slug);
                if (slug != null)
                {
                    ModelState.AddModelError("", "The Category already exists");
                    return View(category);
                }

                context.Add(category);
                await context.SaveChangesAsync();

                TempData["Success"] = "The Category has been added!";



                return RedirectToAction("Index");
            }

            return View(category);
        }

        //get/admin/category/Update
        public async Task<IActionResult> Update(int id)
        {
            Category category = await context.Categories.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        //get/admin/categories/edit

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int id ,Category category)
        {
            if (ModelState.IsValid)
            {
                category.Slug =  category.Name.ToLower().Replace(" ", "|");

                var slug = await context.Categories.Where(x => x.Id !=id).FirstOrDefaultAsync(x => x.Slug == category.Slug);
                if (slug != null)
                {
                    ModelState.AddModelError("", "The Category already exists");
                    return View(category);
                }

                context.Update(category);
                await context.SaveChangesAsync();

                TempData["Success"] = "The Category has been Update!";



                return RedirectToAction("Update", new { id});
            }

            return View(category);
        }
        //get/admin/categories/delete
        public async Task<IActionResult> Delete(int id)
        {
            Category category = await context.Categories.FindAsync(id);
            if (category == null)
            {
                TempData["Error"] = "The Category does not exist!";
            }
            else
            {
                context.Categories.Remove(category);
                await context.SaveChangesAsync();

                TempData["Success"] = "The Category has been deleted! ";
            }
            return RedirectToAction("Index");
        }
        //get/admin/category/reorder

        [HttpPost]
        public async Task<IActionResult> Reorder(int[] id)
        {
            int count = 1;

            foreach (var categoryId in id)
            {
                Category category = await context.Categories.FindAsync(categoryId);
                category.Sorting = count;
                context.Update(category);
                await context.SaveChangesAsync();
                count++;
            }
            return Ok();
        }
    }
}
