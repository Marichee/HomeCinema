using System;
using System.Collections.Generic;
using System.Text;

namespace HomeCinema.WebModels.WebModels
{
   public class UserMoviesRatings
    {
        public string UserId { get; set; }
        public double[] MovieRatings { get; set; }
        public double Score { get; set; }

        public  UserMoviesRatings(string userId, int movieCount)
        {
            UserId = userId;
            MovieRatings = new double[movieCount];
        }

    }
}
