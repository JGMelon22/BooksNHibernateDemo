using NetDevPack.SimpleMediator;
using NHibernateDemo.Core.Domains.Entities;
using NHibernateDemo.Core.Domains.Mappings;
using NHibernateDemo.Core.Shared;
using NHibernateDemo.Infrastructure.Interfaces.Repositories;
using ZiggyCreatures.Caching.Fusion;

namespace NHibernateDemo.Application.Commands.Handlers;

public class UpdateStudentCommandHandler : IRequestHandler<UpdateStudentCommand, Result<bool>>
{
    private readonly IFusionCache _cache;
    private readonly IStudentRepository _repository;

    public UpdateStudentCommandHandler(IFusionCache cache, IStudentRepository repository)
    {
        _cache = cache;
        _repository = repository;
    }

    public async Task<Result<bool>> Handle(UpdateStudentCommand request, CancellationToken cancellationToken)
    {
        try
        {
            Student? student = await _repository.GetStudentAsync(request.Id);
            if (student is null)
                return Result<bool>.Failure($"Student with Id {request.Id} not found!");

            Student updatedStudent = request.Student.ToDomain();

            bool success = await _repository.UpdateStudentAsync(request.Id, updatedStudent);
            await _cache.SetAsync(
                $"student:{request.Id}",
                updatedStudent, options => options.SetDuration(TimeSpan.FromMinutes(1))
            );

            return Result<bool>.Success(success);
        }
        catch (Exception ex)
        {
            return Result<bool>.Failure($"An error occurred while trying to update the student with Id {request.Id}" + ex.Message);
        }
    }
}