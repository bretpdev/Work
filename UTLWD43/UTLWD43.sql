USE ULS
GO

DECLARE @TODAY DATETIME = GETDATE();

INSERT INTO OLS.olqtskbldr.Queues(TargetId, QueueName, InstitutionId, InstitutionType, DateDue, TimeDue, Comment, SourceFilename, AddedAt, AddedBy)
SELECT DISTINCT
	DOD.SSN,
	'DACTIVED',
	'',
	'',
	NULL,
	NULL,
	CASE WHEN DOD.ActiveDutyOnActiveDutyStatusDt = 'X' THEN 'Borrower SCRA Review. Active Duty Begin Date = ' + CAST(CASE WHEN DOD.ActiveDutyBeginDate = '0' THEN '19201231' ELSE DOD.ActiveDutyBeginDate END AS VARCHAR(8)) + ' End Date = ' + CAST(CASE WHEN DOD.ActiveDutyEndDate = '0' THEN '20991231' ELSE DOD.ActiveDutyEndDate END AS VARCHAR(8))
	     WHEN DOD.NotifiedOfActiveDutyRecallOnActiveDutyStatusDt = 'Y' THEN 'Borrower SCRA Review. Active Duty Begin Date = ' + CAST(CASE WHEN DOD.EIDBeginDate = '0' THEN '19201231' ELSE DOD.EIDBeginDate END AS VARCHAR(8)) + ' End Date = ' + CAST(CASE WHEN DOD.ActiveDutyEndDate = '0' THEN '20991231' ELSE DOD.ActiveDutyEndDate END AS VARCHAR(8))
		 WHEN DOD.ActiveDutyOnActiveDutyStatusDt = 'N' AND DOD.LeftActiveDutyLE367DaysFromActiveDutyStatusDt = 'Y' THEN 'Borrower SCRA Review. Active Duty Begin Date = ' + CAST(CASE WHEN DOD.ActiveDutyBeginDate = '0' THEN '19201231' ELSE DOD.ActiveDutyBeginDate END AS VARCHAR(8)) + ' End Date = ' + CAST(CASE WHEN DOD.ActiveDutyEndDate = '0' THEN '20991231' ELSE DOD.ActiveDutyEndDate END AS VARCHAR(8))
	END,
	'UTLWD42',
	@TODAY,
	'UTLWD43'
FROM
	ULS.scra._DODReturnFile DOD
	INNER JOIN ODW..GA01_APP GA01
		ON GA01.DF_PRS_ID_BR = DOD.SSN
	INNER JOIN 
	(
		SELECT
			GA11.AF_APL_ID,
			MIN(GA11.AD_DSB_ADJ) AS AD_DSB_ADJ
		FROM
			ODW..GA11_LON_DSB_ATY GA11
		WHERE
			GA11.AC_DSB_ADJ = 'A'
			AND GA11.AC_DSB_ADJ_STA = 'A' -- Active
		GROUP BY
			GA11.AF_APL_ID
	) GA11
		ON GA11.AF_APL_ID = GA01.AF_APL_ID
WHERE
	GA11.AD_DSB_ADJ <= CAST(CAST(CASE WHEN DOD.ActiveDutyBeginDate = '0' THEN '19201231' ELSE DOD.ActiveDutyBeginDate END AS VARCHAR(8)) AS DATE)
	AND DOD.SourceFile like '%UTLWD42%'
	AND 
	(
		DOD.ActiveDutyOnActiveDutyStatusDt = 'X'
		OR DOD.NotifiedOfActiveDutyRecallOnActiveDutyStatusDt = 'Y'
		OR
		(
			DOD.ActiveDutyOnActiveDutyStatusDt = 'N'
			AND DOD.LeftActiveDutyLE367DaysFromActiveDutyStatusDt = 'Y'
		)
	)

UNION ALL

SELECT DISTINCT
	DOD.SSN,
	'DACTIVED',
	'',
	'',
	NULL,
	NULL,
	CASE WHEN DOD.ActiveDutyOnActiveDutyStatusDt = 'X' THEN 'Co-borrower/Endorser SCRA Review. Active Duty Begin Date = ' + CAST(CASE WHEN DOD.ActiveDutyBeginDate = '0' THEN '19201231' ELSE DOD.ActiveDutyBeginDate END AS VARCHAR(8)) + ' End Date = ' + CAST(CASE WHEN DOD.ActiveDutyEndDate = '0' THEN '20991231' ELSE DOD.ActiveDutyEndDate END AS VARCHAR(8))
	     WHEN DOD.NotifiedOfActiveDutyRecallOnActiveDutyStatusDt = 'Y' THEN 'Co-borrower/Endorser SCRA Review. Active Duty Begin Date = ' + CAST(CASE WHEN DOD.EIDBeginDate = '0' THEN '19201231' ELSE DOD.EIDBeginDate END AS VARCHAR(8)) + ' End Date = ' + CAST(CASE WHEN DOD.ActiveDutyEndDate = '0' THEN '20991231' ELSE DOD.ActiveDutyEndDate END AS VARCHAR(8)) 
		 WHEN DOD.ActiveDutyOnActiveDutyStatusDt = 'N' AND DOD.LeftActiveDutyLE367DaysFromActiveDutyStatusDt = 'Y' THEN 'Co-borrower/Endorser SCRA Review. Active Duty Begin Date = ' + CAST(CASE WHEN DOD.ActiveDutyBeginDate = '0' THEN '19201231' ELSE DOD.ActiveDutyBeginDate END AS VARCHAR(8)) + ' End Date = ' + CAST(CASE WHEN DOD.ActiveDutyEndDate = '0' THEN '20991231' ELSE DOD.ActiveDutyEndDate END AS VARCHAR(8))
	END,
	'UTLWD42 Endorser',
	@TODAY,
	'UTLWD43 Endorser'
FROM
	ULS.scra._DODReturnFile DOD
	INNER JOIN ODW..GA01_APP GA01
		ON GA01.DF_PRS_ID_EDS = DOD.SSN
	INNER JOIN 
	(
		SELECT
			GA11.AF_APL_ID,
			MIN(GA11.AD_DSB_ADJ) AS AD_DSB_ADJ
		FROM
			ODW..GA11_LON_DSB_ATY GA11
		WHERE
			GA11.AC_DSB_ADJ = 'A'
			AND GA11.AC_DSB_ADJ_STA = 'A' -- Active
		GROUP BY
			GA11.AF_APL_ID
	) GA11
		ON GA11.AF_APL_ID = GA01.AF_APL_ID
WHERE
	GA11.AD_DSB_ADJ <= CAST(CAST(CASE WHEN DOD.ActiveDutyBeginDate = '0' THEN '19201231' ELSE DOD.ActiveDutyBeginDate END AS VARCHAR(8)) AS DATE)
	AND DOD.SourceFile like '%UTLWD42%'
	AND 
	(
		DOD.ActiveDutyOnActiveDutyStatusDt = 'X'
		OR DOD.NotifiedOfActiveDutyRecallOnActiveDutyStatusDt = 'Y'
		OR
		(
			DOD.ActiveDutyOnActiveDutyStatusDt = 'N'
			AND DOD.LeftActiveDutyLE367DaysFromActiveDutyStatusDt = 'Y'
		)
	)

--AAP
INSERT INTO ULS..ArcAddProcessing(ArcTypeId, AccountNumber, ARC, ActivityType, ActivityContact, ScriptId, ProcessOn, Comment, IsReference, IsEndorser, CreatedAt, CreatedBy)
SELECT DISTINCT
	6, --LP50
	DOD.SSN,
	'DACTV',
	'MS',
	'96',
	'UTLWD43',
	GETDATE(),
	CASE WHEN DOD.ActiveDutyOnActiveDutyStatusDt = 'X' THEN 'Borrower SCRA Review. Active Duty Begin Date = ' + CAST(CASE WHEN DOD.ActiveDutyBeginDate = '0' THEN '19201231' ELSE DOD.ActiveDutyBeginDate END AS VARCHAR(8)) + ' End Date = ' + CAST(CASE WHEN DOD.ActiveDutyEndDate = '0' THEN '20991231' ELSE DOD.ActiveDutyEndDate END AS VARCHAR(8))
	     WHEN DOD.NotifiedOfActiveDutyRecallOnActiveDutyStatusDt = 'Y' THEN 'Borrower SCRA Review. Active Duty Begin Date = ' + CAST(CASE WHEN DOD.EIDBeginDate = '0' THEN '19201231' ELSE DOD.EIDBeginDate END AS VARCHAR(8)) + ' End Date = ' + CAST(CASE WHEN DOD.ActiveDutyEndDate = '0' THEN '20991231' ELSE DOD.ActiveDutyEndDate END AS VARCHAR(8))
		 WHEN DOD.ActiveDutyOnActiveDutyStatusDt = 'N' AND DOD.LeftActiveDutyLE367DaysFromActiveDutyStatusDt = 'Y' THEN 'Borrower SCRA Review. Active Duty Begin Date = ' + CAST(CASE WHEN DOD.ActiveDutyBeginDate = '0' THEN '19201231' ELSE DOD.ActiveDutyBeginDate END AS VARCHAR(8)) + ' End Date = ' + CAST(CASE WHEN DOD.ActiveDutyEndDate = '0' THEN '20991231' ELSE DOD.ActiveDutyEndDate END AS VARCHAR(8))
	END,
	0,
	0,
	@TODAY,
	'UTLWD43 Borrower'
FROM
	ULS.scra._DODReturnFile DOD
	INNER JOIN ODW..GA01_APP GA01
		ON GA01.DF_PRS_ID_BR = DOD.SSN
	INNER JOIN 
	(
		SELECT
			GA11.AF_APL_ID,
			MIN(GA11.AD_DSB_ADJ) AS AD_DSB_ADJ
		FROM
			ODW..GA11_LON_DSB_ATY GA11
		WHERE
			GA11.AC_DSB_ADJ = 'A'
			AND GA11.AC_DSB_ADJ_STA = 'A' -- Active
		GROUP BY
			GA11.AF_APL_ID
	) GA11
		ON GA11.AF_APL_ID = GA01.AF_APL_ID
WHERE
	GA11.AD_DSB_ADJ <= CAST(CAST(CASE WHEN DOD.ActiveDutyBeginDate = '0' THEN '19201231' ELSE DOD.ActiveDutyBeginDate END AS VARCHAR(8)) AS DATE)
	AND DOD.SourceFile like '%UTLWD42%'
	AND 
	(
		DOD.ActiveDutyOnActiveDutyStatusDt = 'X'
		OR DOD.NotifiedOfActiveDutyRecallOnActiveDutyStatusDt = 'Y'
		OR
		(
			DOD.ActiveDutyOnActiveDutyStatusDt = 'N'
			AND DOD.LeftActiveDutyLE367DaysFromActiveDutyStatusDt = 'Y'
		)
	)

UNION ALL

SELECT DISTINCT
	6, --LP50
	DOD.SSN,
	'DACTV',
	'MS',
	'96',
	'UTLWD43',
	GETDATE(),
	CASE WHEN DOD.ActiveDutyOnActiveDutyStatusDt = 'X' THEN 'Co-borrower/Endorser SCRA Review. Active Duty Begin Date = ' + CAST(CASE WHEN DOD.ActiveDutyBeginDate = '0' THEN '19201231' ELSE DOD.ActiveDutyBeginDate END AS VARCHAR(8)) + ' End Date = ' + CAST(CASE WHEN DOD.ActiveDutyEndDate = '0' THEN '20991231' ELSE DOD.ActiveDutyEndDate END AS VARCHAR(8))
	     WHEN DOD.NotifiedOfActiveDutyRecallOnActiveDutyStatusDt = 'Y' THEN 'Co-borrower/Endorser SCRA Review. Active Duty Begin Date = ' + CAST(CASE WHEN DOD.EIDBeginDate = '0' THEN '19201231' ELSE DOD.EIDBeginDate END AS VARCHAR(8)) + ' End Date = ' + CAST(CASE WHEN DOD.ActiveDutyEndDate = '0' THEN '20991231' ELSE DOD.ActiveDutyEndDate END AS VARCHAR(8)) 
		 WHEN DOD.ActiveDutyOnActiveDutyStatusDt = 'N' AND DOD.LeftActiveDutyLE367DaysFromActiveDutyStatusDt = 'Y' THEN 'Co-borrower/Endorser SCRA Review. Active Duty Begin Date = ' + CAST(CASE WHEN DOD.ActiveDutyBeginDate = '0' THEN '19201231' ELSE DOD.ActiveDutyBeginDate END AS VARCHAR(8)) + ' End Date = ' + CAST(CASE WHEN DOD.ActiveDutyEndDate = '0' THEN '20991231' ELSE DOD.ActiveDutyEndDate END AS VARCHAR(8))
	END,
	0,
	0,
	@TODAY,
	'UTLWD43 Endorser'
FROM
	ULS.scra._DODReturnFile DOD
	INNER JOIN ODW..GA01_APP GA01
		ON GA01.DF_PRS_ID_EDS = DOD.SSN
	INNER JOIN 
	(
		SELECT
			GA11.AF_APL_ID,
			MIN(GA11.AD_DSB_ADJ) AS AD_DSB_ADJ
		FROM
			ODW..GA11_LON_DSB_ATY GA11
		WHERE
			GA11.AC_DSB_ADJ = 'A'
			AND GA11.AC_DSB_ADJ_STA = 'A' -- Active
		GROUP BY
			GA11.AF_APL_ID
	) GA11
		ON GA11.AF_APL_ID = GA01.AF_APL_ID
WHERE
	GA11.AD_DSB_ADJ <= CAST(CAST(CASE WHEN DOD.ActiveDutyBeginDate = '0' THEN '19201231' ELSE DOD.ActiveDutyBeginDate END AS VARCHAR(8)) AS DATE)
	AND DOD.SourceFile like '%UTLWD42%'
	AND 
	(
		DOD.ActiveDutyOnActiveDutyStatusDt = 'X'
		OR DOD.NotifiedOfActiveDutyRecallOnActiveDutyStatusDt = 'Y'
		OR
		(
			DOD.ActiveDutyOnActiveDutyStatusDt = 'N'
			AND DOD.LeftActiveDutyLE367DaysFromActiveDutyStatusDt = 'Y'
		)
	)