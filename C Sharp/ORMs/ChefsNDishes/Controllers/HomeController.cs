using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ChefsNDishes.Models;
using Microsoft.EntityFrameworkCore;

namespace ChefsNDishes.Controllers;

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
        List<Chef> allChefs = _context.Chefs.Include(c => c.DishesFromChef).ToList();
        return View("Index", allChefs);
    }

    [HttpGet("chefs/new")]
    public IActionResult NewChef()
    {
        return View();
    }

    [HttpPost("chefs/create")]
    public IActionResult CreateChef(Chef newChef)
    {
        if (ModelState.IsValid)
        {
            _context.Add(newChef);
            _context.SaveChanges();
            return RedirectToAction("Index");
        } else {
            return View("NewChef");
        }
    }

    [HttpGet("dishes")]
    public IActionResult Dishes()
    {   
        List<Dish> allDishes = _context.Dishes.Include(d => d.Chef).ToList();
        return View("Dishes", allDishes);
    }

    [HttpGet("dishes/new")]
    public IActionResult NewDish()
    {
        ViewBag.Chefs = _context.Chefs.ToList();
        return View("NewDish");
    }

    [HttpPost("dishes/create")]
    public IActionResult CreateDish(Dish newDish)
    {
        if (ModelState.IsValid)
        {
            _context.Add(newDish);
            _context.SaveChanges();
            return RedirectToAction("Dishes");
        } else {
            ViewBag.Chefs = _context.Chefs.ToList();
            return View("NewDish");
        }
    }

}