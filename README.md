Repositório para armazenar anotações e exercícios sendo realizados no curso *MVC Application Design using .NET Core 2.0* da edX, o qual aborda de forma teórica e prática a arquitetura MVC utilizando .NET Core 2.0

**Theory of .NET Core course**

*The classification of Models*

* Domain Models Design
* View Model Design
* Data Transfer Model Design

1) Domain models: represent the real-world objects that participate in the business logic. For example, Student and Teacher objects in a school system, or Order (Pedido) and Product objects in a sales management system.. So, a Student could have name, classes, grades, etc. 

```csharp
public partial class Actor {
    public int Actor_ID {get; set;}
    public string First_Name {get; set;}
    public string Last_Name {get; set;}
    public DateTime Last_Update {get; set;}
}
```

The two modification below are more suitable for ORM. 

```csharp
public partial class Actor {
    [Key]
    public int Actor_ID {get; set;}
    public string First_Name {get; set;}
    public string Last_Name {get; set;}
    public DateTime Last_Update {get; set;}
}
 
[Table(“actor”)]
public partial class Actor {
      [Key, Column(“actor_id”)]
public int ActorID {get; set;}

[Column(“first_name”)]
public string FirstName {get; set;}

      [Column(“last_name”)]
public string LastName {get; set;}

      [Column(“last_update”)]
public DateTime LastUpdate {get; set;}
}
```

We created the model class as a partial class because we should separate the operation/behavior portion from the data/entity portion. The benefit of doing this is that you can generate the data/entity portion of the model class using automation tools without overriding of removing the operation code. For example, the code below will not be overridden when you regenerate the entity portion of the Actor class:

```csharp
public partial class Actor {
  public IList<Film> GetFilmsInStock(){
  // call stored procedures
}

public IList<Film> GetFilmsNotInStock() {
  //call stored procedures
}
```

2) View models: designed for specific views. For example, if we want to show a customer (cliente/comprador) and all his/her orders on the page, we can create a class called CustomerOrdersViewModel and let this class have a Customer type property and an IList<Order> type property. So, we will have like a composition, with a relationship that One Customer has Many Orders.

There are three types of View Model:
* Strongly-typed model view
* Loosely-typed model view that uses ListView
* Loosely-typed model view that uses ListBag


**Strongly-typed model view**

If not explicitly stated, when mentioning view model, the view model is strongly-typed. When we design a view, we can set the view model for this view. This view, actually a generic class, has a property named Model, which type is just the view model type. 

For example, In the code below, we set a view’s view model type to be the Actor class we created in the last lesson, and we also render the view model in an HTML table.

```no-highlight
@model HelloMVC.Models.Actor <!--A view model type strongly-typed for Actor-->
<html>
    <head>
    <title> Actor Detail </title>
    </head>
 
<body>
    <h2>Actor Detail</h2>
        <table border=1>
	<tr>
	    <td>ID</td>
	    <td>@Model.ActorID</td>
	</tr>
	<tr>
	    <td>Name</td>
	    <td>@($"{Model.FirstName} {Model.LastName}")</td>
	</tr>
	</table>
</body>
</html>
```

Let’s wrap up:
* We can use the domain model class as the view model class if the view just renders a domain model object
* We have to create a composite view model class for specific views if the view renders more than one object

**Loosely-typed view model**

We use it when we want to render a property that is not really necessary to have explicitly in a class. For example, seems weird add a title property to the class below if we just want to render it in a page inside h1 tags.

```csharp
public class DirectorFilmsViewModel {
    public Director Director { get; set; }
    public IList<Film> Films { get; set; }
    public string Title {get; set;}
}
```

For that, we can use two kinds of loosely-typed view models:
* ViewData**: a dictionary class that implements the IDictionary<string object> interfaces
* ViewBag**:  a dynamic object

```csharp
//Controllers/TestController.cs

public class TestController : Controller {

public IActionResult Foo(){
    //First: Using ViewData
    ViewData["Title"] = "Foo";
    ViewData["Header"] = "Header Foo";
    ViewData["Items"] = "AAA BBB CCC".Split(' ');

    //Second: Using ViewBag
    ViewBag.Title = "Bar";
    ViewBag.Header = "Header Bar";
    ViewBag.Items = "DDD EEE FFF".Split(' ');
    return View();
   }
 }
 ```
 
```no-highlight
<!--Views/Test/Foo.cshtml-->
<html>
  <head>
      <title>@ViewData["Title"]</title> <!--or @ViewBag.Title-->
  </head>
  <body>
      <h1>@ViewData["Header"]</h1> <!--or @ViewBag.Header-->
      <ul>
	  @foreach (var item in (string[])ViewData["items"]) 
		   <!--or (string[])ViewBag.Items)-->
	  {
	      <li>@item</li>
	  }
      </ul>
  </body>
</html>
```
3) Data transfer model: usually, we can them Data Transfer Objects (DTO). Sometimes we have to transfer some temporary combination of data fields from views to controllers, especially when submitting a form or invoking an AJAX call. 

*Domain Model as DTO*

```no-highlight
  <!--Views/Film/Create.cshtml-->

 <form action="film/create" method="POST">
    <span>Name</span>
    <input type="text" name="name" value="Transformer">
    <span>Year</span>
    <select name="year">
        <option value="2015">2015</option>
        <option value="2016">2016</option>    
        <option value="2017">2017</option>    
    </select><br/>
    <span>Genre</span>
    <input type="checkbox" name="genres" value="action" /><span>Action</span>
    <input type="checkbox" name="genres" value="comedy" /><span>Comedy</span>
    <input type="checkbox" name="genres" value="war" /><span>War</span><br/>
    <span>In Store</span>
    <input type="radio" name="isinstore" value="true" /><span>Yes</span>
    <input type="radio" name="isinstore" value="false" /><span>No</span><br/>
    <input type="submit" value="Create"> 
</form>


 <!--Views/Film/Update.cshtml-->

 <form action="film/update" method="POST">
    <input type="hidden" name="id" value="101" />
    <span>Name</span>
    <input type="text" name="name" value="Transformer" /><br/>
    <!--other elements-->
    <input type="submit" value="Update" />
</form>
```

*The DTO class is the same as the Film domain model class:*

```csharp
// Models/Film.cs

public class Film {
    public int ID { get; set; }
    public string Name { get; set; }
    public int Year { get; set; }
    public ICollection<Genre> Genres { get; set; }
    public bool IsInStore { get; set; }
}
```
	
*In this situation, when they are both equal, we don’t need to create CreateOrUpdateDTO al all, we can use only the Film domain model class and add the below code*:

```csharp
  // Controllers/FilmController.cs

  public IActionResult Create(Film film) {
      // create a new film
      return View(“Create”);
  }

  public IActionResult Update(Film film) {
      // update the existing film by ID
      return View(“Update”);
  }
```

*DTO Class* (if the DTO has to be different of the Film domain model class)

```csharp
public class CreateOrUpdateDTO {
    public int ID { get; set; }
    public string Name { get; set; }
    public int Year { get; set; }
    public ICollection<Genre> Genres { get; set; }
    public bool IsInStore { get; set; }
}
```


**Controllers**

A controller is a .NET class that used to define and group a set of actions. An action (or action method) is a method on a controller which handles HTTP requests. A controller logically groups actions together. 
For example, the ProductController contains the actions of creating, deleting, updating or searching products. HTTP requests are mapped to actions through URL routing.
Usually, the actions will launch the rendering process of the views and return the rendering result. Sometimes an action can transfer the logic execution to another action, which called redirection.

**TL;DR**
A controller logically groups actions together
i.e. the ProductController contains actions of CRUD. 
*URL routing: when HTTP requests are mapped to actions 
Actions launch rendering process of views and return the rendering result
*Redirection: when a action transfer the logic execution to another action

*Use a singular noun for controller of .NET Core application (ProductController)
*Use a plural noun for controller of .NET Core web API application (ProductsController)

```csharp
using Microsoft.AspNetCore.Mvc;

namespace App.Controllers {
  // 1: the class name is suffixed with "Controller"
  public class ProductController {
      // Actions
  }

  // 2: the class inherits from a class whose name is or is suffixed with “Controller”
  public class Product : Controller {
      // Actions
  }

  // 3: the class is decorated with the [Controller] attribute
  [Controller]
  public class Product {
      // Actions
  }

  // 4: preferred name
  public class ProductController : Controller {
      // Actions
  }
}
```

Common ASP.NET Core application actions always return IActionResult type values, except in the case that they returns JsonResult instances. 

The feature of converting return values to JSON objects means that ASP.NET Core web application can work as web API applications. But there is no reason for us to put web API applications actions in a web application controller.

Usually, the returns of actions are:

```csharp
return View(something);
return RedirectToAction(someAction);
```

```csharp
public class ProductController : Controller {
  public DatabaseContext dbContext { get; set; }

  public IActionResult GetProductsByName(string name) {
      IList<Product> products = null;
      if(String.IsNullOrWhiteSpace(name)) {
          products = dbContext.GetAllProducts();
      }
      else {
          products = dbContext.GetProductsByName(name);
      }

      return RedirectToAction("Index", products);
  }

  public IActionResult Index(IList<Product> products) {
      return View(products);
  }
}
```
**URL Routing**

 Map an incoming request to a router handler (usually, the handler is an action)
* Routes are case-insensitive
* There are two types of routes supported in .NET Core:

 1. Conventional routing* (most used for .NET Core web applications): “{controller=Home}/{action=Index}/{id?}”
 
 2. Attribute routing
 If a controller class is modified by [Route(“Product”)] and an action in this controller is modified by [Route(“Create”)], the URL “product/create” will be handled just by this action
 When mapping the {controller} parameter to the target controller, class, the Controller suffix (if it exists) of the class name will be ignored. For example, if the {controller} parameter gets the value Product, it matches the class ProductController, the class Product modified by [Controller] and the class Product inherits Microsoft.AspNetCore.Mvc.Controller.
 A ? suffix indicates the parameter can be omitted, for example {id?}  

You can modify the route parameter by adding constraints, as 
{id:int}, 
{ssn:regex{^\\d{{3}}-\\d{{2}}-\\d{{4}}$)}}
{password:minlength(6):maxlength(20)}

Example:
http://localhost:5000/home/index?id=101	
http://localhost:5000/home/index/101

**Modify actions with proper attributes**

Usually, for Create, Update and Delete operations, we create action pairs
One action responsible for navigating users to the operational view
The Create action without parameters will navigate the user to the page on which user creates a product (shows the page)
One action that does the current job
The Create action that has a Film parameter accepts the film created by the user

```csharp
public class FilmController : Controller {
    [HttpGet]
    public IActionResult Create() {
        return View();
    }

    [HttpPost]
    public IActionResult Create(Film film){
        if (this.ModelState.IsValid){
            //create a new film
            return View("Created");
        }
        else{
            return View("Error");
        }
    }

    [HttpGet]
    public IActionResult Update(){
        return View();
    }

    [HttpPost]
    public IActionResult Update(Film film){
        if(this.ModelState.IsValid){
            //update the existing film by Id
            return View("Updated");
        }
        else{
            return View("Error");
        }
    }

    [HttpGet]
    public IActionResult Delete(){
        return View();
    }

   [HttpPost]
   public IActionResult Delete(int id){
        if(this.ModelState.IsValid){
            //remove the existing film by Id
            return View("Deleted");
        }
        else{
            return View("Error");
        }
    }
  }
```


An example of how to get a list from database:

```csharp
public IActionResult GetAllProducts() {
    IList<Product> products = dbContext.GetAllProducts();
    return new View("ProductList", products);
}
```
 
The action above will render:...
<th>
  <td>ID</td>
  <td>Name</td>
  <td>Price</td>
</th>
<tr>
  <td>101</td>
  <td>Book</td>
  <td>19.99</td>
</tr>

* First action**: MVC web application action. It uses a view called ProductList to render a list of products and generate HTML, which is sent back to web client. 

* Views in MVC pattern**: they consist in HTML templates with view logic code, and generate a HTML content to web client
* Razor**: the name of ASP.NET Core's view engine
* View engine**: the component in ASP.NET Core which executes the rendering job, generates HTML and content using views and models

* Next three actions**: web API actions, that return the products data directly to the web client. The data is serialized to JSON string. These actions don't follow the MVC pattern

```csharp
public IList<Product> GetAllProducts() {
    IList<Product> products = dbContext.GetAllProducts();
    return products;
}

public IActionResult GetAllProducts() {
      IList<Product> products = dbContext.GetAllProducts();
      return new JsonResult(products);
}

public IActionResult GetAllProducts() {
    IList<Product> products = dbContext.GetAllProducts();
    return new Json(products);
}

//Each action above will return a JSON object:

[
   {"ID":101, "Name":"Book", "Price":19.99},
   {"ID":102, "Name":"Bike", "Price":29.99},
   {"ID":103, "Name":"Fireworks", "Price":39.99}
]
```

**Differences of [HttpGet] and [HttpPost]**

[HttpGet]: Show the page for creating/deleting/updating. Search or show data.
[HttpPost]: Actual creation/update/deletion. Complex searching.

**Extract data from URL**

```csharp
public class Product {
    public int ID { get; set; }
    public string Name { get; set; }
    public double Price { get; set; }
    public string MadeIn { get; set; }
    public bool IsAvailable { get; set; }
    public string Description { get; set; }
}
```
When a web client launches an HTTP POST request to the URL:
http://localhost:5000/product/create/101?isavailable=false&madein=USA
And the data in the body is:
name=Autopilot Car, price = 99999.9

The value of the product parameter is a reference to a Product type object created by model binding, and its property values are:


Principles
* For HTTP GET requests, use URL of query strings to transfer data
* For HTTP POST requests, use form data to transfer data
* Distribute data to different parts only when necessary

**Custom model binder**

If the type of an action parameter is too complex, the model binding engine will fail to create the object. For example, if we have domain model classes:

```csharp
public class Player {
    public string Name { get; set; }
    public int Rank { get; set; }
}

public class Game {
    public string City { get; set; }
    public Player Player1 { get; set; }
    public Player Player2 { get; set; }
}
```

And the name-value pairs in the form body are:

p1Name=Superman
p1Rank=101
p2Name=Ironman
p2Rank=202
gameCity=Seattle

We need a custom model binder, which is a class that implements the IModelBinder interface. For this case:

```csharp
public class GameModelBinder : IModelBinder {
   public Task BindModelAsync(ModelBindingContext bindingContext) {
      var game = new Game();
      var bc = bindingContext.HttpContext.Request;
      game.Player1 = new Player();
      game.Player2 = new Player();
      game.Player1.Name = bc.Form["p1Name"];
      game.Player1.Rank = int.Parse(bc.Form["p1Rank"]);
      game.Player2.Name = bc.Form["p2Name"];
      game.Player2.Rank = int.Parse(bc.Form["p2Rank"]);
game.City = bc.Form["gameCity"];
      // set the model binding result
      bindingContext.Result = ModelBindingResult.Success(game); 
      return TaskCache.CompletedTask;
   }
}
```

```csharp
public class GameController : Controller {
  public IActionResult Create(
       [ModelBinder(BinderType = typeof(GameModelBinder))] Game game) {
      // business logic ...
      return View(game);
  }
 }
```

**Views and the Razor Engine**

Code in a Razor syntax isn’t HTML code, it’s a markup syntax that consists of Razon markup, C#, and HTML.
* Static text: HTML elements and plain text
* Executable code: the code after the @ symbol, that is C# or “C#-like”
* HTML files containing Razor code have a .cshtml extension
* An .NET Core view is a .cshtml file

**Razor keywords**
* Pure Razor keywords: model, functions, inherits, section, helper
* C# Razor keywords: using, for, foreach, if, else, while, do, switch, case, default, try, catch, finally
Base type of view class: Microsoft.AspNetCore.Mvc.Razor.RazorPage<TModel>

Two common mistakes:
* Adding a semicolon (;) after Razon expressions
* Using a Razor code block @{...} where there should be @(...)

**View discovery**

* First action: Razor engine will 
1. try to find project-root level folder called Views
2. try to find subfolder that has the same name as the controller (Views/Home)
3. try to find .cshtml file which has the same name as the action (Views/Home/Index.cstml)

* Second action:
1. try to find folder Views
2. try to find the subfolder Home
3. try to find Index.cshtml

* Third action: 
1. try to find the relative path

For the first and second View methods, if the folder or view file does not exists, the Razor engine try to find for the Index.cshtml file in Views/Shared folder

```csharp
public class HomeController : Controller {
    //First action
    public IActionResult Index() {
        return View();
    }

    //Second action
    public IActionResult Index() {
        return View("Index");
    }

    //Third action
    public IActionResult Index() {
        return View("~/Views/Home/Index.cshtml");
    }
}
```

**Difference of ILIST and List**

*List<T> is a class that implements IList<T>. It’s more specific, and should be used when we want to use fields, that is, instantiate a list like this:

```csharp
List <int> myList = new List<int>();
```

* IList<T> is an interface implemented by List<T>. It should be used as arguments or properties because it’s more general. It cannot be instantiated

Wrong: 

```csharp
ILIst<int> myGenericList = new IList<int>();
```

Right: 
```csharp
IList<int> myList = new List<int>();
```

**Lab: Razor Syntax Fundamentals**

We can show a table on the screen with data from two models with the following steps:
1. Develop two kind of models:
2. Product and Discount, which are Domain Models
3. ProductListVM, which is View-Model, and used to centralize data in one point to be showed on the screen

```csharp
using System;
namespace MyWebApp.Models{
    public class Discount{
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public double Rate { get; set; }
    }
}

namespace MyWebApp.Models{
    public class Product {
        public int ID {get; set;}
        public string Name {get; set;}
        public double Price {get; set;}
    }
}


using System.Collections.Generic;
namespace MyWebApp.Models{
    public class ProductListVM{
        public Discount Discount { get; set; }
        public IList<Product> Products { get; set; }
    }
}
```

Develop a controller to deal with models and renderize them in a view

We created an instance of ProductListVM and initialize their properties Discount and Products instantiating them and initializing with data to be rendered. After that, we set a value for the model property of a view instance. In this case, the view is Model/Product/ProductList.cshtml, and the value is vm. 

```csharp
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using MyWebApp.Models;
using System;

public class ProductController : Controller {

public IActionResult Index(){
    var vm = new ProductListVM();
    vm.Discount = new Discount{ 
      Start=DateTime.Today, 
      End = DateTime.Today.AddDays(30), 
      Rate=0.75};
    vm.Products = new List<Product>();
    vm.Products.Add(new Product{ID=1001, Name="Book", Price=20});
    vm.Products.Add(new Product{ID=2002, Name="Bike", Price=30});
    vm.Products.Add(new Product{ID=3003, Name="Fireworks", Price=40});
    return View("ProductList", vm);
    public virtual ViewResult View(string ViewName, object Model) 
```


**Develop a view to show the renderized data** 

In the first two lines we say that we wanna to use ProductListVM. After that, we create some variables to hold the ProductListVM data, which are start, end, rate. Besides, we create a variable called off to hold the percentage of price that are off.
	
So we create the HTML structure to show the @off and inside that we create a table to show the list of Products that we passed as a parameter in the View(...) command.
    
 ```no-highlight
@using MyWebApp.Models
@model ProductListVM //it has the value vm, setted in View(...) in ProductController

@{
var start = this.Model.Discount.Start.ToString("d");
var end = this.Model.Discount.End.ToString("d");
var rate = this.Model.Discount.Rate;
var off = (1-rate)*100;
}

<html>
<body>
    <h1>Product List</h1>
    <h4 style="color:blue">
	The @off% of discount will be valid from @start to @end!
    </h4>

    <table border="1">
	<tr>
	    <th>ID</th>
	    <th>Name</th>
	    <th>Price</th>
	    <th>Discount Price</th>
	</tr>
	@foreach(var p in this.Model.Products){
	    <tr>
		<td>@p.ID</td>
		<td>@p.Name</td>
		<td>@p.Price</td>
		<td>@(p.Price*rate)</td>
	    </tr>
	}
    </table>
</body>
</html>
```

**Difference between @model and @Model**

@model: sets the type (name) of the view’s model (in case, ProductListVM)
@Model (or @this.Model): is the access to the Model property of the view instance

**Extension methods**

Extension methods are additional methods. They allow you to inject new methods without modifying, deriving or recompiling the original class, struct or interface. Extension methods can be added to your own custom class, .NET framework classes, C# classes, etc.

As an example, IsGreaterThan() is a method injected of int data type (Int32 struct), and written by programmer for customize the data type.

An extension methods is actually a special kind of static method defined in a static class. The first parameter of the static method has to be the type on which the extension method is applicable (in our case, int type). Then we are going to use this int i. 

```csharp
namespace ExtensionMethods; 
{
    public static class IntExtensions
    {
        public static bool isGreaterThan(this int i, int value)
        {
            return i > value;
        }
    }
}

////////////////////////////////////

using ExtensionMethods;

class MainClass {
    public static void Main(string[] args){
        int i = 10;
        bool result = i.isGreaterThan(100);
        //result = false
    }
```
 
**HTML Helpers and extension methods (.NET Core context)**

HTML helpers are used to inject HTML code using a Razor expression.
