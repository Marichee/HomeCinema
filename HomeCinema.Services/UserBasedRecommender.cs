using AutoMapper;
using HomeCinema.DataAccess.Interfaces;
using HomeCinema.Domain;
using HomeCinema.Domain.Models;
using HomeCinema.Services.Helpers;
using HomeCinema.Services.Interfaces;
using HomeCinema.WebModels.Enums;
using HomeCinema.WebModels.WebModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HomeCinema.Services
{
    public class UserBasedRecommender : IRecommenderService
    {
        private readonly IRepository<UserActionMovies> _userActionMovieRepo;
        private readonly IRepository<UserActions> _userActionRepo;
        private readonly IUserRepository<User> _userRepository;
        private readonly IRepository<Movie> _movieRepository;


        private readonly IMapper _mapper;
        public UserBasedRecommender(IRepository<UserActionMovies> userActionMovieRepo, IUserRepository<User> userRepository, IMapper mapper, IRepository<Movie> movieRepository, IRepository<UserActions> userActionRepo)
        {
            _userActionMovieRepo = userActionMovieRepo;
            _userRepository = userRepository;
            _mapper = mapper;
            _movieRepository = movieRepository;
            _userActionRepo = userActionRepo;


        }
        public List<MovieViewModel> GetSuggestions(string userId, int numSuggestions, UserMovieRatingsTable table)
        {

            int userIndex = table.UserIndexToId.IndexOf(userId);
            UserMoviesRatings user = table.Users[userIndex];
            List<Suggestion> suggestions = new List<Suggestion>();
            List<UserMoviesRatings> neighbors = GetNearestNeighbors(userId, user, 5, table);
            if (neighbors.ToList()[0].Score <= 0.5)
            {
                ItemBasedRecommender itemBasedRecommender = new ItemBasedRecommender(_mapper, _movieRepository);
                return itemBasedRecommender.GetSuggestions(userId, 10, table);
            }
            for (int movieIndex = 0; movieIndex < table.MovieIndexToId.Count; movieIndex++)
            {
                if (user.MovieRatings[movieIndex] != 0)
                {
                    double score = 0.0;
                    int count = 0;
                    for (var u = 0; u < neighbors.Count; u++)
                    {
                        if (neighbors[u].MovieRatings[movieIndex] != 0)
                        {
                            score += neighbors[u].MovieRatings[movieIndex] - ((u + 1.0) / 100.0);
                            count++;
                        }
                    }
                    if (count > 0)
                    {
                        score /= count;
                    }
                    suggestions.Add(new Suggestion(userId, table.MovieIndexToId[movieIndex], score));
                }
            }
            suggestions.Sort((c, n) => n.Rating.CompareTo(c.Rating));
            List<Suggestion> suggestionsList = suggestions.Take(numSuggestions).ToList();
            List<MovieViewModel> movieViewModels = new List<MovieViewModel>();
            foreach (var suggestion in suggestionsList)
            {
                Movie movie = _movieRepository.GetById(suggestion.MovieId);
                movieViewModels.Add(_mapper.Map<MovieViewModel>(movie));
            }
            return movieViewModels;
        }

        public double Rating(List<UserActionsViewModel> actions)
        {
            double downVoteWeight = -0.5;
            double upVoteWeight = 1.0;
            double viewWeight = 3.0;
            double downloadWeight = 0.5;
            double minWeight = 0.1;
            double maxWeight = 5.0;
            int up = actions.Count(x => x.Action == ActionsViewModel.upVote);
            int down = actions.Count(x => x.Action == ActionsViewModel.downVote);
            int view = actions.Count(x => x.Action == ActionsViewModel.view);
            int download = actions.Count(x => x.Action == ActionsViewModel.download);
            double rating = up * upVoteWeight + down * downVoteWeight + view * viewWeight + download * downloadWeight;
            return Math.Min(maxWeight, Math.Max(minWeight, rating));
        }
        private List<UserMoviesRatings> GetNearestNeighbors(string userId, UserMoviesRatings user, int numUsers, UserMovieRatingsTable table)
        {
            List<UserMoviesRatings> neighbors = new List<UserMoviesRatings>();
            Methods pearsoneCorrelation = new Methods { };
            for (var i = 0; i < table.Users.Count(); i++)
            {
                if (table.Users[i].UserId == user.UserId)
                {

                    table.Users[i].Score = double.NegativeInfinity;
                }
                else
                {
                    table.Users[i].Score = pearsoneCorrelation.PearsoneCorrelation(table.Users[i].MovieRatings, user.MovieRatings);
                }


            }
            var similarUsers = table.Users.OrderByDescending(x => x.Score);

            return similarUsers.Take(numUsers).ToList();
        }
        public UserMovieRatingsTable GetUserMovieRatingsTable()
        {
            var users = _mapper.Map<IEnumerable<UserViewModel>>(_userRepository.GetAll());
            var usersIdList = new List<string>();
            foreach (var user in users)
            {
                usersIdList.Add(user.Id);
            }
            UserMovieRatingsTable table = new UserMovieRatingsTable();
            table.UserIndexToId = usersIdList;
            var movies = _mapper.Map<IEnumerable<MovieViewModel>>(_movieRepository.GetAll());
            var moviesIdList = new List<int>();
            foreach (var movie in movies)
            {
                moviesIdList.Add(movie.Id);
            }
            table.MovieIndexToId = moviesIdList;
            foreach (var userId in table.UserIndexToId)
            {
                table.Users.Add(new UserMoviesRatings(userId, table.MovieIndexToId.Count));
            }
            foreach (var user in _userRepository.GetAll())
            {
                var userActions = _userActionRepo.GetAll().Where(x => x.UserId == user.Id);

                foreach (var movie in _movieRepository.GetAll())
                {
                    List<UserActionsViewModel> actionsForMovie = new List<UserActionsViewModel>();

                    foreach (var userAction in userActions)
                    {

                        var userActionsMovie = _userActionMovieRepo.GetAll().Where(x => x.MovieId == movie.Id && x.UserActionId == userAction.Id);
                        foreach (var userActionMovie in userActionsMovie)
                        {

                            UserActionsViewModel userActionsViewModel = new UserActionsViewModel
                            {
                                Action = (ActionsViewModel)userActionMovie.UserActions.Action,

                            };
                            actionsForMovie.Add(userActionsViewModel);
                        }

                    }
                    int userIndex = table.UserIndexToId.IndexOf(user.Id);
                    int movieIndex = table.MovieIndexToId.IndexOf(movie.Id);
                    table.Users[userIndex].MovieRatings[movieIndex] = Rating(actionsForMovie);
                }

            }
            return table;
        }
    }
}