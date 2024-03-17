using System.ComponentModel.DataAnnotations;

namespace AppleWalletPassWithApnsIntegration.Models;

public class UserRegistrationRequest
{
    [Required]
    public string Login { get; set; }
    [Required]
    public string Password { get; set; }
}