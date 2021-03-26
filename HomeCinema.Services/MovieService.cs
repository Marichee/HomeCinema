using AutoMapper;
using HomeCinema.DataAccess.Interfaces;
using HomeCinema.Domain.Models;
using HomeCinema.Services.Helpers;
using HomeCinema.Services.Interfaces;
using HomeCinema.WebModels.Enums;
using HomeCinema.WebModels.WebModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using MovieRatings = HomeCinema.Domain.Models.MovieRatings;

namespace HomeCinema.Services
{
    public class MovieService : IMovieService
    {
        private readonly IRepository<Movie> _movieRepository;
   
        private readonly IRepository<UserActionMovies> _userActionMovieRepo;
        private readonly IRepository<UserActions> _userActionRepo;
        private readonly IMapper _mapper;
        public MovieService(IRepository<UserActions> userActionRepo,IRepository<UserActionMovies> userActionMovieRepo, IRepository<Movie> movieRepository, IMapper mapper)
        {
            _mapper = mapper;
            _movieRepository = movieRepository;

            _userActionMovieRepo = userActionMovieRepo;
            _userActionRepo = userActionRepo;
            
        }
        public void CreateMovie(MovieViewModel movie)
        {
            Movie movieDb = _movieRepository.GetAll().FirstOrDefault(m => m.Name == movie.Name && m.OriginalId == movie.OriginalId);
            if (movieDb == null) { _movieRepository.Insert(_mapper.Map<Movie>(movie)); }
        }
        //public void CreateRatingForMovie(int movieId,string userId)
        //{
        //    Methods rating = new Methods { };
        //    var userActions = _userActionRepo.GetAll().Where(x => x.UserId == userId);
        //    List<UserActionsViewModel> actionsForMovie = new List<UserActionsViewModel>();
        //    foreach (var userAction in userActions)
        //    {

        //        var userActionsMovie = _userActionMovieRepo.GetAll().Where(x => x.MovieId == movieId && x.UserActionId == userAction.Id);
        //        foreach (var userActionMovie in userActionsMovie)
        //        {
        //            UserActionsViewModel userActionsViewModel = new UserActionsViewModel
        //            {
        //                Action = (ActionsViewModel)userActionMovie.UserActions.Action,

        //            };
        //            actionsForMovie.Add(userActionsViewModel);
        //        }
        //    }
        //            MovieRatings movieRating = new MovieRatings
        //    {
        //        MovieId = movieId,
        //        UserId = userId,
        //        Rating=rating.Rating(actionsForMovie)  
        //    };
        //    _ratingRepository.Insert(movieRating);
        //}
        public void DeleteMovie(int id)
        {
            _movieRepository.Delete(id);
        }
        public IEnumerable<MovieViewModel> GetAllMovies()
        {
            return _mapper.Map<IEnumerable<MovieViewModel>>(_movieRepository.GetAll());
        }
      
        public MovieViewModel GetMovieById(int id)
        {
            Movie movie = _movieRepository.GetById(id);
            if (movie == null) throw new Exception("No such movie");
            return _mapper.Map<MovieViewModel>(movie);
        }

        public void UpdateMovie(MovieViewModel movie)
        {
            _movieRepository.Update(_mapper.Map<Movie>(movie));
        }
    }
}
