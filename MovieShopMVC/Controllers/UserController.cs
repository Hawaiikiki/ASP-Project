﻿using ApplicationCore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieShopMVC.Infra;

namespace MovieShopMVC.Controllers
{
    [Authorize]// check if authenticated or not
               // if not logged in, it will redirect to login page by AddAuthentication details in program.cs
    public class UserController : Controller 
    {
        private readonly ICurrentUser _currentUser;
        public UserController(ICurrentUser currentUser)
        {
            _currentUser = currentUser;
        }

        [HttpGet]
        public async Task<IActionResult> Purchases()
        {
            // get all the movies purchased by user (userId)
            // httpcontext.user.claims -> then call the database -> then get the info to the view
            var userId = _currentUser.UserId;
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> Favorites()
        {
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> EditProfile()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> EditProfile(UserEditModel model)
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> BuyMovie()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> FavoriteMovie()
        {
            return View();
        }
    }
}