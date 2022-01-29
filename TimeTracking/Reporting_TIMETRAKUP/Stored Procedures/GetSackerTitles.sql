CREATE PROCEDURE [dbo].[GetSackerTitles]
	@System varchar(6)
AS
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