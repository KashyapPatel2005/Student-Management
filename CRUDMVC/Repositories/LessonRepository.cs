using CRUDMVC.Data;
using Microsoft.EntityFrameworkCore;

public class LessonRepository : ILessonRepository
{
    private readonly ApplicationDbContext _context;

    public LessonRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<Lesson>> GetAllAsync()
    {
        return await _context.Lessons
            .OrderByDescending(x => x.CreatedAt)
            .ToListAsync();
    }

    public async Task AddAsync(Lesson lesson)
    {
        _context.Lessons.Add(lesson);
        await _context.SaveChangesAsync();
    }

    public async Task<int> CountAsync()
    {
        return await _context.Lessons.CountAsync();
    }


    public async Task<List<LessonAnalyticsViewModel>> GetLessonAnalyticsAsync()
    {
        var totalUsers = await _context.Users.CountAsync();

        var lessons = await _context.Lessons.ToListAsync();

        var result = new List<LessonAnalyticsViewModel>();

        foreach (var lesson in lessons)
        {
            var completedCount = await _context.UserLessonProgresses
                .CountAsync(x => x.LessonId == lesson.Id && x.IsCompleted);

            result.Add(new LessonAnalyticsViewModel
            {
                LessonId = lesson.Id,
                Title = lesson.Title,
                TotalUsers = totalUsers,
                CompletedUsers = completedCount
            });
        }

        return result;
    }

}
