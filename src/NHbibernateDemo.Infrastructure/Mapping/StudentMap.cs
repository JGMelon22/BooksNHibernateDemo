using FluentNHibernate.Mapping;
using NHbibernateDemo.Core.Domains.Entities;

namespace NHbibernateDemo.Infrastructure.Mapping;

public class StudentMap : ClassMap<Student>
{
    public StudentMap()
    {
        Id(x => x.Id)
            .Column("id")
            .Index("idx_student_id")
            .GeneratedBy.Identity();

        Map(x => x.Name).Not.Nullable()
            .Column("name")
            .Length(100)
            .Not.Nullable();

        Map(x => x.Course).Not.Nullable()
            .Column("course")
            .Length(100)
            .Not.Nullable();

        Map(x => x.Gender).Not.Nullable()
                   .Column("gender")
                   .Length(6)
                   .Not.Nullable();
    }
}