using ApplicationCore.Models;
using ApplicationCore.ServiceContracts;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace MovieShopMVC.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;
        [HttpGet]
        public IActionResult Register()
        {
            // show the view so that user can register
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(UserRegisterModel model)
        {
            // when user registers account
            // it'll use service and hash the password to save in database
            var user = await _accountService.CreateUser(model);
            return RedirectToAction("Login");
        }
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(UserLoginModel model)
        {
            var user = await _accountService.ValidateUser(model);
            if (user == null)
            {
                return View(model);
            }
            // after successful authentication
            // create claims userid, email, firstname, lastname
            var claims = new List<Claim> {
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.NameIdentifier,user.Id.ToString()),
                new Claim(ClaimTypes.Surname,user.LastName),
                new Claim(ClaimTypes.GivenName,user.FirstName),
                new Claim("language","english")  // can also custom claim
            };
            // we need to create identity object
            // telling this identity is using following schema (from program.cs)
            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);


            // create cookie with some expiration time, automatically encrypts the information
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,new ClaimsPrincipal(claimsIdentity));


            return LocalRedirect("~/");
        }
    }
}
