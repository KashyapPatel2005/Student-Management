using CRUDMVC.Models;
using CRUDMVC.Models.Entities;
using CRUDMVC.Repositories.Interfaces;

public class StudentService : IStudentService
{
    private readonly IStudentRepository _repository;

    public StudentService(IStudentRepository repository)
    {
        _repository = repository;
    }

    public async Task AddStudentAsync(AddStudentViewModel model)
    {
        var student = new Student
        {
            Name = model.Name,
            Email = model.Email,
            Phone = model.Phone
        };

        await _repository.AddAsync(student);
    }

    public async Task<List<Student>> GetStudentsAsync()
    {
        return await _repository.GetAllAsync();
    }

    public async Task<Student?> GetStudentAsync(Guid id)
    {
        return await _repository.GetByIdAsync(id);
    }

    public async Task UpdateStudentAsync(Student student)
    {
        await _repository.UpdateAsync(student);
    }

    public async Task DeleteStudentAsync(Guid id)
    {
        await _repository.DeleteAsync(id);
    }
}
