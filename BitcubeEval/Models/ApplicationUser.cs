using System.ComponentModel.DataAnnotations;

namespace BitcubeEval.Models
{
    public class ApplicationUser
    {
        public int ID { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email Address")]
        public string EmailAddress { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{6,15}$", ErrorMessage = "Password must have at least 1 uppercase character, 1 lowercase character, 1 special character, 1 number and must be at least 6 characters long")]
        public string Password { get; set; }
    }
}
