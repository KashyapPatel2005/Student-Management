public interface ILessonRepository
{
    Task<List<Lesson>> GetAllAsync();
    Task AddAsync(Lesson lesson);
    Task<int> CountAsync();
}
