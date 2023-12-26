using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Blazor.Api.User.Models;

public class UserLoginResponse(string at, string rf)
{
    public string AccessToken { get; set; } = at;
    public string RefreshToken { get; set; } = rf;
    public int ExpiresIn { get; set; } = 3600;
}

public class Token
{
    [Required]
    [JsonPropertyName("refresh-token")]
    public string RefreshToken { get; set; }
}

public class UserRegister
{
    [Required(ErrorMessage = "The {0} is required")]
    [EmailAddress(ErrorMessage = "The {0} is in a incorrect format")]
    public string Email { get; set; }

    [Required(ErrorMessage = "The {0} is required")]
    [StringLength(100, ErrorMessage = "The {0} must have between {2} and {1} characters", MinimumLength = 6)]
    public string Password { get; set; }

    [DisplayName("Confirm Password")]
    [Compare("Password", ErrorMessage = "The passwords doesn't match.")]
    public string ConfirmPassword { get; set; }
}

public class UserLogin
{
    [Required(ErrorMessage = "The {0} is required")]
    [EmailAddress(ErrorMessage = "The {0} is in a incorrect format")]
    public string Email { get; set; }

    [Required(ErrorMessage = "The {0} is required")]
    [StringLength(100, ErrorMessage = "The {0} must have between {2} and {1} characters", MinimumLength = 6)]
    public string Password { get; set; }
}