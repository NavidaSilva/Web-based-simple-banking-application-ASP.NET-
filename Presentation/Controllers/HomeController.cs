using Microsoft.AspNetCore.Mvc;
using Presentation.Models;
using System.Diagnostics;

namespace Presentation.Controllers
{
    [Route("presentation/[controller]")]
    public class HomeController : Controller
    {
        [HttpGet("")]
        public IActionResult Index()
        {
            return View();
        }


        [HttpGet("UserDashboard")]
        public IActionResult UserDashboard()
        {
            return View(); // This returns the User Dashboard view
        }


        [HttpGet("AdminDashboard")]
        public IActionResult AdminDashboard()
        {
            return View(); // This will return the Admin Dashboard view (can be a placeholder for now)
        }

        [HttpGet("ChangeDetails")]
        public IActionResult ChnageDetails()
        {
            return View(); // This returns the User Dashboard view
        }

    }
}


