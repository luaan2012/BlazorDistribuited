namespace Blazor.Api.User.InputModels;

public record RequestLogin
{
    public string Email { get; set; }
    public string Password { get; set; }
}

public class RequestRegister
{
    public string Email { get; set; }
    public string UserName { get; set; }
    public string Password { get; set; }
    public string? Name { get; set; }
    public string? LastName { get; set; }
    public string? Address { get; set; }
    public int Number { get; set; }

}