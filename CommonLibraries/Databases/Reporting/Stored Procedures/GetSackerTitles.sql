CREATE PROCEDURE [dbo].[GetSackerTitles]
	@System varchar(6)
AS
BEGIN
	IF (@System = 'script')
		BEGIN
			SELECT
				Title
			FROM
				BSYS.dbo.SCKR_DAT_ScriptRequests
		END
	ELSE
		BEGIN
			SELECT
				Title
			FROM
				BSYS.dbo.SCKR_DAT_SASRequests
		END
END

RETURN 0

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetSackerTitles] TO [db_executor]
    AS [dbo];