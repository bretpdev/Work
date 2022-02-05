BEGIN TRY
	BEGIN TRANSACTION

		DROP TABLE IF EXISTS #DEMO;

		DECLARE @NOW DATETIME = GETDATE();
		DECLARE @TODAY DATE = @NOW;
		DECLARE	@CDATE DATE = (IIF(DATENAME(WEEKDAY,@TODAY) = 'Monday', DATEADD(DAY,-3,@TODAY), DATEADD(DAY,-1,@TODAY))),--if run on Monday then do Friday, else do yesterday
				@JobId VARCHAR(10) = 'UTLWS11'
		--select @NOW,@TODAY,@CDATE,@JobId

		DECLARE @LetterArcs TABLE 
		(
			Report TINYINT,
			Letter VARCHAR(10),
			Arc VARCHAR(10)
		);
		INSERT INTO @LetterArcs
		(
			Report,Letter,Arc
		)
		VALUES
		(2	,'ECONDEF'	  ,'G7077'),
		(3	,'UNEMPDEF'	  ,'G708C'),
		(4  ,'TMPHRDFORB' ,'H742A'),
		(5	,'AUTOPAYREQ' ,'DRNDP'),
		(6  ,'MISSDEF'	  ,'DMISH'), --RETIRED
		(7	,'INCSEN'	  ,'LS261'),
		(8	,'CBPCNF'	  ,'PHNPL'),
		(9	,'INDEFER'	  ,'ALSCH'),
		(10	,'UNDEFTILP'  ,'TPUNL'),
		(11	,'ECODEFTILP' ,'TPECL'),
		(12 ,'TILPMISDEF' ,'TPMIS'), --RETIRED?
		(13	,'INSCHLTILP' ,'TPISD'),
		(14	,'MILDEFTILP' ,'TPMDL'),
		(15	,'TPBDADEN'	  ,'TPBBD'),
		(16	,'TPBBDAPP'	  ,'TPBBA'),
		(17	,'APBBAPPDEN' ,'APBDN'),
		(18	,'FORBSPCONS' ,'THFSC'),
		(19	,'NMCFLT'	  ,'NAMCH'),
		(25	,'MILDEF'	  ,'MILDF'),
		(26 ,'TPDCSVCVRL' ,'TPDCL'), --RETIRED
		(27	,'IBRCOVLTR'  ,'IBRCL'),
		(28	,'TPAMD'	  ,'RQ003'),
		(29	,'RPFCVR'	  ,'IVRM1')
		;
		--SELECT * FROM @LetterArcs order by Report  --TEST

		--demographic data set
		SELECT
			PD10.DF_PRS_ID AS BF_SSN
			,AY10.PF_REQ_ACT
			,PD10.DF_SPE_ACC_ID
			,PD10.DM_PRS_MID
			,PD10.DM_PRS_1
			,RTRIM(CONCAT(RTRIM(PD10.DM_PRS_LST),' ',PD10.DM_PRS_LST_SFX)) AS DM_PRS_LST
			,PD30.DX_STR_ADR_1
			,PD30.DX_STR_ADR_2
			,PD30.DX_STR_ADR_3
			,PD30.DM_CT
			,PD30.DC_DOM_ST
			,PD30.DF_ZIP_CDE
			,PD30.DM_FGN_CNY
			,PD30.DM_FGN_ST
			,PD30.DC_ADR
			,AY10.LD_ATY_REQ_RCV
			,SUBSTRING(AY20.LX_ATY, 1, 9) AS PMT_AMT
		INTO
			#DEMO
		FROM
			UDW..LN10_LON LN10
			INNER JOIN UDW..AY10_BR_LON_ATY AY10
				ON LN10.BF_SSN = AY10.BF_SSN
			INNER JOIN @LetterArcs LA
				ON LA.Arc = AY10.PF_REQ_ACT
			INNER JOIN UDW..PD10_PRS_NME PD10
				ON AY10.BF_SSN = PD10.DF_PRS_ID
			INNER JOIN UDW..PD30_PRS_ADR PD30
				ON PD10.DF_PRS_ID = PD30.DF_PRS_ID
			LEFT JOIN UDW..AY15_ATY_CMT AY15
				ON AY10.BF_SSN = AY15.BF_SSN
				AND AY10.LN_ATY_SEQ = AY15.LN_ATY_SEQ
				AND AY15.LC_STA_AY15 = 'A' 
			LEFT JOIN UDW..AY20_ATY_TXT AY20
				ON AY15.BF_SSN = AY20.BF_SSN
				AND AY15.LN_ATY_SEQ = AY20.LN_ATY_SEQ
				AND AY15.LN_ATY_CMT_SEQ = AY20.LN_ATY_CMT_SEQ
		WHERE 
			AY10.LD_ATY_REQ_RCV = @CDATE
			AND PD30.DC_ADR = 'L' --legal
			AND PD10.DF_PRS_ID LIKE ('[0-9]%')
			AND PD30.DF_PRS_ID LIKE ('[0-9]%')
			AND LN10.LA_CUR_PRI > 0.00 
			AND LN10.LC_STA_LON10 = 'R' 
			AND AY10.LC_STA_ACTY10 = 'A' 
		;
		--select distinct * from #DEMO --TEST

		INSERT INTO ULS.[print].PrintProcessing
		(
			AccountNumber, 
			EmailAddress, 
			ScriptDataId, 
			LetterData, 
			CostCenter, 
			InValidAddress, 
			DoNotProcessEcorr, 
			OnEcorr, 
			ArcNeeded, 
			ImagingNeeded, 
			AddedAt, 
			AddedBy
		)
		SELECT DISTINCT
			POPCALC.DF_SPE_ACC_ID AS AccountNumber,
			EM.EmailAddress,
			SD.ScriptDataId,
			CONCAT(--file header: SSN,Arc,AN,FirstName,MI,LastName,Address1,Address2,Address3,City,State,Zip,Country,Foreign State,KeyLine,Arc Date,Payment,State_Ind,COST_CENTER_CODE,INTEREST
					POPCALC.BF_SSN
					,',',POPCALC.PF_REQ_ACT --Arc
					,',',POPCALC.DF_SPE_ACC_ID --AN
					,',',RTRIM(POPCALC.DM_PRS_1) --FirstName
					,',',RTRIM(POPCALC.DM_PRS_MID) --MI
					,',',POPCALC.DM_PRS_LST --LastName
					,',',RTRIM(POPCALC.DX_STR_ADR_1) --Address1
					,',',RTRIM(POPCALC.DX_STR_ADR_2) --Address2
					,',',RTRIM(POPCALC.DX_STR_ADR_3) --Address3
					,',',RTRIM(POPCALC.DM_CT) --City
					,',',POPCALC.DC_DOM_ST --State
					,',',RTRIM(POPCALC.DF_ZIP_CDE) --Zip
					,',',RTRIM(POPCALC.DM_FGN_CNY) --Country
					,',',RTRIM(POPCALC.DM_FGN_ST) --Foreign State
					,',',POPCALC.ACSKEY --KeyLine
					,',',POPCALC.LD_ATY_REQ_RCV--Arc Date
					,',',POPCALC.PMT --Payment
					,',',POPCALC.DC_DOM_ST --State_Ind
					,',',POPCALC.COST_CENTER_CODE
					,',',POPCALC.IPMT_AMT --INTEREST
				)
			AS LetterData,
			POPCALC.COST_CENTER_CODE AS CostCenter,
			0 AS InValidAddress,
			0 AS DoNotProcessEcorr,
			0 AS OnEcorr,
			0 AS ArcNeeded,
			0 AS ImagingNeeded,
			@NOW AS AddedAt,
			@JobId AS AddedBy
		FROM
			(--calculate population payment and interest
				SELECT
					CentralData.dbo.CreateACSKeyline(D.BF_SSN, 'B', D.DC_ADR) AS ACSKEY
					,D.BF_SSN
					,D.PF_REQ_ACT
					,D.DF_SPE_ACC_ID
					,D.DM_PRS_MID
					,D.DM_PRS_1
					,DM_PRS_LST
					,D.DX_STR_ADR_1
					,D.DX_STR_ADR_2
					,D.DX_STR_ADR_3
					,D.DM_CT
					,D.DC_DOM_ST
					,D.DF_ZIP_CDE
					,D.DM_FGN_CNY
					,D.DM_FGN_ST
					,D.DC_ADR
					,D.LD_ATY_REQ_RCV
					,D.PMT_AMT
					--,IIF(D.DC_DOM_ST IN ('FC',''), 1, 2) AS SVAR --sort variable used in output (might not be needed) --TEST
					,'MA2324' AS COST_CENTER_CODE
					,(ISUM.CALC3 / 365 * 31 + 5) AS INTEREST
					,IIF(D.PF_REQ_ACT = 'PHNPL', SUBSTRING(PMT_AMT,2,8), NULL) AS IPMT_AMT
					,IIF(D.PF_REQ_ACT = 'IBAPV', (ISUM.CALC3 / 365 * 31 + 5), NULL) AS PMT
				FROM
					#DEMO D
					INNER JOIN
					(--interest summing
						SELECT DISTINCT
							BF_SSN,
							SUM(CALC1 * CALC2) AS CALC3
						FROM
							(--interest data set
								SELECT
									LN10.BF_SSN,
									LN10.LN_SEQ,
									(LN72.LR_ITR / 100) AS CALC1,
									LN10.LA_CUR_PRI AS CALC2
								FROM
									UDW..LN72_INT_RTE_HST LN72
									INNER JOIN UDW..LN10_LON LN10
										ON LN10.BF_SSN = LN72.BF_SSN
										AND LN10.LN_SEQ = LN72.LN_SEQ
									INNER JOIN #DEMO D
										ON D.BF_SSN = LN72.BF_SSN
								WHERE
									LN72.LC_STA_LON72 = 'A'
									AND LN72.LD_ITR_EFF_BEG <= @TODAY
									AND LN72.LD_ITR_EFF_END >= @TODAY
									AND LN10.LC_STA_LON10 = 'R' --released
									AND LN10.LA_CUR_PRI > 0.00
							) ISET
						GROUP BY
							BF_SSN
					) ISUM
						ON D.BF_SSN = ISUM.BF_SSN
			) POPCALC
			INNER JOIN UDW.calc.EmailAddress EM
				ON EM.DF_PRS_ID = POPCALC.BF_SSN
			INNER JOIN 
			(--get ScriptDataId
				SELECT
					ScriptDataId,
					L.Letter,
					LA.Arc
				FROM 
					ULS.[print].ScriptData SD
					INNER JOIN ULS.[print].Letters L
						ON L.LetterId = SD.LetterId
					INNER JOIN @LetterArcs LA
						ON LA.Letter = L.Letter
				WHERE
					SD.ScriptID = 'BTCHLTRS'
			) SD
				ON SD.Arc = POPCALC.PF_REQ_ACT
			LEFT JOIN ULS.[print].PrintProcessing ExistingData
				ON ExistingData.AccountNumber = POPCALC.DF_SPE_ACC_ID
				AND ExistingData.EmailAddress = EM.EmailAddress
				AND ExistingData.ScriptDataId = SD.ScriptDataId
				AND ExistingData.LetterData = 
					CONCAT(--file header: SSN,Arc,AN,FirstName,MI,LastName,Address1,Address2,Address3,City,State,Zip,Country,Foreign State,KeyLine,Arc Date,Payment,State_Ind,COST_CENTER_CODE,INTEREST
							POPCALC.BF_SSN
							,',',POPCALC.PF_REQ_ACT --Arc
							,',',POPCALC.DF_SPE_ACC_ID --AN
							,',',RTRIM(POPCALC.DM_PRS_1) --FirstName
							,',',RTRIM(POPCALC.DM_PRS_MID) --MI
							,',',POPCALC.DM_PRS_LST --LastName
							,',',RTRIM(POPCALC.DX_STR_ADR_1) --Address1
							,',',RTRIM(POPCALC.DX_STR_ADR_2) --Address2
							,',',RTRIM(POPCALC.DX_STR_ADR_3) --Address3
							,',',RTRIM(POPCALC.DM_CT) --City
							,',',POPCALC.DC_DOM_ST --State
							,',',RTRIM(POPCALC.DF_ZIP_CDE) --Zip
							,',',RTRIM(POPCALC.DM_FGN_CNY) --Country
							,',',RTRIM(POPCALC.DM_FGN_ST) --Foreign State
							,',',POPCALC.ACSKEY --KeyLine
							,',',POPCALC.LD_ATY_REQ_RCV--Arc Date
							,',',POPCALC.PMT --Payment
							,',',POPCALC.DC_DOM_ST --State_Ind
							,',',POPCALC.COST_CENTER_CODE
							,',',POPCALC.IPMT_AMT --INTEREST
						)
				AND ExistingData.CostCenter = POPCALC.COST_CENTER_CODE
				AND ExistingData.InValidAddress = 0
				AND ExistingData.DoNotProcessEcorr = 0
				AND ExistingData.OnEcorr = 0
				AND ExistingData.ArcNeeded = 0
				AND ExistingData.ImagingNeeded = 0
				AND (
						CONVERT(DATE,ExistingData.AddedAt) = @TODAY
						OR (
								ExistingData.EcorrDocumentCreatedAt IS NULL
								AND ExistingData.PrintedAt IS NULL
								AND ExistingData.DeletedAt IS NULL
							)
					)
		WHERE
			ExistingData.AccountNumber IS NULL
		;
		--select * from uls.[print].PrintProcessing where AddedBy = 'UTLWS11' --TEST
	COMMIT TRANSACTION
END TRY
BEGIN CATCH
	DECLARE @EM VARCHAR(4000) = @JobId + ' encountered an error.  Transaction not committed. Error: ' + (SELECT ERROR_MESSAGE());

	ROLLBACK TRANSACTION;

	DECLARE @ProcessLogId INT;
	DECLARE @ProcessNotificationId INT;
	DECLARE @NotificationTypeId INT = (SELECT TOP 1 NotificationTypeId FROM ProcessLogs..NotificationTypes WHERE NotificationTypeDescription = 'Error Report'); --Error report
	DECLARE @NotificationSeverityTypeId INT = (SELECT TOP 1 NotificationSeverityTypeId FROM ProcessLogs..NotificationSeverityTypes WHERE NotificationSeverityTypeDescription = 'Critical'); --Critical
		
	INSERT INTO ProcessLogs..ProcessLogs (StartedOn, EndedOn, ScriptId, Region, RunBy) VALUES(@NOW,@NOW,@JobId,'uheaa',SUSER_SNAME());
	SET @ProcessLogId = SCOPE_IDENTITY()

	INSERT INTO ProcessLogs..ProcessNotifications (NotificationTypeId,NotificationSeverityTypeId,ProcessLogId, ResolvedAt, ResolvedBy) VALUES(@NotificationTypeId,@NotificationSeverityTypeId,@ProcessLogId, NULL, NULL)
	SET @ProcessNotificationId = SCOPE_IDENTITY()

	INSERT INTO ULS.[log].ProcessLogMessages (ProcessNotificationId, LogMessage) VALUES(@ProcessNotificationId,@EM);

	THROW;
END CATCH;