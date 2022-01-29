USE CDW
GO

INSERT INTO 
	CLS..ArcAddProcessing
	(
		[ArcTypeId],
		[ArcResponseCodeId],
		[AccountNumber],
		[RecipientId],
		[ARC],
		[ScriptId],
		[ProcessOn],
		[Comment],
		[IsReference],
		[IsEndorser],
		[ProcessFrom],
		[ProcessTo],
		[NeededBy],
		[RegardsTo],
		[RegardsCode],
		[CreatedAt],
		[CreatedBy],
		[ProcessedAt]
	)
SELECT
	1 [ArcTypeId],
	NULL [ArcResponseCodeId],
	POP.BF_SSN [AccountIdentifier],
	NULL [RecipientId],
	'SCIGR' [ARC],
	'UTNWS99' [ScriptId],
	GETDATE() [ProcessOn],
	NULL [Comment],
	0 [IsReference],
	0 [IsEndorser],
	NULL [ProcessFrom],
	NULL [ProcessTo],
	NULL [NeededBy],
	NULL [RegardsTo],
	NULL [RegardsCode],
	GETDATE() [CreatedAt],
	'UTNWS99' [CreatedBy],
	NULL [ProcessedAt]
FROM
(
	SELECT DISTINCT
		LN10.BF_SSN
	FROM
		(
			SELECT
				LN10.BF_SSN,
				MIN(LN10.LD_LON_1_DSB) AS MIN_LD_LON_1_DSB
			FROM
				CDW..LN10_LON LN10
			GROUP BY
				LN10.BF_SSN
		) AS LN10
		INNER JOIN 
		( -- loans in the required statuses
			SELECT
				LN10.BF_SSN,
				LN10.LN_SEQ,
				LN10.LA_CUR_PRI,
				LN10.LC_STA_LON10,
				DW01.WC_DW_LON_STA,
				DF10.LC_DFR_TYP
			FROM
				CDW..LN10_LON LN10
				LEFT JOIN CDW..LN50_BR_DFR_APV LN50 
					ON LN50.BF_SSN = LN10.BF_SSN 
					AND LN50.LN_SEQ = LN10.LN_SEQ
					AND LN50.LC_STA_LON50 = 'A'
					AND LN50.LC_DFR_RSP != '003'
					AND CAST(GETDATE() AS DATE) BETWEEN LN50.LD_DFR_BEG AND LN50.LD_DFR_END
				LEFT JOIN CDW..DF10_BR_DFR_REQ DF10 
					ON DF10.BF_SSN = LN10.BF_SSN 
					AND LN50.LF_DFR_CTL_NUM = DF10.LF_DFR_CTL_NUM
					AND DF10.LC_STA_DFR10 = 'A'
					AND DF10.LC_DFR_TYP IN 
					(
						'15', --Enrolled full time
						'18', -- Enrolled Half time
						'19', -- Parent of Student enrolled full time
						'22', -- Parent of Student enrolled half time
						'45', -- Parent Plus In- School
						'46' -- Post enrollment 
					)
				INNER JOIN CDW..DW01_DW_CLC_CLU DW01 
					ON DW01.BF_SSN = LN10.BF_SSN 
					AND DW01.LN_SEQ = LN10.LN_SEQ
				LEFT JOIN CDW..LN16_LON_DLQ_HST LN16 
					ON LN16.BF_SSN = LN10.BF_SSN 
					AND LN16.LN_SEQ = LN10.LN_SEQ
					AND LN16.LC_STA_LON16 = '1'
				
			WHERE
				LN10.LA_CUR_PRI > 0 -- has a balance
				AND	LN10.LC_STA_LON10 = 'R' -- released loan
				AND	DW01.WC_DW_LON_STA != '03' -- not in repayment
				AND	LN16.BF_SSN IS NULL -- not delinquent
				AND 
				(
					DW01.WC_DW_LON_STA IN ('01', '02', '05')
					OR DF10.BF_SSN IS NOT NULL
				)
		) Loans ON Loans.BF_SSN = LN10.BF_SSN
		LEFT JOIN CLS..ArcAddProcessing APP
			ON APP.AccountNumber = Loans.BF_SSN
			AND ARC = 'SCIGR'
			AND CAST(CreatedAt AS DATE) >= CAST(DATEADD(MONTH, -2, GETDATE()) AS DATE) 
		
	WHERE
		MONTH(LN10.MIN_LD_LON_1_DSB) % 3 = MONTH(GETDATE()) % 3 -- compare mod of disbursement month and current month, when the same include in the population'
		AND DAY(LN10.MIN_LD_LON_1_DSB) <= DAY(GETDATE()) 
		AND APP.ArcAddProcessingId IS NULL --excludes borrowers who have the arc recently (used for recovery just in case we miss a day)
) POP