using Eshopping.web.Controllers.Infrastructure;
using Eshopping.web.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eshopping.web.Controllers
{
    public class CartController : Controller
    {
        private readonly EshoppingCartContext context;

        public CartController(EshoppingCartContext context)
        {
            this.context = context;
        }

        //GET/cart
        public IActionResult Index()
        {
            List<CartItem> cart = HttpContext.Session.GetJson<List<CartItem>>("Cart") ?? new List<CartItem>();

            CartViewModel cartVM = new CartViewModel
            {
                CartItems = cart,
                GrandTotal = cart.Sum(x => x.Price * x.Quantity)
            };
            
            return View(cartVM);
        }

        //GET/cart/add/
        public async Task<IActionResult> Add(int id)
        {
            Product product =await context.products.FindAsync(id);
        
            List<CartItem> cart = HttpContext.Session.GetJson<List<CartItem>>("Cart") ?? new List<CartItem>();

            CartItem cartItem = cart.Where(x => x.ProductId == id).FirstOrDefault();

            if(cartItem  == null)
            {
                cart.Add(new CartItem(product));
            }
            else
            {
                cartItem.Quantity += 1;
            }
            HttpContext.Session.SetJson("Cart", cart);

            if(HttpContext.Request.Headers["X-Requested-With"] !="XMLHttpRequest")
                return RedirectToAction("Index");

            return ViewComponent("SmallCart");
        }
        //GET/cart/add/
        public IActionResult Decrease(int id)
        {

            List<CartItem> cart = HttpContext.Session.GetJson<List<CartItem>>("Cart");

            CartItem cartItem = cart.Where(x => x.ProductId == id).FirstOrDefault();

            if (cartItem.Quantity > 1)
            {
                --cartItem.Quantity;
            }
            else
            {
                cart.RemoveAll(x => x.ProductId == id);
            }
            HttpContext.Session.SetJson("Cart", cart);
             if(cart.Count == 0)
            {
                HttpContext.Session.Remove("Cart");
            }
            else
            {
                HttpContext.Session.SetJson("Cart",cart);

            }
            return RedirectToAction("Index");
        }
        //GET/cart/add/
        public IActionResult Remove(int id)
        {
            List<CartItem> cart = HttpContext.Session.GetJson<List<CartItem>>("Cart");

            cart.RemoveAll(x => x.ProductId == id);
            
            HttpContext.Session.SetJson("Cart", cart);
            if (cart.Count == 0)
            {
                HttpContext.Session.Remove("Cart");
            }
            else
            {
                HttpContext.Session.SetJson("Cart", cart);

            }
            return RedirectToAction("Index");
        }
        //get/cart/clear
        public IActionResult Clear()
        {
            HttpContext.Session.Remove("Cart");
           
            //return RedirectToAction("Page","Pages");
            //return Redirect("/");
            if(HttpContext.Request.Headers["X-Requested-With"] != "XMLHttpRequest")
                  return Redirect(Request.Headers["Referer"].ToString());

            return Ok();
        }
    }
}
