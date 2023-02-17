using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using SessionWorkshop.Models;

namespace SessionWorkshop.Controllers;

public class HomeController : Controller
{
    [HttpGet("")]
    public IActionResult Index()
    {
        return View();
    }

    [HttpGet("buttons")]
    public IActionResult Buttons()
    {
        string? username = HttpContext.Session.GetString("Username");
        if (username != null) {
            return View("Buttons");
        } else {
            return RedirectToAction("Index");
        }
    }

    [HttpPost("submit")]
    public IActionResult Submit(string Name)
    {
        HttpContext.Session.SetString("Username", Name);
        HttpContext.Session.SetInt32("Count", 10);
        return RedirectToAction("Buttons");
    }

    [HttpPost("logout")]
    public IActionResult Logout()
    {
        HttpContext.Session.Clear();
        return RedirectToAction("Index");
    }

    [HttpPost("plusone")]
    public IActionResult PlusOne()
    {
        int? count = HttpContext.Session.GetInt32("Count");
        if (count == null) {
            count = 0;
        }
        count += 1;
        HttpContext.Session.SetInt32("Count", (int)count);
        return RedirectToAction("Buttons");
    }
    [HttpPost("minusone")]
    public IActionResult MinusOne()
    {
        int? count = HttpContext.Session.GetInt32("Count");
        if (count == null) {
            count = 0;
        }
        count -= 1;
        HttpContext.Session.SetInt32("Count", (int)count);
        return RedirectToAction("Buttons");
    }
    [HttpPost("timestwo")]
    public IActionResult TimesTwo()
    {
        int? count = HttpContext.Session.GetInt32("Count");
        if (count == null) {
            count = 0;
        }
        count *= 2;
        HttpContext.Session.SetInt32("Count", (int)count);
        return RedirectToAction("Buttons");
    }
    [HttpPost("random")]
    public IActionResult Random()
    {
        Random rand = new Random();
        int? count = HttpContext.Session.GetInt32("Count");
        if (count == null) {
            count = 0;
        }
        count = count + rand.Next(1,11);
        HttpContext.Session.SetInt32("Count", (int)count);
        return RedirectToAction("Buttons");
    }
}