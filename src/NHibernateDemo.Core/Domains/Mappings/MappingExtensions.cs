using NHibernateDemo.Core.Domains.DTOs.Requests;
using NHibernateDemo.Core.Domains.DTOs.Responses;
using NHibernateDemo.Core.Domains.Entities;

namespace NHibernateDemo.Core.Domains.Mappings;

public static class MappingExtensions
{
    public static Student ToDomain(this StudentRequest request)
        => new Student(request.Name, request.Email, request.Course, request.Gender);

    public static StudentResponse ToResponse(this Student student)
        => new StudentResponse
        {
            Id = student.Id,
            Name = student.Name,
            Email = student.Email,
            Course = student.Course,
            Gender = student.Gender
        };

    public static IEnumerable<StudentResponse> ToResponse(this IEnumerable<Student> student)
    {
        return student.Select(student =>
            new StudentResponse
            {
                Id = student.Id,
                Name = student.Name,
                Email = student.Email,
                Course = student.Course,
                Gender = student.Gender
            });
    }
}