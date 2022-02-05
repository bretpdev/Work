CREATE PROCEDURE [dbo].[PromoteScriptLetter]
	@Data ScriptLetterData READONLY,
	@Letter VARCHAR(10),
	@User VARCHAR(100)
AS BEGIN
	DECLARE @LetterId int = (SELECT DocDetailId FROM LTDB_DAT_DocDetail WHERE ID = @Letter)
BEGIN TRANSACTION

	DECLARE @ERROR INT

	INSERT INTO LTDB_File_Headers(Header, CreatedBy, CreatedOn)
		SELECT
			D.Header,
			@User,
			GETDATE()
		FROM
			@Data D
		LEFT JOIN LTDB_File_Headers FH
			ON FH.Header = D.Header
		WHERE
			FH.Header IS NULL

	SELECT @ERROR = @@ERROR

	INSERT INTO LTDB_LST_HeaderTypes(HeaderType, CreatedBy, CreatedOn)
		SELECT
			D.HeaderType,
			@User,
			GETDATE()
		FROM
			@Data D
		LEFT JOIN LTDB_LST_HeaderTypes HT
			ON HT.HeaderType = D.HeaderType
		WHERE
			HT.HeaderType IS NULL

	SELECT @ERROR = @ERROR + @@ERROR

	DELETE
		LTDB_Letter_Header_Mapping 
	WHERE
		LetterId = @LetterId

	INSERT INTO LTDB_Letter_Header_Mapping(LetterId, HeaderTypeId, HeaderId, [Order], CreatedBy, CreatedOn)
		SELECT
			@LetterId,
			HT.HeaderTypeId,
			FH.HeaderId,
			D.[Order],
			@User,
			GETDATE()
		FROM
			@Data D
		INNER JOIN LTDB_File_Headers FH
			ON FH.Header = D.Header
		INNER JOIN LTDB_LST_HeaderTypes HT
			ON HT.HeaderType = D.HeaderType

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
