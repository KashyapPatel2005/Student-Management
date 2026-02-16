public class LessonService : ILessonService
{
    private readonly ILessonRepository _repository;

    public LessonService(ILessonRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<Lesson>> GetAllAsync()
        => await _repository.GetAllAsync();

    public async Task AddAsync(Lesson lesson)
        => await _repository.AddAsync(lesson);

    public async Task<int> CountAsync()
        => await _repository.CountAsync();

    public async Task<List<LessonAnalyticsViewModel>> GetLessonAnalyticsAsync()
    => await _repository.GetLessonAnalyticsAsync();

}
