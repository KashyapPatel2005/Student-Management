public class LessonAnalyticsViewModel
{
    public int LessonId { get; set; }
    public string Title { get; set; }

    public int TotalUsers { get; set; }
    public int CompletedUsers { get; set; }

    public int Percentage =>
        TotalUsers == 0 ? 0 : (CompletedUsers * 100) / TotalUsers;
}
