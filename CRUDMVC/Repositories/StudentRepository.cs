using CRUDMVC.Data;
using CRUDMVC.Models.Entities;
using CRUDMVC.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

public class StudentRepository : IStudentRepository
{
    private readonly ApplicationDbContext _context;

    public StudentRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Student student)
    {
        await _context.students.AddAsync(student);
        await _context.SaveChangesAsync();
    }

    public async Task<List<Student>> GetAllAsync()
    {
        return await _context.students.ToListAsync();
    }

    public async Task<Student?> GetByIdAsync(Guid id)
    {
        return await _context.students.FindAsync(id);
    }

    public async Task UpdateAsync(Student student)
    {
        _context.students.Update(student);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var student = await _context.students.FindAsync(id);
        if (student != null)
        {
            _context.students.Remove(student);
            await _context.SaveChangesAsync();
        }
    }
}
