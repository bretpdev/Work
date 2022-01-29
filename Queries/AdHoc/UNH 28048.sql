/****** Script for SelectTopNRows command from SSMS  ******/
SELECT DISTINCT 
	AC.SSN
FROM 
	[UDW_Temp].[dbo].[ach compiled] AC
	LEFT OUTER JOIN OPENQUERY
	(
	DUSTER, 
	'
		SELECT 
			* 
		FROM 
			OLWHRM1.BR30_BR_EFT BR30
		WHERE 
			BR30.BC_EFT_STA = ''A''
	') OQ 
		ON OQ.BF_SSN = AC.SSN
WHERE 
	OQ.BF_SSN IS NULL
