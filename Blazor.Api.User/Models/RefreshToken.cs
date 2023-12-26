namespace Blazor.Api.User.Models;

public class RefreshToken
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string? Username { get; set; }
    public Guid Token { get; set; } = Guid.NewGuid();
    public DateTime ExpirationDate { get; set; }
}