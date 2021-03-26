using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace HomeCinema.WebModels.WebModels
{
    public class RegisterViewModel
    {

        [Required]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "First name too short")]
        [Display(Name = "FirstName")]
        public string FirstName { get; set; }
        [Required]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Last name too short")]
        [Display(Name = "LastName")]
        public string LastName { get; set; }
        [Required]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Username name too short")]
        [Display(Name = "Username")]
        public string Username { get; set; }
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [RegularExpression("^((?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[^a-zA-Z0-9])).{6,}$", ErrorMessage = "Passwords must be at least 6 characters and contain : upper case (A-Z), lower case (a-z), number (0-9) and special character (e.g. !@#$%^&*)")]
        public string Password { get; set; }
        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The passwords do not match.")]
        public string ConfirmPassword { get; set; }
    }

}
