public interface ITestRepository
{
    Task<List<Test>> GetAllAsync();
    Task<Test> GetByIdAsync(int id);

    Task AddTestAsync(Test test);
    Task AddQuestionAsync(Question question);

    Task<bool> HasUserAttempted(string userId, int testId);

    Task SaveAttemptAsync(TestAttempt attempt);

    Task<List<TestAttempt>> GetAllAttemptsAsync();
}
