using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HomeCinema.Services;
using HomeCinema.Services.Interfaces;
using HomeCinema.WebModels.WebModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HomeCinema.WebApp.Controllers
{
    [Authorize]
    public class MovieController : Controller
    {
        private readonly IMovieService _movieService;
        private readonly IUserActionService _userActionService;
        private readonly IUserService _userService;
        private readonly IRecommenderService _recommenderService;
       public MovieController(IMovieService movieService,IUserActionService userActionService,IUserService userService,IRecommenderService recommenderService)
        {
            _movieService = movieService;
            _userActionService = userActionService;
            _userService = userService;
            _recommenderService = recommenderService;
        }
       [Authorize(Roles = "user")]
        public IActionResult Index()
        {
           // UserViewModel user = _userService.GetCurrentUser(User.Identity.Name);
            var a = _movieService.GetAllMovies();

            return View();
        }
       [Authorize(Roles ="user")]
        [HttpGet("movie/MovieDetails/{id}")]
        public IActionResult MovieDetails(int id)
        {
            UserViewModel user = _userService.GetCurrentUser(User.Identity.Name);
            ViewBag.id = id;
            MovieViewModel movie = _movieService.GetAllMovies().FirstOrDefault(x => x.OriginalId == id);
            IEnumerable<UserActionsViewModel> movieAction =_userActionService.GetActionForMovie(movie.Id,user.Id);
            foreach (var model in movieAction)
            {
                if (model.Action == WebModels.Enums.ActionsViewModel.upVote)
                {
                  var upVote = WebModels.Enums.ActionsViewModel.upVote;
                    ViewBag.action = upVote;
                }
                else if (model.Action == WebModels.Enums.ActionsViewModel.downVote)
                {
                   var downVote = WebModels.Enums.ActionsViewModel.downVote;
                    ViewBag.action = downVote;
                }
            }

            return View();
        }
        [Authorize(Roles ="user")]
        [HttpGet("movie/CreateViewAction/{movie.name}/{movie.genre}/{movie.originalId}")]
        public IActionResult CreateViewAction(MovieViewModel movie)
        {
           _movieService.CreateMovie(movie);
          
            CreateAction(WebModels.Enums.ActionsViewModel.view, movie.OriginalId);
            return RedirectToAction("MovieDetails","movie",new {id=movie.OriginalId});
        }
        [Authorize(Roles = "user")]
        [HttpGet("CreateAction/{actionss}/{movieId}")]
        public IActionResult CreateAction(WebModels.Enums.ActionsViewModel actionss, int movieId)
        {
            UserViewModel user = _userService.GetCurrentUser(User.Identity.Name);
            MovieViewModel movie = _movieService.GetAllMovies().FirstOrDefault(x => x.OriginalId == movieId);

            _userActionService.CreateAction(new UserActionsViewModel
            {
                Action = actionss,
                User = new UserViewModel
                {
                    Id = user.Id
                }
            });
            _userActionService.CheckActionIfExist(movie.Id, user.Id);
            return RedirectToAction("MovieDetails", "movie", new { id = movieId });
        }
    }
}