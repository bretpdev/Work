CREATE PROCEDURE [complaints].[ComplaintTypesSelectAll]
AS

	select ComplaintTypeId, TypeName
	  from [complaints].ComplaintTypes
	 where DeletedOn is null

RETURN 0