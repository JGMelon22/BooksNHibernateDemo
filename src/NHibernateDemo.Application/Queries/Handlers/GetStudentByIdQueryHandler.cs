using NetDevPack.SimpleMediator;
using NHibernateDemo.Core.Domains.DTOs.Responses;
using NHibernateDemo.Core.Domains.Entities;
using NHibernateDemo.Core.Domains.Mappings;
using NHibernateDemo.Core.Shared;
using NHibernateDemo.Infrastructure.Interfaces.Repositories;

namespace NHibernateDemo.Application.Queries.Handlers;

public class GetStudentByIdQueryHandler : IRequestHandler<GetStudentByIdQuery, Result<StudentResponse>>
{
    private readonly IStudentRepository _repository;

    public GetStudentByIdQueryHandler(IStudentRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<StudentResponse>> Handle(GetStudentByIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            Student? student = await _repository.GetStudentAsync(request.Id);

            if (student is null)
                return Result<StudentResponse>.Failure($"Student with Id {request.Id} not found!");

            StudentResponse response = student.ToResponse();

            return Result<StudentResponse>.Success(response);
        }
        catch (Exception ex)
        {
            return Result<StudentResponse>.Failure($"An error occurred while fetching student with Id {request.Id}: " + ex.Message);
        }
    }
}