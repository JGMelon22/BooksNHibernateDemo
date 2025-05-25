using NHbibernateDemo.Core.Domains.Entities;
using NHbibernateDemo.Infrastructure.Interfaces.Repositories;
using NHibernate;
using NHibernate.Linq;

namespace NHbibernateDemo.Infrastructure.Repositories;

public class StudentRepository : IStudentRepository
{
    private readonly ISessionFactory _sessionFactory;

    public StudentRepository(ISessionFactory sessionFactory)
    {
        _sessionFactory = sessionFactory;
    }

    public async Task<bool> AddStudentAsync(Student student)
    {
        using ISession session = _sessionFactory.OpenSession();
        using ITransaction transaction = session.BeginTransaction();

        try
        {
            await session.SaveAsync(student);
            await transaction.CommitAsync();

            return true;
        }
        catch (Exception ex)
        {
            return false;
        }
    }

    public async Task<Student?> GetStudentAsync(int id)
    {
        using ISession session = _sessionFactory.OpenSession();

        try
        {
            return await session.GetAsync<Student>(id);
        }
        catch (Exception ex)
        {
            return null;
        }
    }

    public async Task<IEnumerable<Student>> GetStudentsListAsync()
    {
        using ISession session = _sessionFactory.OpenSession();

        try
        {
            return await session.Query<Student>().ToListAsync();
        }
        catch (Exception ex)
        {
            return [];
        }
    }

    public async Task<bool> RemoveStudentAsync(int id)
    {
        using ISession session = _sessionFactory.OpenSession();
        using ITransaction transaction = session.BeginTransaction();

        try
        {
            Student student = await session.GetAsync<Student>(id);
            if (student is null)
                return false;

            await session.DeleteAsync(student);
            await transaction.CommitAsync();

            return true;
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            return false;
        }
    }

    public async Task<bool> UpdateStudentAsync(int id, Student updatedStudent)
    {
        using ISession session = _sessionFactory.OpenSession();
        using ITransaction transaction = session.BeginTransaction();
        try
        {
            Student student = await session.GetAsync<Student>(id);

            if (student is null)
                return false;

            student.Name = updatedStudent.Name;
            student.Course = updatedStudent.Course;
            student.Gender = updatedStudent.Gender;

            await session.UpdateAsync(student);
            await transaction.CommitAsync();

            return true;
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            return false;
        }
    }
}