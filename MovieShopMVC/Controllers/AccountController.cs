using ApplicationCore.Models;
using ApplicationCore.ServiceContracts;
using Microsoft.AspNetCore.Mvc;

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
            if (user == false)
            {
                return View(model);
            }
            return LocalRedirect("~/");
        }
    }
}
