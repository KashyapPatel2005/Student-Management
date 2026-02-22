public class TestService : ITestService
{
    private readonly ITestRepository _repository;

    public TestService(ITestRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<Test>> GetAllAsync()
        => await _repository.GetAllAsync();

    public async Task<Test> GetByIdAsync(int id)
        => await _repository.GetByIdAsync(id);

    public async Task AddTestAsync(Test test)
        => await _repository.AddTestAsync(test);

    public async Task AddQuestionAsync(Question question)
        => await _repository.AddQuestionAsync(question);

    public async Task<int> SubmitTest(string userId, int testId, Dictionary<int, string> answers)
    {

        if (await _repository.HasUserAttempted(userId, testId))
            throw new Exception("You have already attempted this test.");

        var test = await _repository.GetByIdAsync(testId);

        if (test == null || test.Questions == null)
            throw new Exception("Invalid test.");

        int score = 0;

        foreach (var question in test.Questions)
        {
            if (answers.ContainsKey(question.Id))
            {
                var selectedAnswer = answers[question.Id];

                if (selectedAnswer == question.CorrectAnswer.ToString())
                {
                    score++;
                }
            }
        }

        var attempt = new TestAttempt
        {
            UserId = userId,
            TestId = testId,
            Score = score,
            AttemptedAt = DateTime.Now
        };

        await _repository.SaveAttemptAsync(attempt);

        return score;
    }

    public async Task<List<TestAttempt>> GetAllAttemptsAsync()
        => await _repository.GetAllAttemptsAsync();
}
