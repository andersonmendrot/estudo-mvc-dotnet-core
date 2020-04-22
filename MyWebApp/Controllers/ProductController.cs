using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using MyWebApp.Models;
using System;

namespace MyWebApp.Controllers {
    //The name of the controller is Product, not the class name
    //Do not use plural nouns for the Web application controller. That means, ProductController is correct but ProductsController is not. 
    //The method ShowAll is an action
    public class ProductController : Controller {
        public IActionResult ShowAll() {
            //ViewData is used to pass data from a controller to corresponding view
            ViewData["Heading"] = "All Products";
            var products = new List<Product>();
            products.Add(new Product {ID=101, Name="Apple", Price=1.1});
            products.Add(new Product {ID=202, Name="Bike", Price=2.2});
            products.Add(new Product {ID=303, Name="Calculator", Price=3.3});
            return View(products);
        }

        public IActionResult Index(){
            var vm = new ProductListVM();
            vm.Discount = new Discount{ Start=DateTime.Today, End = DateTime.Today.AddDays(30), Rate=0.75};
            vm.Products = new List<Product>();
            vm.Products.Add(new Product{ID=1001, Name="Book", Price=20});
            vm.Products.Add(new Product{ID=2002, Name="Bike", Price=30});
            vm.Products.Add(new Product{ID=3003, Name="Fireworks", Price=40});

            return View("ProductList", vm);
        }
    }

}