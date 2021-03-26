using HomeCinema.WebModels.WebModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace HomeCinema.Services.Interfaces
{
   public interface IRecommenderService
    {
        List<MovieViewModel> GetSuggestions(string userId, int numSuggestions,UserMovieRatingsTable table);
        UserMovieRatingsTable GetUserMovieRatingsTable();
    }
}
