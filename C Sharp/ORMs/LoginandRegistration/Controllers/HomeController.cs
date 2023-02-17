using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using LoginandRegistration.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Filters;

namespace LoginandRegistration.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private MyContext _context;

    public HomeController(ILogger<HomeController> logger, MyContext context)
    {
        _logger = logger;
        _context = context;
    }

    public IActionResult Index()
    {
        return View();
    }

    [HttpPost("users/create")]
    public IActionResult Create(User newUser)
    {
        if (ModelState.IsValid) 
        {
            PasswordHasher<User> Hasher = new PasswordHasher<User>();
            newUser.Password = Hasher.HashPassword(newUser, newUser.Password);
            _context.Add(newUser);
            _context.SaveChanges();
            HttpContext.Session.SetInt32("uuid", newUser.UserId);
            return RedirectToAction("Success");
        } else {
            return View("Index");
        }
    }

    [HttpPost("users/login")]
    public IActionResult UserLogin(UserLogin userInfo)
    {
        if (ModelState.IsValid) 
        {
            User? userInDb = _context.Users.FirstOrDefault(u => u.Email == userInfo.LoginEmail);
            if (userInDb == null)
            {
                ModelState.AddModelError("Email", "Invalid credentials.");
                return View("Index");
            } 
            PasswordHasher<UserLogin> hasher = new PasswordHasher<UserLogin>();
            var result = hasher.VerifyHashedPassword(userInfo, userInDb.Password, userInfo.LoginPassword);
            if (result == 0)
            {
                ModelState.AddModelError("Email", "Invalid credentials.");
                return View("Index");
            }
            //login success
            HttpContext.Session.SetInt32("uuid", userInDb.UserId);
            return RedirectToAction("Success");
        } else {
            return View("Index");
        }
    }

    [SessionCheck]
    [HttpGet("success")]
    public IActionResult Success()
    {   
        return View();
    }

    [SessionCheck]
    [HttpPost("logout")]
    public IActionResult Logout()
    {
        HttpContext.Session.Clear();
        return RedirectToAction("Index");
    }
}

public class SessionCheckAttribute : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        int? userId = context.HttpContext.Session.GetInt32("uuid");
        if(userId == null)
        {
            context.Result = new RedirectToActionResult("Index", "Home", null);
        }
    }
}