# ORMs I
## LINQ
- pronounced "Link" and is short for **Language Integrated Query**
- generates queries to retreive data without needing to actually know how to write queries for the db that data is stored in
### LINQ Methods
- LINQ uses lambda functions as callbacks inside of its methods
```cs
List<int> numsAboveTen = numbers.Where(num => num > 10).ToList();
                                    // parameter => expression
```
> Lambda Predicates
- an expression that evaluates to a boolean.  These are the most common types of queries
- the expressions use a comparison operator (==, !=, >, <, etc)
- some operations have a second operation inside that still returns a boolean
```cs
// uses comparison operator
List<Person> namedNeil = persons.Where(person => person.FirstName == "Neil").ToList();
// uses a second operation that returns a boolean
List<Person> nameStartsWithN = persons.Where(p => p.FirstName.StartsWith("N")).ToList();
```
> Lambda Selectors
- an expression that grabs something based on a property or column in our table
- it does not return a boolean, it simply selects a column of data from our table
```cs
Person youngest = people.Min(person => person.Age);
```
### Method Syntax
- by default, a query that returns a collection of data will return an IEnumerable of that type.
- we can easily cast this to the type of data that we want using .ToList() or .ToArray()
- all methods to query the database can be chained together by separating them with a period
```cs
List<Product> orderedProductList = myProducts.Where(c => c.Category == "Clothing").OrderByDescending(p => p.Price);
// selects all products that have a category of Clothing and then orders them by descending price
```
> List of common queries:
```cs
.All()  //returns true if all items in the collection match the argument
.Any() //returns true if at least one item in the collection matches the argument
.Contains("string") //returns true if the collection contains a specific item or value
.Count() //returns the number of items in the collection
.First() //returns the first item in the collection
.FirstOrDefault() //returns the first item from the collection to match the arguments or a default value (null)
.Last() //returns the last item from the collection
.Max() //returns the largest value in the collection
.Min() //returns the smallest value in the collection
.OrderBy() //sorts the collection in ascending order based on argument
.OrderByDescending() //sorts the collection in descending order based on argumentj
.Select() //returns the result of a specified query
.Sum() //totals the values specified by the collection
.Take(int) //selects a specified number of elements from the start of the collection
.ToArray()/.ToList() //converts the data provided to some collection type
.Where() //filters the items based on the argument
```
---
## Entity Framework Core
- EF Core is able to map our model classes to SQL tables and format the SQL statements from methods that we make
- Install the EF Core using the following line of code:
```cs
dotnet tool install --global dotnet-ef
// can check install using:
dotnet ef
```
### Initial Setup and Models
> Making our project and adding packages
```cs
//create project in the usual way
dotnet new mvc --no-https -o ProjectName
//run two commands 
dotnet add package Pomelo.EntityFrameworkCore.MySql --version 6.0.1
dotnet add package Microsoft.EntityFrameworkCore.Design --version 6.0.3
```
- you can verify these worked by looking in the ProjectName.csproj file and looking for the packages
> Making our Models
- model name should be Singular, i.e. User, Monster, Pet
- the names given to the properties will be the columns in our database
- all names should be in PascalCase
- we need an auto-incremented integer ID named "ModelNameId"
```cs
#pragma warning disable CS8618
using System.ComponentModel.DataAnnotations;
namespace YourProjectName.Models;
public class Monster
{
    [Key]
    public int MonsterId { get; set; }
    public string Name { get; set; } 
    public int Height { get; set; }
    public string Description { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime UpdatedAt { get; set; } = DateTime.Now;
}
```
- [Key] indicates to our database that this is the name of our key in the db
- the name of Key **MUST** be ModelNameId
- use default values for CreatedAt and UpdatedAt by adding = DateTime.Now to the end
> The Context Class
- the **context class** forms our relationship between our app and the database and translates our queries for us
- by convention, they always have names that end in Context
#### Models/MyContext.cs
```cs
#pragma warning disable CS8618
// We can disable our warnings safely because we know the framework will assign non-null values 
// when it constructs this class for us.
using Microsoft.EntityFrameworkCore;
namespace YourProjectName.Models;
// the MyContext class represents a session with our MySQL database, allowing us to query for or save data
// DbContext is a class that comes from EntityFramework, we want to inherit its features
public class MyContext : DbContext 
{   
    // This line will always be here. It is what constructs our context upon initialization  
    public MyContext(DbContextOptions options) : base(options) { }    
    // We need to create a new DbSet<Model> for every model in our project that is making a table
    // The name of our table in our database will be based on the name we provide here
    // This is where we provide a plural version of our model to fit table naming standards    
    public DbSet<Monster> Monsters { get; set; } 
}
```
### Connecting to MySQL
- create a configuration file to store your username and password so that you can .gitignore the file and not have your information visible when you upload to GitHub
- projects, by default if you used dotnet new mvc come with a file called **appsettings.json** in the main directory
- the code inside can be overwritten with the below code, making special note of the connections strings field
```json
{  
    "Logging": {    
        "LogLevel": {      
            "Default": "Information",      
            "Microsoft.AspNetCore": "Warning"    
        }  
    },  
    "AllowedHosts": "*",    
    "ConnectionStrings":    
    {        
        "DefaultConnection": "Server=localhost;port=3306;userid=root;password=root;database=monsterdb;"    
    }
}
```
- Update the userid to your userid for MySQL (often root)
- Update the password to your MySQL password (often root)
- Update database to a unique name for your project **IMPORTANT**
- (optional) change the port to whatever your MySQL runs on
#### Program.cs
```cs
// Add this using statement
using Microsoft.EntityFrameworkCore;
// You will need access to your models for your context file
using ProjectName.Models;
// Builder code from before
var builder = WebApplication.CreateBuilder(args)
// Create a variable to hold your connection string
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
// All your builder.services go here
// And we will add one more service
// Make sure this is BEFORE var app = builder.Build()!!
builder.Services.AddDbContext<MyContext>(options =>
{
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
});
// The rest of the code below
```
### Migrations and Controllers
- migrations are files that contain instructions for the database to create or modify tables
- they allow us to use MySQL without ever needing to open the program
> Adding Migrations
```
dotnet ef migrations add FirstMigration
```
- FirstMigration is a chosen name.  It can be anything, but they must be unique and not reused for the same project.
- a good rule of thumb is to use FirstMigrations, SecondaMigration, ThirdMigration, etc.
- note that adding model validations like [Required] does NOT require another migration, because it does not affect how the data is stored in the database
> Applying Migrations to the Database
- the following command takes the migration files we generated and applies them to the database, performing the actual table creation.
- the name of the tables is create is deteremines by the name we gave the corrosponding DbSet (in the context file)
```
dotnet ef database update
```
- if you get an error that build fails when running migrations, something is wrong in your code somewhere.
- run the migrations add line again but put a -v at the end for verbose mode
- if you messed something up, you can delete the migrations folder and start over
> Controller Connection
- finally, setup the controller to be able to access information from our db
- we do this in the Context file using **dependency injection**, by initializing our context in our controller's constructor method
#### Controller.cs
```cs
// Using statements
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using YourProjectName.Models;
namespace YourProjectName.Controllers;    
public class HomeController : Controller
{    
    private readonly ILogger<HomeController> _logger;
    // Add a private variable of type MyContext (or whatever you named your context file)
    private MyContext _context;         
    // Here we can "inject" our context service into the constructor 
    // The "logger" was something that was already in our code, we're just adding around it   
    public HomeController(ILogger<HomeController> logger, MyContext context)    
    {        
        _logger = logger;
        // When our HomeController is instantiated, it will fill in _context with context
        // Remember that when context is initialized, it brings in everything we need from DbContext
        // which comes from Entity Framework Core
        _context = context;    
    }         
    [HttpGet("")]    
    public IActionResult Index()    
    {     
        // Now any time we want to access our database we use _context   
        List<Monster> AllMonsters = _context.Monsters.ToList();
        return View();    
    } 
}
```
---
## CRUD
### Create
- adding objects to the database in the controller file
- we verify that our form passes the model validations
- then, we access our database using _context and use the .Add method
- finally, we save our changes using _context.SaveChanges()
```cs
// Inside HomeController
[HttpPost("monsters/create")]
public IActionResult CreateMonster(Monster newMon)
{    
    if(ModelState.IsValid)
    {
        // We can take the Monster object created from a form submission
        // and pass the object through the .Add() method  
        // Remember that _context is our database  
        _context.Add(newMon);    
        // OR _context.Monsters.Add(newMon); if we want to specify the table
        // EF Core will be able to figure out which table you meant based on the model  
        // VERY IMPORTANT: save your changes at the end! 
        _context.SaveChanges();
        return RedirectToAction("SomeAction");
    } else {
        // Handle unsuccessful validations
    }
}
```
### Read
> Reading data from our database
- now that we have data in our database, we can read that information to access it
- again, we use _context to access the database and the name of the table
- then we use LINQ to narrow down the data we're getting back
```cs
[HttpGet("")]    
public IActionResult Index()    
{        
    // Get all Monsters
    ViewBag.AllMonsters = _context.Monsters.ToList();             
    
    // Get Monsters with the Name "Mike"
    ViewBag.AllMikes = _context.Monsters.Where(n => n.Name == "Mike");     	
    
    // Get the 5 most recently added Monsters        
    ViewBag.MostRecent = _context.Monsters.OrderByDescending(u => u.CreatedAt).Take(5).ToList(); 	
    
    // Get one Monster who has a certain description
    ViewBag.MatchedDesc = _context.Monsters.FirstOrDefault(u => u.Description == "Super scary");
    return View();  
}
```
> Know What you're getting back
- .Where() gives back an IEnumerable<>, even if only a single result
- .Select() gives back a single object of a data type
### Update
> Creating the update form
- it can look visually the same, so copy/paste the original form
- use a unique characteristic (usually ID) to determine what data should go in the form.  This can be used in the route link to the page
> Auto-populating the Form
- when routing to the edit page, query for the unique ID and pass the item down as a ViewModel
- the **asp-for**'s throughout the form will auto-populate the form for you using the ViewModel data, as long as your form still has all the asp-for attributes and the @modelName
```cs
[HttpGet("monsters/{MonsterId}/edit")]
public IActionResult EditMonster(int MonsterId)
{
    Monster? MonsterToEdit = _context.Monsters.FirstOrDefault(i => i.MonsterId == MonsterId);
    // Tip: it would be good to add a check here to ensure what you are grabbing will not return a null item
    return View(MonsterToEdit);
}
```
> Handling the POST request
- trigger the post request that contains the updated instance from the form data and the ID of that instance
- verify that the new version of our instance passes validations
- if it does, find the old version of the instance in the db and continue, otherwise, display error messages
- overwrite the old version with the new version
- save changes and redirect to an appropriate page
```html
<form asp-action="UpdateMonster" asp-controller="Home" asp-route-MonsterId="@Model.MonsterId" method="post">
@* the rest of our form *@
```
```cs
// {MonsterId} comes from asp-route-MonsterId
[HttpPost("monsters/{MonsterId}/update")]
// We are passing down both the Monster from our form and the ID from our route parameter
public IActionResult UpdateMonster(Monster newMon, int MonsterId)
{ 
    // Code coming soon 
}
```
```cs
// 1. Trigger a post request that contains the updated instance from our form and the ID of that instance
[HttpPost("monsters/{MonsterId}/update")]
public IActionResult UpdateMonster(Monster newMon, int MonsterId)
{
    // 2. Verify that the new instance passes validations
    if(ModelState.IsValid)
    {
    	// 3. If it does, find the old version of the instance in your database
        Monster? OldMonster = _context.Monsters.FirstOrDefault(i => i.MonsterId == MonsterId);
        // 4. Overwrite the old version with the new version
    	// Yes, this has to be done one attribute at a time
    	OldMonster.Name = newMon.Name;
        OldMonster.Height = newMon.Height;
        OldMonster.Description = newMon.Description;
    	// You updated it, so update the UpdatedAt field!
        OldMonster.UpdatedAt = DateTime.Now;
    	// 5. Save your changes
        _context.SaveChanges();
    	// 6. Redirect to an appropriate page
        return RedirectToAction("Index");
    } else {
    	// 3.5. If it does not pass validations, show error messages
    	// Be sure to pass the form back in so you don't lose your changes
        return View("EditMonster", newMon);
    }
}
```
### Delete
- the easiest way to access one item is by using it's unique ID, which can be placed in the route
- similar to updating, we want to trigger a delete POST request
- the delete feature can be created using a form with only a submit button:
```html
<form asp-action="DestroyMonster" asp-controller="Home" asp-route-MonsterId="@Model.MonsterId" method="post">
    <input type="submit" value="Delete">
</form>
```
- the keyword to delete something from the database is "Remove"
#### Controller.cs
```cs
[HttpPost("monsters/{MonsterId}/destroy")]
public IActionResult DestroyMonster(int MonsterId)
{
    Monster? MonToDelete = _context.Monsters.SingleOrDefault(i => i.MonsterId == MonsterId);
    // Once again, it could be a good idea to verify the monster exists before deleting
    _context.Monsters.Remove(MonToDelete);
    _context.SaveChanges();
    return RedirectToAction("Index");
}
```
> SingleOrDefault vs FirstOrDefault
- we use single because it will only find one instance that matches our query, and if it finds more it will throw an error, reducing ambiguity
### Restful Routing
- REST stands for Representational State Transfer
- it is a set of standards used to create efficient, reusable routes for CRUD operations.
```
Route Name | URL Path              | HTTP METHOD | Purpose
Index      | /objects              | GET         | display all
New        | /objects/new          | GET         | view form to create new obj
Create     | objects/create        | POST        | create new obj
Show       | /objects/{id}         | GET         | display one object
Edit       | /objects/{id}/edit    | GET         | view form to update
Update     | /objects/{id}/update  | POST        | updates one object
Destroy    | /objects/{id}/destroy | POST        | delete one object
```
---