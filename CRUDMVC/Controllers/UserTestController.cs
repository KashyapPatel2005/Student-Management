using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

[Authorize(Roles = "User")]
public class UserTestController : Controller
{
    private readonly ITestService _service;
    private readonly UserManager<IdentityUser> _userManager;

    public UserTestController(ITestService service,
        UserManager<IdentityUser> userManager)
    {
        _service = service;
        _userManager = userManager;
    }

    public async Task<IActionResult> Index()
    {
        var tests = await _service.GetAllAsync();
        return View(tests);
    }

    public async Task<IActionResult> TakeTest(int id)
    {
        var test = await _service.GetByIdAsync(id);
        return View(test);
    }

    [HttpPost]
    public async Task<IActionResult> Submit(int testId)
    {
        var userId = _userManager.GetUserId(User);

        var answers = Request.Form
            .Where(x => x.Key.StartsWith("q_"))
            .ToDictionary(
                x => int.Parse(x.Key.Replace("q_", "")),
                x => x.Value.ToString());

        try
        {
            var score = await _service.SubmitTest(userId, testId, answers);
            TempData["Success"] = $"Your Score: {score}";
        }
        catch (Exception ex)
        {
            TempData["Error"] = ex.Message;
        }

        return RedirectToAction("Index");
    }
}
