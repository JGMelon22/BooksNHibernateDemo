namespace NHbibernateDemo.Core.Domains.Entities;

public class Student
{

    public virtual int Id { get; set; }
    public virtual string Name { get; set; } = string.Empty;
    public virtual string Course { get; set; } = string.Empty;
    public virtual string Gender { get; set; } = string.Empty;

    public Student()
    {
    }

    public Student(string name, string course, string gender)
    {
        Name = name;
        Course = course;
        Gender = gender;
    }

}