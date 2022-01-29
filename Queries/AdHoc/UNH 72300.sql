USE Udw

GO

BEGIN TRANSACTION
	DECLARE @ERROR INT = 0
	DECLARE @ROWCOUNT INT = 0

DECLARE @LastWeek DATE = DATEADD(WEEK, -1, CAST(GETDATE() - 1 AS DATE)), --Looks at prior week, offset by one day since we are not looking at today
@Yesterday DATE = CAST(GETDATE() - 1 AS DATE)

--INSERT INTO UDW..LT20_LTR_REQ_PRC
SELECT distinct
	P.AccountNumber,
	'TSX08',
	p.ArcRequestDate,
	1,
	1,
	p.LetterId,
	'P',
	PD10.DF_PRS_ID,
	1,
	p.ActivitySequence,
	'Y',
	'',
	'',
	'',
	'',
	'',
	'',
	'N',
	'A',
	'N',
	p.ArcRequestDate,
	'',
	'',
	'',
	'',
	'',
	'',
	'',
	'',
	'',
	'',
	'',
	'N',
	NULL,
	CASE 
		WHEN PH05.DF_SPE_ID IS NOT NULL AND PH05.DI_VLD_CNC_EML_ADR = 'Y' AND PH05.DI_CNC_ELT_OPI = 'Y' THEN 1
		ELSE 0
	END AS ECORR,
	NULL,
	NULL,
	NULL,
	NULL,
	getdate(),
	0
FROM
	(
		SELECT 
			PD10.DF_SPE_ACC_ID AS AccountNumber,
			CAST(AY10.LD_ATY_REQ_RCV AS DATE) AS ArcRequestDate,
			AY10.PF_REQ_ACT AS ARC,
			AC11.PF_LTR AS LetterId,
			AY10.LN_ATY_SEQ AS ActivitySequence
		FROM
			UDW..AY10_BR_LON_ATY AY10
			INNER JOIN UDW..AC11_ACT_REQ_LTR AC11
				ON AC11.PF_REQ_ACT = AY10.PF_REQ_ACT
			INNER JOIN UDW..PD10_PRS_NME PD10
				ON PD10.DF_PRS_ID = AY10.BF_SSN
			LEFT JOIN
			(
				SELECT DISTINCT
					LT20.DF_SPE_ACC_ID,
					LT20.RM_DSC_LTR_PRC,
					LT20.RF_SBJ_PRC,
					LT20.RN_ATY_SEQ_PRC
				FROM
					UDW..LT20_LTR_REQ_PRC LT20
				WHERE
					CAST(RT_RUN_SRT_DTS_PRC AS DATE) >= @LastWeek
			) HasLt20Record
				ON HasLt20Record.RN_ATY_SEQ_PRC = AY10.LN_ATY_SEQ
				AND HasLt20Record.RM_DSC_LTR_PRC = AC11.PF_LTR
				AND
				(
					HasLt20Record.DF_SPE_ACC_ID = PD10.DF_SPE_ACC_ID
					or HasLt20Record.RF_SBJ_PRC = PD10.DF_PRS_ID
				)
		WHERE
			HasLt20Record.DF_SPE_ACC_ID IS NULL --Didnt make it to LT20
			AND CAST(AY10.LD_ATY_REQ_RCV AS DATE) BETWEEN @LastWeek AND @Yesterday
			AND AY10.LC_STA_ACTY10 = 'A'
	) p
	inner JOIN UDW..PD10_PRS_NME PD10
		ON PD10.DF_SPE_ACC_ID = P.AccountNumber
	LEFT JOIN UDW.dbo.PH05_CNC_EML PH05
		ON PH05.DF_SPE_ID = P.AccountNumber 


SELECT @ROWCOUNT = @@ROWCOUNT, @ERROR = @@ERROR


IF @ROWCOUNT = 1 AND @ERROR = 0
	BEGIN
		PRINT 'Transaction committed'
		COMMIT TRANSACTION
		--ROLLBACK TRANSACTION
	END
ELSE
	BEGIN
		PRINT 'ROWCOUNT:  ' + CAST(@ROWCOUNT as VARCHAR(10))
		PRINT 'ERROR:  ' + CAST(@ERROR as VARCHAR(10))
		PRINT '!!!  Transaction NOT committed.  Contact a member of Application Development to have this error corrected.'
		ROLLBACK TRANSACTION
	END