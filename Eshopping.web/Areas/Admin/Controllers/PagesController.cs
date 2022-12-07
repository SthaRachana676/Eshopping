using Eshopping.web.Controllers.Infrastructure;
using Eshopping.web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eshopping.web.Controllers
{
    [Area("Admin")]
    public class PagesController : Controller
    {
        private readonly EshoppingCartContext context;

        public PagesController(EshoppingCartContext context)
        {
            this.context = context;
        }
        //get/admin/pages
        public async Task<IActionResult> Index()
        {
            IQueryable<Page> pages = from p in context.pages orderby p.Sorting select p;
            List<Page> pagesList = await pages.ToListAsync();

            
            return View(pagesList);
        }


        //get/admin/pages/details
        public async Task<IActionResult> Details(int id)
        {
            Page page = await context.pages.FirstOrDefaultAsync(x => x.Id == id);
            if(page == null)
            {
                return NotFound();
            }
            return View(page);
        }

        //get/admin/pages/create
        [HttpGet]
        public IActionResult Create() => View();
        
        
        //get/admin/pages/create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create( Page page)
        {
            if (ModelState.IsValid)
            {
                page.Slug = page.Title.ToLower().Replace(" ", "|");
                page.Sorting = 100;
                var slug = await context.pages.FirstOrDefaultAsync(x => x.Slug == page.Slug);
                if (slug != null)
                {
                    ModelState.AddModelError("", "The page already exists");
                    return View(page);
                }

                context.Add(page);
                await context.SaveChangesAsync();

                TempData["Success"] = "The Page has been added!";



                return RedirectToAction("Index");
            }

            return View(page);
        }


        //get/admin/pages/Update
        public async Task<IActionResult> Update(int id)
        {
            Page page = await context.pages.FindAsync(id);
            if (page == null)
            {
                return NotFound();
            }
            return View(page);
        }

        //get/admin/pages/edit

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(Page page)
        {
            if (ModelState.IsValid)
            {
                page.Slug = page.Id == 1 ? "home": page.Title.ToLower().Replace(" ", "|");
             
                var slug = await context.pages.Where(x => x.Id !=page.Id).FirstOrDefaultAsync(x => x.Slug == page.Slug);
                if (slug != null)
                {
                    ModelState.AddModelError("", "The page already exists");
                    return View(page);
                }

                context.Update(page);
                await context.SaveChangesAsync();

                TempData["Success"] = "The Page has been Update!";



                return RedirectToAction("Update" , new { id = page.Id});
            }

            return View(page);
        }
        //get/admin/pages/delete
        public async Task<IActionResult> Delete(int id)
        {
            Page page = await context.pages.FindAsync(id);
            if (page == null)
            {
                TempData["Error"] = "The Page does not exist!";
            }else
            {
                context.pages.Remove(page);
                await context.SaveChangesAsync();

                TempData["Success"] = "The page has been deleted! ";
            }
            return RedirectToAction("Index");
        }


        //get/admin/pages/reorder

        [HttpPost]
        public async Task<IActionResult> Reorder(int[] id)
        {
            int count = 1;

           foreach(var pageId in id)
            {
                Page page = await context.pages.FindAsync(pageId);
                page.Sorting = count;
                context.Update(page);
                await context.SaveChangesAsync();
                count++;
            }
            return Ok();
         }
        }
    }

