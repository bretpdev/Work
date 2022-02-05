USE [ULS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[PopulateScraTables_UTLWS76]
AS
BEGIN
	IF 
		EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'scra' and TABLE_NAME = '_BorrowersPopulation_UTLWS76') 
		AND EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'scra' and TABLE_NAME = '_EndorsersPopulation_UTLWS76') 
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

	--BORROWERS pivot up to five begin/end dates for ARC comment:
	SELECT
		MULTI_BEG.*
		,MULTI_END.LD_ACT_DUT_END1
		,MULTI_END.LD_ACT_DUT_END2
		,MULTI_END.LD_ACT_DUT_END3
		,MULTI_END.LD_ACT_DUT_END4
		,MULTI_END.LD_ACT_DUT_END5
	INTO
		#MULTI_BORR
	FROM 
		(
			SELECT 
				DF_SPE_ACC_ID
				,[1] AS LD_ACT_DUT_BEG1
				,[2] AS LD_ACT_DUT_BEG2
				,[3] AS LD_ACT_DUT_BEG3
				,[4] AS LD_ACT_DUT_BEG4
				,[5] AS LD_ACT_DUT_BEG5
			FROM 
				(/*assigns #'s 1-5 to dates*/
					SELECT
						DF_SPE_ACC_ID
						,LD_ACT_DUT_BEG
						,ROW_NUMBER() OVER(PARTITION BY DF_SPE_ACC_ID ORDER BY DF_SPE_ACC_ID, LD_ACT_DUT_BEG) AS ROWNUM
					FROM 
						(
							SELECT DISTINCT
								DF_SPE_ACC_ID
								,CASE
									WHEN LC_NTF_ACT_DUT = 'Y'
										THEN LD_EID_BEGIN_DATE
									ELSE LD_ACT_DUT_BEG
								END AS LD_ACT_DUT_BEG
								,LD_ACT_DUT_END
							FROM
								ULS.scra._BorrowersPopulation_UTLWS76
							WHERE NOT /*excludes NNN borrowers*/
								(
									LC_ACT_DUT = 'N'
									AND LC_LFT_ACT_DUT = 'N'
									AND LC_NTF_ACT_DUT = 'N'
								)
						)NNN
				) A1
			PIVOT /*transposes column to row based on number assignment with #'s 1-5 as column names*/
				(MAX(LD_ACT_DUT_BEG) FOR ROWNUM IN 
					(
						[1],[2],[3],[4],[5]
					)
				) AS PVT1
		) MULTI_BEG
	INNER JOIN
		(
			SELECT 
				DF_SPE_ACC_ID
				,[1] AS LD_ACT_DUT_END1
				,[2] AS LD_ACT_DUT_END2
				,[3] AS LD_ACT_DUT_END3
				,[4] AS LD_ACT_DUT_END4
				,[5] AS LD_ACT_DUT_END5
			FROM 
				(/*assigns #'s 1-5 to dates*/
					SELECT
						DF_SPE_ACC_ID
						,LD_ACT_DUT_END
						,ROW_NUMBER() OVER(PARTITION BY DF_SPE_ACC_ID ORDER BY DF_SPE_ACC_ID, LD_ACT_DUT_BEG) AS ROWNUM
					FROM 
						(
							SELECT DISTINCT
								DF_SPE_ACC_ID
								,CASE
									WHEN LC_NTF_ACT_DUT = 'Y'
										THEN LD_EID_BEGIN_DATE
									ELSE LD_ACT_DUT_BEG
								END AS LD_ACT_DUT_BEG
								,LD_ACT_DUT_END
							FROM
								ULS.scra._BorrowersPopulation_UTLWS76
							WHERE NOT /*excludes NNN borrowers*/
								(
									LC_ACT_DUT = 'N'
									AND LC_LFT_ACT_DUT = 'N'
									AND LC_NTF_ACT_DUT = 'N'
								)
						)NNN
				) A2
			PIVOT /*transposes column to row based on number assignment with #'s 1-5 as column names*/
				(MAX(LD_ACT_DUT_END) FOR ROWNUM IN 
					(
						[1],[2],[3],[4],[5]
					)
				) AS PVT2
		) MULTI_END
	ON MULTI_BEG.DF_SPE_ACC_ID = MULTI_END.DF_SPE_ACC_ID;

	
	/*Borrower USCRA/MSCRA
	USCRA for updates on borrowers that already have SCRA database entry
	MSCRA for new borrowers being added to SCRA database*/
	INSERT INTO @R2
		SELECT DISTINCT
			2 AS ArcTypeId,
			NULL AS ArcResponseCodeId,
			BP.DF_SPE_ACC_ID AS AccountNumber,
			NULL AS RecipientId,
			'HSCRA' AS Arc,
			'UTLWS75' AS ScriptId,
			GETDATE() AS ProcessOn,
			CASE
				WHEN MB.LD_ACT_DUT_BEG2 IS NULL 
					AND MB.LD_ACT_DUT_BEG3 IS NULL 
					AND MB.LD_ACT_DUT_BEG4 IS NULL 
					AND MB.LD_ACT_DUT_BEG5 IS NULL
					THEN CONCAT('ADB: ', CONVERT(VARCHAR(10),MB.LD_ACT_DUT_BEG1,101),
								' ADE: ', COALESCE(CONVERT(VARCHAR(10),MB.LD_ACT_DUT_END1,101),'12/31/2099'))
				WHEN MB.LD_ACT_DUT_BEG3 IS NULL 
					AND MB.LD_ACT_DUT_BEG4 IS NULL 
					AND MB.LD_ACT_DUT_BEG5 IS NULL
					THEN CONCAT('ADB: ', CONVERT(VARCHAR(10),MB.LD_ACT_DUT_BEG1,101),
								' ADE: ', COALESCE(CONVERT(VARCHAR(10),MB.LD_ACT_DUT_END1,101),'12/31/2099'), ' |',
								' ADB: ', CONVERT(VARCHAR(10),MB.LD_ACT_DUT_BEG2,101),
								' ADE: ', COALESCE(CONVERT(VARCHAR(10),MB.LD_ACT_DUT_END2,101),'12/31/2099'))
				WHEN MB.LD_ACT_DUT_BEG4 IS NULL 
					AND MB.LD_ACT_DUT_BEG5 IS NULL
					THEN CONCAT('ADB: ', CONVERT(VARCHAR(10),MB.LD_ACT_DUT_BEG1,101),
								' ADE: ', COALESCE(CONVERT(VARCHAR(10),MB.LD_ACT_DUT_END1,101),'12/31/2099'), ' |',
								' ADB: ', CONVERT(VARCHAR(10),MB.LD_ACT_DUT_BEG2,101),
								' ADE: ', COALESCE(CONVERT(VARCHAR(10),MB.LD_ACT_DUT_END2,101),'12/31/2099'), ' |',
								' ADB: ', CONVERT(VARCHAR(10),MB.LD_ACT_DUT_BEG3,101),
								' ADE: ', COALESCE(CONVERT(VARCHAR(10),MB.LD_ACT_DUT_END3,101),'12/31/20199'))
				WHEN MB.LD_ACT_DUT_BEG5 IS NULL
					THEN CONCAT('ADB: ', CONVERT(VARCHAR(10),MB.LD_ACT_DUT_BEG1,101),
						' ADE: ', COALESCE(CONVERT(VARCHAR(10),MB.LD_ACT_DUT_END1,101),'12/31/2099'), ' |',
						' ADB: ', CONVERT(VARCHAR(10),MB.LD_ACT_DUT_BEG2,101),
						' ADE: ', COALESCE(CONVERT(VARCHAR(10),MB.LD_ACT_DUT_END2,101),'12/31/2099'), ' |',
						' ADB: ', CONVERT(VARCHAR(10),MB.LD_ACT_DUT_BEG3,101),
						' ADE: ', COALESCE(CONVERT(VARCHAR(10),MB.LD_ACT_DUT_END3,101),'12/31/2099'), ' |',
						' ADB: ', CONVERT(VARCHAR(10),MB.LD_ACT_DUT_BEG4,101),
						' ADE: ', COALESCE(CONVERT(VARCHAR(10),MB.LD_ACT_DUT_END4,101),'12/31/2099'))
				ELSE CONCAT('ADB: ', CONVERT(VARCHAR(10),MB.LD_ACT_DUT_BEG1,101),
						' ADE: ', COALESCE(CONVERT(VARCHAR(10),MB.LD_ACT_DUT_END1,101),'12/31/2099'), ' |',
						' ADB: ', CONVERT(VARCHAR(10),MB.LD_ACT_DUT_BEG2,101),
						' ADE: ', COALESCE(CONVERT(VARCHAR(10),MB.LD_ACT_DUT_END2,101),'12/31/2099'), ' |',
						' ADB: ', CONVERT(VARCHAR(10),MB.LD_ACT_DUT_BEG3,101),
						' ADE: ', COALESCE(CONVERT(VARCHAR(10),MB.LD_ACT_DUT_END3,101),'12/31/2099'), ' |',
						' ADB: ', CONVERT(VARCHAR(10),MB.LD_ACT_DUT_BEG4,101),
						' ADE: ', COALESCE(CONVERT(VARCHAR(10),MB.LD_ACT_DUT_END4,101),'12/31/2099'), ' |',
						' ADB: ', CONVERT(VARCHAR(10),MB.LD_ACT_DUT_BEG5,101),
						' ADE: ', COALESCE(CONVERT(VARCHAR(10),MB.LD_ACT_DUT_END5,101),'12/31/2099'))
			END AS Comment,
			0 AS IsReference,
			0 AS IsEndorser,
			NULL AS ProcessFrom,
			NULL AS ProcessTo,
			NULL AS NeededBy,
			NULL AS RegardsTo,
			NULL AS RegardsCode,
			GETDATE() AS CreatedAt,
			'UTLWS75' AS CreatedBy,
			NULL AS ProcessedAt
		FROM
			ULS.scra._BorrowersPopulation_UTLWS76 BP
			LEFT JOIN #MULTI_BORR MB
				ON MB.DF_SPE_ACC_ID = BP.DF_SPE_ACC_ID
			LEFT JOIN ULS.scra.Borrowers MIL
				ON MIL.BorrowerAccountNumber = BP.DF_SPE_ACC_ID
			LEFT JOIN ULS.scra.ActiveDuty AD
				ON AD.BorrowerId = MIL.BorrowerId
			LEFT JOIN UDW..PD10_PRS_NME PD10
				ON PD10.DF_SPE_ACC_ID = BP.DF_SPE_ACC_ID
			LEFT JOIN 
			(
				SELECT DISTINCT
					BF_SSN
				FROM
					UDW..LN72_INT_RTE_HST
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
				(   --exclude reservists where loans disburse after active duty
					BP.LD_LON_1_DSB > ISNULL(BP.LD_EID_BEGIN_DATE,'2099-12-31') 
					AND BP.LC_NTF_ACT_DUT = 'Y'
				)
			)
			AND NOT 
			(   --non active duty borrowers
				BP.LC_ACT_DUT = 'N'
				AND BP.LC_LFT_ACT_DUT = 'N'
				AND BP.LC_NTF_ACT_DUT = 'N'
			)
			AND 
			(
				(   --active duty not receiving interest benefit
					BP.LR_ITR > 6 
					AND BP.LC_ACT_DUT IN ('X','Y')
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
						OR AD.EndDate != COALESCE(BP.LD_ACT_DUT_END ,'2099-12-31')
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
				OR
				(
					BP.LC_ACT_DUT = 'N'
					AND BP.LC_LFT_ACT_DUT = 'Y'
					AND BP.LC_NTF_ACT_DUT = 'N'	
				)
				)
			);

	DROP TABLE #MULTI_BORR;

	/********************************************************************
						TRANSACTION HANDLING
	********************************************************************/
	SELECT @ERROR = @@ERROR

	--ENDORSERS pivot up to five begin/end dates for ARC comment:
	SELECT
		MULTI_BEG.*
		,MULTI_END.LD_ACT_DUT_END1
		,MULTI_END.LD_ACT_DUT_END2
		,MULTI_END.LD_ACT_DUT_END3
		,MULTI_END.LD_ACT_DUT_END4
		,MULTI_END.LD_ACT_DUT_END5
	INTO
		#MULTI_ENDOR
	FROM 
		(
			SELECT 
				DF_SPE_ACC_ID
				,[1] AS LD_ACT_DUT_BEG1
				,[2] AS LD_ACT_DUT_BEG2
				,[3] AS LD_ACT_DUT_BEG3
				,[4] AS LD_ACT_DUT_BEG4
				,[5] AS LD_ACT_DUT_BEG5
			FROM 
				(/*assigns #'s 1-5 to dates*/
					SELECT
						DF_SPE_ACC_ID
						,LD_ACT_DUT_BEG
						,ROW_NUMBER() OVER(PARTITION BY DF_SPE_ACC_ID ORDER BY DF_SPE_ACC_ID, LD_ACT_DUT_BEG) AS ROWNUM
					FROM 
						(
							SELECT DISTINCT
								DF_SPE_ACC_ID
								,CASE
									WHEN LC_NTF_ACT_DUT = 'Y'
										THEN LD_EID_BEGIN_DATE
									ELSE LD_ACT_DUT_BEG
								END AS LD_ACT_DUT_BEG
								,LD_ACT_DUT_END
							FROM
								ULS.scra._EndorsersPopulation_UTLWS76
							WHERE NOT /*excludes NNN endorsers*/
								(
									LC_ACT_DUT = 'N'
									AND LC_LFT_ACT_DUT = 'N'
									AND LC_NTF_ACT_DUT = 'N'
								)
						)NNN
				) A1
			PIVOT /*transposes column to row based on number assignment with #'s 1-5 as column names*/
				(MAX(LD_ACT_DUT_BEG) FOR ROWNUM IN 
					(
						[1],[2],[3],[4],[5]
					)
				) AS PVT1
		) MULTI_BEG
	INNER JOIN
		(
			SELECT 
				DF_SPE_ACC_ID
				,[1] AS LD_ACT_DUT_END1
				,[2] AS LD_ACT_DUT_END2
				,[3] AS LD_ACT_DUT_END3
				,[4] AS LD_ACT_DUT_END4
				,[5] AS LD_ACT_DUT_END5
			FROM 
				(/*assigns #'s 1-5 to dates*/
					SELECT
						DF_SPE_ACC_ID
						,LD_ACT_DUT_END
						,ROW_NUMBER() OVER(PARTITION BY DF_SPE_ACC_ID ORDER BY DF_SPE_ACC_ID, LD_ACT_DUT_BEG) AS ROWNUM
					FROM 
						(
							SELECT DISTINCT
								DF_SPE_ACC_ID
								,CASE
									WHEN LC_NTF_ACT_DUT = 'Y'
										THEN LD_EID_BEGIN_DATE
									ELSE LD_ACT_DUT_BEG
								END AS LD_ACT_DUT_BEG
								,LD_ACT_DUT_END
							FROM
								ULS.scra._EndorsersPopulation_UTLWS76
							WHERE NOT /*excludes NNN borrowers*/
								(
									LC_ACT_DUT = 'N'
									AND LC_LFT_ACT_DUT = 'N'
									AND LC_NTF_ACT_DUT = 'N'
								)
						)NNN
				) A2
			PIVOT /*transposes column to row based on number assignment with #'s 1-5 as column names*/
				(MAX(LD_ACT_DUT_END) FOR ROWNUM IN 
					(
						[1],[2],[3],[4],[5]
					)
				) AS PVT2
		) MULTI_END
	ON MULTI_BEG.DF_SPE_ACC_ID = MULTI_END.DF_SPE_ACC_ID;

	--Endorser USCRA/MSCRA
	INSERT INTO @R3
		SELECT DISTINCT
			4 AS ArcTypeId,
			NULL AS ArcResponseCodeId,
			EP.DF_SPE_ACC_ID AS AccountNumber,
			NULL AS RecipientId,
			'HSCRA' AS Arc,
			'UTLWS75' AS ScriptId,
			GETDATE() AS ProcessOn,
			--CONCAT('Active Duty Begin Date: ', CONVERT(VARCHAR(10),EP.LD_ACT_DUT_BEG,101), ' Active Duty End Date: ', COALESCE(CONVERT(VARCHAR(10),EP.LD_ACT_DUT_END,101),'12/31/2099')) AS Comment,
			CASE
				WHEN ME.LD_ACT_DUT_BEG2 IS NULL 
					AND ME.LD_ACT_DUT_BEG3 IS NULL 
					AND ME.LD_ACT_DUT_BEG4 IS NULL 
					AND ME.LD_ACT_DUT_BEG5 IS NULL
					THEN CONCAT('ADB: ', CONVERT(VARCHAR(10),ME.LD_ACT_DUT_BEG1,101),
								' ADE: ', COALESCE(CONVERT(VARCHAR(10),ME.LD_ACT_DUT_END1,101),'12/31/2099'))
				WHEN ME.LD_ACT_DUT_BEG3 IS NULL 
					AND ME.LD_ACT_DUT_BEG4 IS NULL 
					AND ME.LD_ACT_DUT_BEG5 IS NULL
					THEN CONCAT('ADB: ', CONVERT(VARCHAR(10),ME.LD_ACT_DUT_BEG1,101),
								' ADE: ', COALESCE(CONVERT(VARCHAR(10),ME.LD_ACT_DUT_END1,101),'12/31/2099'), ' |',
								' ADB: ', CONVERT(VARCHAR(10),ME.LD_ACT_DUT_BEG2,101),
								' ADE: ', COALESCE(CONVERT(VARCHAR(10),ME.LD_ACT_DUT_END2,101),'12/31/2099'))
				WHEN ME.LD_ACT_DUT_BEG4 IS NULL 
					AND ME.LD_ACT_DUT_BEG5 IS NULL
					THEN CONCAT('ADB: ', CONVERT(VARCHAR(10),ME.LD_ACT_DUT_BEG1,101),
								' ADE: ', COALESCE(CONVERT(VARCHAR(10),ME.LD_ACT_DUT_END1,101),'12/31/2099'), ' |',
								' ADB: ', CONVERT(VARCHAR(10),ME.LD_ACT_DUT_BEG2,101),
								' ADE: ', COALESCE(CONVERT(VARCHAR(10),ME.LD_ACT_DUT_END2,101),'12/31/2099'), ' |',
								' ADB: ', CONVERT(VARCHAR(10),ME.LD_ACT_DUT_BEG3,101),
								' ADE: ', COALESCE(CONVERT(VARCHAR(10),ME.LD_ACT_DUT_END3,101),'12/31/20199'))
				WHEN ME.LD_ACT_DUT_BEG5 IS NULL
					THEN CONCAT('ADB: ', CONVERT(VARCHAR(10),ME.LD_ACT_DUT_BEG1,101),
						' ADE: ', COALESCE(CONVERT(VARCHAR(10),ME.LD_ACT_DUT_END1,101),'12/31/2099'), ' |',
						' ADB: ', CONVERT(VARCHAR(10),ME.LD_ACT_DUT_BEG2,101),
						' ADE: ', COALESCE(CONVERT(VARCHAR(10),ME.LD_ACT_DUT_END2,101),'12/31/2099'), ' |',
						' ADB: ', CONVERT(VARCHAR(10),ME.LD_ACT_DUT_BEG3,101),
						' ADE: ', COALESCE(CONVERT(VARCHAR(10),ME.LD_ACT_DUT_END3,101),'12/31/2099'), ' |',
						' ADB: ', CONVERT(VARCHAR(10),ME.LD_ACT_DUT_BEG4,101),
						' ADE: ', COALESCE(CONVERT(VARCHAR(10),ME.LD_ACT_DUT_END4,101),'12/31/2099'))
				ELSE CONCAT('ADB: ', CONVERT(VARCHAR(10),ME.LD_ACT_DUT_BEG1,101),
						' ADE: ', COALESCE(CONVERT(VARCHAR(10),ME.LD_ACT_DUT_END1,101),'12/31/2099'), ' |',
						' ADB: ', CONVERT(VARCHAR(10),ME.LD_ACT_DUT_BEG2,101),
						' ADE: ', COALESCE(CONVERT(VARCHAR(10),ME.LD_ACT_DUT_END2,101),'12/31/2099'), ' |',
						' ADB: ', CONVERT(VARCHAR(10),ME.LD_ACT_DUT_BEG3,101),
						' ADE: ', COALESCE(CONVERT(VARCHAR(10),ME.LD_ACT_DUT_END3,101),'12/31/2099'), ' |',
						' ADB: ', CONVERT(VARCHAR(10),ME.LD_ACT_DUT_BEG4,101),
						' ADE: ', COALESCE(CONVERT(VARCHAR(10),ME.LD_ACT_DUT_END4,101),'12/31/2099'), ' |',
						' ADB: ', CONVERT(VARCHAR(10),ME.LD_ACT_DUT_BEG5,101),
						' ADE: ', COALESCE(CONVERT(VARCHAR(10),ME.LD_ACT_DUT_END5,101),'12/31/2099'))
			END AS Comment,
			0 AS IsReference,
			1 AS IsEndorser,
			NULL AS ProcessFrom,
			NULL AS ProcessTo,
			NULL AS NeededBy,
			EP.LF_EDS AS RegardsTo,
			'E' AS RegardsCode,
			GETDATE() AS CreatedAt,
			'UTLWS75' AS CreatedBy,
			NULL AS ProcessedAt
		FROM
			ULS.scra._EndorsersPopulation_UTLWS76 EP
			LEFT JOIN #MULTI_ENDOR ME
				ON ME.DF_SPE_ACC_ID = EP.DF_SPE_ACC_ID
			LEFT JOIN ULS.scra.Borrowers MIL
				ON MIL.BorrowerAccountNumber = EP.DF_SPE_ACC_ID
			LEFT JOIN ULS.scra.ActiveDuty AD
				ON AD.BorrowerId = MIL.BorrowerId
			LEFT JOIN UDW..PD10_PRS_NME PD10
				ON PD10.DF_SPE_ACC_ID = EP.DF_SPE_ACC_ID
			LEFT JOIN 
			(
				SELECT DISTINCT
					BF_SSN
				FROM
					UDW..LN72_INT_RTE_HST
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
					AND EP.LC_ACT_DUT IN ('X','Y')
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
				OR
				(
					EP.LC_ACT_DUT = 'N'
					AND EP.LC_LFT_ACT_DUT = 'Y'
					AND EP.LC_NTF_ACT_DUT = 'N'	
				)
				)
			);

	DROP TABLE #MULTI_ENDOR;

	/********************************************************************
						TRANSACTION HANDLING
	********************************************************************/
	SELECT @ERROR = @ERROR + @@ERROR

	--ADD ARCS TO ARC ADD
	INSERT INTO	ULS.dbo.ARCADDPROCESSING
	(
		ARCTYPEID,
		ACCOUNTNUMBER,
		RECIPIENTID,
		ARC,
		SCRIPTID,
		PROCESSON,
		COMMENT,
		ISREFERENCE,
		ISENDORSER,
		PROCESSFROM,
		PROCESSTO,
		NEEDEDBY,
		REGARDSTO,
		REGARDSCODE,
		CREATEDAT,
		CREATEDBY
	)
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
	)

	/********************************************************************
						TRANSACTION HANDLING
	********************************************************************/
	SELECT @ERROR = @ERROR + @@ERROR

	--ADD TO SSRS
	INSERT INTO	ULS.scra.SSRS_UTLWS76
	(
		[DF_SPE_ACC_ID]
		,[END_ACC_ID]
		,[LD_ACT_DUT_BEG]
		,[LD_ACT_DUT_END]
		,[PAID_IN_FULL_DATE]
		,[DECONVERTED_DATE]
	)
	(
		SELECT DISTINCT
			[DF_SPE_ACC_ID]
			,[END_ACC_ID]
			,COALESCE([LD_ACT_DUT_BEG], [LD_EID_BEGIN_DATE]) AS LD_ACT_DUT_BEG
			,COALESCE([LD_ACT_DUT_END], '2099-12-31') AS LD_ACT_DUT_END
			,MAX([PAID_IN_FULL_DATE]) AS PAID_IN_FULL_DATE
			,MAX([DECONVERTED_DATE]) AS DECONVERTED_DATE
		FROM
			ULS.scra._SSRS_UTLWS76
		GROUP BY
			[DF_SPE_ACC_ID]
			,[END_ACC_ID]
			,COALESCE([LD_ACT_DUT_BEG], [LD_EID_BEGIN_DATE])
			,[LD_ACT_DUT_END]			
	)

	/********************************************************************
						TRANSACTION HANDLING
	********************************************************************/
	SELECT @ERROR = @ERROR + @@ERROR

	DROP TABLE ULS.scra._BorrowersPopulation_UTLWS76

	SELECT @ERROR = @ERROR + @@ERROR

	DROP TABLE ULS.scra._EndorsersPopulation_UTLWS76

	SELECT @ERROR = @ERROR + @@ERROR

	DROP TABLE ULS.scra._SSRS_UTLWS76

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
