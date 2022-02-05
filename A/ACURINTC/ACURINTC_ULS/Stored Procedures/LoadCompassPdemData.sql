CREATE PROCEDURE [acurintc].[LoadCompassPdemData]
	@Queue VARCHAR(20),
	@SubQueue VARCHAR(20)
AS


DECLARE @CompassPendingPdemDemo INT = 3
DECLARE @CompassSys INT = 3
DECLARE @Today DATE = CAST(GETDATE() AS DATE)

MERGE [acurintc].[ProcessQueue] AS T USING (
	SELECT
		WQ20.WF_QUE [Queue],
		WQ20.WF_SUB_QUE SubQueue,
		RTRIM(WQ20.WN_CTL_TSK) TaskControlNumber,
		CAST(CAST(CONCAT(WQ20.WD_INI_TSK, ' ', WT_INI_TSK) AS DATETIME2) AS DATETIME) [TaskTimeStamp],
		WQ20.PF_REQ_ACT RequestIdentifier,
		RTRIM(WQ20.BF_SSN) Ssn,
		RTRIM(PD10.DF_SPE_ACC_ID) AccountNumber,
		RTRIM(PD55.DX_STR_ADR_1_PND) Address1,
		RTRIM(PD55.DX_STR_ADR_2_PND) Address2,
		RTRIM(PD55.DX_STR_ADR_3_PND) Address3,
		RTRIM(PD55.DM_CT_PND) City,
		RTRIM(PD55.DC_DOM_ST_PND) State,
		RTRIM(PD55.DF_ZIP_CDE_PND) ZipCode,
		RTRIM(PD55.DM_FGN_CNY_PND) Country,
		RTRIM(PD55.DM_FGN_ST_PND) ForeignState,
		RTRIM(RTRIM(ISNULL(PD30.DX_STR_ADR_1, '')) + ' ' + RTRIM(ISNULL(PD30.DX_STR_ADR_2, '')) + ' ' + RTRIM(ISNULL(PD30.DX_STR_ADR_3, ''))) + ', ' 
			+ RTRIM(ISNULL(PD30.DM_CT, '')) + ', ' + RTRIM(ISNULL(PD30.DC_DOM_ST, '')) + ' ' + RTRIM(ISNULL(PD30.DF_ZIP_CDE, '')) + ' ' + RTRIM(ISNULL(PD30.DM_FGN_CNY, '')) OriginalAddressText,
		CASE WHEN PD30.DI_VLD_ADR = 'Y' THEN 1 ELSE 0 END OriginalAddressIsValid,
		RTRIM(COALESCE(
			NULLIF(PD55.DN_DOM_PHN_ARA_PND + PD55.DN_DOM_PHN_XCH_PND + PD55.DN_DOM_PHN_LCL_PND + PD55.DN_DOM_PHN_XTN_PND, ''), --local number
			NULLIF(PD55.DN_FGN_PHN_INL_PND + PD55.DN_FGN_PHN_CNY_PND + PD55.DN_FGN_PHN_CT_PND + PD55.DN_FGN_PHN_LCL_PND + DN_FGN_PHN_XTN_PND, '')  --foreign number
		)) PrimaryPhone,
		WQ20.WF_CRT_DTS_WQ20 PendingVerificationDate,
		CASE
			WHEN RTRIM(PD55.DN_DOM_PHN_ARA_PND) = '' OR RTRIM(PD55.DN_FGN_PHN_INL_PND) = ''
			THEN PD30.DD_VER_ADR 
			ELSE NULL --no verification date for phones
		END CurrentVerificationDate,
		RTRIM(COALESCE(
			NULLIF(PD42H.DN_DOM_PHN_ARA + PD42H.DN_DOM_PHN_XCH + PD42H.DN_DOM_PHN_LCL + PD42H.DN_PHN_XTN, ''),
			NULLIF(PD42H.DN_FGN_PHN_INL + PD42H.DN_FGN_PHN_CNY + PD42H.DN_FGN_PHN_CT + PD42H.DN_FGN_PHN_LCL, '')
		)) OriginalHomePhone,
		PD42H.DD_PHN_VER HomePhoneVerificationDate,
		CASE WHEN PD42H.DI_PHN_VLD = 'Y' THEN 1 ELSE 0 END OriginalHomePhoneIsValid,
		RTRIM(COALESCE(
			NULLIF(PD42A.DN_DOM_PHN_ARA + PD42A.DN_DOM_PHN_XCH + PD42A.DN_DOM_PHN_LCL + PD42A.DN_PHN_XTN, ''),
			NULLIF(PD42A.DN_FGN_PHN_INL + PD42A.DN_FGN_PHN_CNY + PD42A.DN_FGN_PHN_CT + PD42A.DN_FGN_PHN_LCL, '')
		)) OriginalAltPhone,
		PD42A.DD_PHN_VER AltPhoneVerificationDate,
		CASE WHEN PD42A.DI_PHN_VLD = 'Y' THEN 1 ELSE 0 END OriginalAltPhoneIsValid,
		RTRIM(COALESCE(
			NULLIF(PD42W.DN_DOM_PHN_ARA + PD42W.DN_DOM_PHN_XCH + PD42W.DN_DOM_PHN_LCL + PD42W.DN_PHN_XTN, ''),
			NULLIF(PD42W.DN_FGN_PHN_INL + PD42W.DN_FGN_PHN_CNY + PD42W.DN_FGN_PHN_CT + PD42W.DN_FGN_PHN_LCL, '')
		)) OriginalWorkPhone,
		PD42W.DD_PHN_VER WorkPhoneVerificationDate,
		CASE WHEN PD42W.DI_PHN_VLD = 'Y' THEN 1 ELSE 0 END OriginalWorkPhoneIsValid,
		RTRIM(COALESCE(
			NULLIF(PD42M.DN_DOM_PHN_ARA + PD42M.DN_DOM_PHN_XCH + PD42M.DN_DOM_PHN_LCL + PD42M.DN_PHN_XTN, ''),
			NULLIF(PD42M.DN_FGN_PHN_INL + PD42M.DN_FGN_PHN_CNY + PD42M.DN_FGN_PHN_CT + PD42M.DN_FGN_PHN_LCL, '')
		)) OriginalMobilePhone,
		PD42M.DD_PHN_VER MobilePhoneVerificationDate,
		CASE WHEN PD42M.DI_PHN_VLD = 'Y' THEN 1 ELSE 0 END OriginalMobilePhoneIsValid
	FROM
		UDW.dbo.WQ20_TSK_QUE WQ20
		LEFT JOIN UDW.dbo.PD55_PRS_PND_DMO PD55 ON PD55.DF_PRS_ID = WQ20.BF_SSN AND WQ20.WF_QUE = PD55.WF_QUE AND WQ20.WF_SUB_QUE = PD55.WF_SUB_QUE AND DATEADD(NS, -DATEPART(NS, PD55.DF_LST_DTS_PD55), PD55.DF_LST_DTS_PD55) = CAST(CAST(CONCAT(WQ20.WD_INI_TSK, ' ', WT_INI_TSK) AS DATETIME2) AS DATETIME)
		LEFT JOIN UDW.dbo.PD10_PRS_NME PD10 ON WQ20.BF_SSN = PD10.DF_PRS_ID
		LEFT JOIN UDW.dbo.PD30_PRS_ADR PD30 ON PD10.DF_PRS_ID = PD30.DF_PRS_ID AND PD30.DC_ADR = 'L'
		LEFT JOIN UDW.dbo.PD42_PRS_PHN PD42H ON WQ20.BF_SSN = PD42H.DF_PRS_ID AND PD42H.DC_PHN = 'H'
		LEFT JOIN UDW.dbo.PD42_PRS_PHN PD42A ON WQ20.BF_SSN = PD42A.DF_PRS_ID AND PD42A.DC_PHN = 'A'
		LEFT JOIN UDW.dbo.PD42_PRS_PHN PD42W ON WQ20.BF_SSN = PD42W.DF_PRS_ID AND PD42W.DC_PHN = 'W'
		LEFT JOIN UDW.dbo.PD42_PRS_PHN PD42M ON WQ20.BF_SSN = PD42M.DF_PRS_ID AND PD42M.DC_PHN = 'M'
	WHERE
		WQ20.WC_STA_WQUE20 = 'U'
		AND 
		WQ20.WF_QUE = @Queue
		AND 
		WQ20.WF_SUB_QUE = @SubQueue
) AS S
ON
	S.Queue = T.Queue 
	AND S.SubQueue = T.SubQueue
	AND S.TaskControlNumber = T.TaskControlNumber
	AND S.RequestIdentifier = T.RequestIdentifier
	AND S.TaskTimeStamp = T.TaskTimeStamp
	AND (T.ProcessedAt IS NULL OR CAST(T.ProcessedAt AS DATE) = @Today) --Adding check for processed at on the current date to prevent double working tasks on the day they are processed.
WHEN NOT MATCHED BY TARGET THEN
	INSERT (  [Queue],   SubQueue,   TaskControlNumber,   TaskTimeStamp,   RequestIdentifier,   Ssn,   AccountNumber,   Address1,   Address2,   Address3,   City,   [State],   ZipCode,   Country,   OriginalAddressText,   OriginalAddressIsValid,   PrimaryPhone,   DemographicsSourceId,   SystemSourceId,   PendingVerificationDate,   CurrentVerificationDate,   OriginalHomePhone,   HomePhoneVerificationDate,   OriginalHomePhoneIsValid,   OriginalAltPhone,   AltPhoneVerificationDate,   OriginalAltPhoneIsValid,   OriginalWorkPhone,   WorkPhoneVerificationDate,   OriginalWorkPhoneIsValid,   OriginalMobilePhone,   MobilePhoneVerificationDate,   OriginalMobilePhoneIsValid)
	VALUES (S.[Queue], S.SubQueue, S.TaskControlNumber, S.TaskTimeStamp, S.RequestIdentifier, S.Ssn, S.AccountNumber, S.Address1, S.Address2, S.Address3, S.City, S.[State], S.ZipCode, S.Country, S.OriginalAddressText, S.OriginalAddressIsValid, S.PrimaryPhone, @CompassPendingPdemDemo,     @CompassSys, S.PendingVerificationDate, S.CurrentVerificationDate, S.OriginalHomePhone, S.HomePhoneVerificationDate, S.OriginalHomePhoneIsValid, S.OriginalAltPhone, S.AltPhoneVerificationDate, S.OriginalAltPhoneIsValid, S.OriginalWorkPhone, S.WorkPhoneVerificationDate, S.OriginalWorkPhoneIsValid, S.OriginalMobilePhone, S.MobilePhoneVerificationDate, S.OriginalMobilePhoneIsValid)
;