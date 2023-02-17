// This brings all the MVC features we need to this file
using Microsoft.AspNetCore.Mvc;
// Be sure to use your own project's namespace here! 
namespace DojoSurvey.Controllers;
public class SurveyController : Controller
{      
    [HttpGet] 
    [Route("")]
    public ViewResult Index()
    {
        return View();
    }

    [HttpPost("results")]
    public IActionResult Results(string name, string location, string fav, string comment)
    {
        ViewBag.Name = name;
        ViewBag.Location = location;
        ViewBag.Favorite = fav;
        ViewBag.Comment = "No Comment";
        if (comment != null) {
            ViewBag.Comment = comment;
        }
        return View("Results");
    }
}