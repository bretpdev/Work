CREATE PROCEDURE [dbo].[LTDB_GetLetterIds]
	@Region varchar(11) = NULL
AS

IF @Region IS NULL
	SELECT
		DD.ID
	FROM
		[BSYS].[dbo].[LTDB_DAT_DocDetail] DD
		LEFT JOIN [BSYS].[dbo].LTDB_DAT_CentralPrintingDocData CP ON DD.ID = CP.ID
	WHERE
		DD.[Status] = 'Active'
		AND DD.DocName NOT LIKE '%FED%'
		AND DD.ID IS NOT NULL
	ORDER BY
		DD.ID
ELSE
	SELECT
		DD.ID
	FROM
		[BSYS].[dbo].[LTDB_DAT_DocDetail] DD
		JOIN [BSYS].[dbo].LTDB_DAT_CentralPrintingDocData CP ON DD.ID = CP.ID
	WHERE
		DD.[Status] = 'Active'
		AND ((DD.DocName LIKE '% FED%' AND DD.DocName NOT LIKE '%federal%') OR DD.DocName LIKE '%-FED%')
		AND DD.ID IS NOT NULL
	ORDER BY
		DD.ID

RETURN 0