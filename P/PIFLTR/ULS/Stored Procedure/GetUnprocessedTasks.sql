CREATE PROCEDURE [pifltr].[GetUnprocessedTasks]
AS

DECLARE @UnprocessedTasksTemp TABLE (WF_QUE VARCHAR(2), WF_SUB_QUE VARCHAR(2), WN_CTL_TSK VARCHAR(18), PF_REQ_ACT VARCHAR(5), LnSeq VARCHAR(4), IC_LON_PGM VARCHAR(6),LD_LON_1_DSB DATE, OriginalBalance NUMERIC(8,2), WD_ACT_REQ DATE, Ssn VARCHAR(9), AccountNumber VARCHAR(10), MaxTranPif DATE, MaxTranConsol DATE, MaxTranClc DATE)			
INSERT INTO @UnprocessedTasksTemp (WF_QUE, WF_SUB_QUE, WN_CTL_TSK, PF_REQ_ACT, LnSeq, IC_LON_PGM, LD_LON_1_DSB, OriginalBalance, WD_ACT_REQ, Ssn, AccountNumber, MaxTranPif, MaxTranConsol, MaxTranClc)
SELECT
	WQ20.WF_QUE,
	WQ20.WF_SUB_QUE,
	WQ20.WN_CTL_TSK,
	WQ20.PF_REQ_ACT,
	LN10.LN_SEQ AS LnSeq,
	LN10.IC_LON_PGM,
	LN10.LD_LON_1_DSB,
	LN15.OriginalBalance,
	WQ20.WD_ACT_REQ,
	WQ20.BF_SSN AS Ssn,
	PD10.DF_SPE_ACC_ID AS AccountNumber,
	LN10.LD_PIF_RPT AS MaxTranPif,
	MaxDateConsol.MaxTranConsol,
	MaxDateClc.MaxTranClc
	FROM
		UDW..WQ20_TSK_QUE WQ20
		INNER JOIN UDW..PD10_PRS_NME PD10
			ON PD10.DF_PRS_ID = WQ20.BF_SSN
		INNER JOIN UDW..LN10_LON LN10
			ON LN10.BF_SSN = WQ20.BF_SSN
			AND LN10.LN_SEQ =  SUBSTRING(WQ20.WN_CTL_TSK, 10,4)
			AND REPLACE(STR( LN10.LN_SEQ, 4), ' ', '0') = SUBSTRING(WQ20.WN_CTL_TSK, 10,4)
		LEFT JOIN
		(
			SELECT
				LN15.BF_SSN,
				LN15.LN_SEQ,
				SUM(ISNULL(LN15.LA_DSB,0.00) - ISNULL(LN15.LA_DSB_CAN,0.00)) AS OriginalBalance
			FROM
				UDW..LN15_DSB LN15
			WHERE
				LN15.LC_STA_LON15 = '1' --Active
				AND LN15.LC_DSB_TYP = '2'
			GROUP BY
				LN15.BF_SSN,
				LN15.LN_SEQ
		) LN15
			ON LN15.BF_SSN = LN10.BF_SSN
			AND LN15.LN_SEQ = LN10.LN_SEQ
		LEFT JOIN --Max trans cancel
		(
			SELECT 
				LN90.BF_SSN,
				LN90.LN_SEQ,
				MAX(LN90.LD_FAT_EFF) AS MaxTranClc
				FROM 
					UDW..LN90_FIN_ATY LN90
				WHERE 
				(
					(LN90.PC_FAT_TYP  =  '10' AND LN90.PC_FAT_SUB_TYP IN ('40', '45')) -- School refund, returned disbursment
					OR (LN90.PC_FAT_TYP  =  '20' AND LN90.PC_FAT_SUB_TYP IN('10')) -- Refund to the borrower
				)
					AND LN90.LC_STA_LON90 = 'A'
					AND RTRIM(COALESCE(LN90.LC_FAT_REV_REA,'')) = ''
				GROUP BY
					LN90.BF_SSN,
					LN90.LN_SEQ
		) MaxDateClc
			ON MaxDateClc.BF_SSN = WQ20.BF_SSN 
			AND MaxDateClc.LN_SEQ = LN10.LN_SEQ	
		--LEFT JOIN
		--(
		--	SELECT
		--		LN90.BF_SSN,
		--		LN90.LN_SEQ,
		--		MAX(LN90.LD_FAT_EFF) AS MaxTranPif
		--		FROM 
		--			UDW..LN90_FIN_ATY LN90
		--		WHERE 
		--		(
		--			(LN90.PC_FAT_TYP  =  '10' AND LN90.PC_FAT_SUB_TYP IN ('10','20', '35', '36', '41', '50', '54')) -- Borrower, Federal Government, Borrower Cancel
		--			OR (LN90.PC_FAT_TYP IN ('50','60') AND LN90.PC_FAT_SUB_TYP IN ('02', '03')) -- Manual or machine write off / write up
		--			OR (LN90.PC_FAT_TYP  =  '20' AND LN90.PC_FAT_SUB_TYP = '10') -- Refund to the borrower
		--		)
		--			AND LN90.LC_STA_LON90 = 'A'
		--			AND RTRIM(COALESCE(LN90.LC_FAT_REV_REA,'')) = ''
		--		GROUP BY
		--			LN90.BF_SSN,
		--			LN90.LN_SEQ
		--) MaxDatePif
		--	ON WQ20.BF_SSN = MaxDatePif.BF_SSN
		--	AND MaxDatePif.LN_SEQ = LN10.LN_SEQ
		LEFT JOIN
		(
			SELECT
				LN90.BF_SSN,
				LN90.LN_SEQ,
				MAX(LN90.LD_FAT_EFF) AS MaxTranConsol
				FROM 
					UDW..LN90_FIN_ATY LN90
					
				WHERE
				(
					(LN90.PC_FAT_TYP  IN ('10', '20', '50', '60') AND LN90.PC_FAT_SUB_TYP IN ('70','80')) -- Consolidation
				)
				AND LN90.LC_STA_LON90 = 'A'
				AND RTRIM(COALESCE(LN90.LC_FAT_REV_REA,'')) = ''
				GROUP BY
					LN90.BF_SSN,
					LN90.LN_SEQ
		) MaxDateConsol
			ON MaxDateConsol.BF_SSN = WQ20.BF_SSN
			AND MaxDateConsol.LN_SEQ = LN10.LN_SEQ

	WHERE 
			WQ20.WF_QUE = 'R9'
			AND WQ20.WF_SUB_QUE IN ('01','02')
			AND WQ20.WC_STA_WQUE20 = 'U'
			AND WQ20.WD_ACT_REQ < DATEADD(DAY, -14, GETDATE()) --Only process tasks after the task is two weeks old
			

/*
Now using data from temp table @UnprocessedTasksTemp 
to determine which tasks are consols, cancels, LetterId, 
and checking for dupes before inserting into PQ
*/

DECLARE @UnprocessedTasks TABLE (WF_QUE VARCHAR(2), WF_SUB_QUE VARCHAR(2), WN_CTL_TSK VARCHAR(18), PF_REQ_ACT VARCHAR(5), LnSeq VARCHAR(4), IC_LON_PGM VARCHAR(6), LD_LON_1_DSB DATE, LA_CUR_PRI NUMERIC(8,2), OriginalBalance NUMERIC(8,2), WD_ACT_REQ DATE, Ssn VARCHAR(9), AccountNumber VARCHAR(10), MaxTranPif DATE, MaxTranConsol DATE, MaxTranClc DATE, IsConsol BIT, IsCancel BIT, TranPriority INT)			
INSERT INTO @UnprocessedTasks (WF_QUE, WF_SUB_QUE, WN_CTL_TSK, PF_REQ_ACT, LnSeq, IC_LON_PGM, LD_LON_1_DSB, LA_CUR_PRI, OriginalBalance, WD_ACT_REQ, Ssn, AccountNumber, MaxTranPif, MaxTranConsol, MaxTranClc, IsConsol, IsCancel, TranPriority)
SELECT
	UTT.WF_QUE,
	UTT.WF_SUB_QUE,
	UTT.WN_CTL_TSK,
	UTT.PF_REQ_ACT,
	UTT.LnSeq,
	UTT.IC_LON_PGM,
	LN10.LD_LON_1_DSB,
	LN10.LA_CUR_PRI,
	UTT.OriginalBalance,
	UTT.WD_ACT_REQ,
	UTT.Ssn,
	UTT.AccountNumber,
	UTT.MaxTranPif,
	UTT.MaxTranConsol,
	UTT.MaxTranClc,
	CASE 
		WHEN (ISNULL(UTT.MaxTranConsol,'1900-01-01') > ISNULL(UTT.MaxTranPif,'1900-01-01') AND ISNULL(UTT.MaxTranConsol,'1900-01-01') > ISNULL(UTT.MaxTranClc,'1900-01-01')) THEN 1
		ELSE 0
	END AS IsConsol,
	CASE
		WHEN (UTT.MaxTranPif IS NULL AND UTT.MaxTranConsol IS NULL AND UTT.MaxTranClc IS NULL) THEN 1
		WHEN (ISNULL(UTT.MaxTranClc,'1900-01-01') >= ISNULL(UTT.MaxTranPif,'1900-01-01') AND ISNULL(UTT.MaxTranClc,'1900-01-01') >= ISNULL(UTT.MaxTranConsol,'1900-01-01')) THEN 1 --SLIGHTLY DIFF FROM VBA LOGIC, AS 40 & 45 TRANS POSTDATING OTHER TRANS WILL CANCEL TASK
		WHEN LN10.LA_CUR_PRI != 0 THEN 1
		ELSE 0
	END AS IsCancel,
	ROW_NUMBER() OVER (PARTITION BY SORT.WN_CTL_TSK ORDER BY SORT.PriorityNumber ASC) AS TranPriority
	FROM 
		@UnprocessedTasksTemp UTT
		INNER JOIN
		(
			SELECT
				WQ20.BF_SSN [Ssn],
				WQ20.WN_CTL_TSK,
				TaskTran.LN_SEQ [LnSeq],
				CASE
					WHEN TaskTran.PC_FAT_TYP IN ('10') AND TaskTran.PC_FAT_SUB_TYP IN ('40','45') THEN 1 -- Cancel tasks take priority
					WHEN TaskTran.PC_FAT_TYP IN ('10', '20', '50', '60') AND TaskTran.PC_FAT_SUB_TYP IN ('70','80') THEN 2 -- Consol tasks (if a loan has both pif & consol, consol is more recent)
					WHEN TaskTran.PC_FAT_TYP IN ('50', '60') AND TaskTran.PC_FAT_SUB_TYP IN ('02','03') THEN 3
					WHEN TaskTran.PC_FAT_TYP IN ('20') AND TaskTran.PC_FAT_SUB_TYP IN ('10') THEN 3
					ELSE 4
				END AS PriorityNumber
			FROM
				UDW..WQ20_TSK_QUE WQ20
			LEFT JOIN
			(
				SELECT
					LN90.BF_SSN,
					LN90.LN_SEQ,
					LN90.PC_FAT_SUB_TYP,
					LN90.PC_FAT_TYP,
					LN90.LC_STA_LON90,
					LN90.LC_FAT_REV_REA
				FROM
					UDW..LN90_FIN_ATY LN90
				WHERE
				(
					(LN90.PC_FAT_TYP = '10' AND LN90.PC_FAT_SUB_TYP IN ('10','20','35','36','40','45','41','50','54','70','80'))
					OR (LN90.PC_FAT_TYP = '20' AND LN90.PC_FAT_SUB_TYP IN ('10', '70', '80'))
					OR (LN90.PC_FAT_TYP IN ('50','60') AND LN90.PC_FAT_SUB_TYP IN ('02', '03'))
				)
			) TaskTran
				ON TaskTran.BF_SSN = WQ20.BF_SSN
				--AND TaskTran.LN_SEQ = CAST(CAST(SUBSTRING(WQ20.WN_CTL_TSK, 10,4) AS INTEGER) AS VARCHAR)
				AND REPLACE(STR( TaskTran.LN_SEQ, 4), ' ', '0') = SUBSTRING(WQ20.WN_CTL_TSK, 10,4)
				AND TaskTran.LC_STA_LON90 ='A'
				AND RTRIM(COALESCE(TaskTran.LC_FAT_REV_REA,'')) = ''
			WHERE WQ20.WF_QUE = 'R9'
				AND WQ20.WF_SUB_QUE IN ('01','02')
				AND WQ20.WC_STA_WQUE20 = 'U'
		) SORT
			ON SORT.WN_CTL_TSK = UTT.WN_CTL_TSK
		INNER JOIN UDW..LN10_LON LN10
			ON LN10.LN_SEQ = UTT.LnSeq
			AND LN10.BF_SSN = UTT.Ssn
	

DECLARE @PIFCLLTR INT = --153
	(
		SELECT  
			ScriptDataId
		FROM 
			ULS.[print].Letters LT
			INNER JOIN ULS.[print].ScriptData SD
				ON SD.LetterId = LT.LetterId
		WHERE 
			LT.Letter = ('PIFCLLTR')
	)

DECLARE @PIFCLLTRLetterID INT = --78
	(
		SELECT  
			LetterId
		FROM 
			ULS.[print].Letters LT
		WHERE 
			LT.Letter = ('PIFCLLTR')
	)

DECLARE @PIFLTR INT = --151
	(
		SELECT  
			ScriptDataId 
		FROM 
			ULS.[print].Letters LT
			INNER JOIN ULS.[print].ScriptData SD
				ON SD.LetterId = LT.LetterId
		WHERE 
			LT.Letter = ('PIFLTR')
	)

DECLARE @PIFLTRLetterID INT = --77
	(
		SELECT  
			LetterId
		FROM 
			ULS.[print].Letters LT
		WHERE 
			LT.Letter = ('PIFLTR')
	)
		
INSERT INTO [Uls].[pifltr].[ProcessingQueue]([Queue],SubQueue,TaskControlNumber,RequestArc, TaskRequestedDate, Ssn, CoBorrowerSsn,AccountNumber, LoanSeq, FirstDisbursementDate, OriginalBalance, IsConsolPif,IsCanceled,ScriptDataId,AddedAt,AddedBy)
SELECT DISTINCT
	UT.WF_QUE AS [Queue],
	UT.WF_SUB_QUE AS SubQueue,
	UT.WN_CTL_TSK AS TaskControlNumber,
	UT.PF_REQ_ACT AS RequestArc,
	UT.WD_ACT_REQ AS TaskRequestedDate,
	UT.Ssn,
	LN20.LF_EDS AS CoBorrowerSsn,
	UT.AccountNumber,
	UT.LnSeq [LoanSeq],
	UT.LD_LON_1_DSB AS FirstDisbursementDate,
	UT.OriginalBalance AS OriginalBalance,
	UT.IsConsol AS IsConsolPif,
	UT.IsCancel AS IsCanceled,
	CASE 
		WHEN UT.IsConsol = 1 THEN @PIFCLLTR
		WHEN UT.IsConsol = 0 THEN @PIFLTR
		ELSE NULL 
	END AS ScriptDataId,
	GETDATE() AS AddedAt,
	SUSER_NAME() AS AddedBy
	FROM 
		@UnprocessedTasks UT
		LEFT  JOIN [UDW].[dbo].[LN20_EDS] LN20
			ON LN20.BF_SSN = UT.Ssn
			AND LN20.LN_SEQ = UT.LnSeq
			AND LN20.LC_EDS_TYP = 'M'
			AND LN20.LC_STA_LON20 = 'A'
		LEFT JOIN ULS.pifltr.ProcessingQueue PQ
			ON PQ.TaskControlNumber = UT.WN_CTL_TSK 
			AND PQ.TaskRequestedDate = UT.WD_ACT_REQ 
	WHERE
		UT.TranPriority = 1
		AND (PQ.TaskControlNumber IS NULL) --Don't allow duplicate tasks that have the same create date


	SELECT DISTINCT
		[Queue],
		SubQueue,
		TaskControlNumber,
		RequestArc,
		TaskRequestedDate,
		UT.Ssn,
		CoBorrowerSsn,
		UT.AccountNumber,
		LoanSeq,
		FT.[Label] AS LoanProgram,
		CASE 
			WHEN IsConsolPif = 1 THEN UT.MaxTranConsol
			WHEN IsConsolPif = 0 THEN UT.MaxTranPif
			ELSE NULL
		END AS EffectiveDate,
		FirstDisbursementDate,
		PQ.OriginalBalance,
		ScriptDataId,
		IsConsolPif,
		IsCanceled,
		ScriptDataId,
		CASE 
			WHEN ScriptDataId = @PIFCLLTR THEN @PIFCLLTRLetterID
			WHEN ScriptDataId = @PIFLTR THEN @PIFLTRLetterID
			ELSE NULL 
		END AS LetterId,
		PrintProcessingId,
		CoBwrPrintProcessingId,
		ProcessQueueId
	 FROM 
		ULS.pifltr.ProcessingQueue PQ
		INNER JOIN @UnprocessedTasks UT
			ON TaskControlNumber = UT.WN_CTL_TSK
		INNER JOIN [UDW].[dbo].[FormatTranslation] FT
			ON FT.[Start] = UT.IC_LON_PGM
	 WHERE 
		ProcessedAt IS NULL
		AND DeletedAt IS NULL

RETURN 0
