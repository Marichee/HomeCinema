using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HomeCinema.Domain.Models
{
    public class UserActionMovies
    {
        [Key, Column(Order = 0)]
        public int MovieId { get; set; }
        public virtual Movie Movie { get; set; }
        [Key, Column(Order = 1)]
        public int UserActionId { get; set; }
        public virtual UserActions UserActions { get; set; }
    }
}
