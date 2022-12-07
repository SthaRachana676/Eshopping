using Eshopping.web.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eshopping.web.Controllers.Infrastructure
{
    public class EshoppingCartContext:IdentityDbContext<AppUser>
    {
        public EshoppingCartContext(DbContextOptions<EshoppingCartContext> options)
           : base(options)
        {
        }
        public DbSet<Page> pages { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> products { get; set; }




        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    modelBuilder.Entity<Page>().HasData(
        //         new Page
        //         {
        //             Title = "Home",
        //             Slug = "home",
        //             Content = "home page",
        //             Sorting = 0
        //         },
        //             new Page
        //             {
        //                 Title = "About Us",
        //                 Slug = "about-us",
        //                 Content = "about us page",
        //                 Sorting = 100
        //             },
        //               new Page
        //               {
        //                   Title = "Services",
        //                   Slug = "services",
        //                   Content = "services page",
        //                   Sorting = 100
        //               },
        //               new Page
        //               {
        //                   Title = "Contact",
        //                   Slug = "contact",
        //                   Content = "contact page",
        //                   Sorting = 100
        //               }
        //        );
    }
}

