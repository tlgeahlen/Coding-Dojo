using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using DojoSurveyWithValidations.Models;

namespace DojoSurveyWithValidations.Controllers;

public class HomeController : Controller
{
    [HttpGet("")]
    public IActionResult Index()
    {
        return View("Index");
    }

    [HttpPost("submit")]
    public IActionResult Submit(User userInstance)
    {
        if (ModelState.IsValid)
        {
            System.Console.WriteLine("valid");
            if (userInstance.Comment == null) 
            {
                userInstance.Comment = "No Comment";
            }
            return View("Results", userInstance);
        }
        else 
        {
            System.Console.WriteLine("was not valid");
            return View("Index");
        }
    }
}