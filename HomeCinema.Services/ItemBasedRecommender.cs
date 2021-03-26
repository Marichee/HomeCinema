
using AutoMapper;
using HomeCinema.DataAccess.Interfaces;
using HomeCinema.Domain.Models;
using HomeCinema.Services.Interfaces;
using HomeCinema.WebModels.WebModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MovieRating = HomeCinema.WebModels.WebModels.MovieRating;

namespace HomeCinema.Services.Helpers
{
    public class ItemBasedRecommender : IRecommenderService
    {
        private readonly IRepository<Movie> _movieRepository;
        private readonly IMapper _mapper;
        private double[][] transposedRatings;
        public ItemBasedRecommender(IMapper mapper, IRepository<Movie> movieRepository)
        {
            _mapper = mapper;
            _movieRepository = movieRepository;
        }


        private void FillTransposedRatings(UserMovieRatingsTable table)
        {
            int features = table.Users.Count;
            transposedRatings = new double[table.MovieIndexToId.Count][];
            for (int a = 0; a < table.MovieIndexToId.Count; a++)
            {
                transposedRatings[a] = new double[features];

                for (int f = 0; f < features; f++)
                {
                    transposedRatings[a][f] = table.Users[f].MovieRatings[a];
                }
            }
        }
      
        public List<MovieViewModel> GetSuggestions(string userId, int numSuggestions, UserMovieRatingsTable table)
        {
      
            int userIndex = table.UserIndexToId.IndexOf(userId);
            List<int> movies = GetHighestRatedArticlesForUser(userIndex, table).Take(5).ToList();
            List<Suggestion> suggestions = new List<Suggestion>();
            foreach (int movieIndex in movies)
            {
                int movieId = table.MovieIndexToId[movieIndex];
                List<MovieRating> neighboringMovies = GetNearestNeighbors(movieId, 5, table);
                foreach (MovieRating neighbor in neighboringMovies)
                {
                    int neighborArticleIndex = table.MovieIndexToId.IndexOf(neighbor.MovieId);
                    double averageMovieRating = 0.0;
                    int count = 0;
                    for (int userRatingIndex = 0; userRatingIndex < table.UserIndexToId.Count; userRatingIndex++)
                    {
                        if (transposedRatings[neighborArticleIndex][userRatingIndex] != 0)
                        {
                            averageMovieRating += transposedRatings[neighborArticleIndex][userRatingIndex];
                            count++;
                        }
                    }
                    if (count > 0)
                    {
                        averageMovieRating /= count;
                    }
                    suggestions.Add(new Suggestion(userId, neighbor.MovieId, averageMovieRating));
                }
            }
            suggestions.Sort((c, n) => n.Rating.CompareTo(c.Rating));
            var suggestionsList = suggestions.Take(numSuggestions).ToList();
            List<MovieViewModel> movieViewModels = new List<MovieViewModel>();
            foreach (var suggestion in suggestionsList)
            {
                var movieViewModel = _mapper.Map<MovieViewModel>(_movieRepository.GetAll().FirstOrDefault(x => x.Id == suggestion.MovieId));
                movieViewModels.Add(movieViewModel);
            }
            return movieViewModels;
        }
        public UserMovieRatingsTable GetUserMovieRatingsTable()
        {
            throw new NotImplementedException();
        }

        private List<int> GetHighestRatedArticlesForUser(int userIndex, UserMovieRatingsTable table)
        {
            List<Tuple<int, double>> items = new List<Tuple<int, double>>();

            for (int articleIndex = 0; articleIndex < table.MovieIndexToId.Count; articleIndex++)
            {
                if (table.Users[userIndex].MovieRatings[articleIndex] != 0)
                {
                    items.Add(new Tuple<int, double>(articleIndex, table.Users[userIndex].MovieRatings[articleIndex]));
                }
            }
            items.Sort((c, n) => n.Item2.CompareTo(c.Item2));
            return items.Select(x => x.Item1).ToList();
        }
        private List<MovieRating> GetNearestNeighbors(int movieId, int nummovies, UserMovieRatingsTable table)
        {
           FillTransposedRatings(table);
            List<MovieRating> neighbors = new List<MovieRating>();
            var mainMovieIndex = table.MovieIndexToId.IndexOf(movieId);
            Methods pearsoneCorrelation = new Methods { };
            for (int movieIndex = 0; movieIndex < table.MovieIndexToId.Count-1; movieIndex++)
            {
                int searchMovieIndex = table.MovieIndexToId[movieIndex];
                double score = pearsoneCorrelation.PearsoneCorrelation(transposedRatings[mainMovieIndex], transposedRatings[searchMovieIndex]);
                neighbors.Add(new MovieRating(searchMovieIndex, score));

            }
            neighbors.Sort((c, n) => n.Rating.CompareTo(c.Rating));
            return neighbors.Take(nummovies).ToList();
        }
    }
}
