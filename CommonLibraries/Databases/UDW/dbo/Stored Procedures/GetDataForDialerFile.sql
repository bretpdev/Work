﻿
CREATE PROCEDURE [dbo].[GetDataForDialerFile]
	@Ssn CHAR(9)
AS
	SELECT DISTINCT
		BF_SSN AS Ssn,
		DM_PRS_1 + ' ' + DM_PRS_LST AS Name,
		ISNULL(LN16.DaysDelq, 0) AS DaysDelq,
		PD42H.CONSENT_IND AS ConsentHomePhone,
		PD42H.DN_DOM_PHN_ARA + PD42H.DN_DOM_PHN_XCH + PD42H.DN_DOM_PHN_LCL AS HomePhone,
		PD42A.CONSENT_IND AS ConsentAltPhone,
		PD42A.DN_DOM_PHN_ARA + PD42A.DN_DOM_PHN_XCH + PD42A.DN_DOM_PHN_LCL AS AltPhone,
		PD42W.CONSENT_IND AS ConsentWorkPhone,
		PD42W.DN_DOM_PHN_ARA + PD42W.DN_DOM_PHN_XCH + PD42W.DN_DOM_PHN_LCL AS WorkPhone,
		PD30.DX_STR_ADR_1 AS Street1,
		PD30.DX_STR_ADR_2 AS Street2,
		'' AS Street3,
		PD30.DM_CT AS City,
		PD30.DC_DOM_ST AS [State],
		PD30.DF_ZIP_CDE AS Zip,
		LN10.LF_LON_CUR_OWN AS OwnerCode,
		ISNULL(AMT.TOT_DUE, 0) AS AmountDue,
		BAL.TotalBalance AS TotalBalance
	FROM
		PD10_Borrower PD10
		LEFT JOIN 
		(
			SELECT
				DF_SPE_ACC_ID,
				MAX(LN_DLQ_MAX) AS DaysDelq
			FROM
				LN16_Delinquency
			GROUP BY
				DF_SPE_ACC_ID
		)LN16
			ON LN16.DF_SPE_ACC_ID = PD10.DF_SPE_ACC_ID
		LEFT JOIN PD42_Phone PD42H
			ON PD42H.DF_SPE_ACC_ID = PD10.DF_SPE_ACC_ID
			AND PD42H.DC_PHN = 'H'
			AND PD42H.DI_PHN_VLD = 'Y'
		LEFT JOIN PD42_Phone PD42A
			ON PD42A.DF_SPE_ACC_ID = PD10.DF_SPE_ACC_ID
			AND PD42A.DC_PHN = 'H'
			AND PD42A.DI_PHN_VLD = 'Y'
		LEFT JOIN PD42_Phone PD42W
			ON PD42W.DF_SPE_ACC_ID = PD10.DF_SPE_ACC_ID
			AND PD42W.DC_PHN = 'H'
			AND PD42W.DI_PHN_VLD = 'Y'
		LEFT JOIN PD30_Address PD30
			ON PD30.DF_SPE_ACC_ID = PD10.DF_SPE_ACC_ID
		LEFT JOIN LN10_lon LN10 
			ON LN10.DF_SPE_ACC_ID = PD10.DF_SPE_ACC_ID
		LEFT JOIN BORR_AmountDue AMT
			ON AMT.DF_SPE_ACC_ID = PD10.DF_SPE_ACC_ID
		INNER JOIN 
		(
			SELECT	
				DF_SPE_ACC_ID,
				(SUM(LA_CUR_PRI)) AS TotalBalance
			FROM
				LN10_Loan
			GROUP BY
				DF_SPE_ACC_ID
				
		)BAL
			ON BAL.DF_SPE_ACC_ID = PD10.DF_SPE_ACC_ID
		WHERE
			PD10.BF_SSN = @Ssn

RETURN 0