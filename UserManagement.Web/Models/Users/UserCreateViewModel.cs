using System;
using System.ComponentModel.DataAnnotations;
using UserManagement.Services.Validation;

namespace UserManagement.Web.Models.Users
{
    public class UserCreateViewModel
    {
        public int Id { get; set; }

        [Required, StringLength(50)]
        public string Forename { get; set; } = string.Empty;

        [Required, StringLength(50)]
        public string Surname { get; set; } = string.Empty;

        [Required, EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Display(Name = "Date of Birth")]
        [Required(ErrorMessage = "Date of Birth is required.")]
        [DateOfBirth(13, 110)]        
        public DateOnly? DateOfBirth { get; set; }

        [Display(Name = "Account Active")]
        public bool IsActive { get; set; }
    }
}
