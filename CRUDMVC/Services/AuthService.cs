using CRUDMVC.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;


using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;


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

        var role = isFirstUser ? "Admin" : "User";      // admin@test.com -> Test@123

        var roleResult = await _userManager.AddToRoleAsync(user, role);

        if (!roleResult.Succeeded)
            return (false, roleResult.Errors.First().Description, null);

        await _signInManager.SignInAsync(user, isPersistent: false);

        return (true, string.Empty, role);
    }

    public async Task<(bool Success, string Error)> LoginAsync(LoginViewModel model)
    {
        var user = await _userManager.FindByEmailAsync(model.Email);

        if (user == null)
            return (false, "User does not exist");

        var result = await _signInManager.CheckPasswordSignInAsync(
            user,
            model.Password,
            lockoutOnFailure: true);

        //if (result.IsLockedOut)
        //    return (false, "Account is locked. Try again later.");

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

    public async Task<string> GenerateJwtTokenAsync(IdentityUser user)
    {
        var roles = await _userManager.GetRolesAsync(user);

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Name, user.UserName)

        };

        foreach(var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }


        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("THIS_IS_MY_SUPER_SECRET_JWT_KEY_1234567890_ABCDEF_0987654321"
));

        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);


        var token = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.Now.AddHours(1),
            signingCredentials: creds
            );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }



}


