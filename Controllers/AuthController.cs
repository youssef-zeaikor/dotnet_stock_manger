using Microsoft.AspNetCore.Mvc;

using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Inventory_M.Models.users_management;
using Inventory_M.Models.ViewModels;
using Inventory_M.Data;
using Microsoft.EntityFrameworkCore;

namespace Inventory_M.Controllers
{
    public class AuthController : Microsoft.AspNetCore.Mvc.Controller
    {

        private readonly InventoryDbContext _context;

        public AuthController(InventoryDbContext context)
        {
            _context = context;
        }

        public IActionResult Login()
        {
            ClaimsPrincipal claimUser = HttpContext.User;

            if (claimUser.Identity.IsAuthenticated)
                return RedirectToAction("Index", "Home");


            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(UserLoginViewModel modelLogin)
        {
            Console.WriteLine("modelLogin.Email: " + modelLogin.Email + " and modelLogin.PassWord " + modelLogin.PassWord);

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == modelLogin.Email && u.PassWord == modelLogin.PassWord);


            if (user != null)
            {


                //=======================================
                // Set session values
                HttpContext.Session.SetString("UserId", user.UserId.ToString());
                HttpContext.Session.SetString("username", user.Name);
                HttpContext.Session.SetString("userlname", user.Lname);
                HttpContext.Session.SetString("UserEmail", user.Email);
                HttpContext.Session.SetString("IsAuthenticated", "true");

                //change user status:
                user.status = true;
                _context.Update(user);
                await _context.SaveChangesAsync();

                //=======================================

                List<Claim> claims = new List<Claim>() {
                    new Claim(ClaimTypes.NameIdentifier, modelLogin.Email),
                    new Claim("OtherProperties","Example Role")

                };

                ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims,
                    CookieAuthenticationDefaults.AuthenticationScheme);

                /*AuthenticationProperties properties = new AuthenticationProperties()
                {

                    AllowRefresh = true,
                    IsPersistent = modelLogin.KeepLoggedIn
                };*/

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity));


                return RedirectToAction("Index", "Home");
            }

            ViewData["ValidateMessage"] = "Invalid email or password. Please try again.";
            return View();
        }
    }
}








