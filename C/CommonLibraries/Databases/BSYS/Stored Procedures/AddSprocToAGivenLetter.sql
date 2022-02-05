CREATE PROCEDURE [dbo].[AddSprocToAGivenLetter]
	@DocDetailId int,
	@SprocName varchar(250),
	@ReturnType varchar(50)	
AS
	DECLARE @ReturnTypeId INT = (SELECT DISTINCT ReturnTypeId FROM [dbo].[LTDB_SystemLettersReturnType] WHERE ReturnType = @ReturnType)

	INSERT INTO [dbo].[LTDB_SystemLettersStoredProcedures] (LetterId, StoredProcedureName, ReturnTypeId)
	VALUES(@DocDetailId, @SprocName, @ReturnTypeId)
RETURN 0
