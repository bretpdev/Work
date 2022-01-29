
CREATE PROCEDURE dbo.LTDB_GetResendMail
	@LetterId varchar(10)
AS
BEGIN

	SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED

	SELECT
		ResendMail
	FROM
		LTDB_DAT_CentralPrintingDocData
	WHERE
		ID = @LetterId

END