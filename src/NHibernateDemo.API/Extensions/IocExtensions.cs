using NHibernateDemo.Infrastructure.Configuration;
using NHibernateDemo.Infrastructure.Interfaces.Repositories;
using NHibernateDemo.Infrastructure.Repositories;
using NHibernate;

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

        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IStudentRepository, StudentRepository>();

            return services;
        }
    }
}