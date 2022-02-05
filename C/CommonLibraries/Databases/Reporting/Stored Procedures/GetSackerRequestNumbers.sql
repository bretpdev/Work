CREATE PROCEDURE [dbo].[GetSackerRequestNumbers]
	@System varchar(6)
AS
BEGIN
	IF (@System = 'script')
		BEGIN
			SELECT
				Request
			FROM
				BSYS.dbo.SCKR_DAT_ScriptRequests
		END
	ELSE
		BEGIN
			SELECT
				Request
			FROM
				BSYS.dbo.SCKR_DAT_SASRequests
		END
END

RETURN 0

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetSackerRequestNumbers] TO [db_executor]
    AS [dbo];