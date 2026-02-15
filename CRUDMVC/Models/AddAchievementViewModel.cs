using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

public class AddAchievementViewModel
{
    [Required(ErrorMessage = "Title is required")]
    public string Title { get; set; }

    public string Description { get; set; }

    [DataType(DataType.Date)]
    public DateTime AchievedOn { get; set; } = DateTime.Now;

    // File upload (ONLY in ViewModel)
    public IFormFile CertificateFile { get; set; }
}
