CREATE IF NOT EXISTS NHibernateStudents;
GO

USE NHibernateStudents;
GO

CREATE TABLE [dbo].[Students](
   [Id] [int] IDENTITY(1,1) NOT NULL,
   [Name] [nvarchar](50) NOT NULL,
   [Email] [nvarchar](100) NOT NULL,
   [Course] [nvarchar](50) NULL,
   [Gender] [nvarchar](50) NULL,
   CONSTRAINT [PK_Student] PRIMARY KEY (Id)
)
GO

CREATE INDEX [idx_student_id]
	ON [dbo].[Students](id);
GO