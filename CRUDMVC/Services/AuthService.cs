using CRUDMVC.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

public class AuthService : IAuthService
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly SignInManager<IdentityUser> _signInManager;
    private readonly IRoleService _roleService;

    public AuthService(
        UserManager<IdentityUser> userManager,
        SignInManager<IdentityUser> signInManager,
        IRoleService roleService)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _roleService = roleService;
    }

    public async Task<(bool Success, string Error, string Role)> RegisterAsync(RegisterViewModel model)
    {
        bool isFirstUser = !_userManager.Users.Any();
        var existingUser = await _userManager.FindByEmailAsync(model.Email);

        if (existingUser != null)
            return (false, "Email already registered", null);

        var user = new IdentityUser
        {
            UserName = model.Email,
            Email = model.Email
        };

        var createResult = await _userManager.CreateAsync(user, model.Password);

        if (!createResult.Succeeded)
            return (false, createResult.Errors.First().Description, null);

        var role = isFirstUser ? "Admin" : "User";

        var roleResult = await _userManager.AddToRoleAsync(user, role);

        if (!roleResult.Succeeded)
            return (false, roleResult.Errors.First().Description, null);

        //await _signInManager.SignOutAsync();
        await _signInManager.SignInAsync(user, isPersistent: false);

        return (true, string.Empty, role);
    }


    //public async Task<bool> LoginAsync(LoginViewModel model)
    //{
    //    var result = await _signInManager.PasswordSignInAsync(
    //        model.Email,
    //        model.Password,
    //        isPersistent: false,
    //        lockoutOnFailure: false);

    //    return result.Succeeded;
    //}


    public async Task<(bool Success, string Error)> LoginAsync(LoginViewModel model)
    {
        var user = await _userManager.FindByEmailAsync(model.Email);

        if (user == null)
            return (false, "User does not exist");

        var result = await _signInManager.CheckPasswordSignInAsync(
            user,
            model.Password,
            lockoutOnFailure: true);

        if (result.IsLockedOut)
            return (false, "Account is locked. Try again later.");

        if (result.IsNotAllowed)
            return (false, "Login not allowed.");

        if (!result.Succeeded)
            return (false, "Password is incorrect");

        await _signInManager.SignInAsync(user, false);

        return (true, string.Empty);
    }

    public async Task LogoutAsync()
    {
        await _signInManager.SignOutAsync();
    }


    public AuthenticationProperties GetGoogleAuthProperties(string redirectUrl)
    {
       return new AuthenticationProperties { RedirectUri = redirectUrl };
    }

    public async Task<IdentityUser?> HandleGoogleResponseAsync(HttpContext httpContext)
    {
        var result = await httpContext.AuthenticateAsync(
            IdentityConstants.ExternalScheme);

        if (!result.Succeeded)
            return null;

        var email = result.Principal?.FindFirstValue(ClaimTypes.Email);

        if (string.IsNullOrEmpty(email))
            return null;

        var user = await _userManager.FindByEmailAsync(email);

        if (user == null)
        {
            user = new IdentityUser
            {
                UserName = email,
                Email = email
            };

            var createResult = await _userManager.CreateAsync(user);
            if (!createResult.Succeeded)
                return null;

            await _userManager.AddToRoleAsync(user, "User");
        }

        await _signInManager.SignInAsync(user, isPersistent: false);
        return user;
    }



}


