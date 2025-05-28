using NetDevPack.SimpleMediator;
using NHibernateDemo.Core.Domains.Entities;
using NHibernateDemo.Core.Domains.Mappings;
using NHibernateDemo.Core.Shared;
using NHibernateDemo.Infrastructure.Interfaces.Repositories;

namespace NHibernateDemo.Application.Commands.Handlers;

public class CreateStudentCommandHandler : IRequestHandler<CreateStudentCommand, Result<bool>>
{
    private readonly IStudentRepository _repository;

    public CreateStudentCommandHandler(IStudentRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<bool>> Handle(CreateStudentCommand request, CancellationToken cancellationToken)
    {
        try
        {
            Student student = request.Student.ToDomain();
            bool success = await _repository.AddStudentAsync(student);

            return Result<bool>.Success(success);

        }
        catch (Exception ex)
        {
            return Result<bool>.Failure("An error occurred while trying to insert a new student" + ex.Message);
        }
    }
}