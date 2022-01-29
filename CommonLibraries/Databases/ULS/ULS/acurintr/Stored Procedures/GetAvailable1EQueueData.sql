CREATE PROCEDURE [acurintr].[GetAvailable1EQueueData]
	@TestMode bit
AS
DECLARE @Query VARCHAR(Max)
IF @TestMode = 0
	SET @Query = 'MERGE [acurintr].[CompassQueue] AS T USING (SELECT D.* FROM OPENQUERY(DUSTER,'
ELSE 
	SET @Query = 'MERGE [acurintr].[CompassQueue] AS T USING (SELECT D.* FROM OPENQUERY(QADBD004,'

SELECT @Query = @Query + 
		'''
			SELECT
				WQ20.WF_QUE Queue,
				WQ20.WF_SUB_QUE SubQueue,
				RTRIM(WQ20.WN_CTL_TSK) TaskControlNumber,
				RTRIM(PD10.DF_PRS_ID) Ssn,
				RTRIM(PD10.DF_SPE_ACC_ID) AccountNumber,
				RTRIM(PD55.DX_STR_ADR_1_PND) Address1,
				RTRIM(PD55.DX_STR_ADR_2_PND) Address2,
				RTRIM(PD55.DM_CT_PND) City,
				RTRIM(PD55.DC_DOM_ST_PND) State,
				RTRIM(PD55.DF_ZIP_CDE_PND) ZipCode,
				RTRIM(PD55.DM_FGN_CNY_PND) Country,
				RTRIM(PD55.DM_FGN_ST_PND) ForeignState,
				RTRIM(PD55.DC_PDM_SRC_PND) AddressSource,
				RTRIM(PD55.DD_VER_ADR_PND) AddressValidityDate,
				CASE PD55.DN_DOM_PHN_ARA_PND || PD55.DN_DOM_PHN_XCH_PND || PD55.DN_DOM_PHN_LCL_PND
					WHEN '''''''' THEN
						RTRIM(PD30.DX_STR_ADR_1) || '''' '''' || RTRIM(PD30.DX_STR_ADR_2) || '''', '''' || RTRIM(PD30.DM_CT) || '''', '''' || RTRIM(PD30.DC_DOM_ST) || '''' '''' || RTRIM(PD30.DF_ZIP_CDE) || '''' '''' || RTRIM(PD30.DM_FGN_CNY)
				END OriginalAddressText,
				PD55.DN_DOM_PHN_ARA_PND || PD55.DN_DOM_PHN_XCH_PND || PD55.DN_DOM_PHN_LCL_PND PrimaryPhone,
				RTRIM(PD55.DC_PHN_SRC_PND) PhoneSource,
				RTRIM(PD55.DD_PHN_LST_VER_PND) PhoneValidityDate,
				CASE PD55.DX_STR_ADR_1_PND || PD55.DX_STR_ADR_2_PND
					WHEN '''''''' THEN
						RTRIM(PD42.DN_DOM_PHN_ARA) || RTRIM(PD42.DN_DOM_PHN_XCH) || RTRIM(PD42.DN_DOM_PHN_LCL)
				END OriginalPhoneText
			FROM
				OLWHRM1.WQ20_TSK_QUE WQ20
				LEFT JOIN OLWHRM1.PD55_PRS_PND_DMO PD55 ON PD55.DF_PRS_ID = WQ20.BF_SSN AND WQ20.WF_QUE = PD55.WF_QUE AND WQ20.WF_SUB_QUE = PD55.WF_SUB_QUE
				LEFT JOIN OLWHRM1.PD10_PRS_NME PD10 ON WQ20.BF_SSN = PD10.DF_PRS_ID
				LEFT JOIN OLWHRM1.PD30_PRS_ADR PD30 ON PD10.DF_PRS_ID = PD30.DF_PRS_ID AND PD30.DC_ADR = ''''L''''
				LEFT JOIN OLWHRM1.PD42_PRS_PHN PD42 ON WQ20.BF_SSN = PD42.DF_PRS_ID AND PD42.DC_PHN = ''''H''''
			WHERE
				WQ20.WC_STA_WQUE20 = ''''U''''
				AND WQ20.WF_QUE = ''''1E''''
				AND WQ20.WF_SUB_QUE = ''''01''''
		'') D) AS S
	ON
		S.Queue = T.Queue AND S.SubQueue = T.SubQueue
		AND T.ProcessedAt IS NULL
		AND S.TaskControlNumber = T.TaskControlNumber
	WHEN NOT MATCHED BY TARGET THEN
		INSERT (Queue, SubQueue, TaskControlNumber, Ssn, AccountNumber, Address1, Address2, City, State, ZipCode, Country, ForeignState, AddressSource, AddressValidityDate, OriginalAddressText, PrimaryPhone, PhoneSource, PhoneValidityDate, OriginalPhoneText)
		VALUES(S.Queue, S.SubQueue, S.TaskControlNumber, S.Ssn, S.AccountNumber, S.Address1, S.Address2, S.City, S.State, S.ZipCode, S.Country, S.ForeignState, S.AddressSource, S.AddressValidityDate, S.OriginalAddressText, S.PrimaryPhone, S.PhoneSource, S.PhoneValidityDate, S.OriginalPhoneText)
	--WHEN NOT MATCHED BY SOURCE AND (T.ArcAddedAt IS NULL AND T.PrintedAt IS NULL AND T.DeletedAt IS NULL AND T.AddedAt < DATEADD(DAY, -3, GETDATE())) THEN
	--	UPDATE
	--	SET
	--		DeletedAt = GETDATE(),
	--		DeletedBy = SYSTEM_USER()
		;'
EXEC (@Query)