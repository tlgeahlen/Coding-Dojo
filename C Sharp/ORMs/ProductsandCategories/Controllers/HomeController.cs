using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using ProductsandCategories.Models;
using Microsoft.EntityFrameworkCore;

namespace ProductsandCategories.Controllers;

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
        MyViewModel MyModels = new MyViewModel
        {
            AllProducts = _context.Products.ToList()
        };
        return View("Index", MyModels);
    }

    [HttpPost("product/create")]
    public IActionResult CreateProduct(Product newProduct)
    {
        if (ModelState.IsValid)
        {
            _context.Add(newProduct);
            _context.SaveChanges();
            return RedirectToAction("Index");
        } else {
            MyViewModel MyModels = new MyViewModel
            {
                AllProducts = _context.Products.ToList()
            };
            return View("Index", MyModels);
        }
    }

    [HttpGet("categories")]
    public IActionResult Categories()
    {
        MyViewModel MyModels = new MyViewModel
        {
            AllCategories = _context.Categories.ToList()
        };
        return View("Categories", MyModels);
    }

    [HttpPost("categories/create")]
    public IActionResult CreateCategory(Category newCategory)
    {
        if (ModelState.IsValid)
        {
            _context.Add(newCategory);
            _context.SaveChanges();
            return RedirectToAction("Categories");
        } else {
            MyViewModel MyModels = new MyViewModel
            {
                AllCategories = _context.Categories.ToList()
            };
            return View("Categories", MyModels);
        }
    }

    [HttpGet("products/{ProductId}")]
    public IActionResult ShowProduct(int ProductId)
    {
        Association Assoc = new Association();
        Assoc.ProductId = ProductId;
        MyViewModel MyModels = new MyViewModel()
        {
            Assoc = Assoc,
            Product = _context.Products.Include(prod => prod.CategoriesOfProduct).ThenInclude(cop => cop.Category).FirstOrDefault(p => p.ProductId == ProductId),
            NotChosenCategories = _context.Categories
                .Include(category => category.ProductsInCategory)
                .Where(category => !category.ProductsInCategory.Any(p => p.ProductId == ProductId))
                .ToList(),
        };
        return View("ShowProduct", MyModels);
    }

    [HttpGet("categories/{CategoryId}")]
    public IActionResult ShowCategory(int CategoryId)
    {
        Association Assoc = new Association();
        Assoc.CategoryId = CategoryId;
        MyViewModel MyModels = new MyViewModel()
        {
            Assoc = Assoc,
            Category = _context.Categories.Include(category => category.ProductsInCategory).ThenInclude(pic => pic.Product).FirstOrDefault(c => c.CategoryId == CategoryId),
            NotProducts = _context.Products
                .Include(product => product.CategoriesOfProduct)
                .Where(product => !product.CategoriesOfProduct.Any(c => c.CategoryId == CategoryId))
                .ToList()
        };
        return View("ShowCategory", MyModels);
    }

    [HttpPost("associations/{ProductId}/create")]
    public IActionResult AddCategory(Association newAssociation, int ProductId)
    {
        newAssociation.ProductId = ProductId;
        if (ModelState.IsValid)
        {
            _context.Add(newAssociation);
            _context.SaveChanges();
            return RedirectToAction("ShowProduct", new {ProductId = newAssociation.ProductId});
        } 
        return View("ShowProduct", new {ProductId = newAssociation.ProductId});
    }

    [HttpPost("associations/product/{CategoryId}/create")]
    public IActionResult AddProduct(Association newAssociation, int CategoryId)
    {
        newAssociation.CategoryId = CategoryId;
        if (ModelState.IsValid) 
        {
            _context.Add(newAssociation);
            _context.SaveChanges();
            return RedirectToAction("ShowCategory", new {CategoryId = newAssociation.CategoryId});
        }
        return View("ShowCategory", new {CategoryId = newAssociation.CategoryId});
    }
}