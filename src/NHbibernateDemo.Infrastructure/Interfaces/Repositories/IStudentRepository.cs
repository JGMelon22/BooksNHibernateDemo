using NHbibernateDemo.Core.Domains.Entities;
using NHbibernateDemo.Core.Shared;

namespace NHbibernateDemo.Infrastructure.Interfaces.Repositories;

public interface IStudentRepository
{
    Task<Student?> GetStudentAsync (int id);
    Task<IEnumerable<Student>> GetStudentsListAsync();
    Task<bool> AddStudentAsync(Student student);
    Task<bool> UpdateStudentAsync(int id, Student updatedStudent);
    Task<bool> RemoveStudentAsync(int id);
}