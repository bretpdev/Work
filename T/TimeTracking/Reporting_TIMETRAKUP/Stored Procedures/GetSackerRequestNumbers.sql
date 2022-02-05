CREATE PROCEDURE [dbo].[GetSackerRequestNumbers]
	@System varchar(6)
AS
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