using HW5._11.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace HW5._11.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return RedirectToAction("Index","Notes");
        }
    }
}