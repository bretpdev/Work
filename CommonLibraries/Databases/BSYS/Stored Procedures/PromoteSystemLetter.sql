CREATE PROCEDURE [dbo].[PromoteSystemLetter]
	@Data SystemLetterData READONLY,
	@Letter VARCHAR(10),
	@User VARCHAR(100)
AS BEGIN
	DECLARE @LetterId int = (SELECT DocDetailId FROM LTDB_DAT_DocDetail WHERE ID = @Letter)
BEGIN TRANSACTION

	DECLARE @ERROR INT

	INSERT INTO LTDB_SystemLettersReturnType(ReturnType)
		SELECT
			D.ReturnType
		FROM
			@Data D
		LEFT JOIN LTDB_SystemLettersReturnType SLRT
			ON SLRT.ReturnType = D.ReturnType
		WHERE
			SLRT.ReturnType IS NULL

	SELECT @ERROR = @@ERROR

	DELETE
		LTDB_SystemLettersStoredProcedures 
	WHERE
		LetterId = @LetterId

	INSERT INTO LTDB_SystemLettersStoredProcedures(LetterId, StoredProcedureName, ReturnTypeId)
		SELECT
			@LetterId,
			D.StoredProcedureName,
			SLRT.ReturnTypeId
		FROM
			@Data D
		INNER JOIN LTDB_SystemLettersReturnType SLRT
			ON SLRT.ReturnType = D.ReturnType

	SELECT @ERROR = @ERROR + @@ERROR

	IF @ERROR = 0
		BEGIN
			COMMIT TRANSACTION
		END
	ELSE
		BEGIN
			RAISERROR ('Failed to retrieve dbo.DocumentDetails record for processing.', -- error message
				   16, -- Severity.
				   1 -- State.
				   );
			ROLLBACK TRANSACTION
		END
END
