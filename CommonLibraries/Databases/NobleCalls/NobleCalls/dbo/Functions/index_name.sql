

CREATE FUNCTION [dbo].[index_name] (@object_id int, @index_id int) 
RETURNS sysname 
AS 
BEGIN 
DECLARE @index_name sysname 
SELECT @index_name = name FROM sys.indexes 
WHERE object_id = @object_id and index_id = @index_id 
RETURN(@index_name) 
END;