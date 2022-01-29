
DECLARE @TODAY DATE = GETDATE();
DECLARE @ARCTYPEID TINYINT = 2;	--Atd22ByBalance - Add arc for all loans with a balance
DECLARE @ARC CHAR(5) = 'DIFST';
DECLARE @COMMENT VARCHAR(100) = 'Borrower has loan sequences that are currently in different statuses. Review account.';
DECLARE @SCRIPTID  VARCHAR(10) = 'UTLWS69';

BEGIN TRY
BEGIN TRANSACTION

INSERT INTO 
	ULS..ArcAddProcessing
		(
			ArcTypeId,
			ArcResponseCodeId,
			AccountNumber,
			RecipientId,
			ARC,
			ActivityType,
			ActivityContact,
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
			LN_ATY_SEQ,
			ProcessingAttempts,
			CreatedAt,
			CreatedBy,
			ProcessedAt
		)
SELECT DISTINCT
	@ARCTYPEID AS ArcTypeId,
	NULL AS	ArcResponseCodeId,
	LOANS.DF_SPE_ACC_ID AS AccountNumber,
	NULL AS RecipientId,
	@ARC AS ARC,
	NULL AS ActivityType,
	NULL AS ActivityContact,
	@SCRIPTID AS ScriptId,
	@TODAY AS ProcessOn, 
	@COMMENT AS Comment,
	0 AS IsReference,
	0 AS IsEndorser,
	NULL AS ProcessFrom,
	NULL AS ProcessTo,
	NULL AS NeededBy,
	NULL AS RegardsTo,
	NULL AS RegardsCode,
	0 AS LN_ATY_SEQ,
	0 AS ProcessingAttempts, --this gets updated by the arc add script so it is initialized as 0 attempts made
	@TODAY AS CreatedAt,
	@SCRIPTID AS CreatedBy,
	NULL AS ProcessedAt
FROM 
--LOANS wrap new results into a derived table so they can be checked against existing ArcAddProcessing records to prevent duplicates
	(
		SELECT DISTINCT
			PD10.DF_SPE_ACC_ID,
			CASE
				WHEN
				--not included for parent plus with parent plus for different students
					(
						LN10.IC_LON_PGM IN('PLUS') 
						AND DW01Diff.IC_LON_PGM IN('PLUS')
						AND LN10.LF_STU_SSN != DW01Diff.LF_STU_SSN --only exclude the loans if they are for different students (include them if they are for the same student)
					)
					OR
				--not included for spousal with plus or non spousal
					(
						LN10.IC_LON_PGM IN('SPCNSL','SUBSPC','UNSPC') 
						AND DW01Diff.IC_LON_PGM IN('CNSLDN','SUBCNS','UNCNS','PLUS','PLUSGB')
					)
					OR
					(
						LN10.IC_LON_PGM IN('CNSLDN','SUBCNS','UNCNS','PLUS','PLUSGB')
						AND DW01Diff.IC_LON_PGM IN('SPCNSL','SUBSPC','UNSPC') 
						
					)
					OR
				--not included for non spousal with grad plus or parent plus
					(
						LN10.IC_LON_PGM IN('CNSLDN','SUBCNS','UNCNS') 
						AND DW01Diff.IC_LON_PGM IN('PLUS','PLUSGB')
					)
					OR
					(
						LN10.IC_LON_PGM IN('PLUS','PLUSGB')
						AND DW01Diff.IC_LON_PGM IN('CNSLDN','SUBCNS','UNCNS') 
					)
					OR
				--not included for parent plus with grad plus
					(
						LN10.IC_LON_PGM IN('PLUS')
						AND DW01Diff.IC_LON_PGM IN('PLUSGB')
					)
					OR
					(
						LN10.IC_LON_PGM IN('PLUSGB')
						AND DW01Diff.IC_LON_PGM IN('PLUS')
					)
					OR
				--not included for TILP with any other program
					(
						LN10.IC_LON_PGM IN('TILP') 
						AND DW01Diff.IC_LON_PGM IN('CNSLDN','COMPLT','PLUS','PLUSGB','SLS','STFFRD','UNSTFD','SUBCNS','SUBSPC','UNCNS','UNSPC')
					)
					OR
					(
						LN10.IC_LON_PGM IN('CNSLDN','COMPLT','PLUS','PLUSGB','SLS','STFFRD','UNSTFD','SUBCNS','SUBSPC','UNCNS','UNSPC')
						AND DW01Diff.IC_LON_PGM IN('TILP') 
					)
					OR
				--not included for POP8 scenario (loans that are in different statuses have a combination status In-School and Deferment pop8)
					POP8.BF_SSN IS NOT NULL 
				THEN 
					0
				ELSE 
					1 
			END AS [INCLUDE]
		FROM
			UDW..PD10_PRS_NME PD10	
			INNER JOIN UDW..LN10_LON LN10
				ON LN10.BF_SSN = PD10.DF_PRS_ID
			INNER JOIN UDW..DW01_DW_CLC_CLU DW01
				ON LN10.BF_SSN = DW01.BF_SSN
				AND LN10.LN_SEQ = DW01.LN_SEQ
		--This is grabbing loans of different statuses for the same borrower
			INNER JOIN 
			(
				SELECT
					DW01Inner.BF_SSN,
					DW01Inner.LN_SEQ,
					DW01Inner.WC_DW_LON_STA,
					LN10Inner.IC_LON_PGM,
					LN10Inner.LF_STU_SSN
				FROM
					UDW..DW01_DW_CLC_CLU DW01Inner
					INNER JOIN UDW..LN10_LON LN10Inner
						ON DW01Inner.BF_SSN = LN10Inner.BF_SSN
						AND DW01Inner.LN_SEQ = LN10Inner.LN_SEQ
						AND LN10Inner.LA_CUR_PRI > 0
						AND LN10Inner.LC_STA_LON10 = 'R'
			) DW01Diff
				ON DW01.BF_SSN = DW01Diff.BF_SSN
				AND DW01.LN_SEQ != DW01Diff.LN_SEQ
				AND DW01.WC_DW_LON_STA != DW01Diff.WC_DW_LON_STA	
		--Population of borrowers that are in a Pre-Claim, Claim Submitted or Claim Pending status - CLM
			LEFT JOIN 
			(
				SELECT
					DW01.BF_SSN
				FROM 
					UDW..DW01_DW_CLC_CLU DW01
				WHERE 
					DW01.WC_DW_LON_STA IN ('07','08','12','13','14','15')
			) CLM
				ON LN10.BF_SSN = CLM.BF_SSN
		--'DIFST' arc has been left on their account in the last 30 days - ARC
			LEFT JOIN 
			(
				SELECT
					AY10.BF_SSN
				FROM 
					UDW..AY10_BR_LON_ATY AY10
				WHERE
					AY10.LC_STA_ACTY10 = 'A'
					AND AY10.PF_REQ_ACT = 'DIFST' --'DIFST' arc has been left on their account in the last 30 days - ARC
					--AND DAYS(CURRENT DATE) - DAYS(AY10.LD_ATY_REQ_RCV) <= 30 TODO:  original SAS code, remove for production
					AND DATEDIFF(DAY,CAST(AY10.LD_ATY_REQ_RCV AS DATE),@TODAY) <= 30
			) ARC
				ON LN10.BF_SSN = ARC.BF_SSN
		--If the borrowers loans that are in different statuses have a combination status In-School and Deferment pop8
			LEFT JOIN 
			(
				SELECT
					DW0102.BF_SSN
				FROM 
					UDW..DW01_DW_CLC_CLU DW0102
					INNER JOIN UDW..DW01_DW_CLC_CLU DW0104
						ON DW0104.BF_SSN = DW0102.BF_SSN
						AND DW0104.WC_DW_LON_STA = '04'
					INNER JOIN UDW..DF10_BR_DFR_REQ DF10
						ON DF10.BF_SSN = DW0104.BF_SSN
						AND DF10.LC_DFR_STA = 'A'
						AND DF10.LC_STA_DFR10 = 'A'
						AND DF10.LC_DFR_TYP IN ('15','18') --the deferment type is either half-time or full-time
					INNER JOIN UDW..LN50_BR_DFR_APV LN50
						ON LN50.BF_SSN = DF10.BF_SSN
						AND LN50.LF_DFR_CTL_NUM = DF10.LF_DFR_CTL_NUM
						AND LN50.LC_STA_LON50 = 'A'
						AND LN50.LC_DFR_RSP != '003' --not rejected
						AND @TODAY BETWEEN CAST(LN50.LD_DFR_BEG AS DATE) AND CAST(LN50.LD_DFR_END AS DATE)
				WHERE 
					DW0102.WC_DW_LON_STA IN ('02')
			) POP8
				ON LN10.BF_SSN = POP8.BF_SSN
		--borrower already has an active queue task
			LEFT JOIN
			(
				SELECT
					BF_SSN
				FROM
					UDW..WQ20_TSK_QUE WQ20
				WHERE
					WQ20.WF_QUE = '36'
					AND WQ20.WF_SUB_QUE = '01'
					AND WQ20.WC_STA_WQUE20 IN('A','H','P','U','W')
			) ActiveQueue
				ON ActiveQueue.BF_SSN = LN10.BF_SSN
		WHERE	
			LN10.LC_STA_LON10 = 'R'
			AND LN10.LA_CUR_PRI > 0
			AND CLM.BF_SSN IS NULL --Population of borrowers that are in a Pre-Claim, Claim Submitted or Claim Pending status - CLM  --TODO:  uncomment for production
			AND ARC.BF_SSN IS NULL --'DIFST' arc has been left on their account in the last 30 days - ARC  --TODO:  uncomment for production
			AND ActiveQueue.BF_SSN IS NULL --Doesnt have an open task to be worked already  --TODO:  uncomment for production
			--AND PD10.DF_SPE_ACC_ID = '5109079153'  --TODO:  delete for production
	) LOANS
--left join to ArcAddProcessing to check for an existing row to prevent duplicates
	LEFT JOIN ULS..ArcAddProcessing AAP
		ON LOANS.DF_SPE_ACC_ID = AAP.AccountNumber
		AND AAP.ARC = @ARC
		AND AAP.Comment = @COMMENT
		AND CAST(AAP.CreatedAt AS DATE) = CAST(@TODAY AS DATE)
WHERE
	AAP.AccountNumber IS NULL --exclude duplicates
GROUP BY 
	LOANS.DF_SPE_ACC_ID
HAVING
	SUM(LOANS.[INCLUDE]) > 0

	COMMIT TRANSACTION;

END TRY
--write message to process logger if an error occurs
BEGIN CATCH
	DECLARE @EM VARCHAR(4000) = @SCRIPTID + ' encountered an error.  Transaction not committed. Error: ' + (SELECT ERROR_MESSAGE());

	ROLLBACK TRANSACTION;

	DECLARE @ProcessLogId INT;
	DECLARE @ProcessNotificationId INT;
	DECLARE @NotificationTypeId INT = (SELECT TOP 1 NotificationTypeId FROM ProcessLogs..NotificationTypes WHERE NotificationTypeDescription = 'Error Report'); --Error report
	DECLARE @NotificationSeverityTypeId INT = (SELECT TOP 1 NotificationSeverityTypeId FROM ProcessLogs..NotificationSeverityTypes WHERE NotificationSeverityTypeDescription = 'Critical'); --Critical
		
	INSERT INTO ProcessLogs..ProcessLogs (StartedOn, EndedOn, ScriptId, Region, RunBy) VALUES(GETDATE(),GETDATE(),@SCRIPTID,'uheaa',SUSER_SNAME());
	SET @ProcessLogId = SCOPE_IDENTITY()

	INSERT INTO ProcessLogs..ProcessNotifications (NotificationTypeId,NotificationSeverityTypeId,ProcessLogId, ResolvedAt, ResolvedBy) VALUES(@NotificationTypeId,@NotificationSeverityTypeId,@ProcessLogId, NULL, NULL)
	SET @ProcessNotificationId = SCOPE_IDENTITY()

	INSERT INTO ULS.[log].ProcessLogMessages (ProcessNotificationId, LogMessage) VALUES(@ProcessNotificationId,@EM);

	THROW;
END CATCH;