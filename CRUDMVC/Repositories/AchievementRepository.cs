using CRUDMVC.Data;
using CRUDMVC.Models.Entities;
using Microsoft.EntityFrameworkCore;

public class AchievementRepository : IAchievementRepository
{

    private readonly ApplicationDbContext _context;

    public AchievementRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<Achievement>> GetByUserIdAsync(string userId)
    {
        return await _context.Achievements
            .Where(a => a.UserId == userId)
            .OrderByDescending(a => a.AchievedOn)
            .ToListAsync();
    }

    public async Task AddAsync(Achievement achievement)
    {
        _context.Achievements.Add(achievement);
        await _context.SaveChangesAsync();
        Console.WriteLine("Saved achievement with title: " + achievement.Title);
    }

    public async Task<Achievement> GetByIdAsync(int id)
    {
        return await _context.Achievements.FirstOrDefaultAsync(a => a.Id == id);
    }
}
