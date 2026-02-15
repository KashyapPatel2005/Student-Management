using CRUDMVC.Models.Entities;


public class AchievementService : IAchievementService
{
    private readonly IAchievementRepository _repository;
    private readonly IWebHostEnvironment _env;

    public AchievementService(
        IAchievementRepository repository,
        IWebHostEnvironment env)
    {
        _repository = repository;
        _env = env;
    }

    public async Task<List<Achievement>> GetUserAchievementsAsync(string userId)
    {
        return await _repository.GetByUserIdAsync(userId);
    }

    public async Task AddAchievementAsync(AddAchievementViewModel model, string userId)
    {
        var achievement = new Achievement
        {
            UserId = userId,
            Title = model.Title,
            Description = model.Description,
            AchievedOn = model.AchievedOn
        };

        if (model.CertificateFile != null && model.CertificateFile.Length > 0)
        {
            string folder = Path.Combine(_env.WebRootPath, "certificates");
            Directory.CreateDirectory(folder);

            string uniqueName = Guid.NewGuid() + "_" + model.CertificateFile.FileName;
            string path = Path.Combine(folder, uniqueName);

            using var stream = new FileStream(path, FileMode.Create);
            await model.CertificateFile.CopyToAsync(stream);

            achievement.CertificateFileName = model.CertificateFile.FileName;
            achievement.CertificateFilePath = "/certificates/" + uniqueName;
        }

        await _repository.AddAsync(achievement);
    }

    public async Task<Achievement> GetByIdAsync(int id)
    {
        return await _repository.GetByIdAsync(id);
    }
}
