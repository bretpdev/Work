create procedure testprocedure
as
BEGIN
	select top 1 * from sys.tables
END