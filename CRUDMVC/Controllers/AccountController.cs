using CRUDMVC.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Security.Claims;

public class AccountController : Controller
{
    private readonly IAuthService _authService;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly SignInManager<IdentityUser> _signInManager;

    public AccountController(IAuthService authService, UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
    {
        _authService = authService;
        _userManager = userManager;
        _signInManager = signInManager;
    }

    [HttpGet]
    public IActionResult Register()
    {
        if (User.Identity.IsAuthenticated)
            return RedirectToAction("List", "Students");

        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var (success, error, role) = await _authService.RegisterAsync(model);
        if (!success)
        {
            ModelState.AddModelError("Email", error);
            return View(model);
        }

        if (role == "Admin")
            return RedirectToAction("List", "Students");

        return RedirectToAction("Index", "User");
    }

    [HttpGet]
    public IActionResult Login()
    {
        if (User.Identity.IsAuthenticated)
            return RedirectToAction("List", "Students");
        return View();
    }
    //[HttpPost]
    //public async Task<IActionResult> Login(LoginViewModel model)
    //{

    //    if (!ModelState.IsValid)
    //        return View(model);

    //    var success = await _authService.LoginAsync(model);

    //    if (!success)
    //    {
    //        ModelState.AddModelError("", "Invalid login attempt");
    //        return View(model);
    //    }

    //    if (User.IsInRole("Admin"))
    //        return RedirectToAction("List", "Students");

    //    if (User.IsInRole("User"))
    //        return RedirectToAction("Index", "User");

    //    return RedirectToAction("Index", "HomePage");
    //}


    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var (success, error) = await _authService.LoginAsync(model);

        if (!success)
        {
            ModelState.AddModelError("", error);
            return View(model);
        }

        var user = await _userManager.FindByEmailAsync(model.Email);

        if (await _userManager.IsInRoleAsync(user, "Admin"))
            return RedirectToAction("List", "Students");

        if (await _userManager.IsInRoleAsync(user, "User"))
            return RedirectToAction("Index", "User");

        return RedirectToAction("Index", "HomePage");
    }


    public async Task<IActionResult> Logout()
    {
        await _authService.LogoutAsync();
        return RedirectToAction("Login");
    }


    [HttpGet]

    public IActionResult GoogleLogin()
    {
        Console.WriteLine("GoogleLogin hit");

        var redirectUrl = Url.Action("GoogleResponse", "Account");
        var properties = _authService.GetGoogleAuthProperties(redirectUrl);

        return Challenge(properties, GoogleDefaults.AuthenticationScheme);
    }

    [HttpGet]

    public async Task<IActionResult> GoogleResponse() {


        var result = await HttpContext.AuthenticateAsync(IdentityConstants.ExternalScheme);

        if (!result.Succeeded)
            return RedirectToAction("Login");

        var email = result.Principal.FindFirstValue(System.Security.Claims.ClaimTypes.Email);

        var user = await _userManager.FindByEmailAsync(email);

        if (user == null)
        {
            user = new IdentityUser
            {
                UserName = email,
                Email = email
            };

            await _userManager.CreateAsync(user);
            await _userManager.AddToRoleAsync(user, "User");
        }

        await _signInManager.SignInAsync(user, isPersistent: false);

        if (await _userManager.IsInRoleAsync(user, "Admin"))
            return RedirectToAction("List", "Students");

        return RedirectToAction("Index", "User");


    }


}
