using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Blazor.Api.Core.OutputModels;
using Blazor.Api.User.InputModels;
using Blazor.Api.User.Models;
using Blazor.Api.User.OutputModels;
using MassTransit;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using NetDevPack.Security.Jwt.Core.Interfaces;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;
using UserLoginResponse = Blazor.Api.User.Models.UserLoginResponse;

namespace Blazor.Api.User.Services;

public interface IAuthServices
{
    Task<UserLoginResponse> RefreshToken(Token token);
    Task<string> GenerateAccessToken(string? email);
    Task<string> GenerateRefreshToken(string? email);
    Task UpdateLastGeneratedClaim(string? email, string jti);
    Task<Result<UserLoginResponse, string>> SignIn(RequestLogin request);
    Task<Result<IdentityResult, IEnumerable<IdentityError>>> SignUp(RequestRegister request);
}

public class AuthServices(
    IJwtService jwtService,
    SignInManager<IdentityUser> signInManager,
    UserManager<IdentityUser> userManager,
    IOptionsMonitor<AppSettings> appSettings,
    IPublishEndpoint bus
) : IAuthServices
{
    private readonly IPublishEndpoint _publishEndpoint = bus;
    private readonly IJwtService _jwtService = jwtService;
    private readonly SignInManager<IdentityUser> _signInManager = signInManager;
    private readonly UserManager<IdentityUser> _userManager = userManager;
    private readonly AppSettings _appSettings = appSettings.CurrentValue;

    public async Task<Result<UserLoginResponse, string>> SignIn(RequestLogin request)
    {
        var result = await _signInManager.PasswordSignInAsync(request.Email, request.Password, false, true);

        if (result.Succeeded)
        {
            var at = await GenerateAccessToken(request.Email);
            var rt = await GenerateRefreshToken(request.Email);
            return new UserLoginResponse(at, rt);
        }

        return "Não autorizado";
    }

    public async Task<Result<IdentityResult, IEnumerable<IdentityError>>> SignUp(RequestRegister request)
    {
        var user = new IdentityUser
        {
            UserName = request.UserName,
            Email = request.Email,
            PasswordHash = request.Password
        };

        var result = await _userManager.CreateAsync(user, request.Password);

        if (result.Succeeded)
        {
            await _publishEndpoint.Publish(ClientToClientCommand(request));
            return result;
        }

        return result.Errors.ToArray();
    }

    public async Task<UserLoginResponse> RefreshToken(Token token)
    {
        var handler = new JsonWebTokenHandler();

        var result = handler.ValidateToken(token.RefreshToken, new TokenValidationParameters()
        {
            ValidIssuer = "https://localhost", // <- Your website
            ValidAudience = "BlazorApp",
            RequireSignedTokens = false,
            IssuerSigningKey = await _jwtService.GetCurrentSecurityKey(),
        });

        if (!result.IsValid)
            return null;

        var user = await _userManager.FindByEmailAsync(result.Claims[JwtRegisteredClaimNames.Email].ToString());
        var claims = await _userManager.GetClaimsAsync(user);

        if (!claims.Any(c =>
                c.Type == "LastRefreshToken" && c.Value == result.Claims[JwtRegisteredClaimNames.Jti].ToString()))
            return null;

        if (user.LockoutEnabled)
            if (user.LockoutEnd < DateTime.Now)
                return null;

        if (claims.Any(c => c.Type == "TenhoQueRelogar" && c.Value == "true"))
            return null;


        var at = await GenerateAccessToken(result.Claims[JwtRegisteredClaimNames.Email].ToString());
        var rt = await GenerateRefreshToken(result.Claims[JwtRegisteredClaimNames.Email].ToString());
        return new UserLoginResponse(at, rt);
    }

    public async Task<string> GenerateAccessToken(string? email)
    {
        var user = await _userManager.FindByEmailAsync(email);
        var userRoles = await _userManager.GetRolesAsync(user);
        var identityClaims = new ClaimsIdentity();
        identityClaims.AddClaims(await _userManager.GetClaimsAsync(user));
        identityClaims.AddClaims(userRoles.Select(s => new Claim("role", s)));

        identityClaims.AddClaim(new Claim(JwtRegisteredClaimNames.Sub, user.Id));
        identityClaims.AddClaim(new Claim(JwtRegisteredClaimNames.Email, user.Email));
        identityClaims.AddClaim(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));

        var tokenHandler = new JwtSecurityTokenHandler();

        var token = tokenHandler.CreateToken(new SecurityTokenDescriptor
        {
            Issuer = _appSettings.ValidIssuer, 
            Audience = _appSettings.ValidAudience,
            IssuedAt = DateTime.Now,
            NotBefore = DateTime.Now,
            Expires = DateTime.UtcNow.AddHours(_appSettings.Expiration),
            Subject = identityClaims,
            SigningCredentials = await _jwtService.GetCurrentSigningCredentials()
        });

        return tokenHandler.WriteToken(token);
    }

    public async Task<string> GenerateRefreshToken(string? email)
    {
        var jti = Guid.NewGuid().ToString();
        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Email, email),
            new Claim(JwtRegisteredClaimNames.Jti, jti)
        };

        var identityClaims = new ClaimsIdentity();
        identityClaims.AddClaims(claims);

        var handler = new JwtSecurityTokenHandler();

        var securityToken = handler.CreateToken(new SecurityTokenDescriptor
        {
            Issuer = "https://localhost", // <- Your website                
            Audience = "BlazorApp",
            SigningCredentials = await _jwtService.GetCurrentSigningCredentials(),
            Subject = identityClaims,
            NotBefore = DateTime.Now,
            Expires = DateTime.Now.AddDays(30),
            TokenType = "rt+jwt"
        });
        await UpdateLastGeneratedClaim(email, jti);
        var encodedJwt = handler.WriteToken(securityToken);
        return encodedJwt;
    }

    public async Task UpdateLastGeneratedClaim(string? email, string jti)
    {
        var user = await _userManager.FindByEmailAsync(email);
        var claims = await _userManager.GetClaimsAsync(user);
        var newLastRtClaim = new Claim("LastRefreshToken", jti);

        var claimLastRt = claims.FirstOrDefault(f => f.Type == "LastRefreshToken");
        if (claimLastRt != null)
        {
            await _userManager.ReplaceClaimAsync(user, claimLastRt, newLastRtClaim);
        }
        else
        {
            await _userManager.AddClaimAsync(user, newLastRtClaim);
        }
    }
    private ClientCommander ClientToClientCommand(RequestRegister requestRegister)
    {
        return new ClientCommander
        {
            Name = requestRegister.Name,
            LastName = requestRegister.LastName,
            Address = requestRegister.Address,
            Number = requestRegister.Number
        };
    }
}