CREATE PROCEDURE [bcsretmail].[GetLetterIds]
AS
	SELECT
		ID
	FROM
		[BSYS].[dbo].[LTDB_DAT_DocDetail]
	WHERE
		[Status] = 'Active'
		AND ID IS NOT NULL
	ORDER BY
		ID