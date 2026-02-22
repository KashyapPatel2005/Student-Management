public interface ITestService
{
    Task<List<Test>> GetAllAsync();
    Task<Test> GetByIdAsync(int id);

    Task AddTestAsync(Test test);
    Task AddQuestionAsync(Question question);

    Task<int> SubmitTest(string userId, int testId, Dictionary<int, string> answers);

    Task<List<TestAttempt>> GetAllAttemptsAsync();
}
