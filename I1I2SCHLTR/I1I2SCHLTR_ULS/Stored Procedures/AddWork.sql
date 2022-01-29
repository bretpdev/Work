﻿CREATE PROCEDURE [i1i2schltr].[AddWork]
AS

DECLARE @NOW DATETIME = GETDATE();
DECLARE @TODAY DATE = @NOW;
DECLARE @LastRunDate DATE = (SELECT MAX(RunDate) FROM i1i2schltr.RunDates WHERE DeletedAt IS NULL AND DeletedBy IS NULL);
IF @LastRunDate IS NULL
BEGIN
	SET @LastRunDate = CONVERT(DATE, '19000101');
END

IF @TODAY > @LastRunDate
BEGIN
BEGIN TRY
	BEGIN TRANSACTION;

	INSERT INTO i1i2schltr.RunDates(RunDate,AddedAt)
	VALUES (@TODAY,@NOW);

	DECLARE @RunDateId INT = @@IDENTITY;

	TRUNCATE TABLE ULS.[i1i2schltr].ProcessingDemographics;
	TRUNCATE TABLE ULS.[i1i2schltr].ProcessingLG29;
	TRUNCATE TABLE ULS.[i1i2schltr].ProcessingBorrowerSchools;

	INSERT INTO ULS.[i1i2schltr].ProcessingDemographics(BF_SSN,WF_QUE,DM_PRS_1,DM_PRS_MID,DM_PRS_LST,DX_STR_ADR_1,DX_STR_ADR_2,DM_CT,DC_DOM_ST,DF_ZIP_CDE,PHN_EVE,DI_VLD_ADR,DI_PHN_VLD,DN_ALT_PHN,DX_ADR_EML,DD_SKP_BEG)
	--GET PDEM INFORMATION FOR ALL BORROWERS IN QUEUE
	SELECT DISTINCT
		WQ20.BF_SSN,
		WQ20.WF_QUE,
		PD10.DM_PRS_1,
		PD10.DM_PRS_MID,
		PD10.DM_PRS_LST,
		PD30.DX_STR_ADR_1,
		PD30.DX_STR_ADR_2,
		PD30.DM_CT,
		PD30.DC_DOM_ST,
		PD30.DF_ZIP_CDE,
		PD42.DN_DOM_PHN_ARA + PD42.DN_DOM_PHN_XCH + PD42.DN_DOM_PHN_LCL AS PHN_EVE,
		PD30.DI_VLD_ADR,
		PD42.DI_PHN_VLD,
		CASE 
			WHEN LEN(PD42_ALT.DN_DOM_PHN_ARA) > 0 THEN PD42_ALT.DN_DOM_PHN_ARA + PD42_ALT.DN_DOM_PHN_XCH + PD42_ALT.DN_DOM_PHN_LCL 
			ELSE '' 
		END AS DN_ALT_PHN,
		PD32.DX_ADR_EML,
		PD27.DD_SKP_BEG
	FROM
		UDW..WQ20_TSK_QUE WQ20
		INNER JOIN UDW..PD10_PRS_NME PD10
			ON PD10.DF_PRS_ID = WQ20.BF_SSN
		INNER JOIN UDW..PD30_PRS_ADR PD30
			ON PD30.DF_PRS_ID = PD10.DF_PRS_ID
		INNER JOIN UDW..PD42_PRS_PHN PD42
			ON PD42.DF_PRS_ID = PD30.DF_PRS_ID
			AND PD42.DC_PHN = 'H'
		INNER JOIN UDW..PD27_PRS_SKP_PRC PD27
			ON PD27.DF_PRS_ID = PD42.DF_PRS_ID
		LEFT JOIN UDW..PD42_PRS_PHN PD42_ALT
			ON PD42_ALT.DF_PRS_ID = PD42.DF_PRS_ID
			AND PD42_ALT.DC_PHN = 'A'
		LEFT JOIN UDW..PD32_PRS_ADR_EML PD32
			ON PD32.DF_PRS_ID = PD42.DF_PRS_ID
			AND PD32.DC_STA_PD32 = 'A'
			AND PD32.DI_VLD_ADR_EML = 'Y'
		LEFT JOIN UDW..AY10_BR_LON_ATY AY10
			ON AY10.BF_SSN = PD42.DF_PRS_ID
			AND PD27.DD_SKP_BEG < AY10.LD_ATY_REQ_RCV
			AND AY10.PF_REQ_ACT IN ('KLSLT','SCHL1')
	WHERE
		PD30.DC_ADR = 'L'
		AND PD27.DC_STA_SKP = '2' --ACTIVE SKIP
		AND WQ20.WF_QUE IN ('I1','I2')
		AND AY10.BF_SSN IS NULL;


	INSERT INTO ULS.[i1i2schltr].ProcessingLG29(BF_SSN,SCL_ID,IC_SSR_PTC,IC_DOE_SCL_STA)
	--GET ALL UNIQUE SCHOOLS ON LG29 FOR STUDENTS ASSOCIATED WITH BRWS IN QUEUES
	SELECT DISTINCT
		LN10.BF_SSN,
		SD02.IF_OPS_SCL_RPT AS SCL_ID,
		SC01.IC_SSR_PTC,
		SC01.IC_DOE_SCL_STA
	FROM
		ODW..SD02_STU_ENR SD02
		INNER JOIN ODW..SC01_LGS_SCL_INF SC01
			ON SC01.IF_IST = SD02.IF_OPS_SCL_RPT
		INNER JOIN  UDW..LN10_LON LN10
			ON LN10.LF_STU_SSN = SD02.DF_PRS_ID_STU
		INNER JOIN UDW..WQ20_TSK_QUE WQ20
			ON WQ20.BF_SSN = LN10.BF_SSN
	WHERE
		WQ20.WF_QUE IN ('I1','I2')
		AND LN10.LC_STA_LON10 = 'R';

	INSERT INTO ULS.[i1i2schltr].ProcessingBorrowerSchools(BF_SSN,SCL_ID)
	--GET MOST RECENT GTY DATE & SCHOOL FOR BORROWERS IN THESE QUEUES
	SELECT DISTINCT
		GA01.DF_PRS_ID_BR AS SSN,
		CASE 
			WHEN GR10.LF_DOE_SCL_ENR_CUR IS NOT NULL THEN GR10.LF_DOE_SCL_ENR_CUR 
			ELSE GA01.AF_APL_OPS_SCL
		END AS SCL_ID
	FROM
		ODW..GA01_APP GA01
		INNER JOIN ODW..GA10_LON_APP GA10
			ON GA10.AF_APL_ID = GA01.AF_APL_ID
		INNER JOIN UDW..WQ20_TSK_QUE WQ20
			ON WQ20.BF_SSN = GA01.DF_PRS_ID_BR
		--Make sure the borrower has a I1, I2 queue
		INNER JOIN
		(
			SELECT
				GA01.DF_PRS_ID_BR,
				MAX(GA10.AD_PRC) AS AD_PRC
			FROM
				ODW..GA01_APP GA01
				INNER JOIN ODW..GA10_LON_APP GA10
					ON GA10.AF_APL_ID = GA01.AF_APL_ID
				INNER JOIN UDW..WQ20_TSK_QUE WQ20
					ON WQ20.BF_SSN = GA01.DF_PRS_ID_BR
			WHERE
				GA10.AC_PRC_STA = 'A'
				AND 0 < GA10.AA_GTE_LON_AMT - GA10.AA_TOT_RFD - ISNULL(GA10.AA_TOT_CAN,0)
				AND WQ20.WF_QUE IN ('I1','I2')
			GROUP BY
				GA01.DF_PRS_ID_BR
		) GA01_AGG
			ON GA01_AGG.AD_PRC = GA10.AD_PRC
			AND GA01_AGG.DF_PRS_ID_BR = GA01.DF_PRS_ID_BR
		INNER JOIN UDW..LN10_LON LN10
			ON LN10.LF_LON_ALT = GA10.AF_APL_ID
			AND GA10.AF_APL_ID_SFX = RIGHT('0' + CAST(LN10.LN_LON_ALT_SEQ AS VARCHAR(3)), 2)
		LEFT JOIN [i1i2schltr].[PrivateLoanPrograms] PL
			ON PL.LoanProgram = LN10.IC_LON_PGM
		--Private Loan School Information
		LEFT JOIN
		(
			SELECT
				GR10.BF_SSN,
				GR10.LF_DOE_SCL_ENR_CUR,
				MAX(GR10.WN_SEQ_GR10) AS WN_SEQ_GR10,
				SC10.II_SCL_CHS_PTC
			FROM
				UDW..GR10_RPT_LON_APL GR10
				INNER JOIN UDW..SC10_SCH_DMO SC10
					ON SC10.IF_DOE_SCL = GR10.LF_DOE_SCL_ENR_CUR
			GROUP BY
				GR10.BF_SSN,
				GR10.LF_DOE_SCL_ENR_CUR,
				SC10.II_SCL_CHS_PTC
		) GR10
			ON GR10.BF_SSN = GA01.DF_PRS_ID_BR
			AND PL.LoanProgram IS NOT NULL
			AND GR10.II_SCL_CHS_PTC IN ('N','',' ')
	WHERE
		GA10.AC_PRC_STA = 'A'
		AND 0 < GA10.AA_GTE_LON_AMT - GA10.AA_TOT_RFD - ISNULL(GA10.AA_TOT_CAN,0)
		AND LN10.LC_STA_LON10 = 'R';

	--Corresponds to the R2 file from UTLWD11
	INSERT INTO ULS.[i1i2schltr].PrintData(SSN,[Queue],FirstName,MiddleInitial,LastName,Address1,Address2,City,[State],Zip,Phone,School,SchoolStatus,AlternatePhone,Email,AddedAt,RunDateId)
	SELECT
		PD.BF_SSN AS SSN,
		PD.WF_QUE AS [Queue],
		PD.DM_PRS_1 AS FirstName,
		PD.DM_PRS_MID AS MiddleInitial,
		PD.DM_PRS_LST AS LastName,
		PD.DX_STR_ADR_1 AS Address1,
		PD.DX_STR_ADR_2 AS Address2,
		PD.DM_CT AS City,
		PD.DC_DOM_ST AS [State],
		PD.DF_ZIP_CDE AS Zip,
		PD.PHN_EVE AS Phone,
		CASE WHEN PL.SCL_ID = '88888800' OR ISNULL(PL.SCL_ID, '') = '' THEN PBS.SCL_ID ELSE PL.SCL_ID END AS School,
		ISNULL(PL.IC_DOE_SCL_STA, '') AS SchoolStatus,
		ISNULL(PD.DN_ALT_PHN, '') AS AlternatePhone,
		ISNULL(PD.DX_ADR_EML, '') AS Email,
		@NOW AS AddedAt,
		@RunDateId AS RunDateId
	FROM
		ULS.[i1i2schltr].ProcessingDemographics PD
		LEFT JOIN ULS.[i1i2schltr].ProcessingLG29 PL
			ON PL.BF_SSN = PD.BF_SSN
		LEFT JOIN ULS.[i1i2schltr].ProcessingBorrowerSchools PBS
			ON PBS.BF_SSN = PD.BF_SSN
	WHERE
		(
			(
				ISNULL(PL.SCL_ID, '') != '88888800' 
				AND ISNULL(PL.SCL_ID, '') != ''
			)
			OR 
			(
				ISNULL(PBS.SCL_ID, '') != '88888800' 
				AND ISNULL(PBS.SCL_ID, '') != ''
			)
		);
		
	--Corresponds to the R3 file from UTLWD11
	INSERT INTO ULS.[i1i2schltr].QueueTaskData(SSN,[Queue],AddedAt,RunDateId)
	SELECT
		PD.BF_SSN AS SSN,
		PD.WF_QUE AS [Queue],
		@NOW AS AddedAt,
		@RunDateId AS RunDateId
	FROM
		ULS.[i1i2schltr].ProcessingDemographics PD
		LEFT JOIN ULS.[i1i2schltr].ProcessingLG29 PL
			ON PL.BF_SSN = PD.BF_SSN
		LEFT JOIN ULS.[i1i2schltr].ProcessingBorrowerSchools PBS
			ON PBS.BF_SSN = PD.BF_SSN
	WHERE
		(
			(
				ISNULL(PL.SCL_ID, '') != '88888800' 
				AND ISNULL(PL.SCL_ID, '') != ''
			)
			OR 
			(
				ISNULL(PBS.SCL_ID, '') != '88888800' 
				AND ISNULL(PBS.SCL_ID, '') != ''
			)
		)
		AND
		(
			PD.DI_VLD_ADR = 'Y' 
			OR 
			(
				PD.DI_VLD_ADR = 'N' 
				AND ISNULL(PL.IC_SSR_PTC, '') = 'C'
			)
		);

	--Corresponds to the R4 file from UTLWD11
	INSERT INTO ULS.[i1i2schltr].CommentData(SSN,AddedAt,RunDateId)
	SELECT
		PD.BF_SSN AS SSN,
		@NOW AS AddedAt,
		@RunDateId AS RunDateId
	FROM
		ULS.[i1i2schltr].ProcessingDemographics PD
		LEFT JOIN ULS.[i1i2schltr].ProcessingLG29 PL
			ON PL.BF_SSN = PD.BF_SSN
		LEFT JOIN ULS.[i1i2schltr].ProcessingBorrowerSchools PBS
			ON PBS.BF_SSN = PD.BF_SSN
	WHERE
		(
			(
				ISNULL(PL.SCL_ID, '') != '88888800' 
				AND ISNULL(PL.SCL_ID, '') != ''
			)
			OR 
			(
				ISNULL(PBS.SCL_ID, '') != '88888800' 
				AND ISNULL(PBS.SCL_ID, '') != ''
			)
		);

	COMMIT TRANSACTION;
END TRY
BEGIN CATCH
	ROLLBACK TRANSACTION;
	THROW;
END CATCH
END
ELSE
BEGIN
	PRINT 'Add Work Already Ran For the Current Date';
END;