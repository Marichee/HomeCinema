using System;
using System.Collections.Generic;
using System.Text;

namespace HomeCinema.WebModels.WebModels
{
   public class UserMovieRatingsTable
    {
        public List<UserMoviesRatings> Users { get; set; } = new List<UserMoviesRatings>();
        public List<string> UserIndexToId { get; set; } = new List<string>();
        public List<int> MovieIndexToId { get; set; }
    }
}
