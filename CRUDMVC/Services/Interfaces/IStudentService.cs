using CRUDMVC.Models;
using CRUDMVC.Models.Entities;

public interface IStudentService
{
    Task AddStudentAsync(AddStudentViewModel model);
    Task<List<Student>> GetStudentsAsync();
    Task<Student?> GetStudentAsync(Guid id);
    Task UpdateStudentAsync(Student student);
    Task DeleteStudentAsync(Guid id);
}
