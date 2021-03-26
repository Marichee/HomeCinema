using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using HomeCinema.Domain.Enums;

namespace HomeCinema.Domain.Models
{
   public class Movie
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Genre { get; set; }
        [Required]
        public int OriginalId { get; set; }
        public IEnumerable<UserActionMovies> UserActionMovies { get; set; }
        
    }
}
