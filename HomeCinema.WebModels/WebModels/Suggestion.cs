using System;
using System.Collections.Generic;
using System.Text;

namespace HomeCinema.WebModels.WebModels
{
    public class Suggestion
    {
        public string UserId { get; set; }
        public int MovieId { get; set; }
        public double Rating { get; set; }
        public Suggestion(string userId, int movieId, double assurance)
        {
            UserId = userId;
            MovieId = movieId;
            Rating = assurance;
        }
    }
}
