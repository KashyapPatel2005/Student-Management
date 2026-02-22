using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

public class TestAttempt
{
    public int Id { get; set; }

    [Required]
    public string UserId { get; set; }   // MUST BE STRING

    [Required]
    public int TestId { get; set; }

    public int Score { get; set; }

    public DateTime AttemptedAt { get; set; } = DateTime.Now;

    public IdentityUser User { get; set; }
    public Test Test { get; set; }
}
