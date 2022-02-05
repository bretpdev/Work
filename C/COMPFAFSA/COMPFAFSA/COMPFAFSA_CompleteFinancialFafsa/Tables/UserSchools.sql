CREATE TABLE [compfafsa].[UserSchools]
(
	[UserSchoolId] INT NOT NULL PRIMARY KEY IDENTITY,
	[UserId] INT NOT NULL,
	[SchoolId] INT NOT NULL
)
