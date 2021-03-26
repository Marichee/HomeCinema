using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HomeCinema.Domain.Models
{
   public class MovieRatings
    {
        [Key, Column(Order = 0)]
        public int MovieId { get; set; }
        public virtual Movie Movie { get; set; }
        [Key, Column(Order = 1)]
        public string UserId { get; set; }
        public virtual User User { get; set; }
        [Key,Column(Order =2)]
        public double Rating { get; set; }
    }
}
