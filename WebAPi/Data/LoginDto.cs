using System.ComponentModel.DataAnnotations;

namespace WebAPi.Data;

public class LoginDto
{
    [Required]
    public string Email { get; set; }
    
    [Required]

    public string Password { get; set; }
}