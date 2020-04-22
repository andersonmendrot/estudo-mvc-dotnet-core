using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Html;
namespace MyWebApp.Controllers{
    
    // Add actions here
    // The name of the controller has to be the same of the equivalent view. For example, in our case, TestController is the controller used for Views/Test
    // If the name change to another different of TestController, the code does not work
    // The same is valid for IActionResult Foo(), because the name of the view equivalent has to be Test/Foo.cshtml

    //Ways to mark a class as Controller
    //public class PublicController
    //public class Product : Controller
    /*[Controller]
    public class Product*/
    //public class ProductController : Controller
    public class TestController : Controller {
        
        //This controller shows how to use loosely-typed view model to pass data from controller to view
        public IActionResult Foo(){
            //First: Using ViewData
            ViewData["Title"] = "Foo";
            ViewData["Header"] = "Header Foo";
            ViewData["Items"] = "AAA BBB CCC".Split(' ');
            return View();
        }

        public IActionResult Bar(){
            //Second: Using ViewBag
            ViewBag.Title = "Bar";
            ViewBag.Header = "Header Bar";
            ViewBag.Items = "DDD EEE FFF".Split(' ');
            return View();
        }
    }
}