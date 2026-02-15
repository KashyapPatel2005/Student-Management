using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Authorize(Roles = "Admin")]
public class LessonsController : Controller
{
    private readonly ILessonService _lessonService;

    public LessonsController(ILessonService lessonService)
    {
        _lessonService = lessonService;
    }

    public async Task<IActionResult> Index()
    {
        var lessons = await _lessonService.GetAllAsync();
        return View(lessons);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(Lesson lesson)
    {
        if (!ModelState.IsValid)
            return View(lesson);

        await _lessonService.AddAsync(lesson);

        return RedirectToAction(nameof(Index));
    }
}
