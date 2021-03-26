using System;
using System.Collections.Generic;
using System.Text;

namespace HomeCinema.WebModels.WebModels
{
   public class MovieRating
    {
        public int MovieId { get; set; }
        public double Rating { get; set; }
        public MovieRating(int movieId,double rating)
        {
            MovieId = movieId;
            Rating = rating;
        }
    }
}
