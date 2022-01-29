DECLARE @Arc VARCHAR(5) = 'DELQR'
DECLARE @ScriptId VARCHAR(9) = 'OUTDLQQUE'
DECLARE @Comment VARCHAR(200) = 'Borrower is in a Forbearance Status with a Delinquency'

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
	INNER JOIN
	(
		SELECT
			LN60.BF_SSN,
			LN60.LN_SEQ
		FROM
			UDW..LN60_BR_FOR_APV LN60
			INNER JOIN UDW..FB10_BR_FOR_REQ FB10
				ON FB10.BF_SSN = LN60.BF_SSN
				AND FB10.LF_FOR_CTL_NUM = LN60.LF_FOR_CTL_NUM
		WHERE
			FB10.LC_STA_FOR10 = 'A'
			AND FB10.LC_FOR_STA = 'A'
			AND LN60.LC_STA_LON60 = 'A'
			AND LN60.LC_FOR_RSP != '003' --Exclude denials
			AND CAST(GETDATE() AS DATE) >= CAST(LN60.LD_FOR_BEG AS DATE)
			AND CAST(GETDATE() AS DATE) <= CAST(LN60.LD_FOR_END AS DATE)
			AND CAST(GETDATE() AS DATE) > CAST(LN60.LD_FOR_APL AS DATE)
	) Forb
		ON Forb.BF_SSN = LN10.BF_SSN
		AND Forb.LN_SEQ = LN10.LN_SEQ
	LEFT JOIN UDW..DW01_DW_CLC_CLU DW01
		ON DW01.BF_SSN = LN10.BF_SSN
		AND DW01.LN_SEQ = LN10.LN_SEQ
		AND DW01.WC_DW_LON_STA IN('16','17','18','19','20','21')
	LEFT JOIN UDW..WQ20_TSK_QUE WQ20
		ON WQ20.BF_SSN = LN10.BF_SSN
		AND WQ20.WF_QUE = 'D8'
		AND WQ20.WC_STA_WQUE20 NOT IN ('X','C')
	LEFT JOIN ULS..ArcAddProcessing ExistingAAP
		ON ExistingAAP.AccountNumber = PD10.DF_SPE_ACC_ID
		AND ExistingAAP.ARC = @Arc
		AND ExistingAAP.ScriptId = @ScriptId
		AND ExistingAAP.Comment = @Comment
		AND CAST(ExistingAAP.CreatedAt AS DATE) = CAST(GETDATE() AS DATE)
WHERE
	WQ20.BF_SSN IS NULL --Exclude borrowers with an open D8 queue
	AND ExistingAAP.AccountNumber IS NULL -- Dont re-add something already added today.
	AND DW01.BF_SSN IS NULL --Exclude DW01 of TPD status
