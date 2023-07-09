using HW5._11.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HW5._11.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(string username, string password)
        {
            var userExists = Database.Users.Any(u => u.Username == username);
            if (userExists)
            {
                ModelState.AddModelError(string.Empty, "Username already exists");
                return View();
            }
            var user = new User
            {
                Id = Guid.NewGuid(),
                Username = username,
                Password = password
            };
            Database.Users.Add(user);
            return RedirectToAction("Create", "Notes");
        }
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string username, string password)
        {
            var user = Database.Users.Find(u=>u.Username == username && u.Password == password);
            if (user != null)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.Username),
                    new Claim(ClaimTypes.NameIdentifier, user.Username)
                };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var authProperties = new AuthenticationProperties();
                HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity),
                    authProperties).Wait();
                return RedirectToAction("Create", "Notes");
            }
            ModelState.AddModelError(string.Empty, "Invalid username or password");
            return RedirectToAction("Register", "Account");
        }
        public IActionResult Logout()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme).Wait();
            return RedirectToAction("Index", "Home");
        }
    }
}
