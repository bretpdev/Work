﻿CREATE PROCEDURE [pretnfrnot].[GetPreTransferData]
	@LetterId VARCHAR(10)
AS

SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED

SELECT DISTINCT
	LT20.RM_APL_PGM_PRC,
	LT20.RT_RUN_SRT_DTS_PRC,
	LT20.RN_SEQ_LTR_CRT_PRC,
	LT20.RN_SEQ_REC_PRC,
	LT20.RM_DSC_LTR_PRC AS [LetterId],
	LT20.DF_SPE_ACC_ID AS [AccountNumber],
	PD10.DF_PRS_ID AS [Ssn],
	LT20.OnEcorr,
	COALESCE(PH05.DI_VLD_CNC_EML_ADR, 'N') AS [ValidEcorrEmail],
	COALESCE(PH05.DX_CNC_EML_ADR, '') AS [Email],
	REPLACE(LTRIM(RTRIM(PD10.DM_PRS_1)),',','') AS [FirstName],
	REPLACE(LTRIM(RTRIM(PD10.DM_PRS_LST)),',','') AS [LastName],
	REPLACE(LTRIM(RTRIM(PD30.DX_STR_ADR_1)),',','') AS [Address1],
	REPLACE(LTRIM(RTRIM(PD30.DX_STR_ADR_2)),',','') AS [Address2],
	REPLACE(LTRIM(RTRIM(PD30.DM_CT)),',','') AS [City],
	REPLACE(LTRIM(RTRIM(PD30.DC_DOM_ST)),',','') AS [State],
	PD30.DF_ZIP_CDE AS [Zip],
	REPLACE(LTRIM(RTRIM(PD30.DM_FGN_CNY)),',','') AS [Country],
	PD30.DI_VLD_ADR AS [ValidAddress],
	PS.TransferDate,
	PS.RegionDeconversion,
	PS.SellingOwnerId,
	PS.LoanSaleStatus,
	PS.DelayCancelCode,
	LN83.LC_STA_LN83 AS [AchStatus],
	LTRIM(RTRIM(LN83.LC_EFT_SUS_REA)) AS [AchSuspensionReason],
	LN90.LD_FAT_EFF AS [LastPaymentDate],
	LN94.LC_RMT_BCH_SRC_IPT AS [LastPaymentSource],
	RM30.LC_RMT_PAY_SRC AS [LastPaymentSubSource],
	RS10.DAY_FLD AS [DueDay],
	0 AS IsCoborrower,
	LT20.DF_SPE_ACC_ID AS LetterAccountNumber
FROM
	LT20_LTR_REQ_PRC LT20
	INNER JOIN PD10_PRS_NME PD10 
		ON PD10.DF_SPE_ACC_ID = LT20.DF_SPE_ACC_ID
	INNER JOIN PD30_PRS_ADR PD30 
		ON PD30.DF_PRS_ID = PD10.DF_PRS_ID
	LEFT JOIN PH05_CNC_EML PH05 
		ON PH05.DF_SPE_ID = PD10.DF_SPE_ACC_ID
	LEFT JOIN 
	(--Presale info
		SELECT DISTINCT
			WK1J.BF_SSN,
			WK1J.IF_LON_SLE,
			OW30.IC_LON_SLE_STA AS [LoanSaleStatus],
			OW30.IC_RGN_RCV_DCV_LON AS [RegionDeconversion],
			OW30.IF_SLL_OWN AS [SellingOwnerId],
			OW30.ID_LON_SLE AS [TransferDate],
			OW30.IC_DLA_CAN_LTR AS [DelayCancelCode],
			OW30.IF_LST_DTS_OW30,
			DENSE_RANK() OVER(PARTITION BY WK1J.IF_LON_SLE ORDER BY COALESCE(OW30.IF_LST_DTS_OW30,'1900-01-01') DESC) [OW30Seq],
			ROW_NUMBER() OVER(PARTITION BY WK1J.IF_LON_SLE, WK1J.BF_SSN, WK1J.LN_SEQ ORDER BY WK1J.ID_LON_SLE_LST_PLR DESC) [Wk1jSeq1],
			ROW_NUMBER() OVER(PARTITION BY WK1J.IF_LON_SLE, WK1J.BF_SSN, WK1J.LN_SEQ, WK1J.ID_LON_SLE_LST_PLR ORDER BY WK1J.IT_SLE_LST_PLR DESC) [Wk1jSeq2],
			DENSE_RANK() OVER(PARTITION BY WK1J.IF_LON_SLE, WK1J.BF_SSN, WK1J.LN_SEQ ORDER BY COALESCE(LN99.LF_LST_DTS_LN99,'1900-01-01') DESC) [Ln99Seq]
		FROM
			WK1J_LON_SLE_PLR_History WK1J
			LEFT JOIN LN99_LON_SLE_FAT_History LN99 
				ON LN99.BF_SSN = WK1J.BF_SSN 
				AND LN99.LN_SEQ = WK1J.LN_SEQ
				AND CAST(COALESCE(LN99.LF_LST_DTS_LN99, GETDATE()) AS DATE) > DATEADD(DAY, -14, CAST(GETDATE() AS DATE))
			LEFT JOIN OW30_LON_SLE_CTL_History OW30 
				ON OW30.IF_LON_SLE = WK1J.IF_LON_SLE
	) PS 
		ON PS.BF_SSN = PD10.DF_PRS_ID 
		AND PS.OW30Seq = 1 
		AND PS.Wk1jSeq1 = 1 
		AND PS.Wk1jSeq2 = 1 
		AND PS.Ln99Seq = 1
	LEFT JOIN 
	(
		SELECT
			BF_SSN,
			LC_STA_LN83,
			LC_EFT_SUS_REA,
			LD_EFT_EFF_BEG,
			ROW_NUMBER() OVER(PARTITION BY BF_SSN ORDER BY LD_EFT_EFF_BEG DESC) [MaxFlag]
		FROM
			LN83_EFT_TO_LON
		WHERE
			LD_EFT_EFF_BEG <= CAST(GETDATE() AS DATE)
			AND	COALESCE(LD_EFT_EFF_END, DATEADD(DAY, 1, CAST(GETDATE() AS DATE))) > CAST(GETDATE() AS DATE) --If we don't know what the date is, then the date is not set so use tomorrow's date
	) LN83 
		ON LN83.BF_SSN = PD10.DF_PRS_ID
		AND LN83.MaxFlag = 1
	LEFT JOIN
	(
		SELECT
			LN90.BF_SSN,
			LN90.LN_SEQ,
			LN90.LN_FAT_SEQ,
			LN90.LD_FAT_EFF,
			ROW_NUMBER() OVER (PARTITION BY LN90.BF_SSN ORDER BY LN90.LD_FAT_EFF DESC) [MaxFlag]
		FROM
			LN90_FIN_ATY LN90
			INNER JOIN PD10_PRS_NME PD10 
				ON PD10.DF_PRS_ID = LN90.BF_SSN
			INNER JOIN LT20_LTR_REQ_PRC LT20
				ON LT20.DF_SPE_ACC_ID = PD10.DF_SPE_ACC_ID
		WHERE
			LN90.LC_STA_LON90 = 'A'
			AND COALESCE(LTRIM(RTRIM(LN90.LC_FAT_REV_REA)),'') = ''
			AND LN90.PC_FAT_TYP = '10'
			AND LN90.PC_FAT_SUB_TYP = '10'
			AND LT20.RM_DSC_LTR_PRC = @LetterId
			AND 
			(
				(
					LT20.PrintedAt IS NULL 
					AND LT20.OnEcorr = 0
				) 
				OR 
				(
					LT20.EcorrDocumentCreatedAt IS NULL
				)
			)
			AND LT20.InactivatedAt IS NULL
	) LN90 
		ON LN90.BF_SSN = PD10.DF_PRS_ID
		AND LN90.MaxFlag = 1
	LEFT JOIN LN94_LON_PAY_FAT LN94 
		ON LN94.BF_SSN = LN90.BF_SSN
		AND LN94.LN_SEQ = LN90.LN_SEQ 
		AND LN94.LN_FAT_SEQ = LN90.LN_FAT_SEQ
	LEFT JOIN RM30_BR_RMT RM30 
		ON RM30.LD_RMT_BCH_INI = LN94.LD_RMT_BCH_INI
		AND RM30.LC_RMT_BCH_SRC_IPT = LN94.LC_RMT_BCH_SRC_IPT
		AND RM30.LN_RMT_BCH_SEQ = LN94.LN_RMT_BCH_SEQ
		AND RM30.LN_RMT_SEQ = LN94.LN_RMT_SEQ
		AND RM30.LN_RMT_ITM = LN94.LN_RMT_ITM
		AND RM30.LN_RMT_ITM_SEQ = LN94.LN_RMT_ITM_SEQ
		AND RM30.BF_SSN = PD10.DF_PRS_ID
	LEFT JOIN  
	(
		SELECT DISTINCT 
			RS10.BF_SSN, 
			DAY(RS10.LD_RPS_1_PAY_DU) AS DAY_FLD
		FROM 
			RS10_BR_RPD RS10
		WHERE  
			RS10.LC_STA_RPST10 = 'A'
	) RS10
		ON RS10.BF_SSN = PD10.DF_PRS_ID

WHERE
    LT20.RM_DSC_LTR_PRC = @LetterId
    AND 
	(
		(
			LT20.PrintedAt IS NULL 
			AND LT20.OnEcorr = 0
		) 
		OR 
		(
			LT20.EcorrDocumentCreatedAt IS NULL
		)
	)
    AND LT20.InactivatedAt IS NULL

UNION ALL

SELECT DISTINCT
	LT20.RM_APL_PGM_PRC,
	LT20.RT_RUN_SRT_DTS_PRC,
	LT20.RN_SEQ_LTR_CRT_PRC,
	LT20.RN_SEQ_REC_PRC,
	LT20.RM_DSC_LTR_PRC AS [LetterId],
	LT20.DF_SPE_ACC_ID AS [AccountNumber],
	PD10.DF_PRS_ID AS [Ssn],
	LT20.OnEcorr,
	COALESCE(PH05.DI_VLD_CNC_EML_ADR, 'N') AS [ValidEcorrEmail],
	COALESCE(PH05.DX_CNC_EML_ADR, '') AS [Email],
	REPLACE(LTRIM(RTRIM(PD10.DM_PRS_1)),',','') AS [FirstName],
	REPLACE(LTRIM(RTRIM(PD10.DM_PRS_LST)),',','') AS [LastName],
	REPLACE(LTRIM(RTRIM(PD30.DX_STR_ADR_1)),',','') AS [Address1],
	REPLACE(LTRIM(RTRIM(PD30.DX_STR_ADR_2)),',','') AS [Address2],
	REPLACE(LTRIM(RTRIM(PD30.DM_CT)),',','') AS [City],
	REPLACE(LTRIM(RTRIM(PD30.DC_DOM_ST)),',','') AS [State],
	PD30.DF_ZIP_CDE AS [Zip],
	REPLACE(LTRIM(RTRIM(PD30.DM_FGN_CNY)),',','') AS [Country],
	PD30.DI_VLD_ADR AS [ValidAddress],
	PS.TransferDate,
	PS.RegionDeconversion,
	PS.SellingOwnerId,
	PS.LoanSaleStatus,
	PS.DelayCancelCode,
	LN83.LC_STA_LN83 AS [AchStatus],
	LTRIM(RTRIM(LN83.LC_EFT_SUS_REA)) AS [AchSuspensionReason],
	LN90.LD_FAT_EFF AS [LastPaymentDate],
	LN94.LC_RMT_BCH_SRC_IPT AS [LastPaymentSource],
	RM30.LC_RMT_PAY_SRC AS [LastPaymentSubSource],
	RS10.DAY_FLD AS [DueDay],
	1 AS IsCoborrower,
	LT20.Borrower_DF_SPE_ACC_ID AS LetterAccountNumber
FROM
	LT20_LTR_REQ_PRC_Coborrower LT20
	INNER JOIN PD10_PRS_NME PD10 
		ON PD10.DF_SPE_ACC_ID = LT20.DF_SPE_ACC_ID
	INNER JOIN PD30_PRS_ADR PD30 
		ON PD30.DF_PRS_ID = PD10.DF_PRS_ID
	INNER JOIN CDW..LN20_EDS LN20
		ON LN20.LF_EDS = PD10.DF_PRS_ID
		AND LN20.LC_STA_LON20 = 'A'
		AND LN20.LC_EDS_TYP = 'M'
	LEFT JOIN PH05_CNC_EML PH05 
		ON PH05.DF_SPE_ID = PD10.DF_SPE_ACC_ID
	LEFT JOIN 
	(--Presale info
		SELECT DISTINCT
			WK1J.BF_SSN,
			WK1J.IF_LON_SLE,
			OW30.IC_LON_SLE_STA AS [LoanSaleStatus],
			OW30.IC_RGN_RCV_DCV_LON AS [RegionDeconversion],
			OW30.IF_SLL_OWN AS [SellingOwnerId],
			OW30.ID_LON_SLE AS [TransferDate],
			OW30.IC_DLA_CAN_LTR AS [DelayCancelCode],
			OW30.IF_LST_DTS_OW30,
			DENSE_RANK() OVER(PARTITION BY WK1J.IF_LON_SLE ORDER BY COALESCE(OW30.IF_LST_DTS_OW30,'1900-01-01') DESC) [OW30Seq],
			ROW_NUMBER() OVER(PARTITION BY WK1J.IF_LON_SLE, WK1J.BF_SSN, WK1J.LN_SEQ ORDER BY WK1J.ID_LON_SLE_LST_PLR DESC) [Wk1jSeq1],
			ROW_NUMBER() OVER(PARTITION BY WK1J.IF_LON_SLE, WK1J.BF_SSN, WK1J.LN_SEQ, WK1J.ID_LON_SLE_LST_PLR ORDER BY WK1J.IT_SLE_LST_PLR DESC) [Wk1jSeq2],
			DENSE_RANK() OVER(PARTITION BY WK1J.IF_LON_SLE, WK1J.BF_SSN, WK1J.LN_SEQ ORDER BY COALESCE(LN99.LF_LST_DTS_LN99,'1900-01-01') DESC) [Ln99Seq]
		FROM
			WK1J_LON_SLE_PLR_History WK1J
			LEFT JOIN LN99_LON_SLE_FAT_History LN99 
				ON LN99.BF_SSN = WK1J.BF_SSN 
				AND LN99.LN_SEQ = WK1J.LN_SEQ
				AND CAST(COALESCE(LN99.LF_LST_DTS_LN99, GETDATE()) AS DATE) > DATEADD(DAY, -14, CAST(GETDATE() AS DATE))
			LEFT JOIN OW30_LON_SLE_CTL_History OW30 
				ON OW30.IF_LON_SLE = WK1J.IF_LON_SLE
	) PS 
		ON PS.BF_SSN = LN20.BF_SSN
		AND PS.OW30Seq = 1 
		AND PS.Wk1jSeq1 = 1 
		AND PS.Wk1jSeq2 = 1 
		AND PS.Ln99Seq = 1
	LEFT JOIN 
	(
		SELECT
			BF_SSN,
			LC_STA_LN83,
			LC_EFT_SUS_REA,
			LD_EFT_EFF_BEG,
			ROW_NUMBER() OVER(PARTITION BY BF_SSN ORDER BY LD_EFT_EFF_BEG DESC) [MaxFlag]
		FROM
			LN83_EFT_TO_LON
		WHERE
			LD_EFT_EFF_BEG <= CAST(GETDATE() AS DATE)
			AND	COALESCE(LD_EFT_EFF_END, DATEADD(DAY, 1, CAST(GETDATE() AS DATE))) > CAST(GETDATE() AS DATE) --If we don't know what the date is, then the date is not set so use tomorrow's date
	) LN83 
		ON LN83.BF_SSN = LN20.BF_SSN
		AND LN83.MaxFlag = 1
	LEFT JOIN
	(
		SELECT
			LN90.BF_SSN,
			LN90.LN_SEQ,
			LN90.LN_FAT_SEQ,
			LN90.LD_FAT_EFF,
			ROW_NUMBER() OVER (PARTITION BY LN90.BF_SSN, LN90.LN_SEQ ORDER BY LN90.LD_FAT_EFF DESC) [MaxFlag]
		FROM
			LN90_FIN_ATY LN90
			INNER JOIN CDW..LN20_EDS LN20
				ON LN20.BF_SSN = LN90.BF_SSN
				AND LN20.LN_SEQ = LN90.LN_SEQ
				AND LN20.LC_STA_LON20 = 'A'
				AND LN20.LC_EDS_TYP = 'M'
			INNER JOIN PD10_PRS_NME PD10 
				ON PD10.DF_PRS_ID = LN20.LF_EDS
			INNER JOIN LT20_LTR_REQ_PRC_Coborrower LT20
				ON LT20.DF_SPE_ACC_ID = PD10.DF_SPE_ACC_ID
		WHERE
			LN90.LC_STA_LON90 = 'A'
			AND COALESCE(LTRIM(RTRIM(LN90.LC_FAT_REV_REA)),'') = ''
			AND LN90.PC_FAT_TYP = '10'
			AND LN90.PC_FAT_SUB_TYP = '10'
			AND LT20.RM_DSC_LTR_PRC = @LetterId
			AND 
			(
				(
					LT20.PrintedAt IS NULL 
					AND LT20.OnEcorr = 0
				) 
				OR 
				(
					LT20.EcorrDocumentCreatedAt IS NULL
				)
			)
			AND LT20.InactivatedAt IS NULL
	) LN90 
		ON LN90.BF_SSN = LN20.BF_SSN
		AND LN90.LN_SEQ = LN20.LN_SEQ
		AND LN90.MaxFlag = 1
	LEFT JOIN LN94_LON_PAY_FAT LN94 
		ON LN94.BF_SSN = LN90.BF_SSN
		AND LN94.LN_SEQ = LN90.LN_SEQ 
		AND LN94.LN_FAT_SEQ = LN90.LN_FAT_SEQ
	LEFT JOIN RM30_BR_RMT RM30 
		ON RM30.LD_RMT_BCH_INI = LN94.LD_RMT_BCH_INI
		AND RM30.LC_RMT_BCH_SRC_IPT = LN94.LC_RMT_BCH_SRC_IPT
		AND RM30.LN_RMT_BCH_SEQ = LN94.LN_RMT_BCH_SEQ
		AND RM30.LN_RMT_SEQ = LN94.LN_RMT_SEQ
		AND RM30.LN_RMT_ITM = LN94.LN_RMT_ITM
		AND RM30.LN_RMT_ITM_SEQ = LN94.LN_RMT_ITM_SEQ
		AND RM30.BF_SSN = LN20.BF_SSN
	LEFT JOIN  
	(
		SELECT DISTINCT 
			RS10.BF_SSN, 
			DAY(RS10.LD_RPS_1_PAY_DU) AS DAY_FLD
		FROM 
			RS10_BR_RPD RS10
		WHERE  
			RS10.LC_STA_RPST10 = 'A'
	) RS10
		ON RS10.BF_SSN = LN20.BF_SSN

WHERE
    LT20.RM_DSC_LTR_PRC = @LetterId
    AND 
	(
		(
			LT20.PrintedAt IS NULL 
			AND LT20.OnEcorr = 0
		) 
		OR 
		(
			LT20.EcorrDocumentCreatedAt IS NULL
		)
	)
    AND LT20.InactivatedAt IS NULL
