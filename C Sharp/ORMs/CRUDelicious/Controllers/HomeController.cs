using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using CRUDelicious.Models;

namespace CRUDelicious.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private MyContext _context;

    public HomeController(ILogger<HomeController> logger, MyContext context)
    {
        _logger = logger;
        _context = context;
    }

    [HttpGet("")]
    public IActionResult Index()
    {   
        List<Dish> allDishes = _context.Dishes.OrderByDescending(d => d.CreatedAt).ToList();
        return View("Index", allDishes);
    }

    [HttpGet("dishes/new")]
    public IActionResult New()
    {
        return View("New");
    }

    [HttpPost("create")]
    public IActionResult Create(Dish newDish)
    {
        //check validation model
        if (ModelState.IsValid)
        {
        //add to db
        _context.Add(newDish);
        //save changes
        _context.SaveChanges();
        //redirect
        return RedirectToAction("Index");
        } else {
            return View("New", newDish);
        }
    }

    [HttpGet("dishes/{DishId}")]
    public IActionResult Show(int DishId)
    {
        Dish? dishToShow = _context.Dishes.SingleOrDefault(i => i.DishId == DishId);
        if (dishToShow != null) {
            return View("Show", dishToShow);
        } else {
            return RedirectToAction("Index");
        }
    }

    [HttpPost("dishes/{DishId}/destroy")]
    public IActionResult Destroy(int DishId)
    {
        Dish? dishToDelete = _context.Dishes.SingleOrDefault(i => i.DishId == DishId);
        if (dishToDelete != null) {
            _context.Dishes.Remove(dishToDelete);
            _context.SaveChanges();
            return RedirectToAction("Index");
        } else {
            return RedirectToAction("Index");
        }
    }

    [HttpGet("dishes/{DishId}/edit")]
    public IActionResult Edit(int DishId)
    {
        Dish? dishToEdit = _context.Dishes.FirstOrDefault(i => i.DishId == DishId);
        if (dishToEdit != null)
        {
            return View(dishToEdit);
        } else {
            return RedirectToAction("Index");
        }
    }

    [HttpPost("dishes/{DishId}/update")]
    public IActionResult Update(int DishId, Dish updatedDish)
    {
        if (ModelState.IsValid) {
            Dish? oldDish = _context.Dishes.FirstOrDefault(i => i.DishId == DishId);
            oldDish.Name = updatedDish.Name;
            oldDish.Chef = updatedDish.Chef;
            oldDish.Calories = updatedDish.Calories;
            oldDish.Tastiness = updatedDish.Tastiness;
            oldDish.UpdatedAt = DateTime.Now;
            _context.SaveChanges();
            return RedirectToAction("Index");
        } else {
            return View("Edit", updatedDish);
        }
    }
}