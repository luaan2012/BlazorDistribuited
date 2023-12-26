namespace Blazor.Api.User.Models;

public class AppSettings
{
    public string SecretKey { get; set; }
    public bool ValidadeIssuer { get; set; }
    public bool ValidateAudience { get; set; }
    public bool ValidateLifetime { get; set; }
    public bool ValidateIssuerSigningKey { get; set; }
    public string ValidIssuer { get; set; }
    public string ValidAudience { get; set; }
    public int Expiration { get; set; }
}