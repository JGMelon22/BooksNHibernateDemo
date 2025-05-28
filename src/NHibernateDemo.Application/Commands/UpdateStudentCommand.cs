using NetDevPack.SimpleMediator;
using NHibernateDemo.Core.Domains.DTOs.Requests;
using NHibernateDemo.Core.Shared;

namespace NHibernateDemo.Application.Commands;

public record UpdateStudentCommand(int Id, StudentRequest Student) : IRequest<Result<bool>>;
