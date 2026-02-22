using CRUDMVC.Data;
using Microsoft.EntityFrameworkCore;

public class TestRepository : ITestRepository
{
    private readonly ApplicationDbContext _context;

    public TestRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<Test>> GetAllAsync()
        => await _context.Tests.ToListAsync();

    public async Task<Test> GetByIdAsync(int id)
        => await _context.Tests
            .Include(x => x.Questions)
            .FirstOrDefaultAsync(x => x.Id == id);

    public async Task AddTestAsync(Test test)
    {
        _context.Tests.Add(test);
        await _context.SaveChangesAsync();
    }

    public async Task AddQuestionAsync(Question question)
    {
        _context.Questions.Add(question);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> HasUserAttempted(string userId, int testId)
    {
        return await _context.TestAttempts
            .AnyAsync(x => x.UserId == userId && x.TestId == testId);
    }

    public async Task SaveAttemptAsync(TestAttempt attempt)
    {
        _context.TestAttempts.Add(attempt);
        await _context.SaveChangesAsync();
    }

    public async Task<List<TestAttempt>> GetAllAttemptsAsync()
    {
        return await _context.TestAttempts
            .Include(x => x.Test)
            .ToListAsync();
    }
}
