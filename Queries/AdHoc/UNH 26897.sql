USE ULS
GO

-- drop temp table to if it exist
IF OBJECT_ID('tempdb..#ArcInfo') IS NOT NULL DROP TABLE #ArcInfo

CREATE TABLE #ArcInfo
(
	ArcInfoId INT IDENTITY(1,1),
	LN_SEQ VARCHAR(3) NOT NULL,
	[ArcTypeId] [int] NOT NULL,
	[ArcResponseCodeId] [int] NULL,
	[AccountNumber] [char](10) NOT NULL,
	[RecipientId] [char](9) NULL,
	[ARC] [varchar](5) NOT NULL,
	[ScriptId] [char](10) NOT NULL,
	[ProcessOn] [datetime] NOT NULL,
	[Comment] [varchar](300) NULL,
	[IsReference] [bit] NOT NULL,
	[IsEndorser] [bit] NOT NULL,
	[ProcessFrom] [datetime] NULL,
	[ProcessTo] [datetime] NULL,
	[NeededBy] [datetime] NULL,
	[RegardsTo] [char](9) NULL,
	[RegardsCode] [char](1) NULL,
	[CreatedBy] [varchar](50) NOT NULL,
	[ProcessedAt] [datetime] NULL,
	ArcAddProcessingID INT NULL
)

INSERT INTO
	#ArcInfo
(
	LN_SEQ,
	ArcTypeId, 
	ArcResponseCodeId, 
	AccountNumber, 
	RecipientId, 
	ARC, 
	ScriptId, 
	ProcessOn, 
	Comment, 
	IsReference, 
	IsEndorser, 
	ProcessFrom, 
	ProcessTo, 
	NeededBy, 
	RegardsTo, 
	RegardsCode,
	CreatedBy
)
SELECT
	LN_SEQ,
	ArcTypeId, 
	ArcResponseCodeId, 
	AccountNumber, 
	RecipientId, 
	ARC, 
	ScriptId, 
	ProcessOn, 
	Comment, 
	IsReference, 
	IsEndorser, 
	ProcessFrom, 
	ProcessTo, 
	NeededBy, 
	RegardsTo, 
	RegardsCode,
	CreatedBy
--SELECT	*
FROM
	OPENQUERY
	( 
		DUSTER,
		'
			SELECT DISTINCT
				LN10.LN_SEQ,
				0 AS ArcTypeID,
				null AS ArcResponseCodeId,
				PD10.DF_SPE_ACC_ID AS AccountNumber,
				null AS RecipientId,
				''INTFX'' AS ARC,
				''UNH_26897'' AS ScriptId,
				CURRENT DATE AS ProcessOn,
				CONCAT(CONCAT(VARCHAR_FORMAT(COALESCE(LN50.BEGINDATE, LN60.BEGINDATE), ''MM-DD-YYYY''), '' - ''), VARCHAR_FORMAT(COALESCE(LN50.ENDDATE, LN60.ENDDATE), ''MM-DD-YYYY'')) AS Comment,
				0 AS IsReference,
				0 AS IsEndorser,
				null AS ProcessFrom,
				null AS ProcessTo,
				null AS NeededBy,
				null AS RegardsTo,
				null AS RegardsCode,
				''DCR'' AS CreatedBy
			FROM
				OLWHRM1.PD10_PRS_NME PD10
				INNER JOIN OLWHRM1.LN10_LON LN10 ON LN10.BF_SSN = PD10.DF_PRS_ID
				INNER JOIN OLWHRM1.BR30_BR_EFT BR30 ON BR30.BF_SSN = LN10.BF_SSN
				LEFT OUTER JOIN 
				(
					SELECT
						MAX(FB10.LD_FOR_REQ_END) AS ENDDATE,
						MAX(FB10.LD_FOR_REQ_BEG) AS BEGINDATE,
						LN60.BF_SSN,
						LN60.LN_SEQ
					FROM
						OLWHRM1.LN60_BR_FOR_APV LN60
						LEFT OUTER JOIN OLWHRM1.FB10_BR_FOR_REQ FB10
							ON FB10.BF_SSN = LN60.BF_SSN 
							AND FB10.LF_FOR_CTL_NUM = LN60.LF_FOR_CTL_NUM
					WHERE
						FB10.LC_FOR_STA = ''A''
						AND FB10.LC_STA_FOR10 = ''A''
						AND	LN60.LC_STA_LON60 = ''A''
					GROUP BY
						LN60.BF_SSN,
						LN60.LN_SEQ
				) LN60 ON LN60.BF_SSN = LN10.BF_SSN and LN60.LN_SEQ = LN10.LN_SEQ AND LN60.ENDDATE >= LN10.LD_LON_ACL_ADD
				LEFT OUTER JOIN 
				(
					SELECT
						MAX(DF10.LD_DFR_REQ_END) AS ENDDATE,
						MAX(DF10.LD_DFR_REQ_BEG) AS BEGINDATE,
						LN50.BF_SSN,
						LN50.LN_SEQ
					FROM
						OLWHRM1.LN50_BR_DFR_APV LN50
						LEFT OUTER JOIN OLWHRM1.DF10_BR_DFR_REQ DF10
							ON DF10.BF_SSN = LN50.BF_SSN 
							AND DF10.LF_DFR_CTL_NUM = LN50.LF_DFR_CTL_NUM
					WHERE
						DF10.LC_DFR_STA = ''A''
						AND DF10.LC_STA_DFR10  = ''A''
						AND	LN50.LC_STA_LON50 = ''A''
					GROUP BY
						LN50.BF_SSN,
						LN50.LN_SEQ
				) LN50 ON LN50.BF_SSN = LN10.BF_SSN and LN50.LN_SEQ = LN10.LN_SEQ AND LN50.ENDDATE >= LN10.LD_LON_ACL_ADD
			WHERE
				LN10.LF_LON_CUR_OWN IN (''82976901'', ''82976902'', ''82976903'', ''82976904'', ''82976905'', ''82976906'', ''82976907'', ''82976908'')
				AND
				BR30.BC_EFT_STA = ''A''
				AND
				LN10.LD_LON_ACL_ADD IN (''2016-02-11'', ''2016-02-26'', ''2016-03-16'')
		'
	) X
WHERE
	X.Comment IS NOT NULL


DECLARE 
	@AccountNumber CHAR(10),
	@ArcAddProcessingId INT,
	@Comment VARCHAR(250)

WHILE EXISTS(SELECT NULL FROM #ArcInfo WHERE ArcAddProcessingID IS NULL)
BEGIN

	SELECT TOP 1
		@AccountNumber = AI.AccountNumber,
		@Comment = AI.Comment
	FROM
		#ArcInfo AI
	WHERE
		AI.ArcAddProcessingID IS NULL
	
	
	INSERT INTO 
		ArcAddProcessing
	(
		ArcTypeId, 
		ArcResponseCodeId, 
		AccountNumber, 
		RecipientId, 
		ARC, 
		ScriptId, 
		ProcessOn, 
		Comment,
		IsReference,
		IsEndorser, 
		ProcessFrom, 
		ProcessTo, 
		NeededBy, 
		RegardsTo, 
		RegardsCode,
		CreatedBy
	)
	SELECT DISTINCT
		ArcTypeId, 
		ArcResponseCodeId, 
		AccountNumber, 
		RecipientId, 
		ARC, 
		ScriptId, 
		ProcessOn, 
		Comment,
		IsReference, 
		IsEndorser, 
		ProcessFrom, 
		ProcessTo, 
		NeededBy, 
		RegardsTo, 
		RegardsCode,
		CreatedBy
	FROM
		#ArcInfo AI
	WHERE
		AI.AccountNumber = @AccountNumber
		AND
		AI.Comment = @Comment


	SET @ArcAddProcessingID = SCOPE_IDENTITY()
		
	UPDATE
		#ArcInfo
	SET
		ArcAddProcessingID = @ArcAddProcessingID
	WHERE
		AccountNumber = @AccountNumber
		AND
		Comment = @Comment

	
	INSERT INTO ArcLoanSequenceSelection
	(
		ArcAddProcessingId,
		LoanSequence
	)
	SELECT
		ArcAddProcessingId,
		LN_SEQ
	FROM
		#ArcInfo
	WHERE
		AccountNumber = @AccountNumber
		AND
		Comment = @Comment
		
		
	UPDATE
		ArcAddProcessing
	SET
		Comment = REPLACE('ACH RIR needs to be updated for loan(s) ' + (SELECT CAST(LoanSequence AS VARCHAR(2)) + ',' FROM ArcLoanSequenceSelection WHERE ArcAddProcessingId = @ArcAddProcessingId FOR XML PATH ('')) + ' during the following time period: ' + Comment, ', ', ' ') 
	WHERE
		ArcAddProcessingId = @ArcAddProcessingId

END


/* FOR TESTING TESTING  ONLY */
--SELECT TOP 10000
--	AAP.AccountNumber,
--	COUNT(AAP.AccountNumber) [a_count]
--FROM
--	ArcAddProcessing AAP
--WHERE
--	AAP.ARC = 'INTFX'
--GROUP BY
--	AAP.AccountNumber
--HAVING
--	COUNT(AAP.AccountNumber) > 1
--ORDER BY
--	a_count


--SELECT TOP 1000
--	*
--FROM
--	ArcAddProcessing AAP
--WHERE
--	AAP.ARC = 'INTFX'
--	AND
--	AAP.AccountNumber = '4055638046'

--SELECT
--	*
--FROM
--	#ArcInfo
--WHERE
--	AccountNumber = '4055638046'


/*  RESET ArcLoanSequenceSelection and ArcAddProcessing

DELETE FROM
	ArcLoanSequenceSelection
WHERE
	ArcAddProcessingId in (SELECT ArcAddProcessingId FROM ArcAddProcessing WHERE ARC = 'INTFX')

DELETE FROM 
	ArcAddProcessing
WHERE
	ARC = 'INTFX'

DBCC CHECKIDENT(ArcAddProcessing, RESEED)
DBCC CHECKIDENT(ArcLoanSequenceSelection, RESEED)
*/