CREATE PROCEDURE [acurintc].[LoadCompassCommaData]
	@Queue VARCHAR(20),
	@SubQueue VARCHAR(20)
AS


DECLARE @AccurintDemo INT = 1
DECLARE @EmailDemo INT = 5
DECLARE @AutopayDemo INT = 2

DECLARE @AccurintSys INT = 1
DECLARE @AutopaySys INT = 2
DECLARE @CompassEmailSys INT = 4
DECLARE @Today DATE = CAST(GETDATE() AS DATE)

MERGE [acurintc].[ProcessQueue] AS T USING
(
	SELECT
		D.WF_QUE [Queue],
		D.WF_SUB_Que [SubQueue],
		D.WN_CTL_TSK [TaskControlNumber],
		D.PF_REQ_ACT [RequestIdentifier],
		CAST(CAST(CONCAT(D.WD_INI_TSK, ' ', D.WT_INI_TSK) AS DATETIME2) AS DATETIME) [TaskTimeStamp],
		D.BF_SSN [Ssn],
		D.DF_SPE_ACC_ID [AccountNumber],
		dbo.SplitAndRemoveQuotes(D.QueueMessage, ',', 0, 1) [Address1],
		dbo.SplitAndRemoveQuotes(D.QueueMessage, ',', 1, 1) [Address2],
		dbo.SplitAndRemoveQuotes(D.QueueMessage, ',', 2, 1) [City],
		dbo.SplitAndRemoveQuotes(D.QueueMessage, ',', 3, 1) [State],
		dbo.SplitAndRemoveQuotes(D.QueueMessage, ',', 4, 1) [ZipCode],
		dbo.SplitAndRemoveQuotes(D.QueueMessage, ',', 5, 1) [Country],
		RTRIM(dbo.SplitAndRemoveQuotes(D.QueueMessage, ',', 6, 1)) [PrimaryPhone],
		RTRIM(dbo.SplitAndRemoveQuotes(D.QueueMessage, ',', 7, 1)) [AlternatePhone],
		dbo.SplitAndRemoveQuotes(D.QueueMessage, ',', 8, 1) [EmailAddress],
		D.OriginalAddressText,
		D.OriginalAddressIsValid,
		RTRIM(D.OriginalHomePhone) OriginalHomePhone,
		D.HomePhoneVerificationDate,
		D.OriginalHomePhoneIsValid,
		RTRIM(D.OriginalAltPhone) OriginalAltPhone,
		D.AltPhoneVerificationDate,
		D.OriginalAltPhoneIsValid,
		RTRIM(D.OriginalWorkPhone) OriginalWorkPhone,
		D.WorkPhoneVerificationDate,
		D.OriginalWorkPhoneIsValid,
		RTRIM(D.OriginalMobilePhone) OriginalMobilePhone,
		D.MobilePhoneVerificationDate,
		D.OriginalMobilePhoneIsValid,
		D.WF_CRT_DTS_WQ20 PendingVerificationDate,
		CASE
			WHEN RTRIM(dbo.SplitAndRemoveQuotes(D.QueueMessage, ',', 0, 1)) = ''
			THEN NULL --no date for phones
			ELSE D.DD_VER_ADR
		END CurrentVerificationDate,
		CASE 
			WHEN dbo.SplitAndRemoveQuotes(D.QueueMessage, ',', 9, 1) LIKE '%EMAIL%' OR dbo.SplitAndRemoveQuotes(D.QueueMessage, ',', 8, 1) LIKE '%EMAIL%' THEN @EmailDemo
			WHEN dbo.SplitAndRemoveQuotes(D.QueueMessage, ',', 9, 1) LIKE '%ACCURINT%' OR dbo.SplitAndRemoveQuotes(D.QueueMessage, ',', 8, 1) LIKE '%ACCURINT%' THEN @AccurintDemo
			ELSE @AutopayDemo
		END [DemographicsSourceId],
		CASE 
			WHEN dbo.SplitAndRemoveQuotes(D.QueueMessage, ',', 9, 1) LIKE '%EMAIL%' OR dbo.SplitAndRemoveQuotes(D.QueueMessage, ',', 8, 1) LIKE '%EMAIL%' THEN @CompassEmailSys
			WHEN dbo.SplitAndRemoveQuotes(D.QueueMessage, ',', 9, 1) LIKE '%ACCURINT%' OR dbo.SplitAndRemoveQuotes(D.QueueMessage, ',', 8, 1) LIKE '%ACCURINT%'  THEN @AccurintSys
			ELSE @AutopaySys
		END [SystemSourceId]
	FROM
		(
			SELECT
				WQ20.*,
				PD10.DF_PRS_ID,
				PD10.DF_SPE_ACC_ID,
				RTRIM(ISNULL(WQ20.WX_MSG_1_TSK, '')) + RTRIM(ISNULL(WQ20.WX_MSG_2_TSK, '')) + ',,,,,,,,,,' QueueMessage,
				RTRIM(RTRIM(ISNULL(PD30.DX_STR_ADR_1, '')) + ' ' + RTRIM(ISNULL(PD30.DX_STR_ADR_2, '')) + ' ' + RTRIM(ISNULL(PD30.DX_STR_ADR_3, ''))) + ',' + RTRIM(ISNULL(PD30.DM_CT, '')) 
					+ ',' + RTRIM(ISNULL(PD30.DC_DOM_ST, '')) + ' ' + RTRIM(ISNULL(PD30.DF_ZIP_CDE, '')) + ' ' + RTRIM(ISNULL(PD30.DM_FGN_CNY, '')) OriginalAddressText,
				PD30.DD_VER_ADR, 
				PD42H.DD_PHN_VER,
				CASE WHEN PD30.DI_VLD_ADR = 'Y' THEN 1 ELSE 0 END OriginalAddressIsValid,
				COALESCE(
					NULLIF(PD42H.DN_DOM_PHN_ARA + PD42H.DN_DOM_PHN_XCH + PD42H.DN_DOM_PHN_LCL + PD42H.DN_PHN_XTN, ''),
					NULLIF(PD42H.DN_FGN_PHN_INL + PD42H.DN_FGN_PHN_CNY + PD42H.DN_FGN_PHN_CT + PD42H.DN_FGN_PHN_LCL, '')
				) OriginalHomePhone,
				PD42H.DD_PHN_VER HomePhoneVerificationDate,
				CASE WHEN PD42H.DI_PHN_VLD = 'Y' THEN 1 ELSE 0 END OriginalHomePhoneIsValid,
				COALESCE(
					NULLIF(PD42A.DN_DOM_PHN_ARA + PD42A.DN_DOM_PHN_XCH + PD42A.DN_DOM_PHN_LCL + PD42A.DN_PHN_XTN, ''),
					NULLIF(PD42A.DN_FGN_PHN_INL + PD42A.DN_FGN_PHN_CNY + PD42A.DN_FGN_PHN_CT + PD42A.DN_FGN_PHN_LCL, '')
				) OriginalAltPhone,
				PD42A.DD_PHN_VER AltPhoneVerificationDate,
				CASE WHEN PD42A.DI_PHN_VLD = 'Y' THEN 1 ELSE 0 END OriginalAltPhoneIsValid,
				COALESCE(
					NULLIF(PD42W.DN_DOM_PHN_ARA + PD42W.DN_DOM_PHN_XCH + PD42W.DN_DOM_PHN_LCL + PD42W.DN_PHN_XTN, ''),
					NULLIF(PD42W.DN_FGN_PHN_INL + PD42W.DN_FGN_PHN_CNY + PD42W.DN_FGN_PHN_CT + PD42W.DN_FGN_PHN_LCL, '')
				) OriginalWorkPhone,
				PD42W.DD_PHN_VER WorkPhoneVerificationDate,
				CASE WHEN PD42W.DI_PHN_VLD = 'Y' THEN 1 ELSE 0 END OriginalWorkPhoneIsValid,
				COALESCE(
					NULLIF(PD42M.DN_DOM_PHN_ARA + PD42M.DN_DOM_PHN_XCH + PD42M.DN_DOM_PHN_LCL + PD42M.DN_PHN_XTN, ''),
					NULLIF(PD42M.DN_FGN_PHN_INL + PD42M.DN_FGN_PHN_CNY + PD42M.DN_FGN_PHN_CT + PD42M.DN_FGN_PHN_LCL, '')
				) OriginalMobilePhone,
				PD42M.DD_PHN_VER MobilePhoneVerificationDate,
				CASE WHEN PD42M.DI_PHN_VLD = 'Y' THEN 1 ELSE 0 END OriginalMobilePhoneIsValid
			FROM
				UDW.dbo.WQ20_TSK_QUE WQ20
				LEFT JOIN UDW.dbo.PD10_PRS_NME PD10 ON WQ20.BF_SSN = PD10.DF_PRS_ID
				LEFT JOIN UDW.dbo.PD30_PRS_ADR PD30 ON PD10.DF_PRS_ID = PD30.DF_PRS_ID AND PD30.DC_ADR = 'L'
				LEFT JOIN UDW.dbo.PD42_PRS_PHN PD42H ON WQ20.BF_SSN = PD42H.DF_PRS_ID AND PD42H.DC_PHN = 'H'
				LEFT JOIN UDW.dbo.PD42_PRS_PHN PD42A ON WQ20.BF_SSN = PD42A.DF_PRS_ID AND PD42A.DC_PHN = 'A'
				LEFT JOIN UDW.dbo.PD42_PRS_PHN PD42W ON WQ20.BF_SSN = PD42W.DF_PRS_ID AND PD42W.DC_PHN = 'W'
				LEFT JOIN UDW.dbo.PD42_PRS_PHN PD42M ON WQ20.BF_SSN = PD42M.DF_PRS_ID AND PD42M.DC_PHN = 'M'
			WHERE
				WQ20.WF_QUE = @Queue
				AND 
				WQ20.WF_SUB_QUE = @SubQueue
				AND 
				WQ20.WC_STA_WQUE20 = 'U'
		) D
) as S ON
	S.[Queue] = T.[Queue]
	AND S.SubQueue = T.SubQueue
	AND S.TaskControlNumber = T.TaskControlNumber
	AND S.RequestIdentifier = T.RequestIdentifier
	AND S.TaskTimeStamp = T.TaskTimeStamp
	AND (T.ProcessedAt IS NULL OR CAST(T.ProcessedAt AS DATE) = @Today) --Adding check for processed at on the current date to prevent double working tasks on the day they are processed.
WHEN NOT MATCHED BY TARGET THEN
	INSERT (  [Queue],   SubQueue,   TaskControlNumber,   TaskTimeStamp,   RequestIdentifier,   Ssn,   AccountNumber,   Address1,   Address2,   City,   [State],   ZipCode,   Country,   OriginalAddressText,   OriginalAddressIsValid,   PrimaryPhone,   DemographicsSourceId,   SystemSourceId,   PendingVerificationDate,   CurrentVerificationDate,   OriginalHomePhone,   HomePhoneVerificationDate,   OriginalHomePhoneIsValid,   OriginalAltPhone,   AltPhoneVerificationDate,   OriginalAltPhoneIsValid,   OriginalWorkPhone,   WorkPhoneVerificationDate,   OriginalWorkPhoneIsValid,   OriginalMobilePhone,   MobilePhoneVerificationDate,   OriginalMobilePhoneIsValid)
	VALUES (S.[Queue], S.SubQueue, S.TaskControlNumber, S.TaskTimeStamp, S.RequestIdentifier, S.Ssn, S.AccountNumber, S.Address1, S.Address2, S.City, S.[State], S.ZipCode, S.Country, S.OriginalAddressText, S.OriginalAddressIsValid, S.PrimaryPhone, S.DemographicsSourceId, S.SystemSourceId, S.PendingVerificationDate, S.CurrentVerificationDate, S.OriginalHomePhone, S.HomePhoneVerificationDate, S.OriginalHomePhoneIsValid, S.OriginalAltPhone, S.AltPhoneVerificationDate, S.OriginalAltPhoneIsValid, S.OriginalWorkPhone, S.WorkPhoneVerificationDate, S.OriginalWorkPhoneIsValid, S.OriginalMobilePhone, S.MobilePhoneVerificationDate, S.OriginalMobilePhoneIsValid)
;

	

RETURN 0