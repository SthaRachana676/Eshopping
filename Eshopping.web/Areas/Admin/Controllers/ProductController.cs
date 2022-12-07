using Eshopping.web.Controllers.Infrastructure;
using Eshopping.web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Hosting;
using System.Threading.Tasks;
using System.IO;

namespace Eshopping.web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly EshoppingCartContext context;
        private readonly IWebHostEnvironment webHostEnvironment;
        public ProductController(EshoppingCartContext context, IWebHostEnvironment webHostEnvironment)
        {
            this.context = context;
            this.webHostEnvironment = webHostEnvironment;
        }
     
        //get/admin/products/
        public async Task<IActionResult> Index(int p = 1)
        {
            int pageSize = 6;
            var products = context.products.OrderByDescending(x => x.Id)
                .Include(x => x.Category)
                .Skip((p - 1) * pageSize)
                .Take(pageSize);


            ViewBag.PageNumber = p;
            ViewBag.PageRange = pageSize;
            ViewBag.TotalPages = (int)Math.Ceiling((decimal)context.products.Count() / pageSize);
            return View(await products.ToListAsync());
        }

        //get/admin/products/details
        public async Task<IActionResult> Details(int id)
        {
            Product product = await context.products.Include(x => x.Category).FirstOrDefaultAsync(x => x.Id == id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        //Get/admin/products/create
        public IActionResult Create()
        {

            ViewBag.CategoryId = new SelectList(context.Categories.OrderBy(x => x.Sorting), "Id","Name");
            
            return View();
        }

        //get/admin/product/create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Product product)
        {
            ViewBag.CategoryId = new SelectList(context.Categories.OrderBy(x => x.Sorting), "Id", "Name");

            if (ModelState.IsValid)
            {
                product.Slug = product.Name.ToLower().Replace(" ", "|");
               
                var slug = await context.products.FirstOrDefaultAsync(x => x.Slug == product.Slug);
                if (slug != null)
                {
                    ModelState.AddModelError("", "The Product already exists");
                    return View(product);
                }

                string imageName = "noimage.png";
                if(product.ImageUpload != null)
                {
                    string uploadDir = Path.Combine(webHostEnvironment.WebRootPath,"media/products");
                    imageName = Guid.NewGuid().ToString() + "_" + product.ImageUpload.FileName;
                    string filePath = Path.Combine(uploadDir, imageName);
                    FileStream fs = new FileStream(filePath, FileMode.Create);
                    await product.ImageUpload.CopyToAsync(fs);
                    fs.Close();
                }

                product.Image = imageName;

                context.Add(product);
                await context.SaveChangesAsync();

                TempData["Success"] = "The Product has been added!";



                return RedirectToAction("Index");
            }

            return View(product);
        }

        public async Task<IActionResult> Update(int id)
        {
            Product product = await context.products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            ViewBag.CategoryId = new SelectList(context.Categories.OrderBy(x => x.Sorting), "Id", "Name",product.CategoryId);

            return View(product);
        }
        //get/admin/product/update

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update( int id,Product product)
        {
            ViewBag.CategoryId = new SelectList(context.Categories.OrderBy(x => x.Sorting), "Id", "Name",product.CategoryId);

            if (ModelState.IsValid)
            {
                product.Slug = product.Name.ToLower().Replace(" ", "|");

                var slug = await context.products.Where(x => x.Id !=id).FirstOrDefaultAsync(x => x.Slug == product.Slug);
                if (slug != null)
                {
                    ModelState.AddModelError("", "The Product already exists");
                    return View(product);
                }

 
                if (product.ImageUpload != null)
                {
                    string uploadDir = Path.Combine(webHostEnvironment.WebRootPath, "media/products");

                    if (!string.Equals(product.Image, "noimage.png"))
                    {
                        string oldImagePath = Path.Combine(uploadDir,product.Image);
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);

                        }
                    }
                   string imageName = Guid.NewGuid().ToString() + "_" + product.ImageUpload.FileName;
                    string filePath = Path.Combine(uploadDir, imageName);
                    FileStream fs = new FileStream(filePath, FileMode.Create);
                    await product.ImageUpload.CopyToAsync(fs);
                    fs.Close();
                    product.Image = imageName;
                }

                

                context.Update(product);
                await context.SaveChangesAsync();

                TempData["Success"] = "The Product has been Update!";



                return RedirectToAction("Index");
            }

            return View(product);
        }

        //get/admin/product/delete
        public async Task<IActionResult> Delete(int id)
        {
            Product product = await context.products.FindAsync(id);
            if (product == null)
            {
                TempData["Error"] = "The product does not exist!";
            }
            else
            {
                if (!string.Equals(product.Image, "noimage.png"))
                {
                    string uploadDir = Path.Combine(webHostEnvironment.WebRootPath, "media/products");
                    string oldImagePath = Path.Combine(uploadDir, product.Image);
                    if (System.IO.File.Exists(oldImagePath))
                    {
                        System.IO.File.Delete(oldImagePath);

                    }
                }
                context.products.Remove(product);
                await context.SaveChangesAsync();

                TempData["Success"] = "The product has been deleted! ";
            }
            return RedirectToAction("Index");
        }
    }
}
