/****** Script for SelectTopNRows command from SSMS  ******/
SELECT COUNT(DISTINCT BF_SSN)
  FROM [AuditCDW].[calc].[RepaymentSchedules_NovXXXX]
  where
	LC_TYP_SCH_DIS  IN 
	(
		'IB',
		'IL',
		'IX',
		'IP',
		'CX',
		'CX',
		'CX',
		'CQ',
		'CA',
		'CP',
		'IA',
		'IX',
		'RE'
)
and CurrentGradation = X

/****** Script for SelectTopNRows command from SSMS  ******/
SELECT COUNT(DISTINCT BF_SSN)
  FROM [AuditCDW].[calc].[RepaymentSchedules_OctXXXX]
  where
	LC_TYP_SCH_DIS  IN 
	(
		'IB',
		'IL',
		'IX',
		'IP',
		'CX',
		'CX',
		'CX',
		'CQ',
		'CA',
		'CP',
		'IA',
		'IX',
		'RE'
)
and CurrentGradation = X