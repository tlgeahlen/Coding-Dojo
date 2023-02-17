# ORMs II
## Partials
- partials allow us to write code snippets of cshtml that can be plugged into another cshtml file whenever we need them
- to start using partials, we must first ensure that we have tag helpers enabled in our project inside _ViewImports.cshtml
```
// other imports
@addTagHelper *, Microsoft.AspNetCore.Mvc.TagHelpers
```
> Making a Partial
- we can make a navbar snippet by creating a new file in our views folder called _Navbar.cshtml
- add any cshtml elements desired:
```html
<ul>        
    <li><a href="/">Home</a></li>        
    <li><a href="/about">About Me</a></li>    
</ul>
```
> Using the Partial
```html
@await Html.PartialAsync("_Navbar")
<h1>Welcome to this site!</h1>
<p>The rest of your View content here...</p>
```
- we used **PartialAsync** to render the partial.  Change the _Navbar to whatever the name of the partial you want to render is.
### Multiple Models in a Single View
- only one model can be used on a particular view, but we can get around this by using Partials
- each partial counts as its own page and can have its own model attached
#### _ProductForm.cshtml
```html
@model Product
<form asp-action="CreateProduct" asp-controller="Home" method="post">    
    <div class="mb-3">        
        <label asp-for="ProductName"></label>        
        <input asp-for="ProductName" class="form-control">        
        <span asp-validation-for="ProductName" class="text-danger"></span>    
    </div>    
    <div class="mb-3">        
        <label asp-for="Price"></label>        
        <input asp-for="Price" class="form-control">        
        <span asp-validation-for="Price" class="text-danger"></span>    
    </div>    
    <input type="submit" value="Add Product">
</form>
```
#### _UserForm.cshtml
```html
@model User
<form asp-action="CreateUser" asp-controller="Home" method="post">    
    <div class="mb-3">        
        <label asp-for="Username"></label>        
        <input asp-for="Username" class="form-control">        
        <span asp-validation-for="Username" class="text-danger"></span>    
    </div>    
    <div class="mb-3">        
        <label asp-for="Age"></label>        
        <input asp-for="Age" class="form-control">        
        <span asp-validation-for="Age" class="text-danger"></span>    
    </div>    
    <input type="submit" value="Add User">
</form>
```
- both forms could be added to the index like so:
- the controller can be set up to handle both requests as normal, as both forms trigger their own unique post request
```html
<h1>Add a Product</h1>
@await Html.PartialAsync("_ProductForm")
<h1>Add a User</h1>
@await Html.PartialAsync("_UserForm")
```
### Passing Data to Partials
> Creating MyViewModel
- we could use ViewBag to pass data from multiple models down to a single page, but it's considered bad practice.  We should use ViewModel instead
- we can do this by combining all the models and data types together into a new "single object" and passing it down as the ViewModel
- to do so, we need to create a new model in your models folder called MyViewModel
```cs
#pragma warning disable CS8618
namespace YourNamespace.Models;
public class MyViewModel
{    
    public Product Product {get;set;}
    public List<Product> AllProducts {get;set;}
}
```
- we can consider this model a "single object" for all intents and purposes and call it in our cshtml file
```html
@model MyViewModel
<div class="d-flex">
	@await Html.PartialAsync("_ProductForm")
	@await Html.PartialAsync("_AllProducts")
</div>
```
- the only thing missing now is our product data, which we can query the database for in the controller
```cs
// All other code
[HttpGet("")]    
public IActionResult Index()    
{   
    MyViewModel MyModels = new MyViewModel
    {
        AllProducts = _context.Products.ToList()
    };     
    return View(MyModels);    
}
```
- 1. We created a new instace of MyViewModel to fill in
- 2. We filled in AllProducts using a query
- 3. We did not need to make any mention of the singular Product
- 4. We passed down the ViewModel like normal
> Passing the Information down to the partial
```html
@model MyViewModel
<div class="d-flex">
    @await Html.PartialAsync("_ProductForm", Model.Product)
    @await Html.PartialAsync("_AllProducts", Model.AllProducts)
</div>
```
- **Special Note:** Be aware that now that Index requires information to be passed in from our database, this could cause some problems if we hit a validation error.  
---
## Login and Registration
### Registration
> Creating the Model
- elements like name, email, password, and password confirmation should be added to a model
- the password confirmation does not need to be added to the database, so we use the attribute [NotMapped], which will tell Entity Framework to not attempt to map a property to the db
```cs
#pragma warning disable CS8618
using System.ComponentModel.DataAnnotations;
// Add this using statement to access NotMapped
using System.ComponentModel.DataAnnotations.Schema;
namespace YourProjectName.models;
public class User
{        
    [Key]        
    public int UserId { get; set; }
    
    [Required]        
    public string FirstName { get; set; }
    
    [Required]        
    public string LastName { get; set; }         
    
    [Required]
    [EmailAddress]
    public string Email { get; set; }        
    
    [Required]
    [DataType(DataType.Password)]
    [MinLength(8, "Password must be at least 8 characters")]
    public string Password { get; set; }          
    
    public DateTime CreatedAt {get;set;} = DateTime.Now;        
    public DateTime UpdatedAt {get;set;} = DateTime.Now;
    
    // This does not need to be moved to the bottom
    // But it helps make it clear what is being mapped and what is not
    [NotMapped]
    // There is also a built-in attribute for comparing two fields we can use now!
    [Compare("Password")]
    public string PasswordConfirm { get; set; }
}
```
> Validating Unique Email
- we can use a custom validation to check for duplicate emails
- this can live inside the User model file or elsewhere if you intent to use it on multiple models
```cs
public class UniqueEmailAttribute : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
    	// Though we have Required as a validation, sometimes we make it here anyways
    	// In which case we must first verify the value is not null before we proceed
        if(value == null)
        {
    	    // If it was, return the required error
            return new ValidationResult("Email is required!");
        }
    	// This will connect us to our database since we are not in our Controller
        MyContext _context = (MyContext)validationContext.GetService(typeof(MyContext));
        // Check to see if there are any records of this email in our database
    	if(_context.Users.Any(e => e.Email == value.ToString()))
        {
    	    // If yes, throw an error
            return new ValidationResult("Email must be unique!");
        } else {
    	    // If no, proceed
            return ValidationResult.Success;
        }
    }
}
```
- then we can update our model to say:
```cs
[Required]
[EmailAddress]
[UniqueEmail]
public string Email { get; set; }
```
### Password Hashing
- ASP.NET Core gives us access to the  **PasswordHasher** class, which allows us to generate and verify hashed strings
- after we've passed all our validations and now we're in the controller, we need to run the commands to hash our password before it gets saved to the database.
- this requires one package import and two lines of code:
```cs
// Add this using statement to be able to use PasswordHasher
using Microsoft.AspNetCore.Identity;
// Other using statements
namespace YourNamespace.Controllers;    
public class YourController : Controller
{    
    [HttpPost("users/create")]   
    public IActionResult Method(User newUser)    
    {        
        if(ModelState.IsValid)        
        {
            // Initializing a PasswordHasher object, providing our User class as its type            
            PasswordHasher<User> Hasher = new PasswordHasher<User>();   
            // Updating our newUser's password to a hashed version         
            newUser.Password = Hasher.HashPassword(newUser, newUser.Password);            
            //Save your user object to the database 
            _context.Add(newUser);
            _context.SaveChanges();       
        } else {
            // handle else
        }   
    }
}
```
### Handling Login
- we have to use a different model for our login, as it doesn't need the same information that registration did
- this model does not require us to update the database or add the model to MyContext, because it's just for operational purposes and can do its job by just existing
#### LoginUser.cs
```cs
#pragma warning disable CS8618
// using statements and namespace go here
public class LoginUser
{
    // No other fields!
    [Required]    
    public string Email { get; set; }    
    [Required]    
    public string Password { get; set; } 
}
```
- once the form is ready to receive a LoginUser, we need to find the user in the database and compare the password provided from the form against the hashed password in the database
- if either fails we must see error messages
#### Controller.cs
```cs
public IActionResult Login(LoginUser userSubmission)
{    
    if(ModelState.IsValid)    
    {        
        // If initial ModelState is valid, query for a user with the provided email        
        User? userInDb = _context.Users.FirstOrDefault(u => u.Email == userSubmission.Email);        
        // If no user exists with the provided email        
        if(userInDb == null)        
        {            
            // Add an error to ModelState and return to View!            
            ModelState.AddModelError("Email", "Invalid Email/Password");            
            return View("SomeView");        
        }   
        // Otherwise, we have a user, now we need to check their password                 
        // Initialize hasher object        
        PasswordHasher<LoginUser> hasher = new PasswordHasher<LoginUser>();                    
        // Verify provided password against hash stored in db        
        var result = hasher.VerifyHashedPassword(userSubmission, userInDb.Password, userSubmission.Password);                                    // Result can be compared to 0 for failure        
        if(result == 0)        
        {            
            // Handle failure (this should be similar to how "existing email" is handled)        
        } 
        // Handle success (this should route to an internal page)  
    } else {
        // Handle else
    }
}
```
### Session Security
- we can track logged-in users using session
- we can add the userId to session right before redirecting the user to the first internal page 
```cs
// Surrounding registration code
HttpContext.Session.SetInt32("UserId", newUser.UserId);
return RedirectToAction("SomeAction");
// Surrounding registration code
```
- if we search for a session with a specific key and it does not exist, we get back "null"
- we can use this fact to make sure only logged-in users can see certain pages
- one way to do this is by using **ActionFilterAttribute**, which looks very similar to a custom validation but is applied to a whole route in the controller
#### Controller.cs
```cs
using Microsoft.AspNetCore.Mvc.Filters;
```
```cs
// Name this anything you want with the word "Attribute" at the end
public class SessionCheckAttribute : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        // Find the session, but remember it may be null so we need int?
        int? userId = context.HttpContext.Session.GetInt32("UserId");
        // Check to see if we got back null
        if(userId == null)
        {
            // Redirect to the Index page if there was nothing in session
            // "Home" here is referring to "HomeController", you can use any controller that is appropriate here
            context.Result = new RedirectToActionResult("Index", "Home", null);
        }
    }
}
```
- now we can include [SessionCheck] above any route where we want to verify the user is logged in
---
## Database Relationships
- when creating models, we also need to be concerned with a model's relationship with other models
### One to Many
- a relationship between two tables where one instance from a table is capable of being attached to multiple instances of another table
- ex. ONE user has MANY comments
#### User.cs
```cs
#pragma warning disable CS8618
public class User
{    
    [Key]    
    public int UserId { get; set; }       
    public string Name { get; set; } 
    public string Email { get; set; }
}
```
#### Comment.cs
```cs
#pragma warning disable CS8618
public class Comment
{    
    [Key]    
    public int CommentId { get; set; }
    public string Content { get; set; }  
    // This is the ID we will use to know which User left the comment
    // This name must match the name of the key from the User table (UserId)
    public int UserId { get; set; }
}
```
- MySQL is not capable of holding complex data like objects in its columns so we use the matching IDs to represent the relationship
- Entity Framework CAN keep track of complex data, allowing us to use something called **Navigation Property** to reference the object or list of objects that connects our tables
- Navigation Property will *NOT* map to our database by default, since it is complex data and meant only for EF Core.  
- we can create Navigation Properties that connect our models to make it easy to access the adjoining table from either side
#### User.cs
```cs
#pragma warning disable CS8618
public class User
{    
    [Key]    
    public int UserId { get; set; }      
    public string Name { get; set; } 
    public string Email { get; set; }
    // Our navigation property to track the many Comments our user has made
    public List<Comment> CreatedComments { get; set; } = new List<Comment>();
}
```
#### Comment.cs
```cs
#pragma warning disable CS8618
public class Comment
{    
    [Key]    
    public int CommentId { get; set; }  
    public string Content { get; set; }  
    public int UserId { get; set; }
    // Our navigation property to track which User made this Comment
    // It is VERY important to include the ? on the datatype or this won't work!
    public User? Creator { get; set; }
}
```
- now if we run migrations, we will see the Comment table has a column for UserId, but neither has a reference ot the navigation property.
### Include
- Navigation Property is like a bookmark.  It binds two tables together with the stipulation that if we want the data we have to *ask* for it.  Until then, it's just an empty object
The Microsoft.EntityFrameworkCore package comes with the tools we need to make talking between our tables a breeze.
- If we want to pull data about an adjoining table from another table we will use a method called `.Include()`.  This tells our query to include whatever information is inside the lambda function
```cs
public IActionResult Index()
{        
    List<Comment> allComments = _context.Comments.Include(c => c.Creator).ToList();        
    return View(allComments);
}
```
> Querying Related Data
- using .Include() we can write much more complex and detailed queries to pull data that spans across tables
- ex. we want to know how many Comments a User has made, or the User with the longest message
```cs
// Number of comments created by this User    
int numComments = _context.Users        
                          // Including Comments, so that we may query on this field        
                          .Include(user => user.CreatedComments)        
                          // Get a User with a particular UserId        
                          .FirstOrDefault(user => user.UserId == userId)        
                          // Now, with a reference to a User object, and access to a User's Comments        
                          // we can get the .Count property of the Comments List        
                          .CreatedComments.Count;         

// User with the longest Comment, we can do this in two stages    
// First, find the Length of the longest Comment    
int longestCommentLength = _context.Comments.Max(com => com.Content.Length);         
// Second, select one User whose CreatedComments has Any that matches this character count    
// Note here that CreatedComments is a List, and thus can take a LINQ expression such as .Any()    
User userWithLongest = _context.Users        
                               .Include(user => user.CreatedComments)        
                               .FirstOrDefault(user => user.CreatedComments
                               .Any(c => c.Content.Length == longestCommentLength));         

// Comments NOT related to this User:    
// Since this query only requires checking a Comment's UserId    
// and doesn't require us to check data contained in a Comment's Creator    
// we can do this WITHOUT a .Include()    
List<Comment> unrelatedComments = _context.Comments.Where(c => c.UserId != userId).ToList();
```
### Many to Many Relationships
- many to many relationships require a third table to be set up.  This is our **joining table**, where all associations between the original two tables live
- in the below example, notice how one person is able to have many subscriptions to magazines and one magazine can have many people who subscribe to it
#### Magazine.cs
```cs
public class Magazine
{    
    [Key]
    public int MagazineId { get; set; }    
    public string Title { get; set; }  
    // Our navigation property to our Subscription class
    // Notice there is NO reference to the Person class   
    public List<Subscription> Readers { get; set; } = new List<Subscription>();
}
```
#### Person.cs
```cs
public class Person
{    
    [Key]
    public int PersonId { get; set; }    
    public string Name { get; set; }  
    // Our Person class also needs a reference to Subscriptions
    // and contains NO reference to Magazines  
    public List<Subscription> Subscriptions { get; set; } = new List<Subscription>();
}
```
#### Subscription.cs
```cs
public class Subscription
{
    [Key]    
    public int SubscriptionId { get; set; } 
    // The IDs linking to the adjoining tables   
    public int PersonId { get; set; }    
    public int MagazineId { get; set; }
    // Our navigation properties - don't forget the ?    
    public Person? Person { get; set; }    
    public Magazine? Magazine { get; set; }
}
```
#### YourContext.cs
```cs
//Other code
public DbSet<Person> People { get; set; } 
public DbSet<Magazine> Magazines { get; set; } 
public DbSet<Subscription> Subscriptions { get; set; } 
```
- we need to go through the subscription table to access data from the other table
- we can do this using `.ThenInclude()`
```cs
Person personWithMags = _context.People
                                .Include(subs => subs.Subscriptions)            
                                .ThenInclude(sub => sub.Magazine)        
                                .FirstOrDefault(person => person.PersonId == personId);
```
- first we used Include to get into the subscriptions table and then we used ThenInclude to get data from the magazines table before narrowing the table down to one person
- now we would have information on one person and all the magazines they are subscribed to
> Rendering the data in the cshtml
```html
@model Person
<h1>Subscriptions for @Model.Name</h1>
@foreach(Subscription sub in Model.Subscriptions)
{    
    <p>@sub.Magazine.Title</p>
}
```
---