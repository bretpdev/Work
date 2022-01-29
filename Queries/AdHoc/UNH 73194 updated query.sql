--Please submit a query for inbound and outbound calls for borrowers who reside in the state of California 
--during a scope period from 10/1/2021 to 10/31/2021. Please include the following output:

--1. Borrower Name
--2. Agent/Representative handling the call (use Agent ID #)
--3. Loan type (subsidized, unsubsidized)
--4. Inbound or Outbound call
--5. Call Duration

SELECT DISTINCT
	CAST(NCH.ActivityDate AS DATE) AS CallDate,
	RTRIM(RTRIM(Account.DM_PRS_1) + ' ' + RTRIM(Account.DM_PRS_MID)) + ' ' + RTRIM(RTRIM(Account.DM_PRS_LST) + ' ' + RTRIM(Account.DM_PRS_LST_SFX)) AS BorrowerName,
	NCH.AgentId AS AgentOnCall,
	AccountLN10.IC_LON_PGM AS LoanType,
	CASE WHEN NCH.CallType = 0 THEN 'Outbound' ELSE 'Inbound' END AS CallDirection,
	RIGHT('000' + CAST(NCH.CallLength / 60 / 60 AS VARCHAR(3)),3) + ':' + RIGHT('00' + CAST(NCH.CallLength / 60 % 60 AS VARCHAR(2)),2) + ':' + RIGHT('00' + CAST(NCH.CallLength % 60 AS VARCHAR(2)),2) AS CallDuration,
	NCH.CallLength,
	YEAR(NCH.ActivityDate) AS CallDateYear,
	MONTH(NCH.ActivityDate) AS CallDateMonth,
	DAY(NCH.ActivityDate) AS CallDateDay,
	CONVERT(VARCHAR, NCH.ActivityDate, 101) AS Concat,
	NCH.PhoneNumber,
	NCH.CallLength AS CallLengthActual,
	CAST(NCH.CallLength / 60 / 60 AS VARCHAR) + ':' + RIGHT('00' + CAST(NCH.CallLength / 60 % 60 AS VARCHAR(2)),2) + ':' + RIGHT('00' + CAST(NCH.CallLength % 60 AS VARCHAR(2)),2) AS CallDurationActual
FROM
	NobleCalls..NobleCallHistory NCH
	INNER JOIN UDW..PD10_PRS_NME Account
		ON Account.DF_SPE_ACC_ID = NCH.AccountIdentifier
	INNER JOIN UDW..LN10_LON AccountLN10
		ON AccountLN10.BF_SSN = Account.DF_PRS_ID
	INNER JOIN UDW..PD30_PRS_ADR PD30
		ON PD30.DF_PRS_ID = Account.DF_PRS_ID
		AND PD30.DC_ADR = 'L'
		AND PD30.DC_DOM_ST = 'CA'
WHERE
	CAST(NCH.ActivityDate AS DATE) BETWEEN '2021-10-01' AND '2021-10-31'
	AND NCH.CallType = 1

UNION

SELECT DISTINCT
	CAST(NCH.ActivityDate AS DATE) AS CallDate,
	RTRIM(RTRIM(Social.DM_PRS_1) + ' ' + RTRIM(Social.DM_PRS_MID)) + ' ' + RTRIM(RTRIM(Social.DM_PRS_LST) + ' ' + RTRIM(Social.DM_PRS_LST_SFX)) AS BorrowerName,
	NCH.AgentId AS AgentOnCall,
	SocialLN10.IC_LON_PGM AS LoanType,
	CASE WHEN NCH.CallType = 0 THEN 'Outbound' ELSE 'Inbound' END AS CallDirection,
	RIGHT('000' + CAST(NCH.CallLength / 60 / 60 AS VARCHAR(3)),3) + ':' + RIGHT('00' + CAST(NCH.CallLength / 60 % 60 AS VARCHAR(2)),2) + ':' + RIGHT('00' + CAST(NCH.CallLength % 60 AS VARCHAR(2)),2) AS CallDuration,
	NCH.CallLength,
	YEAR(NCH.ActivityDate) AS CallDateYear,
	MONTH(NCH.ActivityDate) AS CallDateMonth,
	DAY(NCH.ActivityDate) AS CallDateDay,
	CONVERT(VARCHAR, NCH.ActivityDate, 101) AS Concat,
	NCH.PhoneNumber,
	NCH.CallLength AS CallLengthActual,
	CAST(NCH.CallLength / 60 / 60 AS VARCHAR) + ':' + RIGHT('00' + CAST(NCH.CallLength / 60 % 60 AS VARCHAR(2)),2) + ':' + RIGHT('00' + CAST(NCH.CallLength % 60 AS VARCHAR(2)),2) AS CallDurationActual
FROM
	NobleCalls..NobleCallHistory NCH
	INNER JOIN UDW..PD10_PRS_NME Social
		ON Social.DF_PRS_ID = NCH.AccountIdentifier
	INNER JOIN UDW..LN10_LON SocialLN10
		ON SocialLN10.BF_SSN = Social.DF_PRS_ID
	INNER JOIN UDW..PD30_PRS_ADR PD30
		ON PD30.DF_PRS_ID = Social.DF_PRS_ID
		AND PD30.DC_ADR = 'L'
		AND PD30.DC_DOM_ST = 'CA'
WHERE
	CAST(NCH.ActivityDate AS DATE) BETWEEN '2021-10-01' AND '2021-10-31'	
	AND NCH.CallType = 1

UNION

SELECT DISTINCT
	CAST(NCH.ActivityDate AS DATE) AS CallDate,
	RTRIM(RTRIM(PhoneBorr.DM_PRS_1) + ' ' + RTRIM(PhoneBorr.DM_PRS_MID)) + ' ' + RTRIM(RTRIM(PhoneBorr.DM_PRS_LST) + ' ' + RTRIM(PhoneBorr.DM_PRS_LST_SFX)) AS BorrowerName,
	NCH.AgentId AS AgentOnCall,
	PhoneLN10.IC_LON_PGM AS LoanType,
	CASE WHEN NCH.CallType = 0 THEN 'Outbound' ELSE 'Inbound' END AS CallDirection,
	RIGHT('000' + CAST(NCH.CallLength / 60 / 60 AS VARCHAR(3)),3) + ':' + RIGHT('00' + CAST(NCH.CallLength / 60 % 60 AS VARCHAR(2)),2) + ':' + RIGHT('00' + CAST(NCH.CallLength % 60 AS VARCHAR(2)),2) AS CallDuration,
	NCH.CallLength,
	YEAR(NCH.ActivityDate) AS CallDateYear,
	MONTH(NCH.ActivityDate) AS CallDateMonth,
	DAY(NCH.ActivityDate) AS CallDateDay,
	CONVERT(VARCHAR, NCH.ActivityDate, 101) AS Concat,
	NCH.PhoneNumber,
	NCH.CallLength AS CallLengthActual,
	CAST(NCH.CallLength / 60 / 60 AS VARCHAR) + ':' + RIGHT('00' + CAST(NCH.CallLength / 60 % 60 AS VARCHAR(2)),2) + ':' + RIGHT('00' + CAST(NCH.CallLength % 60 AS VARCHAR(2)),2) AS CallDurationActual
FROM
	NobleCalls..NobleCallHistory NCH
	INNER JOIN UDW..PD42_PRS_PHN Phone
		ON RTRIM(Phone.DN_DOM_PHN_ARA) + RTRIM(Phone.DN_DOM_PHN_XCH) + RTRIM(Phone.DN_DOM_PHN_LCL) = NCH.PhoneNumber
	INNER JOIN UDW..PD10_PRS_NME PhoneBorr
		ON PhoneBorr.DF_PRS_ID = Phone.DF_PRS_ID
	INNER JOIN UDW..LN10_LON PhoneLN10
		ON PhoneLN10.BF_SSN = PhoneBorr.DF_PRS_ID
	INNER JOIN UDW..PD30_PRS_ADR PD30
		ON PD30.DF_PRS_ID = PhoneBorr.DF_PRS_ID
		AND PD30.DC_ADR = 'L'
		AND PD30.DC_DOM_ST = 'CA'
WHERE
	CAST(NCH.ActivityDate AS DATE) BETWEEN '2021-10-01' AND '2021-10-31'
	AND NCH.CallType = 1
ORDER BY
	NCH.CallLength
DESC


	