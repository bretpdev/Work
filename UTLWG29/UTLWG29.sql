USE ODW
GO

SET xact_abort on --Rollback if there is an error on the transaction
GO

DECLARE @Today DATE = GETDATE()
DECLARE @ARC VARCHAR(5) = 'DEOL1'
DECLARE @ScriptId VARCHAR(7) = 'UTLWG29'

BEGIN TRANSACTION

INSERT INTO 
	ULS..ArcAddProcessing(ArcTypeId, AccountNumber, ARC, ScriptId, ProcessOn, Comment, IsReference, IsEndorser, ProcessingAttempts, CreatedAt, CreatedBy, ProcessedAt)
SELECT DISTINCT
	1, --ArcTypeId, All Loans
	PD01.DF_SPE_ACC_ID, --AccountNumber
	@ARC, --ARC
	@ScriptId, --ScriptId
	GETDATE(), --ProcessOn
	'Enroll Status equals Death on OneLink; active loans on Compass', --Comment
	0, --IsReference
	0, --IsEndorser
	0, --ProcessingAttempts
	GETDATE(), --CreatedAt
	SUSER_NAME(), --CreatedBy
	NULL --ProcessedAt
FROM
	SD02_STU_ENR SD02
	INNER JOIN PD01_PDM_INF PD01
		ON SD02.DF_PRS_ID_STU = PD01.DF_PRS_ID
	INNER JOIN ODW..SC01_LGS_SCL_INF SC01
		ON SD02.IF_OPS_SCL_RPT = SC01.IF_IST
	LEFT JOIN
	(
		SELECT
			LN10.LF_STU_SSN,
			LN10.LN_SEQ,
			LN10.LF_LON_CUR_OWN
		FROM
			UDW..LN10_LON LN10
			INNER JOIN  UDW..DW01_DW_CLC_CLU DW01
				ON DW01.BF_SSN = LN10.BF_SSN
				AND DW01.LN_SEQ = LN10.LN_SEQ
				AND DW01.WC_DW_LON_STA NOT IN ('12', '16', '17') --Death/Inactive Status
		WHERE
			LN10.LC_STA_LON10 = 'R'
			AND LN10.IF_GTR = '000749' --Guarantor Code
			AND LN10.LA_CUR_PRI > 0.00
	) LN10
		ON SD02.DF_PRS_ID_STU = LN10.LF_STU_SSN
	--Duplication exclusion
	--Check Processing Attempts are less than or equal to 1 to allow it to 
	--try a couple times before adding another arc
	LEFT JOIN ULS..ArcAddProcessing AAP
		ON PD01.DF_SPE_ACC_ID = AAP.AccountNumber
		AND AAP.ARC = @ARC
		AND AAP.ScriptId = @ScriptId
		AND CAST(AAP.ProcessOn AS DATE) = @Today
		AND ISNULL(AAP.Comment, '') = 'Enroll Status equals Death on OneLink; active loans on Compass'
		AND AAP.IsReference = 0
		AND AAP.IsEndorser = 0
		AND AAP.ProcessingAttempts <= 1
		AND CAST(AAP.CreatedAt AS DATE) = @Today
		AND AAP.ProcessedAt IS NULL
	LEFT JOIN UDW..WQ20_TSK_QUE WQ20
		ON PD01.DF_PRS_ID = WQ20.BF_SSN
		AND	WQ20.PF_REQ_ACT = @ARC
		AND WQ20.WC_STA_WQUE20 IN ('A','H','P','U','W') -- Open status
WHERE
	SD02.LC_STA_SD02 = 'A'
	AND SD02.LC_STU_ENR_TYP = 'D'
	AND LN10.LF_STU_SSN IS NOT NULL
	AND AAP.AccountNumber IS NULL
	AND WQ20.BF_SSN IS NULL --Excludes open tasks for the @ARC: DEOL1

COMMIT TRANSACTION;