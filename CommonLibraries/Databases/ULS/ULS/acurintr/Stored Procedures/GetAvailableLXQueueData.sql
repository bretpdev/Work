CREATE PROCEDURE [acurintr].[GetAvailableLXQueueData]
AS
MERGE [acurintr].[CompassQueue] AS T USING
(
	SELECT
		D.WF_QUE [Queue]
		,D.WF_SUB_Que [SubQueue]
		,D.WN_CTL_TSK [TaskControlNumber]
		,D.DF_PRS_ID [Ssn]
		,D.DF_SPE_ACC_ID [AccountNumber]
		,dbo.SplitAndRemoveQuotes(ISNULL(D.QueueMessage, ''), ',', 0, 1) [Address1]
		,dbo.SplitAndRemoveQuotes(ISNULL(D.QueueMessage, ''), ',', 1, 1) [Address2]
		,dbo.SplitAndRemoveQuotes(ISNULL(D.QueueMessage, ''), ',', 2, 1) [City]
		,dbo.SplitAndRemoveQuotes(ISNULL(D.QueueMessage, ''), ',', 3, 1) [State]
		,dbo.SplitAndRemoveQuotes(ISNULL(D.QueueMessage, ''), ',', 4, 1) [ZipCode]
		,dbo.SplitAndRemoveQuotes(ISNULL(D.QueueMessage, ''), ',', 5, 1) [Country]
		,dbo.SplitAndRemoveQuotes(ISNULL(D.QueueMessage, ''), ',', 6, 1) [PrimaryPhone]
		,dbo.SplitAndRemoveQuotes(ISNULL(D.QueueMessage, ''), ',', 7, 1) [AlternatePhone]
		,dbo.SplitAndRemoveQuotes(ISNULL(D.QueueMessage, ''), ',', 8, 1) [EmailAddress]
		,CASE
			WHEN dbo.SplitAndRemoveQuotes(ISNULL(D.QueueMessage, ''), ',', 6, 1) = '' THEN
			D.OriginalAddressText
		 END [OriginalAddressText]
		,CASE
			WHEN (dbo.SplitAndRemoveQuotes(ISNULL(D.QueueMessage, ''), ',', 0, 1) + dbo.SplitAndRemoveQuotes(ISNULL(D.QueueMessage, ''), ',', 1, 1) + dbo.SplitAndRemoveQuotes(ISNULL(D.QueueMessage, ''), ',', 2, 1) + dbo.SplitAndRemoveQuotes(ISNULL(D.QueueMessage, ''), ',', 3, 1) + dbo.SplitAndRemoveQuotes(ISNULL(D.QueueMessage, ''), ',', 4, 1)) = '' THEN
				D.OriginalPhoneText
		END [OriginalPhoneText]
	FROM
		OPENQUERY(DUSTER,
			'
				SELECT
					WQ20.*,
					PD10.DF_PRS_ID,
					PD10.DF_SPE_ACC_ID,
					WQ20.WX_MSG_1_TSK || WQ20.WX_MSG_2_TSK QueueMessage,
					RTRIM(PD30.DX_STR_ADR_1) || '' '' || RTRIM(PD30.DX_STR_ADR_2) || '', '' || RTRIM(PD30.DM_CT) || '', '' || RTRIM(PD30.DC_DOM_ST) || '' '' || RTRIM(PD30.DF_ZIP_CDE) || '' '' || RTRIM(PD30.DM_FGN_CNY) OriginalAddressText,
					RTRIM(PD42.DN_DOM_PHN_ARA) || RTRIM(PD42.DN_DOM_PHN_XCH) || RTRIM(PD42.DN_DOM_PHN_LCL) OriginalPhonetext
				FROM
					OLWHRM1.WQ20_TSK_QUE WQ20
					LEFT JOIN OLWHRM1.PD10_PRS_NME PD10 ON WQ20.BF_SSN = PD10.DF_PRS_ID
					LEFT JOIN OLWHRM1.PD30_PRS_ADR PD30 ON PD10.DF_PRS_ID = PD30.DF_PRS_ID AND PD30.DC_ADR = ''L''
					LEFT JOIN OLWHRM1.PD42_PRS_PHN PD42 ON WQ20.BF_SSN = PD42.DF_PRS_ID AND PD42.DC_PHN = ''H''
				WHERE
					WQ20.WF_QUE IN (''LX'',''DO'')
					AND WQ20.WF_SUB_QUE = ''01''
					AND WQ20.WC_STA_WQUE20 = ''U''
			'
		) D
) as S ON
	S.[Queue] = T.[Queue]
	AND S.SubQueue = T.SubQueue
	AND S.TaskControlNumber = T.TaskControlNumber
	AND T.ProcessedAt IS NULL
WHEN NOT MATCHED BY TARGET THEN
	INSERT ([Queue], SubQueue, TaskControlNumber, Ssn, AccountNumber, Address1, Address2, City, [State], ZipCode, Country, OriginalAddressText, PrimaryPhone, OriginalPhoneText)
	VALUES(S.[Queue], S.SubQueue, S.TaskControlNumber, S.Ssn, S.AccountNumber, S.Address1, S.Address2, S.City, S.[State], S.ZipCode, S.Country, S.OriginalAddressText, S.PrimaryPhone, S.OriginalPhoneText)
;

RETURN 0

GRANT EXECUTE ON [acurintr].[GetAvailableLXQueueData] TO db_executor