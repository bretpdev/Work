DECLARE @Arc VARCHAR(5) = 'DQACH'
DECLARE @ScriptId VARCHAR(9) = 'OUTDLQQUE'
DECLARE @Comment VARCHAR(200) = 'Borrower is on ACH with a Delinquency'

INSERT INTO ULS..ArcAddProcessing(ArcTypeId, AccountNumber, ARC, ScriptId, ProcessOn, Comment, IsReference, IsEndorser, CreatedAt, CreatedBy)
SELECT DISTINCT
	1,
	PD10.DF_SPE_ACC_ID,
	@Arc,
	@ScriptId,
	GETDATE(),
	@Comment,
	0,
	0,
	GETDATE(),
	SUSER_SNAME()
FROM
	UDW..PD10_PRS_NME PD10
	INNER JOIN UDW..LN10_LON LN10
		ON PD10.DF_PRS_ID = LN10.BF_SSN
		AND LN10.LC_STA_LON10 ='R'
		AND LN10.LA_CUR_PRI > 0
	INNER JOIN UDW..LN16_LON_DLQ_HST LN16
		ON LN16.BF_SSN = LN10.BF_SSN
		AND LN16.LN_SEQ = LN10.LN_SEQ
		AND LN16.LC_STA_LON16 = '1'
		AND LN16.LC_DLQ_TYP = 'P'
		AND 
		(
			DATEADD(DAY,-5,CAST(GETDATE() AS DATE)) <= LN16.LD_DLQ_MAX --Account for AES holidays that we dont honor
			OR LN16.LN_DLQ_MAX >= 360
		)
	INNER JOIN UDW..LN83_EFT_TO_LON LN83
		ON LN83.BF_SSN = LN10.BF_SSN
		AND LN83.LN_SEQ = LN10.LN_SEQ
		AND LN83.LC_STA_LN83 = 'A'
		AND LN83.LD_EFT_EFF_BEG >= DATEADD(DAY,-14,LN16.LD_DLQ_OCC) --Give 2 week window
	LEFT JOIN UDW..WQ20_TSK_QUE WQ20
		ON WQ20.BF_SSN = LN10.BF_SSN
		AND WQ20.WF_QUE = 'D7'
		AND WQ20.WC_STA_WQUE20 NOT IN ('X','C')
	LEFT JOIN ULS..ArcAddProcessing ExistingAAP
		ON ExistingAAP.AccountNumber = PD10.DF_SPE_ACC_ID
		AND ExistingAAP.ARC = @Arc
		AND ExistingAAP.ScriptId = @ScriptId
		AND ExistingAAP.Comment = @Comment
		AND CAST(ExistingAAP.CreatedAt AS DATE) = CAST(GETDATE() AS DATE)
WHERE
	WQ20.BF_SSN IS NULL --Exclude borrowers with an open D7 queue
	AND ExistingAAP.AccountNumber IS NULL -- Dont re-add something already added today.