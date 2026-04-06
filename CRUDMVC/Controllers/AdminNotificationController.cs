using CRUDMVC.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Authorize(Roles = "Admin")]
public class AdminNotificationController : Controller
{
    private readonly ApplicationDbContext _db;
    private readonly IWebHostEnvironment _env;

    public AdminNotificationController(ApplicationDbContext db, IWebHostEnvironment env)
    {
        _db = db;
        _env = env;
    }

    public async Task<IActionResult> Index()
    {
        var notifications = await _db.Notifications.OrderByDescending(n => n.CreatedAt).ToListAsync();
        return View(notifications);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([FromForm] string title, [FromForm] string description, [FromForm] string type, [FromForm] IFormFile? file)
    {
        var notification = new Notification
        {
            Title = title,
            Description = description,
            Type = type,
            CreatedAt = DateTime.Now
        };

        if (file != null && file.Length > 0)
        {
            var uploadsFolder = Path.Combine(_env.WebRootPath, "uploads", "notifications");
            Directory.CreateDirectory(uploadsFolder);
            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            var filePath = Path.Combine(uploadsFolder, fileName);
            using (var stream = new FileStream(filePath, FileMode.Create))
                await file.CopyToAsync(stream);

            notification.FileName = file.FileName;
            notification.FilePath = "/uploads/notifications/" + fileName;
        }

        _db.Notifications.Add(notification);
        await _db.SaveChangesAsync();
        TempData["Success"] = "Notification published successfully.";
        return RedirectToAction("Index");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete([FromForm] int id)
    {
        var n = await _db.Notifications.FindAsync(id);
        if (n != null)
        {
            if (n.FilePath != null)
            {
                var fullPath = Path.Combine(_env.WebRootPath, n.FilePath.TrimStart('/'));
                if (System.IO.File.Exists(fullPath))
                    System.IO.File.Delete(fullPath);
            }
            _db.Notifications.Remove(n);
            await _db.SaveChangesAsync();
            TempData["Success"] = "Notification deleted.";
        }
        return RedirectToAction("Index");
    }
}