using System.ComponentModel.DataAnnotations;

namespace Application_Auth.Models;

public class LoginViewModel
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }
    
    [Required]
    public string Password { get; set; }
}