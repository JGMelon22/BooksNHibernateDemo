using NetDevPack.SimpleMediator;
using NHibernateDemo.Core.Shared;

namespace NHibernateDemo.Application.Commands;

public record RemoveStudentCommand(int Id) : IRequest<Result<bool>>;
