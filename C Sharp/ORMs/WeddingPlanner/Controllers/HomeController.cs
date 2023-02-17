using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Identity;
using WeddingPlanner.Models;
using Microsoft.EntityFrameworkCore;

namespace WeddingPlanner.Controllers;

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

    [SessionCheck]
    [HttpGet("weddings")]
    public IActionResult Weddings()
    {
        MyViewModel MyModel = new MyViewModel
        {
            AllWeddings = _context.Weddings
                .Include(w => w.Attendees)
                .ThenInclude(a => a.User)
                .ToList()
        };
        return View("Weddings", MyModel);
    }

    [HttpPost("users/create")]
    public IActionResult CreateUser(User newUser)
    {
        if (ModelState.IsValid)
        {
            PasswordHasher<User> hasher = new PasswordHasher<User>();
            newUser.Password = hasher.HashPassword(newUser, newUser.Password);
            _context.Add(newUser);
            _context.SaveChanges();
            HttpContext.Session.SetInt32("uuid", newUser.UserId);
            HttpContext.Session.SetString("Username", newUser.FirstName);
            return RedirectToAction("Weddings");
        } else {
            return View("Index");
        }
    }

    [HttpPost("users/login")]
    public IActionResult LoginUser(LoginUser loginUser)
    {
        if (ModelState.IsValid)
        {
            User? userInDb = _context.Users.FirstOrDefault(u => u.Email == loginUser.LoginEmail);
            if (userInDb == null) 
            {
                ModelState.AddModelError("LoginEmail", "Invalid credentials.");
                return View("Index");
            }
            PasswordHasher<LoginUser> hasher = new PasswordHasher<LoginUser>();
            var result = hasher.VerifyHashedPassword(loginUser, userInDb.Password, loginUser.LoginPassword);
            if (result == 0)
            {
                ModelState.AddModelError("LoginEmail", "Invalid credentials.");
                return View("Index");
            }
            HttpContext.Session.SetInt32("uuid", userInDb.UserId);
            HttpContext.Session.SetString("Username", userInDb.FirstName);
            return RedirectToAction("Weddings");
        } else {
            return View("Index");
        }
    }

    [SessionCheck]
    [HttpGet("weddings/new")]
    public IActionResult NewWedding()
    {
        return View("NewWedding");
    }

    [SessionCheck]
    [HttpPost("weddings/create")]
    public IActionResult CreateWedding(Wedding newWedding)
    {
        int? creator = HttpContext.Session.GetInt32("uuid");
        newWedding.CreatorId = (int)creator;
        if (ModelState.IsValid)
        {
            _context.Add(newWedding);
            _context.SaveChanges();
            return RedirectToAction("Weddings");
        } else {
        return View("NewWedding");
        }
    }

    [SessionCheck]
    [HttpPost("logout")]
    public IActionResult Logout()
    {
        HttpContext.Session.Clear();
        return View("Index");
    }

    [SessionCheck]
    [HttpPost("weddings/{WeddingId}/destroy")]
    public IActionResult DestroyWedding(int WeddingId)
    {
        Wedding? weddingToDelete = _context.Weddings.SingleOrDefault(w => w.WeddingId == WeddingId);
        if (weddingToDelete != null)
        {
            _context.Weddings.Remove(weddingToDelete);
            _context.SaveChanges();
            return RedirectToAction("Weddings");
        }
        MyViewModel MyModel = new MyViewModel
        {
            AllWeddings = _context.Weddings
                .Include(w => w.Attendees)
                .ThenInclude(a => a.User)
                .ToList()
        };
        return View("Weddings", MyModel);
    }

    [SessionCheck]
    [HttpPost("associations/{WeddingId}/create")]
    public IActionResult RSVP(int WeddingId)
    {
        Association newAssociation = new Association();
        newAssociation.WeddingId = WeddingId;
        newAssociation.UserId = (int)HttpContext.Session.GetInt32("uuid");
        _context.Associations.Add(newAssociation);
        _context.SaveChanges();
        return RedirectToAction("Weddings");
    }

    [SessionCheck]
    [HttpPost("associations/{WeddingId}/destroy")]
    public IActionResult UnRSVP(int WeddingId)
    {
        int userId = (int)HttpContext.Session.GetInt32("uuid");
        Association? assocToDestroy = _context.Associations.SingleOrDefault(a => a.UserId == userId && a.WeddingId == WeddingId);
        if (assocToDestroy != null)
        {
            _context.Associations.Remove(assocToDestroy);
            _context.SaveChanges();
            return RedirectToAction("Weddings");
        }
        MyViewModel MyModel = new MyViewModel
        {
            AllWeddings = _context.Weddings
                .Include(w => w.Attendees)
                .ThenInclude(a => a.User)
                .ToList()
        };
        return View("Weddings");
    }

    [HttpGet("weddings/{WeddingId}")]
    public IActionResult ShowWedding(int WeddingId)
    {   
        MyViewModel MyModel = new MyViewModel
        {
            Wedding = _context.Weddings
                .Include(w => w.Attendees)
                .ThenInclude(a => a.User)
                .FirstOrDefault(f => f.WeddingId == WeddingId)
        };
        if (MyModel.Wedding != null) {
            return View("ShowWedding", MyModel);
        } else {
            return RedirectToAction("Weddings");
        }
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