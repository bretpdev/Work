--SCRIPTS
SELECT DISTINCT
	ID,
	S.Script as [Name],
	sr.Request
FROM 
	[BSYS].[dbo].[SCKR_REF_Unit] U
	INNER JOIN SCKR_DAT_Scripts S
		ON S.Script = U.Program
	INNER JOIN SCKR_DAT_ScriptRequests SR
		ON SR.[Script] = S.[Script]
WHERE 
	U.Unit in ('LGP Accounting','Loan Management','Claims Review')
	and S.[Status] = 'Active'
	AND SR.CurrentStatus = 'Complete'
	AND SR.StatusDate between '7/01/2018' and '6/30/2019'
--SAS
SELECT DISTINCT 
	Program,
	SR.Request
FROM 
	[BSYS].[dbo].[SCKR_REF_UnitSAS] U
	INNER JOIN SCKR_DAT_SAS S
		ON S.Job = U.Program
	INNER JOIN SCKR_DAT_SASRequests SR
		ON SR.[Job] = S.Job
WHERE 
	U.Unit in ('LGP Accounting','Loan Management','Claims Review')
	AND S.[Status] = 'Active'
	AND SR.CurrentStatus = 'Complete'
	AND SR.StatusDate between '7/01/2018' and '6/30/2019'