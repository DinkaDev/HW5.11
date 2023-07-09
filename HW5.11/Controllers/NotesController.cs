using HW5._11.Models;
using Microsoft.AspNetCore.Mvc;

namespace HW5._11.Controllers
{
    public class NotesController : Controller
    {
        public IActionResult Index()
        {
            if (!IsUserAuthenticated())
            {
                return RedirectToAction("Login", "Account");
            }

            string userId = GetCurrentUserId();
            var notes = Database.Notes.Where(n=>n.UserId == userId).ToList();
            return View(notes);
        }

        private string GetCurrentUserId()
        {
            return HttpContext.User.Identity.Name;
        }

        private bool IsUserAuthenticated()
        {
           return HttpContext.User.Identity.IsAuthenticated;
        }

        public IActionResult Create()
        {
            if (!IsUserAuthenticated())
            {
                return RedirectToAction("Login", "Account");
            }
            return View();
        }

        [HttpPost]
        public IActionResult Create(Note note)
        {
            if (!IsUserAuthenticated())
            {
                return RedirectToAction("Login", "Account");
            }
            if(ModelState.IsValid)
            {
                note.UserId = GetCurrentUserId();
                note.Id = Database.Notes.Count + 1;
                Database.Notes.Add(note);
                return RedirectToAction("Index", "Notes");
            }
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors)
                                              .Select(e => e.ErrorMessage);
                foreach (var error in errors)
                {
                    Console.WriteLine($"Validation Error: {error}");
                }
            }
            return View(note);
        }
    }
}
