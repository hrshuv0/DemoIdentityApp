using System.ComponentModel.DataAnnotations;

namespace DemoIdentityApp.DAL.ViewModels;

public class RegisterVm
{
    [Required]
    [EmailAddress]
    public string? Email { get; set; }
    
    [Required]
    public string? Password { get; set; }
    
    [Required]
    [Compare("Password")]
    public string? ConfirmPassword { get; set; }
}