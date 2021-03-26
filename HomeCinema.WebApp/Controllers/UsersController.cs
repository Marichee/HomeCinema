using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HomeCinema.Services.Interfaces;
using HomeCinema.WebModels.WebModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HomeCinema.WebApp.Controllers
{
    [Authorize]
    public class UsersController : Controller
    {
        private readonly IUserService _userService;
        private readonly IRecommenderService _recommenderService;
        public UsersController(IUserService userService,IRecommenderService recommenderService)
        {
            _userService = userService;
            _recommenderService = recommenderService;
        }
        [AllowAnonymous]
        public IActionResult LogIn()
        {
            
            return View(new LoginViewModel());
        }
        [AllowAnonymous]
        [HttpPost]
        public IActionResult LogIn(LoginViewModel model)
        {
           
            if (_userService.CheckUsername(model.Username) == false)
            {
                ModelState.AddModelError("password", "Username or password does not match");
                return View();
            }

          _userService.Login(model);
        
            return RedirectToAction("Index", "Movie");
        }
        [AllowAnonymous]
        public IActionResult Register()
        {
            return View(new RegisterViewModel());
        }
        [AllowAnonymous]
        [HttpPost]
        public IActionResult Register(RegisterViewModel model)
        {
            if (_userService.CheckUsername(model.Username) == true)
            {
                ModelState.AddModelError("username", "Username already exist");
                return View();

            }
            else if (_userService.CheckEmail(model.Email) == true)
            {
                ModelState.AddModelError("email", "Email already in use");
                return View();
            }
             _userService.Register(model);
          
          _userService.Login(new LoginViewModel {Username=model.Username,Password=model.Password });
            return RedirectToAction("index", "movie");

        }
        public IActionResult LogOut()
        {
            _userService.Logout();
            return RedirectToAction("Login", "Users");
        }
    }
}