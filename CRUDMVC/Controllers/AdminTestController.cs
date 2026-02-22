using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Authorize(Roles = "Admin")]
public class AdminTestController : Controller
{
    private readonly ITestService _service;

    public AdminTestController(ITestService service)
    {
        _service = service;
    }

    public async Task<IActionResult> Index()
    {
        var tests = await _service.GetAllAsync();
        return View(tests);
    }

    public IActionResult Create()
        => View();

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Test test)
    {
        if (!ModelState.IsValid)
            return View(test);

        await _service.AddTestAsync(test);

        TempData["Success"] = "Test created successfully.";

        return RedirectToAction("AddQuestion", new { testId = test.Id });
    }


    //public IActionResult AddQuestion(int testId)
    //{
    //    ViewBag.TestId = testId;
    //    return View();
    //}

    //[HttpPost]
    //public async Task<IActionResult> AddQuestion(Question question)
    //{
    //    if (!ModelState.IsValid)
    //    {
    //        ViewBag.TestId = question.TestId;
    //        return View(question);
    //    }

    //    await _service.AddQuestionAsync(question);

    //    return RedirectToAction("AddQuestion", new { testId = question.TestId });
    //}


    public IActionResult AddQuestion(int testId)
    {
        var vm = new AddMultipleQuestionsViewModel
        {
            TestId = testId
        };

        // Start with one empty question
        vm.Questions.Add(new QuestionInputModel());

        return View(vm);
    }

    [HttpPost]
    public async Task<IActionResult> AddQuestion(AddMultipleQuestionsViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        foreach (var q in model.Questions)
        {
            var question = new Question
            {
                TestId = model.TestId,
                Text = q.Text,
                OptionA = q.OptionA,
                OptionB = q.OptionB,
                OptionC = q.OptionC,
                OptionD = q.OptionD,
                CorrectAnswer = q.CorrectAnswer
            };

            await _service.AddQuestionAsync(question);
        }

        TempData["Success"] = "All questions added successfully!";
        return RedirectToAction("Index");
    }


    public async Task<IActionResult> Results()
    {
        var results = await _service.GetAllAttemptsAsync();
        return View(results);
    }
}
