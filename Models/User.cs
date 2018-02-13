using System.ComponentModel.DataAnnotations;
 
namespace login_reg.Models
{
    public class User
    {
        [Required]
        [MinLength(2)]
        public string First_Name { get; set; }

        [Required]
        [MinLength(2)]
        public string Last_Name { get; set; }
 
        [Required]
        [EmailAddress]
        public string Email { get; set; }
 
        [Required]
        [MinLength(8)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [MinLength(8)]
        [DataType(DataType.Password)]
        [Compare(nameof(Password),ErrorMessage="Passwords do not match!")]
        public string Password_Confirmation { get; set; }
    }
}