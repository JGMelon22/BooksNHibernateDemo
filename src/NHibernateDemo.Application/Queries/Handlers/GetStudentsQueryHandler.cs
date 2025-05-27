using NetDevPack.SimpleMediator;
using NHibernateDemo.Core.Domains.Mappings;
using NHibernateDemo.Core.Domains.DTOs.Responses;
using NHibernateDemo.Core.Domains.Entities;
using NHibernateDemo.Core.Shared;
using NHibernateDemo.Infrastructure.Interfaces.Repositories;

namespace NHibernateDemo.Application.Queries.Handlers;

public class GetStudentsQueryHandler : IRequestHandler<GetStudentsQuery, Result<IEnumerable<StudentResponse>>>
{
    private readonly IStudentRepository _repository;

    public GetStudentsQueryHandler(IStudentRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<IEnumerable<StudentResponse>>> Handle(GetStudentsQuery request, CancellationToken cancellationToken)
    {
        try
        {
            IEnumerable<Student> students = await _repository.GetStudentsListAsync();
            IEnumerable<StudentResponse> responses = students.ToResponse();

            return Result<IEnumerable<StudentResponse>>.Success(responses);
        }
        catch (Exception ex)
        {
            return Result<IEnumerable<StudentResponse>>.Failure("An error occurred while fetching students: " + ex.Message);
        }
    }

}