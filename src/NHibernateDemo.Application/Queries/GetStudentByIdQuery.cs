using NetDevPack.SimpleMediator;
using NHibernateDemo.Core.Domains.DTOs.Responses;
using NHibernateDemo.Core.Shared;

namespace NHibernateDemo.Application.Queries;

public sealed record GetStudentByIdQuery(int Id) : IRequest<Result<StudentResponse>>;
