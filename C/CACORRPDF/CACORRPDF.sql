--CACORRPDF.sql: CA Correspondence Response - Default Services
BEGIN TRY
	BEGIN TRANSACTION

		DROP TABLE IF EXISTS #lettersToAdd;

		DECLARE @ARC_InvalidAddress VARCHAR(5) = 'CAINV', --California invalid address arc
				@Comment VARCHAR(255) = 'CA correspondence response was attempted but the address is invalid',
				@ScriptId VARCHAR(10) = 'CACORRPDF',
				@NOW DATETIME = GETDATE(),
				@LetterId INT = 
				(
					SELECT 
						LetterId 
					FROM 
						ULS.[print].Letters 
					WHERE 
						Letter = 'RQRCVDLGP'
				);

		DECLARE @TODAY DATE = @NOW,
				@ScriptDataIdBorrower INT = 
				(
					SELECT 
						ScriptDataId 
					FROM 
						ULS.[print].ScriptData 
					WHERE 
						ScriptID = @ScriptId
						AND LetterId = @LetterId 
						AND IsEndorser = 0
				),
				@ScriptDataIdCoBorrower INT = 
				(
					SELECT 
						ScriptDataId 
					FROM 
						ULS.[print].ScriptData 
					WHERE 
						ScriptID = @ScriptId
						AND LetterId = @LetterId 
						AND IsEndorser = 1
				);
		--select @ARC_InvalidAddress, @Comment, @ScriptId, @NOW, @TODAY, @LetterId, @ScriptDataIdBorrower, @ScriptDataIdCoBorrower --TEST

		SELECT
			'Borrower' AS Pop,
			PD01.DF_SPE_ACC_ID AS AccountNumber,
			PD01.DF_PRS_ID AS BorrowerSsn,
			NULL AS CoBorrowerSsn, --needed for union with coborrower dataset later
			'Ecorr@UHEAA.org' AS EmailAddress, --email address not needed as not doing Ecorr for this group so default substituted for NULL
			@ScriptDataIdBorrower AS ScriptDataId,
			CentralData.dbo.CreateACSKeyline(PD01.DF_PRS_ID,'B','L') + ',' 
				+ RTRIM(PD01.DM_PRS_1) + ' ' 
				+ RTRIM(PD01.DM_PRS_LST) + ','
				+ RTRIM(PD03.DX_STR_ADR_1) + ','
				+ RTRIM(PD03.DX_STR_ADR_2) + ','
				+ RTRIM(PD03.DM_CT) + ','
				+ RTRIM(PD03.DC_DOM_ST) + ','
				+ RTRIM(PD03.DF_ZIP) + ','
				+ ',' --space holder for foreign state which OneLINK doesn't have
				+ RTRIM(PD03.DM_FGN_CNY) + ','
				+ RTRIM(PD01.DF_SPE_ACC_ID) + ','
				+ 'MA2329' AS LetterData,
			'MA2329' AS CostCenter,
			IIF(PD03.DI_VLD_ADR = 'Y', 0, 1) AS InValidAddress,
			SD.DoNotProcessEcorr,
			0 AS OnEcorr,
			IIF(ASDM.ArcId IS NOT NULL, 1, 0) AS ArcNeeded,
			IIF(SD.DocIdId IS NOT NULL, 1, 0) AS ImagingNeeded,
			DID.DocumentId,
			DP.DocumentProcessedId
		INTO
			#lettersToAdd
		FROM
			ULS.docid.DocumentResponseStateMapping_OneLINK DRSM 
			INNER JOIN ULS.docid.DocIdMapping DID
				ON DID.DocIdMappingId = DRSM.DocIdMappingId
			INNER JOIN ULS.docid.Documents D
				ON D.DocumentsId = DID.DocumentId
			INNER JOIN ULS.[print].ScriptData SD
				ON SD.ScriptDataId = @ScriptDataIdBorrower
				AND SD.LetterId = @LetterId
				AND SD.Active = 1 --active flag
			INNER JOIN ULS.[print].ArcScriptDataMapping ASDM
				ON ASDM.ScriptDataId = @ScriptDataIdBorrower
			INNER JOIN ODW..PD03_PRS_ADR_PHN PD03
				ON PD03.DC_DOM_ST = DRSM.StateCode
				AND PD03.DC_ADR = 'L' --legal
			INNER JOIN ODW..PD01_PDM_INF PD01
				ON PD01.DF_PRS_ID = PD03.DF_PRS_ID
				AND PD01.DD_DTH IS NULL --exclude verified death
			INNER JOIN ULS.docid.DocumentsProcessed DP
				ON DP.AccountIdentifier = PD03.DF_PRS_ID
				AND DP.DocumentsId = D.DocumentsId
				AND CAST(PD03.DF_LST_DTS_PD03 AS DATE) <= CAST(DP.AddedAt AS DATE)
				AND CAST(DP.AddedAt AS DATE) > CONVERT(DATE,'20180701')
			INNER JOIN ODW..GA01_APP GA01
				ON GA01.DF_PRS_ID_BR = PD01.DF_PRS_ID
			INNER JOIN ODW..GA14_LON_STA GA14
				ON GA14.AF_APL_ID = GA01.AF_APL_ID 
				AND GA14.AC_STA_GA14 = 'A' --active
			LEFT JOIN ULS.docid.DocumentsProcessedResponse_OneLINK DPR
				ON DPR.DocumentsProcessedId = DP.DocumentProcessedId
				AND DPR.PrintProcessingId IS NOT NULL
			LEFT JOIN
			(--exclude verified death
				SELECT 
					AF_APL_ID 
				FROM 
					ODW..GA14_LON_STA
				WHERE 
					AC_STA_GA14 = 'A' --active
					AND AC_LON_STA_REA = 'DE' --death
					AND AC_LON_STA_TYP IN ('CR','CP')
			) AS CertifiedDeath
				ON GA14.AF_APL_ID = CertifiedDeath.AF_APL_ID
		WHERE
			CertifiedDeath.AF_APL_ID IS NULL --exclude verified death
			AND DPR.DocumentsProcessedId IS NULL --not already processed

		UNION ALL

		--CO-BORROWER:
		SELECT
			'Co-borrower' AS Pop,
			--BorrPD01.DF_SPE_ACC_ID AS AccountNumber, --borrower acct#
			PD01.DF_SPE_ACC_ID AS AccountNumber, --coborrower acct#
			BorrPD01.DF_PRS_ID AS BorrowerSsn,
			GA01.DF_PRS_ID_EDS AS CoBorrowerSsn, --coborrower SSN
			'Ecorr@UHEAA.org' AS EmailAddress, --email address not needed as not doing Ecorr for this group so default substituted for NULL
			@ScriptDataIdCoBorrower AS ScriptDataId,
			CentralData.dbo.CreateACSKeyline(PD01.DF_PRS_ID,'B','L') + ',' 
				+ RTRIM(PD01.DM_PRS_1) + ' ' 
				+ RTRIM(PD01.DM_PRS_LST) + ','
				+ RTRIM(EndPD03.DX_STR_ADR_1) + ','
				+ RTRIM(EndPD03.DX_STR_ADR_2) + ','
				+ RTRIM(EndPD03.DM_CT) + ','
				+ RTRIM(EndPD03.DC_DOM_ST) + ','
				+ RTRIM(EndPD03.DF_ZIP) + ','
				+ ',' --space holder for foreign state which OneLINK doesn't have
				+ RTRIM(EndPD03.DM_FGN_CNY) + ','
				+ RTRIM(BorrPD01.DF_SPE_ACC_ID) + ',' --borrower acct#
				+ RTRIM(GA01.DF_PRS_ID_EDS) + ',' --coborrower ssn
				+ 'MA2329' AS LetterData,
			'MA2329' AS CostCenter,
			IIF(EndPD03.DI_VLD_ADR = 'Y', 0, 1) AS InValidAddress,
			SD.DoNotProcessEcorr,
			0 AS OnEcorr,
			IIF(ASDM.ArcId IS NOT NULL, 1, 0) AS ArcNeeded,
			IIF(SD.DocIdId IS NOT NULL, 1, 0) AS ImagingNeeded,
			DID.DocumentId,
			DP.DocumentProcessedId
		FROM
			ULS.docid.DocumentResponseStateMapping_OneLINK DRSM
			INNER JOIN ULS.docid.DocIdMapping DID
				ON DID.DocIdMappingId = DRSM.DocIdMappingId
			INNER JOIN ULS.docid.Documents D
				ON D.DocumentsId = DID.DocumentId
			INNER JOIN ULS.[print].ScriptData SD
				ON SD.ScriptDataId = @ScriptDataIdCoBorrower --join to coborrower (resolves git mapping issue)
				AND SD.LetterId = @LetterId
				AND SD.Active = 1 --active
			INNER JOIN ULS.[print].ArcScriptDataMapping ASDM
				ON ASDM.ScriptDataId = @ScriptDataIdCoBorrower
			INNER JOIN ODW..PD03_PRS_ADR_PHN EndPD03
				ON EndPD03.DC_DOM_ST = DRSM.StateCode
				AND EndPD03.DC_ADR = 'L' --legal
			INNER JOIN ODW..GA01_APP GA01
				ON GA01.DF_PRS_ID_EDS = EndPD03.DF_PRS_ID 
				AND GA01.AC_APL_TYP = 'C'
			INNER JOIN ODW..PD01_PDM_INF PD01 --endorser (coborrower) demographics
				ON PD01.DF_PRS_ID = GA01.DF_PRS_ID_EDS
				AND PD01.DD_DTH IS NULL --exclude verified death
			INNER JOIN ODW..PD01_PDM_INF BorrPD01 --borrower demographics
				ON BorrPD01.DF_PRS_ID = GA01.DF_PRS_ID_BR
			INNER JOIN ULS.docid.DocumentsProcessed DP
				ON DP.AccountIdentifier = GA01.DF_PRS_ID_BR
				AND DP.DocumentsId = D.DocumentsId
				AND CAST(EndPD03.DF_LST_DTS_PD03 AS DATE) <= CAST(DP.AddedAt AS DATE)
				AND CAST(DP.AddedAt AS DATE) > CONVERT(DATE,'20180701')
			INNER JOIN ODW..GA14_LON_STA GA14
				ON GA14.AF_APL_ID = GA01.AF_APL_ID 
				AND GA14.AC_STA_GA14 = 'A' --active
			LEFT JOIN ULS.docid.DocumentsProcessedResponse_OneLINK DPR
				ON DPR.DocumentsProcessedId = DP.DocumentProcessedId
				AND DPR.PrintProcessingId IS NOT NULL
			LEFT JOIN
			(--exclude verified death
				SELECT 
					AF_APL_ID 
				FROM 
					ODW..GA14_LON_STA
				WHERE 
					AC_STA_GA14 = 'A' --active
					AND AC_LON_STA_REA = 'DE' --death
					AND AC_LON_STA_TYP IN ('CR','CP')
			) AS CertifiedDeath
				ON GA14.AF_APL_ID = CertifiedDeath.AF_APL_ID
		WHERE
			CertifiedDeath.AF_APL_ID IS NULL --exclude verified death
			AND DPR.DocumentsProcessedResponseId IS NULL --not already processed
		;
		--select distinct * from #lettersToAdd --TEST

		/*************** ADD VALID ADDRESSES TO PRINT PROCESSING TABLE ***************/
		DECLARE @PrintProcessingIds TABLE
		(
			PrintProcessingId INT,
			AccountNumber VARCHAR(10),
			LetterData VARCHAR(8000)
		);

		INSERT INTO ULS.[print].PrintProcessing
		(
			AccountNumber,
			EmailAddress, 
			ScriptDataId, 
			SourceFile, 
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
		OUTPUT 
			INSERTED.PrintProcessingId, 
			INSERTED.AccountNumber,
			INSERTED.LetterData
		INTO 
			@PrintProcessingIds
		(
			PrintProcessingId, 
			AccountNumber,
			LetterData
		)
		SELECT DISTINCT
			POP.AccountNumber,
			POP.EmailAddress,
			POP.ScriptDataId,
			NULL AS SourceFile,
			POP.LetterData,
			POP.CostCenter,
			POP.InValidAddress,
			POP.DoNotProcessEcorr,
			POP.OnEcorr,
			POP.ArcNeeded,
			POP.ImagingNeeded,
			@NOW,
			SUSER_SNAME()
		FROM			
			#lettersToAdd POP
			LEFT JOIN ULS.[print].PrintProcessing EXISTING_DATA
				ON  EXISTING_DATA.AccountNumber = POP.AccountNumber
				AND EXISTING_DATA.EmailAddress = POP.EmailAddress
				AND EXISTING_DATA.ScriptDataId = IIF(POP.Pop = 'Co-borrower', @ScriptDataIdCoBorrower, @ScriptDataIdBorrower)
				AND EXISTING_DATA.LetterData = POP.LetterData
				AND EXISTING_DATA.CostCenter = POP.CostCenter
				AND EXISTING_DATA.InValidAddress = POP.InValidAddress
				AND EXISTING_DATA.DoNotProcessEcorr = POP.DoNotProcessEcorr
				AND EXISTING_DATA.OnEcorr = POP.OnEcorr
				AND EXISTING_DATA.ArcNeeded = POP.ArcNeeded
				AND EXISTING_DATA.ImagingNeeded = POP.ImagingNeeded
				AND CONVERT(DATE,EXISTING_DATA.AddedAt) = @TODAY
				AND EXISTING_DATA.EcorrDocumentCreatedAt IS NULL
				AND EXISTING_DATA.PrintedAt IS NULL
				AND EXISTING_DATA.DeletedAt IS NULL
		WHERE
			EXISTING_DATA.AccountNumber IS NULL --exclude existing data
			AND POP.InValidAddress = 0 --has valid address
		;
		--select * from @PrintProcessingIds --TEST
		--select * from ULS.[print].PrintProcessing WHERE AddedBy = 'UHEAA\jnolasco' --TEST
		
		/*************** INSERT TO DPR ***************/
		
		INSERT INTO ULS.docid.DocumentsProcessedResponse_OneLINK
		(
			DocumentsProcessedId, 
			PrintProcessingId
		)
		SELECT DISTINCT
			LTA.DocumentProcessedId,
			PP.PrintProcessingId
		FROM
			@PrintProcessingIds PP
			INNER JOIN #lettersToAdd LTA
				ON LTA.AccountNumber = PP.AccountNumber
				AND LTA.LetterData = PP.LetterData
		WHERE
			LTA.InValidAddress = 0 --has valid address
		;
		--select * from ULS.docid.DocumentsProcessedResponse_OneLINK --TEST

		/*************** ADD INVALID ADDRESSES TO ARC ADD PROCESSING AND DOCUMENTS PROCESSED REPSONSE ***************/

		DECLARE @ArcAddProcessingIds TABLE
		(
			ArcAddProcessingId INT,
			AccountNumber VARCHAR(10),
			RecipientId VARCHAR(9),
			IsEndorser BIT
		);

		--add invalid address ARC
		INSERT INTO ULS..ArcAddProcessing 
		(
			ArcTypeId, 
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
			ProcessingAttempts, 
			CreatedAt, 
			CreatedBy
		)
		OUTPUT
			INSERTED.ArcAddProcessingId,
			INSERTED.AccountNumber,
			INSERTED.RecipientId,
			INSERTED.IsEndorser
		INTO
			@ArcAddProcessingIds
		(
			ArcAddProcessingId,
			AccountNumber,
			RecipientId,
			IsEndorser
		)
		SELECT DISTINCT
			6 AS ArcTypeId,
			LTA.AccountNumber,
			IIF(LTA.Pop = 'Co-borrower', LTA.CoBorrowerSsn, '') AS RecipientId,
			@ARC_InvalidAddress AS ARC,
			'AM' AS ActivityType,
			'10' AS ActivityContact,
			@ScriptId AS ScriptId,
			@NOW AS ProcessOn,
			@Comment AS Comment,
			0 AS IsReference,
			IIF(LTA.Pop = 'Co-borrower', 1, 0) AS IsEndorser,
			0 AS ProcessingAttempts,
			@NOW AS CreatedAt,
			SUSER_SNAME() AS CreatedBy
		FROM
			#lettersToAdd LTA
			LEFT JOIN ULS..ArcAddProcessing EXISTING_AAP
				ON EXISTING_AAP.AccountNumber = LTA.AccountNumber
				AND EXISTING_AAP.RecipientId = IIF(LTA.Pop = 'Co-borrower', LTA.CoBorrowerSsn, '')
				AND EXISTING_AAP.IsEndorser = IIF(LTA.Pop = 'Co-borrower', 1, 0)
				AND EXISTING_AAP.ARC = @ARC_InvalidAddress
				AND EXISTING_AAP.ScriptId = @ScriptId
				AND COALESCE(EXISTING_AAP.Comment,'') = @Comment
				AND CAST(EXISTING_AAP.CreatedAt AS DATE) = @TODAY --prevents multiple arcs from being added on the same day
			LEFT JOIN ULS.docid.DocumentsProcessedResponse_OneLINK EXISTING_DPR
				ON EXISTING_DPR.DocumentsProcessedId = LTA.DocumentProcessedId
				AND EXISTING_DPR.InvalidAddressArcAddProcessingId IS NOT NULL
		WHERE
			EXISTING_AAP.AccountNumber IS NULL --No matching existing record
			AND LTA.InValidAddress = 1 --has invalid address
			AND EXISTING_DPR.DocumentsProcessedId IS NULL --not already processed
		;
		--select * from @ArcAddProcessingIds; --TEST
		--select * from ULS..ArcAddProcessing where CreatedBy = 'UHEAA\jnolasco' --TEST
		--select * from #lettersToAdd --TEST
		--update uls..ArcAddProcessing set CreatedAt = DATEADD(day,-1,createdat)  where CreatedBy = 'UHEAA\jnolasco' --TEST

		INSERT INTO ULS.docid.DocumentsProcessedResponse_OneLINK
		(
			DocumentsProcessedId,
			InvalidAddressArcAddProcessingId
		)
		SELECT DISTINCT
			LTA.DocumentProcessedId AS DocumentsProcessedId,
			AAPI.ArcAddProcessingId AS InvalidAddressArcAddProcessingId
		FROM
			@ArcAddProcessingIds AAPI
			INNER JOIN #lettersToAdd LTA
				ON AAPI.AccountNumber = LTA.AccountNumber
				AND AAPI.RecipientId = IIF(LTA.Pop = 'Co-borrower', LTA.CoBorrowerSsn, '')
				AND AAPI.IsEndorser = IIF(LTA.Pop = 'Co-borrower', 1, 0)
		WHERE
			LTA.InValidAddress = 1 --has invalid address
		;
		--select * from ULS.docid.DocumentsProcessedResponse_OnelINK where InvalidAddressArcAddProcessingId is not null --TEST
		
	COMMIT TRANSACTION
END TRY
BEGIN CATCH
	DECLARE @EM VARCHAR(4000) = @ScriptId + ' encountered an error.  Transaction not committed. Error: ' + (SELECT ERROR_MESSAGE());

	ROLLBACK TRANSACTION;

	DECLARE @ProcessLogId INT;
	DECLARE @ProcessNotificationId INT;
	DECLARE @NotificationTypeId INT = (SELECT TOP 1 NotificationTypeId FROM ProcessLogs..NotificationTypes WHERE NotificationTypeDescription = 'Error Report'); --Error report
	DECLARE @NotificationSeverityTypeId INT = (SELECT TOP 1 NotificationSeverityTypeId FROM ProcessLogs..NotificationSeverityTypes WHERE NotificationSeverityTypeDescription = 'Critical'); --Critical
		
	INSERT INTO ProcessLogs..ProcessLogs (StartedOn, EndedOn, ScriptId, Region, RunBy) VALUES(@NOW,@NOW,@ScriptId,'uheaa',SUSER_SNAME());
	SET @ProcessLogId = SCOPE_IDENTITY()

	INSERT INTO ProcessLogs..ProcessNotifications (NotificationTypeId,NotificationSeverityTypeId,ProcessLogId, ResolvedAt, ResolvedBy) VALUES(@NotificationTypeId,@NotificationSeverityTypeId,@ProcessLogId, NULL, NULL)
	SET @ProcessNotificationId = SCOPE_IDENTITY()

	INSERT INTO ULS.[log].ProcessLogMessages (ProcessNotificationId, LogMessage) VALUES(@ProcessNotificationId,@EM);

	THROW;
END CATCH;