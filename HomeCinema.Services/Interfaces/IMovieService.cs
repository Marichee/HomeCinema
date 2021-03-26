using HomeCinema.WebModels.WebModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace HomeCinema.Services.Interfaces
{
   public interface IMovieService
    {
        IEnumerable<MovieViewModel> GetAllMovies();
        MovieViewModel GetMovieById(int id);
        //void CreateRatingForMovie(int movieId, string userId);
        void CreateMovie(MovieViewModel movie);
        void UpdateMovie(MovieViewModel movie);
        void DeleteMovie(int id);

    }
}
