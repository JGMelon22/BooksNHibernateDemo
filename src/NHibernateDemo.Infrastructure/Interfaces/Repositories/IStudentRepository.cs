using NHibernateDemo.Core.Domains.Entities;

namespace NHibernateDemo.Infrastructure.Interfaces.Repositories;

public interface IStudentRepository
{
    Task<Student?> GetStudentAsync(int id);
    Task<IEnumerable<Student>> GetStudentsListAsync();
    Task<bool> AddStudentAsync(Student student);
    Task<bool> UpdateStudentAsync(int id, Student updatedStudent);
    Task<bool> RemoveStudentAsync(int id);
}