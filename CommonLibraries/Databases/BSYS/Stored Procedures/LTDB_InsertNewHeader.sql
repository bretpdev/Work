CREATE PROCEDURE [dbo].[LTDB_InsertNewHeader]
	@Header varchar(50),
	@User VARCHAR(50)

AS
	INSERT INTO LTDB_File_Headers(Header, CreatedBy)
	VALUES(@Header, @User)

	SELECT
		HeaderId,
		Header
	FROM
		LTDB_File_Headers
	WHERE
		HeaderId = SCOPE_IDENTITY()
RETURN 0
