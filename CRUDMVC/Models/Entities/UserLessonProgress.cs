public class UserLessonProgress
{
    public int Id { get; set; }

    public string UserId { get; set; }

    public int LessonId { get; set; }

    public bool IsCompleted { get; set; }

    public DateTime? CompletedAt { get; set; }
}
