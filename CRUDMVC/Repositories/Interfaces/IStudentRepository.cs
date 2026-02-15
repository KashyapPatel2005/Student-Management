using CRUDMVC.Models.Entities;

namespace CRUDMVC.Repositories.Interfaces
{
    public interface IStudentRepository
    {
        Task AddAsync(Student student);
        Task<List<Student>> GetAllAsync();
        Task<Student?> GetByIdAsync(Guid id);
        Task UpdateAsync(Student student);
        Task DeleteAsync(Guid id);
    }
}
