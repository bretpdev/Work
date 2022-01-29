CREATE PROCEDURE [dbo].[GetLetterRequestNumbers]
AS
	SELECT
		Request
	FROM
		BSYS.dbo.LTDB_DAT_Requests