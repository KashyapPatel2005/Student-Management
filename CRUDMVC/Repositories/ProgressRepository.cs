using CRUDMVC.Data;
using Microsoft.EntityFrameworkCore;

public class ProgressRepository : IProgressRepository
{
    private readonly ApplicationDbContext _context;

    public ProgressRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<int>> GetCompletedLessonIds(string userId)
    {
        return await _context.UserLessonProgresses
            .Where(x => x.UserId == userId && x.IsCompleted)
            .Select(x => x.LessonId)
            .ToListAsync();
    }

    public async Task MarkCompleted(string userId, int lessonId)
    {
        var existing = await _context.UserLessonProgresses
            .FirstOrDefaultAsync(x => x.UserId == userId && x.LessonId == lessonId);

        if (existing == null)
        {
            var progress = new UserLessonProgress
            {
                UserId = userId,
                LessonId = lessonId,
                IsCompleted = true,
                CompletedAt = DateTime.Now
            };

            _context.UserLessonProgresses.Add(progress);
        }
        else
        {
            existing.IsCompleted = true;
            existing.CompletedAt = DateTime.Now;
        }

        await _context.SaveChangesAsync();
    }

    public async Task<int> CountCompleted(string userId)
    {
        return await _context.UserLessonProgresses
            .CountAsync(x => x.UserId == userId && x.IsCompleted);
    }
}
