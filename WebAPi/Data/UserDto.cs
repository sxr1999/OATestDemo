using System.ComponentModel.DataAnnotations;

namespace WebAPi.Data
{
    public class UserDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [Phone]
        public string Phone { get; set; }

        [Required]
        public string PassWord { get; set; }
    }
}
