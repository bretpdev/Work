CREATE PROCEDURE [dbo].[InsertProject]
	@Name nvarchar(max),
	@Notes nvarchar(max),
	@IsFederal bit
AS
	insert into Projects (Name, Notes, IsFederal)
	values (@Name, @Notes, @IsFederal)
SELECT SCOPE_IDENTITY()

grant execute on [dbo].[InsertProject] to [db_executor]