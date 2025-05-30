using Microsoft.Extensions.Logging;
using NHibernateDemo.Core.Domains.Entities;
using NHibernateDemo.Infrastructure.Interfaces.Repositories;
using NHibernate;
using NHibernate.Linq;

namespace NHibernateDemo.Infrastructure.Repositories;

public class StudentRepository : IStudentRepository
{
    private readonly ISessionFactory _sessionFactory;
    private readonly ILogger<StudentRepository> _logger;

    public StudentRepository(ISessionFactory sessionFactory, ILogger<StudentRepository> logger)
    {
        _sessionFactory = sessionFactory;
        _logger = logger;
    }

    public async Task<bool> AddStudentAsync(Student student)
    {
        _logger.LogInformation("Adding a new student: {Student}", student);

        using ISession session = _sessionFactory.OpenSession();
        using ITransaction transaction = session.BeginTransaction();

        try
        {
            const string sql = """
                INSERT INTO Students
                (
                    Name, Email,
                    Course, Gender
                )
                VALUES
                (
                    :name, :email,
                    :course, :gender
                );
                """;

            int result = await session.CreateSQLQuery(sql)
                                            .SetParameter("name", student.Name)
                                            .SetParameter("email", student.Email)
                                            .SetParameter("course", student.Course)
                                            .SetParameter("gender", student.Gender)
                                            .ExecuteUpdateAsync();

            await transaction.CommitAsync();

            if (result == 0)
            {
                _logger.LogWarning("No student was added.");
                return false;
            }

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to add student: {Student}", student);
            return false;
        }
    }

    public async Task<Student?> GetStudentAsync(int id)
    {
        _logger.LogInformation("Fetching student with ID: {StudentId}", id);

        using ISession session = _sessionFactory.OpenSession();

        try
        {
            const string sql = """
                SELECT * 
                FROM Students
                WHERE Id = :id; 
                """;

            Student student = await session.CreateSQLQuery(sql)
                                           .AddEntity(typeof(Student))
                                           .SetParameter("id", id)
                                           .UniqueResultAsync<Student>();

            if (student is not null)
                _logger.LogInformation("Student retrieved: {Student}", student);

            return student;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving student with ID: {StudentId}", id);
            return null;
        }
    }

    public async Task<IEnumerable<Student>> GetStudentsListAsync()
    {
        _logger.LogInformation("Fetching list of all students...");

        using ISession session = _sessionFactory.OpenSession();

        try
        {
            const string sql = """
                SELECT * 
                FROM Students;
                """;

            IEnumerable<Student> students = await session.CreateSQLQuery(sql)
                                           .AddEntity(typeof(Student))
                                           .ListAsync<Student>();

            _logger.LogInformation("Retrieved {Count} students.", students.Count());
            return students;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving student list.");
            return [];
        }
    }

    public async Task<bool> RemoveStudentAsync(int id)
    {
        _logger.LogInformation("Removing student with ID: {StudentId}", id);

        using ISession session = _sessionFactory.OpenSession();
        using ITransaction transaction = session.BeginTransaction();

        try
        {
            const string sql = """
                DELETE 
                FROM Students
                WHERE Id = :id; 
                """;

            int result = await session.CreateSQLQuery(sql) 
                                      .AddEntity(typeof(Student))
                                      .SetParameter("id", id)
                                      .ExecuteUpdateAsync();

            await transaction.CommitAsync();

            if(result == 0)
            {
                 _logger.LogWarning("No student was removed.");
                return false;
            }

            _logger.LogInformation("Student with ID {StudentId} removed successfully.", id);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error removing student with ID: {StudentId}", id);
            await transaction.RollbackAsync();
            return false;
        }
    }

    public async Task<bool> UpdateStudentAsync(int id, Student updatedStudent)
    {
        _logger.LogInformation("Updating student with ID: {StudentId}", id);

        using ISession session = _sessionFactory.OpenSession();
        using ITransaction transaction = session.BeginTransaction();

        try
        {
            const string sql = """
                UPDATE Students
                SET Name = :name,
                    Email = :email,
                    Course = :course,
                    Gender = :gender
                WHERE Id = :id; 
                """;

            int result = await session.CreateSQLQuery(sql)
                                      .AddEntity(typeof(Student))
                                      .SetParameter("name", updatedStudent.Name)
                                      .SetParameter("email", updatedStudent.Email)
                                      .SetParameter("course", updatedStudent.Course)
                                      .SetParameter("gender", updatedStudent.Gender)
                                      .SetParameter("id", id)
                                      .ExecuteUpdateAsync();

            await transaction.CommitAsync();

            if (result == 0)
            {
                _logger.LogWarning("No student was removed.");
                return false;
            }

            _logger.LogInformation("Student with ID {StudentId} updated successfully.", id);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating student with ID: {StudentId}", id);
            await transaction.RollbackAsync();
            return false;
        }
    }
}
