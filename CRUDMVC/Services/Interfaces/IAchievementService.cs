using CRUDMVC.Models.Entities;

public interface IAchievementService
{
    Task<List<Achievement>> GetUserAchievementsAsync(string userId);
    Task AddAchievementAsync(AddAchievementViewModel model, string userId);
    Task<Achievement> GetByIdAsync(int id);
}
