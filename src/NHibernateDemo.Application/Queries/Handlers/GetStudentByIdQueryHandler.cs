using NetDevPack.SimpleMediator;
using NHibernateDemo.Core.Domains.DTOs.Responses;
using NHibernateDemo.Core.Domains.Entities;
using NHibernateDemo.Core.Domains.Mappings;
using NHibernateDemo.Core.Shared;
using NHibernateDemo.Infrastructure.Interfaces.Repositories;
using ZiggyCreatures.Caching.Fusion;

namespace NHibernateDemo.Application.Queries.Handlers;

public class GetStudentByIdQueryHandler : IRequestHandler<GetStudentByIdQuery, Result<StudentResponse>>
{
    private IFusionCache _cache;
    private readonly IStudentRepository _repository;

    public GetStudentByIdQueryHandler(IFusionCache cache, IStudentRepository repository)
    {
        _cache = cache;
        _repository = repository;
    }

    public async Task<Result<StudentResponse>> Handle(GetStudentByIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            Student? student = await _cache.GetOrSetAsync(
                $"student:{request.Id}",
                _ => _repository.GetStudentAsync(request.Id)
                );

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