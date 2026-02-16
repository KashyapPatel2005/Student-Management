public interface ILessonService
{
    Task<List<Lesson>> GetAllAsync();
    Task AddAsync(Lesson lesson);
    Task<int> CountAsync();
    Task<List<LessonAnalyticsViewModel>> GetLessonAnalyticsAsync();

}
