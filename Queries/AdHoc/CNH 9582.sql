/*CNH XXXX*/
/*first execute lines X-X, then execute from XX-end*/
USE [CLS]
GO

DROP PROCEDURE [dbo].[PopulateScraTables];
GO

/*start Xnd execution here*/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[PopulateScraTables]
AS
BEGIN
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
--Borrower USCRA/MSCRA
INSERT INTO @RX
	SELECT DISTINCT
		X AS ArcTypeId,
		NULL AS ArcResponseCodeId,
		BP.DF_SPE_ACC_ID AS AccountNumber,
		NULL AS RecipientId,
		CASE
			--Notified of active duty
			WHEN	BP.LC_NTF_ACT_DUT = 'Y' 
					AND BP.LC_INT_RDC_PGM = 'M'
					AND BP.LR_ITR <= X 
					AND (
							AD.BeginDate != BP.LD_EID_BEGIN_DATE 
							OR AD.EndDate != 'XXXX-XX-XX'
						) 
					THEN 'USCRA'
			--Either the begin or end date has changed
			WHEN	BP.LC_INT_RDC_PGM = 'M' 
					AND BP.LR_ITR <= X 
					AND (
							AD.BeginDate != BP.LD_ACT_DUT_BEG 
							OR AD.EndDate != BP.LD_ACT_DUT_END
						) 
					THEN 'USCRA'
			--No Entry in the ActiveDuty table
			WHEN	BP.LC_INT_RDC_PGM = 'M' 
					AND BP.LR_ITR <= X 
					AND AD.ActiveDutyId IS NULL
					THEN 'USCRA'
			--Handles adding a new USCRA if the end date is unknown and the borrower has had the benefit for almost a year
			WHEN	BP.LC_INT_RDC_PGM = 'M' 
					AND BP.LR_ITR <= X 
					AND DATEDIFF(DAY,GETDATE(),BP.LD_ITR_EFF_END) <= XX
					AND AD.BeginDate = BP.LD_ACT_DUT_BEG
					AND AD.EndDate = BP.LD_ACT_DUT_END
					AND AD.EndDate = 'XXXX-XX-XX'
					THEN 'USCRA'
					ELSE 'MSCRA'
		END AS Arc,
		'SCRAINTUPD' AS ScriptId,
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
		'SCRAFED' AS CreatedBy,
		NULL AS ProcessedAt
	FROM
		CLS.scra._BorrowersPopulation BP
		LEFT JOIN CLS.scra.Borrowers MIL
				ON MIL.BorrowerAccountNumber = BP.DF_SPE_ACC_ID
			LEFT JOIN CLS.scra.ActiveDuty AD
				ON AD.BorrowerId = MIL.BorrowerId
	WHERE
		NOT (
				(   --exclude loans disbursed after active duty
					BP.LD_LON_X_DSB > BP.LD_ACT_DUT_BEG 
					AND BP.LC_NTF_ACT_DUT != 'Y'
				) 
				OR (    --exclude reservists where loans disburse after active duty
						BP.LD_LON_X_DSB > ISNULL(BP.LD_EID_BEGIN_DATE,'XXXX-XX-XX') 
						AND BP.LC_NTF_ACT_DUT = 'Y'
					)
			)
		AND NOT (       --non active duty borrowers
						BP.LC_ACT_DUT = 'N'
						AND BP.LC_LFT_ACT_DUT = 'N'
						AND BP.LC_NTF_ACT_DUT = 'N'
					)
		AND (
				(   --active duty not receiving interest benefit
					BP.LR_ITR > X 
					AND BP.LC_ACT_DUT = 'X'
				)
			OR (    --reservist not receiving interest benefit
					BP.LC_ACT_DUT = 'N' 
					AND BP.LC_NTF_ACT_DUT = 'Y' 
					AND BP.LR_ITR > X
				)
			OR (    --reservist receiving interest benefit
					GETDATE() BETWEEN BP.LD_ITR_EFF_BEG AND BP.LD_ITR_EFF_END 
					AND BP.LC_INT_RDC_PGM = 'M' 
					AND BP.LR_ITR <= X
					AND BP.LC_NTF_ACT_DUT = 'Y'
					AND (AD.BeginDate != BP.LD_EID_BEGIN_DATE OR AD.EndDate != 'XXXX-XX-XX' OR AD.ActiveDutyId IS NULL)
				)
			OR (   --non reservist receiving interest benefit
					GETDATE() BETWEEN BP.LD_ITR_EFF_BEG AND BP.LD_ITR_EFF_END 
					AND BP.LC_INT_RDC_PGM = 'M' 
					AND BP.LR_ITR <= X
					AND BP.LC_NTF_ACT_DUT != 'Y'
					AND (AD.BeginDate != BP.LD_ACT_DUT_BEG OR AD.EndDate != BP.LD_ACT_DUT_END OR AD.ActiveDutyId IS NULL)
				)
			)

--Borrower NSCRA 
INSERT INTO @RX
	SELECT DISTINCT
		X AS ArcTypeId,
		NULL AS ArcResponseCodeId,
		BP.DF_SPE_ACC_ID AS AccountNumber,
		NULL AS RecipientId,
		'NSCRA' AS Arc, 
		'SCRAINTUPD' AS ScriptId,
		GETDATE() AS ProcessOn,
		CASE
		    --Notified of active duty
			WHEN	BP.LC_NTF_ACT_DUT = 'Y' 
					AND BP.LC_INT_RDC_PGM = 'M'
					AND BP.LR_ITR <= X 
					AND (
							AD.BeginDate != BP.LD_EID_BEGIN_DATE 
							OR AD.EndDate != 'XXXX-XX-XX'
						) 
					THEN 'Update Record'
		    --Either the begin or end date has changed
			WHEN	BP.LC_INT_RDC_PGM = 'M' 
					AND BP.LR_ITR <= X 
					AND (
							AD.BeginDate != BP.LD_ACT_DUT_BEG 
							OR AD.EndDate != BP.LD_ACT_DUT_END
						) 
					THEN 'Update Record'
			--No Entry in the ActiveDuty table
			WHEN	BP.LC_INT_RDC_PGM = 'M' 
					AND BP.LR_ITR <= X 
					AND AD.ActiveDutyId IS NULL 
					THEN 'Update Record'
			--Handles adding a new USCRA if the end date is unknown and the borrower has had the benefit for almost a year
			WHEN	BP.LC_INT_RDC_PGM = 'M' 
					AND BP.LR_ITR <= X 
					AND DATEDIFF(DAY, GETDATE(), BP.LD_ITR_EFF_END) <= XX
					AND AD.BeginDate = BP.LD_ACT_DUT_BEG
					AND AD.EndDate = BP.LD_ACT_DUT_END
					AND AD.EndDate = 'XXXX-XX-XX'
					THEN 'Update Record'
					ELSE 'New Record'
		END AS Comment,
		X AS IsReference,
		X AS IsEndorser,
		NULL AS ProcessFrom,
		NULL AS ProcessTo,
		NULL AS NeededBy,
		NULL AS RegardsTo,
		NULL AS RegardsCode,
		GETDATE() AS CreatedAt,
		'SCRAFED' AS CreatedBy,
		NULL AS ProcessedAt
	FROM
		CLS.scra._BorrowersPopulation BP
		LEFT JOIN CLS.scra.Borrowers MIL
				ON MIL.BorrowerAccountNumber = BP.DF_SPE_ACC_ID
			LEFT JOIN CLS.scra.ActiveDuty AD
				ON AD.BorrowerId = MIL.BorrowerId
	WHERE
		NOT (
				(
					BP.LD_LON_X_DSB > BP.LD_ACT_DUT_BEG 
					AND BP.LC_NTF_ACT_DUT != 'Y'
				) 
				OR (
						BP.LD_LON_X_DSB > ISNULL(BP.LD_EID_BEGIN_DATE,'XXXX-XX-XX') 
						AND BP.LC_NTF_ACT_DUT = 'Y'
					)
			)
		AND NOT (
						BP.LC_ACT_DUT = 'N'
						AND BP.LC_LFT_ACT_DUT = 'N'
						AND BP.LC_NTF_ACT_DUT = 'N'
					)
		AND (
				(
					BP.LR_ITR > X 
					AND BP.LC_ACT_DUT = 'X'
				)
			OR (
					BP.LC_ACT_DUT = 'N' 
					AND BP.LC_NTF_ACT_DUT = 'Y' 
					AND BP.LR_ITR > X
				)
			OR (
					GETDATE() BETWEEN BP.LD_ITR_EFF_BEG AND BP.LD_ITR_EFF_END 
					AND BP.LC_INT_RDC_PGM = 'M' 
					AND BP.LR_ITR <= X
					AND BP.LC_NTF_ACT_DUT = 'Y'
					AND (AD.BeginDate != BP.LD_EID_BEGIN_DATE OR AD.EndDate != 'XXXX-XX-XX' OR AD.ActiveDutyId IS NULL)
				)
			OR (
					GETDATE() BETWEEN BP.LD_ITR_EFF_BEG AND BP.LD_ITR_EFF_END 
					AND BP.LC_INT_RDC_PGM = 'M' 
					AND BP.LR_ITR <= X
					AND BP.LC_NTF_ACT_DUT != 'Y'
					AND (AD.BeginDate != BP.LD_ACT_DUT_BEG OR AD.EndDate != BP.LD_ACT_DUT_END OR AD.ActiveDutyId IS NULL)
				)
			)

--Endorser USCRA/MSCRA
INSERT INTO @RX
	SELECT DISTINCT
		X AS ArcTypeId,
		NULL AS ArcResponseCodeId,
		EP.DF_SPE_ACC_ID AS AccountNumber,
		NULL AS RecipientId,
		CASE
			--Notified of active duty
			WHEN	EP.LC_NTF_ACT_DUT = 'Y' 
					AND EP.LC_INT_RDC_PGM = 'M' 
					AND EP.LR_ITR <= X
					AND (
							AD.BeginDate != EP.LD_EID_BEGIN_DATE 
							OR AD.EndDate != 'XXXX-XX-XX'
						) 
					THEN 'USCRA'
			--Either the begin or end date has changed
			WHEN	EP.LC_INT_RDC_PGM = 'M' 
					AND EP.LR_ITR <= X 
					AND (
							AD.BeginDate != EP.LD_ACT_DUT_BEG 
							OR AD.EndDate != EP.LD_ACT_DUT_END
						)
					THEN 'USCRA'
			--No Entry in the ActiveDuty table
			WHEN	EP.LC_INT_RDC_PGM = 'M' 
					AND EP.LR_ITR <= X 
					AND AD.ActiveDutyId IS NULL 
					THEN 'USCRA'
			--Handles adding a new USCRA if the end date is unknown and the borrower has had the benefit for almost a year
			WHEN	EP.LC_INT_RDC_PGM = 'M' 
					AND EP.LR_ITR <= X 
					AND DATEDIFF(DAY,GETDATE(),EP.LD_ITR_EFF_END) <= XX
					AND AD.BeginDate = EP.LD_ACT_DUT_BEG
					AND AD.EndDate = EP.LD_ACT_DUT_END
					AND AD.EndDate = 'XXXX-XX-XX'
					THEN 'USCRA'
					ELSE 'MSCRA'
		END AS Arc,
		'SCRAINTUPD' AS ScriptId,
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
		'SCRAFED' AS CreatedBy,
		NULL AS ProcessedAt
	FROM
		CLS.scra._EndorsersPopulation EP
		LEFT JOIN CLS.scra.Borrowers MIL
			ON MIL.BorrowerAccountNumber = EP.DF_SPE_ACC_ID
		LEFT JOIN CLS.scra.ActiveDuty AD
			ON AD.BorrowerId = MIL.BorrowerId
	WHERE
			NOT (
					(
						EP.LD_LON_X_DSB > EP.LD_ACT_DUT_BEG 
						AND EP.LC_NTF_ACT_DUT != 'Y'
					) 
					OR (
							EP.LD_LON_X_DSB > ISNULL(EP.LD_EID_BEGIN_DATE,'XXXX-XX-XX') 
							AND EP.LC_NTF_ACT_DUT = 'Y'
						)
				)
			AND NOT (
						EP.LC_ACT_DUT = 'N'
						AND EP.LC_LFT_ACT_DUT = 'N'
						AND EP.LC_NTF_ACT_DUT = 'N'
					)
			AND (
					(
						EP.LR_ITR > X 
						AND EP.LC_ACT_DUT = 'X'
					)
			OR (
					EP.LC_ACT_DUT = 'N' 
					AND EP.LC_NTF_ACT_DUT = 'Y' 
					AND EP.LR_ITR > X
				)
			OR (
					GETDATE() BETWEEN EP.LD_ITR_EFF_BEG AND EP.LD_ITR_EFF_END 
					AND EP.LC_INT_RDC_PGM = 'M' 
					AND EP.LR_ITR <= X
					AND EP.LC_NTF_ACT_DUT = 'Y'
					AND (
							AD.BeginDate != EP.LD_EID_BEGIN_DATE 
							OR AD.EndDate != 'XXXX-XX-XX' 
							OR AD.ActiveDutyId IS NULL
						)
				)
			OR (
					GETDATE() BETWEEN EP.LD_ITR_EFF_BEG AND EP.LD_ITR_EFF_END 
					AND EP.LC_INT_RDC_PGM = 'M' 
					AND EP.LR_ITR <= X
					AND EP.LC_NTF_ACT_DUT != 'Y'
					AND (
							AD.BeginDate != EP.LD_ACT_DUT_BEG 
							OR AD.EndDate != EP.LD_ACT_DUT_END 
							OR AD.ActiveDutyId IS NULL
						)
				)
			)

--Endorser NSCRA
INSERT INTO @RX
	SELECT DISTINCT
		X AS ArcTypeId,
		NULL AS ArcResponseCodeId,
		EP.DF_SPE_ACC_ID AS AccountNumber,
		NULL AS RecipientId,
		'NSCRA' AS ARC,
		'SCRAINTUPD' AS ScriptId,
		GETDATE() AS ProcessOn,
		CASE
			--Notified of active duty
			WHEN	EP.LC_NTF_ACT_DUT = 'Y' 
					AND EP.LC_INT_RDC_PGM = 'M' 
					AND EP.LR_ITR <= X
					AND (
							AD.BeginDate != EP.LD_EID_BEGIN_DATE 
							OR AD.EndDate != 'XXXX-XX-XX'
						) 
					THEN 'Update Record'
			--Either the begin or end date has changed
			WHEN	EP.LC_INT_RDC_PGM = 'M' 
					AND EP.LR_ITR <= X 
					AND (
							AD.BeginDate != EP.LD_ACT_DUT_BEG 
							OR AD.EndDate != EP.LD_ACT_DUT_END
						)
					THEN 'Update Record'
			--No Entry in the ActiveDuty table
			WHEN	EP.LC_INT_RDC_PGM = 'M' 
					AND EP.LR_ITR <= X 
					AND AD.ActiveDutyId IS NULL 
					THEN 'Update Record'
			--Handles adding a new USCRA if the end date is unknown and the borrower has had the benefit for almost a year
			WHEN	EP.LC_INT_RDC_PGM = 'M' 
					AND EP.LR_ITR <= X 
					AND DATEDIFF(DAY,GETDATE(), EP.LD_ITR_EFF_END) <= XX
					AND AD.BeginDate = EP.LD_ACT_DUT_BEG
					AND AD.EndDate = EP.LD_ACT_DUT_END
					AND AD.EndDate = 'XXXX-XX-XX'
					THEN 'Update Record'
					ELSE 'New Record'
		END AS Comment,
		X AS IsReference,
		X AS IsEndorser,
		NULL AS ProcessFrom,
		NULL AS ProcessTo,
		NULL AS NeededBy,
		EP.LF_EDS AS RegardsTo,
		'E' AS RegardsCode,
		GETDATE() AS CreatedAt,
		'SCRAFED' AS CreatedBy,
		NULL AS ProcessedAt
	FROM
		CLS.scra._EndorsersPopulation EP
		LEFT JOIN CLS.scra.Borrowers MIL
			ON MIL.BorrowerAccountNumber = EP.DF_SPE_ACC_ID
		LEFT JOIN CLS.scra.ActiveDuty AD
			ON AD.BorrowerId = MIL.BorrowerId
	WHERE
			NOT (
					(
						EP.LD_LON_X_DSB > EP.LD_ACT_DUT_BEG 
						AND EP.LC_NTF_ACT_DUT != 'Y'
					) 
					OR (
							EP.LD_LON_X_DSB > ISNULL(EP.LD_EID_BEGIN_DATE,'XXXX-XX-XX') 
							AND EP.LC_NTF_ACT_DUT = 'Y'
						)
				)
			AND NOT (
						EP.LC_ACT_DUT = 'N'
						AND EP.LC_LFT_ACT_DUT = 'N'
						AND EP.LC_NTF_ACT_DUT = 'N'
					)
			AND (
					(
						EP.LR_ITR > X 
						AND EP.LC_ACT_DUT = 'X'
					)
			OR (
					EP.LC_ACT_DUT = 'N' 
					AND EP.LC_NTF_ACT_DUT = 'Y' 
					AND EP.LR_ITR > X
				)
			OR (
					GETDATE() BETWEEN EP.LD_ITR_EFF_BEG AND EP.LD_ITR_EFF_END 
					AND EP.LC_INT_RDC_PGM = 'M' 
					AND EP.LR_ITR <= X
					AND EP.LC_NTF_ACT_DUT = 'Y'
					AND (
							AD.BeginDate != EP.LD_EID_BEGIN_DATE 
							OR AD.EndDate != 'XXXX-XX-XX' 
							OR AD.ActiveDutyId IS NULL
						)
				)
			OR (
					GETDATE() BETWEEN EP.LD_ITR_EFF_BEG AND EP.LD_ITR_EFF_END 
					AND EP.LC_INT_RDC_PGM = 'M' 
					AND EP.LR_ITR <= X
					AND EP.LC_NTF_ACT_DUT != 'Y'
					AND (
							AD.BeginDate != EP.LD_ACT_DUT_BEG 
							OR AD.EndDate != EP.LD_ACT_DUT_END 
							OR AD.ActiveDutyId IS NULL
						)
				)
			)

--Borrower ASCRA
INSERT INTO @RX
	SELECT DISTINCT
		X AS ArcTypeId,
		NULL AS ArcResponseCodeId,
		BP.DF_SPE_ACC_ID AS AccountNumber,
		NULL AS RecipientId,
		'ASCRA' AS Arc,
		'UTNWSXX' AS ScriptId,
		GETDATE() AS ProcessOn,
		CASE WHEN	BP.LC_ACT_DUT = 'N' 
					AND BP.LC_NTF_ACT_DUT = 'Y'		
				THEN CASE WHEN AD.ActiveDutyId IS NULL
							THEN 'Borrower is Active duty. Active Duty Begin date = ' + CAST(CAST(BP.LD_EID_BEGIN_DATE AS DATE) AS VARCHAR) + ', Active Duty End Date = ' + CAST(CAST(ISNULL(BP.LD_ACT_DUT_END,'XXXX-XX-XX') AS DATE) AS VARCHAR)
							ELSE 'Borrower Military Dates updated. Active Duty Begin date = ' + CAST(CAST(BP.LD_EID_BEGIN_DATE AS DATE) AS VARCHAR) + ', Active Duty End Date = ' + CAST(CAST(ISNULL(BP.LD_ACT_DUT_END,'XXXX-XX-XX') AS DATE) AS VARCHAR)
							END
				ELSE CASE WHEN AD.ActiveDutyId IS NULL
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
		'SCRAFED' AS CreatedBy,
		NULL AS ProcessedAt
		FROM
			CLS.scra._BorrowersPopulation BP 
			LEFT JOIN CLS.scra.Borrowers MIL
				ON MIL.BorrowerAccountNumber = BP.DF_SPE_ACC_ID
			LEFT JOIN CLS.scra.ActiveDuty AD
				ON AD.BorrowerId = MIL.BorrowerId
		WHERE
			NOT (
					BP.LC_ACT_DUT ='X' 
					AND AD.BeginDate = BP.LD_ACT_DUT_BEG
					AND AD.EndDate = BP.LD_ACT_DUT_END 
					AND AD.Active = X
				) 
			AND NOT (
						BP.LC_ACT_DUT = 'N' 
						AND BP.LC_LFT_ACT_DUT = 'Y' 
						AND BP.LC_NTF_ACT_DUT = 'N' 
						AND AD.BeginDate = BP.LD_ACT_DUT_BEG 
						AND AD.EndDate = BP.LD_ACT_DUT_END 
						AND AD.Active = X
					)
			AND NOT (
						BP.LC_ACT_DUT = 'N' 
						AND BP.LC_NTF_ACT_DUT = 'Y' 
						AND AD.BeginDate = BP.LD_EID_BEGIN_DATE 
						AND AD.EndDate = 'XXXX-XX-XX' AND AD.Active = X
					)
			AND NOT (
						BP.LC_ACT_DUT = 'N'
						AND BP.LC_LFT_ACT_DUT = 'N'
						AND BP.LC_NTF_ACT_DUT = 'N'
					)
			OR (
					AD.ActiveDutyId IS NULL
					AND NOT (
								BP.LC_ACT_DUT = 'N'
								AND BP.LC_LFT_ACT_DUT = 'N'
								AND BP.LC_NTF_ACT_DUT = 'N'
							)
				)
--Endorser ASCRA
INSERT INTO @RX
	SELECT DISTINCT
		X AS ArcTypeId,
		NULL AS ArcResponseCodeId,
		EP.DF_SPE_ACC_ID AS AccountNumber,
		NULL AS RecipientId,
		'ASCRA' AS Arc,
		'UTNWSXX' AS ScriptId,
		GETDATE() AS ProcessOn,
		CASE WHEN	EP.LC_ACT_DUT = 'N' 
					AND EP.LC_NTF_ACT_DUT = 'Y'
				THEN CASE WHEN AD.ActiveDutyId IS NULL
							THEN 'Endorser/Co-borrower is Active duty. Active Duty Begin date = ' + CAST(CAST(EP.LD_EID_BEGIN_DATE AS DATE) AS VARCHAR) + ', Active Duty End Date = ' + CAST(CAST(ISNULL(EP.LD_ACT_DUT_END,'XXXX-XX-XX') AS DATE) AS VARCHAR)
							ELSE 'Endorser/Co-borrower Military Dates updated. Active Duty Begin date = ' + CAST(CAST(EP.LD_EID_BEGIN_DATE AS DATE) AS VARCHAR) + ', Active Duty End Date = ' + CAST(CAST(ISNULL(EP.LD_ACT_DUT_END,'XXXX-XX-XX') AS DATE) AS VARCHAR)
							END
				ELSE CASE WHEN AD.ActiveDutyId IS NULL
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
		'SCRAFED' AS CreatedBy,
		NULL AS ProcessedAt
	FROM
		CLS.scra._EndorsersPopulation EP
		LEFT JOIN CLS.scra.Borrowers MIL
			ON MIL.BorrowerAccountNumber = EP.DF_SPE_ACC_ID
		LEFT JOIN CLS.scra.ActiveDuty AD
			ON AD.BorrowerId = MIL.BorrowerId
		WHERE
			NOT (
					EP.LC_ACT_DUT ='X' 
					AND AD.BeginDate = EP.LD_ACT_DUT_BEG 
					AND AD.EndDate = EP.LD_ACT_DUT_END 
					AND AD.Active = X
				) 
			AND NOT (
						EP.LC_ACT_DUT = 'N' 
						AND EP.LC_LFT_ACT_DUT = 'Y' 
						AND EP.LC_NTF_ACT_DUT = 'N'  
						AND AD.BeginDate = EP.LD_ACT_DUT_BEG 
						AND AD.EndDate = EP.LD_ACT_DUT_END 
						AND AD.Active = X
					)
			AND NOT (
						EP.LC_ACT_DUT = 'N' 
						AND EP.LC_NTF_ACT_DUT = 'Y' 
						AND AD.BeginDate = EP.LD_EID_BEGIN_DATE 
						AND AD.EndDate = 'XXXX-XX-XX' 
						AND AD.Active = X
					)
			AND NOT (
						EP.LC_ACT_DUT = 'N'
						AND EP.LC_LFT_ACT_DUT = 'N'
						AND EP.LC_NTF_ACT_DUT = 'N'
					)
			OR (
					AD.ActiveDutyId IS NULL
					AND NOT (
								EP.LC_ACT_DUT = 'N'
								AND EP.LC_LFT_ACT_DUT = 'N'
								AND EP.LC_NTF_ACT_DUT = 'N'
							)
				)

--Borrower no longer active duty
INSERT INTO @RX
	SELECT DISTINCT
		X AS ArcTypeId,
		NULL AS ArcResponseCodeId,
		BP.DF_SPE_ACC_ID AS AccountNumber,
		NULL AS RecipientId,
		'ISCRA' AS Arc,
		'UTNWSXX' AS ScriptId,
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
		'SCRAFED' AS CreatedBy,
		NULL AS ProcessedAt
	FROM
		CLS.scra._BorrowersPopulation BP
		LEFT JOIN CLS.scra.Borrowers MIL
			ON MIL.BorrowerAccountNumber = BP.DF_SPE_ACC_ID
		LEFT JOIN CLS.scra.ActiveDuty AD
			ON AD.BorrowerId = MIL.BorrowerId
	WHERE
		BP.LC_ACT_DUT = 'N'
		AND BP.LC_NTF_ACT_DUT != 'Y'
		AND BP.LC_LFT_ACT_DUT != 'Y'
		AND AD.Active = X

--Endorser no longer active duty
INSERT INTO @RX
	SELECT DISTINCT
		X AS ArcTypeId,
		NULL AS ArcResponseCodeId,
		EP.DF_SPE_ACC_ID AS AccountNumber,
		NULL AS RecipientId,
		'ISCRA' AS Arc,
		'UTNWSXX' AS ScriptId,
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
		'SCRAFED' AS CreatedBy,
		NULL AS ProcessedAt
	FROM
		CLS.scra._EndorsersPopulation EP
		LEFT JOIN CLS.scra.Borrowers MIL
			ON MIL.BorrowerAccountNumber = EP.DF_SPE_ACC_ID
		LEFT JOIN CLS.scra.ActiveDuty AD
			ON AD.BorrowerId = MIL.BorrowerId
	WHERE
		EP.LC_ACT_DUT = 'N'
		AND EP.LC_NTF_ACT_DUT != 'Y'
		AND EP.LC_LFT_ACT_DUT != 'Y'
		AND AD.Active = X

--INSERT BORROWERS NOT IN DATABASE
INSERT INTO CLS.scra.Borrowers(BorrowerAccountNumber,EndorserAccountNumber)
	(
	SELECT DISTINCT
		BP.DF_SPE_ACC_ID,
		NULL
	FROM 
		CLS.scra._BorrowersPopulation BP
		LEFT JOIN
			(
				SELECT 
					BorrowerAccountNumber
				FROM
					CLS.scra.Borrowers
			) BX
				ON BX.BorrowerAccountNumber = BP.DF_SPE_ACC_ID
	WHERE
		BX.BorrowerAccountNumber IS NULL
	)
--INSERT BORROWER/ENDORSER COMBOS NOT IN DATABASE
INSERT INTO CLS.scra.Borrowers(BorrowerAccountNumber,EndorserAccountNumber)
	(
	SELECT DISTINCT
		EP.DF_SPE_ACC_ID,
		EP.END_ACC_ID
	FROM 
		CLS.scra._EndorsersPopulation EP
		LEFT JOIN
			(
				SELECT
					BorrowerAccountNumber,
					EndorserAccountNumber
				FROM
					CLS.scra.Borrowers
			) EX
				ON EX.BorrowerAccountNumber = EP.DF_SPE_ACC_ID
	WHERE
		EX.EndorserAccountNumber IS NULL 
		AND EX.BorrowerAccountNumber IS NULL
	)

--UPDATE BORROWERS WITH ENDORSERS
UPDATE
		 BOR 
	SET
		BOR.EndorserAccountNumber = EP.END_ACC_ID
	FROM
		CLS.scra.Borrowers BOR
		INNER JOIN CLS.scra._EndorsersPopulation EP
			ON EP.DF_SPE_ACC_ID = BOR.BorrowerAccountNumber
			AND EP.END_ACC_ID IS NOT NULL
	WHERE 
		BOR.EndorserAccountNumber IS NULL


--DELETE FROM RX where ENDORSER is still active even though borrower is not
--TODO: Copy code and set benefit indicator to "endorser"
DELETE RX
FROM @RX RX 
	INNER JOIN CLS.scra.Borrowers BOR 
		ON BOR.BorrowerAccountNumber = RX.AccountNumber
	INNER JOIN CLS.scra._EndorsersPopulation EP
		ON EP.DF_SPE_ACC_ID = BOR.BorrowerAccountNumber
WHERE
	BOR.EndorserAccountNumber IS NOT NULL
	AND (
			EP.LC_ACT_DUT = 'X'
			OR (
					EP.LC_ACT_DUT = 'N'
					AND EP.LC_NTF_ACT_DUT = 'Y'
				)
			OR (
					EP.LC_ACT_DUT = 'N'
					AND EP.LC_LFT_ACT_DUT = 'Y'
				)
		)
--DELETE FROM RX where BORROWER is still active even though endorser is not
--TODO:  Copy code and set benefit indicator to "borrower"
DELETE RX
FROM @RX RX 
	INNER JOIN CLS.scra.Borrowers BOR 
		ON BOR.BorrowerAccountNumber = RX.AccountNumber
	INNER JOIN CLS.scra._BorrowersPopulation BP
		ON BP.DF_SPE_ACC_ID = BOR.BorrowerAccountNumber
WHERE
	(
		BP.LC_ACT_DUT = 'X'
		OR (
				BP.LC_ACT_DUT = 'N'
				AND BP.LC_NTF_ACT_DUT = 'Y'
			)
		OR (
				BP.LC_ACT_DUT = 'N'
				AND BP.LC_LFT_ACT_DUT = 'Y'
			)
	)

--CREATE ACTIVE DUTY RECORDS
INSERT INTO	CLS.scra.ActiveDuty	(BorrowerId,BeginDate,EndDate,IsBorrower,CreatedAt,Active,BenefitSourceId,NotificationDate,IsReservist)
	(
	--NO ENDORSER SO NO DATE MANIPULATION
	SELECT DISTINCT
		BOR.BorrowerId,
		CASE WHEN	DODREC.LC_ACT_DUT = 'N' 
					AND DODREC.LC_NTF_ACT_DUT = 'Y'
				THEN DODREC.LD_EID_BEGIN_DATE
				ELSE DODREC.LD_ACT_DUT_BEG 
			 END,
		CASE WHEN	DODREC.LC_ACT_DUT = 'N' 
					AND DODREC.LC_NTF_ACT_DUT = 'Y'
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
		CLS.scra._BorrowersPopulation DODREC
		INNER JOIN CLS.scra.Borrowers BOR
			ON DODREC.DF_SPE_ACC_ID = BOR.BorrowerAccountNumber
		LEFT JOIN 
		(
			SELECT
				BorrowerId, 
				ActiveDutyId
			FROM
				CLS.scra.ActiveDuty
		) AD 
			ON AD.BorrowerId = BOR.BorrowerId
	WHERE
		AD.ActiveDutyId IS NULL 
		AND (
				DODREC.LD_ACT_DUT_BEG IS NOT NULL 
				OR DODREC.LD_EID_BEGIN_DATE IS NOT NULL
			)
		AND BOR.EndorserAccountNumber IS NULL

	UNION ALL

	--ENDORSERS GROUP NEED TO CARE ABOUT BOTH BORROWER AND ENDORSER DATES IF BORROWER IS ALSO MILITARY
	SELECT DISTINCT
		EN.BorrowerId,
		CASE WHEN	BP.DF_SPE_ACC_ID IS NOT NULL
					AND BP.LC_ACT_DUT = 'N'
					AND BP.LC_NTF_ACT_DUT = 'N'
					AND BP.LC_LFT_ACT_DUT = 'N'
				THEN CASE WHEN	ENREC.LC_ACT_DUT = 'N' AND ENREC.LC_NTF_ACT_DUT = 'Y'
							THEN ENREC.LD_EID_BEGIN_DATE
							ELSE ENREC.LD_ACT_DUT_BEG
					END
			 WHEN	BP.DF_SPE_ACC_ID IS NOT NULL
					AND ENREC.LC_ACT_DUT = 'N'
					AND ENREC.LC_NTF_ACT_DUT = 'N'
					AND ENREC.LC_LFT_ACT_DUT = 'N'
				THEN CASE WHEN	BP.LC_ACT_DUT = 'N' AND BP.LC_NTF_ACT_DUT = 'Y'
							THEN BP.LD_EID_BEGIN_DATE
							ELSE BP.LD_ACT_DUT_BEG
					END
			 WHEN	BP.DF_SPE_ACC_ID IS NOT NULL
					AND BP.LC_ACT_DUT = 'X'
					AND ENREC.LC_ACT_DUT = 'X'
				THEN CASE WHEN	BP.LD_ACT_DUT_BEG <= ENREC.LD_ACT_DUT_BEG 
								AND BP.LD_ACT_DUT_BEG >= BP.LD_LON_X_DSB
							THEN BP.LD_ACT_DUT_BEG
						  WHEN	ENREC.LD_ACT_DUT_BEG <= BP.LD_ACT_DUT_BEG
								AND ENREC.LD_ACT_DUT_BEG >= ENREC.LD_LON_X_DSB
							THEN ENREC.LD_ACT_DUT_BEG
						  WHEN	BP.LD_ACT_DUT_BEG <= ENREC.LD_ACT_DUT_BEG 
								AND BP.LD_ACT_DUT_BEG < BP.LD_LON_X_DSB
							THEN CASE WHEN ENREC.LD_ACT_DUT_BEG > ENREC.LD_LON_X_DSB
										THEN ENREC.LD_ACT_DUT_BEG
									    ELSE BP.LD_ACT_DUT_BEG
								 END
						  WHEN	ENREC.LD_ACT_DUT_BEG <= BP.LD_ACT_DUT_BEG 
								AND ENREC.LD_ACT_DUT_BEG < ENREC.LD_LON_X_DSB
							THEN CASE WHEN BP.LD_ACT_DUT_BEG > BP.LD_LON_X_DSB
										THEN BP.LD_ACT_DUT_BEG
									    ELSE ENREC.LD_ACT_DUT_BEG
								 END
					 END
			 WHEN	BP.DF_SPE_ACC_ID IS NOT NULL
					AND BP.LC_ACT_DUT = 'X'
					AND ENREC.LC_ACT_DUT = 'N' 
					AND ENREC.LC_NTF_ACT_DUT = 'Y'
				THEN CASE WHEN	BP.LD_ACT_DUT_BEG <= ENREC.LD_EID_BEGIN_DATE 
								AND BP.LD_ACT_DUT_BEG >= BP.LD_LON_X_DSB
							THEN BP.LD_ACT_DUT_BEG
						  WHEN	ENREC.LD_EID_BEGIN_DATE <= BP.LD_ACT_DUT_BEG 
								AND ENREC.LD_EID_BEGIN_DATE >= ENREC.LD_LON_X_DSB
							THEN ENREC.LD_EID_BEGIN_DATE
						  WHEN	BP.LD_ACT_DUT_BEG <= ENREC.LD_EID_BEGIN_DATE 
								AND BP.LD_ACT_DUT_BEG < BP.LD_LON_X_DSB
							THEN CASE WHEN ENREC.LD_EID_BEGIN_DATE > ENREC.LD_LON_X_DSB
										THEN ENREC.LD_EID_BEGIN_DATE
									    ELSE BP.LD_ACT_DUT_BEG
								 END
						  WHEN	ENREC.LD_EID_BEGIN_DATE <= BP.LD_ACT_DUT_BEG 
								AND ENREC.LD_EID_BEGIN_DATE < ENREC.LD_LON_X_DSB
							THEN CASE WHEN BP.LD_ACT_DUT_BEG > BP.LD_LON_X_DSB
										THEN BP.LD_ACT_DUT_BEG
									    ELSE ENREC.LD_EID_BEGIN_DATE
								 END
					 END
			 WHEN	BP.DF_SPE_ACC_ID IS NOT NULL
					AND BP.LC_ACT_DUT = 'N'
					AND BP.LC_NTF_ACT_DUT = 'Y'
					AND ENREC.LC_ACT_DUT = 'X' 
				THEN CASE WHEN	BP.LD_EID_BEGIN_DATE <= ENREC.LD_ACT_DUT_BEG 
								AND BP.LD_EID_BEGIN_DATE >= BP.LD_LON_X_DSB
							THEN BP.LD_EID_BEGIN_DATE
						  WHEN	ENREC.LD_ACT_DUT_BEG <= BP.LD_EID_BEGIN_DATE 
								AND ENREC.LD_ACT_DUT_BEG >= ENREC.LD_LON_X_DSB
							THEN ENREC.LD_ACT_DUT_BEG
						  WHEN	BP.LD_EID_BEGIN_DATE <= ENREC.LD_ACT_DUT_BEG 
								AND BP.LD_EID_BEGIN_DATE < BP.LD_LON_X_DSB
							THEN CASE WHEN ENREC.LD_ACT_DUT_BEG > ENREC.LD_LON_X_DSB
										THEN ENREC.LD_ACT_DUT_BEG
									    ELSE BP.LD_EID_BEGIN_DATE
								 END
						  WHEN	ENREC.LD_ACT_DUT_BEG <= BP.LD_EID_BEGIN_DATE 
								AND ENREC.LD_ACT_DUT_BEG < ENREC.LD_LON_X_DSB
							THEN CASE WHEN BP.LD_EID_BEGIN_DATE > BP.LD_LON_X_DSB
										THEN BP.LD_EID_BEGIN_DATE
									    ELSE ENREC.LD_ACT_DUT_BEG
								 END
					 END
			 WHEN	BP.DF_SPE_ACC_ID IS NOT NULL 
					AND BP.LC_ACT_DUT = 'N' 
					AND BP.LC_NTF_ACT_DUT = 'Y' 
					AND ENREC.LC_ACT_DUT = 'N' 
					AND ENREC.LC_NTF_ACT_DUT = 'Y'
				THEN CASE WHEN	BP.LD_EID_BEGIN_DATE <= ENREC.LD_EID_BEGIN_DATE 
								AND BP.LD_EID_BEGIN_DATE >= BP.LD_LON_X_DSB
							THEN BP.LD_EID_BEGIN_DATE
						  WHEN	ENREC.LD_EID_BEGIN_DATE <= BP.LD_EID_BEGIN_DATE 
								AND ENREC.LD_EID_BEGIN_DATE >= ENREC.LD_LON_X_DSB
							THEN ENREC.LD_EID_BEGIN_DATE
						  WHEN	BP.LD_EID_BEGIN_DATE <= ENREC.LD_EID_BEGIN_DATE 
								AND BP.LD_EID_BEGIN_DATE < BP.LD_LON_X_DSB
							THEN CASE WHEN ENREC.LD_EID_BEGIN_DATE > ENREC.LD_LON_X_DSB
										THEN ENREC.LD_EID_BEGIN_DATE
									    ELSE BP.LD_EID_BEGIN_DATE
								 END
						  WHEN	ENREC.LD_EID_BEGIN_DATE <= BP.LD_EID_BEGIN_DATE 
								AND ENREC.LD_EID_BEGIN_DATE < ENREC.LD_LON_X_DSB
							THEN CASE WHEN BP.LD_EID_BEGIN_DATE > BP.LD_LON_X_DSB
										THEN BP.LD_EID_BEGIN_DATE
									    ELSE ENREC.LD_EID_BEGIN_DATE
								 END
					 END
		     WHEN	BP.DF_SPE_ACC_ID IS NOT NULL 
					AND BP.LC_ACT_DUT = 'N' 
					AND BP.LC_NTF_ACT_DUT = 'Y' 
					AND NOT(
								ENREC.LC_ACT_DUT = 'N' 
								AND ENREC.LC_NTF_ACT_DUT = 'Y'
							)
				THEN CASE WHEN	BP.LD_EID_BEGIN_DATE <= ENREC.LD_ACT_DUT_BEG 
								AND BP.LD_EID_BEGIN_DATE >= BP.LD_LON_X_DSB
							THEN BP.LD_EID_BEGIN_DATE
						  WHEN	ENREC.LD_ACT_DUT_BEG <= BP.LD_EID_BEGIN_DATE 
								AND ENREC.LD_ACT_DUT_BEG >= ENREC.LD_LON_X_DSB
							THEN ENREC.LD_ACT_DUT_BEG
						  WHEN	BP.LD_EID_BEGIN_DATE <= ENREC.LD_ACT_DUT_BEG 
								AND BP.LD_EID_BEGIN_DATE < BP.LD_LON_X_DSB
							THEN CASE WHEN ENREC.LD_ACT_DUT_BEG > ENREC.LD_LON_X_DSB
										THEN ENREC.LD_ACT_DUT_BEG
									    ELSE BP.LD_EID_BEGIN_DATE
								 END
						  WHEN	ENREC.LD_ACT_DUT_BEG <= BP.LD_EID_BEGIN_DATE 
								AND ENREC.LD_ACT_DUT_BEG < ENREC.LD_LON_X_DSB
							THEN CASE WHEN BP.LD_EID_BEGIN_DATE > BP.LD_LON_X_DSB
										THEN BP.LD_EID_BEGIN_DATE
									    ELSE ENREC.LD_ACT_DUT_BEG
								 END
					 END
			 WHEN	BP.DF_SPE_ACC_ID IS NOT NULL 
					AND NOT(
								BP.LC_ACT_DUT = 'N' 
								AND BP.LC_NTF_ACT_DUT = 'Y'
							)
					AND ENREC.LC_ACT_DUT = 'N' 
					AND ENREC.LC_NTF_ACT_DUT = 'Y'
				THEN CASE WHEN	BP.LD_ACT_DUT_BEG <= ENREC.LD_EID_BEGIN_DATE 
								AND BP.LD_ACT_DUT_BEG >= BP.LD_LON_X_DSB
							THEN BP.LD_ACT_DUT_BEG
						  WHEN	ENREC.LD_EID_BEGIN_DATE <= BP.LD_ACT_DUT_BEG 
								AND ENREC.LD_EID_BEGIN_DATE >= ENREC.LD_LON_X_DSB
							THEN ENREC.LD_EID_BEGIN_DATE
						  WHEN	BP.LD_ACT_DUT_BEG <= ENREC.LD_EID_BEGIN_DATE 
								AND BP.LD_ACT_DUT_BEG < BP.LD_LON_X_DSB
							THEN CASE WHEN ENREC.LD_EID_BEGIN_DATE > ENREC.LD_LON_X_DSB
										THEN ENREC.LD_EID_BEGIN_DATE
									    ELSE BP.LD_ACT_DUT_BEG
								 END
						  WHEN	ENREC.LD_EID_BEGIN_DATE <= BP.LD_ACT_DUT_BEG 
								AND ENREC.LD_EID_BEGIN_DATE < ENREC.LD_LON_X_DSB
							THEN CASE WHEN BP.LD_ACT_DUT_BEG > BP.LD_LON_X_DSB
										THEN BP.LD_ACT_DUT_BEG
									    ELSE ENREC.LD_EID_BEGIN_DATE
								 END
					 END
			 WHEN	BP.DF_SPE_ACC_ID IS NOT NULL 
					AND NOT(
								BP.LC_ACT_DUT = 'N' 
								AND BP.LC_NTF_ACT_DUT = 'Y'
							) 
					AND NOT(
								ENREC.LC_ACT_DUT = 'N' 
								AND ENREC.LC_NTF_ACT_DUT = 'Y'
							)
				THEN CASE WHEN	BP.LD_ACT_DUT_BEG <= ENREC.LD_ACT_DUT_BEG 
								AND BP.LD_ACT_DUT_BEG >= BP.LD_LON_X_DSB
							THEN BP.LD_ACT_DUT_BEG
						  WHEN	ENREC.LD_ACT_DUT_BEG <= BP.LD_ACT_DUT_BEG
								AND ENREC.LD_ACT_DUT_BEG >= ENREC.LD_LON_X_DSB
							THEN ENREC.LD_ACT_DUT_BEG
						  WHEN	BP.LD_ACT_DUT_BEG <= ENREC.LD_ACT_DUT_BEG 
								AND BP.LD_ACT_DUT_BEG < BP.LD_LON_X_DSB
							THEN CASE WHEN ENREC.LD_ACT_DUT_BEG > ENREC.LD_LON_X_DSB
										THEN ENREC.LD_ACT_DUT_BEG
									    ELSE BP.LD_ACT_DUT_BEG
								 END
						  WHEN	ENREC.LD_ACT_DUT_BEG <= BP.LD_ACT_DUT_BEG 
								AND ENREC.LD_ACT_DUT_BEG < ENREC.LD_LON_X_DSB
							THEN CASE WHEN BP.LD_ACT_DUT_BEG > BP.LD_LON_X_DSB
										THEN BP.LD_ACT_DUT_BEG
									    ELSE ENREC.LD_ACT_DUT_BEG
								 END
					 END
			 WHEN	BP.DF_SPE_ACC_ID IS NULL 
					AND ENREC.LC_ACT_DUT = 'N' 
					AND ENREC.LC_NTF_ACT_DUT = 'Y'
				THEN ENREC.LD_EID_BEGIN_DATE
			 WHEN	BP.DF_SPE_ACC_ID IS NULL 
					AND NOT(
								ENREC.LC_ACT_DUT = 'N' 
								AND ENREC.LC_NTF_ACT_DUT = 'Y'
							)
				THEN ENREC.LD_ACT_DUT_BEG
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
			 ELSE
				ISNULL(ENREC.LD_ACT_DUT_END,'XXXX-XX-XX')
		END,
		X,
		GETDATE(),
		X,
		CASE WHEN BP.DF_SPE_ACC_ID IS NULL
				THEN X
				ELSE X 
			END, --endorser or both
		CASE WHEN BP.DF_SPE_ACC_ID IS NULL --take endorser date if borrower is not military
				THEN ENREC.LD_NOTIFICATION_DATE
			 WHEN BP.DF_SPE_ACC_ID IS NOT NULL --take the later of the X dates if both are military
				THEN CASE WHEN ENREC.LD_NOTIFICATION_DATE >= BP.LD_NOTIFICATION_DATE
							THEN ENREC.LD_NOTIFICATION_DATE
							ELSE BP.LD_NOTIFICATION_DATE
						END
			END, --notificationDate
		CASE WHEN ENREC.LC_NTF_ACT_DUT = 'Y' AND BP.DF_SPE_ACC_ID IS NULL --Is reservist indicator
				THEN X
			 WHEN ENREC.LC_NTF_ACT_DUT = 'Y' AND BP.LC_NTF_ACT_DUT = 'Y' AND BP.DF_SPE_ACC_ID IS NOT NULL --Is reservist indicator
				THEN X
				ELSE X 
			END
	FROM
		CLS.scra._EndorsersPopulation ENREC
		INNER JOIN CLS.scra.Borrowers EN
			ON ENREC.DF_SPE_ACC_ID = EN.BorrowerAccountNumber
		LEFT JOIN 
		(
			SELECT
				BorrowerId,
				ActiveDutyId
			FROM
				CLS.scra.ActiveDuty
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
				CLS.scra._BorrowersPopulation
		) BP
			ON BP.DF_SPE_ACC_ID = ENREC.DF_SPE_ACC_ID --The endorser record is endorsing a borrower with his or her own record already
	WHERE
		AD.ActiveDutyId IS NULL 
		AND (ENREC.LD_ACT_DUT_BEG IS NOT NULL OR ENREC.LD_EID_BEGIN_DATE IS NOT NULL)
	)
--Borrower only update
UPDATE
	AD
SET
	AD.BeginDate= (CASE WHEN	BP.LC_ACT_DUT = 'X'
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
					END),
	AD.EndDate= (CASE WHEN	BP.LC_ACT_DUT = 'N'
							AND BP.LC_LFT_ACT_DUT = 'Y'
							AND BP.LC_NTF_ACT_DUT = 'Y' 
						THEN 'XXXX-XX-XX'
					  WHEN	BP.LC_ACT_DUT = 'N'
							AND BP.LC_LFT_ACT_DUT = 'N'
							AND BP.LC_NTF_ACT_DUT = 'N'
							AND BP.LD_ACT_DUT_END IS NULL
						THEN AD.EndDate
						ELSE ISNULL(BP.LD_ACT_DUT_END,'XXXX-XX-XX')
					END),
	AD.IsBorrower= X,
	AD.Active = (CASE WHEN RX.AccountNumber IS NOT NULL
						THEN X
					  WHEN RX.AccountNumber IS NOT NULL
						THEN X
						ELSE AD.Active
				END),
	AD.BenefitSourceId = X,
	AD.NotificationDate = BP.LD_NOTIFICATION_DATE,
	AD.IsReservist = (CASE WHEN BP.LC_NTF_ACT_DUT = 'Y' --Is reservist indicator
							THEN X 
							ELSE X 
						END)
FROM 
	CLS.scra.ActiveDuty AD
	INNER JOIN CLS.scra.Borrowers BOR
	ON BOR.BorrowerId = AD.BorrowerId
	LEFT OUTER JOIN CLS.scra._BorrowersPopulation BP
	ON BP.DF_SPE_ACC_ID = BOR.BorrowerAccountNumber
	LEFT OUTER JOIN CLS.scra._EndorsersPopulation ENREC
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
	ENREC.DF_SPE_ACC_ID IS NULL AND BP.DF_SPE_ACC_ID IS NOT NULL

--endorser exists
UPDATE
	AD
SET 
	AD.BeginDate= (CASE WHEN	BP.DF_SPE_ACC_ID IS NOT NULL
								AND BP.LC_ACT_DUT = 'N'
								AND BP.LC_NTF_ACT_DUT = 'N'
								AND BP.LC_LFT_ACT_DUT = 'N'
							THEN CASE WHEN	ENREC.LC_ACT_DUT = 'N' AND ENREC.LC_NTF_ACT_DUT = 'Y'
										THEN ENREC.LD_EID_BEGIN_DATE
										ELSE COALESCE(ENREC.LD_ACT_DUT_BEG, AD.BeginDate)
								END
						 WHEN	BP.DF_SPE_ACC_ID IS NOT NULL
								AND ENREC.LC_ACT_DUT = 'N'
								AND ENREC.LC_NTF_ACT_DUT = 'N'
								AND ENREC.LC_LFT_ACT_DUT = 'N'
							THEN CASE WHEN	BP.LC_ACT_DUT = 'N' AND BP.LC_NTF_ACT_DUT = 'Y'
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
							THEN CASE WHEN	BP.LD_ACT_DUT_BEG <= ENREC.LD_ACT_DUT_BEG 
											AND BP.LD_ACT_DUT_BEG >= BP.LD_LON_X_DSB
										THEN BP.LD_ACT_DUT_BEG
									  WHEN	ENREC.LD_ACT_DUT_BEG <= BP.LD_ACT_DUT_BEG
											AND ENREC.LD_ACT_DUT_BEG >= ENREC.LD_LON_X_DSB
										THEN ENREC.LD_ACT_DUT_BEG
									  WHEN	BP.LD_ACT_DUT_BEG <= ENREC.LD_ACT_DUT_BEG 
											AND BP.LD_ACT_DUT_BEG < BP.LD_LON_X_DSB
										THEN CASE WHEN ENREC.LD_ACT_DUT_BEG > ENREC.LD_LON_X_DSB
													THEN ENREC.LD_ACT_DUT_BEG
												    ELSE BP.LD_ACT_DUT_BEG
											 END
									  WHEN	ENREC.LD_ACT_DUT_BEG <= BP.LD_ACT_DUT_BEG 
											AND ENREC.LD_ACT_DUT_BEG < ENREC.LD_LON_X_DSB
										THEN CASE WHEN BP.LD_ACT_DUT_BEG > BP.LD_LON_X_DSB
													THEN BP.LD_ACT_DUT_BEG
												    ELSE ENREC.LD_ACT_DUT_BEG
											 END
								 END
						 WHEN	BP.DF_SPE_ACC_ID IS NOT NULL
								AND BP.LC_ACT_DUT = 'X'
								AND ENREC.LC_ACT_DUT = 'N' 
								AND ENREC.LC_NTF_ACT_DUT = 'Y'
							THEN CASE WHEN	BP.LD_ACT_DUT_BEG <= ENREC.LD_EID_BEGIN_DATE 
											AND BP.LD_ACT_DUT_BEG >= BP.LD_LON_X_DSB
										THEN BP.LD_ACT_DUT_BEG
									  WHEN	ENREC.LD_EID_BEGIN_DATE <= BP.LD_ACT_DUT_BEG 
											AND ENREC.LD_EID_BEGIN_DATE >= ENREC.LD_LON_X_DSB
										THEN ENREC.LD_EID_BEGIN_DATE
									  WHEN	BP.LD_ACT_DUT_BEG <= ENREC.LD_EID_BEGIN_DATE 
											AND BP.LD_ACT_DUT_BEG < BP.LD_LON_X_DSB
										THEN CASE WHEN ENREC.LD_EID_BEGIN_DATE > ENREC.LD_LON_X_DSB
													THEN ENREC.LD_EID_BEGIN_DATE
												    ELSE BP.LD_ACT_DUT_BEG
											 END
									  WHEN	ENREC.LD_EID_BEGIN_DATE <= BP.LD_ACT_DUT_BEG 
											AND ENREC.LD_EID_BEGIN_DATE < ENREC.LD_LON_X_DSB
										THEN CASE WHEN BP.LD_ACT_DUT_BEG > BP.LD_LON_X_DSB
													THEN BP.LD_ACT_DUT_BEG
												    ELSE ENREC.LD_EID_BEGIN_DATE
											 END
								 END
						 WHEN	BP.DF_SPE_ACC_ID IS NOT NULL
								AND BP.LC_ACT_DUT = 'N'
								AND BP.LC_NTF_ACT_DUT = 'Y'
								AND ENREC.LC_ACT_DUT = 'X' 
							THEN CASE WHEN	BP.LD_EID_BEGIN_DATE <= ENREC.LD_ACT_DUT_BEG 
											AND BP.LD_EID_BEGIN_DATE >= BP.LD_LON_X_DSB
										THEN BP.LD_EID_BEGIN_DATE
									  WHEN	ENREC.LD_ACT_DUT_BEG <= BP.LD_EID_BEGIN_DATE 
											AND ENREC.LD_ACT_DUT_BEG >= ENREC.LD_LON_X_DSB
										THEN ENREC.LD_ACT_DUT_BEG
									  WHEN	BP.LD_EID_BEGIN_DATE <= ENREC.LD_ACT_DUT_BEG 
											AND BP.LD_EID_BEGIN_DATE < BP.LD_LON_X_DSB
										THEN CASE WHEN ENREC.LD_ACT_DUT_BEG > ENREC.LD_LON_X_DSB
													THEN ENREC.LD_ACT_DUT_BEG
												    ELSE BP.LD_EID_BEGIN_DATE
											 END
									  WHEN	ENREC.LD_ACT_DUT_BEG <= BP.LD_EID_BEGIN_DATE 
											AND ENREC.LD_ACT_DUT_BEG < ENREC.LD_LON_X_DSB
										THEN CASE WHEN BP.LD_EID_BEGIN_DATE > BP.LD_LON_X_DSB
													THEN BP.LD_EID_BEGIN_DATE
												    ELSE ENREC.LD_ACT_DUT_BEG
											 END
								 END
						 WHEN	BP.DF_SPE_ACC_ID IS NOT NULL 
								AND BP.LC_ACT_DUT = 'N' 
								AND BP.LC_NTF_ACT_DUT = 'Y' 
								AND ENREC.LC_ACT_DUT = 'N' 
								AND ENREC.LC_NTF_ACT_DUT = 'Y'
							THEN CASE WHEN	BP.LD_EID_BEGIN_DATE <= ENREC.LD_EID_BEGIN_DATE 
											AND BP.LD_EID_BEGIN_DATE >= BP.LD_LON_X_DSB
										THEN BP.LD_EID_BEGIN_DATE
									  WHEN	ENREC.LD_EID_BEGIN_DATE <= BP.LD_EID_BEGIN_DATE 
											AND ENREC.LD_EID_BEGIN_DATE >= ENREC.LD_LON_X_DSB
										THEN ENREC.LD_EID_BEGIN_DATE
									  WHEN	BP.LD_EID_BEGIN_DATE <= ENREC.LD_EID_BEGIN_DATE 
											AND BP.LD_EID_BEGIN_DATE < BP.LD_LON_X_DSB
										THEN CASE WHEN ENREC.LD_EID_BEGIN_DATE > ENREC.LD_LON_X_DSB
													THEN ENREC.LD_EID_BEGIN_DATE
												    ELSE BP.LD_EID_BEGIN_DATE
											 END
									  WHEN	ENREC.LD_EID_BEGIN_DATE <= BP.LD_EID_BEGIN_DATE 
											AND ENREC.LD_EID_BEGIN_DATE < ENREC.LD_LON_X_DSB
										THEN CASE WHEN BP.LD_EID_BEGIN_DATE > BP.LD_LON_X_DSB
													THEN BP.LD_EID_BEGIN_DATE
												    ELSE ENREC.LD_EID_BEGIN_DATE
											 END
								 END
					     WHEN	BP.DF_SPE_ACC_ID IS NOT NULL 
								AND BP.LC_ACT_DUT = 'N' 
								AND BP.LC_NTF_ACT_DUT = 'Y' 
								AND NOT(
											ENREC.LC_ACT_DUT = 'N' 
											AND ENREC.LC_NTF_ACT_DUT = 'Y'
										)
							THEN CASE WHEN	BP.LD_EID_BEGIN_DATE <= ENREC.LD_ACT_DUT_BEG 
											AND BP.LD_EID_BEGIN_DATE >= BP.LD_LON_X_DSB
										THEN BP.LD_EID_BEGIN_DATE
									  WHEN	ENREC.LD_ACT_DUT_BEG <= BP.LD_EID_BEGIN_DATE 
											AND ENREC.LD_ACT_DUT_BEG >= ENREC.LD_LON_X_DSB
										THEN ENREC.LD_ACT_DUT_BEG
									  WHEN	BP.LD_EID_BEGIN_DATE <= ENREC.LD_ACT_DUT_BEG 
											AND BP.LD_EID_BEGIN_DATE < BP.LD_LON_X_DSB
										THEN CASE WHEN ENREC.LD_ACT_DUT_BEG > ENREC.LD_LON_X_DSB
													THEN ENREC.LD_ACT_DUT_BEG
												    ELSE BP.LD_EID_BEGIN_DATE
											 END
									  WHEN	ENREC.LD_ACT_DUT_BEG <= BP.LD_EID_BEGIN_DATE 
											AND ENREC.LD_ACT_DUT_BEG < ENREC.LD_LON_X_DSB
										THEN CASE WHEN BP.LD_EID_BEGIN_DATE > BP.LD_LON_X_DSB
													THEN BP.LD_EID_BEGIN_DATE
												    ELSE ENREC.LD_ACT_DUT_BEG
											 END
								 END
						 WHEN	BP.DF_SPE_ACC_ID IS NOT NULL 
								AND NOT(
											BP.LC_ACT_DUT = 'N' 
											AND BP.LC_NTF_ACT_DUT = 'Y'
										)
								AND ENREC.LC_ACT_DUT = 'N' 
								AND ENREC.LC_NTF_ACT_DUT = 'Y'
							THEN CASE WHEN	BP.LD_ACT_DUT_BEG <= ENREC.LD_EID_BEGIN_DATE 
											AND BP.LD_ACT_DUT_BEG >= BP.LD_LON_X_DSB
										THEN BP.LD_ACT_DUT_BEG
									  WHEN	ENREC.LD_EID_BEGIN_DATE <= BP.LD_ACT_DUT_BEG 
											AND ENREC.LD_EID_BEGIN_DATE >= ENREC.LD_LON_X_DSB
										THEN ENREC.LD_EID_BEGIN_DATE
									  WHEN	BP.LD_ACT_DUT_BEG <= ENREC.LD_EID_BEGIN_DATE 
											AND BP.LD_ACT_DUT_BEG < BP.LD_LON_X_DSB
										THEN CASE WHEN ENREC.LD_EID_BEGIN_DATE > ENREC.LD_LON_X_DSB
													THEN ENREC.LD_EID_BEGIN_DATE
												    ELSE BP.LD_ACT_DUT_BEG
											 END
									  WHEN	ENREC.LD_EID_BEGIN_DATE <= BP.LD_ACT_DUT_BEG 
											AND ENREC.LD_EID_BEGIN_DATE < ENREC.LD_LON_X_DSB
										THEN CASE WHEN BP.LD_ACT_DUT_BEG > BP.LD_LON_X_DSB
													THEN BP.LD_ACT_DUT_BEG
												    ELSE ENREC.LD_EID_BEGIN_DATE
											 END
								 END
						 WHEN	BP.DF_SPE_ACC_ID IS NOT NULL 
								AND NOT(
											BP.LC_ACT_DUT = 'N' 
											AND BP.LC_NTF_ACT_DUT = 'Y'
										) 
								AND NOT(
											ENREC.LC_ACT_DUT = 'N' 
											AND ENREC.LC_NTF_ACT_DUT = 'Y'
										)
							THEN CASE WHEN	BP.LD_ACT_DUT_BEG <= ENREC.LD_ACT_DUT_BEG 
											AND BP.LD_ACT_DUT_BEG >= BP.LD_LON_X_DSB
										THEN BP.LD_ACT_DUT_BEG
									  WHEN	ENREC.LD_ACT_DUT_BEG <= BP.LD_ACT_DUT_BEG
											AND ENREC.LD_ACT_DUT_BEG >= ENREC.LD_LON_X_DSB
										THEN ENREC.LD_ACT_DUT_BEG
									  WHEN	BP.LD_ACT_DUT_BEG <= ENREC.LD_ACT_DUT_BEG 
											AND BP.LD_ACT_DUT_BEG < BP.LD_LON_X_DSB
										THEN CASE WHEN ENREC.LD_ACT_DUT_BEG > ENREC.LD_LON_X_DSB
													THEN ENREC.LD_ACT_DUT_BEG
												    ELSE BP.LD_ACT_DUT_BEG
											 END
									  WHEN	ENREC.LD_ACT_DUT_BEG <= BP.LD_ACT_DUT_BEG 
											AND ENREC.LD_ACT_DUT_BEG < ENREC.LD_LON_X_DSB
										THEN CASE WHEN BP.LD_ACT_DUT_BEG > BP.LD_LON_X_DSB
													THEN BP.LD_ACT_DUT_BEG
												    ELSE ENREC.LD_ACT_DUT_BEG
											 END
								 END
						 WHEN	BP.DF_SPE_ACC_ID IS NULL 
								AND ENREC.LC_ACT_DUT = 'N' 
								AND ENREC.LC_NTF_ACT_DUT = 'Y'
							THEN ENREC.LD_EID_BEGIN_DATE
						 WHEN	BP.DF_SPE_ACC_ID IS NULL 
								AND NOT(
											ENREC.LC_ACT_DUT = 'N' 
											AND ENREC.LC_NTF_ACT_DUT = 'Y'
										)
							THEN ENREC.LD_ACT_DUT_BEG
					END),
	AD.EndDate= (CASE WHEN	BP.DF_SPE_ACC_ID IS NOT NULL
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
					 ELSE
						ISNULL(ENREC.LD_ACT_DUT_END,'XXXX-XX-XX')
				END),
	AD.IsBorrower= (CASE WHEN ENREC.DF_SPE_ACC_ID IS NOT NULL 
							THEN X 
							ELSE X 
					  END),
	AD.Active= (CASE WHEN RX.AccountNumber IS NOT NULL OR RX.AccountNumber IS NOT NULL
						THEN X
					  WHEN RX.AccountNumber IS NOT NULL OR RX.AccountNumber IS NOT NULL
						THEN X
						ELSE AD.Active
				END),
	AD.BenefitSourceId = (CASE WHEN BP.DF_SPE_ACC_ID IS NULL
								THEN X
							/* ADDED TO FED */
							WHEN ENREC.DF_SPE_ACC_ID IS NOT NULL
							AND ENREC.LC_ACT_DUT = 'N'
							AND ENREC.LC_LFT_ACT_DUT = 'N'
							AND ENREC.LC_NTF_ACT_DUT = 'N' 
								THEN X
								ELSE X 
							END), --endorser or both
	AD.NotificationDate = (CASE WHEN BP.DF_SPE_ACC_ID IS NULL --take endorser date if borrower is not military
									THEN ENREC.LD_NOTIFICATION_DATE
								WHEN BP.DF_SPE_ACC_ID IS NOT NULL --take the later of the X dates if both are military
									THEN CASE WHEN ENREC.LD_NOTIFICATION_DATE >= BP.LD_NOTIFICATION_DATE
												THEN ENREC.LD_NOTIFICATION_DATE
												ELSE BP.LD_NOTIFICATION_DATE
											END
							END), --notificationDate
	AD.IsReservist = (CASE WHEN ENREC.LC_NTF_ACT_DUT = 'Y' AND BP.DF_SPE_ACC_ID IS NULL --Is reservist indicator
								THEN X
					       WHEN ENREC.LC_NTF_ACT_DUT = 'Y' AND BP.LC_NTF_ACT_DUT = 'Y' AND BP.DF_SPE_ACC_ID IS NOT NULL --Is reservist indicator
								THEN X
								ELSE X 
						END)
FROM 
	CLS.scra.ActiveDuty AD
	INNER JOIN CLS.scra.Borrowers BOR
	ON BOR.BorrowerId = AD.BorrowerId
	LEFT OUTER JOIN CLS.scra._BorrowersPopulation BP
	ON BP.DF_SPE_ACC_ID = BOR.BorrowerAccountNumber
	LEFT OUTER JOIN CLS.scra._EndorsersPopulation ENREC
	ON ENREC.DF_SPE_ACC_ID = BOR.BorrowerAccountNumber
	LEFT OUTER JOIN @RX RX 
	ON RX.AccountNumber = BOR.BorrowerAccountNumber
	LEFT OUTER JOIN @RX RX 
	ON RX.AccountNumber = BOR.BorrowerAccountNumber
	LEFT OUTER JOIN @RX RX 
	ON RX.AccountNumber = BOR.BorrowerAccountNumber
	LEFT OUTER JOIN @RX RX 
	ON RX.AccountNumber = BOR.BorrowerAccountNumber
	LEFT OUTER JOIN CLS.scra._EndorsersPopulation EP /* ADDED TO FED */
	ON EP.DF_SPE_ACC_ID = BOR.EndorserAccountNumber
WHERE	
	ENREC.DF_SPE_ACC_ID IS NOT NULL

--ADD ARCS TO ARC ADD
INSERT INTO	CLS.dbo.ARCADDPROCESSING(ARCTYPEID,ACCOUNTNUMBER,RECIPIENTID,ARC,SCRIPTID,PROCESSON,COMMENT,ISREFERENCE,ISENDORSER,PROCESSFROM,PROCESSTO,NEEDEDBY,REGARDSTO,REGARDSCODE,CREATEDAT,CREATEDBY)
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
DROP TABLE CLS.scra._BorrowersPopulation
DROP TABLE CLS.scra._EndorsersPopulation

END

