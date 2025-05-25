using NHbibernateDemo.Core.Domains.DTOs.Requests;
using NHbibernateDemo.Core.Domains.DTOs.Responses;
using NHbibernateDemo.Core.Domains.Entities;

namespace NHbibernateDemo.Core.Domains.Mappings;

public static class MappingExtensions
{
    public static Student ToDomain(this StudentRequest request)
        => new Student(request.Name, request.Course, request.Gender);

    public static StudentResponse ToResponse(this Student student)
        => new StudentResponse
        {
            Id = student.Id,
            Name = student.Name,
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
                Course = student.Course,
                Gender = student.Gender
            });
    }
}