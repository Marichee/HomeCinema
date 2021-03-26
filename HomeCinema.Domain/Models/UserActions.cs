using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using HomeCinema.Domain.Enums;
namespace HomeCinema.Domain.Models
{
   public class UserActions
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public Actions Action { get; set; }
        [Required]
        public string UserId { get; set; }
        public virtual User User { get; set; }
        public IEnumerable<UserActionMovies> UserActionMovies { get; set; }
    }
}
