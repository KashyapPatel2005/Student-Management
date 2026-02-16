using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

[Authorize(Roles = "User")]
public class UserController : Controller
{
    private readonly IAchievementService _achievementService;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly SignInManager<IdentityUser> _signInManager;
    private readonly ILessonService _lessonService;
    private readonly IProgressService _progressService;



    public UserController(
        IAchievementService achievementService,
        UserManager<IdentityUser> userManager,
        SignInManager<IdentityUser> signInManager,ILessonService lessonService,IProgressService progressService)
    {
        _achievementService = achievementService;
        _userManager = userManager;
        _signInManager = signInManager;
        _lessonService = lessonService;
        _progressService = progressService;
    }

    public async Task<IActionResult> Index()
    {
        var user = await _userManager.GetUserAsync(User);

        var model = new EditProfileViewModel
        {
            UserName = user.UserName,
            Email = user.Email
        };
        return View(model);
    }

    public async Task<IActionResult> Achievements()
    {
        var userId = _userManager.GetUserId(User);
        var data = await _achievementService.GetUserAchievementsAsync(userId);
        return View(data);
    }

    public IActionResult AddAchievement()
    {
        return View(new AddAchievementViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddAchievement(AddAchievementViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var userId = _userManager.GetUserId(User);
        await _achievementService.AddAchievementAsync(model, userId);

        return RedirectToAction(nameof(Achievements));
    }

    public async Task<IActionResult> DownloadCertificate(int id)
    {
        var achievement = await _achievementService.GetByIdAsync(id);
        if (achievement == null || achievement.CertificateFilePath == null)
            return NotFound();

        var filePath = Path.Combine( 
            Directory.GetCurrentDirectory(),
            "wwwroot",
            achievement.CertificateFilePath.TrimStart('/'));

        return PhysicalFile(filePath, "application/octet-stream", achievement.CertificateFileName);
    }



    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdateProfile(EditProfileViewModel model)
    {
        if (!ModelState.IsValid)
            return RedirectToAction(nameof(Index));

        var user = await _userManager.GetUserAsync(User);

        user.UserName = model.UserName;
        user.Email = model.Email;

        await _userManager.UpdateAsync(user);

        return RedirectToAction(nameof(Index));
    }


    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ChangePassword(
        string currentPassword,
        string newPassword,
        string confirmPassword)
    {
        var user = await _userManager.GetUserAsync(User);

        if (newPassword != confirmPassword)
        {
            ModelState.AddModelError("", "New password and confirm password do not match.");
            return View("Index", await BuildProfileModel());
        }

        var result = await _userManager.ChangePasswordAsync(
            user,
            currentPassword,
            newPassword);

        if (!result.Succeeded)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }

            return View("Index", await BuildProfileModel());
        }

        await _signInManager.RefreshSignInAsync(user);

        TempData["Success"] = "Password changed successfully!";
        return RedirectToAction("Index");
    }

    private async Task<EditProfileViewModel> BuildProfileModel()
    {
        var user = await _userManager.GetUserAsync(User);

        return new EditProfileViewModel
        {
            UserName = user.UserName,
            Email = user.Email
        };
    }

    public async Task<IActionResult> Lessons()
    {
        var userId = _userManager.GetUserId(User);

        var lessons = await _lessonService.GetAllAsync();
        var completed = await _progressService.GetCompletedLessonIds(userId);

        ViewBag.Completed = completed;

        return View(lessons);
    }

    [HttpPost]
    public async Task<IActionResult> CompleteLesson(int lessonId)
    {
        var userId = _userManager.GetUserId(User);

        await _progressService.MarkCompleted(userId, lessonId);

        return RedirectToAction(nameof(Lessons));
    }

    public async Task<IActionResult> Progress()
    {
        var userId = _userManager.GetUserId(User);
        
        ViewBag.title = "Pogresssss";
        


        return View();
    }




}
