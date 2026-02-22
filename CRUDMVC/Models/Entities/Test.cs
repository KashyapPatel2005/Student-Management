using System.ComponentModel.DataAnnotations;

public class Test
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Test title is required")]
    [StringLength(100)]
    public string Title { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.Now;

    public ICollection<Question> Questions { get; set; } = new List<Question>();
    public ICollection<TestAttempt> TestAttempts { get; set; } = new List<TestAttempt>();
}
