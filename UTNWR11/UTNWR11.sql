SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED

BEGIN TRANSACTION
	DECLARE @ERROR INT = 0
	
DECLARE @ARC VARCHAR(5) = 'RNSLD'
DECLARE @SCRIPTID VARCHAR(9) = 'UTNWR11'
DECLARE @COMMENT VARCHAR(200) = 'manually reopen the NSLDS reporting'
DECLARE @ArcAddProcessingId INT

DECLARE @ArcAddProcessingIds TABLE(ArcAddProcessingId INT, AccountNumber VARCHAR(10))
DECLARE @Pop TABLE(AccountNumber VARCHAR(10), LoanSequence SMALLINT)

;WITH COD AS
(--get COD 0101 loans
	SELECT
		 PD10.DF_SPE_ACC_ID
		,LN90.BF_SSN
		,LN90.LN_SEQ
		,LN90.LA_FAT_CUR_PRI
		,LN90.LD_FAT_EFF
	FROM
		CDW..LN90_FIN_ATY LN90
		INNER JOIN CDW..PD10_PRS_NME PD10
			ON LN90.BF_SSN = PD10.DF_PRS_ID
	WHERE
		LN90.PC_FAT_TYP = '01' 
		AND LN90.PC_FAT_SUB_TYP = '01'
)
,CANCELLEDDREDUCED AS
(--get COD cancelled/reduced loans 0786
	SELECT DISTINCT
		 COD.DF_SPE_ACC_ID
		,COD.BF_SSN
		,COD.LN_SEQ
		,LN90_0786.MAX_LD_FAT_PST	AS _0786_LD_FAT_PST
		,LN90_0786.LD_FAT_EFF		AS _0786_LD_FAT_EFF
		,LN90_0786.NULL_LD_FAT_PST
		,LN90_0786.LA_FAT_CUR_PRI
	FROM
		COD
		INNER JOIN 
		(--gets date tied to greatest ld_fat_pst
			SELECT
				 LN90.BF_SSN
				,LN90.LN_SEQ
				,LN90_MAX.MAX_LN_FAT_SEQ
				,LN90_MAX.MAX_LD_FAT_PST
				,LN90.LD_FAT_EFF--date
				,CASE
					WHEN LD_FAT_PST IS NULL
					THEN 1
					ELSE 0
				END AS NULL_LD_FAT_PST
				,LN90.LA_FAT_CUR_PRI
			FROM
				CDW..LN90_FIN_ATY LN90
				INNER JOIN
				(--gets greatest ld_fat_pst
					SELECT
						BF_SSN
						,LN_SEQ
						,MAX(LN_FAT_SEQ) AS MAX_LN_FAT_SEQ
						,MAX(COALESCE(LD_FAT_PST, LD_FAT_APL)) AS MAX_LD_FAT_PST
					FROM
						CDW..LN90_FIN_ATY
					WHERE
						PC_FAT_TYP = '07'
						AND PC_FAT_SUB_TYP = '86'
					GROUP BY
						 BF_SSN
						,LN_SEQ
				) LN90_MAX
					ON LN90.BF_SSN = LN90_MAX.BF_SSN
					AND LN90.LN_SEQ = LN90_MAX.LN_SEQ
					AND LN90.LN_FAT_SEQ = LN90_MAX.MAX_LN_FAT_SEQ
					AND COALESCE(LN90.LD_FAT_PST, LN90.LD_FAT_APL) = LN90_MAX.MAX_LD_FAT_PST
		) LN90_0786
			ON COD.BF_SSN = LN90_0786.BF_SSN
			AND COD.LN_SEQ = LN90_0786.LN_SEQ
)
,REINSTATEMENT AS
(--get COD reinstatement loans 0785
	SELECT distinct
		 COD.DF_SPE_ACC_ID
		,COD.BF_SSN
		,COD.LN_SEQ
		,LN90_0785.MAX_LD_FAT_PST	AS _0785_LD_FAT_PST
		,LN90_0785.LD_FAT_EFF		AS _0785_LD_FAT_EFF
		,LN90_0785.NULL_LD_FAT_PST
		,LN90_0785.LA_FAT_CUR_PRI
	FROM
		COD
		INNER JOIN 
		(--gets date tied to greatest ld_fat_pst
			SELECT
				 LN90.BF_SSN
				,LN90.LN_SEQ
				,LN90_MAX.MAX_LN_FAT_SEQ
				,LN90_MAX.MAX_LD_FAT_PST
				,LN90.LD_FAT_EFF--date
				,CASE
					WHEN LD_FAT_PST IS NULL
					THEN 1
					ELSE 0
				END AS NULL_LD_FAT_PST
				,LN90.LA_FAT_CUR_PRI
			FROM
				CDW..LN90_FIN_ATY LN90
				INNER JOIN
				(--gets greatest ld_fat_pst
					SELECT
						BF_SSN
						,LN_SEQ
						,MAX(LN_FAT_SEQ) AS MAX_LN_FAT_SEQ
						,MAX(COALESCE(LD_FAT_PST, LD_FAT_APL)) AS MAX_LD_FAT_PST
					FROM
						CDW..LN90_FIN_ATY
					WHERE
						PC_FAT_TYP = '07'
						AND PC_FAT_SUB_TYP = '85'
					GROUP BY
						 BF_SSN
						,LN_SEQ
				) LN90_MAX
					ON LN90.BF_SSN = LN90_MAX.BF_SSN
					AND LN90.LN_SEQ = LN90_MAX.LN_SEQ
					AND LN90.LN_FAT_SEQ = LN90_MAX.MAX_LN_FAT_SEQ
					AND COALESCE(LN90.LD_FAT_PST, LN90.LD_FAT_APL) = LN90_MAX.MAX_LD_FAT_PST
		) LN90_0785
			ON COD.BF_SSN = LN90_0785.BF_SSN
			AND COD.LN_SEQ = LN90_0785.LN_SEQ
)

--Get the population to add the queues to
INSERT INTO @Pop(AccountNumber, LoanSequence)
SELECT DISTINCT
	AccountNumber,
	LoanSequence
FROM
(
	SELECT DISTINCT
		COD.DF_SPE_ACC_ID AS [AccountNumber]
		,COD.LN_SEQ AS [LoanSequence]
		--,RI._0785_LD_FAT_PST  AS [Posting Date of Last Reinstatement]
	FROM
		COD COD
		INNER JOIN REINSTATEMENT RI --0785's
			ON COD.BF_SSN = RI.BF_SSN
			AND COD.LN_SEQ = RI.LN_SEQ
		INNER JOIN CANCELLEDDREDUCED CR --0786's
			ON COD.BF_SSN = CR.BF_SSN
			AND COD.LN_SEQ = CR.LN_SEQ
		INNER JOIN 
		(
			SELECT
				BF_SSN
				,LN_SEQ
				,MAX(WD_NDS_LN_DSB_RPT) AS WD_NDS_LN_DSB_RPT
			FROM		
				CDW..GR10_RPT_LON_APL
			GROUP BY
				BF_SSN
				,LN_SEQ
		) GR10
			ON COD.BF_SSN = GR10.BF_SSN
			AND COD.LN_SEQ = GR10.LN_SEQ
			AND	GR10.WD_NDS_LN_DSB_RPT < RI._0785_LD_FAT_PST
		LEFT JOIN CDW..WQ20_TSK_QUE WQ20
			ON WQ20.BF_SSN = COD.BF_SSN
			AND WQ20.WF_QUE = 'P8'
			AND WQ20.WF_SUB_QUE = '01'
			AND WQ20.WC_STA_WQUE20 NOT IN ('C', 'X') --Status is not closed or cancelled
	WHERE
		--0785 refers to the payment made in the reinstatement population and 0786 refers to the cancelled/reduced payment
		--Effective date of reinstatement payment is equal to the date of cancelled/reduced payment
		RI._0785_LD_FAT_EFF = CR._0786_LD_FAT_EFF
		AND (
				(
					RI._0785_LD_FAT_PST > CR._0786_LD_FAT_PST
					AND RI.NULL_LD_FAT_PST = 0
					AND CR.NULL_LD_FAT_PST = 0
				)
				OR
				(
					RI._0785_LD_FAT_PST >= CR._0786_LD_FAT_PST
					AND RI.NULL_LD_FAT_PST = 1
				)
				OR
				(
					RI._0785_LD_FAT_PST >= CR._0786_LD_FAT_PST
					AND CR.NULL_LD_FAT_PST = 1
				)
			)
		AND COD.LD_FAT_EFF = RI._0785_LD_FAT_EFF
		AND COD.LD_FAT_EFF = CR._0786_LD_FAT_EFF
		AND WQ20.BF_SSN IS NULL

	UNION

	SELECT DISTINCT
		PD10.DF_SPE_ACC_ID AS AccountNumber,
		LN10.LN_SEQ AS LoanSequence
	FROM
		CDW..PD10_PRS_NME PD10
		INNER JOIN CDW..LN10_LON LN10
			ON PD10.DF_PRS_ID = LN10.BF_SSN
		INNER JOIN CDW..GR10_RPT_LON_APL GR10
			ON LN10.BF_SSN = GR10.BF_SSN
			AND LN10.LN_SEQ = GR10.LN_SEQ
		LEFT JOIN CDW..WQ20_TSK_QUE WQ20
			ON WQ20.BF_SSN = LN10.BF_SSN
			AND WQ20.WF_QUE = 'P8'
			AND WQ20.WF_SUB_QUE = '01'
			AND WQ20.WC_STA_WQUE20 NOT IN ('C', 'X') --Status is not closed or cancelled
	WHERE
		GR10.WC_NDS_RPT_STA = 'C'
		AND LN10.LA_CUR_PRI > 0.00
		AND LN10.LC_STA_LON10 = 'R'
		AND WQ20.BF_SSN IS NULL
) R1

SELECT @ERROR = @ERROR + @@ERROR

--Add arcs for the records in POP
INSERT INTO CLS.[dbo].[ArcAddProcessing]
           ([ArcTypeId]
           ,[AccountNumber]
           ,[ARC]
           ,[ScriptId]
           ,[ProcessOn]
           ,[Comment]
           ,[IsReference]
           ,[IsEndorser]
           ,[CreatedAt]
           ,[CreatedBy])
OUTPUT INSERTED.ArcAddProcessingId, INSERTED.AccountNumber INTO @ArcAddProcessingIds(ArcAddProcessingId, AccountNumber)
SELECT DISTINCT
	0,
	POP.AccountNumber,
	@ARC,
	@SCRIPTID,
	GETDATE(),
	CONCAT(@COMMENT, ' Loan Sequences: ', POP.commentLoanSequences),
	0,
	0,
	GETDATE(),
	SUSER_NAME()
FROM
	(
		SELECT DISTINCT	
			P.AccountNumber,
			AdditionalComment.commentLoanSequences
		FROM
			@Pop P 
			INNER JOIN --Adding Loan Sequence Comment
			(
				SELECT
					POP_ID.AccountNumber,
					commentLoanSequences = STUFF((
						SELECT ',' + CAST(POP_XML.LoanSequence AS VARCHAR(5))
						FROM @POP POP_XML
						WHERE POP_XML.AccountNumber = POP_ID.AccountNumber
						FOR XML PATH('')), 1, 1, '')
				FROM
					@POP POP_ID
				GROUP BY
					POP_ID.AccountNumber
			) AdditionalComment
				ON P.AccountNumber = AdditionalComment.AccountNumber
	) POP
	LEFT JOIN CLS..ArcAddProcessing ExistingAAP
		ON ExistingAAP.AccountNumber = POP.AccountNumber
		AND ExistingAAP.ARC = @Arc
		AND ExistingAAP.ScriptId = @ScriptId
		AND ExistingAAP.Comment = @Comment
		AND CAST(ExistingAAP.CreatedAt AS DATE) = CAST(GETDATE() AS DATE)
WHERE
	ExistingAAP.AccountNumber IS NULL

SELECT @ERROR = @ERROR + @@ERROR

--Insert the loans to add the arc to for each of the previous records
INSERT INTO CLS.[dbo].[ArcLoanSequenceSelection]
           ([ArcAddProcessingId]
           ,[LoanSequence])
SELECT
	ARC.ArcAddProcessingId,
	POP.LoanSequence
FROM
	@Pop POP
	INNER JOIN @ArcAddProcessingIds ARC
		ON POP.AccountNumber = ARC.AccountNumber

SELECT @ERROR = @ERROR + @@ERROR

IF @ERROR = 0 
 	BEGIN 
 		PRINT 'Transaction committed' 
 		COMMIT TRANSACTION 
 		--ROLLBACK TRANSACTION 
 	END 
 ELSE 
 	BEGIN 
 		PRINT 'ERROR:  ' + CAST(@ERROR as VARCHAR(10)) 
 		PRINT 'Transaction NOT committed' 
 		ROLLBACK TRANSACTION 
 	END 