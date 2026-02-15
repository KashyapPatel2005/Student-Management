using CRUDMVC.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using static System.Runtime.InteropServices.JavaScript.JSType;

public interface IAuthService
{
    Task<(bool Success, string Error,string Role)> RegisterAsync(RegisterViewModel model);
    Task<(bool Success, string Error)> LoginAsync(LoginViewModel model);
    Task LogoutAsync();

    AuthenticationProperties GetGoogleAuthProperties(string redirectUrl);

    Task<IdentityUser?> HandleGoogleResponseAsync(HttpContext httpContext);

    // Role-based redirect
    //Task<string> GetRedirectUrlByRoleAsync(IdentityUser user);
}