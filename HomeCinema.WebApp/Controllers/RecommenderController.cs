using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HomeCinema.Services.Interfaces;
using HomeCinema.WebModels.WebModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace HomeCinema.WebApp.Controllers
{
    [Authorize]
    public class RecommenderController : Controller
    {
        private readonly IRecommenderService _recommenderService;
        private readonly IUserService _userService;
        public RecommenderController(IRecommenderService recommenderService, IUserService userService)
        {
            _recommenderService = recommenderService;
            _userService = userService;
        }
        [Authorize(Roles = "user")]
        public IActionResult GetSuggestions()
        {
            UserViewModel user = _userService.GetCurrentUser(User.Identity.Name);
            var table = _recommenderService.GetUserMovieRatingsTable();
            var recomendation = _recommenderService.GetSuggestions(user.Id, 10, table);
            return Json(recomendation);
        }
    }
}