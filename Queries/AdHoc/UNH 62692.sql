--ALL SCRIPTS

SELECT DISTINCT
	ID,
	S.Script as [Name],
	ScriptType
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
	
-- BATCH SCRIPTS
SELECT DISTINCT
	ID,
	S.Script as [Name],
	ScriptType
FROM 
	[BSYS].[dbo].[SCKR_REF_Unit] U
	INNER JOIN SCKR_DAT_Scripts S
		ON S.Script = U.Program
	INNER JOIN SCKR_DAT_ScriptRequests SR
		ON SR.[Script] = S.[Script]
WHERE 
	U.Unit in ('LGP Accounting','Loan Management','Claims Review')
	and S.[Status] = 'Active'
	and s.ScriptType = 'Batch Script'
	
--ALL SAS
SELECT DISTINCT 
	Program, 
	S.[Type]
FROM 
	[BSYS].[dbo].[SCKR_REF_UnitSAS] U
	INNER JOIN SCKR_DAT_SAS S
		ON S.Job = U.Program
	INNER JOIN SCKR_DAT_SASRequests SR
		ON SR.[Job] = S.Job
WHERE 
	U.Unit in ('LGP Accounting','Loan Management','Claims Review')
	and S.[Status] = 'Active'

--SCHEDULED SAS
SELECT DISTINCT 
	Program, 
	S.[Type]
FROM 
	[BSYS].[dbo].[SCKR_REF_UnitSAS] U
	INNER JOIN SCKR_DAT_SAS S
		ON S.Job = U.Program
	INNER JOIN SCKR_DAT_SASRequests SR
		ON SR.[Job] = S.Job
WHERE 
	U.Unit in ('LGP Accounting','Loan Management','Claims Review')
	and S.[Status] = 'Active'
	AND S.[Type] = 'Scheduled'