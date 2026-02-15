public class ProgressService : IProgressService
{
    private readonly IProgressRepository _repository;

    public ProgressService(IProgressRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<int>> GetCompletedLessonIds(string userId)
        => await _repository.GetCompletedLessonIds(userId);

    public async Task MarkCompleted(string userId, int lessonId)
        => await _repository.MarkCompleted(userId, lessonId);

    public async Task<int> CountCompleted(string userId)
        => await _repository.CountCompleted(userId);
}
