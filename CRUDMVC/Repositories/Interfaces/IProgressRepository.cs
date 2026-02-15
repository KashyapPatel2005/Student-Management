public interface IProgressRepository
{
    Task<List<int>> GetCompletedLessonIds(string userId);
    Task MarkCompleted(string userId, int lessonId);
    Task<int> CountCompleted(string userId);
}
