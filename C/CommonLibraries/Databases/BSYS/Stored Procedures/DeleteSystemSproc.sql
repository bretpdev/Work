CREATE PROCEDURE [dbo].[DeleteSystemSproc]
	@Id int 
	
AS
	DELETE FROM 
		LTDB_SystemLettersStoredProcedures 
	WHERE 
		SystemLettersStoredProcedureId = @Id
RETURN 0
