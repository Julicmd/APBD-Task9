using System.ComponentModel.DataAnnotations;

namespace Application_Auth.Models;

public class RegisterViewModel
{
    [Required]
    [EmailAddress]
    public string Email { get; set; } 
    
    [Required]
    [MinLength(8)]
    public string Password { get; set; }
}