BEGIN TRY
	BEGIN TRANSACTION

		DROP TABLE IF EXISTS #ONELINK;
		DROP TABLE IF EXISTS #COMPASS;
		DROP TABLE IF EXISTS #ONELINK_COMMENT;
		DROP TABLE IF EXISTS #COMPASS_COMMENT;

		DECLARE @AddedAt DATETIME = GETDATE();
		DECLARE @Today DATE = @AddedAt;
		DECLARE @QueName VARCHAR(10) = 'KOTHDEMO';
		DECLARE @ScriptId VARCHAR(10) = 'UTLWK04';

	--#ONELINK populate master OneLINK table (OLINK SAS dataset)
		SELECT DISTINCT 
			PD01.DF_PRS_ID,
			--ADDRESS INFO
			PD03.DC_ADR,
			PD03.DI_VLD_ADR,
			PD03.DF_ZIP,
			PD03.DM_CT,
			PD03.DX_STR_ADR_1,
			PD03.DX_STR_ADR_2,
			PD03.DC_DOM_ST,
			PD03.DM_FGN_CNY,
			--PHONE INFO
			PD03.DI_PHN_VLD,
			PD03.DN_PHN,
			PD03.DI_ALT_PHN_VLD,
			PD03.DN_ALT_PHN,
			MAX_SKIP.DD_LST_UPD_ADR
		INTO
			#ONELINK
		FROM
			ODW..PD01_PDM_INF PD01
			INNER JOIN 
			( --Gets any borrower that does not have a valid legal address
				SELECT
					DF_PRS_ID,
					MAX(DD_LST_UPD_ADR) AS DD_LST_UPD_ADR
				FROM
					ODW..PD03_PRS_ADR_PHN
				WHERE
					DC_ADR = 'L'
				GROUP BY
					DF_PRS_ID
			) MAX_SKIP
				ON MAX_SKIP.DF_PRS_ID = PD01.DF_PRS_ID
			INNER JOIN ODW..PD03_PRS_ADR_PHN PD03
				ON PD03.DF_PRS_ID = PD01.DF_PRS_ID
			INNER JOIN ODW..GA01_APP GA01
				ON GA01.DF_PRS_ID_BR = PD01.DF_PRS_ID
			INNER JOIN ODW..GA10_LON_APP GA10
				ON GA10.AF_APL_ID = GA01.AF_APL_ID
				AND GA10.AA_CUR_PRI > 0.00	
			LEFT JOIN --DC01
			(
				SELECT
					DC01_DET.AF_APL_ID,
					DC01_DET.AF_APL_ID_SFX,
					DC01_DET.LC_STA_DC10
				FROM
					ODW..DC01_LON_CLM_INF DC01_DET
				--MAX_DC01: get most recent DC01 record (by LF_CRT_DTS_DC10)
					INNER JOIN 
					(
						SELECT
							CLM_INF.AF_APL_ID,
							CLM_INF.AF_APL_ID_SFX,
							MAX(CLM_INF.LF_CRT_DTS_DC10) AS LF_CRT_DTS_DC10
						FROM
							ODW..DC01_LON_CLM_INF CLM_INF
						GROUP BY
							CLM_INF.AF_APL_ID,
							CLM_INF.AF_APL_ID_SFX
					) MAX_DC01
						ON MAX_DC01.AF_APL_ID = DC01_DET.AF_APL_ID
						AND MAX_DC01.AF_APL_ID_SFX = DC01_DET.AF_APL_ID_SFX
						AND MAX_DC01.LF_CRT_DTS_DC10 = DC01_DET.LF_CRT_DTS_DC10
			) DC01
				ON DC01.AF_APL_ID = GA10.AF_APL_ID
				AND DC01.AF_APL_ID_SFX = GA10.AF_APL_ID_SFX
			INNER JOIN
			(
				SELECT
					GA14.AF_APL_ID,
					GA14.AF_APL_ID_SFX,
					GA14.AF_CRT_DTS_GA14
				FROM
					ODW..GA14_LON_STA GA14
					INNER JOIN
					(
						SELECT
							AF_APL_ID,
							AF_APL_ID_SFX,
							MAX(AF_CRT_DTS_GA14) AS AF_CRT_DTS_GA14
						FROM
							ODW..GA14_LON_STA
						GROUP BY
							AF_APL_ID,
							AF_APL_ID_SFX
					) MaxGA14
						ON MaxGA14.AF_APL_ID = GA14.AF_APL_ID
						AND MaxGA14.AF_APL_ID_SFX = GA14.AF_APL_ID_SFX
						AND MaxGA14.AF_CRT_DTS_GA14 = GA14.AF_CRT_DTS_GA14
				WHERE
					GA14.AC_STA_GA14 = 'A'
					AND GA14.AC_LON_STA_TYP != 'DN'
			) GA14
				ON GA14.AF_APL_ID = GA10.AF_APL_ID
				AND GA14.AF_APL_ID_SFX = GA10.AF_APL_ID_SFX
			LEFT JOIN ODW..CT30_CALL_QUE CT30
				ON CT30.DF_PRS_ID_BR = PD01.DF_PRS_ID
				AND CT30.IC_TSK_STA IN ('A','W') --Active / working statuses
				AND CT30.IF_WRK_GRP = @QueName
			LEFT JOIN ODW..AY01_BR_ATY AY01
				ON AY01.DF_PRS_ID = PD01.DF_PRS_ID
				AND AY01.PF_ACT = 'KOTHR'
				AND AY01.BD_ATY_PRF >= DATEADD(DAY, -45, @Today)
		WHERE 
			GA10.AC_GTE_TRF != 'O'
			AND DC01.LC_STA_DC10 IN (1,2,4)
			AND CT30.DF_PRS_ID_BR IS NULL
			AND AY01.DF_PRS_ID IS NULL;

	--#COMPASS: populate master COMPASS table
		SELECT DISTINCT 
			PD10.DF_PRS_ID,
			--ADDRESS INFO
			PD30.DC_ADR,
			PD30.DI_VLD_ADR,
			PD30.DF_ZIP_CDE,
			PD30.DM_CT,
			PD30.DX_STR_ADR_1,
			PD30.DX_STR_ADR_2,
			PD30.DX_STR_ADR_3,
			PD30.DC_DOM_ST,
			PD30.DM_FGN_CNY,
			PD30.DM_FGN_ST
		INTO
			#COMPASS
		FROM 
			UDW..PD10_PRS_NME PD10
			INNER JOIN ODW..PD01_PDM_INF PD01
				ON PD01.DF_PRS_ID = PD10.DF_PRS_ID --Make sure to remove any non uheaa borrower (bana, wyoming)
			INNER JOIN
			(
				SELECT
					PD30.DF_PRS_ID,
					MAX_DATE.DD_STA_PDEM30
				FROM
					UDW..PD30_PRS_ADR PD30
					INNER JOIN
					(
						SELECT
							DF_PRS_ID,
							MAX(DD_STA_PDEM30) AS DD_STA_PDEM30
						FROM
							UDW..PD30_PRS_ADR
						GROUP BY
							DF_PRS_ID
					) MAX_DATE
						ON MAX_DATE.DF_PRS_ID = PD30.DF_PRS_ID
						AND MAX_DATE.DD_STA_PDEM30 = PD30.DD_STA_PDEM30
				WHERE
					PD30.DI_VLD_ADR = 'N'
					AND PD30.DC_ADR = 'L'
			) MAX_PD30
				ON MAX_PD30.DF_PRS_ID = PD10.DF_PRS_ID
			INNER JOIN UDW..PD30_PRS_ADR PD30
				ON PD30.DF_PRS_ID = MAX_PD30.DF_PRS_ID
			INNER JOIN UDW..LN10_LON LN10
				ON LN10.BF_SSN = PD10.DF_PRS_ID
			LEFT JOIN ODW..CT30_CALL_QUE CT30
				ON CT30.DF_PRS_ID_BR = PD10.DF_PRS_ID
				AND CT30.IF_WRK_GRP = @QueName
			LEFT JOIN UDW..AY10_BR_LON_ATY AY10
				ON AY10.BF_SSN = PD10.DF_PRS_ID
				AND AY10.PF_REQ_ACT = 'KOTHR'
				AND AY10.LD_ATY_REQ_RCV >= DATEADD(DAY, -45, @Today)
		WHERE 
			LN10.LC_STA_LON10 = 'R'
			AND LN10.LA_CUR_PRI > 0.00  
			AND CT30.DF_PRS_ID_BR IS NULL
			AND AY10.BF_SSN IS NULL;


/***************************************************************/


	--#ONELINK_COMMENT: compile ONELINK comment from address and phone information
		SELECT
			ISNULL(OLADD.DF_PRS_ID,OLPHN.DF_PRS_ID) AS DF_PRS_ID,
			CASE
				WHEN OLADD.DF_PRS_ID IS NOT NULL AND OLPHN.DF_PRS_ID IS NOT NULL THEN CONCAT(OLADD.OLAADD,',',OLPHN.OLAPHN)
				WHEN OLADD.DF_PRS_ID IS NOT NULL AND OLPHN.DF_PRS_ID IS NULL THEN OLADD.OLAADD
				WHEN OLADD.DF_PRS_ID IS NULL AND OLPHN.DF_PRS_ID IS NOT NULL THEN OLPHN.OLAPHN
				ELSE ''
			END AS OLCOMMENT,
			DD_LST_UPD_ADR
		INTO
			#ONELINK_COMMENT
		FROM
		--OLADD: OneLINK address information A
			(
				SELECT DISTINCT 
					OALT.DF_PRS_ID,
					CONCAT('ONELINK',',',RTRIM(LTRIM(OALT.DC_ADR)),',',RTRIM(LTRIM(OALT.DX_STR_ADR_1)),',',RTRIM(LTRIM(OALT.DX_STR_ADR_2)),',',RTRIM(LTRIM(OALT.DM_CT)),',',RTRIM(LTRIM(OALT.DC_DOM_ST)),',',RTRIM(LTRIM(OALT.DF_ZIP)),',',RTRIM(LTRIM(OALT.DM_FGN_CNY))) AS OLAADD,
					DD_LST_UPD_ADR
				FROM
				--OLEGAL: all OneLINK accounts with an invalid (skip) legal address
					(
						SELECT 
							DF_PRS_ID,
							DC_ADR,
							DI_VLD_ADR
						FROM
							#ONELINK
						WHERE
							DC_ADR='L' 
							AND DI_VLD_ADR='N'
					) OLEGAL --A
				--OALT: OneLINK alternate address information
					INNER JOIN
					(
						SELECT
							DF_PRS_ID,
							DC_ADR,
							DI_VLD_ADR,
							DF_ZIP,
							DM_CT,
							DX_STR_ADR_1,
							DX_STR_ADR_2,
							DC_DOM_ST,
							DM_FGN_CNY,
							DD_LST_UPD_ADR
						FROM
							#ONELINK
						WHERE
							DC_ADR IN ('A','I','T')
							AND DI_VLD_ADR = 'Y' 
							AND DX_STR_ADR_1 != ''
					) OALT --B
						ON OLEGAL.DF_PRS_ID = OALT.DF_PRS_ID
			) OLADD --A
		----OLPHN: OneLINK phone information B
			FULL OUTER JOIN
			(
				SELECT DISTINCT
					OAPHN.DF_PRS_ID,
					CONCAT('ONELINK',',','A',',',RTRIM(LTRIM(OAPHN.DN_ALT_PHN))) AS OLAPHN 
				FROM
				--OHPHN: : all OneLINK accounts with an invalid (skip) phone
					(
						SELECT
							DF_PRS_ID,
							DI_PHN_VLD 
						FROM
							#ONELINK
						WHERE
							DC_ADR = 'L'
							AND DI_PHN_VLD = 'N' 
					) OHPHN --A
				--OAPHN: OneLINK alternate phone information
					INNER JOIN
					(
						SELECT
							DF_PRS_ID,
							DN_ALT_PHN,
							DI_ALT_PHN_VLD
						FROM
							#ONELINK
						WHERE
							DC_ADR = 'L'
							AND DN_ALT_PHN != ''
		 					AND DI_ALT_PHN_VLD = 'Y'
					) OAPHN  --B
						ON OAPHN.DF_PRS_ID = OHPHN.DF_PRS_ID
			) OLPHN --B
				ON OLADD.DF_PRS_ID = OLPHN.DF_PRS_ID;



	--#COMPASS_COMMENT: compile COMPASS comment from address
		SELECT
			COADD.DF_PRS_ID AS DF_PRS_ID,
			CASE
				WHEN COADD.DF_PRS_ID IS NOT NULL THEN COADD.COAADD
				ELSE ''
			END AS COCOMMENT
		INTO
			#COMPASS_COMMENT
		FROM
			(
			--COADD: compass address
				SELECT DISTINCT
					CALT.DF_PRS_ID,
					CONCAT('COMPASS',',',RTRIM(LTRIM(CALT.DC_ADR)),',',RTRIM(LTRIM(CALT.DX_STR_ADR_1)),',',RTRIM(LTRIM(CALT.DX_STR_ADR_2)),',',RTRIM(LTRIM(CALT.DX_STR_ADR_3)),',',RTRIM(LTRIM(CALT.DM_CT)),',',RTRIM(LTRIM(CALT.DC_DOM_ST)),',',RTRIM(LTRIM(CALT.DF_ZIP_CDE)),',',RTRIM(LTRIM(CALT.DM_FGN_CNY))) AS COAADD 
				FROM
				--CLEGAL: compass borrowers with an invalid (skip) legal address A
					(
						SELECT
							DF_PRS_ID,
							DC_ADR,
							DI_VLD_ADR
						FROM
							#COMPASS
						 WHERE
							DC_ADR = 'L'
							AND DI_VLD_ADR = 'N'
					) CLEGAL --A
				--CALT B
					INNER JOIN
					(
						SELECT
							DF_PRS_ID,
							DC_ADR,
							DI_VLD_ADR,
							DF_ZIP_CDE,
							DM_CT,
							DX_STR_ADR_1,
							DX_STR_ADR_2,
							DX_STR_ADR_3,
							DC_DOM_ST,
							DM_FGN_CNY
						FROM
							#COMPASS
						WHERE
							DC_ADR IN ('D','B')
							AND DI_VLD_ADR = 'Y' 
							AND DX_STR_ADR_1 != ''
					) CALT --B
						ON CALT.DF_PRS_ID = CLEGAL.DF_PRS_ID
			) COADD --A

		--SELECT * FROM #COMPASS_COMMENT --for testing

					
		INSERT INTO OLS.olqtskbldr.Queues --TODO: restore insert for production
					(
						TargetId,
						QueueName,
						InstitutionId,
						InstitutionType,
						DateDue,
						TimeDue,
						Comment,
						SourceFilename,
						AddedAt,
						AddedBy
					)
				SELECT DISTINCT
					NEW_DATA.TargetId	AS TargetId,
					@QueName	AS QueueName,
					''			AS InstitutionId,
					''			AS InstitutionType,
					NULL		AS DateDue,
					NULL		AS TimeDue,
					NEW_DATA.Comment		AS Comment,
					''			AS SourceFilename,
					@AddedAt	AS AddedAt,
					@ScriptId	AS AddedBy
				FROM
					(
						SELECT
							ISNULL(OLCOM.DF_PRS_ID, COCOM.DF_PRS_ID) AS TargetId,
							CASE
								WHEN OLCOM.DF_PRS_ID IS NOT NULL AND COCOM.DF_PRS_ID IS NOT NULL THEN CONCAT(OLCOM.OLCOMMENT,',',COCOM.COCOMMENT)
								WHEN OLCOM.DF_PRS_ID IS NOT NULL AND COCOM.DF_PRS_ID IS NULL     THEN OLCOM.OLCOMMENT
								WHEN OLCOM.DF_PRS_ID IS NULL     AND COCOM.DF_PRS_ID IS NOT NULL THEN COCOM.COCOMMENT
								ELSE ''
							END AS Comment,
							OLCOM.DD_LST_UPD_ADR
						FROM
							#COMPASS_COMMENT COCOM --#COMPASS_COMMENT COCOM
							FULL OUTER JOIN #ONELINK_COMMENT OLCOM --#ONELINK_COMMENT OLCOM
								ON OLCOM.DF_PRS_ID = COCOM.DF_PRS_ID
				) NEW_DATA
			--check for existing record to add queue task for the current date
				LEFT JOIN  OLS.olqtskbldr.Queues ExistingData
					ON ExistingData.TargetId = NEW_DATA.TargetId
					AND ExistingData.QueueName = @QueName
					AND ExistingData.DeletedAt IS NULL
					AND ExistingData.Comment = NEW_DATA.Comment
					AND 
					(
						ExistingData.ProcessedAt IS NULL
						OR CAST(ExistingData.ProcessedAt AS DATE) = CAST(GETDATE() AS DATE)
					)
		WHERE
				ExistingData.TargetId IS NULL; --record to create queue task does already exist for the current date
		
		COMMIT TRANSACTION
	END TRY
BEGIN CATCH
	DECLARE @EM VARCHAR(4000) = @ScriptId + ' encountered an error.  Transaction not committed. Error: ' + (SELECT ERROR_MESSAGE());

	ROLLBACK TRANSACTION;

	DECLARE @ProcessLogId INT;
	DECLARE @ProcessNotificationId INT;
	DECLARE @NotificationTypeId INT = (SELECT TOP 1 NotificationTypeId FROM ProcessLogs..NotificationTypes WHERE NotificationTypeDescription = 'Error Report'); --Error report
	DECLARE @NotificationSeverityTypeId INT = (SELECT TOP 1 NotificationSeverityTypeId FROM ProcessLogs..NotificationSeverityTypes WHERE NotificationSeverityTypeDescription = 'Critical'); --Critical
		
	INSERT INTO ProcessLogs..ProcessLogs (StartedOn, EndedOn, ScriptId, Region, RunBy) VALUES(@AddedAt,@AddedAt,@ScriptId,'uheaa',SUSER_SNAME());
	SET @ProcessLogId = SCOPE_IDENTITY()

	INSERT INTO ProcessLogs..ProcessNotifications (NotificationTypeId,NotificationSeverityTypeId,ProcessLogId, ResolvedAt, ResolvedBy) VALUES(@NotificationTypeId,@NotificationSeverityTypeId,@ProcessLogId, NULL, NULL)
	SET @ProcessNotificationId = SCOPE_IDENTITY()

	INSERT INTO ULS.[log].ProcessLogMessages (ProcessNotificationId, LogMessage) VALUES(@ProcessNotificationId,@EM);

	THROW;
END CATCH;
