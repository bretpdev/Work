USE [CentralData]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [utnws98].[WebPortalStatsFED]
AS
DECLARE @Today DATE = GETDATE();
DECLARE @LastMonth DATE = DATEADD(MONTH, -1, @Today);
DECLARE @BegMonth DATE = DATEFROMPARTS(YEAR(@LastMonth),MONTH(@LastMonth),1);
DECLARE @EndMonth DATE = EOMONTH(@LastMonth);


SELECT 
	ISNULL(SUM(CreatedCount),0) AS CreatedOnlineAccount,
	ISNULL(SUM(AccessedCount),0) AS AccessedOnlineAccount,
	ISNULL(SUM(AccessedEcorrCount),0) AS AccessedEcorrInbox,
	ISNULL(SUM(MadeWebPaymentCount),0) AS MadeWebPayment,
	ISNULL(SUM(MadeMobilePaymentCount),0) AS MadeMobilePayment,
	ISNULL(SUM(RequestedEcorrBillCount),0) AS RequestedEcorrBill,
	ISNULL(SUM(RequestedEcorrLetterCount),0) AS RequestedEcorrLetter,
	ISNULL(SUM(RequestedEcorrTaxCount),0) AS RequestedEcorrTax,
	DATEADD(MONTH, -1, @Today) AS ReportingDate
FROM
	CDW..PD10_PRS_NME PD10
--on-line account counts from WB24
	LEFT JOIN 
	(
		SELECT DISTINCT
			CASE WHEN CAST(DF_CRT_DTS_WB24 AS DATE) >= @BegMonth AND CAST(DF_CRT_DTS_WB24 AS DATE) <= @EndMonth THEN 1 ELSE 0 END AS CreatedCount,
			CASE WHEN CAST(DF_USR_LST_ATH_DTS AS DATE)>= @BegMonth AND CAST(DF_USR_LST_ATH_DTS AS DATE) <= @EndMonth THEN 1 ELSE 0 END AS AccessedCount,
			CASE WHEN CAST(DF_USR_LST_IBX_ACS AS DATE) >= @BegMonth AND CAST(DF_USR_LST_IBX_ACS AS DATE) <= @EndMonth THEN 1 ELSE 0 END AS AccessedEcorrCount,
			DF_USR_SSN
		FROM
			CDW..WB24_CSM_USR_ACC
	) WB24 
		ON PD10.DF_PRS_ID = WB24.DF_USR_SSN
--web payment counts from RM03
	LEFT JOIN
	(
		SELECT DISTINCT
			BF_SSN,
			CASE WHEN NC_PAY_PRC = '01' AND NF_IPH NOT LIKE '%MBLAPP%' THEN 1 ELSE 0 END AS MadeWebPaymentCount,
			CASE WHEN NF_IPH LIKE '%MBLAPP%' THEN 1 ELSE 0 END AS MadeMobilePaymentCount
		FROM
			CDW..RM03_ONL_PAY
		WHERE
			CAST(NF_ONL_PAY_DTS AS DATE) >= @BegMonth
			AND CAST(NF_ONL_PAY_DTS AS DATE) <= @EndMonth
	) RM03 
		ON PD10.DF_PRS_ID = RM03.BF_SSN
--ecorr counts from PH05
	LEFT JOIN
	(
		SELECT DISTINCT
			DF_SPE_ID,
			CASE WHEN DI_CNC_EBL_OPI = 'Y' AND CAST(DF_DTS_EBL_OPI_EFF AS DATE) BETWEEN @BegMonth AND @EndMonth THEN 1 ELSE 0 END AS RequestedEcorrBillCount,
			CASE WHEN DI_CNC_ELT_OPI = 'Y' AND CAST(DF_DTS_ELT_OPI_EFF AS DATE) BETWEEN @BegMonth AND @EndMonth THEN 1 ELSE 0 END AS RequestedEcorrLetterCount,
			CASE WHEN DI_CNC_TAX_OPI = 'Y' AND CAST(DF_DTS_TAX_OPI_EFF AS DATE) BETWEEN @BegMonth AND @EndMonth THEN 1 ELSE 0 END AS RequestedEcorrTaxCount
		FROM
			CDW..PH05_CNC_EML
	) PH05
		ON PH05.DF_SPE_ID = PD10.DF_SPE_ACC_ID
		--active flag not needed as value of active flag is part of case statements above

GO