using NetDevPack.SimpleMediator;
using NHibernateDemo.Core.Domains.Entities;
using NHibernateDemo.Core.Shared;
using NHibernateDemo.Infrastructure.Interfaces.Repositories;
using ZiggyCreatures.Caching.Fusion;

namespace NHibernateDemo.Application.Commands.Handlers;

public class RemoveStudentCommandHandler : IRequestHandler<RemoveStudentCommand, Result<bool>>
{
    private IFusionCache _cache;
    private readonly IStudentRepository _repository;

    public RemoveStudentCommandHandler(IFusionCache cache, IStudentRepository repository)
    {
        _cache = cache;
        _repository = repository;
    }

    public async Task<Result<bool>> Handle(RemoveStudentCommand request, CancellationToken cancellationToken)
    {
        try
        {
            Student? student = await _repository.GetStudentAsync(request.Id);
            if (student is null)
                return Result<bool>.Failure($"Student with Id {request.Id} not found!");

            bool success = await _repository.RemoveStudentAsync(student.Id);
            await _cache.RemoveAsync($"student:{request.Id}");

            return Result<bool>.Success(success);
        }
        catch (Exception ex)
        {
            return Result<bool>.Failure($"An error occurred while trying to delete the student with Id {request.Id}" + ex.Message);
        }
    }
}