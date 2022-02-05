CREATE PROCEDURE [batchesp].[AddNewWorkToTables]
AS

	SET NOCOUNT ON;

DROP TABLE IF EXISTS #WQ20_Pop

CREATE TABLE #WQ20_Pop (BF_SSN CHAR(9), WF_QUE VARCHAR(2), WF_SUB_QUE VARCHAR(2), WN_CTL_TSK VARCHAR(18), PF_REQ_ACT VARCHAR(5), WD_ACT_REQ DATE, WX_MSG_1_TSK VARCHAR(77), WX_MSG_2_TSK VARCHAR(77), WC_STA_WQUE20 CHAR(1), WF_LST_DTS_WQ20 DATETIME2(7), WF_CRT_DTS_WQ20 DATETIME2(7))
INSERT INTO #WQ20_Pop
SELECT
	BF_SSN, 
	WF_QUE, 
	WF_SUB_QUE, 
	WN_CTL_TSK, 
	PF_REQ_ACT, 
	WD_ACT_REQ, 
	WX_MSG_1_TSK, 
	WX_MSG_2_TSK, 
	WC_STA_WQUE20, 
	WF_LST_DTS_WQ20, 
	WF_CRT_DTS_WQ20
FROM
	UDW..WQ20_TSK_QUE WQ20
WHERE
	WQ20.WF_QUE = 'RB'
		AND WQ20.WF_SUB_QUE IN ('00', '01', '02', '05', '08', '10', '11', '18', '22')
		AND WQ20.WC_STA_WQUE20 NOT IN ('X', 'C', 'P', 'H', 'A')
		AND (
				WQ20.WX_MSG_1_TSK LIKE 'POSSIBLE UPDATE REQUIRED-PULL NSLDS FOR ADDITIONAL INFO%'
				OR WQ20.WX_MSG_1_TSK LIKE 'DIFFERENT SCHOOLS - POSSIBLE SEP DATE CHANGE%'
				OR WQ20.WX_MSG_1_TSK LIKE 'DEFER SCHOOL NOT EQUAL INCOMING SCHOOL/REVIEW DEFER%'
				OR WQ20.WX_MSG_1_TSK LIKE 'EQUAL OR NEWER CERT EXISTS FOR INCOMING SCHL - REVIEW DEFER DATES%'
				OR WQ20.WX_MSG_1_TSK LIKE 'REPURCHASED OR REHABILITATED LOAN-MANUAL REVIEW REQUIRED%'
				OR WQ20.WX_MSG_1_TSK LIKE 'OTHER PLUS LOANS EXIST FOR THIS BORROWER-REVIEW ALL LOANS FOR DEFER%'
				OR WQ20.WX_MSG_1_TSK LIKE 'INVALID REPAY START DATE IN REPAY SCHD ROUTINE%'
				OR WQ20.WX_MSG_1_TSK LIKE 'SAME CERT AND SEP DTD INCOMING REASON DIFFERENT - PLEASE REVIEW%'
				OR WQ20.WX_MSG_1_TSK LIKE 'OVERLAP WITH ANOTHER DEFERMENT, PDG, OR FORBEARANCE-UPDATE MANUALLY%'
				OR WQ20.WX_MSG_1_TSK LIKE 'SCHOOL FORB OR DEFER EXISTS > INCOMING SEP - OLDER CERT DT%' 
				OR WQ20.WX_MSG_1_TSK LIKE 'SAME CERT DATE RECEIVED FOR MULTIPLE SCHOOL CAMPUS CODES%'
				OR WQ20.WX_MSG_1_TSK LIKE 'DEFERMENTS EXIST - REVIEW LOAN%'
				OR WQ20.WX_MSG_1_TSK LIKE 'SCHOOL TRANSFER NEW CERT L/T CURRENT CERT DATE PLUS TOLERANCE%'
				OR WQ20.WX_MSG_1_TSK LIKE 'STUDENT NOT EQUAL BORROWER - REVIEW DEFER ELIGIBILITY%'
				OR WQ20.WX_MSG_1_TSK LIKE 'OTHER PLUS DEFER EXIST FOR THIS BORROWER - REVIEW FOR DEFER TYPE%'
				OR WQ20.WX_MSG_1_TSK LIKE 'INCOMING DATE MAY QUALIFY FOR BRIDGE DEFERMENT%'    
				OR WQ20.WX_MSG_1_TSK LIKE 'CERT DTE EQUAL-CONFLICT BETWEEN INCOMING ENROL AND EXISTING DFR%'
				OR WQ20.WX_MSG_1_TSK LIKE 'DEFER CHANGE NEEDED - REVIEW EXISTING ALIGN REPAY FORB%'
				OR WQ20.WX_MSG_1_TSK LIKE 'SCHL DEFR EXISTS WITH DIFFERENT STATUS THAN INCOMING RECORD%'
				OR WQ20.WX_MSG_1_TSK LIKE 'OTHER DEFERMENT TYPE EXISTS DURING INCOMING ENROLLMENT-REVIEW%'
				OR WQ20.WX_MSG_1_TSK LIKE 'ALIGN REPAYMENT FORB OVERLAPS FORB/DEFER%'
				OR WQ20.WX_MSG_1_TSK LIKE 'DEFER SCHOOL NOT EQUAL INCOMING SCHOOL/REVIEW DEFER%'
				OR WQ20.WX_MSG_1_TSK LIKE 'DEFER/FORB TO BE INACTIVATED%'
			)

SET NOCOUNT OFF;

PRINT 'Beginning merge process'

MERGE 
	[batchesp].EspEnrollments AS T 
USING 
	(
		SELECT DISTINCT
			SourcePop.BorrowerSSN,
			SourcePop.AccountNumber,
			SourcePop.[Queue],
			SourcePop.SubQueue,
			SourcePop.TaskControlNumber,
			SourcePop.Arc,
			SourcePop.ArcRequestDate,
			SourcePop.Message1,
			SourcePop.SupplementalMessage,
			SourcePop.StudentSSN,
			SourcePop.StudentSSN2,
			SourcePop.SchoolCode,
			SourcePop.ESP_Status,
			SourcePop.ESP_SeparationDate,
			SourcePop.ESP_CertificationDate,
			SourcePop.EnrollmentBeginDate,
			SourcePop.SourceCode
		FROM
		(
			SELECT
					WQ20.BF_SSN AS BorrowerSSN,
					PD10.DF_SPE_ACC_ID AS AccountNumber,
					WQ20.WF_QUE AS Queue,
					WQ20.WF_SUB_QUE AS SubQueue,
					WQ20.WN_CTL_TSK AS TaskControlNumber,
					WQ20.PF_REQ_ACT AS Arc,
					WQ20.WD_ACT_REQ AS ArcRequestDate,
					WQ20.WX_MSG_1_TSK AS Message1,
					WQ20.WX_MSG_2_TSK AS SupplementalMessage,
					SUBSTRING(WQ20.WX_MSG_2_TSK,1,9) AS StudentSSN,
					SUBSTRING(WQ20.WX_MSG_2_TSK,10,9) AS StudentSSN2,
					SUBSTRING(WQ20.WX_MSG_2_TSK,19,8) AS SchoolCode,
					SUBSTRING(WQ20.WX_MSG_2_TSK,27,2) AS ESP_Status,
					CASE WHEN ISDATE(SUBSTRING(WQ20.WX_MSG_2_TSK,29,8)) = 0
						THEN NULL
						ELSE SUBSTRING(WQ20.WX_MSG_2_TSK,29,8)
					END AS ESP_SeparationDate,
					SUBSTRING(WQ20.WX_MSG_2_TSK,37,8) AS ESP_CertificationDate,
					SUBSTRING(WQ20.WX_MSG_2_TSK,45,8) AS EnrollmentBeginDate,
					SUBSTRING(WQ20.WX_MSG_2_TSK,53,2) AS SourceCode,
					WQ20.WF_LST_DTS_WQ20 AS UpdatedAt,
					ROW_NUMBER() OVER(PARTITION BY WQ20.BF_SSN, WQ20.WF_QUE ORDER BY WQ20.WF_CRT_DTS_WQ20) AS OLDEST
				FROM
					#WQ20_Pop WQ20
					INNER JOIN UDW..PD10_PRS_NME PD10
						ON WQ20.BF_SSN = PD10.DF_PRS_ID
					INNER JOIN UDW..LN10_LON LN10
						ON LN10.BF_SSN = WQ20.BF_SSN
						AND LN10.LA_CUR_PRI > 0
		) SourcePop
		WHERE SourcePop.OLDEST = 1
	) S
		ON T.BorrowerSSN = S.BorrowerSSN
		AND T.[Queue] = S.[Queue]
		AND T.Subqueue = S.Subqueue
		AND T.TaskControlNumber = S.TaskControlNumber
		AND (T.ProcessedAt IS NULL OR CAST(T.CreatedAt AS DATE) = CAST(GETDATE() AS DATE)) --Use ON criteria to prevent dupe tasks
WHEN NOT MATCHED BY TARGET THEN
	INSERT
	(
		BorrowerSSN,
		AccountNumber,
		[Queue],
		SubQueue,
		TaskControlNumber,
		Arc,
		ArcRequestDate,
		Message1,
		SupplementalMessage,
		StudentSSN,
		StudentSSN2,
		SchoolCode,
		ESP_Status,
		ESP_SeparationDate,
		ESP_CertificationDate,
		EnrollmentBeginDate,
		SourceCode,
		CreatedAt,
		CreatedBy,
		UpdatedAt
	)
	VALUES
	(
		S.BorrowerSSN,
		S.AccountNumber,
		S.[Queue],
		S.SubQueue,
		S.TaskControlNumber,
		S.Arc,
		S.ArcRequestDate,
		S.Message1,
		S.SupplementalMessage,
		S.StudentSSN,
		S.StudentSSN2,
		S.SchoolCode,
		S.ESP_Status,
		S.ESP_SeparationDate,
		S.ESP_CertificationDate,
		S.EnrollmentBeginDate,
		S.SourceCode,
		GETDATE(),
		'dbo',
		GETDATE()
	);

PRINT 'Merged the EspEnrollments table'

MERGE 
	[batchesp].TS01Enrollments AS T 
USING 
	(
		SELECT DISTINCT
			SourcePop.BorrowerSSN,
			SourcePop.LoanSequence,
			SourcePop.StudentSSN,
			SourcePop.SeparationDate,
			SourcePop.SchoolCode,
			SourcePop.SeparationReason,
			SourcePop.SeparationSource,
			SourcePop.DateNotified,
			SourcePop.DateCertified
		FROM
		(
			SELECT DISTINCT
				LN10.BF_SSN AS BorrowerSSN,
				LN13.LN_SEQ AS LoanSequence,
				LN10.LF_STU_SSN AS StudentSSN,
				SD10.LD_SCL_SPR AS SeparationDate,
				SD10.LF_DOE_SCL_ENR_CUR AS SchoolCode,
				SD10.LC_REA_SCL_SPR AS SeparationReason,
				SD10.LC_SCR_SCL_SPR AS SeparationSource,
				COALESCE(SD10.LD_NTF_SCL_SPR, LN10.LD_LON_ACL_ADD) AS DateNotified,
				COALESCE(SD10.LD_SCL_CER_STU_STA, LN10.LD_LON_ACL_ADD) AS DateCertified, 
				RANK() OVER(PARTITION BY WQ20.BF_SSN, LN13.LN_SEQ ORDER BY WQ20.WF_CRT_DTS_WQ20) AS OLDEST
			FROM
				#WQ20_Pop WQ20
				INNER JOIN UDW..LN10_LON LN10
					ON LN10.BF_SSN = WQ20.BF_SSN
					AND LN10.LA_CUR_PRI > 0
				INNER JOIN UDW..PD10_PRS_NME PD10
					ON PD10.DF_PRS_ID = LN10.BF_SSN
				INNER JOIN UDW..LN13_LON_STU_OSD LN13
					ON LN10.BF_SSN = LN13.BF_SSN
					AND LN10.LN_SEQ = LN13.LN_SEQ
					AND LN13.LC_STA_LON13 = 'A'
				INNER JOIN UDW..SD10_STU_SPR SD10
					ON LN13.LF_STU_SSN = SD10.LF_STU_SSN
					AND LN13.LN_STU_SPR_SEQ = SD10.LN_STU_SPR_SEQ
					AND SD10.LC_STA_STU10 = 'A'
				LEFT JOIN ULS.quecomplet.Queues QC
					ON QC.[Queue] = WQ20.WF_QUE
					AND QC.[SubQueue] = WQ20.WF_SUB_QUE
					AND QC.AccountIdentifier = PD10.DF_SPE_ACC_ID
					AND QC.TaskControlNumber = WQ20.WN_CTL_TSK
					AND QC.DeletedAt IS NULL
					AND COALESCE(CAST(QC.ProcessedAt AS DATE),CAST(GETDATE() AS DATE)) > CAST(DATEADD(DAY,-1,GETDATE()) AS DATE) --Either not closed, or closed within last day but might not be removed from WQ20 table yet
				WHERE
					QC.AccountIdentifier IS NULL --Not waiting on QUECOMPLET to close task
		) SourcePop
		WHERE SourcePop.OLDEST = '1' --We only want one task at a time, and we want to do the oldest one first
	) S
		ON T.BorrowerSSN = S.BorrowerSSN
		AND T.LoanSequence = S.LoanSequence
		AND (T.ProcessedAt IS NULL OR CAST(T.CreatedAt AS DATE) = CAST(GETDATE() AS DATE)) --Use ON criteria to prevent dupe tasks
WHEN NOT MATCHED BY TARGET THEN
	INSERT
	(
		BorrowerSSN, 
		LoanSequence, 
		StudentSSN, 
		SeparationDate, 
		SchoolCode, 
		SeparationReason, 
		SeparationSource, 
		DateNotified, 
		DateCertified,
		CreatedAt,
		CreatedBy,
		UpdatedAt
	)
	VALUES
	(
		S.BorrowerSSN, 
		S.LoanSequence, 
		S.StudentSSN, 
		S.SeparationDate, 
		S.SchoolCode, 
		S.SeparationReason, 
		S.SeparationSource, 
		S.DateNotified, 
		S.DateCertified,
		GETDATE(),
		'BATCHESP_Init_Pull',
		GETDATE()
	)
WHEN MATCHED THEN UPDATE SET
	T.StudentSSN = S.StudentSSN,
	T.SeparationDate = S.SeparationDate,
	T.SchoolCode = S.SchoolCode, 
	T.SeparationReason = S.SeparationReason, 
	T.SeparationSource = S.SeparationSource, 
	T.DateNotified = S.DateNotified, 
	T.DateCertified = S.DateCertified,
	T.CreatedBy = 'BATCHESP_Merge_Update',
	T.UpdatedAt = GETDATE();

PRINT 'Merged the TS01Enrollments table'

MERGE 
	[batchesp].TSAYDefermentForbearances AS T 
USING 
	(
		SELECT DISTINCT
			SourcePop.BorrowerSSN,
			SourcePop.LoanSequence,
			SourcePop.[Type],
			SourcePop.BeginDate,
			SourcePop.EndDate,
			SourcePop.CertificationDate,
			SourcePop.DeferSchool,
			SourcePop.RequestedBeginDate,
			SourcePop.RequestedEndDate
		FROM
		(		
			SELECT DISTINCT
					LN10.BF_SSN AS BorrowerSSN,
					LN10.LN_SEQ AS LoanSequence,
					'D' + DF10.LC_DFR_TYP AS [Type],
					ISNULL(LN50.LD_DFR_BEG,'9999-12-31') AS BeginDate,
					ISNULL(LN50.LD_DFR_END,'9999-12-31') AS EndDate,
					COALESCE(DF10.LD_DFR_INF_CER, LN50.LD_DFR_APL) AS CertificationDate,
					DF10.LF_DOE_SCL_DFR AS DeferSchool,
					ISNULL(DF10.LD_DFR_REQ_BEG, '9999-12-31') AS RequestedBeginDate,
					ISNULL(DF10.LD_DFR_REQ_END, '9999-12-31') AS RequestedEndDate,
					ROW_NUMBER() OVER(PARTITION BY LN50.BF_SSN, LN50.LN_SEQ ORDER BY LN50.LD_DFR_END) AS OLDEST --We want the earliest end date def in the applicable range
				FROM
					#WQ20_Pop WQ20
					INNER JOIN UDW..LN10_LON LN10
						ON LN10.BF_SSN = WQ20.BF_SSN
						AND LN10.LA_CUR_PRI > 0
					INNER JOIN UDW..PD10_PRS_NME PD10
						ON PD10.DF_PRS_ID = LN10.BF_SSN
					INNER JOIN ULS.batchesp.EspEnrollments ESP
						ON ESP.BorrowerSsn = PD10.DF_PRS_ID 
						AND ESP.ProcessedAt IS NULL
					INNER JOIN UDW..LN50_BR_DFR_APV LN50
						ON LN10.BF_SSN = LN50.BF_SSN
						AND LN10.LN_SEQ = LN50.LN_SEQ
						AND LN50.LC_STA_LON50 = 'A'
						AND LN50.LC_DFR_RSP != '003'
						AND LN50.LD_DFR_END >= ESP.EnrollmentBeginDate
					INNER JOIN UDW..DF10_BR_DFR_REQ DF10
						ON LN50.BF_SSN = DF10.BF_SSN
						AND LN50.LF_DFR_CTL_NUM = DF10.LF_DFR_CTL_NUM
						AND DF10.LC_DFR_STA = 'A'
						AND DF10.LC_STA_DFR10 = 'A'
						AND DF10.LC_DFR_TYP IN ('15', '16', '18', '45')
					LEFT JOIN ULS.quecomplet.Queues QC
						ON QC.[Queue] = WQ20.WF_QUE
						AND QC.[SubQueue] = WQ20.WF_SUB_QUE
						AND QC.AccountIdentifier = PD10.DF_SPE_ACC_ID
						AND QC.TaskControlNumber = WQ20.WN_CTL_TSK
						AND QC.DeletedAt IS NULL
						AND COALESCE(CAST(QC.ProcessedAt AS DATE),CAST(GETDATE() AS DATE)) > CAST(DATEADD(DAY,-1,GETDATE()) AS DATE) --Either not closed, or closed within last day but might not be removed from WQ20 table yet
				WHERE
					QC.AccountIdentifier IS NULL --Not waiting on QUECOMPLET to close task
			) SourcePop
			WHERE SourcePop.OLDEST = 1
		) S
			ON T.BorrowerSSN = S.BorrowerSSN
			AND T.LoanSequence = S.LoanSequence
			AND T.DeferSchool = S.DeferSchool
			AND T.RequestedBeginDate = S.RequestedBeginDate
			AND T.RequestedEndDate = S.RequestedEndDate
			AND (T.ProcessedAt IS NULL OR CAST(T.CreatedAt AS DATE) = CAST(GETDATE() AS DATE)) --Use ON criteria to prevent dupe tasks.
WHEN NOT MATCHED BY TARGET THEN
	INSERT
	(
		BorrowerSSN,
		LoanSequence,
		[Type],
		BeginDate,
		EndDate,
		CertificationDate,
		DeferSchool,
		RequestedBeginDate,
		RequestedEndDate,
		CreatedAt,
		CreatedBy,
		UpdatedAt
	)
	VALUES
	(
		S.BorrowerSSN, 
		S.LoanSequence, 
		S.[Type],
		S.BeginDate,
		S.EndDate,
		S.CertificationDate,
		S.DeferSchool,
		S.RequestedBeginDate,
		S.RequestedEndDate,
		GETDATE(),
		'BATCHESP_Init_Pull',
		GETDATE()
	)
WHEN MATCHED THEN UPDATE SET
	T.[Type] = S.[Type],
	T.BeginDate = S.BeginDate,
	T.EndDate = S.EndDate,
	T.CertificationDate = S.CertificationDate,
	T.DeferSchool = S.DeferSchool,
	T.RequestedBeginDate = S.RequestedBeginDate,
	T.RequestedEndDate = S.RequestedEndDate,
	T.CreatedBy = 'BATCHESP_Merge_Update',
	T.UpdatedAt = GETDATE();

PRINT 'Merged the TSAYDefermentForbearances table'

MERGE 
	[batchesp].TS26LoanInformation AS T 
USING 
	(
		SELECT DISTINCT
			SourcePop.BorrowerSSN,
			SourcePop.LoanSequence,
			SourcePop.LoanProgramType,
			SourcePop.CurrentPrincipal,
			SourcePop.DisbursementDate,
			SourcePop.GraceEndDate,
			SourcePop.RepaymentStartDate,
			SourcePop.EffectAddDate,
			SourcePop.RehabRepurch,
			SourcePop.TermBeg,
			SourcePop.TermEnd
		FROM
		(		
			SELECT DISTINCT
					LN10.BF_SSN AS BorrowerSSN,
					LN10.LN_SEQ AS LoanSequence,
					LN10.IC_LON_PGM AS LoanProgramType,
					LN10.LA_CUR_PRI AS CurrentPrincipal,
					LN10.LD_LON_1_DSB AS DisbursementDate,
					COALESCE(LN10.LD_END_GRC_PRD, LN10.LD_LON_1_DSB) AS GraceEndDate,
					CASE WHEN LN10_MAX.LD_END_GRC_PRD > LN10.LD_LON_1_DSB
						THEN LN10_MAX.LD_END_GRC_PRD
						ELSE LN10.LD_LON_1_DSB
					END AS RepaymentStartDate,
					LN10.LD_LON_EFF_ADD AS EffectAddDate,
					LN10.LC_RPR_TYP AS RehabRepurch,
					LN10.LD_TRM_BEG AS TermBeg,
					LN10.LD_TRM_END AS TermEnd,
					RANK() OVER(PARTITION BY WQ20.BF_SSN, LN10.LN_SEQ ORDER BY WQ20.WF_CRT_DTS_WQ20) AS OLDEST
				FROM
					#WQ20_Pop WQ20
					INNER JOIN UDW..LN10_LON LN10
						ON WQ20.BF_SSN = LN10.BF_SSN
						AND LN10.LA_CUR_PRI > '0'
						AND LN10.LC_STA_LON10 = 'R'
					INNER JOIN UDW..PD10_PRS_NME PD10
						ON PD10.DF_PRS_ID = LN10.BF_SSN
					INNER JOIN 
					(
						SELECT DISTINCT
							LN10.BF_SSN,
							LN10.LN_SEQ,
							DATEADD(DAY,1, MAX(LN10.LD_END_GRC_PRD)) AS LD_END_GRC_PRD
						FROM
							UDW..LN10_LON LN10
						GROUP BY
							LN10.BF_SSN,
							LN10.LN_SEQ
					) LN10_MAX
						ON LN10.BF_SSN = LN10_MAX.BF_SSN
						AND LN10.LN_SEQ = LN10_MAX.LN_SEQ
					LEFT JOIN ULS.quecomplet.Queues QC
						ON QC.[Queue] = WQ20.WF_QUE
						AND QC.[SubQueue] = WQ20.WF_SUB_QUE
						AND QC.AccountIdentifier = PD10.DF_SPE_ACC_ID
						AND QC.TaskControlNumber = WQ20.WN_CTL_TSK
						AND QC.DeletedAt IS NULL
						AND COALESCE(CAST(QC.ProcessedAt AS DATE),CAST(GETDATE() AS DATE)) > CAST(DATEADD(DAY,-1,GETDATE()) AS DATE) --Either not closed, or closed within last day but might not be removed from WQ20 table yet
				WHERE
					QC.AccountIdentifier IS NULL --Not waiting on QUECOMPLET to close task
			) SourcePop
			WHERE SourcePop.OLDEST = 1
		) S
			ON T.BorrowerSSN = S.BorrowerSSN
			AND T.LoanSequence = S.LoanSequence
			AND (T.ProcessedAt IS NULL OR CAST(T.CreatedAt AS DATE) = CAST(GETDATE() AS DATE)) --Use ON criteria to prevent dupe tasks.
WHEN NOT MATCHED BY TARGET THEN
	INSERT
	(
		BorrowerSSN,
		LoanSequence,
		LoanProgramType,
		CurrentPrincipal,
		DisbursementDate,
		GraceEndDate,
		RepaymentStartDate,
		EffectAddDate,
		RehabRepurch,
		TermBeg,
		TermEnd,
		CreatedAt,
		CreatedBy,
		UpdatedAt
	)
	VALUES
	(
		S.BorrowerSSN, 
		S.LoanSequence, 
		S.LoanProgramType,
		S.CurrentPrincipal,
		S.DisbursementDate,
		S.GraceEndDate,
		S.RepaymentStartDate,
		S.EffectAddDate,
		S.RehabRepurch,
		S.TermBeg,
		S.TermEnd,
		GETDATE(),
		'BATCHESP_Init_Pull',
		GETDATE()
	)
WHEN MATCHED THEN UPDATE SET
	T.CurrentPrincipal = S.CurrentPrincipal,
	T.GraceEndDate = S.GraceEndDate,
	T.RepaymentStartDate = S.RepaymentStartDate,
	T.CreatedBy = 'BATCHESP_Merge_Update',
	T.UpdatedAt = GETDATE();

PRINT 'Merged the TS26LoanInformation table'

MERGE 
	[batchesp].TS2HPendingDisbursements AS T 
USING 
	(
		SELECT DISTINCT
			SourcePop.BorrowerSSN,
			SourcePop.LoanSequence,
			SourcePop.DisbSequence,
			SourcePop.DisbType,
			SourcePop.DisbursementDate
		FROM
		(		
			SELECT DISTINCT
				LN15.BF_SSN AS BorrowerSSN,
				LN15.LN_SEQ AS LoanSequence,
				LN15.LN_BR_DSB_SEQ AS DisbSequence,
				CASE WHEN LN15.LA_DSB_CAN = LN15.LA_DSB	THEN 'Cancelled'
						WHEN LN15.LC_DSB_TYP = 1 THEN 'Anticipated'
						WHEN LN15.LC_DSB_TYP = 2 THEN 'Actual'
						ELSE 'ERROR'
				END AS DisbType,
				LN15.LD_DSB AS DisbursementDate,
				RANK() OVER(PARTITION BY WQ20.BF_SSN, LN10.LN_SEQ ORDER BY WQ20.WF_CRT_DTS_WQ20) AS OLDEST
			FROM
				UDW..PD10_PRS_NME PD10
				INNER JOIN UDW..LN10_LON LN10
					ON LN10.BF_SSN = PD10.DF_PRS_ID
					AND LN10.LA_CUR_PRI > 0
				INNER JOIN UDW..LN15_DSB LN15
					ON LN10.BF_SSN = LN15.BF_SSN
					AND LN10.LN_SEQ = LN15.LN_SEQ
				INNER JOIN #WQ20_Pop WQ20
					ON LN15.BF_SSN = WQ20.BF_SSN
				LEFT JOIN ULS.quecomplet.Queues QC
					ON QC.[Queue] = WQ20.WF_QUE
					AND QC.[SubQueue] = WQ20.WF_SUB_QUE
					AND QC.AccountIdentifier = PD10.DF_SPE_ACC_ID
					AND QC.TaskControlNumber = WQ20.WN_CTL_TSK
					AND QC.DeletedAt IS NULL
					AND COALESCE(CAST(QC.ProcessedAt AS DATE),CAST(GETDATE() AS DATE)) > CAST(DATEADD(DAY,-1,GETDATE()) AS DATE) --Either not closed, or closed within last day but might not be removed from WQ20 table yet
				WHERE
					QC.AccountIdentifier IS NULL --Not waiting on QUECOMPLET to close task
			) SourcePop
			WHERE SourcePop.OLDEST = 1
		) S
			ON T.BorrowerSSN = S.BorrowerSSN
			AND T.LoanSequence = S.LoanSequence
			AND T.DisbSequence = S.DisbSequence
			AND (T.ProcessedAt IS NULL OR CAST(T.CreatedAt AS DATE) = CAST(GETDATE() AS DATE)) --Use ON criteria to prevent dupe tasks.
WHEN NOT MATCHED BY TARGET THEN
	INSERT
	(
		BorrowerSSN,
		LoanSequence,
		DisbSequence,
		DisbType,
		CreatedAt,
		CreatedBy,
		UpdatedAt, 
		DisbursementDate
	)
	VALUES
	(
		S.BorrowerSSN, 
		S.LoanSequence, 
		S.DisbSequence,
		S.DisbType,
		GETDATE(),
		'dbo',
		GETDATE(), 
		S.DisbursementDate
	);

PRINT 'Merged the TS2HPendingDisbursements table'

MERGE 
	[batchesp].ParentPlusLoanDetails AS T 
USING 
	(
		SELECT DISTINCT
			SourcePop.BorrowerSSN,
			SourcePop.StudentSSN,
			SourcePop.LoanSequence,
			SourcePop.DefermentRequested,
			SourcePop.PostEnrollmentDefermentEligible
		FROM
		(		
			SELECT DISTINCT
				LN10.BF_SSN AS BorrowerSSN,
				LN10.LF_STU_SSN AS StudentSSN,
				LN10.LN_SEQ AS LoanSequence,
				CASE WHEN COALESCE(NULLIF(LN10.LI_DFR_REQ_ON_APL, ''),'N') = 'N' 
					THEN 0
					ELSE 1
				END AS DefermentRequested,
				CASE WHEN ISNULL(LN10.LD_LON_1_DSB, '2008-06-01') >= '2008-07-01'
					THEN 1
					ELSE 0
				END AS PostEnrollmentDefermentEligible,
				RANK() OVER(PARTITION BY WQ20.BF_SSN, LN10.LN_SEQ ORDER BY WQ20.WF_CRT_DTS_WQ20) AS OLDEST
			FROM
				#WQ20_Pop WQ20
				INNER JOIN UDW..LN10_LON LN10
					ON LN10.BF_SSN = WQ20.BF_SSN
					AND LN10.LA_CUR_PRI > 0
				INNER JOIN UDW..PD10_PRS_NME PD10
					ON PD10.DF_PRS_ID = LN10.BF_SSN
				LEFT JOIN ULS.quecomplet.Queues QC
					ON QC.[Queue] = WQ20.WF_QUE
					AND QC.[SubQueue] = WQ20.WF_SUB_QUE
					AND QC.AccountIdentifier = PD10.DF_SPE_ACC_ID
					AND QC.TaskControlNumber = WQ20.WN_CTL_TSK
					AND QC.DeletedAt IS NULL
					AND COALESCE(CAST(QC.ProcessedAt AS DATE),CAST(GETDATE() AS DATE)) > CAST(DATEADD(DAY,-1,GETDATE()) AS DATE) --Either not closed, or closed within last day but might not be removed from WQ20 table yet
				WHERE
					QC.AccountIdentifier IS NULL --Not waiting on QUECOMPLET to close task
					AND LN10.IC_LON_PGM IN('DLPLUS','PLUS')
					AND LN10.LA_CUR_PRI > '0'
					AND LN10.LC_STA_LON10 = 'R'
			) SourcePop
			WHERE SourcePop.OLDEST = 1
		) S
			ON T.BorrowerSSN = S.BorrowerSSN
			AND T.LoanSequence = S.LoanSequence
			AND T.StudentSsn = S.StudentSsn
			AND (T.ProcessedAt IS NULL OR CAST(T.CreatedAt AS DATE) = CAST(GETDATE() AS DATE)) --Use ON criteria to prevent dupe tasks.
WHEN NOT MATCHED BY TARGET THEN
	INSERT
	(
		BorrowerSSN,
		StudentSSN,
		LoanSequence,
		DefermentRequested,
		PostEnrollmentDefermentEligible,
		CreatedAt,
		CreatedBy,
		UpdatedAt
	)
	VALUES
	(
		S.BorrowerSSN, 
		S.StudentSSN,
		S.LoanSequence, 
		S.DefermentRequested,
		S.PostEnrollmentDefermentEligible,
		GETDATE(),
		'BATCHESP_Init_Pull',
		GETDATE()
	)
WHEN MATCHED THEN UPDATE SET
	T.DefermentRequested = S.DefermentRequested,
	T.CreatedBy = 'BATCHESP_Merge_Update',
	T.UpdatedAt = GETDATE();

PRINT 'Merged the ParentPlusLoanDetails table'
