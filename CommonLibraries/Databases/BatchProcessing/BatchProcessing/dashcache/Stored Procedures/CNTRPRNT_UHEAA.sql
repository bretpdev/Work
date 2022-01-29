CREATE PROCEDURE [dashcache].[CNTRPRNT_UHEAA]
AS

	SELECT
		COUNT(*)
	FROM
		[BSYS].[dbo].[PRNT_DAT_Print] PDP
	WHERE
		DeletedAt IS NULL
		AND
		PDP.RequestedDate <= master.dbo.AddBusinessDays(GETDATE(), -2)
		AND
		(	
			(
				IsOnEcorr = 1
				AND
				EcorrDocumentCreatedAt is NULL
			)
			OR
			(
				PDP.PrintedAt IS NULL
				AND
				IsOnEcorr = 0
			)
		)	

RETURN 0