CREATE PROCEDURE [dbo].[LTDB_InsertNewHeaderType]
	@HeaderType varchar(50),
	@User VARCHAR(50)
AS
	INSERT INTO LTDB_LST_HeaderTypes(HeaderType, CreatedBy)
	VALUES(@HeaderType, @User)

	SELECT
		HeaderTypeId,
		HeaderType
	FROM
		LTDB_LST_HeaderTypes
	WHERE
		HeaderTypeId = SCOPE_IDENTITY()
RETURN 0
