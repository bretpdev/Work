--CACORRRSP.sql:  CA Correspondence Response - UHEAA
BEGIN TRY
	BEGIN TRANSACTION

		DROP TABLE IF EXISTS #lettersToAdd_BOR, #lettersToAdd_COB, #lettersToAdd;

		DECLARE @ARC_InvalidAddress VARCHAR(5) = 'CAINV', --California invalid address arc
				@Comment VARCHAR(255) = 'CA correspondence response was attempted but the address is invalid',
				@ScriptId VARCHAR(10) = 'CACORRRSP',
				@NOW DATETIME = GETDATE(),
				@LetterId INT = 
				(
					SELECT
						LetterId 
					FROM 
						ULS.[print].Letters 
					WHERE
						Letter = 'REQRCVDUH'
				);

		DECLARE @TODAY DATE = @NOW,
				@OneWeekLookback DATE = DATEADD(DAY,-7,@NOW),
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
		--select @ARC_InvalidAddress, @Comment, @ScriptId, @NOW, @LetterId, @TODAY, @OneWeekLookback, @ScriptDataIdBorrower, @ScriptDataIdCoBorrower --TEST

		SELECT
			'Borrower' AS Pop,
			PD10.DF_SPE_ACC_ID AS BorrowerAccountNumber,
			'' AS CoBorrowerAccountNumber, --needed for union with coborrower
			PD10.DF_PRS_ID AS BorrowerSsn,
			'' AS CoBorrowerSsn, --needed for union with coborrower
			COALESCE(PH05.DX_CNC_EML_ADR,'Ecorr@Uheaa.org') AS EmailAddress,
			PH05.DX_CNC_EML_ADR,
			@ScriptDataIdBorrower AS ScriptDataId,
			CONCAT
				(
					CentralData.dbo.CreateACSKeyline(PD10.DF_PRS_ID,'B','L'), ',',
					RTRIM(PD10.DM_PRS_1), ' ',
					RTRIM(PD10.DM_PRS_LST), RTRIM(CONCAT(' ', RTRIM(PD10.DM_PRS_LST_SFX))), ',',
					RTRIM(PD30.DX_STR_ADR_1), ',',
					RTRIM(PD30.DX_STR_ADR_2), ',',
					RTRIM(PD30.DM_CT), ',',
					RTRIM(PD30.DC_DOM_ST), ',',
					RTRIM(PD30.DF_ZIP_CDE), ',',
					RTRIM(PD30.DM_FGN_ST), ',',
					RTRIM(PD30.DM_FGN_CNY), ',',
					RTRIM(PD10.DF_SPE_ACC_ID), ',',
					'MA4119'
				) AS LetterData,
			'MA4119' AS CostCenter,
			IIF(--"InValidAddress" refers to not on ecorr OR no valid physical address
					PD30.DI_VLD_ADR = 'Y' --has valid legal address
					OR (--on ecorr:
							PH05.DI_VLD_CNC_EML_ADR = 'Y'
							AND PH05.DI_CNC_ELT_OPI = 'Y' 
							AND PH05.DX_CNC_EML_ADR IS NOT NULL
						)
				 , 0, 1
				) AS InValidAddress,
			SD.DoNotProcessEcorr,
			IIF(--on ecorr
					PH05.DI_VLD_CNC_EML_ADR = 'Y' 
					AND PH05.DI_CNC_ELT_OPI = 'Y' 
					AND PH05.DX_CNC_EML_ADR IS NOT NULL
				, 1, 0) AS OnEcorr,
			IIF(ASDM.ArcId IS NOT NULL, 1, 0) AS ArcNeeded,
			IIF(SD.DocIdId IS NOT NULL, 1, 0) AS ImagingNeeded,
			DID.DocumentId,
			AY10.LN_ATY_SEQ,
			NULL AS LN_SEQ,
			IIF(PD10.DF_PRS_ID LIKE 'P%', 1, 0) AS PFlag
		INTO
			#lettersToAdd_BOR
		FROM
			ULS.docid.DocumentResponseStateMapping DRSM
			INNER JOIN ULS.docid.DocIdMapping DID
				ON DID.DocIdMappingId = DRSM.DocIdMappingId
			INNER JOIN ULS.docid.Documents D
				ON D.DocumentsId = DID.DocumentId
			INNER JOIN ULS.[print].ScriptData SD
				ON SD.ScriptDataId = @ScriptDataIdBorrower
				AND SD.LetterId = @LetterId
				AND SD.Active = 1 --active
			INNER JOIN UDW..PD30_PRS_ADR PD30
				ON PD30.DC_DOM_ST = DRSM.StateCode
				AND PD30.DC_ADR = 'L'
			INNER JOIN UDW..PD10_PRS_NME PD10
				ON PD10.DF_PRS_ID = PD30.DF_PRS_ID
			INNER JOIN ULS.docid.Arcs A
				ON A.ArcId = DID.ArcId
			INNER JOIN UDW..AY10_BR_LON_ATY AY10
				ON AY10.PF_REQ_ACT = A.Arc
				AND AY10.BF_SSN = PD10.DF_PRS_ID
				AND AY10.LD_ATY_REQ_RCV >= CAST(PD30.DF_LST_DTS_PD30 AS DATE) --TODO: uncomment for prod
				AND AY10.LD_ATY_REQ_RCV > @OneWeekLookback --only get records within past week  --TODO:  uncomment for prod
				AND AY10.LC_STA_ACTY10 = 'A' --active  --TODO:  uncomment for prod
			LEFT JOIN ULS.docid.DocumentsProcessedResponse DPR
				ON DPR.BF_SSN = AY10.BF_SSN
				AND	DPR.LN_ATY_SEQ = AY10.LN_ATY_SEQ
				AND DPR.PrintProcessingId IS NOT NULL
			LEFT JOIN UDW..PH05_CNC_EML PH05
				ON PH05.DF_SPE_ID = PD10.DF_SPE_ACC_ID
			LEFT JOIN ULS.[print].ArcScriptDataMapping ASDM
				ON ASDM.ScriptDataId = @ScriptDataIdBorrower
			LEFT JOIN UDW..PD21_GTR_DTH PD21
				ON PD10.DF_PRS_ID = PD21.DF_PRS_ID
				AND PD21.DC_DTH_STA = '02' --verified death
		WHERE
			PD21.DF_PRS_ID IS NULL --exclude verified death
			AND DPR.DocumentsProcessedResponseId IS NULL --not already processed  --TODO:  uncomment for prod
		;
		--select * from #lettersToAdd_BOR --TEST --TODO: comment out for prod

		--CO-BORROWER:
		SELECT
			'Co-borrower' AS Pop,
			BorrPD10.DF_SPE_ACC_ID AS BorrowerAccountNumber,
			PD10.DF_SPE_ACC_ID AS CoBorrowerAccountNumber,
			BorrPD10.DF_PRS_ID AS BorrowerSsn,
			LN20.LF_EDS AS CoBorrowerSsn,
			COALESCE(PH05.DX_CNC_EML_ADR,'Ecorr@Uheaa.org') AS EmailAddress,
			PH05.DX_CNC_EML_ADR,
			@ScriptDataIdCoBorrower AS ScriptDataId,
			CONCAT
				(
					CentralData.dbo.CreateACSKeyline(PD10.DF_PRS_ID,'B','L'), ',' ,
					RTRIM(PD10.DM_PRS_1), ' ',
					RTRIM(PD10.DM_PRS_LST), RTRIM(CONCAT(' ', RTRIM(PD10.DM_PRS_LST_SFX))), ',',
					RTRIM(EndPD30.DX_STR_ADR_1), ',',
					RTRIM(EndPD30.DX_STR_ADR_2), ',',
					RTRIM(EndPD30.DM_CT), ',',
					RTRIM(EndPD30.DC_DOM_ST), ',',
					RTRIM(EndPD30.DF_ZIP_CDE), ',',
					RTRIM(EndPD30.DM_FGN_ST), ',',
					RTRIM(EndPD30.DM_FGN_CNY), ',',
					RTRIM(BorrPD10.DF_SPE_ACC_ID), ',',
					RTRIM(BorrPD10.DF_PRS_ID), ',',
					'MA4119'
				) AS LetterData,
			'MA4119' AS CostCenter,
			IIF(--"InValidAddress" refers to not on ecorr OR no valid physical address
					EndPD30.DI_VLD_ADR = 'Y' --has valid legal address
					OR (--on ecorr:
							PH05.DI_VLD_CNC_EML_ADR = 'Y'
							AND PH05.DI_CNC_ELT_OPI = 'Y' 
							AND PH05.DX_CNC_EML_ADR IS NOT NULL
						)
				, 0, 1) AS InValidAddress,
			SD.DoNotProcessEcorr,
 			IIF(
					PH05.DI_VLD_CNC_EML_ADR = 'Y' 
					AND PH05.DI_CNC_ELT_OPI = 'Y' 
					AND PH05.DX_CNC_EML_ADR IS NOT NULL
				, 1, 0) AS OnEcorr,
			IIF(ASDM.ArcId IS NOT NULL, 1, 0) AS ArcNeeded,
			IIF(SD.DocIdId IS NOT NULL, 1, 0) AS ImagingNeeded,
			DID.DocumentId,
			AY10.LN_ATY_SEQ,
			LN20.LN_SEQ, --needed for ByLoan arc add
			IIF(PD10.DF_PRS_ID LIKE 'P%', 1, 0) AS PFlag
		INTO
			#lettersToAdd_COB
		FROM
			ULS.docid.DocumentResponseStateMapping DRSM
			INNER JOIN ULS.docid.DocIdMapping DID
				ON DID.DocIdMappingId = DRSM.DocIdMappingId
			INNER JOIN ULS.docid.Documents D
				ON D.DocumentsId = DID.DocumentId
			INNER JOIN ULS.[print].ScriptData SD
				ON SD.ScriptDataId = @ScriptDataIdCoBorrower
				AND SD.LetterId = @LetterId
				AND SD.Active = 1 --active
			INNER JOIN UDW..PD30_PRS_ADR EndPD30
				ON EndPD30.DC_DOM_ST = DRSM.StateCode
				AND EndPD30.DC_ADR = 'L' --legal
			INNER JOIN UDW..LN20_EDS LN20
				ON LN20.LF_EDS = EndPD30.DF_PRS_ID
				AND LN20.LC_STA_LON20 = 'A' --active
				AND LN20.LC_EDS_TYP = 'M' --coborrowers only
			INNER JOIN UDW..PD10_PRS_NME PD10
				ON PD10.DF_PRS_ID = EndPD30.DF_PRS_ID
			INNER JOIN UDW..PD10_PRS_NME BorrPD10
				ON BorrPD10.DF_PRS_ID = LN20.BF_SSN
			INNER JOIN ULS.docid.Arcs A
				ON A.ArcId = DID.ArcId
			INNER JOIN UDW..AY10_BR_LON_ATY AY10
				ON AY10.PF_REQ_ACT = A.Arc
				AND AY10.BF_SSN = BorrPD10.DF_PRS_ID
				AND AY10.LD_ATY_REQ_RCV >= CAST(EndPD30.DF_LST_DTS_PD30 AS DATE)
				AND AY10.LD_ATY_REQ_RCV > @OneWeekLookback --only get records within week
				AND AY10.LC_STA_ACTY10 = 'A' --active
			INNER JOIN UDW..LN10_LON LN10
				ON LN10.BF_SSN = LN20.BF_SSN --borrower
				AND LN10.LN_SEQ = LN20.LN_SEQ
			LEFT JOIN ULS.docid.DocumentsProcessedResponse DPR
				ON DPR.BF_SSN = LN20.LF_EDS --coborrower
				AND DPR.LN_ATY_SEQ = AY10.LN_ATY_SEQ
				AND DPR.PrintProcessingId IS NOT NULL
			LEFT JOIN UDW..PH05_CNC_EML PH05
				ON PH05.DF_SPE_ID = PD10.DF_SPE_ACC_ID
			LEFT JOIN ULS.[print].ArcScriptDataMapping ASDM
				ON ASDM.ScriptDataId = @ScriptDataIdCoBorrower
			LEFT JOIN UDW..PD21_GTR_DTH PD21
				ON PD10.DF_PRS_ID = PD21.DF_PRS_ID
				AND PD21.DC_DTH_STA = '02' --verified death
		WHERE
			PD21.DF_PRS_ID IS NULL --exclude verified death
			AND DPR.DocumentsProcessedResponseId IS NULL --not already processed
		;
		--select * from #lettersToAdd_COB --TEST

		--remove people already added to print processing
		SELECT
			POP.Pop,
			IIF(POP.Pop = 'Co-borrower', POP.CoBorrowerAccountNumber, POP.BorrowerAccountNumber) AS AccountNumber,
			POP.BorrowerAccountNumber,
			POP.CoBorrowerAccountNumber,
			POP.BorrowerSsn,
			POP.CoBorrowerSsn,
			POP.EmailAddress,
			POP.DX_CNC_EML_ADR,
			POP.ScriptDataId,
			POP.LetterData,
			POP.CostCenter,
			POP.InValidAddress,
			POP.DoNotProcessEcorr,
			POP.OnEcorr,
			POP.ArcNeeded,
			POP.ImagingNeeded,
			POP.DocumentId,
			POP.LN_ATY_SEQ,
			POP.LN_SEQ,
			POP.PFlag
		INTO
			#lettersToAdd
		FROM
			(
				SELECT * FROM #lettersToAdd_BOR
				UNION ALL
				SELECT * FROM #lettersToAdd_COB
			) POP
			LEFT JOIN ULS.[print].PrintProcessing EXISTING_DATA --active flag not needed since want to identify all matching records
				ON EXISTING_DATA.AccountNumber = IIF(POP.Pop = 'Co-borrower', POP.CoBorrowerAccountNumber, POP.BorrowerAccountNumber)
				AND (
						POP.DX_CNC_EML_ADR = EXISTING_DATA.EmailAddress
						OR POP.DX_CNC_EML_ADR IS NULL
					)
				AND EXISTING_DATA.ScriptDataId = IIF(POP.Pop = 'Co-borrower', @ScriptDataIdCoBorrower, @ScriptDataIdBorrower)
				AND EXISTING_DATA.LetterData = POP.LetterData
				AND EXISTING_DATA.CostCenter = POP.CostCenter
				AND EXISTING_DATA.InValidAddress = POP.InValidAddress
				AND EXISTING_DATA.DoNotProcessEcorr = POP.DoNotProcessEcorr
				AND EXISTING_DATA.OnEcorr = POP.OnEcorr
				AND EXISTING_DATA.ArcNeeded = POP.ArcNeeded
				AND EXISTING_DATA.ImagingNeeded = POP.ImagingNeeded
				AND (
						CAST(EXISTING_DATA.AddedAt AS DATE) = @TODAY
						OR (
								EXISTING_DATA.EcorrDocumentCreatedAt IS NULL
								AND EXISTING_DATA.PrintedAt IS NULL
								AND EXISTING_DATA.DeletedAt IS NULL
							)
					)
		WHERE
			EXISTING_DATA.AccountNumber IS NULL --not already added today
			AND POP.PFlag = 0 --removes any acct# starting with P
		;
		--select * from #lettersToAdd --TEST

		--Add account number with 30 ln seq
		DECLARE @MappedLoans TABLE
		(
			AccountNumber VARCHAR(10),
			LoanSequence SMALLINT
		); 

		;WITH Loans(LoanSequence) AS 
		(--recursive CTE generates #'s 1-30
			SELECT 1 
			UNION ALL 
			SELECT LoanSequence + 1
			FROM Loans
			WHERE LoanSequence < 30 --# of file header LN_SEQ fields
		) 
		INSERT INTO @MappedLoans 
		(
			AccountNumber, 
			LoanSequence
		)
		SELECT 
			LTA.AccountNumber,
			L.LoanSequence
		FROM
			Loans L
			CROSS JOIN
			(
				SELECT DISTINCT
					LTA.AccountNumber
				FROM
					#lettersToAdd LTA
				WHERE
					LTA.LN_SEQ IS NOT NULL
			) LTA
		;

		--take the 30 ln_seq and map up which ones actually have data
		DECLARE @LoanSequenceList TABLE
		(
			AccountNumber VARCHAR(10), 
			LoanSequences VARCHAR(8000) --avoids use of max
		); 

		INSERT INTO @LoanSequenceList
		(
			AccountNumber, 
			LoanSequences
		)
		SELECT DISTINCT
			OML.AccountNumber,
			LoanData = STUFF
			(
				( 
					SELECT
						',' + COALESCE(CAST(LN_SEQ AS VARCHAR(8000)),'') --avoids use of max
					FROM
					(--"inner" mapped loans data set
						SELECT DISTINCT
							IML.AccountNumber,
							IML.LoanSequence,
							LTA.LN_SEQ
						FROM
							@MappedLoans IML
							LEFT JOIN #lettersToAdd LTA
								ON LTA.AccountNumber = IML.AccountNumber
								AND LTA.LN_SEQ = IML.LoanSequence
						WHERE
							OML.AccountNumber = IML.AccountNumber --This line forces account number integrity when looking up which row to append LN_SEQ values 
					) x
					ORDER BY
						COALESCE(LN_SEQ,10000)
					FOR XML PATH(''), TYPE
				).value('.','VARCHAR(8000)'), --avoids use of max
				1,1,','
			  )
		FROM
			(--"outer" mapped loans data set
				SELECT DISTINCT
					ML.AccountNumber,
					LTA.LN_SEQ
				FROM 
					@MappedLoans ML
					LEFT JOIN #lettersToAdd LTA
						ON ML.AccountNumber = LTA.AccountNumber
						AND ML.LoanSequence = LTA.LN_SEQ
			) OML --outer mapped loans
		ORDER BY
			OML.AccountNumber
		;

		UPDATE
			LTA
		SET
			LTA.LetterData = LTA.LetterData + LSL.LoanSequences
		FROM
			#lettersToAdd LTA
			INNER JOIN @LoanSequenceList LSL
				ON LSL.AccountNumber = LTA.AccountNumber
		;
		--select * from #lettersToAdd

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
			LTA.AccountNumber,
			LTA.EmailAddress,
			LTA.ScriptDataId,
			NULL AS SourceFile,
			LTA.LetterData,
			LTA.CostCenter,
			LTA.InValidAddress,
			LTA.DoNotProcessEcorr,
			LTA.OnEcorr,
			LTA.ArcNeeded,
			LTA.ImagingNeeded,
			@NOW AS AddedAt,
			@ScriptId AS AddedBy
		FROM
			#lettersToAdd LTA
			LEFT JOIN ULS.[print].PrintProcessing EXISTING_DATA
				ON LTA.AccountNumber = EXISTING_DATA.AccountNumber
				AND LTA.EmailAddress = EXISTING_DATA.EmailAddress
				AND LTA.ScriptDataId = EXISTING_DATA.ScriptDataId
				AND LTA.LetterData = EXISTING_DATA.LetterData
				AND LTA.CostCenter = EXISTING_DATA.CostCenter
				AND LTA.InValidAddress = EXISTING_DATA.InValidAddress
				AND LTA.DoNotProcessEcorr = EXISTING_DATA.DoNotProcessEcorr
				AND LTA.OnEcorr = EXISTING_DATA.OnEcorr
				AND LTA.ArcNeeded = EXISTING_DATA.ArcNeeded
				AND LTA.ImagingNeeded = EXISTING_DATA.ImagingNeeded
				AND (
						CONVERT(DATE,EXISTING_DATA.AddedAt) = @TODAY
						OR (
								EXISTING_DATA.EcorrDocumentCreatedAt IS NULL
								AND EXISTING_DATA.PrintedAt IS NULL
								AND EXISTING_DATA.DeletedAt IS NULL
							)
					)
		WHERE
			EXISTING_DATA.AccountNumber IS NULL
			AND LTA.InValidAddress = 0 --has valid address
		;
		--select * from @PrintProcessingIds

		INSERT INTO ULS.docid.DocumentsProcessedResponse
		(
			BF_SSN, 
			LN_ATY_SEQ, 
			PrintProcessingId
		)
		SELECT DISTINCT
			IIF(LTA.Pop = 'Co-borrower', LTA.CoBorrowerSsn, LTA.BorrowerSsn) AS BF_SSN,
			LTA.LN_ATY_SEQ,
			PP.PrintProcessingId
		FROM
			@PrintProcessingIds PP
			INNER JOIN #lettersToAdd LTA
				ON LTA.AccountNumber = PP.AccountNumber
				AND LTA.LetterData = PP.LetterData
		;
		--select * from ULS.docid.DocumentsProcessedResponse --TEST

		/*************** ADD INVALID ADDRESSES TO ARC ADD PROCESSING AND DOCUMENTS PROCESSED REPSONSE ***************/

		DECLARE @ArcAddProcessingIds TABLE
		(
			ArcAddProcessingId INT,
			ArcTypeId BIT,
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
			INSERTED.ArcTypeId,
			INSERTED.AccountNumber,
			INSERTED.RecipientId,
			INSERTED.IsEndorser
		INTO
			@ArcAddProcessingIds
		(
			ArcAddProcessingId,
			ArcTypeId,
			AccountNumber,
			RecipientId,
			IsEndorser
		)
		SELECT DISTINCT
			IIF(LTA.Pop = 'Co-borrower', 0, 1) AS ArcTypeId,
			LTA.BorrowerAccountNumber AS AccountNumber,
			IIF(LTA.Pop = 'Co-borrower', LTA.CoBorrowerSsn, LTA.BorrowerSsn) AS RecipientId,
			@ARC_InvalidAddress AS ARC,
			@ScriptId AS ScriptId,
			@NOW AS ProcessOn,
			@Comment AS Comment,
			0 AS IsReference,
			IIF(LTA.Pop = 'Co-borrower', 1, 0) AS IsEndorser,
			0 AS ProcessingAttempts,
			@NOW AS CreatedAt,
			@ScriptId AS CreatedBy
		FROM
			#lettersToAdd LTA
			LEFT JOIN ULS..ArcAddProcessing EXISTING_AAP
				ON EXISTING_AAP.AccountNumber = LTA.BorrowerAccountNumber
				AND EXISTING_AAP.ArcTypeId = IIF(LTA.Pop = 'Co-borrower', 0, 1)
				AND EXISTING_AAP.RecipientId = IIF(LTA.Pop = 'Co-borrower', LTA.CoBorrowerSsn, LTA.BorrowerSsn)
				AND EXISTING_AAP.IsEndorser = IIF(LTA.Pop = 'Co-borrower', 1, 0)
				AND EXISTING_AAP.ARC = @ARC_InvalidAddress
				AND EXISTING_AAP.ScriptId = @ScriptId
				AND COALESCE(EXISTING_AAP.Comment,'') = @Comment
				AND CAST(EXISTING_AAP.CreatedAt AS DATE) = @TODAY --prevents multiple arcs from being added on the same day
			LEFT JOIN ULS.docid.DocumentsProcessedResponse EXISTING_DPR
				ON EXISTING_DPR.BF_SSN = IIF(LTA.Pop = 'Co-borrower', LTA.CoBorrowerSsn, LTA.BorrowerSsn)
				AND EXISTING_DPR.LN_ATY_SEQ = LTA.LN_ATY_SEQ
				AND EXISTING_DPR.InvalidAddressArcAddProcessingId IS NOT NULL
		WHERE
			EXISTING_AAP.AccountNumber IS NULL --No matching existing record
			AND LTA.InValidAddress = 1 --has invalid address
			AND EXISTING_DPR.BF_SSN IS NULL --not already processed
		;
		--select * from @ArcAddProcessingIds; --TEST

		INSERT INTO ULS.docid.DocumentsProcessedResponse
		(
			BF_SSN,
			LN_ATY_SEQ,
			InvalidAddressArcAddProcessingId
		)
		SELECT DISTINCT
			IIF(LTA.Pop = 'Co-borrower', LTA.CoBorrowerSsn, LTA.BorrowerSsn) AS BF_SSN,
			LTA.LN_ATY_SEQ,
			AAPI.ArcAddProcessingId AS InvalidAddressArcAddProcessingId
		FROM
			@ArcAddProcessingIds AAPI
			INNER JOIN #lettersToAdd LTA
				ON AAPI.AccountNumber = LTA.BorrowerAccountNumber
				AND AAPI.ArcTypeId = IIF(LTA.Pop = 'Co-borrower', 0, 1)
				AND AAPI.RecipientId = IIF(LTA.Pop = 'Co-borrower', LTA.CoBorrowerSsn, LTA.BorrowerSsn)
				AND AAPI.IsEndorser = IIF(LTA.Pop = 'Co-borrower', 1, 0)
		WHERE
			LTA.InValidAddress = 1 --has invalid address
		;
		--select * from ULS.docid.DocumentsProcessedResponse where InvalidAddressArcAddProcessingId is not null --TEST
		
		--add record into AAP loan selection
		INSERT INTO ULS..ArcLoanSequenceSelection
		(
			ArcAddProcessingId,
			LoanSequence
		)
		SELECT DISTINCT
			AAPI.ArcAddProcessingId,
			LTA.LN_SEQ AS LoanSequence
		FROM
			@ArcAddProcessingIds AAPI
			INNER JOIN #lettersToAdd LTA
				ON AAPI.AccountNumber = LTA.BorrowerAccountNumber
				AND AAPI.ArcTypeId = IIF(LTA.Pop = 'Co-borrower', 0, 1)
				AND AAPI.RecipientId = IIF(LTA.Pop = 'Co-borrower', LTA.CoBorrowerSsn, LTA.BorrowerSsn)
				AND AAPI.IsEndorser = IIF(LTA.Pop = 'Co-borrower', 1, 0)
		WHERE
			LTA.InValidAddress = 1 --has invalid address
			AND AAPI.ArcTypeId = 0 --adds coborrowers only
		;

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