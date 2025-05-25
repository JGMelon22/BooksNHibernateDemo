using NHbibernateDemo.Infrastructure.Configuration;
using NHbibernateDemo.Infrastructure.Interfaces.Repositories;
using NHbibernateDemo.Infrastructure.Repositories;
using NHibernate;

namespace NHbibernateDemo.API.Extensions
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