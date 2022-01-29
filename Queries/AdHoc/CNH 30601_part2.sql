USE [ULS]
GO
/****** Object:  StoredProcedure [dbo].[PopulateScraTables]    Script Date: XX/XX/XXXX XX:XX:XX AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER PROCEDURE [dbo].[PopulateScraTables]
AS
BEGIN
	IF 
		EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'scra' and TABLE_NAME = '_BorrowersPopulation') 
		AND EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'scra' and TABLE_NAME = '_EndorsersPopulation') 
	BEGIN
	BEGIN TRANSACTION
		DECLARE @ERROR INT = X

	SET CONCAT_NULL_YIELDS_NULL OFF
	DECLARE @RX TABLE(ArcTypeId INT, ArcResponseCodeId INT, AccountNumber CHAR(XX), RecipientId CHAR(X), Arc VARCHAR(X), 
					ScriptId CHAR(XX), ProcessOn DATETIME, Comment VARCHAR(XXX), IsReference BIT, IsEndorser BIT, 
					ProcessFrom DATETIME, ProcessTo DATETIME, NeededBy DATETIME, RegardsTo CHAR(X), RegardsCode CHAR(X), 
					CreatedAt DATETIME, CreatedBy VARCHAR(XX), ProcessedAt DATETIME)
	DECLARE @RX TABLE(ArcTypeId INT, ArcResponseCodeId INT, AccountNumber CHAR(XX), RecipientId CHAR(X), Arc VARCHAR(X), 
					ScriptId CHAR(XX), ProcessOn DATETIME, Comment VARCHAR(XXX), IsReference BIT, IsEndorser BIT, 
					ProcessFrom DATETIME, ProcessTo DATETIME, NeededBy DATETIME, RegardsTo CHAR(X), RegardsCode CHAR(X), 
					CreatedAt DATETIME, CreatedBy VARCHAR(XX), ProcessedAt DATETIME)
	DECLARE @RX TABLE(ArcTypeId INT, ArcResponseCodeId INT, AccountNumber CHAR(XX), RecipientId CHAR(X), Arc VARCHAR(X), 
					ScriptId CHAR(XX), ProcessOn DATETIME, Comment VARCHAR(XXX), IsReference BIT, IsEndorser BIT, 
					ProcessFrom DATETIME, ProcessTo DATETIME, NeededBy DATETIME, RegardsTo CHAR(X), RegardsCode CHAR(X), 
					CreatedAt DATETIME, CreatedBy VARCHAR(XX), ProcessedAt DATETIME)
	DECLARE @RX TABLE(ArcTypeId INT, ArcResponseCodeId INT, AccountNumber CHAR(XX), RecipientId CHAR(X), Arc VARCHAR(X), 
					ScriptId CHAR(XX), ProcessOn DATETIME, Comment VARCHAR(XXX), IsReference BIT, IsEndorser BIT, 
					ProcessFrom DATETIME, ProcessTo DATETIME, NeededBy DATETIME, RegardsTo CHAR(X), RegardsCode CHAR(X), 
					CreatedAt DATETIME, CreatedBy VARCHAR(XX), ProcessedAt DATETIME)
	DECLARE @RX TABLE(ArcTypeId INT, ArcResponseCodeId INT, AccountNumber CHAR(XX), RecipientId CHAR(X), Arc VARCHAR(X), 
					ScriptId CHAR(XX), ProcessOn DATETIME, Comment VARCHAR(XXX), IsReference BIT, IsEndorser BIT, 
					ProcessFrom DATETIME, ProcessTo DATETIME, NeededBy DATETIME, RegardsTo CHAR(X), RegardsCode CHAR(X), 
					CreatedAt DATETIME, CreatedBy VARCHAR(XX), ProcessedAt DATETIME)
	DECLARE @RX TABLE(ArcTypeId INT, ArcResponseCodeId INT, AccountNumber CHAR(XX), RecipientId CHAR(X), Arc VARCHAR(X), 
					ScriptId CHAR(XX), ProcessOn DATETIME, Comment VARCHAR(XXX), IsReference BIT, IsEndorser BIT, 
					ProcessFrom DATETIME, ProcessTo DATETIME, NeededBy DATETIME, RegardsTo CHAR(X), RegardsCode CHAR(X), 
					CreatedAt DATETIME, CreatedBy VARCHAR(XX), ProcessedAt DATETIME)

	/*Borrower USCRA/MSCRA
	USCRA for updates on borrowers that already have SCRA database entry
	MSCRA for new borrowers being added to SCRA database*/
	INSERT INTO @RX
		SELECT DISTINCT
			X AS ArcTypeId,
			NULL AS ArcResponseCodeId,
			BP.DF_SPE_ACC_ID AS AccountNumber,
			NULL AS RecipientId,
			CASE WHEN	BP.LC_NTF_ACT_DUT = 'Y' 	--Notified of active duty
						AND BP.LC_INT_RDC_PGM = 'M'
						AND BP.LR_ITR <= X 
						AND 
						(
							AD.BeginDate != BP.LD_EID_BEGIN_DATE 
							OR AD.EndDate != 'XXXX-XX-XX'
						) 
					THEN 'USCRA'
				WHEN	BP.LC_INT_RDC_PGM = 'M' --Either the begin or end date has changed
						AND BP.LR_ITR <= X 
						AND 
						(
							AD.BeginDate != BP.LD_ACT_DUT_BEG 
							OR AD.EndDate != BP.LD_ACT_DUT_END
						) 
					THEN 'USCRA'
				WHEN	BP.LC_INT_RDC_PGM = 'M' --No Entry in the ActiveDuty table
						AND BP.LR_ITR <= X 
						AND AD.ActiveDutyId IS NULL
					THEN 'USCRA'
				WHEN	BP.LC_INT_RDC_PGM = 'M' --Handles adding a new USCRA if the end date is unknown and the borrower has had the benefit for almost a year
						AND BP.LR_ITR <= X 
						AND DATEDIFF(DAY,GETDATE(),BP.LD_ITR_EFF_END) <= XX
						AND AD.BeginDate = BP.LD_ACT_DUT_BEG
						AND 
						(
							AD.EndDate = BP.LD_ACT_DUT_END
							OR AD.EndDate = 'XXXX-XX-XX'
						)
					THEN 'USCRA'
				WHEN LNXX.BF_SSN IS NOT NULL
					THEN 'USCRA'
					ELSE 'MSCRA'
			END AS Arc,
			'UTLWSXX' AS ScriptId,
			GETDATE() AS ProcessOn,
			'' AS Comment,
			X AS IsReference,
			X AS IsEndorser,
			NULL AS ProcessFrom,
			NULL AS ProcessTo,
			NULL AS NeededBy,
			NULL AS RegardsTo,
			NULL AS RegardsCode,
			GETDATE() AS CreatedAt,
			'UTLWSXX' AS CreatedBy,
			NULL AS ProcessedAt
		FROM
			ULS.scra._BorrowersPopulation BP
			LEFT JOIN ULS.scra.Borrowers MIL
				ON MIL.BorrowerAccountNumber = BP.DF_SPE_ACC_ID
			LEFT JOIN ULS.scra.ActiveDuty AD
				ON AD.BorrowerId = MIL.BorrowerId
			LEFT JOIN UDW..PDXX_PRS_NME PDXX
				ON PDXX.DF_SPE_ACC_ID = BP.DF_SPE_ACC_ID
			LEFT JOIN 
			(
				SELECT DISTINCT
					BF_SSN
				FROM
					UDW..LNXX_INT_RTE_HST
				WHERE
					LC_INT_RDC_PGM = 'M'
					AND LC_STA_LONXX = 'A'
			) LNXX
				ON LNXX.BF_SSN = PDXX.DF_PRS_ID
		WHERE
			NOT 
			(
				(   --exclude loans disbursed after active duty
					BP.LD_LON_X_DSB > BP.LD_ACT_DUT_BEG 
					AND BP.LC_NTF_ACT_DUT != 'Y'
				) 
				OR 
				(    --exclude reservists where loans disburse after active duty
					BP.LD_LON_X_DSB > ISNULL(BP.LD_EID_BEGIN_DATE,'XXXX-XX-XX') 
					AND BP.LC_NTF_ACT_DUT = 'Y'
				)
			)
			AND NOT 
			(       --non active duty borrowers
				BP.LC_ACT_DUT = 'N'
				AND BP.LC_LFT_ACT_DUT = 'N'
				AND BP.LC_NTF_ACT_DUT = 'N'
			)
			AND 
			(
				(   --active duty not receiving interest benefit
					BP.LR_ITR > X 
					AND BP.LC_ACT_DUT = 'X'
				)
				OR 
				(    --reservist not receiving interest benefit
					BP.LC_ACT_DUT = 'N' 
					AND BP.LC_NTF_ACT_DUT = 'Y' 
					AND BP.LR_ITR > X
				)
				OR 
				(    --reservist receiving interest benefit
					GETDATE() BETWEEN BP.LD_ITR_EFF_BEG AND BP.LD_ITR_EFF_END 
					AND BP.LC_INT_RDC_PGM = 'M' 
					AND BP.LR_ITR <= X
					AND BP.LC_NTF_ACT_DUT = 'Y'
					AND 
					(
						AD.BeginDate != BP.LD_EID_BEGIN_DATE 
						OR AD.EndDate != 'XXXX-XX-XX'
						OR AD.ActiveDutyId IS NULL
					)
				)
				OR 
				(   --non reservist receiving interest benefit
					GETDATE() BETWEEN BP.LD_ITR_EFF_BEG AND BP.LD_ITR_EFF_END 
					AND BP.LC_INT_RDC_PGM = 'M' 
					AND BP.LR_ITR <= X
					AND BP.LC_NTF_ACT_DUT != 'Y'
					AND 
					(
						AD.BeginDate != BP.LD_ACT_DUT_BEG 
						OR AD.EndDate != COALESCE(BP.LD_ACT_DUT_END ,'XXXX-XX-XX')
						OR AD.ActiveDutyId IS NULL
					)
				)
				OR
				(
					BP.LR_ITR <= X 
					AND BP.LC_ACT_DUT = 'X'
					AND BP.LC_INT_RDC_PGM = 'M' --Handles adding a new USCRA if the end date is unknown and the borrower has had the benefit for almost a year
					AND DATEDIFF(DAY,GETDATE(),BP.LD_ITR_EFF_END) <= XX
					AND AD.BeginDate = BP.LD_ACT_DUT_BEG
					AND
					(
						AD.EndDate = BP.LD_ACT_DUT_END
						OR AD.EndDate = 'XXXX-XX-XX'
					)
				)
			)

	/********************************************************************
						TRANSACTION HANDLING
	********************************************************************/
	SELECT @ERROR = @@ERROR


	--Endorser USCRA/MSCRA
	INSERT INTO @RX
		SELECT DISTINCT
			X AS ArcTypeId,
			NULL AS ArcResponseCodeId,
			EP.DF_SPE_ACC_ID AS AccountNumber,
			NULL AS RecipientId,
			CASE WHEN	EP.LC_NTF_ACT_DUT = 'Y' --Notified of active duty
						AND EP.LC_INT_RDC_PGM = 'M' 
						AND EP.LR_ITR <= X
						AND 
						(
							AD.BeginDate != EP.LD_EID_BEGIN_DATE 
							OR AD.EndDate != 'XXXX-XX-XX'
						) 
					THEN 'USCRA'
				WHEN	EP.LC_INT_RDC_PGM = 'M' --Either the begin or end date has changed
						AND EP.LR_ITR <= X 
						AND 
						(
							AD.BeginDate != EP.LD_ACT_DUT_BEG 
							OR AD.EndDate != EP.LD_ACT_DUT_END
						)
					THEN 'USCRA'
				WHEN	EP.LC_INT_RDC_PGM = 'M' --No Entry in the ActiveDuty table
						AND EP.LR_ITR <= X 
						AND AD.ActiveDutyId IS NULL 
					THEN 'USCRA'
				WHEN	EP.LC_INT_RDC_PGM = 'M' --Handles adding a new USCRA if the end date is unknown and the borrower has had the benefit for almost a year
						AND EP.LR_ITR <= X 
						AND DATEDIFF(DAY,GETDATE(),EP.LD_ITR_EFF_END) <= XX
						AND AD.BeginDate = EP.LD_ACT_DUT_BEG
						AND 
						(
							AD.EndDate = EP.LD_ACT_DUT_END
							OR AD.EndDate = 'XXXX-XX-XX'
						)
					THEN 'USCRA'
				WHEN LNXX.BF_SSN IS NOT NULL
					THEN 'USCRA'
					ELSE 'MSCRA'
			END AS Arc,
			'UTLWSXX' AS ScriptId,
			GETDATE() AS ProcessOn,
			'' AS Comment,
			X AS IsReference,
			X AS IsEndorser,
			NULL AS ProcessFrom,
			NULL AS ProcessTo,
			NULL AS NeededBy,
			EP.LF_EDS AS RegardsTo,
			'E' AS RegardsCode,
			GETDATE() AS CreatedAt,
			'UTLWSXX' AS CreatedBy,
			NULL AS ProcessedAt
		FROM
			ULS.scra._EndorsersPopulation EP
			LEFT JOIN ULS.scra.Borrowers MIL
				ON MIL.BorrowerAccountNumber = EP.DF_SPE_ACC_ID
			LEFT JOIN ULS.scra.ActiveDuty AD
				ON AD.BorrowerId = MIL.BorrowerId
			LEFT JOIN UDW..PDXX_PRS_NME PDXX
				ON PDXX.DF_SPE_ACC_ID = EP.DF_SPE_ACC_ID
			LEFT JOIN 
			(
				SELECT DISTINCT
					BF_SSN
				FROM
					UDW..LNXX_INT_RTE_HST
				WHERE
					LC_INT_RDC_PGM = 'M'
					AND LC_STA_LONXX = 'A'
			) LNXX
				ON LNXX.BF_SSN = PDXX.DF_PRS_ID
		WHERE
			NOT 
			(
				(
					EP.LD_LON_X_DSB > EP.LD_ACT_DUT_BEG 
					AND EP.LC_NTF_ACT_DUT != 'Y'
				) 
				OR
				(
					EP.LD_LON_X_DSB > ISNULL(EP.LD_EID_BEGIN_DATE,'XXXX-XX-XX') 
					AND EP.LC_NTF_ACT_DUT = 'Y'
				)
			)
			AND NOT 
			(
				EP.LC_ACT_DUT = 'N'
				AND EP.LC_LFT_ACT_DUT = 'N'
				AND EP.LC_NTF_ACT_DUT = 'N'
			)
			AND
			(
				(
					EP.LR_ITR > X 
					AND EP.LC_ACT_DUT = 'X'
				)
				OR 
				(
					EP.LC_ACT_DUT = 'N' 
					AND EP.LC_NTF_ACT_DUT = 'Y' 
					AND EP.LR_ITR > X
				)
				OR 
				(
					GETDATE() BETWEEN EP.LD_ITR_EFF_BEG AND EP.LD_ITR_EFF_END 
					AND EP.LC_INT_RDC_PGM = 'M' 
					AND EP.LR_ITR <= X
					AND EP.LC_NTF_ACT_DUT = 'Y'
					AND 
					(
						AD.BeginDate != EP.LD_EID_BEGIN_DATE 
						OR AD.EndDate != 'XXXX-XX-XX' 
						OR AD.ActiveDutyId IS NULL
					)
				)
				OR
				(
					GETDATE() BETWEEN EP.LD_ITR_EFF_BEG AND EP.LD_ITR_EFF_END 
					AND EP.LC_INT_RDC_PGM = 'M' 
					AND EP.LR_ITR <= X
					AND EP.LC_NTF_ACT_DUT != 'Y'
					AND 
					(
						AD.BeginDate != EP.LD_ACT_DUT_BEG 
						OR AD.EndDate != EP.LD_ACT_DUT_END 
						OR AD.ActiveDutyId IS NULL
					)
				)
				OR
				(
					EP.LR_ITR <= X 
					AND EP.LC_ACT_DUT = 'X'
					AND EP.LC_INT_RDC_PGM = 'M' --Handles adding a new USCRA if the end date is unknown and the borrower has had the benefit for almost a year
					AND DATEDIFF(DAY,GETDATE(),EP.LD_ITR_EFF_END) <= XX
					AND AD.BeginDate = EP.LD_ACT_DUT_BEG
					AND
					(
						AD.EndDate = EP.LD_ACT_DUT_END
						OR AD.EndDate = 'XXXX-XX-XX'
					)
				)
			)


	/********************************************************************
						TRANSACTION HANDLING
	********************************************************************/
	SELECT @ERROR = @ERROR + @@ERROR


	--Borrower ASCRA
	INSERT INTO @RX
		SELECT DISTINCT
			X AS ArcTypeId,
			NULL AS ArcResponseCodeId,
			BP.DF_SPE_ACC_ID AS AccountNumber,
			NULL AS RecipientId,
			'ASCRA' AS Arc,
			'UTLWSXX' AS ScriptId,
			GETDATE() AS ProcessOn,
			CASE WHEN BP.LC_ACT_DUT = 'N' AND BP.LC_NTF_ACT_DUT = 'Y' 
					THEN 
						CASE WHEN	AD.ActiveDutyId IS NULL --TODO FORMAT THIS BETTER
								THEN 'Borrower is Active duty. Active Duty Begin date = ' + CAST(CAST(BP.LD_EID_BEGIN_DATE AS DATE) AS VARCHAR) + ', Active Duty End Date = ' + CAST(CAST(ISNULL(BP.LD_ACT_DUT_END,'XXXX-XX-XX') AS DATE) AS VARCHAR)
								ELSE 'Borrower Military Dates updated. Active Duty Begin date = ' + CAST(CAST(BP.LD_EID_BEGIN_DATE AS DATE) AS VARCHAR) + ', Active Duty End Date = ' + CAST(CAST(ISNULL(BP.LD_ACT_DUT_END,'XXXX-XX-XX') AS DATE) AS VARCHAR)
						END
					ELSE 
						CASE WHEN	AD.ActiveDutyId IS NULL 
								THEN 'Borrower is Active duty. Active Duty Begin date = ' + CAST(CAST(BP.LD_ACT_DUT_BEG AS DATE) AS VARCHAR) + ', Active Duty End Date = ' + CAST(CAST(ISNULL(BP.LD_ACT_DUT_END,'XXXX-XX-XX') AS DATE) AS VARCHAR)
								ELSE 'Borrower Military Dates updated. Active Duty Begin date = ' + CAST(CAST(BP.LD_ACT_DUT_BEG AS DATE) AS VARCHAR) + ', Active Duty End Date = ' + CAST(CAST(ISNULL(BP.LD_ACT_DUT_END,'XXXX-XX-XX') AS DATE) AS VARCHAR)
						END
			END AS Comment,
			X AS IsReference,
			X AS IsEndorser,
			NULL AS ProcessFrom,
			NULL AS ProcessTo,
			NULL AS NeededBy,
			NULL AS RegardsTo,
			NULL AS RegardsCode,
			GETDATE() AS CreatedAt,
			'UTLWSXX' AS CreatedBy,
			NULL AS ProcessedAt
			FROM
				ULS.scra._BorrowersPopulation BP 
				LEFT JOIN ULS.scra.Borrowers MIL
					ON MIL.BorrowerAccountNumber = BP.DF_SPE_ACC_ID
				LEFT JOIN ULS.scra.ActiveDuty AD
					ON AD.BorrowerId = MIL.BorrowerId
			WHERE
				NOT 
				(
					BP.LC_ACT_DUT ='X' 
					AND AD.BeginDate = BP.LD_ACT_DUT_BEG
					AND AD.EndDate = BP.LD_ACT_DUT_END 
					AND AD.Active = X
				) 
				AND NOT 
				(
					BP.LC_ACT_DUT = 'N' 
					AND BP.LC_LFT_ACT_DUT = 'Y' 
					AND BP.LC_NTF_ACT_DUT = 'N' 
					AND AD.BeginDate = BP.LD_ACT_DUT_BEG 
					AND AD.EndDate = BP.LD_ACT_DUT_END 
					AND AD.Active = X
				)
				AND NOT 
				(
					BP.LC_ACT_DUT = 'N' 
					AND BP.LC_NTF_ACT_DUT = 'Y' 
					AND AD.BeginDate = BP.LD_EID_BEGIN_DATE 
					AND AD.EndDate = 'XXXX-XX-XX' AND AD.Active = X
				)
				AND NOT 
				(
					BP.LC_ACT_DUT = 'N'
					AND BP.LC_LFT_ACT_DUT = 'N'
					AND BP.LC_NTF_ACT_DUT = 'N'
				)
				OR 
				(
					AD.ActiveDutyId IS NULL
					AND NOT 
					(
						BP.LC_ACT_DUT = 'N'
						AND BP.LC_LFT_ACT_DUT = 'N'
						AND BP.LC_NTF_ACT_DUT = 'N'
					)
				)


	/********************************************************************
						TRANSACTION HANDLING
	********************************************************************/
	SELECT @ERROR = @ERROR + @@ERROR


	--Endorser ASCRA
	INSERT INTO @RX
		SELECT DISTINCT
			X AS ArcTypeId,
			NULL AS ArcResponseCodeId,
			EP.DF_SPE_ACC_ID AS AccountNumber,
			NULL AS RecipientId,
			'ASCRA' AS Arc,
			'UTLWSXX' AS ScriptId,
			GETDATE() AS ProcessOn,
			CASE WHEN EP.LC_ACT_DUT = 'N' AND EP.LC_NTF_ACT_DUT = 'Y'
					THEN 
						CASE WHEN	AD.ActiveDutyId IS NULL
								THEN 'Endorser/Co-borrower is Active duty. Active Duty Begin date = ' + CAST(CAST(EP.LD_EID_BEGIN_DATE AS DATE) AS VARCHAR) + ', Active Duty End Date = ' + CAST(CAST(ISNULL(EP.LD_ACT_DUT_END,'XXXX-XX-XX') AS DATE) AS VARCHAR)
								ELSE 'Endorser/Co-borrower Military Dates updated. Active Duty Begin date = ' + CAST(CAST(EP.LD_EID_BEGIN_DATE AS DATE) AS VARCHAR) + ', Active Duty End Date = ' + CAST(CAST(ISNULL(EP.LD_ACT_DUT_END,'XXXX-XX-XX') AS DATE) AS VARCHAR)
						END
					ELSE 
						CASE WHEN	AD.ActiveDutyId IS NULL
								THEN 'Endorser/Co-borrower is Active duty. Active Duty Begin date = ' + CAST(CAST(EP.LD_ACT_DUT_BEG AS DATE) AS VARCHAR) + ', Active Duty End Date = ' + CAST(CAST(ISNULL(EP.LD_ACT_DUT_END,'XXXX-XX-XX') AS DATE) AS VARCHAR)
								ELSE 'Endorser/Co-borrower Military Dates updated. Active Duty Begin date = ' + CAST(CAST(EP.LD_ACT_DUT_BEG AS DATE) AS VARCHAR) + ', Active Duty End Date = ' + CAST(CAST(ISNULL(EP.LD_ACT_DUT_END,'XXXX-XX-XX') AS DATE) AS VARCHAR)
						END	
			END AS Comment,
			X AS IsReference,
			X AS IsEndorser,
			NULL AS ProcessFrom,
			NULL AS ProcessTo,
			NULL AS NeededBy, 
			EP.LF_EDS AS RegardsTo,
			'E' AS RegardsCode,
			GETDATE() AS CreatedAt,
			'UTLWSXX' AS CreatedBy,
			NULL AS ProcessedAt
		FROM
			ULS.scra._EndorsersPopulation EP
			LEFT JOIN ULS.scra.Borrowers MIL
				ON MIL.BorrowerAccountNumber = EP.DF_SPE_ACC_ID
			LEFT JOIN ULS.scra.ActiveDuty AD
				ON AD.BorrowerId = MIL.BorrowerId
			WHERE
				NOT 
				(
					EP.LC_ACT_DUT ='X' 
					AND AD.BeginDate = EP.LD_ACT_DUT_BEG 
					AND AD.EndDate = EP.LD_ACT_DUT_END 
					AND AD.Active = X
				) 
				AND NOT 
				(
					EP.LC_ACT_DUT = 'N' 
					AND EP.LC_LFT_ACT_DUT = 'Y' 
					AND EP.LC_NTF_ACT_DUT = 'N'  
					AND AD.BeginDate = EP.LD_ACT_DUT_BEG 
					AND AD.EndDate = EP.LD_ACT_DUT_END 
					AND AD.Active = X
				)
				AND NOT 
				(
					EP.LC_ACT_DUT = 'N' 
					AND EP.LC_NTF_ACT_DUT = 'Y' 
					AND AD.BeginDate = EP.LD_EID_BEGIN_DATE 
					AND AD.EndDate = 'XXXX-XX-XX' 
					AND AD.Active = X
				)
				AND NOT 
				(
					EP.LC_ACT_DUT = 'N'
					AND EP.LC_LFT_ACT_DUT = 'N'
					AND EP.LC_NTF_ACT_DUT = 'N'
				)
				OR 
				(
					AD.ActiveDutyId IS NULL
					AND NOT 
					(
						EP.LC_ACT_DUT = 'N'
						AND EP.LC_LFT_ACT_DUT = 'N'
						AND EP.LC_NTF_ACT_DUT = 'N'
					)
				)


	/********************************************************************
						TRANSACTION HANDLING
	********************************************************************/
	SELECT @ERROR = @ERROR + @@ERROR


	--Borrower no longer active duty
	INSERT INTO @RX
		SELECT DISTINCT
			X AS ArcTypeId,
			NULL AS ArcResponseCodeId,
			BP.DF_SPE_ACC_ID AS AccountNumber,
			NULL AS RecipientId,
			'ISCRA' AS Arc,
			'UTLWSXX' AS ScriptId,
			GETDATE() AS ProcessOn,
			'Active duty status ended' AS Comment,
			X AS IsReference,
			X AS IsEndorser,
			NULL AS ProcessFrom,
			NULL AS ProcessTo,
			NULL AS NeededBy,
			NULL AS RegardsTo,
			NULL AS RegardsCode,
			GETDATE() AS CreatedAt,
			'UTLWSXX' AS CreatedBy,
			NULL AS ProcessedAt
		FROM
			ULS.scra._BorrowersPopulation BP
			LEFT JOIN ULS.scra.Borrowers MIL
				ON MIL.BorrowerAccountNumber = BP.DF_SPE_ACC_ID
			LEFT JOIN ULS.scra.ActiveDuty AD
				ON AD.BorrowerId = MIL.BorrowerId
		WHERE
			BP.LC_ACT_DUT = 'N'
			AND BP.LC_NTF_ACT_DUT != 'Y'
			AND BP.LC_LFT_ACT_DUT != 'Y'
			AND AD.Active = X


	/********************************************************************
						TRANSACTION HANDLING
	********************************************************************/
	SELECT @ERROR = @ERROR + @@ERROR


	--Endorser no longer active duty
	INSERT INTO @RX
		SELECT DISTINCT
			X AS ArcTypeId,
			NULL AS ArcResponseCodeId,
			EP.DF_SPE_ACC_ID AS AccountNumber,
			NULL AS RecipientId,
			'ISCRA' AS Arc,
			'UTLWSXX' AS ScriptId,
			GETDATE() AS ProcessOn,
			'Endorser active duty status ended' AS Comment, 
			X AS IsReference,
			X AS IsEndorser,
			NULL AS ProcessFrom,
			NULL AS ProcessTo,
			NULL AS NeededBy,
			EP.LF_EDS AS RegardsTo,
			'E' AS RegardsCode,
			GETDATE() AS CreatedAt,
			'UTLWSXX' AS CreatedBy,
			NULL AS ProcessedAt
		FROM
			ULS.scra._EndorsersPopulation EP
			LEFT JOIN ULS.scra.Borrowers MIL
				ON MIL.BorrowerAccountNumber = EP.DF_SPE_ACC_ID
			LEFT JOIN ULS.scra.ActiveDuty AD
				ON AD.BorrowerId = MIL.BorrowerId
		WHERE
			EP.LC_ACT_DUT = 'N'
			AND EP.LC_NTF_ACT_DUT != 'Y'
			AND EP.LC_LFT_ACT_DUT != 'Y'
			AND AD.Active = X


	/********************************************************************
						TRANSACTION HANDLING
	********************************************************************/
	SELECT @ERROR = @ERROR + @@ERROR


	--INSERT (NEW BORROWER/ENDORSERS)
	INSERT INTO ULS.scra.Borrowers(BorrowerAccountNumber,EndorserAccountNumber)
	(
		SELECT DISTINCT
			EP.DF_SPE_ACC_ID,
			EP.END_ACC_ID
		FROM 
			ULS.scra._EndorsersPopulation EP
			LEFT JOIN
			(
				SELECT
					BorrowerAccountNumber,
					EndorserAccountNumber
				FROM
					ULS.scra.Borrowers
			) EX
				ON EX.BorrowerAccountNumber = EP.DF_SPE_ACC_ID
		WHERE
			EX.EndorserAccountNumber IS NULL 
			AND EX.BorrowerAccountNumber IS NULL
	)


	/********************************************************************
						TRANSACTION HANDLING
	********************************************************************/
	SELECT @ERROR = @ERROR + @@ERROR


	--INSERT (NEW BORROWERS)
	INSERT INTO ULS.scra.Borrowers(BorrowerAccountNumber,EndorserAccountNumber)
	(
		SELECT DISTINCT
			BP.DF_SPE_ACC_ID,
			NULL
		FROM 
			ULS.scra._BorrowersPopulation BP
			LEFT JOIN
			(
				SELECT 
					BorrowerAccountNumber
				FROM
					ULS.scra.Borrowers
			) BX
				ON BX.BorrowerAccountNumber = BP.DF_SPE_ACC_ID
		WHERE
			BX.BorrowerAccountNumber IS NULL
	)


	/********************************************************************
						TRANSACTION HANDLING
	********************************************************************/
	SELECT @ERROR = @ERROR + @@ERROR


	--UPDATE (NEW ENDORSER INFO FOR AN EXISTING BORROWER)
	UPDATE
		BOR 
	SET
		BOR.EndorserAccountNumber = EP.END_ACC_ID
	FROM
		ULS.scra.Borrowers BOR
		INNER JOIN ULS.scra._EndorsersPopulation EP
			ON EP.DF_SPE_ACC_ID = BOR.BorrowerAccountNumber
			AND EP.END_ACC_ID IS NOT NULL
	WHERE 
		BOR.EndorserAccountNumber IS NULL


	/********************************************************************
						TRANSACTION HANDLING
	********************************************************************/
	SELECT @ERROR = @ERROR + @@ERROR


	/*DELETE FROM RX where ENDORSER is still active even though borrower is not
	We dont want to arc the account as no longer active if only the borrower is now inactive*/
	DELETE 
		RX
	FROM 
		@RX RX 
		INNER JOIN ULS.scra.Borrowers BOR 
			ON BOR.BorrowerAccountNumber = RX.AccountNumber
		INNER JOIN ULS.scra._EndorsersPopulation EP
			ON EP.DF_SPE_ACC_ID = BOR.BorrowerAccountNumber
	WHERE
		BOR.EndorserAccountNumber IS NOT NULL
		AND 
		(
			EP.LC_ACT_DUT = 'X'
			OR 
			(
				EP.LC_ACT_DUT = 'N'
				AND EP.LC_NTF_ACT_DUT = 'Y'
			)
			OR
			(
				EP.LC_ACT_DUT = 'N'
				AND EP.LC_LFT_ACT_DUT = 'Y'
			)
		)


	/********************************************************************
						TRANSACTION HANDLING
	********************************************************************/
	SELECT @ERROR = @ERROR + @@ERROR


	/*DELETE FROM RX where BORROWER is still active even though endorser is not
	We dont want to arc the account as no longer active if only the endorser is now inactive*/
	DELETE 
		RX
	FROM 
		@RX RX 
		INNER JOIN ULS.scra.Borrowers BOR 
			ON BOR.BorrowerAccountNumber = RX.AccountNumber
		INNER JOIN ULS.scra._BorrowersPopulation BP
			ON BP.DF_SPE_ACC_ID = BOR.BorrowerAccountNumber
	WHERE
		(
			BP.LC_ACT_DUT = 'X'
			OR 
			(
				BP.LC_ACT_DUT = 'N'
				AND BP.LC_NTF_ACT_DUT = 'Y'
			)
			OR 
			(
				BP.LC_ACT_DUT = 'N'
				AND BP.LC_LFT_ACT_DUT = 'Y'
			)
		)


	/********************************************************************
						TRANSACTION HANDLING
	********************************************************************/
	SELECT @ERROR = @ERROR + @@ERROR


	--CREATE ACTIVE DUTY RECORDS
	INSERT INTO	ULS.scra.ActiveDuty	(BorrowerId,BeginDate,EndDate,IsBorrower,CreatedAt,Active,BenefitSourceId,NotificationDate,IsReservist)
	(
		--NO ENDORSER SO NO DATE MANIPULATION
		SELECT DISTINCT
			BOR.BorrowerId,
			CASE WHEN	DODREC.LC_ACT_DUT = 'N' AND DODREC.LC_NTF_ACT_DUT = 'Y'
					THEN DODREC.LD_EID_BEGIN_DATE
					ELSE DODREC.LD_ACT_DUT_BEG 
			END,
			CASE WHEN	DODREC.LC_ACT_DUT = 'N' AND DODREC.LC_NTF_ACT_DUT = 'Y'
					THEN 'XXXX-XX-XX'
					ELSE ISNULL(DODREC.LD_ACT_DUT_END,'XXXX-XX-XX')
			END,
			X,
			GETDATE(),
			X,
			X, --borrower
			DODREC.LD_NOTIFICATION_DATE, --notificationData
			CASE WHEN DODREC.LC_NTF_ACT_DUT = 'Y' --Is reservist indicator
					THEN X 
					ELSE X 
			END
		FROM
			ULS.scra._BorrowersPopulation DODREC
			INNER JOIN ULS.scra.Borrowers BOR
				ON DODREC.DF_SPE_ACC_ID = BOR.BorrowerAccountNumber
			LEFT JOIN 
			(
				SELECT
					BorrowerId, 
					ActiveDutyId
				FROM
					ULS.scra.ActiveDuty
			) AD 
				ON AD.BorrowerId = BOR.BorrowerId
		WHERE
			AD.ActiveDutyId IS NULL 
			AND BOR.EndorserAccountNumber IS NULL
			AND 
			(
				DODREC.LD_ACT_DUT_BEG IS NOT NULL 
				OR DODREC.LD_EID_BEGIN_DATE IS NOT NULL
			)
		
		UNION ALL

		--ENDORSERS GROUP NEED TO CARE ABOUT BOTH BORROWER AND ENDORSER DATES IF BORROWER IS ALSO MILITARY
		SELECT DISTINCT
			EN.BorrowerId,
			CASE WHEN	BP.DF_SPE_ACC_ID IS NOT NULL
						AND BP.LC_ACT_DUT = 'N'
						AND BP.LC_NTF_ACT_DUT = 'N'
						AND BP.LC_LFT_ACT_DUT = 'N'
					THEN 
						CASE WHEN	ENREC.LC_ACT_DUT = 'N' 
									AND ENREC.LC_NTF_ACT_DUT = 'Y'
								THEN ENREC.LD_EID_BEGIN_DATE
								ELSE ENREC.LD_ACT_DUT_BEG
						END
				 WHEN	BP.DF_SPE_ACC_ID IS NOT NULL
						AND ENREC.LC_ACT_DUT = 'N'
						AND ENREC.LC_NTF_ACT_DUT = 'N'
						AND ENREC.LC_LFT_ACT_DUT = 'N'
					THEN 
						CASE WHEN	BP.LC_ACT_DUT = 'N' 
									AND BP.LC_NTF_ACT_DUT = 'Y'
								THEN BP.LD_EID_BEGIN_DATE
								ELSE BP.LD_ACT_DUT_BEG
						END
				 WHEN	BP.DF_SPE_ACC_ID IS NOT NULL
						AND BP.LC_ACT_DUT = 'X'
						AND ENREC.LC_ACT_DUT = 'X'
					THEN 
						CASE WHEN	BP.LD_ACT_DUT_BEG <= ENREC.LD_ACT_DUT_BEG 
									AND BP.LD_ACT_DUT_BEG >= BP.LD_LON_X_DSB
								THEN BP.LD_ACT_DUT_BEG
							 WHEN	ENREC.LD_ACT_DUT_BEG <= BP.LD_ACT_DUT_BEG
									AND ENREC.LD_ACT_DUT_BEG >= ENREC.LD_LON_X_DSB
								THEN ENREC.LD_ACT_DUT_BEG
							 WHEN	BP.LD_ACT_DUT_BEG <= ENREC.LD_ACT_DUT_BEG 
									AND BP.LD_ACT_DUT_BEG < BP.LD_LON_X_DSB
								THEN 
									CASE WHEN	ENREC.LD_ACT_DUT_BEG > ENREC.LD_LON_X_DSB
											THEN ENREC.LD_ACT_DUT_BEG
											ELSE BP.LD_ACT_DUT_BEG
									END
							 WHEN	ENREC.LD_ACT_DUT_BEG <= BP.LD_ACT_DUT_BEG 
									AND ENREC.LD_ACT_DUT_BEG < ENREC.LD_LON_X_DSB
								THEN 
									CASE WHEN	BP.LD_ACT_DUT_BEG > BP.LD_LON_X_DSB
											THEN BP.LD_ACT_DUT_BEG
											ELSE ENREC.LD_ACT_DUT_BEG
									END
								ELSE BP.LD_ACT_DUT_BEG
						END
				 WHEN	BP.DF_SPE_ACC_ID IS NOT NULL
						AND BP.LC_ACT_DUT = 'X'
						AND ENREC.LC_ACT_DUT = 'N' 
						AND ENREC.LC_NTF_ACT_DUT = 'Y'
					THEN 
						CASE WHEN	BP.LD_ACT_DUT_BEG <= ENREC.LD_EID_BEGIN_DATE 
									AND BP.LD_ACT_DUT_BEG >= BP.LD_LON_X_DSB
								THEN BP.LD_ACT_DUT_BEG
							 WHEN	ENREC.LD_EID_BEGIN_DATE <= BP.LD_ACT_DUT_BEG 
									AND ENREC.LD_EID_BEGIN_DATE >= ENREC.LD_LON_X_DSB
								THEN ENREC.LD_EID_BEGIN_DATE
							 WHEN	BP.LD_ACT_DUT_BEG <= ENREC.LD_EID_BEGIN_DATE 
									AND BP.LD_ACT_DUT_BEG < BP.LD_LON_X_DSB
								THEN 
									CASE WHEN	ENREC.LD_EID_BEGIN_DATE > ENREC.LD_LON_X_DSB
											THEN ENREC.LD_EID_BEGIN_DATE
											ELSE BP.LD_ACT_DUT_BEG
									END
							 WHEN	ENREC.LD_EID_BEGIN_DATE <= BP.LD_ACT_DUT_BEG 
									AND ENREC.LD_EID_BEGIN_DATE < ENREC.LD_LON_X_DSB
								THEN 
									CASE WHEN	BP.LD_ACT_DUT_BEG > BP.LD_LON_X_DSB
											THEN BP.LD_ACT_DUT_BEG
											ELSE ENREC.LD_EID_BEGIN_DATE
									END
								ELSE BP.LD_ACT_DUT_BEG
						END
				 WHEN	BP.DF_SPE_ACC_ID IS NOT NULL
						AND BP.LC_ACT_DUT = 'N'
						AND BP.LC_NTF_ACT_DUT = 'Y'
						AND ENREC.LC_ACT_DUT = 'X' 
					THEN 
						CASE WHEN	BP.LD_EID_BEGIN_DATE <= ENREC.LD_ACT_DUT_BEG 
									AND BP.LD_EID_BEGIN_DATE >= BP.LD_LON_X_DSB
								THEN BP.LD_EID_BEGIN_DATE
							 WHEN	ENREC.LD_ACT_DUT_BEG <= BP.LD_EID_BEGIN_DATE 
									AND ENREC.LD_ACT_DUT_BEG >= ENREC.LD_LON_X_DSB
								THEN ENREC.LD_ACT_DUT_BEG
							 WHEN	BP.LD_EID_BEGIN_DATE <= ENREC.LD_ACT_DUT_BEG 
									AND BP.LD_EID_BEGIN_DATE < BP.LD_LON_X_DSB
								THEN 
									CASE WHEN	ENREC.LD_ACT_DUT_BEG > ENREC.LD_LON_X_DSB
											THEN ENREC.LD_ACT_DUT_BEG
											ELSE BP.LD_EID_BEGIN_DATE
									END
							 WHEN	ENREC.LD_ACT_DUT_BEG <= BP.LD_EID_BEGIN_DATE 
									AND ENREC.LD_ACT_DUT_BEG < ENREC.LD_LON_X_DSB
								THEN
									CASE WHEN	BP.LD_EID_BEGIN_DATE > BP.LD_LON_X_DSB
											THEN BP.LD_EID_BEGIN_DATE
											ELSE ENREC.LD_ACT_DUT_BEG
									END
								ELSE BP.LD_EID_BEGIN_DATE
						END
				 WHEN	BP.DF_SPE_ACC_ID IS NOT NULL 
						AND BP.LC_ACT_DUT = 'N' 
						AND BP.LC_NTF_ACT_DUT = 'Y' 
						AND ENREC.LC_ACT_DUT = 'N' 
						AND ENREC.LC_NTF_ACT_DUT = 'Y'
					THEN 
						CASE WHEN	BP.LD_EID_BEGIN_DATE <= ENREC.LD_EID_BEGIN_DATE 
									AND BP.LD_EID_BEGIN_DATE >= BP.LD_LON_X_DSB
								THEN BP.LD_EID_BEGIN_DATE
							 WHEN	ENREC.LD_EID_BEGIN_DATE <= BP.LD_EID_BEGIN_DATE 
									AND ENREC.LD_EID_BEGIN_DATE >= ENREC.LD_LON_X_DSB
								THEN ENREC.LD_EID_BEGIN_DATE
							 WHEN	BP.LD_EID_BEGIN_DATE <= ENREC.LD_EID_BEGIN_DATE 
									AND BP.LD_EID_BEGIN_DATE < BP.LD_LON_X_DSB
								THEN 
									CASE WHEN	ENREC.LD_EID_BEGIN_DATE > ENREC.LD_LON_X_DSB
											THEN ENREC.LD_EID_BEGIN_DATE
											ELSE BP.LD_EID_BEGIN_DATE
									END
							 WHEN	ENREC.LD_EID_BEGIN_DATE <= BP.LD_EID_BEGIN_DATE 
									AND ENREC.LD_EID_BEGIN_DATE < ENREC.LD_LON_X_DSB
								THEN 
									CASE WHEN	BP.LD_EID_BEGIN_DATE > BP.LD_LON_X_DSB
											THEN BP.LD_EID_BEGIN_DATE
											ELSE ENREC.LD_EID_BEGIN_DATE
									END
								ELSE BP.LD_EID_BEGIN_DATE
						END
				 WHEN	BP.DF_SPE_ACC_ID IS NOT NULL 
						AND BP.LC_ACT_DUT = 'N' 
						AND BP.LC_NTF_ACT_DUT = 'Y' 
						AND NOT
						(
							ENREC.LC_ACT_DUT = 'N' 
							AND ENREC.LC_NTF_ACT_DUT = 'Y'
						)
					THEN 
						CASE WHEN	BP.LD_EID_BEGIN_DATE <= ENREC.LD_ACT_DUT_BEG 
									AND BP.LD_EID_BEGIN_DATE >= BP.LD_LON_X_DSB
								THEN BP.LD_EID_BEGIN_DATE
							 WHEN	ENREC.LD_ACT_DUT_BEG <= BP.LD_EID_BEGIN_DATE 
									AND ENREC.LD_ACT_DUT_BEG >= ENREC.LD_LON_X_DSB
								THEN ENREC.LD_ACT_DUT_BEG
							 WHEN	BP.LD_EID_BEGIN_DATE <= ENREC.LD_ACT_DUT_BEG 
									AND BP.LD_EID_BEGIN_DATE < BP.LD_LON_X_DSB
								THEN 
									CASE WHEN	ENREC.LD_ACT_DUT_BEG > ENREC.LD_LON_X_DSB
											THEN ENREC.LD_ACT_DUT_BEG
											ELSE BP.LD_EID_BEGIN_DATE
									END
							 WHEN	ENREC.LD_ACT_DUT_BEG <= BP.LD_EID_BEGIN_DATE 
									AND ENREC.LD_ACT_DUT_BEG < ENREC.LD_LON_X_DSB
								THEN 
									CASE WHEN	BP.LD_EID_BEGIN_DATE > BP.LD_LON_X_DSB
											THEN BP.LD_EID_BEGIN_DATE
											ELSE ENREC.LD_ACT_DUT_BEG
									END
								ELSE BP.LD_EID_BEGIN_DATE
						END
				 WHEN	BP.DF_SPE_ACC_ID IS NOT NULL 
						AND NOT
						(
							BP.LC_ACT_DUT = 'N' 
							AND BP.LC_NTF_ACT_DUT = 'Y'
						)
						AND ENREC.LC_ACT_DUT = 'N' 
						AND ENREC.LC_NTF_ACT_DUT = 'Y'
					THEN 
						CASE WHEN	BP.LD_ACT_DUT_BEG <= ENREC.LD_EID_BEGIN_DATE 
									AND BP.LD_ACT_DUT_BEG >= BP.LD_LON_X_DSB
								THEN BP.LD_ACT_DUT_BEG
							 WHEN	ENREC.LD_EID_BEGIN_DATE <= BP.LD_ACT_DUT_BEG 
									AND ENREC.LD_EID_BEGIN_DATE >= ENREC.LD_LON_X_DSB
								THEN ENREC.LD_EID_BEGIN_DATE
							 WHEN	BP.LD_ACT_DUT_BEG <= ENREC.LD_EID_BEGIN_DATE 
									AND BP.LD_ACT_DUT_BEG < BP.LD_LON_X_DSB
								THEN 
									CASE WHEN	ENREC.LD_EID_BEGIN_DATE > ENREC.LD_LON_X_DSB
											THEN ENREC.LD_EID_BEGIN_DATE
											ELSE BP.LD_ACT_DUT_BEG
									END
							 WHEN	ENREC.LD_EID_BEGIN_DATE <= BP.LD_ACT_DUT_BEG 
									AND ENREC.LD_EID_BEGIN_DATE < ENREC.LD_LON_X_DSB
								THEN 
									CASE WHEN	BP.LD_ACT_DUT_BEG > BP.LD_LON_X_DSB
											THEN BP.LD_ACT_DUT_BEG
											ELSE ENREC.LD_EID_BEGIN_DATE
									END
								ELSE BP.LD_ACT_DUT_BEG
						END 
				 WHEN	BP.DF_SPE_ACC_ID IS NOT NULL 
						AND NOT
						(
							BP.LC_ACT_DUT = 'N' 
							AND BP.LC_NTF_ACT_DUT = 'Y'
						) 
						AND NOT
						(
							ENREC.LC_ACT_DUT = 'N' 
							AND ENREC.LC_NTF_ACT_DUT = 'Y'
						)
					THEN 
						CASE WHEN	BP.LD_ACT_DUT_BEG <= ENREC.LD_ACT_DUT_BEG 
									AND BP.LD_ACT_DUT_BEG >= BP.LD_LON_X_DSB
								THEN BP.LD_ACT_DUT_BEG
							 WHEN	ENREC.LD_ACT_DUT_BEG <= BP.LD_ACT_DUT_BEG
									AND ENREC.LD_ACT_DUT_BEG >= ENREC.LD_LON_X_DSB
								THEN ENREC.LD_ACT_DUT_BEG
							 WHEN	BP.LD_ACT_DUT_BEG <= ENREC.LD_ACT_DUT_BEG 
									AND BP.LD_ACT_DUT_BEG < BP.LD_LON_X_DSB
								THEN 
									CASE WHEN	ENREC.LD_ACT_DUT_BEG > ENREC.LD_LON_X_DSB
											THEN ENREC.LD_ACT_DUT_BEG
											ELSE BP.LD_ACT_DUT_BEG
									END
							 WHEN	ENREC.LD_ACT_DUT_BEG <= BP.LD_ACT_DUT_BEG 
									AND ENREC.LD_ACT_DUT_BEG < ENREC.LD_LON_X_DSB
								THEN 
									CASE WHEN	BP.LD_ACT_DUT_BEG > BP.LD_LON_X_DSB
											THEN BP.LD_ACT_DUT_BEG
											ELSE ENREC.LD_ACT_DUT_BEG
									END
								ELSE BP.LD_ACT_DUT_BEG
						 END
				 WHEN	BP.DF_SPE_ACC_ID IS NULL 
						AND ENREC.LC_ACT_DUT = 'N' 
						AND ENREC.LC_NTF_ACT_DUT = 'Y'
					THEN ENREC.LD_EID_BEGIN_DATE
				 WHEN	BP.DF_SPE_ACC_ID IS NULL 
						AND NOT
						(
							ENREC.LC_ACT_DUT = 'N' 
							AND ENREC.LC_NTF_ACT_DUT = 'Y'
						)
					THEN ENREC.LD_ACT_DUT_BEG
					ELSE BP.LD_ACT_DUT_BEG
			END,
			CASE WHEN	BP.DF_SPE_ACC_ID IS NOT NULL 
						AND BP.LC_ACT_DUT = 'N'
						AND BP.LC_NTF_ACT_DUT = 'N'
						AND BP.LC_LFT_ACT_DUT = 'N'
					THEN ISNULL(ENREC.LD_ACT_DUT_END,'XXXX-XX-XX')
				 WHEN	BP.DF_SPE_ACC_ID IS NOT NULL 
						AND ENREC.LC_ACT_DUT = 'N'
						AND ENREC.LC_NTF_ACT_DUT = 'N'
						AND ENREC.LC_LFT_ACT_DUT = 'N'
					THEN ISNULL(BP.LD_ACT_DUT_END,'XXXX-XX-XX')
				 WHEN	BP.DF_SPE_ACC_ID IS NOT NULL 
						AND ISNULL(BP.LD_ACT_DUT_END,'XXXX-XX-XX') <= ISNULL(ENREC.LD_ACT_DUT_END,'XXXX-XX-XX')
					THEN ISNULL(ENREC.LD_ACT_DUT_END,'XXXX-XX-XX')
				 WHEN	BP.DF_SPE_ACC_ID IS NOT NULL 
						AND ISNULL(BP.LD_ACT_DUT_END,'XXXX-XX-XX') > ISNULL(ENREC.LD_ACT_DUT_END,'XXXX-XX-XX')
					THEN ISNULL(BP.LD_ACT_DUT_END,'XXXX-XX-XX')
					ELSE ISNULL(ENREC.LD_ACT_DUT_END,'XXXX-XX-XX')
			END,
			X,
			GETDATE(),
			X,
			CASE WHEN	BP.DF_SPE_ACC_ID IS NULL
					THEN X
					ELSE X 
			END, --endorser or both
			CASE WHEN	BP.DF_SPE_ACC_ID IS NULL --take endorser date if borrower is not military
					THEN ENREC.LD_NOTIFICATION_DATE
				 WHEN	BP.DF_SPE_ACC_ID IS NOT NULL --take the later of the X dates if both are military
					THEN 
						CASE WHEN	ENREC.LD_NOTIFICATION_DATE >= BP.LD_NOTIFICATION_DATE
								THEN ENREC.LD_NOTIFICATION_DATE
								ELSE BP.LD_NOTIFICATION_DATE
						END
					ELSE BP.LD_NOTIFICATION_DATE
			END,
			CASE WHEN	ENREC.LC_NTF_ACT_DUT = 'Y' AND BP.DF_SPE_ACC_ID IS NULL --Is reservist indicator
					THEN X
				 WHEN	ENREC.LC_NTF_ACT_DUT = 'Y' AND BP.LC_NTF_ACT_DUT = 'Y' AND BP.DF_SPE_ACC_ID IS NOT NULL --Is reservist indicator
					THEN X
					ELSE X 
			END
		FROM
			ULS.scra._EndorsersPopulation ENREC
			INNER JOIN ULS.scra.Borrowers EN
				ON ENREC.DF_SPE_ACC_ID = EN.BorrowerAccountNumber
			LEFT JOIN 
			(
				SELECT
					BorrowerId,
					ActiveDutyId
				FROM
					ULS.scra.ActiveDuty
			) AD 
				ON AD.BorrowerId = EN.BorrowerId
			LEFT JOIN
			(
				SELECT
					DF_SPE_ACC_ID,
					LD_ACT_DUT_BEG,
					LD_ACT_DUT_END,
					LC_ACT_DUT,
					LC_NTF_ACT_DUT,
					LC_LFT_ACT_DUT,
					LD_LON_X_DSB,
					LD_EID_BEGIN_DATE,
					LD_NOTIFICATION_DATE
				FROM
					ULS.scra._BorrowersPopulation
			) BP
				ON BP.DF_SPE_ACC_ID = ENREC.DF_SPE_ACC_ID --The endorser record is endorsing a borrower with his or her own record already
		WHERE
			AD.ActiveDutyId IS NULL 
			AND 
			(
				ENREC.LD_ACT_DUT_BEG IS NOT NULL 
				OR ENREC.LD_EID_BEGIN_DATE IS NOT NULL
				OR BP.LD_ACT_DUT_BEG IS NOT NULL
				OR BP.LD_EID_BEGIN_DATE IS NOT NULL
			)
		)


	/********************************************************************
						TRANSACTION HANDLING
	********************************************************************/
	SELECT @ERROR = @ERROR + @@ERROR


	--Borrower only update
	UPDATE
		AD
	SET
		AD.BeginDate= 
		(
			CASE WHEN	BP.LC_ACT_DUT = 'X'
					THEN BP.LD_ACT_DUT_BEG
				 WHEN	BP.LC_NTF_ACT_DUT = 'Y'
					THEN BP.LD_EID_BEGIN_DATE
				 WHEN	BP.LC_LFT_ACT_DUT = 'Y'
						AND BP.LC_NTF_ACT_DUT = 'N'
					THEN BP.LD_ACT_DUT_BEG
				 WHEN	BP.LC_ACT_DUT = 'N'
						AND BP.LC_LFT_ACT_DUT = 'N'
						AND BP.LC_NTF_ACT_DUT = 'N'
						AND COALESCE(BP.LD_ACT_DUT_BEG,BP.LD_EID_BEGIN_DATE) IS NULL
					THEN AD.BeginDate
					ELSE AD.BeginDate							
			END
		),
		AD.EndDate= 
		(
			CASE WHEN	BP.LC_ACT_DUT = 'N'
						AND BP.LC_LFT_ACT_DUT = 'Y'
						AND BP.LC_NTF_ACT_DUT = 'Y' 
					THEN 'XXXX-XX-XX'
				 WHEN	BP.LC_ACT_DUT = 'N'
						AND BP.LC_LFT_ACT_DUT = 'N'
						AND BP.LC_NTF_ACT_DUT = 'N'
						AND BP.LD_ACT_DUT_END IS NULL
					THEN AD.EndDate
					ELSE ISNULL(BP.LD_ACT_DUT_END,'XXXX-XX-XX')
			END
		),
		AD.IsBorrower= X,
		AD.Active = 
		(
			CASE WHEN	RX.AccountNumber IS NOT NULL
					THEN X
				 WHEN	RX.AccountNumber IS NOT NULL
					THEN X
					ELSE AD.Active
			END
		),
		AD.BenefitSourceId = X,
		AD.NotificationDate = BP.LD_NOTIFICATION_DATE,
		AD.IsReservist = 
		(
			CASE WHEN BP.LC_NTF_ACT_DUT = 'Y' --Is reservist indicator
					THEN X 
					ELSE X 
			END
		)
	FROM 
		ULS.scra.ActiveDuty AD
		INNER JOIN ULS.scra.Borrowers BOR
			ON BOR.BorrowerId = AD.BorrowerId
		LEFT OUTER JOIN ULS.scra._BorrowersPopulation BP
			ON BP.DF_SPE_ACC_ID = BOR.BorrowerAccountNumber
		LEFT OUTER JOIN ULS.scra._EndorsersPopulation ENREC
			ON ENREC.DF_SPE_ACC_ID = BOR.BorrowerAccountNumber
		LEFT OUTER JOIN @RX RX 
			ON RX.AccountNumber = BOR.BorrowerAccountNumber
		LEFT OUTER JOIN @RX RX 
			ON RX.AccountNumber = BOR.BorrowerAccountNumber
		LEFT OUTER JOIN @RX RX 
			ON RX.AccountNumber = BOR.BorrowerAccountNumber
		LEFT OUTER JOIN @RX RX 
			ON RX.AccountNumber = BOR.BorrowerAccountNumber
	WHERE
		ENREC.DF_SPE_ACC_ID IS NULL 
		AND BP.DF_SPE_ACC_ID IS NOT NULL


	/********************************************************************
						TRANSACTION HANDLING
	********************************************************************/
	SELECT @ERROR = @ERROR + @@ERROR


	--endorser exists
	UPDATE
		AD
	SET 
		AD.BeginDate= 
		(
			CASE WHEN	BP.DF_SPE_ACC_ID IS NOT NULL
						AND BP.LC_ACT_DUT = 'N'
						AND BP.LC_NTF_ACT_DUT = 'N'
						AND BP.LC_LFT_ACT_DUT = 'N'
					THEN 
						CASE WHEN	ENREC.LC_ACT_DUT = 'N' AND ENREC.LC_NTF_ACT_DUT = 'Y'
								THEN ENREC.LD_EID_BEGIN_DATE
								ELSE COALESCE(ENREC.LD_ACT_DUT_BEG, AD.BeginDate)
						END
				 WHEN	BP.DF_SPE_ACC_ID IS NOT NULL
						AND ENREC.LC_ACT_DUT = 'N'
						AND ENREC.LC_NTF_ACT_DUT = 'N'
						AND ENREC.LC_LFT_ACT_DUT = 'N'
					THEN 
						CASE WHEN	BP.LC_ACT_DUT = 'N' AND BP.LC_NTF_ACT_DUT = 'Y'
								THEN BP.LD_EID_BEGIN_DATE
								ELSE COALESCE(BP.LD_ACT_DUT_BEG, AD.BeginDate)
						END
							/* ADDED TO FED */
				 WHEN	BP.DF_SPE_ACC_ID IS NULL
						AND ENREC.LC_ACT_DUT = 'N'
						AND ENREC.LC_NTF_ACT_DUT = 'N'
						AND ENREC.LC_LFT_ACT_DUT = 'N'
					THEN AD.BeginDate
				 WHEN	BP.DF_SPE_ACC_ID IS NOT NULL
						AND BP.LC_ACT_DUT = 'X'
						AND ENREC.LC_ACT_DUT = 'X'
					THEN 
						CASE WHEN	BP.LD_ACT_DUT_BEG <= ENREC.LD_ACT_DUT_BEG 
									AND BP.LD_ACT_DUT_BEG >= BP.LD_LON_X_DSB
								THEN BP.LD_ACT_DUT_BEG
							 WHEN	ENREC.LD_ACT_DUT_BEG <= BP.LD_ACT_DUT_BEG
									AND ENREC.LD_ACT_DUT_BEG >= ENREC.LD_LON_X_DSB
								THEN ENREC.LD_ACT_DUT_BEG
							 WHEN	BP.LD_ACT_DUT_BEG <= ENREC.LD_ACT_DUT_BEG 
									AND BP.LD_ACT_DUT_BEG < BP.LD_LON_X_DSB
								THEN 
									CASE WHEN	ENREC.LD_ACT_DUT_BEG > ENREC.LD_LON_X_DSB
											THEN ENREC.LD_ACT_DUT_BEG
											ELSE BP.LD_ACT_DUT_BEG
									END
							 WHEN	ENREC.LD_ACT_DUT_BEG <= BP.LD_ACT_DUT_BEG 
									AND ENREC.LD_ACT_DUT_BEG < ENREC.LD_LON_X_DSB
								THEN 
									CASE WHEN	BP.LD_ACT_DUT_BEG > BP.LD_LON_X_DSB
											THEN BP.LD_ACT_DUT_BEG
											ELSE ENREC.LD_ACT_DUT_BEG
									END
								ELSE BP.LD_ACT_DUT_BEG
						END
				 WHEN	BP.DF_SPE_ACC_ID IS NOT NULL
						AND BP.LC_ACT_DUT = 'X'
						AND ENREC.LC_ACT_DUT = 'N' 
						AND ENREC.LC_NTF_ACT_DUT = 'Y'
					THEN 
						CASE WHEN	BP.LD_ACT_DUT_BEG <= ENREC.LD_EID_BEGIN_DATE 
									AND BP.LD_ACT_DUT_BEG >= BP.LD_LON_X_DSB
								THEN BP.LD_ACT_DUT_BEG
							 WHEN	ENREC.LD_EID_BEGIN_DATE <= BP.LD_ACT_DUT_BEG 
									AND ENREC.LD_EID_BEGIN_DATE >= ENREC.LD_LON_X_DSB
								THEN ENREC.LD_EID_BEGIN_DATE
							 WHEN	BP.LD_ACT_DUT_BEG <= ENREC.LD_EID_BEGIN_DATE 
									AND BP.LD_ACT_DUT_BEG < BP.LD_LON_X_DSB
								THEN 
									CASE WHEN	ENREC.LD_EID_BEGIN_DATE > ENREC.LD_LON_X_DSB
											THEN ENREC.LD_EID_BEGIN_DATE
											ELSE BP.LD_ACT_DUT_BEG
									END
							 WHEN	ENREC.LD_EID_BEGIN_DATE <= BP.LD_ACT_DUT_BEG 
									AND ENREC.LD_EID_BEGIN_DATE < ENREC.LD_LON_X_DSB
								THEN 
									CASE WHEN	BP.LD_ACT_DUT_BEG > BP.LD_LON_X_DSB
											THEN BP.LD_ACT_DUT_BEG
											ELSE ENREC.LD_EID_BEGIN_DATE
									END
								ELSE BP.LD_ACT_DUT_BEG	
						END
				 WHEN	BP.DF_SPE_ACC_ID IS NOT NULL
						AND BP.LC_ACT_DUT = 'N'
						AND BP.LC_NTF_ACT_DUT = 'Y'
						AND ENREC.LC_ACT_DUT = 'X' 
					THEN 
						CASE WHEN	BP.LD_EID_BEGIN_DATE <= ENREC.LD_ACT_DUT_BEG 
									AND BP.LD_EID_BEGIN_DATE >= BP.LD_LON_X_DSB
								THEN BP.LD_EID_BEGIN_DATE
							 WHEN	ENREC.LD_ACT_DUT_BEG <= BP.LD_EID_BEGIN_DATE 
									AND ENREC.LD_ACT_DUT_BEG >= ENREC.LD_LON_X_DSB
								THEN ENREC.LD_ACT_DUT_BEG
							 WHEN	BP.LD_EID_BEGIN_DATE <= ENREC.LD_ACT_DUT_BEG 
									AND BP.LD_EID_BEGIN_DATE < BP.LD_LON_X_DSB
								THEN
									CASE WHEN	ENREC.LD_ACT_DUT_BEG > ENREC.LD_LON_X_DSB
											THEN ENREC.LD_ACT_DUT_BEG
											ELSE BP.LD_EID_BEGIN_DATE
									END
							 WHEN	ENREC.LD_ACT_DUT_BEG <= BP.LD_EID_BEGIN_DATE 
									AND ENREC.LD_ACT_DUT_BEG < ENREC.LD_LON_X_DSB
								THEN 
									CASE WHEN	BP.LD_EID_BEGIN_DATE > BP.LD_LON_X_DSB
											THEN BP.LD_EID_BEGIN_DATE
											ELSE ENREC.LD_ACT_DUT_BEG
									END
								ELSE BP.LD_EID_BEGIN_DATE
						END
				 WHEN	BP.DF_SPE_ACC_ID IS NOT NULL 
						AND BP.LC_ACT_DUT = 'N' 
						AND BP.LC_NTF_ACT_DUT = 'Y' 
						AND ENREC.LC_ACT_DUT = 'N' 
						AND ENREC.LC_NTF_ACT_DUT = 'Y'
					THEN 
						CASE WHEN	BP.LD_EID_BEGIN_DATE <= ENREC.LD_EID_BEGIN_DATE 
									AND BP.LD_EID_BEGIN_DATE >= BP.LD_LON_X_DSB
								THEN BP.LD_EID_BEGIN_DATE
							 WHEN	ENREC.LD_EID_BEGIN_DATE <= BP.LD_EID_BEGIN_DATE 
									AND ENREC.LD_EID_BEGIN_DATE >= ENREC.LD_LON_X_DSB
								THEN ENREC.LD_EID_BEGIN_DATE
							 WHEN	BP.LD_EID_BEGIN_DATE <= ENREC.LD_EID_BEGIN_DATE 
									AND BP.LD_EID_BEGIN_DATE < BP.LD_LON_X_DSB
								THEN 
									CASE WHEN	ENREC.LD_EID_BEGIN_DATE > ENREC.LD_LON_X_DSB
											THEN ENREC.LD_EID_BEGIN_DATE
											ELSE BP.LD_EID_BEGIN_DATE
									END
							 WHEN	ENREC.LD_EID_BEGIN_DATE <= BP.LD_EID_BEGIN_DATE 
									AND ENREC.LD_EID_BEGIN_DATE < ENREC.LD_LON_X_DSB
								THEN 
									CASE WHEN	BP.LD_EID_BEGIN_DATE > BP.LD_LON_X_DSB
											THEN BP.LD_EID_BEGIN_DATE
											ELSE ENREC.LD_EID_BEGIN_DATE
									END
								ELSE BP.LD_EID_BEGIN_DATE
						END
				 WHEN	BP.DF_SPE_ACC_ID IS NOT NULL 
						AND BP.LC_ACT_DUT = 'N' 
						AND BP.LC_NTF_ACT_DUT = 'Y' 
						AND NOT
						(
							ENREC.LC_ACT_DUT = 'N' 
							AND ENREC.LC_NTF_ACT_DUT = 'Y'
						)
					THEN 
						CASE WHEN	BP.LD_EID_BEGIN_DATE <= ENREC.LD_ACT_DUT_BEG 
									AND BP.LD_EID_BEGIN_DATE >= BP.LD_LON_X_DSB
								THEN BP.LD_EID_BEGIN_DATE
							 WHEN	ENREC.LD_ACT_DUT_BEG <= BP.LD_EID_BEGIN_DATE 
									AND ENREC.LD_ACT_DUT_BEG >= ENREC.LD_LON_X_DSB
								THEN ENREC.LD_ACT_DUT_BEG
							 WHEN	BP.LD_EID_BEGIN_DATE <= ENREC.LD_ACT_DUT_BEG 
									AND BP.LD_EID_BEGIN_DATE < BP.LD_LON_X_DSB
								THEN 
									CASE WHEN	ENREC.LD_ACT_DUT_BEG > ENREC.LD_LON_X_DSB
											THEN ENREC.LD_ACT_DUT_BEG
											ELSE BP.LD_EID_BEGIN_DATE
									END
							 WHEN	ENREC.LD_ACT_DUT_BEG <= BP.LD_EID_BEGIN_DATE 
									AND ENREC.LD_ACT_DUT_BEG < ENREC.LD_LON_X_DSB
								THEN 
									CASE WHEN	BP.LD_EID_BEGIN_DATE > BP.LD_LON_X_DSB
											THEN BP.LD_EID_BEGIN_DATE
											ELSE ENREC.LD_ACT_DUT_BEG
									END
								ELSE BP.LD_EID_BEGIN_DATE
						END
				 WHEN	BP.DF_SPE_ACC_ID IS NOT NULL 
						AND ENREC.LC_ACT_DUT = 'N' 
						AND ENREC.LC_NTF_ACT_DUT = 'Y'
						AND NOT
						(
							BP.LC_ACT_DUT = 'N' 
							AND BP.LC_NTF_ACT_DUT = 'Y'
						)
					THEN 
						CASE WHEN	BP.LD_ACT_DUT_BEG <= ENREC.LD_EID_BEGIN_DATE 
									AND BP.LD_ACT_DUT_BEG >= BP.LD_LON_X_DSB
								THEN BP.LD_ACT_DUT_BEG
							 WHEN	ENREC.LD_EID_BEGIN_DATE <= BP.LD_ACT_DUT_BEG 
									AND ENREC.LD_EID_BEGIN_DATE >= ENREC.LD_LON_X_DSB
								THEN ENREC.LD_EID_BEGIN_DATE
							 WHEN	BP.LD_ACT_DUT_BEG <= ENREC.LD_EID_BEGIN_DATE 
									AND BP.LD_ACT_DUT_BEG < BP.LD_LON_X_DSB
								THEN 
									CASE WHEN	ENREC.LD_EID_BEGIN_DATE > ENREC.LD_LON_X_DSB
											THEN ENREC.LD_EID_BEGIN_DATE
											ELSE BP.LD_ACT_DUT_BEG
									END
							 WHEN	ENREC.LD_EID_BEGIN_DATE <= BP.LD_ACT_DUT_BEG 
									AND ENREC.LD_EID_BEGIN_DATE < ENREC.LD_LON_X_DSB
								THEN 
									CASE WHEN	BP.LD_ACT_DUT_BEG > BP.LD_LON_X_DSB
											THEN BP.LD_ACT_DUT_BEG
											ELSE ENREC.LD_EID_BEGIN_DATE
									END
								ELSE BP.LD_ACT_DUT_BEG
						END
				 WHEN	BP.DF_SPE_ACC_ID IS NOT NULL 
						AND NOT
						(
							BP.LC_ACT_DUT = 'N' 
							AND BP.LC_NTF_ACT_DUT = 'Y'
						) 
						AND NOT
						(
							ENREC.LC_ACT_DUT = 'N' 
							AND ENREC.LC_NTF_ACT_DUT = 'Y'
						)
					THEN 
						CASE WHEN	BP.LD_ACT_DUT_BEG <= ENREC.LD_ACT_DUT_BEG 
									AND BP.LD_ACT_DUT_BEG >= BP.LD_LON_X_DSB
								THEN BP.LD_ACT_DUT_BEG
							 WHEN	ENREC.LD_ACT_DUT_BEG <= BP.LD_ACT_DUT_BEG
									AND ENREC.LD_ACT_DUT_BEG >= ENREC.LD_LON_X_DSB
								THEN ENREC.LD_ACT_DUT_BEG
							 WHEN	BP.LD_ACT_DUT_BEG <= ENREC.LD_ACT_DUT_BEG 
									AND BP.LD_ACT_DUT_BEG < BP.LD_LON_X_DSB
								THEN 
									CASE WHEN	ENREC.LD_ACT_DUT_BEG > ENREC.LD_LON_X_DSB
											THEN ENREC.LD_ACT_DUT_BEG
											ELSE BP.LD_ACT_DUT_BEG
									END
							 WHEN	ENREC.LD_ACT_DUT_BEG <= BP.LD_ACT_DUT_BEG 
									AND ENREC.LD_ACT_DUT_BEG < ENREC.LD_LON_X_DSB
								THEN 
									CASE WHEN	BP.LD_ACT_DUT_BEG > BP.LD_LON_X_DSB
											THEN BP.LD_ACT_DUT_BEG
											ELSE ENREC.LD_ACT_DUT_BEG
									END
								ELSE BP.LD_ACT_DUT_BEG
						END
				 WHEN	BP.DF_SPE_ACC_ID IS NULL 
						AND ENREC.LC_ACT_DUT = 'N' 
						AND ENREC.LC_NTF_ACT_DUT = 'Y'
					THEN ENREC.LD_EID_BEGIN_DATE
				 WHEN	BP.DF_SPE_ACC_ID IS NULL 
						AND NOT
						(
							ENREC.LC_ACT_DUT = 'N' 
							AND ENREC.LC_NTF_ACT_DUT = 'Y'
						)
					THEN ENREC.LD_ACT_DUT_BEG
					ELSE BP.LD_ACT_DUT_BEG
			END
		),
		AD.EndDate= 
		(
			CASE WHEN	BP.DF_SPE_ACC_ID IS NOT NULL
						AND BP.LC_ACT_DUT = 'N'
						AND BP.LC_LFT_ACT_DUT = 'N'
						AND BP.LC_NTF_ACT_DUT = 'N' 
						AND ENREC.LC_ACT_DUT = 'N'
						AND ENREC.LC_LFT_ACT_DUT = 'N'
						AND ENREC.LC_NTF_ACT_DUT = 'N'
					THEN AD.EndDate
				 WHEN	BP.DF_SPE_ACC_ID IS NOT NULL
						AND BP.LC_ACT_DUT = 'N'
						AND BP.LC_LFT_ACT_DUT = 'N'
						AND BP.LC_NTF_ACT_DUT = 'N' 
					THEN ISNULL(ENREC.LD_ACT_DUT_END,'XXXX-XX-XX')
				 WHEN	BP.DF_SPE_ACC_ID IS NOT NULL
						AND ENREC.LC_ACT_DUT = 'N'
						AND ENREC.LC_LFT_ACT_DUT = 'N'
						AND ENREC.LC_NTF_ACT_DUT = 'N'
					THEN ISNULL(BP.LD_ACT_DUT_END,'XXXX-XX-XX')
				 WHEN	BP.DF_SPE_ACC_ID IS NOT NULL 
						AND ISNULL(BP.LD_ACT_DUT_END,'XXXX-XX-XX') <= ISNULL(ENREC.LD_ACT_DUT_END,'XXXX-XX-XX') 
					THEN ISNULL(ENREC.LD_ACT_DUT_END,'XXXX-XX-XX')
				 WHEN	BP.DF_SPE_ACC_ID IS NOT NULL 
						AND ISNULL(BP.LD_ACT_DUT_END,'XXXX-XX-XX') > ISNULL(ENREC.LD_ACT_DUT_END,'XXXX-XX-XX')
					THEN ISNULL(BP.LD_ACT_DUT_END,'XXXX-XX-XX')
					ELSE ISNULL(ENREC.LD_ACT_DUT_END,'XXXX-XX-XX')
			END
		),
		AD.IsBorrower= 
		(
			CASE WHEN	ENREC.DF_SPE_ACC_ID IS NOT NULL 
					THEN X 
					ELSE X 
			END
		),
		AD.Active= 
		(
			CASE WHEN	RX.AccountNumber IS NOT NULL 
						OR RX.AccountNumber IS NOT NULL
					THEN X
				 WHEN	RX.AccountNumber IS NOT NULL 
						OR RX.AccountNumber IS NOT NULL
					THEN X
					ELSE AD.Active
			END
		),
		AD.BenefitSourceId = 
		(
			CASE WHEN	BP.DF_SPE_ACC_ID IS NULL
					THEN X
				/* ADDED TO FED */
				 WHEN	ENREC.DF_SPE_ACC_ID IS NOT NULL
						AND ENREC.LC_ACT_DUT = 'N'
						AND ENREC.LC_LFT_ACT_DUT = 'N'
						AND ENREC.LC_NTF_ACT_DUT = 'N' 
					THEN X
					ELSE X 
			END
		), --endorser or both
		AD.NotificationDate = 
		(
			CASE WHEN	BP.DF_SPE_ACC_ID IS NULL --take endorser date if borrower is not military
					THEN ENREC.LD_NOTIFICATION_DATE
				 WHEN	BP.DF_SPE_ACC_ID IS NOT NULL --take the later of the X dates if both are military
					THEN 
						CASE WHEN	 ENREC.LD_NOTIFICATION_DATE >= BP.LD_NOTIFICATION_DATE
								THEN ENREC.LD_NOTIFICATION_DATE
								ELSE BP.LD_NOTIFICATION_DATE
						END
					ELSE BP.LD_NOTIFICATION_DATE
			END
		),--notificationDate
		AD.IsReservist = 
		(
			CASE WHEN	ENREC.LC_NTF_ACT_DUT = 'Y' 
						AND BP.DF_SPE_ACC_ID IS NULL --Is reservist indicator
					THEN X
				 WHEN	ENREC.LC_NTF_ACT_DUT = 'Y' 
						AND BP.LC_NTF_ACT_DUT = 'Y' 
						AND BP.DF_SPE_ACC_ID IS NOT NULL --Is reservist indicator
					THEN X
					ELSE X 
			END
		)
	FROM 
		ULS.scra.ActiveDuty AD
		INNER JOIN ULS.scra.Borrowers BOR
			ON BOR.BorrowerId = AD.BorrowerId
		LEFT OUTER JOIN ULS.scra._BorrowersPopulation BP
			ON BP.DF_SPE_ACC_ID = BOR.BorrowerAccountNumber
		LEFT OUTER JOIN ULS.scra._EndorsersPopulation ENREC
			ON ENREC.DF_SPE_ACC_ID = BOR.BorrowerAccountNumber
		LEFT OUTER JOIN @RX RX 
			ON RX.AccountNumber = BOR.BorrowerAccountNumber
		LEFT OUTER JOIN @RX RX 
			ON RX.AccountNumber = BOR.BorrowerAccountNumber
		LEFT OUTER JOIN @RX RX 
			ON RX.AccountNumber = BOR.BorrowerAccountNumber
		LEFT OUTER JOIN @RX RX 
			ON RX.AccountNumber = BOR.BorrowerAccountNumber
	WHERE	
		ENREC.DF_SPE_ACC_ID IS NOT NULL


	/********************************************************************
						TRANSACTION HANDLING
	********************************************************************/
	SELECT @ERROR = @ERROR + @@ERROR


	--ADD ARCS TO ARC ADD
	INSERT INTO	ULS.dbo.ARCADDPROCESSING(ARCTYPEID,ACCOUNTNUMBER,RECIPIENTID,ARC,SCRIPTID,PROCESSON,COMMENT,ISREFERENCE,ISENDORSER,PROCESSFROM,PROCESSTO,NEEDEDBY,REGARDSTO,REGARDSCODE,CREATEDAT,CREATEDBY)
	(
		SELECT
			ArcTypeId,
			AccountNumber,
			RecipientId,
			Arc,
			ScriptId,
			GETDATE(), 
			Comment,
			IsReference,
			IsEndorser,
			ProcessFrom,
			ProcessTo,
			NeededBy,
			RegardsTo,
			RegardsCode,
			GETDATE(),
			CreatedBy
		FROM
			@RX

		UNION ALL

		SELECT
			ArcTypeId,
			AccountNumber,
			RecipientId,
			Arc,
			ScriptId,
			GETDATE(), 
			Comment,
			IsReference,
			IsEndorser,
			ProcessFrom,
			ProcessTo,
			NeededBy,
			RegardsTo,
			RegardsCode,
			GETDATE(),
			CreatedBy
		FROM
			@RX

		UNION ALL

		SELECT
			ArcTypeId,
			AccountNumber,
			RecipientId,
			Arc,
			ScriptId,
			GETDATE(), 
			Comment,
			IsReference,
			IsEndorser,
			ProcessFrom,
			ProcessTo,
			NeededBy,
			RegardsTo,
			RegardsCode,
			GETDATE(),
			CreatedBy
		FROM
			@RX

		UNION ALL

		SELECT
			ArcTypeId,
			AccountNumber,
			RecipientId,
			Arc,
			ScriptId,
			GETDATE(), 
			Comment,
			IsReference,
			IsEndorser,
			ProcessFrom,
			ProcessTo,
			NeededBy,
			RegardsTo,
			RegardsCode,
			GETDATE(),
			CreatedBy
		FROM
			@RX

		UNION ALL

		SELECT
			ArcTypeId,
			AccountNumber,
			RecipientId,
			Arc,
			ScriptId,
			GETDATE(), 
			Comment,
			IsReference,
			IsEndorser,
			ProcessFrom,
			ProcessTo,
			NeededBy,
			RegardsTo,
			RegardsCode,
			GETDATE(),
			CreatedBy
		FROM
			@RX

		UNION ALL

		SELECT
			ArcTypeId,
			AccountNumber,
			RecipientId,
			Arc,
			ScriptId,
			GETDATE(), 
			Comment,
			IsReference,
			IsEndorser,
			ProcessFrom,
			ProcessTo,
			NeededBy,
			RegardsTo,
			RegardsCode,
			GETDATE(),
			CreatedBy
		FROM
			@RX
	)


	/********************************************************************
						TRANSACTION HANDLING
	********************************************************************/
	SELECT @ERROR = @ERROR + @@ERROR


	DROP TABLE ULS.scra._BorrowersPopulation


	/********************************************************************
						TRANSACTION HANDLING
	********************************************************************/
	SELECT @ERROR = @ERROR + @@ERROR


	DROP TABLE ULS.scra._EndorsersPopulation


	/********************************************************************
						TRANSACTION HANDLING
	********************************************************************/
	SELECT @ERROR = @ERROR + @@ERROR


	IF @ERROR = X
		BEGIN
			PRINT 'Transaction committed'
			COMMIT TRANSACTION
			--ROLLBACK TRANSACTION
		END
	ELSE
		BEGIN
			PRINT 'ERROR:  ' + CAST(@ERROR as VARCHAR(XX))
			PRINT 'Transaction NOT committed'
			ROLLBACK TRANSACTION
		END
	END
END



