CREATE PROCEDURE [dbo].[LTDB_UpdateSystemSproc]
	@Id int,
	@StoredProcedureName varchar(100),
	@ReturnType varchar(100),
	@Active bit
AS
	DECLARE @ReturnTypeId INT = (SELECT DISTINCT ReturnTypeId FROM LTDB_SystemLettersReturnType WHERE ReturnType = @ReturnType)

	UPDATE
		LTDB_SystemLettersStoredProcedures
	SET
		StoredProcedureName = @StoredProcedureName,
		ReturnTypeId = @ReturnTypeId,
		Active = @Active
	WHERE
		SystemLettersStoredProcedureId = @Id

RETURN 0
