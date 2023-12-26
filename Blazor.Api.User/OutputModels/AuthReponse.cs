namespace Blazor.Api.User.OutputModels;

public class UserLoginResponse(string at, string rf)
{
    public string AccessToken { get; set; } = at;
    public string RefreshToken { get; set; } = rf;
    public int ExpiresIn { get; set; } = 3600;
}