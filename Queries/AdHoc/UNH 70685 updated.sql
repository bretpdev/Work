


		DECLARE @NOW DATETIME = GETDATE(),
				@ScriptId VARCHAR(10) = 'SYSAPPDEFF',
				@LetterId INT = 
				(
					SELECT
						LetterId 
					FROM 
						ULS.[print].Letters 
					WHERE
						Letter = 'SADEFER'
				);

		DECLARE @TODAY DATE = @NOW,
				@FiveDaysAgo DATE= DATEADD(DAY,-5,@NOW),
				@ScriptDataId INT =
				(
					SELECT 
						ScriptDataId 
					FROM 
						ULS.[print].ScriptData 
					WHERE 
						ScriptID = @ScriptId
						AND LetterId = @LetterId 
				)
		--select @TODAY,@ScriptId,@LetterId,@FiveDaysAgo,@ScriptDataId --test

		INSERT INTO ULS.[print].PrintProcessing
		(
			AccountNumber, 
			EmailAddress, 
			ScriptDataId, 
			LetterData, 
			CostCenter, 
			InValidAddress, 
			DoNotProcessEcorr, 
			OnEcorr, 
			ArcNeeded, 
			ImagingNeeded, 
			AddedAt, 
			AddedBy
		)
		SELECT DISTINCT
			ALLPOP.DF_SPE_ACC_ID AS AccountNumber,
			ALLPOP.EmailAddress,
			@ScriptDataId AS ScriptDataId,
			CONCAT(--data to be inserted into letter
					ALLPOP.BF_SSN
					,',',CentralData.dbo.CreateACSKeyline(ALLPOP.BF_SSN, 'B', ALLPOP.DC_ADR)
					,',',ALLPOP.DF_SPE_ACC_ID
					,',',RTRIM(ALLPOP.DM_PRS_MID)
					,',',RTRIM(ALLPOP.DM_PRS_1)
					,',',RTRIM(ALLPOP.DM_PRS_LST)
					,',',RTRIM(ALLPOP.DX_STR_ADR_1)
					,',',RTRIM(ALLPOP.DX_STR_ADR_2)
					,',',RTRIM(ALLPOP.DM_CT)
					,',',ALLPOP.DC_DOM_ST
					,',',RTRIM(ALLPOP.DF_ZIP_CDE)
					,',',RTRIM(ALLPOP.DM_FGN_CNY)
					,',',RTRIM(ALLPOP.DM_FGN_ST)
					,',',ALLPOP.DC_ADR
					,',',RTRIM(ALLPOP.LF_LON_CUR_OWN)
					,',',ALLPOP.DC_DOM_ST
					,',',ALLPOP.CostCenter
				) AS LetterData,
			ALLPOP.CostCenter,
			0 AS InValidAddress,
			0 AS DoNotProcessEcorr,
			ALLPOP.ON_ECORR AS OnEcorr,
			0 AS ArcNeeded,
			0 AS ImagingNeeded,
			@TODAY AS AddedAt,
			@ScriptId AS AddedBy
		FROM
			(
				SELECT
					ISNULL(EM.EmailAddress, 'ECORR@UHEAA.ORG') AS EmailAddress,
					LN10.BF_SSN,
					--LN10.LN_SEQ,
					PD10.DF_SPE_ACC_ID,
					PD10.DM_PRS_MID,
					PD10.DM_PRS_1,
					CONCAT(RTRIM(PD10.DM_PRS_LST),' ',RTRIM(PD10.DM_PRS_LST_SFX)) AS DM_PRS_LST,
					PD30.DX_STR_ADR_1,
					PD30.DX_STR_ADR_2,
					PD30.DM_CT,
					PD30.DC_DOM_ST,
					PD30.DF_ZIP_CDE,
					PD30.DM_FGN_CNY,
					PD30.DM_FGN_ST,
					PD30.DC_ADR,
					IIF(LN10.LF_LON_CUR_OWN IN ('829769','82976901','82976902','82976903','82976904','82976905','82976906','82976907','82976908'), '829769', LN10.LF_LON_CUR_OWN) AS LF_LON_CUR_OWN, --BANA case handling
					IIF(LN10.LF_LON_CUR_OWN = UHEAA_LIST.LenderId, 'MA2324','MA2327') AS CostCenter,
					IIF(AY10.LD_ATY_REQ_RCV >= LN50.LD_DFR_BEG, 1, 0) AS ArcAfterDefer,
					IIF(DF10.LF_USR_CRT_REQ_DFR NOT LIKE 'UT%', 1, 0) AS SystemCreated,
					IIF(LN10.IC_LON_PGM = 'TILP', 1, 0) AS TILP,
					IIF(LN10.LF_LON_CUR_OWN = UHEAA_LIST.LenderId, 'Y',NULL) AS INHOUSE,
					IIF(PD30.DC_DOM_ST IN ('FC',' '), 1, 2) AS SRTVR,
					CASE 
						WHEN PH05.DF_SPE_ID IS NOT NULL AND EM.DF_PRS_ID IS NOT NULL THEN 1
						WHEN PD30.DF_PRS_ID IS NOT NULL THEN 0
						ELSE NULL
					END AS ON_ECORR,
					DF10.LD_CRT_REQ_DFR
				FROM
					UDW..LN10_LON LN10
					INNER JOIN UDW..LN50_BR_DFR_APV LN50
						ON LN10.BF_SSN = LN50.BF_SSN
						AND LN10.LN_SEQ = LN50.LN_SEQ
					INNER JOIN UDW..DF10_BR_DFR_REQ DF10
						ON LN50.BF_SSN = DF10.BF_SSN
						AND LN50.LF_DFR_CTL_NUM = DF10.LF_DFR_CTL_NUM
					INNER JOIN UDW..PD10_PRS_NME PD10
						ON LN10.BF_SSN = PD10.DF_PRS_ID
					LEFT JOIN UDW..PD30_PRS_ADR PD30
						ON PD10.DF_PRS_ID = PD30.DF_PRS_ID
						AND PD30.DC_ADR = 'L' --legal address
						AND PD30.DI_VLD_ADR = 'Y'
					LEFT JOIN UDW..PH05_CNC_EML PH05
						ON PH05.DF_SPE_ID = PD10.DF_SPE_ACC_ID
						AND PH05.DI_CNC_ELT_OPI = 'Y'
					LEFT JOIN UDW.calc.EmailAddress EM
						ON LN10.BF_SSN = EM.DF_PRS_ID
					LEFT JOIN 
					(--gets most recent MSD0A
						SELECT 
							BF_SSN,
							MAX(LD_ATY_REQ_RCV) AS LD_ATY_REQ_RCV
						FROM 
							UDW..AY10_BR_LON_ATY
						WHERE 
							PF_REQ_ACT = 'MSD0A'
						GROUP BY 
							BF_SSN
					) AY10
						ON LN10.BF_SSN = AY10.BF_SSN
					LEFT JOIN 
					(--gets only UHEAA lenders
						SELECT
							LenderId,
							Affiliation
						FROM
							UDW.progrevw.LDR_AFF
						WHERE
							Affiliation = 'UHEAA'
					) UHEAA_LIST
						ON LN10.LF_LON_CUR_OWN = UHEAA_LIST.LenderId
				WHERE 
					LN10.LA_CUR_PRI > 0.00
					AND LN10.LC_STA_LON10 = 'R'
					AND DF10.LD_CRT_REQ_DFR BETWEEN @FiveDaysAgo AND @TODAY
					AND LN50.LC_DFR_RSP != '003' --not denied
					AND LN50.LC_STA_LON50 = 'A'
					AND DF10.LC_DFR_STA = 'A'
					AND DF10.LC_STA_DFR10 = 'A'
					AND PD10.DF_PRS_ID LIKE '[0-9]%'
			) ALLPOP
			LEFT JOIN ULS.[print].PrintProcessing ExistingData
				ON ExistingData.AccountNumber = ALLPOP.DF_SPE_ACC_ID
				AND ExistingData.EmailAddress = ALLPOP.EmailAddress
				AND ExistingData.ScriptDataId = @ScriptDataId
				AND ExistingData.LetterData = 
					CONCAT(--data to be inserted into letter
							ALLPOP.BF_SSN
							,',',CentralData.dbo.CreateACSKeyline(ALLPOP.BF_SSN, 'B', ALLPOP.DC_ADR)
							,',',ALLPOP.DF_SPE_ACC_ID
							,',',RTRIM(ALLPOP.DM_PRS_MID)
							,',',RTRIM(ALLPOP.DM_PRS_1)
							,',',RTRIM(ALLPOP.DM_PRS_LST)
							,',',RTRIM(ALLPOP.DX_STR_ADR_1)
							,',',RTRIM(ALLPOP.DX_STR_ADR_2)
							,',',RTRIM(ALLPOP.DM_CT)
							,',',ALLPOP.DC_DOM_ST
							,',',RTRIM(ALLPOP.DF_ZIP_CDE)
							,',',RTRIM(ALLPOP.DM_FGN_CNY)
							,',',RTRIM(ALLPOP.DM_FGN_ST)
							,',',ALLPOP.DC_ADR
							,',',RTRIM(ALLPOP.LF_LON_CUR_OWN)
							,',',ALLPOP.DC_DOM_ST
							,',',ALLPOP.CostCenter
						)
				AND ExistingData.CostCenter = ALLPOP.CostCenter
				AND ExistingData.InValidAddress = 0
				AND ExistingData.DoNotProcessEcorr = 0
				AND ExistingData.ArcNeeded = 0
				AND ExistingData.ImagingNeeded = 0
				AND (
						CONVERT(DATE,ExistingData.AddedAt) >= ALLPOP.LD_CRT_REQ_DFR 
						OR (
								ExistingData.EcorrDocumentCreatedAt IS NULL
								AND ExistingData.PrintedAt IS NULL
								AND ExistingData.DeletedAt IS NULL
							)
					)
		WHERE
			ExistingData.AccountNumber IS NULL
			AND ALLPOP.ArcAfterDefer = 0
			AND ALLPOP.SystemCreated = 1
			AND ALLPOP.TILP = 0
			AND ALLPOP.ON_ECORR IS NOT NULL --THIS WILL BE NULL IF THEY HAVE AN INVALID ADDRESS AND ARE NOT ON ECORR
		;
		--select * from uls.[print].PrintProcessing where AddedBy = 'SYSAPPDEFF' and DeletedAt is null and ((Printedat is null and onecorr = 0) OR (EcorrDocumentCreatedAt IS NULL))
	
