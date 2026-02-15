using CRUDMVC.Models.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

public interface IAchievementRepository
{
    Task<List<Achievement>> GetByUserIdAsync(string userId);
    Task AddAsync(Achievement achievement);
    Task<Achievement> GetByIdAsync(int id);

}
