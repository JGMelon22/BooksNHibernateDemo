using NHibernateDemo.Infrastructure.Configuration;
using NHibernateDemo.Infrastructure.Interfaces.Repositories;
using NHibernateDemo.Infrastructure.Repositories;
using NHibernate;
using NetDevPack.SimpleMediator;
using NHibernateDemo.Application.Queries;
using NHibernateDemo.Core.Shared;
using NHibernateDemo.Core.Domains.DTOs.Responses;
using NHibernateDemo.Application.Queries.Handlers;
using NHibernateDemo.Application.Commands;
using NHibernateDemo.Application.Commands.Handlers;

namespace NHibernateDemo.API.Extensions
{
    public static class IocExtensions
    {
        public static IServiceCollection AddNHibernate(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<ISessionFactory>(provider =>
            {
                NHibernateConfiguration config = new NHibernateConfiguration(configuration.GetConnectionString("Default")!);
                return config.BuildSessionFactory();
            });

            return services;
        }

        public static IServiceCollection AddHandlers(this IServiceCollection services)
        {
            services.AddSingleton<IMediator, Mediator>();

            services.AddTransient<IRequestHandler<GetStudentByIdQuery, Result<StudentResponse>>, GetStudentByIdQueryHandler>();
            services.AddTransient<IRequestHandler<GetStudentsQuery, Result<IEnumerable<StudentResponse>>>, GetStudentsQueryHandler>();
            services.AddTransient<IRequestHandler<CreateStudentCommand, Result<bool>>, CreateStudentCommandHandler>();
            services.AddTransient<IRequestHandler<UpdateStudentCommand, Result<bool>>, UpdateStudentCommandHandler>();
            services.AddTransient<IRequestHandler<RemoveStudentCommand, Result<bool>>, RemoveStudentCommandHandler>();

            return services;
        }

        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IStudentRepository, StudentRepository>();

            return services;
        }
    }
}