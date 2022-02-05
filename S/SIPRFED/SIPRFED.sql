USE ServicerInventoryMetrics;
GO

DECLARE @CURRENT_DATE DATE = GETDATE();
DECLARE @ScriptId VARCHAR(10) = 'SIPRFED';
DECLARE @EM_ VARCHAR(4000);
DECLARE @ProcessLogId_ INT;
DECLARE @ProcessNotificationId_ INT;
DECLARE @NotificationTypeId_ INT;
DECLARE @NotificationSeverityTypeId_ INT;

/*FORBEARANCE*/
BEGIN TRY
	BEGIN TRANSACTION

	TRUNCATE TABLE Forbearance; --TODO: uncomment for prod
	--EXEC  spTruncate 'Forbearance'; --for testing (most users don't have permission to run TRUNCATE so this gets around that for testing

	INSERT INTO Forbearance
		(
			BF_SSN,
			WF_QUE,
			WF_SUB_QUE,
			WN_CTL_TSK,
			PF_REQ_ACT,
			WD_ACT_REQ,
			WC_STA_WQUE20,
			WF_LST_DTS_WQ20
		)
		SELECT DISTINCT
			WQ20.BF_SSN,
			WQ20.WF_QUE,
			WQ20.WF_SUB_QUE,
			WQ20.WN_CTL_TSK,
			WQ20.PF_REQ_ACT,
			WQ20.WD_ACT_REQ,
			WQ20.WC_STA_WQUE20,
			WQ20.WF_LST_DTS_WQ20
		FROM
			CDW..WQ20_TSK_QUE WQ20
		WHERE -- open forbearance rqst/adj task OR open verbal forbearance, verb reduce pmt forb, or web request letters task with verbal forbearance, borr requests rpf, or econ hard forb web ARC
			( 
				WQ20.WF_QUE = 'SF' --forbearance rqst/adj

				OR 
					( 
						WQ20.WF_QUE IN ('VB','VR','WR') --verbal forbearance, verb reduce pmt forb, web request letters 
						AND WQ20.PF_REQ_ACT IN ('XFORB','BRRPF','G7096') --verbal forbearance, borr requests rpf, econ hard forb web  
					)
			)
			AND WQ20.WC_STA_WQUE20 NOT IN ('C','X')
	;

	COMMIT TRANSACTION;

END TRY
--write message to process logger if an error occurs
BEGIN CATCH
	SET @EM_ = @ScriptId + '  Forbearance table load encountered an error.  Transaction not committed. Error: ' + (SELECT ERROR_MESSAGE());

	ROLLBACK TRANSACTION;

	SET @NotificationTypeId_ = (SELECT TOP 1 NotificationTypeId FROM ProcessLogs..NotificationTypes WHERE NotificationTypeDescription = 'Error Report'); --Error report
	SET @NotificationSeverityTypeId_ = (SELECT TOP 1 NotificationSeverityTypeId FROM ProcessLogs..NotificationSeverityTypes WHERE NotificationSeverityTypeDescription = 'Critical'); --Critical
		
	INSERT INTO ProcessLogs..ProcessLogs (StartedOn, EndedOn, ScriptId, Region, RunBy) VALUES(GETDATE(),GETDATE(),@ScriptId,'cornerstone',SUSER_SNAME());
	SET @ProcessLogId_ = SCOPE_IDENTITY()

	INSERT INTO ProcessLogs..ProcessNotifications (NotificationTypeId,NotificationSeverityTypeId,ProcessLogId, ResolvedAt, ResolvedBy) VALUES(@NotificationTypeId_,@NotificationSeverityTypeId_,@ProcessLogId_, NULL, NULL)
	SET @ProcessNotificationId_ = SCOPE_IDENTITY()

	INSERT INTO CLS.[log].ProcessLogMessages (ProcessNotificationId, LogMessage) VALUES(@ProcessNotificationId_,@EM_);

	THROW;
END CATCH;


/*DEFERMENT*/
BEGIN TRY
	BEGIN TRANSACTION

	TRUNCATE TABLE Deferment; --TODO: uncomment for prod
	--EXEC dbo.spTruncate 'Deferment'; --for testing (most users don't have permission to run TRUNCATE so this gets around that for testing

	INSERT INTO Deferment
		(
			BF_SSN,
			WF_QUE,
			WF_SUB_QUE,
			WN_CTL_TSK,
			PF_REQ_ACT,
			WD_ACT_REQ,
			WC_STA_WQUE20,
			WF_LST_DTS_WQ20
		)
		SELECT DISTINCT
			WQ20.BF_SSN,
			WQ20.WF_QUE,
			WQ20.WF_SUB_QUE,
			WQ20.WN_CTL_TSK,
			WQ20.PF_REQ_ACT,
			WQ20.WD_ACT_REQ,
			WQ20.WC_STA_WQUE20,
			WQ20.WF_LST_DTS_WQ20
		FROM
			CDW..WQ20_TSK_QUE WQ20
	WHERE --deferments rqst/adjt  task OR web request letters with defer-unem-web ARC
		( 
			WQ20.WF_QUE = 'S4' --deferments rqst/adjt
			OR
				(
					WQ20.WF_QUE = 'WR' --web request letters
					AND WQ20.PF_REQ_ACT = 'G708C' --defer-unem-web
				)
		)
		AND WQ20.WC_STA_WQUE20 NOT IN ('C','X')
	;

	COMMIT TRANSACTION;

END TRY
--write message to process logger if an error occurs
BEGIN CATCH
	SET @EM_ = @ScriptId + '  Deferment table load encountered an error.  Transaction not committed. Error: ' + (SELECT ERROR_MESSAGE());

	ROLLBACK TRANSACTION;

	SET @NotificationTypeId_ = (SELECT TOP 1 NotificationTypeId FROM ProcessLogs..NotificationTypes WHERE NotificationTypeDescription = 'Error Report'); --Error report
	SET @NotificationSeverityTypeId_ = (SELECT TOP 1 NotificationSeverityTypeId FROM ProcessLogs..NotificationSeverityTypes WHERE NotificationSeverityTypeDescription = 'Critical'); --Critical
		
	INSERT INTO ProcessLogs..ProcessLogs (StartedOn, EndedOn, ScriptId, Region, RunBy) VALUES(GETDATE(),GETDATE(),@ScriptId,'cornerstone',SUSER_SNAME());
	SET @ProcessLogId_ = SCOPE_IDENTITY()

	INSERT INTO ProcessLogs..ProcessNotifications (NotificationTypeId,NotificationSeverityTypeId,ProcessLogId, ResolvedAt, ResolvedBy) VALUES(@NotificationTypeId_,@NotificationSeverityTypeId_,@ProcessLogId_, NULL, NULL)
	SET @ProcessNotificationId_ = SCOPE_IDENTITY()

	INSERT INTO CLS.[log].ProcessLogMessages (ProcessNotificationId, LogMessage) VALUES(@ProcessNotificationId_,@EM_);

	THROW;
END CATCH;


/*IDR*/
BEGIN TRY
	BEGIN TRANSACTION

	TRUNCATE TABLE IDR; --TODO: uncomment for prod
	--EXEC dbo.spTruncate 'IDR'; --for testing (most users don't have permission to run TRUNCATE so this gets around that for testing

	INSERT INTO IDR
		(
			BF_SSN,
			WF_QUE,
			WF_SUB_QUE,
			WN_CTL_TSK,
			PF_REQ_ACT,
			WD_ACT_REQ,
			WC_STA_WQUE20,
			WF_LST_DTS_WQ20
		)
		SELECT DISTINCT
				WQ20.BF_SSN,
				WQ20.WF_QUE,
				WQ20.WF_SUB_QUE,
				WQ20.WN_CTL_TSK,
				WQ20.PF_REQ_ACT,
				WQ20.WD_ACT_REQ,
				WQ20.WC_STA_WQUE20,
				WQ20.WF_LST_DTS_WQ20
			FROM
				CDW..WQ20_TSK_QUE WQ20
			WHERE --open income based repymt with rvw-ibr cod cmpl app, rvw-ibr cod prtl app, rcvd-ibr application, or rcvd-incm drvn app-p ARC
				WQ20.WF_QUE = '2A' --income based repymt
				AND WQ20.PF_REQ_ACT IN ('CODCA', 'CODPA', 'IBRDF', 'IDRPR') --codca: rvw-ibr cod cmpl app; codpa: rvw-ibr cod prtl app; ibrdf: rcvd-ibr application; idrpr: rcvd-incm drvn app-p

				AND WQ20.WC_STA_WQUE20 NOT IN ('C','X')
	;

	COMMIT TRANSACTION;

END TRY
--write message to process logger if an error occurs
BEGIN CATCH
	SET @EM_ = @ScriptId + '  IDR table load encountered an error.  Transaction not committed. Error: ' + (SELECT ERROR_MESSAGE());

	ROLLBACK TRANSACTION;

	SET @NotificationTypeId_ = (SELECT TOP 1 NotificationTypeId FROM ProcessLogs..NotificationTypes WHERE NotificationTypeDescription = 'Error Report'); --Error report
	SET @NotificationSeverityTypeId_ = (SELECT TOP 1 NotificationSeverityTypeId FROM ProcessLogs..NotificationSeverityTypes WHERE NotificationSeverityTypeDescription = 'Critical'); --Critical
		
	INSERT INTO ProcessLogs..ProcessLogs (StartedOn, EndedOn, ScriptId, Region, RunBy) VALUES(GETDATE(),GETDATE(),@ScriptId,'cornerstone',SUSER_SNAME());
	SET @ProcessLogId_ = SCOPE_IDENTITY()

	INSERT INTO ProcessLogs..ProcessNotifications (NotificationTypeId,NotificationSeverityTypeId,ProcessLogId, ResolvedAt, ResolvedBy) VALUES(@NotificationTypeId_,@NotificationSeverityTypeId_,@ProcessLogId_, NULL, NULL)
	SET @ProcessNotificationId_ = SCOPE_IDENTITY()

	INSERT INTO CLS.[log].ProcessLogMessages (ProcessNotificationId, LogMessage) VALUES(@ProcessNotificationId_,@EM_);

	THROW;
END CATCH;


/*BANKRUPTCY_NOTICE*/
BEGIN TRY
	BEGIN TRANSACTION

	TRUNCATE TABLE BankruptcyNotice; --TODO: uncomment for prod
	--EXEC dbo.spTruncate 'BankruptcyNotice'; --for testing (most users don't have permission to run TRUNCATE so this gets around that for testing

	INSERT INTO BankruptcyNotice
		(
			BF_SSN,
			WF_QUE,
			WF_SUB_QUE,
			WN_CTL_TSK,
			PF_REQ_ACT,
			WD_ACT_REQ,
			WC_STA_WQUE20,
			WF_LST_DTS_WQ20
		)
		SELECT DISTINCT
			WQ20.BF_SSN,
			WQ20.WF_QUE,
			WQ20.WF_SUB_QUE,
			WQ20.WN_CTL_TSK,
			WQ20.PF_REQ_ACT,
			WQ20.WD_ACT_REQ,
			WQ20.WC_STA_WQUE20,
			WQ20.WF_LST_DTS_WQ20
		FROM
			CDW..WQ20_TSK_QUE WQ20
		WHERE --open bankruptcy document task with bankruptcy documents or  bankruptcy document ARC
			WQ20.WF_QUE = '87' --bankruptcy document
			AND WQ20.PF_REQ_ACT IN ('DIBKP','CRBKP') --crbkp:	bankruptcy documents; dibkp: bankruptcy document
			AND WQ20.WC_STA_WQUE20 NOT IN ('C','X')
	;

	COMMIT TRANSACTION;

END TRY
--write message to process logger if an error occurs
BEGIN CATCH
	SET @EM_ = @ScriptId + '  BankruptcyNotice table load encountered an error.  Transaction not committed. Error: ' + (SELECT ERROR_MESSAGE());

	ROLLBACK TRANSACTION;

	SET @NotificationTypeId_ = (SELECT TOP 1 NotificationTypeId FROM ProcessLogs..NotificationTypes WHERE NotificationTypeDescription = 'Error Report'); --Error report
	SET @NotificationSeverityTypeId_ = (SELECT TOP 1 NotificationSeverityTypeId FROM ProcessLogs..NotificationSeverityTypes WHERE NotificationSeverityTypeDescription = 'Critical'); --Critical
		
	INSERT INTO ProcessLogs..ProcessLogs (StartedOn, EndedOn, ScriptId, Region, RunBy) VALUES(GETDATE(),GETDATE(),@ScriptId,'cornerstone',SUSER_SNAME());
	SET @ProcessLogId_ = SCOPE_IDENTITY()

	INSERT INTO ProcessLogs..ProcessNotifications (NotificationTypeId,NotificationSeverityTypeId,ProcessLogId, ResolvedAt, ResolvedBy) VALUES(@NotificationTypeId_,@NotificationSeverityTypeId_,@ProcessLogId_, NULL, NULL)
	SET @ProcessNotificationId_ = SCOPE_IDENTITY()

	INSERT INTO CLS.[log].ProcessLogMessages (ProcessNotificationId, LogMessage) VALUES(@ProcessNotificationId_,@EM_);

	THROW;
END CATCH;


/*BANKRUPTCY_PROOF_OF_CLAIM*/
BEGIN TRY
	BEGIN TRANSACTION

	TRUNCATE TABLE BankruptcyProofOfClaim; --TODO: uncomment for prod
	--EXEC sptruncate 'BankruptcyProofOfClaim'; --for testing (most users don't have permission to run TRUNCATE so this gets around that for testing
	INSERT INTO BankruptcyProofOfClaim
		(
			BF_SSN,
			WF_QUE,
			WF_SUB_QUE,
			WN_CTL_TSK,
			PF_REQ_ACT,
			WD_ACT_REQ,
			WC_STA_WQUE20,
			WF_LST_DTS_WQ20,
			DEADLINE
		)
		SELECT DISTINCT
			WQ20.BF_SSN,
			WQ20.WF_QUE,
			WQ20.WF_SUB_QUE,
			WQ20.WN_CTL_TSK,
			WQ20.PF_REQ_ACT,
			WQ20.WD_ACT_REQ,
			WQ20.WC_STA_WQUE20,
			WQ20.WF_LST_DTS_WQ20,
			TRY_CAST(Deadline.LX_ATY AS DATETIME) AS DEADLINE --comment (LX_ATY) SHOULD BE date in MM/DD/YYYY format, returns dummy value if comment is not a valid date
		FROM
			CDW..WQ20_TSK_QUE WQ20
			INNER JOIN 
				(
					SELECT
						AY10I.BF_SSN,
						AY20I.LX_ATY,
						AY10I.LD_ATY_REQ_RCV
					FROM 
					--AY10I: LX_ATY and LD_ATY_REQ_RCV for max LD_ATY_REQ_RCV for a BPOCD ARC
						(
							SELECT
								AY10.BF_SSN,
								AY10.LD_ATY_REQ_RCV,
								AY10.LN_ATY_SEQ
							FROM
							--MAXARC: max LD_ATY_REQ_RCV for a BPOCD ARC
								( 
									SELECT 
										BF_SSN,
										MAX(LD_ATY_REQ_RCV) AS LD_ATY_REQ_RCV
									FROM
										CDW..AY10_BR_LON_ATY
									WHERE
										PF_REQ_ACT = 'BPOCD'
										AND LC_STA_ACTY10 = 'A'
									GROUP BY
										BF_SSN
								) MAXARC
								INNER JOIN CDW..AY10_BR_LON_ATY AY10
									ON AY10.BF_SSN = MAXARC.BF_SSN
									AND AY10.LD_ATY_REQ_RCV = MAXARC.LD_ATY_REQ_RCV
									AND AY10.PF_REQ_ACT = 'BPOCD'
									AND AY10.LC_STA_ACTY10 = 'A'
						) AY10I
						INNER JOIN CDW..AY15_ATY_CMT AY15
							ON AY15.BF_SSN = AY10I.BF_SSN
							AND AY15.LN_ATY_SEQ = AY10I.LN_ATY_SEQ  
						INNER JOIN CDW..AY20_ATY_TXT AY20I
							ON  AY20I.BF_SSN = AY10I.BF_SSN
							AND AY20I.LN_ATY_SEQ = AY10I.LN_ATY_SEQ
							AND AY20I.LN_ATY_CMT_SEQ = AY15.LN_ATY_CMT_SEQ
					WHERE
						AY15.LC_STA_AY15 = 'A'
				) Deadline
					ON Deadline.BF_SSN = WQ20.BF_SSN
		WHERE --open banko poc rvw task with banko review pacer ARC
			WQ20.WF_QUE = 'BY' --banko poc rvw
			AND WQ20.PF_REQ_ACT = 'BPOCR' --banko review pacer
			AND WQ20.WC_STA_WQUE20 NOT IN ('C','X')
			AND TRY_CAST(Deadline.LX_ATY AS DATETIME) IS NOT NULL --BankruptcyProofOfClaim.DEADLINE does not accept NULL so this bypasses records where the LX_ATY comment is not a valid date so the script doesn't fail, invalid comments are process logged in the next section
	;
	COMMIT TRANSACTION;

END TRY
--write message to process logger if an error occurs
BEGIN CATCH
	SET @EM_ = @ScriptId + '  BankruptcyProofOfClaim table load encountered an error.  Transaction not committed. Error: ' + (SELECT ERROR_MESSAGE());

	ROLLBACK TRANSACTION;

	SET @NotificationTypeId_ = (SELECT TOP 1 NotificationTypeId FROM ProcessLogs..NotificationTypes WHERE NotificationTypeDescription = 'Error Report'); --Error report
	SET @NotificationSeverityTypeId_ = (SELECT TOP 1 NotificationSeverityTypeId FROM ProcessLogs..NotificationSeverityTypes WHERE NotificationSeverityTypeDescription = 'Critical'); --Critical
		
	INSERT INTO ProcessLogs..ProcessLogs (StartedOn, EndedOn, ScriptId, Region, RunBy) VALUES(GETDATE(),GETDATE(),@ScriptId,'cornerstone',SUSER_SNAME());
	SET @ProcessLogId_ = SCOPE_IDENTITY()

	INSERT INTO ProcessLogs..ProcessNotifications (NotificationTypeId,NotificationSeverityTypeId,ProcessLogId, ResolvedAt, ResolvedBy) VALUES(@NotificationTypeId_,@NotificationSeverityTypeId_,@ProcessLogId_, NULL, NULL)
	SET @ProcessNotificationId_ = SCOPE_IDENTITY()

	INSERT INTO CLS.[log].ProcessLogMessages (ProcessNotificationId, LogMessage) VALUES(@ProcessNotificationId_,@EM_);

	THROW;
END CATCH;


/*BANKRUPTCY_PROOF_OF_CLAIM - PROCESS LOG INVALID DATES IN BPOCD COMMENTS*/
DECLARE @InvalidComments TABLE (LogMessage varchar(4000), Used BIT);
DECLARE @InvalidCommentsCount INT;
DECLARE @InsertedNotifications TABLE(ProcessNotificationId INT, LogMessage VARCHAR(4000));
DECLARE @Message VARCHAR(4000);
DECLARE @NextProcessNotificationId INT;

BEGIN TRY
	BEGIN TRANSACTION

	--insert a log message into table variable for each invalid BPOCD comment (comment (LX_ATY) not in MM/DD/YYYY format)
	INSERT INTO @InvalidComments(LogMessage)
	SELECT
		@ScriptId + '  BankruptcyProofOfClaim invalid date of ' + Deadline.LX_ATY + ' in BPOCD comment for ' +  WQ20.BF_SSN AS LogMessage
	FROM
		CDW..WQ20_TSK_QUE WQ20
		INNER JOIN 
		(
			SELECT
				AY10I.BF_SSN,
				AY20I.LX_ATY,
				AY10I.LD_ATY_REQ_RCV
			FROM 
			--AY10I: LX_ATY and LD_ATY_REQ_RCV for max LD_ATY_REQ_RCV for a BPOCD ARC
				(
					SELECT
						AY10.BF_SSN,
						AY10.LD_ATY_REQ_RCV,
						AY10.LN_ATY_SEQ
					FROM
					--MAXARC: max LD_ATY_REQ_RCV for a BPOCD ARC
						( 
							SELECT 
								BF_SSN,
								MAX(LD_ATY_REQ_RCV) AS LD_ATY_REQ_RCV
							FROM
								CDW..AY10_BR_LON_ATY
							WHERE
								PF_REQ_ACT = 'BPOCD'
								AND LC_STA_ACTY10 = 'A'
							GROUP BY
								BF_SSN
						) MAXARC
						INNER JOIN CDW..AY10_BR_LON_ATY AY10
							ON MAXARC.BF_SSN = AY10.BF_SSN
							AND MAXARC.LD_ATY_REQ_RCV = AY10.LD_ATY_REQ_RCV
							AND AY10.PF_REQ_ACT = 'BPOCD'
							AND AY10.LC_STA_ACTY10 = 'A'
				) AY10I
				INNER JOIN CDW..AY15_ATY_CMT AY15
					ON AY15.BF_SSN = AY10I.BF_SSN
					AND AY15.LN_ATY_SEQ = AY10I.LN_ATY_SEQ  
				INNER JOIN CDW..AY20_ATY_TXT AY20I
					ON AY10I.BF_SSN = AY20I.BF_SSN
					AND AY10I.LN_ATY_SEQ = AY20I.LN_ATY_SEQ
					AND AY15.LN_ATY_CMT_SEQ = AY20I.LN_ATY_CMT_SEQ
			WHERE
				AY15.LC_STA_AY15 = 'A'
		) Deadline
			ON WQ20.BF_SSN = Deadline.BF_SSN
	WHERE --open banko poc rvw task with banko review pacer ARC
		WQ20.WF_QUE = 'BY' --banko poc rvw
		AND WQ20.PF_REQ_ACT = 'BPOCR' --banko review pacer
		AND WQ20.WC_STA_WQUE20 NOT IN ('C','X')
		AND TRY_CAST(Deadline.LX_ATY AS DATETIME) IS NULL   --identifies invalid BPOCR comments (LX_ATY not in MM/DD/YYYY format) as TRY_CAST returns NULL if the comment cannot be cast to a date
;

-- process log invalid comments if thre are any
	SET @InvalidCommentsCount = (SELECT COUNT(*) FROM  @InvalidComments)
	IF @InvalidCommentsCount > 0
		BEGIN
			SET @NotificationTypeId_ = (SELECT TOP 1 NotificationTypeId FROM ProcessLogs..NotificationTypes WHERE NotificationTypeDescription = 'Error Report'); --Error report
			SET @NotificationSeverityTypeId_ = (SELECT TOP 1 NotificationSeverityTypeId FROM ProcessLogs..NotificationSeverityTypes WHERE NotificationSeverityTypeDescription = 'Critical'); --Critical
	
		--create ProcessLogs record and get processLogId
			INSERT INTO ProcessLogs..ProcessLogs (StartedOn, EndedOn, ScriptId, Region, RunBy) VALUES(GETDATE(),GETDATE(),@ScriptId,'cornerstone',SUSER_SNAME());
			SET @ProcessLogId_ = SCOPE_IDENTITY()
			
		--create process notification records and get ProcessNotificationIds
			INSERT INTO ProcessLogs..ProcessNotifications
				(
					NotificationTypeId,
					NotificationSeverityTypeId,
					ProcessLogId,
					ResolvedAt,
					ResolvedBy
				)
			--write ProcessNotificationIds to a table variable to be used later to create ProcessLogMessages records
			OUTPUT
				INSERTED.ProcessNotificationId
			INTO @InsertedNotifications(ProcessNotificationId)
			SELECT
				@NotificationTypeId_,
				@NotificationSeverityTypeId_,
				@ProcessLogId_,
				NULL,
				NULL
			FROM
				@InvalidComments
			
		--assign each LogMessage in @InvalidComments a unique ProcessNotificationIds from @InsertedNotifications
			WHILE EXISTS(SELECT TOP 1 * FROM @InvalidComments WHERE Used IS NULL) --Find a comment we havent assigned a Notification
			BEGIN
				SET @Message = (SELECT TOP 1 LogMessage FROM @InvalidComments WHERE Used IS NULL); --Grab a message to assign a notification
				SET @NextProcessNotificationId = (SELECT TOP 1 ProcessNotificationId FROM @InsertedNotifications WHERE LogMessage IS NULL); --Grab an unused notification record
				UPDATE @InsertedNotifications SET LogMessage = @Message WHERE ProcessNotificationId = @NextProcessNotificationId; --Assign notification record to our message
				UPDATE @InvalidComments SET Used = 1 WHERE LogMessage = @Message; --mark message as used to not be picked up again
			END --Loop until no messages unassigned left
			
		--create ProcessLogMessages record for each invalid comment
			INSERT INTO CLS.[log].ProcessLogMessages (ProcessNotificationId, LogMessage)
			SELECT DISTINCT
				ProcessNotificationId,
				LogMessage
			FROM
				@InsertedNotifications

			--TODO:  for testing, delete or comment out
			--SELECT @ProcessLogId_
			--SELECT @InvalidCommentsCount
			--SELECT * FROM @InvalidComments
		END


	COMMIT TRANSACTION;

END TRY
--write message to process logger if an error occurs
BEGIN CATCH
	SET @EM_ = @ScriptId + '  BankruptcyProofOfClaim invalid date in BPOCD comment process logging encountered an error.  Transaction not committed. Error: ' + (SELECT ERROR_MESSAGE());

	ROLLBACK TRANSACTION;

	SET @NotificationTypeId_ = (SELECT TOP 1 NotificationTypeId FROM ProcessLogs..NotificationTypes WHERE NotificationTypeDescription = 'Error Report'); --Error report
	SET @NotificationSeverityTypeId_ = (SELECT TOP 1 NotificationSeverityTypeId FROM ProcessLogs..NotificationSeverityTypes WHERE NotificationSeverityTypeDescription = 'Critical'); --Critical
		
	INSERT INTO ProcessLogs..ProcessLogs (StartedOn, EndedOn, ScriptId, Region, RunBy) VALUES(GETDATE(),GETDATE(),@ScriptId,'cornerstone',SUSER_SNAME());
	SET @ProcessLogId_ = SCOPE_IDENTITY()

	INSERT INTO ProcessLogs..ProcessNotifications (NotificationTypeId,NotificationSeverityTypeId,ProcessLogId, ResolvedAt, ResolvedBy) VALUES(@NotificationTypeId_,@NotificationSeverityTypeId_,@ProcessLogId_, NULL, NULL)
	SET @ProcessNotificationId_ = SCOPE_IDENTITY()

	INSERT INTO CLS.[log].ProcessLogMessages (ProcessNotificationId, LogMessage) VALUES(@ProcessNotificationId_,@EM_);

	THROW;
END CATCH;


/*DEATH_DISCHARGE*/
BEGIN TRY
	BEGIN TRANSACTION

	TRUNCATE TABLE DeathDischarge; --TODO: uncomment for prod
	--EXEC dbo.spTruncate 'DeathDischarge'; --for testing (most users don't have permission to run TRUNCATE so this gets around that for testing

	INSERT INTO DeathDischarge
		(
			BF_SSN,
			WF_QUE,
			WF_SUB_QUE,
			WN_CTL_TSK,
			PF_REQ_ACT,
			WD_ACT_REQ,
			WC_STA_WQUE20,
			WF_LST_DTS_WQ20
		)
		SELECT DISTINCT
			DIDTH.BF_SSN,
			ISNULL(WQ20.WF_QUE,WQ21.WF_QUE) AS WF_QUE,
			ISNULL(WQ20.WF_SUB_QUE,WQ21.WF_SUB_QUE) AS WF_SUB_QUE,
			ISNULL(WQ20.WN_CTL_TSK,WQ21.WN_CTL_TSK) AS WN_CTL_TSK,
			ISNULL(WQ20.PF_REQ_ACT,WQ21.PF_REQ_ACT) AS PF_REQ_ACT,
			ISNULL(WQ20.WD_ACT_REQ,WQ21.WD_ACT_REQ) AS WD_ACT_REQ,
			ISNULL(WQ20.WC_STA_WQUE20,WQ21.WC_STA_WQUE20) AS WC_STA_WQUE20,
			ISNULL(WQ20.WF_LST_DTS_WQ20,WQ21.WF_LST_DTS_WQ20) AS WF_LST_DTS_WQ20
		FROM
		--DIDTH: max LD_ATY_REQ_RCV for DIDTH ARC
			( 
				SELECT
					BF_SSN,
					MAX(LD_ATY_REQ_RCV) AS LD_ATY_REQ_RCV
				FROM
					CDW..AY10_BR_LON_ATY
				WHERE
					LC_STA_ACTY10 = 'A'
					AND PF_REQ_ACT = 'DIDTH'
				GROUP BY
					BF_SSN
			) DIDTH
		--WQ20: most recent type 23 queue task
			INNER JOIN
				(
					SELECT
						DET.BF_SSN,
						DET.WF_QUE,
						DET.WF_SUB_QUE,
						DET.WN_CTL_TSK,
						DET.PF_REQ_ACT,
						DET.WD_ACT_REQ,
						DET.WC_STA_WQUE20,
						DET.WF_LST_DTS_WQ20
					FROM
					--MXWQ20: most recent type 23 queue task
						(
							SELECT
								BF_SSN,
								MAX(WF_LST_DTS_WQ20) AS WF_LST_DTS_WQ20
							FROM
								CDW..WQ20_TSK_QUE
							WHERE
								WF_QUE = '23'
								--no active flag needed as date of most recent 23 queue task is needed regardless of its status
							GROUP BY
								BF_SSN
						) MXWQ20 --most recent type 23 queue task
						INNER JOIN CDW..WQ20_TSK_QUE DET --get detail for most recent type 23 queue task
							ON DET.BF_SSN = MXWQ20.BF_SSN
							AND DET.WF_LST_DTS_WQ20 = MXWQ20.WF_LST_DTS_WQ20
							--no active flag needed as date of most recent 23 queue task is needed regardless of its status
				) WQ20
					ON WQ20.BF_SSN = DIDTH.BF_SSN
		--WQ21: most recent type 23 queue task
			LEFT JOIN
				(
					SELECT
						DET.BF_SSN,
						DET.WF_QUE,
						DET.WF_SUB_QUE,
						DET.WN_CTL_TSK,
						DET.PF_REQ_ACT,
						DET.WD_ACT_REQ,
						DET.WC_STA_WQUE20,
						DET.WF_LST_DTS_WQ20
					FROM
					--MXWQ21: most recent type 23 queue task
						(
							SELECT
								BF_SSN,
								MAX(WF_LST_DTS_WQ20) AS WF_LST_DTS_WQ20
							FROM
								CDW..WQ21_TSK_QUE_HST
							WHERE
								WF_QUE = '23'
								--no active flag needed as date of most recent 23 queue task is needed regardless of its status
							GROUP BY
								BF_SSN
						) MXWQ21 --most recent type 23 queue task
						INNER JOIN CDW..WQ21_TSK_QUE_HST DET --get detail for most recent type 23 queue task
							ON DET.BF_SSN = MXWQ21.BF_SSN
							AND DET.WF_LST_DTS_WQ20 = MXWQ21.WF_LST_DTS_WQ20
							--no active flag needed as date of most recent 23 queue task is needed regardless of its status
				) WQ21
					ON  WQ21.BF_SSN = DIDTH.BF_SSN
		--DEFSA: max LD_ATY_REQ_RCV for DEFSA ARC
			LEFT JOIN 
			(
				SELECT
					AY10.BF_SSN,
					MAX(AY10.LD_ATY_REQ_RCV) AS LD_ATY_REQ_RCV
				FROM
					CDW..AY10_BR_LON_ATY AY10
				WHERE
					AY10.PF_REQ_ACT = 'DEFSA'
					AND AY10.LC_STA_ACTY10 = 'A'
				GROUP BY
					BF_SSN
			) AS DEFSA
				ON DEFSA.BF_SSN = DIDTH.BF_SSN
				AND CAST(DEFSA.LD_ATY_REQ_RCV AS DATE) > CAST(DIDTH.LD_ATY_REQ_RCV AS DATE) --DEFSA ARC dated after the most recent DIDTH ARC
		WHERE
			DEFSA.BF_SSN IS NULL --Denotes there isnt an DEFSA ARC
	;

	COMMIT TRANSACTION;

END TRY
--write message to process logger if an error occurs
BEGIN CATCH
	SET @EM_ = @ScriptId + '  DeathDischarge table load encountered an error.  Transaction not committed. Error: ' + (SELECT ERROR_MESSAGE());

	ROLLBACK TRANSACTION;

	SET @NotificationTypeId_ = (SELECT TOP 1 NotificationTypeId FROM ProcessLogs..NotificationTypes WHERE NotificationTypeDescription = 'Error Report'); --Error report
	SET @NotificationSeverityTypeId_ = (SELECT TOP 1 NotificationSeverityTypeId FROM ProcessLogs..NotificationSeverityTypes WHERE NotificationSeverityTypeDescription = 'Critical'); --Critical
		
	INSERT INTO ProcessLogs..ProcessLogs (StartedOn, EndedOn, ScriptId, Region, RunBy) VALUES(GETDATE(),GETDATE(),@ScriptId,'cornerstone',SUSER_SNAME());
	SET @ProcessLogId_ = SCOPE_IDENTITY()

	INSERT INTO ProcessLogs..ProcessNotifications (NotificationTypeId,NotificationSeverityTypeId,ProcessLogId, ResolvedAt, ResolvedBy) VALUES(@NotificationTypeId_,@NotificationSeverityTypeId_,@ProcessLogId_, NULL, NULL)
	SET @ProcessNotificationId_ = SCOPE_IDENTITY()

	INSERT INTO CLS.[log].ProcessLogMessages (ProcessNotificationId, LogMessage) VALUES(@ProcessNotificationId_,@EM_);

	THROW;
END CATCH; 


/*CLOSED_SCHOOL_APP*/
BEGIN TRY
	BEGIN TRANSACTION

	TRUNCATE TABLE ClosedSchoolApp; --TODO: uncomment for prod
	--EXEC dbo.spTruncate 'ClosedSchoolApp'; --for testing (most users don't have permission to run TRUNCATE so this gets around that for testing

	INSERT INTO ClosedSchoolApp
		(
			BF_SSN,
			WF_QUE,
			WF_SUB_QUE,
			WN_CTL_TSK,
			PF_REQ_ACT,
			WD_ACT_REQ,
			WC_STA_WQUE20,
			WF_LST_DTS_WQ20
		)
		SELECT DISTINCT
			WQ20.BF_SSN,
			WQ20.WF_QUE,
			WQ20.WF_SUB_QUE,
			WQ20.WN_CTL_TSK,
			WQ20.PF_REQ_ACT,
			WQ20.WD_ACT_REQ,
			WQ20.WC_STA_WQUE20,
			WQ20.WF_LST_DTS_WQ20
		FROM
			CDW..WQ20_TSK_QUE WQ20
		WHERE --open discharge task with discharge-clsd schl ARC
			WQ20.WF_QUE = '15' --discharge  
			AND WQ20.PF_REQ_ACT = 'DICSK' --discharge-clsd schl
			AND WQ20.WC_STA_WQUE20 NOT IN ('C','X')
	;

	COMMIT TRANSACTION;

END TRY
--write message to process logger if an error occurs
BEGIN CATCH
	SET @EM_ = @ScriptId + '  ClosedSchoolApp table load encountered an error.  Transaction not committed. Error: ' + (SELECT ERROR_MESSAGE());

	ROLLBACK TRANSACTION;

	SET @NotificationTypeId_ = (SELECT TOP 1 NotificationTypeId FROM ProcessLogs..NotificationTypes WHERE NotificationTypeDescription = 'Error Report'); --Error report
	SET @NotificationSeverityTypeId_ = (SELECT TOP 1 NotificationSeverityTypeId FROM ProcessLogs..NotificationSeverityTypes WHERE NotificationSeverityTypeDescription = 'Critical'); --Critical
		
	INSERT INTO ProcessLogs..ProcessLogs (StartedOn, EndedOn, ScriptId, Region, RunBy) VALUES(GETDATE(),GETDATE(),@ScriptId,'cornerstone',SUSER_SNAME());
	SET @ProcessLogId_ = SCOPE_IDENTITY()

	INSERT INTO ProcessLogs..ProcessNotifications (NotificationTypeId,NotificationSeverityTypeId,ProcessLogId, ResolvedAt, ResolvedBy) VALUES(@NotificationTypeId_,@NotificationSeverityTypeId_,@ProcessLogId_, NULL, NULL)
	SET @ProcessNotificationId_ = SCOPE_IDENTITY()

	INSERT INTO CLS.[log].ProcessLogMessages (ProcessNotificationId, LogMessage) VALUES(@ProcessNotificationId_,@EM_);

	THROW;
END CATCH;


/*BORROWER_MAIL*/
BEGIN TRY
	BEGIN TRANSACTION

	TRUNCATE TABLE BorrowerMail; --TODO: uncomment for prod
	--EXEC dbo.spTruncate 'BorrowerMail'; --for testing (most users don't have permission to run TRUNCATE so this gets around that for testing

	INSERT INTO BorrowerMail
		(
			BF_SSN,
			WF_QUE,
			WF_SUB_QUE,
			WN_CTL_TSK,
			PF_REQ_ACT,
			WD_ACT_REQ,
			WC_STA_WQUE20,
			WF_LST_DTS_WQ20
		)
 		SELECT DISTINCT
			WQ20.BF_SSN,
			WQ20.WF_QUE,
			WQ20.WF_SUB_QUE,
			WQ20.WN_CTL_TSK,
			WQ20.PF_REQ_ACT,
			WQ20.WD_ACT_REQ,
			WQ20.WC_STA_WQUE20,
			WQ20.WF_LST_DTS_WQ20
		FROM
			CDW..WQ20_TSK_QUE WQ20
		WHERE --open correspondence task with borrower corr ARC
			WQ20.WF_QUE = '88' --correspondence
			AND WQ20.PF_REQ_ACT = 'DIBCR' --borrower corr
			AND WQ20.WC_STA_WQUE20 NOT IN ('C','X')
	;

	COMMIT TRANSACTION;

END TRY
--write message to process logger if an error occurs
BEGIN CATCH
	SET @EM_ = @ScriptId + '  BorrowerMail table load encountered an error.  Transaction not committed. Error: ' + (SELECT ERROR_MESSAGE());

	ROLLBACK TRANSACTION;

	SET @NotificationTypeId_ = (SELECT TOP 1 NotificationTypeId FROM ProcessLogs..NotificationTypes WHERE NotificationTypeDescription = 'Error Report'); --Error report
	SET @NotificationSeverityTypeId_ = (SELECT TOP 1 NotificationSeverityTypeId FROM ProcessLogs..NotificationSeverityTypes WHERE NotificationSeverityTypeDescription = 'Critical'); --Critical
		
	INSERT INTO ProcessLogs..ProcessLogs (StartedOn, EndedOn, ScriptId, Region, RunBy) VALUES(GETDATE(),GETDATE(),@ScriptId,'cornerstone',SUSER_SNAME());
	SET @ProcessLogId_ = SCOPE_IDENTITY()

	INSERT INTO ProcessLogs..ProcessNotifications (NotificationTypeId,NotificationSeverityTypeId,ProcessLogId, ResolvedAt, ResolvedBy) VALUES(@NotificationTypeId_,@NotificationSeverityTypeId_,@ProcessLogId_, NULL, NULL)
	SET @ProcessNotificationId_ = SCOPE_IDENTITY()

	INSERT INTO CLS.[log].ProcessLogMessages (ProcessNotificationId, LogMessage) VALUES(@ProcessNotificationId_,@EM_);

	THROW;
END CATCH;


/*DEATH_DISCHARGE_FSA*/
BEGIN TRY
	BEGIN TRANSACTION
	TRUNCATE TABLE DeathDischargeFSA; --TODO: uncomment for prod
	--EXEC dbo.spTruncate 'DeathDischargeFSA'; --for testing (most users don't have permission to run TRUNCATE so this gets around that for testing

	INSERT INTO DeathDischargeFSA
		(
			BF_SSN,
			PF_REQ_ACT,
			LD_ATY_REQ_RCV
		)
		SELECT
			AY10.BF_SSN,
			AY10.PF_REQ_ACT,
			AY10.LD_ATY_REQ_RCV
		FROM 
		--AY10: max LD_ATY_REQ_RCV for DEFSA ARC
			( 
				SELECT
					BF_SSN,
					PF_REQ_ACT,
					MAX(LD_ATY_REQ_RCV) AS LD_ATY_REQ_RCV
				FROM
					CDW..AY10_BR_LON_ATY
				WHERE
					LC_STA_ACTY10 = 'A'
					AND PF_REQ_ACT = 'DEFSA'
				GROUP BY
					BF_SSN,
					PF_REQ_ACT
			) AY10
		--AY10Complete:	'ADDTH' and'DEDNY' activity records
			LEFT JOIN 
				(
					SELECT
						AY10I.BF_SSN,
						MAX(LD_ATY_REQ_RCV) AS LD_ATY_REQ_RCV
					FROM
						CDW..AY10_BR_LON_ATY AY10I
					WHERE
						AY10I.PF_REQ_ACT IN ('ADDTH','DEDNY')
						AND AY10I.LC_STA_ACTY10 = 'A'
					GROUP BY
						AY10I.BF_SSN
				) AY10Complete
					ON AY10Complete.BF_SSN = AY10.BF_SSN
		WHERE
			AY10Complete.BF_SSN IS NULL -- Denotes there isnt an ADDTH or DEDNY
			OR AY10.LD_ATY_REQ_RCV > AY10Complete.LD_ATY_REQ_RCV  --or most recent DEFSA ARC is after most recent task complete ARC
	;

	COMMIT TRANSACTION;

END TRY
--write message to process logger if an error occurs
BEGIN CATCH
	SET @EM_ = @ScriptId + '  DeathDischargeFSA table load encountered an error.  Transaction not committed. Error: ' + (SELECT ERROR_MESSAGE());

	ROLLBACK TRANSACTION;

	SET @NotificationTypeId_ = (SELECT TOP 1 NotificationTypeId FROM ProcessLogs..NotificationTypes WHERE NotificationTypeDescription = 'Error Report'); --Error report
	SET @NotificationSeverityTypeId_ = (SELECT TOP 1 NotificationSeverityTypeId FROM ProcessLogs..NotificationSeverityTypes WHERE NotificationSeverityTypeDescription = 'Critical'); --Critical
		
	INSERT INTO ProcessLogs..ProcessLogs (StartedOn, EndedOn, ScriptId, Region, RunBy) VALUES(GETDATE(),GETDATE(),@ScriptId,'cornerstone',SUSER_SNAME());
	SET @ProcessLogId_ = SCOPE_IDENTITY()

	INSERT INTO ProcessLogs..ProcessNotifications (NotificationTypeId,NotificationSeverityTypeId,ProcessLogId, ResolvedAt, ResolvedBy) VALUES(@NotificationTypeId_,@NotificationSeverityTypeId_,@ProcessLogId_, NULL, NULL)
	SET @ProcessNotificationId_ = SCOPE_IDENTITY()

	INSERT INTO CLS.[log].ProcessLogMessages (ProcessNotificationId, LogMessage) VALUES(@ProcessNotificationId_,@EM_);

	THROW;
END CATCH;


/*CLOSED_SCHOOL_FSA*/
BEGIN TRY
	BEGIN TRANSACTION

	TRUNCATE TABLE ClosedSchoolFSA; --TODO: uncomment for prod
	--EXEC dbo.spTruncate 'ClosedSchoolFSA'; --for testing (most users don't have permission to run TRUNCATE so this gets around that for testing

	INSERT INTO ClosedSchoolFSA
		(
			BF_SSN,
			PF_REQ_ACT,
			LD_ATY_REQ_RCV
		)
		SELECT
			AY10.BF_SSN,
			AY10.PF_REQ_ACT,
			AY10.LD_ATY_REQ_RCV
		FROM
		--AY10: max LD_ATY_REQ_RCV for CSFSA ARC
			(
				SELECT
					BF_SSN,
					PF_REQ_ACT,
					MAX(LD_ATY_REQ_RCV) AS LD_ATY_REQ_RCV
				FROM
					CDW..AY10_BR_LON_ATY
				WHERE
					LC_STA_ACTY10 = 'A'
					AND PF_REQ_ACT = 'CSFSA'
				GROUP BY
					BF_SSN,
					PF_REQ_ACT
			) AY10
		--AY10Complete:	LN_ATY_SEQ and LD_ATY_REQ_RCV for max LD_ATY_REQ_RCV for CSDNY or ADCSH ARC
			LEFT JOIN
			( 
				SELECT
					AY10I.BF_SSN,
					AY10I.LN_ATY_SEQ,
					AY10I.LD_ATY_REQ_RCV
				FROM
					CDW..AY10_BR_LON_ATY AY10I
				--MAXARC: max LD_ATY_REQ_RCV for CSDNY or ADCSH ARC
					INNER JOIN
					(
						SELECT
							BF_SSN,
							MAX(LD_ATY_REQ_RCV) AS LD_ATY_REQ_RCV
						FROM
							CDW..AY10_BR_LON_ATY
						WHERE
							PF_REQ_ACT IN ('CSDNY','ADCSH')
							AND LC_STA_ACTY10 = 'A'
						GROUP BY
							BF_SSN
					) MAXARC
						ON MAXARC.BF_SSN = AY10I.BF_SSN
						AND MAXARC.LD_ATY_REQ_RCV = AY10I.LD_ATY_REQ_RCV
				WHERE
					AY10I.PF_REQ_ACT IN ('CSDNY','ADCSH')
					AND AY10I.LC_STA_ACTY10 = 'A'
			) AY10Complete
				ON AY10Complete.BF_SSN = AY10.BF_SSN
				AND AY10Complete.LD_ATY_REQ_RCV > AY10.LD_ATY_REQ_RCV
		WHERE
			AY10Complete.BF_SSN IS NULL --Denotes there isnt an ADDTH or DEDNY
	;

	COMMIT TRANSACTION;

END TRY
--write message to process logger if an error occurs
BEGIN CATCH
	SET @EM_ = @ScriptId + '  ClosedSchoolFSA table load encountered an error.  Transaction not committed. Error: ' + (SELECT ERROR_MESSAGE());

	ROLLBACK TRANSACTION;

	SET @NotificationTypeId_ = (SELECT TOP 1 NotificationTypeId FROM ProcessLogs..NotificationTypes WHERE NotificationTypeDescription = 'Error Report'); --Error report
	SET @NotificationSeverityTypeId_ = (SELECT TOP 1 NotificationSeverityTypeId FROM ProcessLogs..NotificationSeverityTypes WHERE NotificationSeverityTypeDescription = 'Critical'); --Critical
		
	INSERT INTO ProcessLogs..ProcessLogs (StartedOn, EndedOn, ScriptId, Region, RunBy) VALUES(GETDATE(),GETDATE(),@ScriptId,'cornerstone',SUSER_SNAME());
	SET @ProcessLogId_ = SCOPE_IDENTITY()

	INSERT INTO ProcessLogs..ProcessNotifications (NotificationTypeId,NotificationSeverityTypeId,ProcessLogId, ResolvedAt, ResolvedBy) VALUES(@NotificationTypeId_,@NotificationSeverityTypeId_,@ProcessLogId_, NULL, NULL)
	SET @ProcessNotificationId_ = SCOPE_IDENTITY()

	INSERT INTO CLS.[log].ProcessLogMessages (ProcessNotificationId, LogMessage) VALUES(@ProcessNotificationId_,@EM_);

	THROW;
END CATCH;


/*AGING360_AT_SERVICER*/
BEGIN TRY
	BEGIN TRANSACTION

	TRUNCATE TABLE Aging360AtServicer; --TODO: uncomment for prod
	--EXEC dbo.spTruncate 'Aging360AtServicer'; --for testing (most users don't have permission to run TRUNCATE so this gets around that for testing

	INSERT INTO Aging360AtServicer
		(
			BF_SSN,
			LN_SEQ,
			AGING_DATE
		)
		SELECT
			LN10.BF_SSN,
			LN10.LN_SEQ,
			DATEADD(DAY,360, LN16.LD_DLQ_OCC) AS AGING_DATE
		FROM
			CDW..LN10_LON LN10
			INNER JOIN CDW..LN16_LON_DLQ_HST LN16
				ON LN16.BF_SSN = LN10.BF_SSN
				AND LN16.LN_SEQ = LN10.LN_SEQ
		--SUSP:	select borrowers in collection suspension status
			LEFT JOIN 
			(
				SELECT
					LN60.BF_SSN,
					LN60.LN_SEQ
				FROM
					CDW..LN60_BR_FOR_APV LN60
					INNER JOIN CDW..FB10_BR_FOR_REQ FB10
						ON FB10.BF_SSN = LN60.BF_SSN
						AND FB10.LF_FOR_CTL_NUM = LN60.LF_FOR_CTL_NUM
				WHERE
					LN60.LC_STA_LON60 = 'A'
					AND FB10.LC_FOR_STA = 'A'
					AND FB10.LC_STA_FOR10 = 'A'
					AND FB10.LC_FOR_TYP = '28'
					AND CAST(LN60.LD_FOR_END AS DATE)  >= @CURRENT_DATE	
					AND LN60.LC_FOR_RSP != '003' --not denied
			) SUSP
				ON SUSP.BF_SSN = LN10.BF_SSN
				AND SUSP.LN_SEQ = LN10.LN_SEQ
		--AY10: select borrowers with DCSTR activity records
			LEFT JOIN 
			(
				SELECT
					AY10I.BF_SSN
				FROM
					CDW..AY10_BR_LON_ATY AY10I
				WHERE
					AY10I.PF_REQ_ACT = 'DCSTR'
					AND LC_STA_ACTY10 = 'A'
			) AY10
				ON AY10.BF_SSN = LN10.BF_SSN
		WHERE
			LN16.LN_DLQ_MAX >= 360
			AND DATEDIFF(DAY, ISNULL(CAST(LN16.LD_DLQ_MAX AS DATE) , @CURRENT_DATE), @CURRENT_DATE) > 5
			AND LN16.LC_STA_LON16 = '1'
			AND LN10.LC_STA_LON10 != 'D' --'D' is deconverted or sent to DMCS, all other statuses are at servicer
			AND LN10.LA_CUR_PRI > 0.00
			AND AY10.BF_SSN IS NULL --exclude borrowers with DCSTR ARC
			AND SUSP.BF_SSN IS NULL  --exclude borrowers in collection suspension status
	;

	COMMIT TRANSACTION;

END TRY
--write message to process logger if an error occurs
BEGIN CATCH
	SET @EM_ = @ScriptId + '  Aging360AtServicer table load encountered an error.  Transaction not committed. Error: ' + (SELECT ERROR_MESSAGE());

	ROLLBACK TRANSACTION;

	SET @NotificationTypeId_ = (SELECT TOP 1 NotificationTypeId FROM ProcessLogs..NotificationTypes WHERE NotificationTypeDescription = 'Error Report'); --Error report
	SET @NotificationSeverityTypeId_ = (SELECT TOP 1 NotificationSeverityTypeId FROM ProcessLogs..NotificationSeverityTypes WHERE NotificationSeverityTypeDescription = 'Critical'); --Critical
		
	INSERT INTO ProcessLogs..ProcessLogs (StartedOn, EndedOn, ScriptId, Region, RunBy) VALUES(GETDATE(),GETDATE(),@ScriptId,'cornerstone',SUSER_SNAME());
	SET @ProcessLogId_ = SCOPE_IDENTITY()

	INSERT INTO ProcessLogs..ProcessNotifications (NotificationTypeId,NotificationSeverityTypeId,ProcessLogId, ResolvedAt, ResolvedBy) VALUES(@NotificationTypeId_,@NotificationSeverityTypeId_,@ProcessLogId_, NULL, NULL)
	SET @ProcessNotificationId_ = SCOPE_IDENTITY()

	INSERT INTO CLS.[log].ProcessLogMessages (ProcessNotificationId, LogMessage) VALUES(@ProcessNotificationId_,@EM_);

	THROW;
END CATCH;


/*AGING360_SENT_TO_DMCS*/
BEGIN TRY
	BEGIN TRANSACTION

	TRUNCATE TABLE Aging360SentToDMCS; --TODO: uncomment for prod
	--EXEC dbo.spTruncate 'Aging360SentToDMCS'; --for testing (most users don't have permission to run TRUNCATE so this gets around that for testing

	INSERT INTO Aging360SentToDMCS
		(
			BF_SSN,
			LN_SEQ,
			AGING_DATE
		)
		SELECT
			LN10.BF_SSN,
			LN10.LN_SEQ,
			AY10.LD_ATY_REQ_RCV AS AGING_DATE -- max LD_ATY_REQ_RCV for DCSTR ARC
		FROM
			CDW..LN10_LON LN10
			INNER JOIN CDW..LN16_LON_DLQ_HST LN16
				ON LN16.BF_SSN = LN10.BF_SSN
				AND LN16.LN_SEQ = LN10.LN_SEQ
		-- AY10: max LD_ATY_REQ_RCV for DCSTR ARC
			INNER JOIN
			( 
				SELECT
					AY10I.BF_SSN,
					MAX(AY10I.LD_ATY_REQ_RCV) AS LD_ATY_REQ_RCV
				FROM
					CDW..AY10_BR_LON_ATY AY10I
				WHERE
					AY10I.PF_REQ_ACT = 'DCSTR'
					AND AY10I.LC_STA_ACTY10 = 'A'
				GROUP BY
					AY10I.BF_SSN
			) AS AY10
				ON AY10.BF_SSN = LN10.BF_SSN
		--AY10Ex: select borrowers with DCSLD activity records
			LEFT JOIN 
			( 
				SELECT
					AY10E.BF_SSN,
					MAX(AY10E.LD_ATY_REQ_RCV) AS LD_ATY_REQ_RCV
				FROM
					CDW..AY10_BR_LON_ATY AY10E
				WHERE
					AY10E.PF_REQ_ACT = 'DCSLD'
					AND AY10E.LC_STA_ACTY10 = 'A'
				GROUP BY
				AY10E.BF_SSN
			) AY10Ex
				ON AY10Ex.BF_SSN = AY10.BF_SSN
				AND AY10Ex.LD_ATY_REQ_RCV > AY10.LD_ATY_REQ_RCV
		WHERE
			LN16.LN_DLQ_MAX >= 360
			AND LN16.LC_STA_LON16 = '1'
			AND LN10.LC_STA_LON10 = 'D' --deconverted (sent to DMCS)
			AND LN10.LA_CUR_PRI > 0.00
			AND LN10.LC_SST_LON10 IN ('5','7')
			AND AY10.BF_SSN IS NOT NULL
			AND AY10Ex.BF_SSN IS NULL
	;

	COMMIT TRANSACTION;

END TRY
--write message to process logger if an error occurs
BEGIN CATCH
	SET @EM_ = @ScriptId + '  Aging360SentToDMCS table load encountered an error.  Transaction not committed. Error: ' + (SELECT ERROR_MESSAGE());

	ROLLBACK TRANSACTION;

	SET @NotificationTypeId_ = (SELECT TOP 1 NotificationTypeId FROM ProcessLogs..NotificationTypes WHERE NotificationTypeDescription = 'Error Report'); --Error report
	SET @NotificationSeverityTypeId_ = (SELECT TOP 1 NotificationSeverityTypeId FROM ProcessLogs..NotificationSeverityTypes WHERE NotificationSeverityTypeDescription = 'Critical'); --Critical
		
	INSERT INTO ProcessLogs..ProcessLogs (StartedOn, EndedOn, ScriptId, Region, RunBy) VALUES(GETDATE(),GETDATE(),@ScriptId,'cornerstone',SUSER_SNAME());
	SET @ProcessLogId_ = SCOPE_IDENTITY()

	INSERT INTO ProcessLogs..ProcessNotifications (NotificationTypeId,NotificationSeverityTypeId,ProcessLogId, ResolvedAt, ResolvedBy) VALUES(@NotificationTypeId_,@NotificationSeverityTypeId_,@ProcessLogId_, NULL, NULL)
	SET @ProcessNotificationId_ = SCOPE_IDENTITY()

	INSERT INTO CLS.[log].ProcessLogMessages (ProcessNotificationId, LogMessage) VALUES(@ProcessNotificationId_,@EM_);

	THROW;
END CATCH;


/*AGING360_NOT_ACCEPTED_BY_DMCS*/
BEGIN TRY
	BEGIN TRANSACTION

	TRUNCATE TABLE Aging360NotAcceptedByDMCS; --TODO: uncomment for prod
	--EXEC dbo.spTruncate 'Aging360NotAcceptedByDMCS'; --for testing (most users don't have permission to run TRUNCATE so this gets around that for testing

	INSERT INTO Aging360NotAcceptedByDMCS
		(
			BF_SSN,
			LN_SEQ,
			AGING_DATE
		)
		SELECT
				LN10.BF_SSN,
				LN10.LN_SEQ,
				AY10.LD_ATY_REQ_RCV AS AGING_DATE
			FROM
				CDW..LN10_LON LN10
				INNER JOIN CDW..LN16_LON_DLQ_HST LN16
					ON LN16.BF_SSN = LN10.BF_SSN
					AND LN16.LN_SEQ = LN10.LN_SEQ
			--AY10 select max LD_ATY_REQ_RCV of DCSRR ARCs
				INNER JOIN 
				(
					SELECT
						ATY.BF_SSN,
						ATY.LN_ATY_SEQ,  -- LN_ATY_SEQ for max LD_ATY_REQ_RCV for DCSRR ARC
						MAXARC.LD_ATY_REQ_RCV
					FROM
						CDW..AY10_BR_LON_ATY ATY
					--MAXARC: max LD_ATY_REQ_RCV for DCSRR ARC
						INNER JOIN
						( 
							SELECT 
								BF_SSN,
								MAX(LD_ATY_REQ_RCV) AS LD_ATY_REQ_RCV
							FROM
								CDW..AY10_BR_LON_ATY
							WHERE
								PF_REQ_ACT = 'DCSRR'
								AND LC_STA_ACTY10 = 'A' 
							GROUP BY
								BF_SSN
						) MAXARC
							ON MAXARC.BF_SSN = ATY.BF_SSN
							AND MAXARC.LD_ATY_REQ_RCV = ATY.LD_ATY_REQ_RCV
						WHERE
							ATY.PF_REQ_ACT = 'DCSRR'
							AND ATY.LC_STA_ACTY10 = 'A'
				) AY10
					ON AY10.BF_SSN = LN10.BF_SSN
				INNER JOIN CDW..AY15_ATY_CMT AY15
					ON AY15.BF_SSN = AY10.BF_SSN
					AND AY15.LN_ATY_SEQ = AY10.LN_ATY_SEQ	
				INNER JOIN CDW..AY20_ATY_TXT AY20
					ON AY20.BF_SSN = AY10.BF_SSN
					AND AY20.LN_ATY_SEQ = AY10.LN_ATY_SEQ
					AND AY20.LN_ATY_CMT_SEQ = AY15.LN_ATY_CMT_SEQ
			--AY10Complete: select borrowers with DCSTR activity records
				LEFT JOIN
				(
					SELECT
						AY10I.BF_SSN,
						MAX(AY10I.LD_ATY_REQ_RCV) AS LD_ATY_REQ_RCV --max LD_ATY_REQ_RCV for DCSTR ARC
					FROM
						CDW..AY10_BR_LON_ATY AY10I
					WHERE
						AY10I.PF_REQ_ACT = 'DCSTR'
						AND AY10I.LC_STA_ACTY10 = 'A'
					GROUP BY
						BF_SSN
				) AS AY10Complete
					ON AY10Complete.BF_SSN = AY10.BF_SSN
					AND AY10Complete.LD_ATY_REQ_RCV > AY10.LD_ATY_REQ_RCV
		WHERE
			AY15.LC_STA_AY15 = 'A'
			AND RIGHT(AY20.LX_ATY,4) != '0031'
			AND LN16.LN_DLQ_MAX >= 360
			AND LN16.LC_STA_LON16 = '1'
			AND LN10.LC_STA_LON10 != 'D' --deconverted
			AND LN10.LA_CUR_PRI > 0.00
			AND AY10Complete.BF_SSN IS NULL
	;

	COMMIT TRANSACTION;

END TRY
--write message to process logger if an error occurs
BEGIN CATCH
	SET @EM_ = @ScriptId + '  Aging360NotAcceptedByDMCS table load encountered an error.  Transaction not committed. Error: ' + (SELECT ERROR_MESSAGE());

	ROLLBACK TRANSACTION;

	SET @NotificationTypeId_ = (SELECT TOP 1 NotificationTypeId FROM ProcessLogs..NotificationTypes WHERE NotificationTypeDescription = 'Error Report'); --Error report
	SET @NotificationSeverityTypeId_ = (SELECT TOP 1 NotificationSeverityTypeId FROM ProcessLogs..NotificationSeverityTypes WHERE NotificationSeverityTypeDescription = 'Critical'); --Critical
		
	INSERT INTO ProcessLogs..ProcessLogs (StartedOn, EndedOn, ScriptId, Region, RunBy) VALUES(GETDATE(),GETDATE(),@ScriptId,'cornerstone',SUSER_SNAME());
	SET @ProcessLogId_ = SCOPE_IDENTITY()

	INSERT INTO ProcessLogs..ProcessNotifications (NotificationTypeId,NotificationSeverityTypeId,ProcessLogId, ResolvedAt, ResolvedBy) VALUES(@NotificationTypeId_,@NotificationSeverityTypeId_,@ProcessLogId_, NULL, NULL)
	SET @ProcessNotificationId_ = SCOPE_IDENTITY()

	INSERT INTO CLS.[log].ProcessLogMessages (ProcessNotificationId, LogMessage) VALUES(@ProcessNotificationId_,@EM_);

	THROW;
END CATCH;