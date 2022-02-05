--Currently only looking at 1 attempt.  Hardcoding all other attempts to default value
DECLARE @PhoneAttempt VARCHAR(MAX) = 
'  ' + --STAT 2 characters attempt code
' ' + --TYPE_HMPH 1 character type of home phone
'               ' + --PHONE 15 characters
'        ' + --DATE 8 character date
'    ' --TIME 4 character time

/*CORNERSTONE PORTION*/
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
SELECT DISTINCT
	WQ20.BF_SSN +
	'F' + --'F' for cornerstone
	WQ20.WF_QUE + WQ20.WF_SUB_QUE + --Queue/subqueue
	COALESCE(BorrowerEndorser.BorrowerSSN,BorrowerEndorser.EndorserSSN) + --Recip_ID
	'Outbound Delinquency Call' + SPACE(35) + --COMMENT1
	SPACE(60) + --COMMENT2
	'UT00204 ' + --AGENT 8 characters AY10.LF_USR_REQ_ACT
	SPACE(34) + --FILLER2 34 characters
	CASE WHEN PD40.DC_PHN IN('H','M')
		THEN 
			RIGHT('  ' + NCH.Disposition,2) + --FRSTSTAT1 2 characters attempt code
			PD40.PhoneType + --TYPE_HMPH 1 character type of home phone
			PD40.PhoneNumber + --PHONE1 15 characters
			CONVERT(VARCHAR,CAST(NCH.ActivityDate AS DATE),112) + --FRSTDATE1 8 character date
			REPLACE(CONVERT(VARCHAR(5),NCH.ActivityDate,108),':','') --FRSTTIME1 4 character time
		ELSE @PhoneAttempt
	END +
	@PhoneAttempt + --SecondAttempt on first phone
	@PhoneAttempt + --ThirdAttempt on first phone
	@PhoneAttempt + --FourthAttempt on first phone
	@PhoneAttempt + --FifthAttempt on first phone
	CASE WHEN PD40.DC_PHN = 'A' 
		THEN 
			RIGHT('  ' + NCH.Disposition,2) + --FRSTSTAT1 2 characters attempt code
			PD40.PhoneType + --TYPE_HMPH 1 character type of home phone
			PD40.PhoneNumber + --PHONE1 15 characters
			CONVERT(VARCHAR,CAST(NCH.ActivityDate AS DATE),112) + --FRSTDATE1 8 character date
			REPLACE(CONVERT(VARCHAR(5),NCH.ActivityDate,108),':','') --FRSTTIME1 4 character time
		ELSE @PhoneAttempt
	END + --FirstAttempt on second phone
	@PhoneAttempt + --SecondAttempt on second phone
	@PhoneAttempt + --ThirdAttempt on second phone
	@PhoneAttempt + --FourthAttempt on second phone
	@PhoneAttempt + --FifthAttempt on second phone
	CASE WHEN PD40.DC_PHN = 'W' 
		THEN 
			RIGHT('  ' + NCH.Disposition,2) + --FRSTSTAT1 2 characters attempt code
			PD40.PhoneType + --TYPE_HMPH 1 character type of home phone
			PD40.PhoneNumber + --PHONE1 15 characters
			CONVERT(VARCHAR,CAST(NCH.ActivityDate AS DATE),112) + --FRSTDATE1 8 character date
			REPLACE(CONVERT(VARCHAR(5),NCH.ActivityDate,108),':','') --FRSTTIME1 4 character time
		ELSE @PhoneAttempt
	END + --FirstAttempt on third phone
	@PhoneAttempt + --SecondAttempt on third phone
	@PhoneAttempt + --ThirdAttempt on third phone
	@PhoneAttempt + --FourthAttempt on third phone
	@PhoneAttempt   --FifthAttempt on third phone
FROM
	CDW..WQ20_TSK_QUE WQ20
	INNER JOIN CDW..PD10_PRS_NME PD10
		ON PD10.DF_PRS_ID = WQ20.BF_SSN
	INNER JOIN
	(
		SELECT DISTINCT
			COALESCE(PD10E.DF_SPE_ACC_ID,'') AS EndorserAccount,
			PD10.DF_SPE_ACC_ID AS BorrowerAccount,
			LN10.BF_SSN AS BorrowerSSN,
			COALESCE(LN20.LF_EDS,'') AS EndorserSSN
		FROM
			CDW..PD10_PRS_NME PD10
			INNER JOIN CDW..LN10_LON LN10
				ON PD10.DF_PRS_ID = LN10.BF_SSN
				AND LN10.LC_STA_LON10 = 'R'
				AND LN10.LA_CUR_PRI > 0.00
			LEFT JOIN CDW..LN20_EDS LN20
				ON PD10.DF_PRS_ID = LN20.LF_EDS
				AND LN20.LC_STA_LON20 = 'A'
			LEFT JOIN CDW..PD10_PRS_NME PD10E
				ON PD10E.DF_PRS_ID = LN20.LF_EDS
	) BorrowerEndorser
		ON PD10.DF_SPE_ACC_ID = BorrowerEndorser.BorrowerAccount
	INNER JOIN 
	(
		SELECT
			Response.AccountIdentifier,
			Response.ActivityDate,
			Response.PhoneNumber,
			Response.Disposition,
			ROW_NUMBER() OVER(PARTITION BY Response.AccountIdentifier, Response.PhoneNumber, Response.Disposition ORDER BY Response.ResponseCodeWeight DESC, Response.ActivityDate) AS RankedCall 
		FROM
		(
			SELECT
				NCH.AccountIdentifier,
				NCH.PhoneNumber,
				NCH.ActivityDate,
				CASE WHEN NCH.DispositionCode IN('FX') THEN '12'
					 WHEN NCH.DispositionCode IN('N','NC','HD','MD','NA','N1') THEN '15'
					 WHEN NCH.DispositionCode IN('BZ','B','B1') THEN '20'
					 WHEN NCH.DispositionCode IN('LM','L1') THEN '34'
					 WHEN NCH.DispositionCode IN('WN','W1') THEN '72'
					 WHEN NCH.DispositionCode IN('MC','92') THEN '91'
					 WHEN NCH.DispositionCode IN('A','AM','V1') THEN '92'
					 WHEN NCH.DispositionCode IN('42','61','PT','56','P') THEN '61'
					 WHEN NCH.DispositionCode IN('DF') THEN '62'
					 WHEN NCH.DispositionCode IN('FB') THEN '63'
					 WHEN NCH.DispositionCode IN('RP') THEN '71'
					 WHEN NCH.DispositionCode IN('RC','C1') THEN '73'
					 WHEN NCH.DispositionCode IN('9','TP') THEN '34'
					 WHEN NCH.DispositionCode IN('01','D','80','D1') THEN '38'
					 ELSE ''
				END AS Disposition,
				CASE WHEN RC.ResponseCode = 'CNTCT' THEN 1 ELSE 0 END AS ResponseCodeWeight
			FROM
				NobleCalls..NobleCallHistory NCH
				INNER JOIN NobleCalls..DispositionCodeMapping DCM
					ON DCM.DispositionCode = NCH.DispositionCode
				INNER JOIN NobleCalls..ResponseCodes RC
					ON RC.ResponseCodeId = DCM.ResponseCodeId
			WHERE
				NCH.IsInbound = 0 --outbound only
				AND NCH.RegionId = '3' --Cornerstone
				AND COALESCE(LTRIM(RTRIM(NCH.AccountIdentifier)),'') != ''
				AND CAST(NCH.CreatedAt AS DATE) = CAST(GETDATE() AS DATE)
				AND NCH.DispositionCode NOT IN ('XX','RS')
		) Response
	) NCH
		ON NCH.AccountIdentifier IN (BorrowerEndorser.BorrowerAccount, BorrowerEndorser.BorrowerSSN, BorrowerEndorser.EndorserAccount, BorrowerEndorser.EndorserSSN)
		AND NCH.RankedCall = 1
	INNER JOIN 
	(
		SELECT
			DF_PRS_ID,
			DC_PHN,
			CASE WHEN LTRIM(RTRIM(DN_FGN_PHN_CNY)) = '' AND LTRIM(RTRIM(DN_FGN_PHN_CT)) = '' AND LTRIM(RTRIM(DN_FGN_PHN_LCL)) = ''
				THEN 'D' --Domestic
				ELSE 'I' --International
			END AS PhoneType,
			CASE WHEN LTRIM(RTRIM(DN_FGN_PHN_CNY)) = '' AND LTRIM(RTRIM(DN_FGN_PHN_CT)) = '' AND LTRIM(RTRIM(DN_FGN_PHN_LCL)) = ''
				THEN CAST(LTRIM(RTRIM(DN_DOM_PHN_ARA)) + LTRIM(RTRIM(DN_DOM_PHN_XCH)) + LTRIM(RTRIM(DN_DOM_PHN_LCL)) AS VARCHAR(15))
				ELSE CAST(LTRIM(RTRIM(DN_FGN_PHN_CNY)) + LTRIM(RTRIM(DN_FGN_PHN_CT)) + LTRIM(RTRIM(DN_FGN_PHN_LCL)) AS VARCHAR(15))
			END AS PhoneNumber,
			COALESCE(DC_PHN_SRC,'  ') AS DC_PHN_SRC,
			 COALESCE(DC_ALW_ADL_PHN, ' ') AS DC_ALW_ADL_PHN
		FROM
			CDW..PD40_PRS_PHN
	) PD40 
		ON PD40.DF_PRS_ID = WQ20.BF_SSN
		AND PD40.PhoneNumber = NCH.PhoneNumber
WHERE
	WQ20.WF_QUE IN('C0','C1','C2','C3','C4','C5','C6','C7','C8','C9')
	AND WQ20.WC_STA_WQUE20 NOT IN ('X','C') --Open only


/*UHEAA PORTION*/
SELECT DISTINCT
	WQ20.BF_SSN +
	'U' + --'U' for uheaa
	WQ20.WF_QUE + WQ20.WF_SUB_QUE + --Queue/subqueue
	COALESCE(BorrowerEndorser.BorrowerSSN,BorrowerEndorser.EndorserSSN) + --Recip_ID
	'Outbound Delinquency Call' + SPACE(35) + --COMMENT1
	SPACE(60) + --COMMENT2
	'UT00204 ' + --AGENT 8 characters AY10.LF_USR_REQ_ACT
	SPACE(34) + --FILLER2 34 characters
	CASE WHEN PD40.DC_PHN IN('H','M')
		THEN 
			RIGHT('  ' + NCH.Disposition,2) + --FRSTSTAT1 2 characters attempt code
			PD40.PhoneType + --TYPE_HMPH 1 character type of home phone
			PD40.PhoneNumber + --PHONE1 15 characters
			CONVERT(VARCHAR,CAST(NCH.ActivityDate AS DATE),112) + --FRSTDATE1 8 character date
			REPLACE(CONVERT(VARCHAR(5),NCH.ActivityDate,108),':','') --FRSTTIME1 4 character time
		ELSE @PhoneAttempt
	END +
	@PhoneAttempt + --SecondAttempt on first phone
	@PhoneAttempt + --ThirdAttempt on first phone
	@PhoneAttempt + --FourthAttempt on first phone
	@PhoneAttempt + --FifthAttempt on first phone
	CASE WHEN PD40.DC_PHN = 'A' 
		THEN 
			RIGHT('  ' + NCH.Disposition,2) + --FRSTSTAT1 2 characters attempt code
			PD40.PhoneType + --TYPE_HMPH 1 character type of home phone
			PD40.PhoneNumber + --PHONE1 15 characters
			CONVERT(VARCHAR,CAST(NCH.ActivityDate AS DATE),112) + --FRSTDATE1 8 character date
			REPLACE(CONVERT(VARCHAR(5),NCH.ActivityDate,108),':','') --FRSTTIME1 4 character time
		ELSE @PhoneAttempt
	END + --FirstAttempt on second phone
	@PhoneAttempt + --SecondAttempt on second phone
	@PhoneAttempt + --ThirdAttempt on second phone
	@PhoneAttempt + --FourthAttempt on second phone
	@PhoneAttempt + --FifthAttempt on second phone
	CASE WHEN PD40.DC_PHN = 'W' 
		THEN 
			RIGHT('  ' + NCH.Disposition,2) + --FRSTSTAT1 2 characters attempt code
			PD40.PhoneType + --TYPE_HMPH 1 character type of home phone
			PD40.PhoneNumber + --PHONE1 15 characters
			CONVERT(VARCHAR,CAST(NCH.ActivityDate AS DATE),112) + --FRSTDATE1 8 character date
			REPLACE(CONVERT(VARCHAR(5),NCH.ActivityDate,108),':','') --FRSTTIME1 4 character time
		ELSE @PhoneAttempt
	END + --FirstAttempt on third phone
	@PhoneAttempt + --SecondAttempt on third phone
	@PhoneAttempt + --ThirdAttempt on third phone
	@PhoneAttempt + --FourthAttempt on third phone
	@PhoneAttempt   --FifthAttempt on third phone
FROM
	UDW..WQ20_TSK_QUE WQ20
	INNER JOIN UDW..PD10_PRS_NME PD10
		ON PD10.DF_PRS_ID = WQ20.BF_SSN
	INNER JOIN
	(
		SELECT DISTINCT
			COALESCE(PD10E.DF_SPE_ACC_ID,'') AS EndorserAccount,
			PD10.DF_SPE_ACC_ID AS BorrowerAccount,
			LN10.BF_SSN AS BorrowerSSN,
			COALESCE(LN20.LF_EDS,'') AS EndorserSSN
		FROM
			UDW..PD10_PRS_NME PD10
			INNER JOIN UDW..LN10_LON LN10
				ON PD10.DF_PRS_ID = LN10.BF_SSN
				AND LN10.LC_STA_LON10 = 'R'
				AND LN10.LA_CUR_PRI > 0.00
			LEFT JOIN UDW..LN20_EDS LN20
				ON PD10.DF_PRS_ID = LN20.LF_EDS
				AND LN20.LC_STA_LON20 = 'A'
			LEFT JOIN UDW..PD10_PRS_NME PD10E
				ON PD10E.DF_PRS_ID = LN20.LF_EDS
	) BorrowerEndorser
		ON PD10.DF_SPE_ACC_ID = BorrowerEndorser.BorrowerAccount
	INNER JOIN 
	(
		SELECT
			Response.AccountIdentifier,
			Response.ActivityDate,
			Response.PhoneNumber,
			Response.Disposition,
			ROW_NUMBER() OVER(PARTITION BY Response.AccountIdentifier, Response.PhoneNumber, Response.Disposition ORDER BY Response.ResponseCodeWeight DESC, Response.ActivityDate) AS RankedCall 
		FROM
		(
			SELECT
				NCH.AccountIdentifier,
				NCH.PhoneNumber,
				NCH.ActivityDate,
				CASE WHEN NCH.DispositionCode IN('FX') THEN '12'
					 WHEN NCH.DispositionCode IN('N','NC','HD','MD','NA','N1') THEN '15'
					 WHEN NCH.DispositionCode IN('BZ','B','B1') THEN '20'
					 WHEN NCH.DispositionCode IN('LM','L1') THEN '34'
					 WHEN NCH.DispositionCode IN('WN','W1') THEN '72'
					 WHEN NCH.DispositionCode IN('MC','92') THEN '91'
					 WHEN NCH.DispositionCode IN('A','AM','V1') THEN '92'
					 WHEN NCH.DispositionCode IN('42','61','PT','56','P') THEN '61'
					 WHEN NCH.DispositionCode IN('DF') THEN '62'
					 WHEN NCH.DispositionCode IN('FB') THEN '63'
					 WHEN NCH.DispositionCode IN('RP') THEN '71'
					 WHEN NCH.DispositionCode IN('RC','C1') THEN '73'
					 WHEN NCH.DispositionCode IN('9','TP') THEN '34'
					 WHEN NCH.DispositionCode IN('01','D','80','D1') THEN '38'
					 ELSE ''
				END AS Disposition,
				CASE WHEN RC.ResponseCode = 'CNTCT' THEN 1 ELSE 0 END AS ResponseCodeWeight
			FROM
				NobleCalls..NobleCallHistory NCH
				INNER JOIN NobleCalls..DispositionCodeMapping DCM
					ON DCM.DispositionCode = NCH.DispositionCode
				INNER JOIN NobleCalls..ResponseCodes RC
					ON RC.ResponseCodeId = DCM.ResponseCodeId
			WHERE
				NCH.IsInbound = 0 --outbound only
				AND NCH.RegionId = '2' --Uheaa
				AND COALESCE(LTRIM(RTRIM(NCH.AccountIdentifier)),'') != ''
				AND CAST(NCH.CreatedAt AS DATE) = CAST(GETDATE() AS DATE)
				AND NCH.DispositionCode NOT IN('XX','RS')
		) Response
	) NCH
		ON NCH.AccountIdentifier IN (BorrowerEndorser.BorrowerAccount, BorrowerEndorser.BorrowerSSN, BorrowerEndorser.EndorserAccount, BorrowerEndorser.EndorserSSN)
		AND NCH.RankedCall = 1
	INNER JOIN 
	(
		SELECT
			DF_PRS_ID,
			DC_PHN,
			CASE WHEN LTRIM(RTRIM(DN_FGN_PHN_CNY)) = '' AND LTRIM(RTRIM(DN_FGN_PHN_CT)) = '' AND LTRIM(RTRIM(DN_FGN_PHN_LCL)) = ''
				THEN 'D' --Domestic
				ELSE 'I' --International
			END AS PhoneType,
			CASE WHEN LTRIM(RTRIM(DN_FGN_PHN_CNY)) = '' AND LTRIM(RTRIM(DN_FGN_PHN_CT)) = '' AND LTRIM(RTRIM(DN_FGN_PHN_LCL)) = ''
				THEN CAST(LTRIM(RTRIM(DN_DOM_PHN_ARA)) + LTRIM(RTRIM(DN_DOM_PHN_XCH)) + LTRIM(RTRIM(DN_DOM_PHN_LCL)) AS VARCHAR(15))
				ELSE CAST(LTRIM(RTRIM(DN_FGN_PHN_CNY)) + LTRIM(RTRIM(DN_FGN_PHN_CT)) + LTRIM(RTRIM(DN_FGN_PHN_LCL)) AS VARCHAR(15))
			END AS PhoneNumber,
			COALESCE(DC_PHN_SRC,'  ') AS DC_PHN_SRC,
			 COALESCE(DC_ALW_ADL_PHN, ' ') AS DC_ALW_ADL_PHN
		FROM
			UDW..PD42_PRS_PHN
	) PD40 
		ON PD40.DF_PRS_ID = WQ20.BF_SSN
		AND PD40.PhoneNumber = NCH.PhoneNumber
WHERE
	WQ20.WF_QUE IN('C0','C1','C2','C3','C4','C5','C6','C7','C8','C9')
	AND WQ20.WC_STA_WQUE20 NOT IN ('X','C') --Open only