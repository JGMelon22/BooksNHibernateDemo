using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernateDemo.Infrastructure.Mapping;
using NHibernate;
using NHibernate.Tool.hbm2ddl;

namespace NHibernateDemo.Infrastructure.Configuration;

public class NHibernateConfiguration
{
    private readonly string _connectionString;

    public NHibernateConfiguration(string connectionString)
    {
        _connectionString = connectionString;
    }

    public ISessionFactory BuildSessionFactory()
    {
        return Fluently.Configure()
            .Database(MsSqlConfiguration.MsSql2012
                .ConnectionString(_connectionString)
                .ShowSql())
            .Mappings(m => m.FluentMappings
                .AddFromAssemblyOf<StudentMap>())
            .ExposeConfiguration(cfg => new SchemaExport(cfg)
                .Create(false, false))
            .BuildSessionFactory();

    }
}