using CRUDMVC.Models;
using CRUDMVC.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Authorize(Roles = "Admin")]
public class StudentsController : Controller
{
    private readonly IStudentService _service;

    public StudentsController(IStudentService service)
    {
        _service = service;
    }
    [Authorize(Roles = "Admin")]
    [HttpGet]
    public IActionResult Add()
    {
        return View();
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<IActionResult> Add(AddStudentViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        await _service.AddStudentAsync(model);

        TempData["Success"] = "Student added successfully!";
        return RedirectToAction("List");
    }

    public async Task<IActionResult> List()
    {
        var students = await _service.GetStudentsAsync();
        return View(students);
    }
    [Authorize(Roles = "Admin")]
    [HttpGet]
    public async Task<IActionResult> Edit(Guid id)
    {
        var student = await _service.GetStudentAsync(id);

        if (student == null)
        {
            return NotFound();
        }

        return View(student);
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<IActionResult> Edit(Student model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        await _service.UpdateStudentAsync(model);

        TempData["Success"] = "Student updated successfully!";
        return RedirectToAction("List");
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _service.DeleteStudentAsync(id);

        TempData["Success"] = "Student deleted successfully!";
        return RedirectToAction("List");
    }

    [HttpGet]

    public async Task<IActionResult> ViewStudent(Guid id)
    {
        var student = await _service.GetStudentAsync(id);
        return View(student);

    }


}



