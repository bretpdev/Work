
CREATE PROCEDURE [dbo].[PopulateScraTables]
AS
BEGIN
	IF 
		EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'scra' and TABLE_NAME = '_BorrowersPopulation') 
		AND EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'scra' and TABLE_NAME = '_EndorsersPopulation') 
	BEGIN
		BEGIN TRANSACTION
		DECLARE @ERROR INT = 0

	SET CONCAT_NULL_YIELDS_NULL OFF
	DECLARE @R2 TABLE(ArcTypeId INT, ArcResponseCodeId INT, AccountNumber CHAR(10), RecipientId CHAR(9), Arc VARCHAR(5), 
					ScriptId CHAR(10), ProcessOn DATETIME, Comment VARCHAR(300), IsReference BIT, IsEndorser BIT, 
					ProcessFrom DATETIME, ProcessTo DATETIME, NeededBy DATETIME, RegardsTo CHAR(9), RegardsCode CHAR(1), 
					CreatedAt DATETIME, CreatedBy VARCHAR(50), ProcessedAt DATETIME)
	DECLARE @R3 TABLE(ArcTypeId INT, ArcResponseCodeId INT, AccountNumber CHAR(10), RecipientId CHAR(9), Arc VARCHAR(5), 
					ScriptId CHAR(10), ProcessOn DATETIME, Comment VARCHAR(300), IsReference BIT, IsEndorser BIT, 
					ProcessFrom DATETIME, ProcessTo DATETIME, NeededBy DATETIME, RegardsTo CHAR(9), RegardsCode CHAR(1), 
					CreatedAt DATETIME, CreatedBy VARCHAR(50), ProcessedAt DATETIME)
	DECLARE @R6 TABLE(ArcTypeId INT, ArcResponseCodeId INT, AccountNumber CHAR(10), RecipientId CHAR(9), Arc VARCHAR(5), 
					ScriptId CHAR(10), ProcessOn DATETIME, Comment VARCHAR(300), IsReference BIT, IsEndorser BIT, 
					ProcessFrom DATETIME, ProcessTo DATETIME, NeededBy DATETIME, RegardsTo CHAR(9), RegardsCode CHAR(1), 
					CreatedAt DATETIME, CreatedBy VARCHAR(50), ProcessedAt DATETIME)
	DECLARE @R7 TABLE(ArcTypeId INT, ArcResponseCodeId INT, AccountNumber CHAR(10), RecipientId CHAR(9), Arc VARCHAR(5), 
					ScriptId CHAR(10), ProcessOn DATETIME, Comment VARCHAR(300), IsReference BIT, IsEndorser BIT, 
					ProcessFrom DATETIME, ProcessTo DATETIME, NeededBy DATETIME, RegardsTo CHAR(9), RegardsCode CHAR(1), 
					CreatedAt DATETIME, CreatedBy VARCHAR(50), ProcessedAt DATETIME)
	DECLARE @R8 TABLE(ArcTypeId INT, ArcResponseCodeId INT, AccountNumber CHAR(10), RecipientId CHAR(9), Arc VARCHAR(5), 
					ScriptId CHAR(10), ProcessOn DATETIME, Comment VARCHAR(300), IsReference BIT, IsEndorser BIT, 
					ProcessFrom DATETIME, ProcessTo DATETIME, NeededBy DATETIME, RegardsTo CHAR(9), RegardsCode CHAR(1), 
					CreatedAt DATETIME, CreatedBy VARCHAR(50), ProcessedAt DATETIME)
	DECLARE @R9 TABLE(ArcTypeId INT, ArcResponseCodeId INT, AccountNumber CHAR(10), RecipientId CHAR(9), Arc VARCHAR(5), 
					ScriptId CHAR(10), ProcessOn DATETIME, Comment VARCHAR(300), IsReference BIT, IsEndorser BIT, 
					ProcessFrom DATETIME, ProcessTo DATETIME, NeededBy DATETIME, RegardsTo CHAR(9), RegardsCode CHAR(1), 
					CreatedAt DATETIME, CreatedBy VARCHAR(50), ProcessedAt DATETIME)

	/*Borrower USCRA/MSCRA
	USCRA for updates on borrowers that already have SCRA database entry
	MSCRA for new borrowers being added to SCRA database*/
	INSERT INTO @R2
		SELECT DISTINCT
			2 AS ArcTypeId,
			NULL AS ArcResponseCodeId,
			BP.DF_SPE_ACC_ID AS AccountNumber,
			NULL AS RecipientId,
			CASE WHEN	BP.LC_NTF_ACT_DUT = 'Y' 	--Notified of active duty
						AND BP.LC_INT_RDC_PGM = 'M'
						AND BP.LR_ITR <= 6 
						AND 
						(
							AD.BeginDate != BP.LD_EID_BEGIN_DATE 
							OR AD.EndDate != '2099-12-31'
						) 
					THEN 'USCRA'
				WHEN	BP.LC_INT_RDC_PGM = 'M' --Either the begin or end date has changed
						AND BP.LR_ITR <= 6 
						AND 
						(
							AD.BeginDate != BP.LD_ACT_DUT_BEG 
							OR AD.EndDate != BP.LD_ACT_DUT_END
						) 
					THEN 'USCRA'
				WHEN	BP.LC_INT_RDC_PGM = 'M' --No Entry in the ActiveDuty table
						AND BP.LR_ITR <= 6 
						AND AD.ActiveDutyId IS NULL
					THEN 'USCRA'
				WHEN	BP.LC_INT_RDC_PGM = 'M' --Handles adding a new USCRA if the end date is unknown and the borrower has had the benefit for almost a year
						AND BP.LR_ITR <= 6 
						AND DATEDIFF(DAY,GETDATE(),BP.LD_ITR_EFF_END) <= 31
						AND AD.BeginDate = BP.LD_ACT_DUT_BEG
						AND 
						(
							AD.EndDate = BP.LD_ACT_DUT_END
							OR AD.EndDate = '2099-12-31'
						)
					THEN 'USCRA'
				WHEN LN72.BF_SSN IS NOT NULL
					THEN 'USCRA'
					ELSE 'MSCRA'
			END AS Arc,
			'UTNW021' AS ScriptId,
			GETDATE() AS ProcessOn,
			CASE WHEN	BP.LC_NTF_ACT_DUT = 'Y' --Notified of active duty
						AND BP.LC_INT_RDC_PGM = 'M'
						AND BP.LR_ITR <= 6 
						AND 
						(
							AD.BeginDate != BP.LD_EID_BEGIN_DATE 
							OR AD.EndDate != '2099-12-31'
						) 
					THEN 'Update Record'
				WHEN	BP.LC_INT_RDC_PGM = 'M'  --Either the begin or end date has changed
						AND BP.LR_ITR <= 6 
						AND 
						(
							AD.BeginDate != BP.LD_ACT_DUT_BEG 
							OR AD.EndDate != BP.LD_ACT_DUT_END
						) 
					THEN 'Update Record'
				WHEN	BP.LC_INT_RDC_PGM = 'M'  --No Entry in the ActiveDuty table
						AND BP.LR_ITR <= 6 
						AND AD.ActiveDutyId IS NULL 
					THEN 'Update Record'
				WHEN	BP.LC_INT_RDC_PGM = 'M' --Handles adding a new USCRA if the end date is unknown and the borrower has had the benefit for almost a year
						AND BP.LR_ITR <= 6 
						AND DATEDIFF(DAY, GETDATE(), BP.LD_ITR_EFF_END) <= 31
						AND AD.BeginDate = BP.LD_ACT_DUT_BEG
						AND AD.EndDate = BP.LD_ACT_DUT_END
						AND AD.EndDate = '2099-12-31'
					THEN 'Update Record'
				WHEN LN72.BF_SSN IS NOT NULL
					THEN 'Update Record'
					ELSE 'New Record'
			END AS Comment,
			0 AS IsReference,
			0 AS IsEndorser,
			NULL AS ProcessFrom,
			NULL AS ProcessTo,
			NULL AS NeededBy,
			NULL AS RegardsTo,
			NULL AS RegardsCode,
			GETDATE() AS CreatedAt,
			'UTNW021' AS CreatedBy,
			NULL AS ProcessedAt
		FROM
			CLS.scra._BorrowersPopulation BP
			LEFT JOIN CLS.scra.Borrowers MIL
				ON MIL.BorrowerAccountNumber = BP.DF_SPE_ACC_ID
			LEFT JOIN CLS.scra.ActiveDuty AD
				ON AD.BorrowerId = MIL.BorrowerId
			LEFT JOIN CDW..PD10_PRS_NME PD10
				ON PD10.DF_SPE_ACC_ID = BP.DF_SPE_ACC_ID
			LEFT JOIN 
			(
				SELECT DISTINCT
					BF_SSN
				FROM
					CDW..LN72_INT_RTE_HST
				WHERE
					LC_INT_RDC_PGM = 'M'
					AND LC_STA_LON72 = 'A'
			) LN72
				ON LN72.BF_SSN = PD10.DF_PRS_ID
		WHERE
			NOT 
			(
				(   --exclude loans disbursed after active duty
					BP.LD_LON_1_DSB > BP.LD_ACT_DUT_BEG 
					AND BP.LC_NTF_ACT_DUT != 'Y'
				) 
				OR 
				(    --exclude reservists where loans disburse after active duty
					BP.LD_LON_1_DSB > ISNULL(BP.LD_EID_BEGIN_DATE,'2099-12-31') 
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
					BP.LR_ITR > 6 
					AND BP.LC_ACT_DUT = 'X'
				)
				OR 
				(    --reservist not receiving interest benefit
					BP.LC_ACT_DUT = 'N' 
					AND BP.LC_NTF_ACT_DUT = 'Y' 
					AND BP.LR_ITR > 6
				)
				OR 
				(    --reservist receiving interest benefit
					GETDATE() BETWEEN BP.LD_ITR_EFF_BEG AND BP.LD_ITR_EFF_END 
					AND BP.LC_INT_RDC_PGM = 'M' 
					AND BP.LR_ITR <= 6
					AND BP.LC_NTF_ACT_DUT = 'Y'
					AND 
					(
						AD.BeginDate != BP.LD_EID_BEGIN_DATE 
						OR AD.EndDate != '2099-12-31'
						OR AD.ActiveDutyId IS NULL
					)
				)
				OR 
				(   --non reservist receiving interest benefit
					GETDATE() BETWEEN BP.LD_ITR_EFF_BEG AND BP.LD_ITR_EFF_END 
					AND BP.LC_INT_RDC_PGM = 'M' 
					AND BP.LR_ITR <= 6
					AND BP.LC_NTF_ACT_DUT != 'Y'
					AND 
					(
						AD.BeginDate != BP.LD_ACT_DUT_BEG 
						OR AD.EndDate != COALESCE(BP.LD_ACT_DUT_END,'2099-12-31')
						OR AD.ActiveDutyId IS NULL
					)
				)
				OR
				(
					BP.LR_ITR <= 6 
					AND BP.LC_ACT_DUT = 'X'
					AND BP.LC_INT_RDC_PGM = 'M' --Handles adding a new USCRA if the end date is unknown and the borrower has had the benefit for almost a year
					AND DATEDIFF(DAY,GETDATE(),BP.LD_ITR_EFF_END) <= 31
					AND AD.BeginDate = BP.LD_ACT_DUT_BEG
					AND
					(
						AD.EndDate = BP.LD_ACT_DUT_END
						OR AD.EndDate = '2099-12-31'
					)
				)
			)
	/********************************************************************
						TRANSACTION HANDLING
	********************************************************************/
	SELECT @ERROR = @@ERROR

	/*Borrower NSCRA (Every borrower receiving new SCRA info will get one of these
		 "Update Record" comment for those receiving a USCRA from above
		 "New Record" comment for those receiving a MSCRA from above*/
	INSERT INTO @R2 
		SELECT DISTINCT
			2 AS ArcTypeId,
			NULL AS ArcResponseCodeId,
			BP.DF_SPE_ACC_ID AS AccountNumber,
			NULL AS RecipientId,
			'NSCRA' AS Arc, 
			'UTNW021' AS ScriptId,
			GETDATE() AS ProcessOn,
			CASE WHEN	BP.LC_NTF_ACT_DUT = 'Y' --Notified of active duty
						AND BP.LC_INT_RDC_PGM = 'M'
						AND BP.LR_ITR <= 6 
						AND 
						(
							AD.BeginDate != BP.LD_EID_BEGIN_DATE 
							OR AD.EndDate != '2099-12-31'
						) 
					THEN 'Update Record'
				WHEN	BP.LC_INT_RDC_PGM = 'M'  --Either the begin or end date has changed
						AND BP.LR_ITR <= 6 
						AND 
						(
							AD.BeginDate != BP.LD_ACT_DUT_BEG 
							OR AD.EndDate != BP.LD_ACT_DUT_END
						) 
					THEN 'Update Record'
				WHEN	BP.LC_INT_RDC_PGM = 'M'  --No Entry in the ActiveDuty table
						AND BP.LR_ITR <= 6 
						AND AD.ActiveDutyId IS NULL 
					THEN 'Update Record'
				WHEN	BP.LC_INT_RDC_PGM = 'M' --Handles adding a new USCRA if the end date is unknown and the borrower has had the benefit for almost a year
						AND BP.LR_ITR <= 6 
						AND DATEDIFF(DAY, GETDATE(), BP.LD_ITR_EFF_END) <= 31
						AND AD.BeginDate = BP.LD_ACT_DUT_BEG
						AND AD.EndDate = BP.LD_ACT_DUT_END
						AND AD.EndDate = '2099-12-31'
					THEN 'Update Record'
				WHEN LN72.BF_SSN IS NOT NULL
					THEN 'Update Record'
					ELSE 'New Record'
			END AS Comment,
			0 AS IsReference,
			0 AS IsEndorser,
			NULL AS ProcessFrom,
			NULL AS ProcessTo,
			NULL AS NeededBy,
			NULL AS RegardsTo,
			NULL AS RegardsCode,
			GETDATE() AS CreatedAt,
			'UTNW021' AS CreatedBy,
			NULL AS ProcessedAt
		FROM
			CLS.scra._BorrowersPopulation BP
			LEFT JOIN CLS.scra.Borrowers MIL
				ON MIL.BorrowerAccountNumber = BP.DF_SPE_ACC_ID
			LEFT JOIN CLS.scra.ActiveDuty AD
				ON AD.BorrowerId = MIL.BorrowerId
			LEFT JOIN CDW..PD10_PRS_NME PD10
				ON PD10.DF_SPE_ACC_ID = BP.DF_SPE_ACC_ID
			LEFT JOIN 
			(
				SELECT DISTINCT
					BF_SSN
				FROM
					CDW..LN72_INT_RTE_HST
				WHERE
					LC_INT_RDC_PGM = 'M'
					AND LC_STA_LON72 = 'A'
			) LN72
				ON LN72.BF_SSN = PD10.DF_PRS_ID
		WHERE
			NOT 
			(
				(
					BP.LD_LON_1_DSB > BP.LD_ACT_DUT_BEG 
					AND BP.LC_NTF_ACT_DUT != 'Y'
				) 
				OR 
				(
					BP.LD_LON_1_DSB > ISNULL(BP.LD_EID_BEGIN_DATE,'2099-12-31') 
					AND BP.LC_NTF_ACT_DUT = 'Y'
				)
			)
			AND NOT 
			(
				BP.LC_ACT_DUT = 'N'
				AND BP.LC_LFT_ACT_DUT = 'N'
				AND BP.LC_NTF_ACT_DUT = 'N'
			)
			AND 
			(
				(
					BP.LR_ITR > 6 
					AND BP.LC_ACT_DUT = 'X'
				)
				OR 
				(
					BP.LC_ACT_DUT = 'N' 
					AND BP.LC_NTF_ACT_DUT = 'Y' 
					AND BP.LR_ITR > 6
				)
				OR 
				(
					GETDATE() BETWEEN BP.LD_ITR_EFF_BEG AND BP.LD_ITR_EFF_END 
					AND BP.LC_INT_RDC_PGM = 'M' 
					AND BP.LR_ITR <= 6
					AND BP.LC_NTF_ACT_DUT = 'Y'
					AND 
					(
						AD.BeginDate != BP.LD_EID_BEGIN_DATE 
						OR AD.EndDate != '2099-12-31' 
						OR AD.ActiveDutyId IS NULL
					)
				)
				OR 
				(
					GETDATE() BETWEEN BP.LD_ITR_EFF_BEG AND BP.LD_ITR_EFF_END 
					AND BP.LC_INT_RDC_PGM = 'M' 
					AND BP.LR_ITR <= 6
					AND BP.LC_NTF_ACT_DUT != 'Y'
					AND 
					(
						AD.BeginDate != BP.LD_ACT_DUT_BEG 
						OR AD.EndDate != BP.LD_ACT_DUT_END 
						OR AD.ActiveDutyId IS NULL
					)
				)
			)


	/********************************************************************
						TRANSACTION HANDLING
	********************************************************************/
	SELECT @ERROR = @ERROR + @@ERROR


	--Endorser USCRA/MSCRA
	INSERT INTO @R3
		SELECT DISTINCT
			4 AS ArcTypeId,
			NULL AS ArcResponseCodeId,
			EP.DF_SPE_ACC_ID AS AccountNumber,
			NULL AS RecipientId,
			CASE WHEN	EP.LC_NTF_ACT_DUT = 'Y' --Notified of active duty
						AND EP.LC_INT_RDC_PGM = 'M' 
						AND EP.LR_ITR <= 6
						AND 
						(
							AD.BeginDate != EP.LD_EID_BEGIN_DATE 
							OR AD.EndDate != '2099-12-31'
						) 
					THEN 'USCRA'
				WHEN	EP.LC_INT_RDC_PGM = 'M' --Either the begin or end date has changed
						AND EP.LR_ITR <= 6 
						AND 
						(
							AD.BeginDate != EP.LD_ACT_DUT_BEG 
							OR AD.EndDate != EP.LD_ACT_DUT_END
						)
					THEN 'USCRA'
				WHEN	EP.LC_INT_RDC_PGM = 'M' --No Entry in the ActiveDuty table
						AND EP.LR_ITR <= 6 
						AND AD.ActiveDutyId IS NULL 
					THEN 'USCRA'
				WHEN	EP.LC_INT_RDC_PGM = 'M' --Handles adding a new USCRA if the end date is unknown and the borrower has had the benefit for almost a year
						AND EP.LR_ITR <= 6 
						AND DATEDIFF(DAY,GETDATE(),EP.LD_ITR_EFF_END) <= 31
						AND AD.BeginDate = EP.LD_ACT_DUT_BEG
						AND
						(
							AD.EndDate = EP.LD_ACT_DUT_END
							OR AD.EndDate = '2099-12-31'
						)
					THEN 'USCRA'
				WHEN LN72.BF_SSN IS NOT NULL
					THEN 'USCRA'
					ELSE 'MSCRA'
			END AS Arc,
			'UTNW021' AS ScriptId,
			GETDATE() AS ProcessOn,
			CASE WHEN	EP.LC_NTF_ACT_DUT = 'Y' --Notified of active duty
						AND EP.LC_INT_RDC_PGM = 'M' 
						AND EP.LR_ITR <= 6
						AND 
						(
							AD.BeginDate != EP.LD_EID_BEGIN_DATE 
							OR AD.EndDate != '2099-12-31'
						) 
					THEN 'Update Record'
				WHEN	EP.LC_INT_RDC_PGM = 'M' --Either the begin or end date has changed
						AND EP.LR_ITR <= 6 
						AND
						(
							AD.BeginDate != EP.LD_ACT_DUT_BEG 
							OR AD.EndDate != EP.LD_ACT_DUT_END
						)
					THEN 'Update Record'
				WHEN	EP.LC_INT_RDC_PGM = 'M' --No Entry in the ActiveDuty table
						AND EP.LR_ITR <= 6 
						AND AD.ActiveDutyId IS NULL 
					THEN 'Update Record'
				WHEN	EP.LC_INT_RDC_PGM = 'M' --Handles adding a new USCRA if the end date is unknown and the borrower has had the benefit for almost a year
						AND EP.LR_ITR <= 6 
						AND DATEDIFF(DAY,GETDATE(), EP.LD_ITR_EFF_END) <= 31
						AND AD.BeginDate = EP.LD_ACT_DUT_BEG
						AND AD.EndDate = EP.LD_ACT_DUT_END
						AND AD.EndDate = '2099-12-31'
					THEN 'Update Record'
				WHEN LN72.BF_SSN IS NOT NULL
					THEN 'Update Record'
					ELSE 'New Record'
			END AS Comment,
			0 AS IsReference,
			1 AS IsEndorser,
			NULL AS ProcessFrom,
			NULL AS ProcessTo,
			NULL AS NeededBy,
			EP.LF_EDS AS RegardsTo,
			'E' AS RegardsCode,
			GETDATE() AS CreatedAt,
			'UTNW021' AS CreatedBy,
			NULL AS ProcessedAt
		FROM
			CLS.scra._EndorsersPopulation EP
			LEFT JOIN CLS.scra.Borrowers MIL
				ON MIL.BorrowerAccountNumber = EP.DF_SPE_ACC_ID
			LEFT JOIN CLS.scra.ActiveDuty AD
				ON AD.BorrowerId = MIL.BorrowerId
			LEFT JOIN CDW..PD10_PRS_NME PD10
				ON PD10.DF_SPE_ACC_ID = EP.DF_SPE_ACC_ID
			LEFT JOIN 
			(
				SELECT DISTINCT
					BF_SSN
				FROM
					CDW..LN72_INT_RTE_HST
				WHERE
					LC_INT_RDC_PGM = 'M'
					AND LC_STA_LON72 = 'A'
			) LN72
				ON LN72.BF_SSN = PD10.DF_PRS_ID
		WHERE
			NOT 
			(
				(
					EP.LD_LON_1_DSB > EP.LD_ACT_DUT_BEG 
					AND EP.LC_NTF_ACT_DUT != 'Y'
				) 
				OR
				(
					EP.LD_LON_1_DSB > ISNULL(EP.LD_EID_BEGIN_DATE,'2099-12-31') 
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
					EP.LR_ITR > 6 
					AND EP.LC_ACT_DUT = 'X'
				)
				OR 
				(
					EP.LC_ACT_DUT = 'N' 
					AND EP.LC_NTF_ACT_DUT = 'Y' 
					AND EP.LR_ITR > 6
				)
				OR 
				(
					GETDATE() BETWEEN EP.LD_ITR_EFF_BEG AND EP.LD_ITR_EFF_END 
					AND EP.LC_INT_RDC_PGM = 'M' 
					AND EP.LR_ITR <= 6
					AND EP.LC_NTF_ACT_DUT = 'Y'
					AND 
					(
						AD.BeginDate != EP.LD_EID_BEGIN_DATE 
						OR AD.EndDate != '2099-12-31' 
						OR AD.ActiveDutyId IS NULL
					)
				)
				OR
				(
					GETDATE() BETWEEN EP.LD_ITR_EFF_BEG AND EP.LD_ITR_EFF_END 
					AND EP.LC_INT_RDC_PGM = 'M' 
					AND EP.LR_ITR <= 6
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
					EP.LR_ITR <= 6 
					AND EP.LC_ACT_DUT = 'X'
					AND EP.LC_INT_RDC_PGM = 'M' --Handles adding a new USCRA if the end date is unknown and the borrower has had the benefit for almost a year
					AND DATEDIFF(DAY,GETDATE(),EP.LD_ITR_EFF_END) <= 31
					AND AD.BeginDate = EP.LD_ACT_DUT_BEG
					AND
					(
						AD.EndDate = EP.LD_ACT_DUT_END
						OR AD.EndDate = '2099-12-31'
					)
				)
			)


	/********************************************************************
						TRANSACTION HANDLING
	********************************************************************/
	SELECT @ERROR = @ERROR + @@ERROR


	--Endorser NSCRA
	INSERT INTO @R3
		SELECT DISTINCT
			4 AS ArcTypeId,
			NULL AS ArcResponseCodeId,
			EP.DF_SPE_ACC_ID AS AccountNumber,
			NULL AS RecipientId,
			'NSCRA' AS ARC,
			'UTNW021' AS ScriptId,
			GETDATE() AS ProcessOn,
			CASE WHEN	EP.LC_NTF_ACT_DUT = 'Y' --Notified of active duty
						AND EP.LC_INT_RDC_PGM = 'M' 
						AND EP.LR_ITR <= 6
						AND 
						(
							AD.BeginDate != EP.LD_EID_BEGIN_DATE 
							OR AD.EndDate != '2099-12-31'
						) 
					THEN 'Update Record'
				WHEN	EP.LC_INT_RDC_PGM = 'M' --Either the begin or end date has changed
						AND EP.LR_ITR <= 6 
						AND
						(
							AD.BeginDate != EP.LD_ACT_DUT_BEG 
							OR AD.EndDate != EP.LD_ACT_DUT_END
						)
					THEN 'Update Record'
				WHEN	EP.LC_INT_RDC_PGM = 'M' --No Entry in the ActiveDuty table
						AND EP.LR_ITR <= 6 
						AND AD.ActiveDutyId IS NULL 
					THEN 'Update Record'
				WHEN	EP.LC_INT_RDC_PGM = 'M' --Handles adding a new USCRA if the end date is unknown and the borrower has had the benefit for almost a year
						AND EP.LR_ITR <= 6 
						AND DATEDIFF(DAY,GETDATE(), EP.LD_ITR_EFF_END) <= 31
						AND AD.BeginDate = EP.LD_ACT_DUT_BEG
						AND AD.EndDate = EP.LD_ACT_DUT_END
						AND AD.EndDate = '2099-12-31'
					THEN 'Update Record'
				WHEN LN72.BF_SSN IS NOT NULL
					THEN 'Update Record'
					ELSE 'New Record'
			END AS Comment,
			0 AS IsReference,
			1 AS IsEndorser,
			NULL AS ProcessFrom,
			NULL AS ProcessTo,
			NULL AS NeededBy,
			EP.LF_EDS AS RegardsTo,
			'E' AS RegardsCode,
			GETDATE() AS CreatedAt,
			'UTNW021' AS CreatedBy,
			NULL AS ProcessedAt
		FROM
			CLS.scra._EndorsersPopulation EP
			LEFT JOIN CLS.scra.Borrowers MIL
				ON MIL.BorrowerAccountNumber = EP.DF_SPE_ACC_ID
			LEFT JOIN CLS.scra.ActiveDuty AD
				ON AD.BorrowerId = MIL.BorrowerId
			LEFT JOIN CDW..PD10_PRS_NME PD10
				ON PD10.DF_SPE_ACC_ID = EP.DF_SPE_ACC_ID
			LEFT JOIN 
			(
				SELECT DISTINCT
					BF_SSN
				FROM
					CDW..LN72_INT_RTE_HST
				WHERE
					LC_INT_RDC_PGM = 'M'
					AND LC_STA_LON72 = 'A'
			) LN72
				ON LN72.BF_SSN = PD10.DF_PRS_ID
		WHERE
			NOT 
			(
				(
					EP.LD_LON_1_DSB > EP.LD_ACT_DUT_BEG 
					AND EP.LC_NTF_ACT_DUT != 'Y'
				) 
				OR 
				(
					EP.LD_LON_1_DSB > ISNULL(EP.LD_EID_BEGIN_DATE,'2099-12-31') 
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
					EP.LR_ITR > 6 
					AND EP.LC_ACT_DUT = 'X'
				)
				OR 
				(
					EP.LC_ACT_DUT = 'N' 
					AND EP.LC_NTF_ACT_DUT = 'Y' 
					AND EP.LR_ITR > 6
				)
				OR
				(
					GETDATE() BETWEEN EP.LD_ITR_EFF_BEG AND EP.LD_ITR_EFF_END 
					AND EP.LC_INT_RDC_PGM = 'M' 
					AND EP.LR_ITR <= 6
					AND EP.LC_NTF_ACT_DUT = 'Y'
					AND 
					(
						AD.BeginDate != EP.LD_EID_BEGIN_DATE 
						OR AD.EndDate != '2099-12-31' 
						OR AD.ActiveDutyId IS NULL
					)
				)
				OR 
				(
					GETDATE() BETWEEN EP.LD_ITR_EFF_BEG AND EP.LD_ITR_EFF_END 
					AND EP.LC_INT_RDC_PGM = 'M' 
					AND EP.LR_ITR <= 6
					AND EP.LC_NTF_ACT_DUT != 'Y'
					AND 
					(
						AD.BeginDate != EP.LD_ACT_DUT_BEG 
						OR AD.EndDate != EP.LD_ACT_DUT_END 
						OR AD.ActiveDutyId IS NULL
					)
				)
			)


	/********************************************************************
						TRANSACTION HANDLING
	********************************************************************/
	SELECT @ERROR = @ERROR + @@ERROR


	--Borrower ASCRA
	INSERT INTO @R6
		SELECT DISTINCT
			2 AS ArcTypeId,
			NULL AS ArcResponseCodeId,
			BP.DF_SPE_ACC_ID AS AccountNumber,
			NULL AS RecipientId,
			'ASCRA' AS Arc,
			'UTNW021' AS ScriptId,
			GETDATE() AS ProcessOn,
			CASE WHEN BP.LC_ACT_DUT = 'N' AND BP.LC_NTF_ACT_DUT = 'Y' 
					THEN 
						CASE WHEN	AD.ActiveDutyId IS NULL 
								THEN 'Borrower is Active duty. Active Duty Begin date = ' + CAST(CAST(BP.LD_EID_BEGIN_DATE AS DATE) AS VARCHAR) + ', Active Duty End Date = ' + CAST(CAST(ISNULL(BP.LD_ACT_DUT_END,'2099-12-31') AS DATE) AS VARCHAR)
								ELSE 'Borrower Military Dates updated. Active Duty Begin date = ' + CAST(CAST(BP.LD_EID_BEGIN_DATE AS DATE) AS VARCHAR) + ', Active Duty End Date = ' + CAST(CAST(ISNULL(BP.LD_ACT_DUT_END,'2099-12-31') AS DATE) AS VARCHAR)
						END
					ELSE 
						CASE WHEN	AD.ActiveDutyId IS NULL 
								THEN 'Borrower is Active duty. Active Duty Begin date = ' + CAST(CAST(BP.LD_ACT_DUT_BEG AS DATE) AS VARCHAR) + ', Active Duty End Date = ' + CAST(CAST(ISNULL(BP.LD_ACT_DUT_END,'2099-12-31') AS DATE) AS VARCHAR)
								ELSE 'Borrower Military Dates updated. Active Duty Begin date = ' + CAST(CAST(BP.LD_ACT_DUT_BEG AS DATE) AS VARCHAR) + ', Active Duty End Date = ' + CAST(CAST(ISNULL(BP.LD_ACT_DUT_END,'2099-12-31') AS DATE) AS VARCHAR)
						END
			END AS Comment,
			0 AS IsReference,
			0 AS IsEndorser,
			NULL AS ProcessFrom,
			NULL AS ProcessTo,
			NULL AS NeededBy,
			NULL AS RegardsTo,
			NULL AS RegardsCode,
			GETDATE() AS CreatedAt,
			'UTNW021' AS CreatedBy,
			NULL AS ProcessedAt
			FROM
				CLS.scra._BorrowersPopulation BP 
				LEFT JOIN CLS.scra.Borrowers MIL
					ON MIL.BorrowerAccountNumber = BP.DF_SPE_ACC_ID
				LEFT JOIN CLS.scra.ActiveDuty AD
					ON AD.BorrowerId = MIL.BorrowerId
			WHERE
				NOT 
				(
					BP.LC_ACT_DUT ='X' 
					AND AD.BeginDate = BP.LD_ACT_DUT_BEG
					AND AD.EndDate = BP.LD_ACT_DUT_END 
					AND AD.Active = 1
				) 
				AND NOT 
				(
					BP.LC_ACT_DUT = 'N' 
					AND BP.LC_LFT_ACT_DUT = 'Y' 
					AND BP.LC_NTF_ACT_DUT = 'N' --Update UHEAA. JW 3/30
				)
				AND NOT 
				(
					BP.LC_ACT_DUT = 'N' 
					AND BP.LC_NTF_ACT_DUT = 'Y' 
					AND AD.BeginDate = BP.LD_EID_BEGIN_DATE 
					AND AD.EndDate = '2099-12-31' 
					AND AD.Active = 1
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
						AND BP.LC_NTF_ACT_DUT = 'N' --Update UHEAA. JW 3/30
					)
				)


	/********************************************************************
						TRANSACTION HANDLING
	********************************************************************/
	SELECT @ERROR = @ERROR + @@ERROR


	--Endorser ASCRA
	INSERT INTO @R7
		SELECT DISTINCT
			4 AS ArcTypeId,
			NULL AS ArcResponseCodeId,
			EP.DF_SPE_ACC_ID AS AccountNumber,
			NULL AS RecipientId,
			'ASCRA' AS Arc,
			'UTNW021' AS ScriptId,
			GETDATE() AS ProcessOn,
			CASE WHEN EP.LC_ACT_DUT = 'N' AND EP.LC_NTF_ACT_DUT = 'Y'
					THEN 
						CASE WHEN	AD.ActiveDutyId IS NULL
								THEN 'Endorser/Co-borrower is Active duty. Active Duty Begin date = ' + CAST(CAST(EP.LD_EID_BEGIN_DATE AS DATE) AS VARCHAR) + ', Active Duty End Date = ' + CAST(CAST(ISNULL(EP.LD_ACT_DUT_END,'2099-12-31') AS DATE) AS VARCHAR)
								ELSE 'Endorser/Co-borrower Military Dates updated. Active Duty Begin date = ' + CAST(CAST(EP.LD_EID_BEGIN_DATE AS DATE) AS VARCHAR) + ', Active Duty End Date = ' + CAST(CAST(ISNULL(EP.LD_ACT_DUT_END,'2099-12-31') AS DATE) AS VARCHAR)
						END
					ELSE 
						CASE WHEN	AD.ActiveDutyId IS NULL
								THEN 'Endorser/Co-borrower is Active duty. Active Duty Begin date = ' + CAST(CAST(EP.LD_ACT_DUT_BEG AS DATE) AS VARCHAR) + ', Active Duty End Date = ' + CAST(CAST(ISNULL(EP.LD_ACT_DUT_END,'2099-12-31') AS DATE) AS VARCHAR)
								ELSE 'Endorser/Co-borrower Military Dates updated. Active Duty Begin date = ' + CAST(CAST(EP.LD_ACT_DUT_BEG AS DATE) AS VARCHAR) + ', Active Duty End Date = ' + CAST(CAST(ISNULL(EP.LD_ACT_DUT_END,'2099-12-31') AS DATE) AS VARCHAR)
						END	
			END AS Comment,
			0 AS IsReference,
			1 AS IsEndorser,
			NULL AS ProcessFrom,
			NULL AS ProcessTo,
			NULL AS NeededBy, 
			EP.LF_EDS AS RegardsTo,
			'E' AS RegardsCode,
			GETDATE() AS CreatedAt,
			'UTNW021' AS CreatedBy,
			NULL AS ProcessedAt
		FROM
			CLS.scra._EndorsersPopulation EP
			LEFT JOIN CLS.scra.Borrowers MIL
				ON MIL.BorrowerAccountNumber = EP.DF_SPE_ACC_ID
			LEFT JOIN CLS.scra.ActiveDuty AD
				ON AD.BorrowerId = MIL.BorrowerId
			WHERE
				NOT 
				(
					EP.LC_ACT_DUT ='X' 
					AND AD.BeginDate = EP.LD_ACT_DUT_BEG 
					AND AD.EndDate = EP.LD_ACT_DUT_END 
					AND AD.Active = 1
				) 
				AND NOT 
				(
					EP.LC_ACT_DUT = 'N' 
					AND EP.LC_LFT_ACT_DUT = 'Y' 
					AND EP.LC_NTF_ACT_DUT = 'N'  --Update UHEAA JW 3/30
				)
				AND NOT 
				(
					EP.LC_ACT_DUT = 'N' 
					AND EP.LC_NTF_ACT_DUT = 'Y' 
					AND AD.BeginDate = EP.LD_EID_BEGIN_DATE 
					AND AD.EndDate = '2099-12-31' 
					AND AD.Active = 1
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
						AND EP.LC_NTF_ACT_DUT = 'N' --Update UHEAA JW 3/30
					)
				)


	/********************************************************************
						TRANSACTION HANDLING
	********************************************************************/
	SELECT @ERROR = @ERROR + @@ERROR


	--Borrower no longer active duty
	INSERT INTO @R8
		SELECT DISTINCT
			2 AS ArcTypeId,
			NULL AS ArcResponseCodeId,
			BP.DF_SPE_ACC_ID AS AccountNumber,
			NULL AS RecipientId,
			'ISCRA' AS Arc,
			'UTNW021' AS ScriptId,
			GETDATE() AS ProcessOn,
			'Active duty status ended' AS Comment,
			0 AS IsReference,
			0 AS IsEndorser,
			NULL AS ProcessFrom,
			NULL AS ProcessTo,
			NULL AS NeededBy,
			NULL AS RegardsTo,
			NULL AS RegardsCode,
			GETDATE() AS CreatedAt,
			'UTNW021' AS CreatedBy,
			NULL AS ProcessedAt
		FROM
			CLS.scra._BorrowersPopulation BP
			LEFT JOIN CLS.scra.Borrowers MIL
				ON MIL.BorrowerAccountNumber = BP.DF_SPE_ACC_ID
			LEFT JOIN CLS.scra.ActiveDuty AD
				ON AD.BorrowerId = MIL.BorrowerId
		WHERE
			BP.LC_ACT_DUT = 'N'
			AND BP.LC_LFT_ACT_DUT = 'Y'
			AND BP.LC_NTF_ACT_DUT = 'N'
			AND AD.Active = 1 --Update UHEAA JW 3/30


	/********************************************************************
						TRANSACTION HANDLING
	********************************************************************/
	SELECT @ERROR = @ERROR + @@ERROR


	--Endorser no longer active duty
	INSERT INTO @R9
		SELECT DISTINCT
			4 AS ArcTypeId,
			NULL AS ArcResponseCodeId,
			EP.DF_SPE_ACC_ID AS AccountNumber,
			NULL AS RecipientId,
			'ISCRA' AS Arc,
			'UTNW021' AS ScriptId,
			GETDATE() AS ProcessOn,
			'Endorser active duty status ended' AS Comment, 
			0 AS IsReference,
			1 AS IsEndorser,
			NULL AS ProcessFrom,
			NULL AS ProcessTo,
			NULL AS NeededBy,
			EP.LF_EDS AS RegardsTo,
			'E' AS RegardsCode,
			GETDATE() AS CreatedAt,
			'UTNW021' AS CreatedBy,
			NULL AS ProcessedAt
		FROM
			CLS.scra._EndorsersPopulation EP
			LEFT JOIN CLS.scra.Borrowers MIL
				ON MIL.BorrowerAccountNumber = EP.DF_SPE_ACC_ID
			LEFT JOIN CLS.scra.ActiveDuty AD
				ON AD.BorrowerId = MIL.BorrowerId
		WHERE
			EP.LC_ACT_DUT = 'N'
			AND EP.LC_LFT_ACT_DUT = 'Y'
			AND EP.LC_NTF_ACT_DUT = 'N'
			AND AD.Active = 1 --Update UHEAA JW 3/30


	/********************************************************************
						TRANSACTION HANDLING
	********************************************************************/
	SELECT @ERROR = @ERROR + @@ERROR


	--INSERT (NEW BORROWER/ENDORSERS)
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
			AND NOT --Dont add non scra borrowers to table
			(
				EP.LC_ACT_DUT = 'N'
				AND EP.LC_LFT_ACT_DUT = 'N'
				AND EP.LC_NTF_ACT_DUT = 'N'
			) --Update UHEAA JW 3/30
	)


	/********************************************************************
						TRANSACTION HANDLING
	********************************************************************/
	SELECT @ERROR = @ERROR + @@ERROR


	--INSERT (NEW BORROWERS)
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
			AND NOT --Dont add non scra borrowers to table
			(
				BP.LC_ACT_DUT = 'N'
				AND BP.LC_LFT_ACT_DUT = 'N'
				AND BP.LC_NTF_ACT_DUT = 'N'
			) --Update UHEAA JW 3/30
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
		CLS.scra.Borrowers BOR
		INNER JOIN CLS.scra._EndorsersPopulation EP
			ON EP.DF_SPE_ACC_ID = BOR.BorrowerAccountNumber
			AND EP.END_ACC_ID IS NOT NULL
	WHERE 
		BOR.EndorserAccountNumber IS NULL


	/********************************************************************
						TRANSACTION HANDLING
	********************************************************************/
	SELECT @ERROR = @ERROR + @@ERROR


	/*DELETE FROM R8 where ENDORSER is still active even though borrower is not
	We dont want to arc the account as no longer active if only the borrower is now inactive*/
	DELETE 
		R8
	FROM 
		@R8 R8 
		INNER JOIN CLS.scra.Borrowers BOR 
			ON BOR.BorrowerAccountNumber = R8.AccountNumber
		INNER JOIN CLS.scra._EndorsersPopulation EP
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
			) --Update UHEAA JW 3/30
		)


	/********************************************************************
						TRANSACTION HANDLING
	********************************************************************/
	SELECT @ERROR = @ERROR + @@ERROR


	/*DELETE FROM R9 where BORROWER is still active even though endorser is not
	We dont want to arc the account as no longer active if only the endorser is now inactive*/
	DELETE 
		R9
	FROM 
		@R9 R9 
		INNER JOIN CLS.scra.Borrowers BOR 
			ON BOR.BorrowerAccountNumber = R9.AccountNumber
		INNER JOIN CLS.scra._BorrowersPopulation BP
			ON BP.DF_SPE_ACC_ID = BOR.BorrowerAccountNumber
	WHERE
		(
			BP.LC_ACT_DUT = 'X'
			OR 
			(
				BP.LC_ACT_DUT = 'N'
				AND BP.LC_NTF_ACT_DUT = 'Y'
			) --Update UHEAA JW 3/30
		)


	/********************************************************************
						TRANSACTION HANDLING
	********************************************************************/
	SELECT @ERROR = @ERROR + @@ERROR


	--CREATE ACTIVE DUTY RECORDS
	INSERT INTO	CLS.scra.ActiveDuty	(BorrowerId,BeginDate,EndDate,IsBorrower,CreatedAt,Active,BenefitSourceId,NotificationDate,IsReservist)
	(
		--NO ENDORSER SO NO DATE MANIPULATION
		SELECT DISTINCT
			BOR.BorrowerId,
			CASE WHEN	DODREC.LC_ACT_DUT = 'N' AND DODREC.LC_NTF_ACT_DUT = 'Y'
					THEN DODREC.LD_EID_BEGIN_DATE
					ELSE DODREC.LD_ACT_DUT_BEG 
			END,
			CASE WHEN	DODREC.LC_ACT_DUT = 'N' AND DODREC.LC_NTF_ACT_DUT = 'Y'
					THEN '2099-12-31'
					ELSE ISNULL(DODREC.LD_ACT_DUT_END,'2099-12-31')
			END,
			1,
			GETDATE(),
			1,
			1, --borrower
			DODREC.LD_NOTIFICATION_DATE, --notificationData
			CASE WHEN DODREC.LC_NTF_ACT_DUT = 'Y' --Is reservist indicator
					THEN 1 
					ELSE 0 
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
									AND BP.LD_ACT_DUT_BEG >= BP.LD_LON_1_DSB
								THEN BP.LD_ACT_DUT_BEG
							 WHEN	ENREC.LD_ACT_DUT_BEG <= BP.LD_ACT_DUT_BEG
									AND ENREC.LD_ACT_DUT_BEG >= ENREC.LD_LON_1_DSB
								THEN ENREC.LD_ACT_DUT_BEG
							 WHEN	BP.LD_ACT_DUT_BEG <= ENREC.LD_ACT_DUT_BEG 
									AND BP.LD_ACT_DUT_BEG < BP.LD_LON_1_DSB
								THEN 
									CASE WHEN	ENREC.LD_ACT_DUT_BEG > ENREC.LD_LON_1_DSB
											THEN ENREC.LD_ACT_DUT_BEG
											ELSE BP.LD_ACT_DUT_BEG
									END
							 WHEN	ENREC.LD_ACT_DUT_BEG <= BP.LD_ACT_DUT_BEG 
									AND ENREC.LD_ACT_DUT_BEG < ENREC.LD_LON_1_DSB
								THEN 
									CASE WHEN	BP.LD_ACT_DUT_BEG > BP.LD_LON_1_DSB
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
									AND BP.LD_ACT_DUT_BEG >= BP.LD_LON_1_DSB
								THEN BP.LD_ACT_DUT_BEG
							 WHEN	ENREC.LD_EID_BEGIN_DATE <= BP.LD_ACT_DUT_BEG 
									AND ENREC.LD_EID_BEGIN_DATE >= ENREC.LD_LON_1_DSB
								THEN ENREC.LD_EID_BEGIN_DATE
							 WHEN	BP.LD_ACT_DUT_BEG <= ENREC.LD_EID_BEGIN_DATE 
									AND BP.LD_ACT_DUT_BEG < BP.LD_LON_1_DSB
								THEN 
									CASE WHEN	ENREC.LD_EID_BEGIN_DATE > ENREC.LD_LON_1_DSB
											THEN ENREC.LD_EID_BEGIN_DATE
											ELSE BP.LD_ACT_DUT_BEG
									END
							 WHEN	ENREC.LD_EID_BEGIN_DATE <= BP.LD_ACT_DUT_BEG 
									AND ENREC.LD_EID_BEGIN_DATE < ENREC.LD_LON_1_DSB
								THEN 
									CASE WHEN	BP.LD_ACT_DUT_BEG > BP.LD_LON_1_DSB
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
									AND BP.LD_EID_BEGIN_DATE >= BP.LD_LON_1_DSB
								THEN BP.LD_EID_BEGIN_DATE
							 WHEN	ENREC.LD_ACT_DUT_BEG <= BP.LD_EID_BEGIN_DATE 
									AND ENREC.LD_ACT_DUT_BEG >= ENREC.LD_LON_1_DSB
								THEN ENREC.LD_ACT_DUT_BEG
							 WHEN	BP.LD_EID_BEGIN_DATE <= ENREC.LD_ACT_DUT_BEG 
									AND BP.LD_EID_BEGIN_DATE < BP.LD_LON_1_DSB
								THEN 
									CASE WHEN	ENREC.LD_ACT_DUT_BEG > ENREC.LD_LON_1_DSB
											THEN ENREC.LD_ACT_DUT_BEG
											ELSE BP.LD_EID_BEGIN_DATE
									END
							 WHEN	ENREC.LD_ACT_DUT_BEG <= BP.LD_EID_BEGIN_DATE 
									AND ENREC.LD_ACT_DUT_BEG < ENREC.LD_LON_1_DSB
								THEN
									CASE WHEN	BP.LD_EID_BEGIN_DATE > BP.LD_LON_1_DSB
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
									AND BP.LD_EID_BEGIN_DATE >= BP.LD_LON_1_DSB
								THEN BP.LD_EID_BEGIN_DATE
							 WHEN	ENREC.LD_EID_BEGIN_DATE <= BP.LD_EID_BEGIN_DATE 
									AND ENREC.LD_EID_BEGIN_DATE >= ENREC.LD_LON_1_DSB
								THEN ENREC.LD_EID_BEGIN_DATE
							 WHEN	BP.LD_EID_BEGIN_DATE <= ENREC.LD_EID_BEGIN_DATE 
									AND BP.LD_EID_BEGIN_DATE < BP.LD_LON_1_DSB
								THEN 
									CASE WHEN	ENREC.LD_EID_BEGIN_DATE > ENREC.LD_LON_1_DSB
											THEN ENREC.LD_EID_BEGIN_DATE
											ELSE BP.LD_EID_BEGIN_DATE
									END
							 WHEN	ENREC.LD_EID_BEGIN_DATE <= BP.LD_EID_BEGIN_DATE 
									AND ENREC.LD_EID_BEGIN_DATE < ENREC.LD_LON_1_DSB
								THEN 
									CASE WHEN	BP.LD_EID_BEGIN_DATE > BP.LD_LON_1_DSB
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
									AND BP.LD_EID_BEGIN_DATE >= BP.LD_LON_1_DSB
								THEN BP.LD_EID_BEGIN_DATE
							 WHEN	ENREC.LD_ACT_DUT_BEG <= BP.LD_EID_BEGIN_DATE 
									AND ENREC.LD_ACT_DUT_BEG >= ENREC.LD_LON_1_DSB
								THEN ENREC.LD_ACT_DUT_BEG
							 WHEN	BP.LD_EID_BEGIN_DATE <= ENREC.LD_ACT_DUT_BEG 
									AND BP.LD_EID_BEGIN_DATE < BP.LD_LON_1_DSB
								THEN 
									CASE WHEN	ENREC.LD_ACT_DUT_BEG > ENREC.LD_LON_1_DSB
											THEN ENREC.LD_ACT_DUT_BEG
											ELSE BP.LD_EID_BEGIN_DATE
									END
							 WHEN	ENREC.LD_ACT_DUT_BEG <= BP.LD_EID_BEGIN_DATE 
									AND ENREC.LD_ACT_DUT_BEG < ENREC.LD_LON_1_DSB
								THEN 
									CASE WHEN	BP.LD_EID_BEGIN_DATE > BP.LD_LON_1_DSB
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
									AND BP.LD_ACT_DUT_BEG >= BP.LD_LON_1_DSB
								THEN BP.LD_ACT_DUT_BEG
							 WHEN	ENREC.LD_EID_BEGIN_DATE <= BP.LD_ACT_DUT_BEG 
									AND ENREC.LD_EID_BEGIN_DATE >= ENREC.LD_LON_1_DSB
								THEN ENREC.LD_EID_BEGIN_DATE
							 WHEN	BP.LD_ACT_DUT_BEG <= ENREC.LD_EID_BEGIN_DATE 
									AND BP.LD_ACT_DUT_BEG < BP.LD_LON_1_DSB
								THEN 
									CASE WHEN	ENREC.LD_EID_BEGIN_DATE > ENREC.LD_LON_1_DSB
											THEN ENREC.LD_EID_BEGIN_DATE
											ELSE BP.LD_ACT_DUT_BEG
									END
							 WHEN	ENREC.LD_EID_BEGIN_DATE <= BP.LD_ACT_DUT_BEG 
									AND ENREC.LD_EID_BEGIN_DATE < ENREC.LD_LON_1_DSB
								THEN 
									CASE WHEN	BP.LD_ACT_DUT_BEG > BP.LD_LON_1_DSB
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
									AND BP.LD_ACT_DUT_BEG >= BP.LD_LON_1_DSB
								THEN BP.LD_ACT_DUT_BEG
							 WHEN	ENREC.LD_ACT_DUT_BEG <= BP.LD_ACT_DUT_BEG
									AND ENREC.LD_ACT_DUT_BEG >= ENREC.LD_LON_1_DSB
								THEN ENREC.LD_ACT_DUT_BEG
							 WHEN	BP.LD_ACT_DUT_BEG <= ENREC.LD_ACT_DUT_BEG 
									AND BP.LD_ACT_DUT_BEG < BP.LD_LON_1_DSB
								THEN 
									CASE WHEN	ENREC.LD_ACT_DUT_BEG > ENREC.LD_LON_1_DSB
											THEN ENREC.LD_ACT_DUT_BEG
											ELSE BP.LD_ACT_DUT_BEG
									END
							 WHEN	ENREC.LD_ACT_DUT_BEG <= BP.LD_ACT_DUT_BEG 
									AND ENREC.LD_ACT_DUT_BEG < ENREC.LD_LON_1_DSB
								THEN 
									CASE WHEN	BP.LD_ACT_DUT_BEG > BP.LD_LON_1_DSB
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
					THEN ISNULL(ENREC.LD_ACT_DUT_END,'2099-12-31')
				 WHEN	BP.DF_SPE_ACC_ID IS NOT NULL 
						AND ENREC.LC_ACT_DUT = 'N'
						AND ENREC.LC_NTF_ACT_DUT = 'N'
						AND ENREC.LC_LFT_ACT_DUT = 'N'
					THEN ISNULL(BP.LD_ACT_DUT_END,'2099-12-31')
				 WHEN	BP.DF_SPE_ACC_ID IS NOT NULL 
						AND ISNULL(BP.LD_ACT_DUT_END,'2099-12-31') <= ISNULL(ENREC.LD_ACT_DUT_END,'2099-12-31')
					THEN ISNULL(ENREC.LD_ACT_DUT_END,'2099-12-31')
				 WHEN	BP.DF_SPE_ACC_ID IS NOT NULL 
						AND ISNULL(BP.LD_ACT_DUT_END,'2099-12-31') > ISNULL(ENREC.LD_ACT_DUT_END,'2099-12-31')
					THEN ISNULL(BP.LD_ACT_DUT_END,'2099-12-31')
					ELSE ISNULL(ENREC.LD_ACT_DUT_END,'2099-12-31')
			END,
			0,
			GETDATE(),
			1,
			CASE WHEN	BP.DF_SPE_ACC_ID IS NULL
					THEN 2
					ELSE 3 
			END, --endorser or both
			CASE WHEN	BP.DF_SPE_ACC_ID IS NULL --take endorser date if borrower is not military
					THEN ENREC.LD_NOTIFICATION_DATE
				 WHEN	BP.DF_SPE_ACC_ID IS NOT NULL --take the later of the 2 dates if both are military
					THEN 
						CASE WHEN	ENREC.LD_NOTIFICATION_DATE >= BP.LD_NOTIFICATION_DATE
								THEN ENREC.LD_NOTIFICATION_DATE
								ELSE BP.LD_NOTIFICATION_DATE
						END
					ELSE BP.LD_NOTIFICATION_DATE
			END,
			CASE WHEN	ENREC.LC_NTF_ACT_DUT = 'Y' AND BP.DF_SPE_ACC_ID IS NULL --Is reservist indicator
					THEN 1
				 WHEN	ENREC.LC_NTF_ACT_DUT = 'Y' AND BP.LC_NTF_ACT_DUT = 'Y' AND BP.DF_SPE_ACC_ID IS NOT NULL --Is reservist indicator
					THEN 1
					ELSE 0 
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
					LD_LON_1_DSB,
					LD_EID_BEGIN_DATE,
					LD_NOTIFICATION_DATE
				FROM
					CLS.scra._BorrowersPopulation
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
					THEN '2099-12-31'
				 WHEN	BP.LC_ACT_DUT = 'N'
						AND BP.LC_LFT_ACT_DUT = 'N'
						AND BP.LC_NTF_ACT_DUT = 'N'
						AND BP.LD_ACT_DUT_END IS NULL
					THEN AD.EndDate
					ELSE ISNULL(BP.LD_ACT_DUT_END,'2099-12-31')
			END
		),
		AD.IsBorrower= 1,
		AD.Active = 
		(
			CASE WHEN	R8.AccountNumber IS NOT NULL
					THEN 0
				 WHEN	R6.AccountNumber IS NOT NULL
					THEN 1
				WHEN	BP.LC_ACT_DUT = 'N' 
						AND BP.LC_LFT_ACT_DUT = 'Y'
						AND BP.LC_NTF_ACT_DUT = 'N'
					THEN 0 --Update UHEAA JW 3/30 
					ELSE AD.Active
			END
		),
		AD.BenefitSourceId = 1,
		AD.NotificationDate = BP.LD_NOTIFICATION_DATE,
		AD.IsReservist = 
		(
			CASE WHEN BP.LC_NTF_ACT_DUT = 'Y' --Is reservist indicator
					THEN 1 
					ELSE 0 
			END
		)
	FROM 
		CLS.scra.ActiveDuty AD
		INNER JOIN CLS.scra.Borrowers BOR
			ON BOR.BorrowerId = AD.BorrowerId
		LEFT OUTER JOIN CLS.scra._BorrowersPopulation BP
			ON BP.DF_SPE_ACC_ID = BOR.BorrowerAccountNumber
		LEFT OUTER JOIN CLS.scra._EndorsersPopulation ENREC
			ON ENREC.DF_SPE_ACC_ID = BOR.BorrowerAccountNumber
		LEFT OUTER JOIN @R8 R8 
			ON R8.AccountNumber = BOR.BorrowerAccountNumber
		LEFT OUTER JOIN @R9 R9 
			ON R9.AccountNumber = BOR.BorrowerAccountNumber
		LEFT OUTER JOIN @R6 R6 
			ON R6.AccountNumber = BOR.BorrowerAccountNumber
		LEFT OUTER JOIN @R7 R7 
			ON R7.AccountNumber = BOR.BorrowerAccountNumber
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
									AND BP.LD_ACT_DUT_BEG >= BP.LD_LON_1_DSB
								THEN BP.LD_ACT_DUT_BEG
							 WHEN	ENREC.LD_ACT_DUT_BEG <= BP.LD_ACT_DUT_BEG
									AND ENREC.LD_ACT_DUT_BEG >= ENREC.LD_LON_1_DSB
								THEN ENREC.LD_ACT_DUT_BEG
							 WHEN	BP.LD_ACT_DUT_BEG <= ENREC.LD_ACT_DUT_BEG 
									AND BP.LD_ACT_DUT_BEG < BP.LD_LON_1_DSB
								THEN 
									CASE WHEN	ENREC.LD_ACT_DUT_BEG > ENREC.LD_LON_1_DSB
											THEN ENREC.LD_ACT_DUT_BEG
											ELSE BP.LD_ACT_DUT_BEG
									END
							 WHEN	ENREC.LD_ACT_DUT_BEG <= BP.LD_ACT_DUT_BEG 
									AND ENREC.LD_ACT_DUT_BEG < ENREC.LD_LON_1_DSB
								THEN 
									CASE WHEN	BP.LD_ACT_DUT_BEG > BP.LD_LON_1_DSB
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
									AND BP.LD_ACT_DUT_BEG >= BP.LD_LON_1_DSB
								THEN BP.LD_ACT_DUT_BEG
							 WHEN	ENREC.LD_EID_BEGIN_DATE <= BP.LD_ACT_DUT_BEG 
									AND ENREC.LD_EID_BEGIN_DATE >= ENREC.LD_LON_1_DSB
								THEN ENREC.LD_EID_BEGIN_DATE
							 WHEN	BP.LD_ACT_DUT_BEG <= ENREC.LD_EID_BEGIN_DATE 
									AND BP.LD_ACT_DUT_BEG < BP.LD_LON_1_DSB
								THEN 
									CASE WHEN	ENREC.LD_EID_BEGIN_DATE > ENREC.LD_LON_1_DSB
											THEN ENREC.LD_EID_BEGIN_DATE
											ELSE BP.LD_ACT_DUT_BEG
									END
							 WHEN	ENREC.LD_EID_BEGIN_DATE <= BP.LD_ACT_DUT_BEG 
									AND ENREC.LD_EID_BEGIN_DATE < ENREC.LD_LON_1_DSB
								THEN 
									CASE WHEN	BP.LD_ACT_DUT_BEG > BP.LD_LON_1_DSB
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
									AND BP.LD_EID_BEGIN_DATE >= BP.LD_LON_1_DSB
								THEN BP.LD_EID_BEGIN_DATE
							 WHEN	ENREC.LD_ACT_DUT_BEG <= BP.LD_EID_BEGIN_DATE 
									AND ENREC.LD_ACT_DUT_BEG >= ENREC.LD_LON_1_DSB
								THEN ENREC.LD_ACT_DUT_BEG
							 WHEN	BP.LD_EID_BEGIN_DATE <= ENREC.LD_ACT_DUT_BEG 
									AND BP.LD_EID_BEGIN_DATE < BP.LD_LON_1_DSB
								THEN
									CASE WHEN	ENREC.LD_ACT_DUT_BEG > ENREC.LD_LON_1_DSB
											THEN ENREC.LD_ACT_DUT_BEG
											ELSE BP.LD_EID_BEGIN_DATE
									END
							 WHEN	ENREC.LD_ACT_DUT_BEG <= BP.LD_EID_BEGIN_DATE 
									AND ENREC.LD_ACT_DUT_BEG < ENREC.LD_LON_1_DSB
								THEN 
									CASE WHEN	BP.LD_EID_BEGIN_DATE > BP.LD_LON_1_DSB
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
									AND BP.LD_EID_BEGIN_DATE >= BP.LD_LON_1_DSB
								THEN BP.LD_EID_BEGIN_DATE
							 WHEN	ENREC.LD_EID_BEGIN_DATE <= BP.LD_EID_BEGIN_DATE 
									AND ENREC.LD_EID_BEGIN_DATE >= ENREC.LD_LON_1_DSB
								THEN ENREC.LD_EID_BEGIN_DATE
							 WHEN	BP.LD_EID_BEGIN_DATE <= ENREC.LD_EID_BEGIN_DATE 
									AND BP.LD_EID_BEGIN_DATE < BP.LD_LON_1_DSB
								THEN 
									CASE WHEN	ENREC.LD_EID_BEGIN_DATE > ENREC.LD_LON_1_DSB
											THEN ENREC.LD_EID_BEGIN_DATE
											ELSE BP.LD_EID_BEGIN_DATE
									END
							 WHEN	ENREC.LD_EID_BEGIN_DATE <= BP.LD_EID_BEGIN_DATE 
									AND ENREC.LD_EID_BEGIN_DATE < ENREC.LD_LON_1_DSB
								THEN 
									CASE WHEN	BP.LD_EID_BEGIN_DATE > BP.LD_LON_1_DSB
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
									AND BP.LD_EID_BEGIN_DATE >= BP.LD_LON_1_DSB
								THEN BP.LD_EID_BEGIN_DATE
							 WHEN	ENREC.LD_ACT_DUT_BEG <= BP.LD_EID_BEGIN_DATE 
									AND ENREC.LD_ACT_DUT_BEG >= ENREC.LD_LON_1_DSB
								THEN ENREC.LD_ACT_DUT_BEG
							 WHEN	BP.LD_EID_BEGIN_DATE <= ENREC.LD_ACT_DUT_BEG 
									AND BP.LD_EID_BEGIN_DATE < BP.LD_LON_1_DSB
								THEN 
									CASE WHEN	ENREC.LD_ACT_DUT_BEG > ENREC.LD_LON_1_DSB
											THEN ENREC.LD_ACT_DUT_BEG
											ELSE BP.LD_EID_BEGIN_DATE
									END
							 WHEN	ENREC.LD_ACT_DUT_BEG <= BP.LD_EID_BEGIN_DATE 
									AND ENREC.LD_ACT_DUT_BEG < ENREC.LD_LON_1_DSB
								THEN 
									CASE WHEN	BP.LD_EID_BEGIN_DATE > BP.LD_LON_1_DSB
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
									AND BP.LD_ACT_DUT_BEG >= BP.LD_LON_1_DSB
								THEN BP.LD_ACT_DUT_BEG
							 WHEN	ENREC.LD_EID_BEGIN_DATE <= BP.LD_ACT_DUT_BEG 
									AND ENREC.LD_EID_BEGIN_DATE >= ENREC.LD_LON_1_DSB
								THEN ENREC.LD_EID_BEGIN_DATE
							 WHEN	BP.LD_ACT_DUT_BEG <= ENREC.LD_EID_BEGIN_DATE 
									AND BP.LD_ACT_DUT_BEG < BP.LD_LON_1_DSB
								THEN 
									CASE WHEN	ENREC.LD_EID_BEGIN_DATE > ENREC.LD_LON_1_DSB
											THEN ENREC.LD_EID_BEGIN_DATE
											ELSE BP.LD_ACT_DUT_BEG
									END
							 WHEN	ENREC.LD_EID_BEGIN_DATE <= BP.LD_ACT_DUT_BEG 
									AND ENREC.LD_EID_BEGIN_DATE < ENREC.LD_LON_1_DSB
								THEN 
									CASE WHEN	BP.LD_ACT_DUT_BEG > BP.LD_LON_1_DSB
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
									AND BP.LD_ACT_DUT_BEG >= BP.LD_LON_1_DSB
								THEN BP.LD_ACT_DUT_BEG
							 WHEN	ENREC.LD_ACT_DUT_BEG <= BP.LD_ACT_DUT_BEG
									AND ENREC.LD_ACT_DUT_BEG >= ENREC.LD_LON_1_DSB
								THEN ENREC.LD_ACT_DUT_BEG
							 WHEN	BP.LD_ACT_DUT_BEG <= ENREC.LD_ACT_DUT_BEG 
									AND BP.LD_ACT_DUT_BEG < BP.LD_LON_1_DSB
								THEN 
									CASE WHEN	ENREC.LD_ACT_DUT_BEG > ENREC.LD_LON_1_DSB
											THEN ENREC.LD_ACT_DUT_BEG
											ELSE BP.LD_ACT_DUT_BEG
									END
							 WHEN	ENREC.LD_ACT_DUT_BEG <= BP.LD_ACT_DUT_BEG 
									AND ENREC.LD_ACT_DUT_BEG < ENREC.LD_LON_1_DSB
								THEN 
									CASE WHEN	BP.LD_ACT_DUT_BEG > BP.LD_LON_1_DSB
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
				 WHEN	BP.DF_SPE_ACC_ID IS NOT NULL --Update UHEAA JW 3/30
						AND BP.LC_ACT_DUT = 'N'
						AND BP.LC_LFT_ACT_DUT = 'N'
						AND BP.LC_NTF_ACT_DUT = 'N'
						AND ENREC.LC_NTF_ACT_DUT = 'Y' --Endorser Reservist
					THEN '2099-12-31'
				 WHEN	BP.DF_SPE_ACC_ID IS NOT NULL --Update UHEAA JW 3/30
						AND BP.LC_ACT_DUT = 'N'
						AND BP.LC_LFT_ACT_DUT = 'N'
						AND BP.LC_NTF_ACT_DUT = 'N'
						AND ENREC.LC_NTF_ACT_DUT = 'N' --Endorser not Reservist
					THEN ISNULL(ENREC.LD_ACT_DUT_END,'2099-12-31')
				 WHEN	BP.DF_SPE_ACC_ID IS NOT NULL --Update UHEAA JW 3/30
						AND ENREC.LC_ACT_DUT = 'N'
						AND ENREC.LC_LFT_ACT_DUT = 'N'
						AND ENREC.LC_NTF_ACT_DUT = 'N'
						AND BP.LC_NTF_ACT_DUT = 'Y'-- Borrower Reservist
					THEN '2099-12-31'
				 WHEN	BP.DF_SPE_ACC_ID IS NOT NULL --Update UHEAA JW 3/30
						AND ENREC.LC_ACT_DUT = 'N'
						AND ENREC.LC_LFT_ACT_DUT = 'N'
						AND ENREC.LC_NTF_ACT_DUT = 'N'
						AND BP.LC_NTF_ACT_DUT = 'N'-- Borrower not Reservist
					THEN ISNULL(BP.LD_ACT_DUT_END,'2099-12-31')
				 WHEN	BP.DF_SPE_ACC_ID IS NOT NULL 
						AND ISNULL(BP.LD_ACT_DUT_END,'2099-12-31') <= ISNULL(ENREC.LD_ACT_DUT_END,'2099-12-31') 
					THEN ISNULL(ENREC.LD_ACT_DUT_END,'2099-12-31')
				 WHEN	BP.DF_SPE_ACC_ID IS NOT NULL 
						AND ISNULL(BP.LD_ACT_DUT_END,'2099-12-31') > ISNULL(ENREC.LD_ACT_DUT_END,'2099-12-31')
					THEN ISNULL(BP.LD_ACT_DUT_END,'2099-12-31')
					ELSE ISNULL(ENREC.LD_ACT_DUT_END,'2099-12-31')
			END
		),
		AD.IsBorrower= 
		(
			CASE WHEN	ENREC.DF_SPE_ACC_ID IS NOT NULL --Update UHEAA JW 3/30
						AND ENREC.LC_ACT_DUT = 'N'
						AND ENREC.LC_NTF_ACT_DUT = 'N' 
					THEN 1 --Use Borrower as Non active military endorser
				 WHEN	ENREC.DF_SPE_ACC_ID IS NOT NULL
					THEN 0 --Use Endorser
					ELSE 1 --Use Borrower if no endorser present
			END
		),
		AD.Active= 
		(
			CASE WHEN	R8.AccountNumber IS NOT NULL 
						OR R9.AccountNumber IS NOT NULL
					THEN 0
				 WHEN	R6.AccountNumber IS NOT NULL 
						OR R7.AccountNumber IS NOT NULL
					THEN 1
				 WHEN   BP.LC_ACT_DUT = 'N'
                        AND BP.LC_LFT_ACT_DUT = 'N'
                        AND BP.LC_NTF_ACT_DUT = 'N'
                        AND ENREC.LC_ACT_DUT = 'N'
                        AND ENREC.LC_LFT_ACT_DUT = 'Y'
                        AND ENREC.LC_NTF_ACT_DUT = 'N'
					THEN 0 
                 WHEN   BP.LC_ACT_DUT = 'N'
                        AND BP.LC_LFT_ACT_DUT = 'Y'
                        AND BP.LC_NTF_ACT_DUT = 'N'
                        AND ENREC.LC_ACT_DUT = 'N'
                        AND ENREC.LC_LFT_ACT_DUT = 'N'
                        AND ENREC.LC_NTF_ACT_DUT = 'N'     
                    THEN 0 
					ELSE AD.Active
			END
		),
		AD.BenefitSourceId = 
		(
			CASE WHEN	BP.DF_SPE_ACC_ID IS NULL
						OR
						(
							BP.LC_ACT_DUT = 'N'
							AND BP.LC_LFT_ACT_DUT = 'N'
							AND BP.LC_NTF_ACT_DUT = 'N'
							AND
							(
								ENREC.LC_ACT_DUT ='X'
								OR ENREC.LC_NTF_ACT_DUT = 'Y'
							)
						) --Update UHEAA JW 3/30
					THEN 2
				 WHEN	ENREC.DF_SPE_ACC_ID IS NOT NULL
						AND ENREC.LC_ACT_DUT = 'N' --Update UHEAA JW 3/30
						AND ENREC.LC_NTF_ACT_DUT = 'N' 
					THEN 1
					ELSE 3 
			END
		), --endorser or both
		AD.NotificationDate = 
		(
			CASE WHEN	BP.DF_SPE_ACC_ID IS NULL --take endorser date if borrower is not military
						OR
						(
							BP.LC_ACT_DUT = 'N'
							AND BP.LC_LFT_ACT_DUT = 'N'
							AND BP.LC_NTF_ACT_DUT = 'N'
						) --Update UHEAA JW 3/30
					THEN ENREC.LD_NOTIFICATION_DATE
				 WHEN	BP.DF_SPE_ACC_ID IS NOT NULL --take the later of the 2 dates if both are military
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
						AND 
						(
							BP.DF_SPE_ACC_ID IS NULL --Is reservist indicator
							OR
							(
								BP.LC_ACT_DUT = 'N'
								AND BP.LC_LFT_ACT_DUT = 'N'
								AND BP.LC_NTF_ACT_DUT = 'N'
							) --Update UHEAA JW 3/30
						)
					THEN 1
				 WHEN	ENREC.LC_NTF_ACT_DUT = 'Y' 
						AND BP.LC_NTF_ACT_DUT = 'Y' 
						AND BP.DF_SPE_ACC_ID IS NOT NULL --Is reservist indicator
					THEN 1
					ELSE 0 
			END
		)
	FROM 
		CLS.scra.ActiveDuty AD
		INNER JOIN CLS.scra.Borrowers BOR
			ON BOR.BorrowerId = AD.BorrowerId
		LEFT OUTER JOIN CLS.scra._BorrowersPopulation BP
			ON BP.DF_SPE_ACC_ID = BOR.BorrowerAccountNumber
		LEFT OUTER JOIN CLS.scra._EndorsersPopulation ENREC
			ON ENREC.DF_SPE_ACC_ID = BOR.BorrowerAccountNumber
		LEFT OUTER JOIN @R8 R8 
			ON R8.AccountNumber = BOR.BorrowerAccountNumber
		LEFT OUTER JOIN @R9 R9 
			ON R9.AccountNumber = BOR.BorrowerAccountNumber
		LEFT OUTER JOIN @R6 R6 
			ON R6.AccountNumber = BOR.BorrowerAccountNumber
		LEFT OUTER JOIN @R7 R7 
			ON R7.AccountNumber = BOR.BorrowerAccountNumber
	WHERE	
		ENREC.DF_SPE_ACC_ID IS NOT NULL


	/********************************************************************
						TRANSACTION HANDLING
	********************************************************************/
	SELECT @ERROR = @ERROR + @@ERROR


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
			@R2

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
			@R3

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
			@R6

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
			@R7

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
			@R8

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
			@R9
	)


	/********************************************************************
						TRANSACTION HANDLING
	********************************************************************/
	SELECT @ERROR = @ERROR + @@ERROR


	DROP TABLE CLS.scra._BorrowersPopulation


	/********************************************************************
						TRANSACTION HANDLING
	********************************************************************/
	SELECT @ERROR = @ERROR + @@ERROR


	DROP TABLE CLS.scra._EndorsersPopulation


	/********************************************************************
						TRANSACTION HANDLING
	********************************************************************/
	SELECT @ERROR = @ERROR + @@ERROR


	IF @ERROR = 0
		BEGIN
			PRINT 'Transaction committed'
			COMMIT TRANSACTION
			--ROLLBACK TRANSACTION
		END
	ELSE
		BEGIN
			PRINT 'ERROR:  ' + CAST(@ERROR as VARCHAR(10))
			PRINT 'Transaction NOT committed'
			ROLLBACK TRANSACTION
		END

	END

END