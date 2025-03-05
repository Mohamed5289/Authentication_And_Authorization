using System.ComponentModel.DataAnnotations;

namespace Authentication.Models
{
    public class RegisterModel
    {
        [Required(ErrorMessage = "First name is required.")]
        [StringLength(30, ErrorMessage = "First name must be at most 30 characters.")]
        [RegularExpression(@"^[A-Za-z]+$", ErrorMessage = "First name can only contain letters.")]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Last name is required.")]
        [StringLength(30, ErrorMessage = "Last name must be at most 30 characters.")]
        [RegularExpression(@"^[A-Za-z]+$", ErrorMessage = "Last name can only contain letters.")]
        public string LastName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Username is required.")]
        [StringLength(60, MinimumLength = 5, ErrorMessage = "Username must be between 5 and 60 characters.")]
        [RegularExpression(@"^[a-zA-Z0-9_]+$", ErrorMessage = "Username can only contain letters, numbers, and underscores.")]
        public string Username { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is required.")]
        [StringLength(60, MinimumLength = 8, ErrorMessage = "Password must be between 8 and 60 characters.")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@#$%^&+=!_]).{8,}$",
        ErrorMessage = "Password must be at least 8 characters long, include an uppercase letter, a lowercase letter, a digit, and a special character.")]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email is required.")]
        [StringLength(60, ErrorMessage = "Email must be at most 60 characters.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public string Email { get; set; } = string.Empty;
    }
}
