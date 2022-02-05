BEGIN TRY
	BEGIN TRANSACTION

		DROP TABLE IF EXISTS #SKIP2, #R3, #CompassOnly, #BasePopulation;

		--Table declaractions
		DECLARE @EndorserByLoan TABLE(ArcAddProcessingId INT, AccountNumber VARCHAR(10), RecipientId VARCHAR(9));
		DECLARE 
			@CompassArc VARCHAR(5) = 'S1BDN',
			@CompassEndorserArc VARCHAR(5) = 'S1ED1',
			@NOW DATETIME = GETDATE(),
			@ScriptId VARCHAR(10) = 'CMP30DYSKP',
			@SASId VARCHAR(10) = 'UTLWK17',
			@LetterId INT = 
			(
				SELECT
					LetterId 
				FROM 
					ULS.[print].Letters 
				WHERE
					Letter = 'VADDPHN'
			)
		DECLARE
			@TODAY DATE = @NOW,
			@30DAYSAGO DATE = DATEADD(DAY,-30,@NOW),
			@ScriptDataId INT =
			(
				SELECT 
					ScriptDataId 
				FROM 
					ULS.[print].ScriptData 
				WHERE 
					ScriptID = @ScriptId
					AND LetterId = @LetterId 
			);

		--select @NOW,@ScriptId,@LetterId,@30DAYSAGO,@ScriptDataId; --TEST

		--Create base population
		SELECT
			BASEPOP.BF_SSN,
			BASEPOP.DF_SPE_ACC_ID,
			BASEPOP.DM_PRS_1,
			BASEPOP.DM_PRS_LST,
			BASEPOP.DM_PRS_LST_SFX,
			BASEPOP.DX_STR_ADR_1,
			BASEPOP.DX_STR_ADR_2,
			BASEPOP.DM_CT,
			BASEPOP.[STATE],
			BASEPOP.DF_ZIP_CDE,
			BASEPOP.DM_FGN_CNY,
			BASEPOP.DC_ADR,
			BASEPOP.PHONE,
			BASEPOP.CCC,
			BASEPOP.DC_DOM_ST,
			BASEPOP.SKIPTASK,
			BASEPOP.TILP,
			BASEPOP.TEMP_DATE,
			BASEPOP.AddressValidPhoneInvalid
		INTO 
			#BasePopulation
		FROM
			(--Base population
				SELECT
					LN10.BF_SSN,
					PD10.DF_SPE_ACC_ID,
					PD10.DM_PRS_1,
					PD10.DM_PRS_LST,
					PD10.DM_PRS_LST_SFX,
					PD30.DX_STR_ADR_1,
					PD30.DX_STR_ADR_2,
					PD30.DM_CT,
					IIF(PD30.DM_FGN_CNY != '', PD30.DM_FGN_ST, PD30.DC_DOM_ST) AS [STATE],
					PD30.DF_ZIP_CDE,
					PD30.DM_FGN_CNY,
					PD30.DC_ADR,
					CONCAT(PD42.DN_DOM_PHN_ARA, PD42.DN_DOM_PHN_XCH, PD42.DN_DOM_PHN_LCL) AS PHONE,
					IIF(LN10.LF_LON_CUR_OWN = COM.LenderId, 'MA2324', 'MA2327') AS CCC,
					IIF(PD30.DC_DOM_ST = 'FC', '', NULL) AS DC_DOM_ST,
					CASE 
						WHEN PD30.DI_VLD_ADR = 'N' AND PD42.DI_PHN_VLD = 'Y' THEN 'BRWRCALS'
						WHEN PD42.DI_PHN_VLD = 'N' THEN 'ACURINT2'
						ELSE NULL
					END AS SKIPTASK
					--EXCLUSION FLAGS:
					,IIF(LN10.IC_LON_PGM = 'TILP', 1, 0) AS TILP
					,CASE
						WHEN PD30.DI_VLD_ADR = 'N' AND PD42.DI_PHN_VLD = 'N'
						THEN
							CASE
								WHEN PD30.DD_VER_ADR > PD42.DD_PHN_VER
								THEN PD30.DD_VER_ADR
								ELSE PD42.DD_PHN_VER
							END
						WHEN PD30.DI_VLD_ADR = 'N' THEN PD30.DD_VER_ADR
						ELSE PD42.DD_PHN_VER			
					END AS TEMP_DATE
					,IIF(PD30.DI_VLD_ADR = 'Y' AND PD42.DI_PHN_VLD = 'N', 1, 0) AS AddressValidPhoneInvalid
					--VALIDATION FIELDS:
					--,PD42.DI_PHN_VLD 
					--,PD30.DI_VLD_ADR 
					--,PD30.DD_VER_ADR
					--,PD42.DD_PHN_VER
				FROM 
					UDW..LN10_LON LN10
					INNER JOIN ODW..PD01_PDM_INF PD01 --gets OneLINK borrowers
						ON  PD01.DF_PRS_ID = LN10.BF_SSN
					INNER JOIN ODW..GA01_APP GA01 
						ON GA01.DF_PRS_ID_BR = LN10.BF_SSN --gets ONLY OneLINK borrowers
					INNER JOIN UDW..PD10_PRS_NME PD10
						ON PD10.DF_PRS_ID = LN10.BF_SSN
					INNER JOIN UDW..PD30_PRS_ADR PD30
						ON PD30.DF_PRS_ID = LN10.BF_SSN
					INNER JOIN UDW..PD42_PRS_PHN PD42
						ON PD42.DF_PRS_ID = LN10.BF_SSN
					LEFT JOIN UDW.progrevw.LDR_AFF COM
						ON COM.LenderId = LN10.LF_LON_CUR_OWN
						AND COM.Affiliation = 'UHEAA' --get COM loans
					LEFT JOIN ODW..CT30_CALL_QUE CT30
						ON CT30.DF_PRS_ID_BR = LN10.BF_SSN
						AND CT30.IF_WRK_GRP IN ('BRWRCALS','ACURINT2') --exclude these groups
					LEFT JOIN UDW.progrevw.GENR_REF_LoanTypes PRIV
						ON PRIV.LoanType = LN10.IC_LON_PGM
						AND PRIV.LoanProgram = 'PRIVATE'	--exclude private loans
					LEFT JOIN
					(--exclude these code claim collection activities
						SELECT
							AY10.BF_SSN 
						FROM 
							UDW..AY10_BR_LON_ATY AY10
							INNER JOIN UDW..AC10_ACT_REQ AC10
								ON AY10.PF_REQ_ACT = AC10.PF_REQ_ACT
						WHERE 
							AY10.LD_ATY_RSP >= @30DAYSAGO 
							AND AC10.PC_CCI_CLM_COL_ATY IN ('SD', 'SO', 'SS', 'YY')
							AND AC10.PC_STA_ACT10 = 'A'
							AND AY10.LC_STA_ACTY10 = 'A'
					) AC10
						ON LN10.BF_SSN = AC10.BF_SSN
					LEFT JOIN 
					(--exclude this arc based on date
						SELECT 
							BF_SSN
							,MAX(LD_ATY_REQ_RCV) AS LD_ATY_REQ_RCV
						FROM
							UDW..AY10_BR_LON_ATY 
						WHERE
							PF_REQ_ACT = 'SKPME'
							AND LC_STA_ACTY10 = 'A'
						GROUP BY 
							BF_SSN
					) AY10
						ON LN10.BF_SSN = AY10.BF_SSN
					LEFT JOIN
					(--exclude verified death/disability/bankruptcy
						SELECT
							BF_SSN
						FROM
							UDW..DW01_DW_CLC_CLU
						WHERE
							WC_DW_LON_STA IN ('17','19','21') --death,disability,bankruptcy
					) DDB
						ON DDB.BF_SSN = LN10.BF_SSN		
					LEFT JOIN
					(--exclude verified death
						SELECT
							DF_PRS_ID
						FROM
							UDW..PD21_GTR_DTH
						WHERE
							DC_DTH_STA = '02'
					) PD21
						ON PD21.DF_PRS_ID = LN10.BF_SSN
					LEFT JOIN
					(--exclude TPD
						SELECT
							DF_PRS_ID
						FROM
							UDW..PD22_PRS_DSA
						WHERE
							DX_PRS_DSA_TPD_REA = 'APPAPPR'
					) PD22
						ON PD22.DF_PRS_ID = LN10.BF_SSN
					LEFT JOIN
					(--exclude verified bankruptcy
						SELECT
							DF_PRS_ID
						FROM
							UDW..PD24_PRS_BKR
						WHERE
							DC_BKR_STA = '06'
					) PD24
						ON PD24.DF_PRS_ID = LN10.BF_SSN
				WHERE
					CT30.DF_PRS_ID_BR IS NULL --exclude BRWRCALS,ACURINT2
					AND PRIV.LoanType IS NULL --exclude private loans
					AND AC10.BF_SSN IS NULL --exclude SKPME
					AND DDB.BF_SSN IS NULL --exclude verified death/disability/bankruptcy
					AND PD21.DF_PRS_ID IS NULL --exclude verified death
					AND PD22.DF_PRS_ID IS NULL --exclude TPD
					AND PD24.DF_PRS_ID IS NULL --exclude verified bankruptcy
					AND LN10.LA_CUR_PRI > 0.00
					AND LN10.LC_STA_LON10 = 'R'
					AND PD30.DC_ADR = 'L'
					AND PD42.DC_PHN = 'H'
					AND (
							PD42.DI_PHN_VLD = 'N' 
							OR PD30.DI_VLD_ADR = 'N'
						)
					AND (--exclude arc based on date
							AY10.LD_ATY_REQ_RCV < @30DAYSAGO 
							OR AY10.LD_ATY_REQ_RCV IS NULL
						)
			) BASEPOP

		SELECT
			BF_SSN,
			DF_SPE_ACC_ID,
			TILP,
			TEMP_DATE,
			AddressValidPhoneInvalid
		INTO 
			#CompassOnly
		FROM
			(
				SELECT DISTINCT
					LN10.BF_SSN,
					PD10.DF_SPE_ACC_ID,
					--EXCLUSION FLAGS:
					IIF(LN10.IC_LON_PGM = 'TILP', 1, 0) AS TILP,
					CASE
						WHEN PD30.DI_VLD_ADR = 'N' AND PD42.DI_PHN_VLD = 'N'
						THEN
							CASE
								WHEN PD30.DD_VER_ADR > PD42.DD_PHN_VER
								THEN PD30.DD_VER_ADR
								ELSE PD42.DD_PHN_VER
							END
						WHEN PD30.DI_VLD_ADR = 'N' THEN PD30.DD_VER_ADR
						ELSE PD42.DD_PHN_VER			
					END AS TEMP_DATE,
					IIF(PD30.DI_VLD_ADR = 'Y' AND PD42.DI_PHN_VLD = 'N', 1, 0) AS AddressValidPhoneInvalid
					--VALIDATION FIELDS:
					--,PD42.DI_PHN_VLD 
					--,PD30.DI_VLD_ADR 
					--,PD30.DD_VER_ADR
					--,PD42.DD_PHN_VER
				FROM 
					UDW..LN10_LON LN10
					INNER JOIN UDW..PD10_PRS_NME PD10
						ON PD10.DF_PRS_ID = LN10.BF_SSN
					INNER JOIN UDW..PD30_PRS_ADR PD30
						ON PD30.DF_PRS_ID = LN10.BF_SSN
					INNER JOIN UDW..PD42_PRS_PHN PD42
						ON PD42.DF_PRS_ID = LN10.BF_SSN		
					LEFT JOIN ODW..PD01_PDM_INF PD01 --gets OneLINK borrowers
						ON  PD01.DF_PRS_ID = LN10.BF_SSN
					LEFT JOIN ODW..GA01_APP GA01 
						ON GA01.DF_PRS_ID_BR = LN10.BF_SSN --gets ONLY OneLINK borrowers
					LEFT JOIN ODW..CT30_CALL_QUE CT30
						ON CT30.DF_PRS_ID_BR = LN10.BF_SSN
						AND CT30.IF_WRK_GRP IN ('BRWRCALS','ACURINT2') --exclude these groups
					LEFT JOIN UDW.progrevw.GENR_REF_LoanTypes PRIV
						ON PRIV.LoanType = LN10.IC_LON_PGM
						AND PRIV.LoanProgram = 'PRIVATE'	--exclude private loans
					LEFT JOIN
					(--exclude these code claim collection activities
						SELECT
							AY10.BF_SSN 
						FROM 
							UDW..AY10_BR_LON_ATY AY10
							INNER JOIN UDW..AC10_ACT_REQ AC10
								ON AY10.PF_REQ_ACT = AC10.PF_REQ_ACT
						WHERE 
							AY10.LD_ATY_RSP >= @30DAYSAGO 
							AND AC10.PC_CCI_CLM_COL_ATY IN ('SD', 'SO', 'SS', 'YY')
							AND AC10.PC_STA_ACT10 = 'A'
							AND AY10.LC_STA_ACTY10 = 'A'
					) AC10
						ON LN10.BF_SSN = AC10.BF_SSN
					LEFT JOIN 
					(--exclude this arc based on date
						SELECT 
							BF_SSN
							,MAX(LD_ATY_REQ_RCV) AS LD_ATY_REQ_RCV
						FROM
							UDW..AY10_BR_LON_ATY 
						WHERE
							PF_REQ_ACT = 'SKPME'
							AND LC_STA_ACTY10 = 'A'
						GROUP BY 
							BF_SSN
					) AY10
						ON LN10.BF_SSN = AY10.BF_SSN
					LEFT JOIN
					(--exclude verified death/disability/bankruptcy
						SELECT
							BF_SSN
						FROM
							UDW..DW01_DW_CLC_CLU
						WHERE
							WC_DW_LON_STA IN ('17','19','21') --death,disability,bankruptcy
					) DDB
						ON DDB.BF_SSN = LN10.BF_SSN		
					LEFT JOIN
					(--exclude verified death
						SELECT
							DF_PRS_ID
						FROM
							UDW..PD21_GTR_DTH
						WHERE
							DC_DTH_STA = '02'
					) PD21
						ON PD21.DF_PRS_ID = LN10.BF_SSN
					LEFT JOIN
					(--exclude TPD
						SELECT
							DF_PRS_ID
						FROM
							UDW..PD22_PRS_DSA
						WHERE
							DX_PRS_DSA_TPD_REA = 'APPAPPR'
					) PD22
						ON PD22.DF_PRS_ID = LN10.BF_SSN
					LEFT JOIN
					(--exclude verified bankruptcy
						SELECT
							DF_PRS_ID
						FROM
							UDW..PD24_PRS_BKR
						WHERE
							DC_BKR_STA = '06'
					) PD24
						ON PD24.DF_PRS_ID = LN10.BF_SSN
				WHERE
					CT30.DF_PRS_ID_BR IS NULL --exclude BRWRCALS,ACURINT2
					AND PD01.DF_PRS_ID IS NULL --exclude Onelink borrowers
					AND PRIV.LoanType IS NULL --exclude private loans
					AND AC10.BF_SSN IS NULL --exclude SKPME
					AND DDB.BF_SSN IS NULL --exclude verified death/disability/bankruptcy
					AND PD21.DF_PRS_ID IS NULL --exclude verified death
					AND PD22.DF_PRS_ID IS NULL --exclude TPD
					AND PD24.DF_PRS_ID IS NULL --exclude verified bankruptcy
					AND LN10.LA_CUR_PRI > 0.00
					AND LN10.LC_STA_LON10 = 'R'
					AND PD30.DC_ADR = 'L'
					AND PD42.DC_PHN = 'H'
					AND (
							PD42.DI_PHN_VLD = 'N' 
							OR PD30.DI_VLD_ADR = 'N'
						)
					AND (--exclude arc based on date
							AY10.LD_ATY_REQ_RCV < @30DAYSAGO 
							OR AY10.LD_ATY_REQ_RCV IS NULL
						)
			) BASEPOP

		--COMPASS ONLY BORROWERS
		INSERT INTO ULS.dbo.ArcAddProcessing
		(
			ArcTypeId,
			ArcResponseCodeId,
			AccountNumber,
			RecipientId,
			ARC,
			ScriptId,
			ProcessOn,
			Comment,
			IsReference,
			IsEndorser,
			CreatedAt,
			CreatedBy
		)
		SELECT DISTINCT
			1 AS ArcTypeId, --All Loans
			NULL AS ArcResponseCodeId,
			BASEPOP.DF_SPE_ACC_ID AS AccountNumber,
			NULL AS RecipientId,
			@CompassArc AS ARC,
			@ScriptId AS ScriptId,
			@NOW AS ProcessOn,
			'' AS Comment,
			0 AS IsReference,
			0 AS IsEndorser,
			@NOW AS CreatedAt,
			@SASId AS CreatedBy		
		FROM
			#CompassOnly BASEPOP
			--check for existing record to add queue task for the current date
			LEFT JOIN  ULS.dbo.ArcAddProcessing ExistingData
				ON ExistingData.AccountNumber = BASEPOP.DF_SPE_ACC_ID
				AND ExistingData.ARC = @CompassArc
				AND CAST(ExistingData.CreatedAt AS DATE) = @Today
		WHERE
			TILP = 0
			AND TEMP_DATE <= @30DAYSAGO
			AND AddressValidPhoneInvalid = 0
			AND ExistingData.AccountNumber IS NULL --record to create queue task does already exist for the current date
		;

		--COMPASS ONLY ENDORSERS
		INSERT INTO ULS.dbo.ArcAddProcessing
		(
			ArcTypeId,
			ArcResponseCodeId,
			AccountNumber,
			RecipientId,
			ARC,
			ScriptId,
			ProcessOn,
			Comment,
			IsReference,
			IsEndorser,
			CreatedAt,
			CreatedBy,
			RegardsTo,
			RegardsCode
		)
		OUTPUT
			INSERTED.ArcAddProcessingId,
			INSERTED.AccountNumber,
			INSERTED.RecipientId
		INTO
			@EndorserByLoan
			(
				ArcAddProcessingId,
				AccountNumber,
				RecipientId
			)
		SELECT DISTINCT
			0 AS ArcTypeId, --By Loan
			NULL AS ArcResponseCodeId,
			BASEPOP.DF_SPE_ACC_ID AS AccountNumber, --Borrower account number
			LN20.LF_EDS AS RecipientId,
			@CompassEndorserArc AS ARC,
			@ScriptId AS ScriptId,
			@NOW AS ProcessOn,
			'' AS Comment,
			0 AS IsReference,
			1 AS IsEndorser,
			@NOW AS CreatedAt,
			@SASId AS CreatedBy,
			LN20.LF_EDS AS RegardsTo,
			'E' AS RegardsCode
		FROM
			#CompassOnly BASEPOP
			INNER JOIN	UDW..LN20_EDS LN20
				ON LN20.BF_SSN = BASEPOP.BF_SSN
				AND LN20.LC_STA_LON20 = 'A'
			--check for existing record to add queue task for the current date
			LEFT JOIN  ULS.dbo.ArcAddProcessing ExistingData
				ON ExistingData.AccountNumber = BASEPOP.DF_SPE_ACC_ID
				AND ExistingData.RecipientId = LN20.LF_EDS
				AND ExistingData.ARC = @CompassEndorserArc
				AND CAST(ExistingData.CreatedAt AS DATE) = @Today
		WHERE
			TILP = 0
			AND TEMP_DATE <= @30DAYSAGO
			AND AddressValidPhoneInvalid = 0
			AND ExistingData.AccountNumber IS NULL --record to create queue task does already exist for the current date
		;

		--Insert the loan sequences the coborrower is on
		INSERT INTO ULS..ArcLoanSequenceSelection
		(
			ArcAddProcessingId,
			LoanSequence
		)
		SELECT
			EBL.ArcAddProcessingId,
			LN20.LN_SEQ
		FROM
			@EndorserByLoan EBL
			INNER JOIN UDW..PD10_PRS_NME PD10
				ON PD10.DF_SPE_ACC_ID = EBL.AccountNumber
			INNER JOIN UDW..LN20_EDS LN20
				ON LN20.BF_SSN = PD10.DF_PRS_ID
				AND LN20.LF_EDS = EBL.RecipientId
				AND LN20.LC_STA_LON20 = 'A'

		--R2: SKIP data set:
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
			BASEPOP.BF_SSN AS TargetId,
			BASEPOP.SKIPTASK AS QueueName,
			''	 AS InstitutionId,
			''	 AS InstitutionType,
			NULL AS DateDue,
			NULL AS TimeDue,
			''	 AS Comment,
			''	 AS SourceFilename,
			@NOW AS AddedAt,
			@SASId AS AddedBy		
		FROM
			#BasePopulation BASEPOP
--check for existing record to add queue task for the current date
			LEFT JOIN  OLS.olqtskbldr.Queues ExistingData
				ON ExistingData.TargetId = BASEPOP.BF_SSN
				AND ExistingData.QueueName = BASEPOP.SKIPTASK
				AND CAST(ExistingData.AddedAt AS DATE) = @Today
				AND ExistingData.DeletedAt IS NULL
				AND ExistingData.DeletedBy IS NULL
		WHERE
			TILP = 0
			AND TEMP_DATE <= @30DAYSAGO
			AND AddressValidPhoneInvalid = 0
			AND ExistingData.TargetId IS NULL --record to create queue task does already exist for the current date
		;
		--select * from OLS.olqtskbldr.Queues where QueueName in ('BRWRCALS','ACURINT2') --TEST

		--R3: SKIP2 data set:
		SELECT DISTINCT
			BASEPOP.BF_SSN,
			BASEPOP.DF_SPE_ACC_ID,
			CentralData.dbo.CreateACSKeyline(BASEPOP.BF_SSN, 'B', BASEPOP.DC_ADR) AS ACSKEY,
			BASEPOP.DM_PRS_1,
			RTRIM(CONCAT(RTRIM(BASEPOP.DM_PRS_LST),' ',RTRIM(BASEPOP.DM_PRS_LST_SFX))) AS DM_PRS_LST,
			BASEPOP.DX_STR_ADR_1,
			BASEPOP.DX_STR_ADR_2,
			BASEPOP.DM_CT,
			BASEPOP.[STATE],
			BASEPOP.DF_ZIP_CDE,
			BASEPOP.DM_FGN_CNY,
			BASEPOP.PHONE,
			BASEPOP.DC_DOM_ST,
			BASEPOP.CCC
		INTO
			#SKIP2
		FROM
			#BasePopulation BASEPOP
		WHERE
			TILP = 0
			AND TEMP_DATE <= @30DAYSAGO
			AND AddressValidPhoneInvalid = 1
		;
		--select * from #SKIP2 order by DF_SPE_ACC_ID --TEST

		--remove borrowers with multiple CostCenterCodes keeping only MA2324
		;WITH MA2324 AS
		(
			SELECT DISTINCT 
				BF_SSN,
				DF_SPE_ACC_ID,
				ACSKEY,
				DM_PRS_1,
				DM_PRS_LST,
				DX_STR_ADR_1,
				DX_STR_ADR_2,
				DM_CT,
				[STATE],
				DF_ZIP_CDE,
				DM_FGN_CNY,
				PHONE,
				DC_DOM_ST,
				CCC
			FROM
				#SKIP2
			WHERE
				CCC = 'MA2324'
		)
		SELECT DISTINCT
			S.BF_SSN,
			S.DF_SPE_ACC_ID,
			S.ACSKEY,
			S.DM_PRS_1,
			S.DM_PRS_LST,
			S.DX_STR_ADR_1,
			S.DX_STR_ADR_2,
			S.DM_CT,
			S.[STATE],
			S.DF_ZIP_CDE,
			S.DM_FGN_CNY,
			S.PHONE,
			S.DC_DOM_ST,
			S.CCC
		INTO
			#R3
		FROM
			#SKIP2 S 
			LEFT JOIN MA2324
				ON S.BF_SSN = MA2324.BF_SSN
		WHERE
			MA2324.BF_SSN IS NULL --remove all MA2324

		UNION

		--pop with MA2324
		SELECT
			BF_SSN,
			DF_SPE_ACC_ID,
			ACSKEY,
			DM_PRS_1,
			DM_PRS_LST,
			DX_STR_ADR_1,
			DX_STR_ADR_2,
			DM_CT,
			[STATE],
			DF_ZIP_CDE,
			DM_FGN_CNY,
			PHONE,
			DC_DOM_ST,
			CCC
		FROM
			MA2324
		;

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
		SELECT
			R3.DF_SPE_ACC_ID AS AccountNumber, 
			EM.EmailAddress, 
			@ScriptDataId, 
			CONCAT(--data to be inserted into letter
					R3.BF_SSN,
					',',R3.DF_SPE_ACC_ID,
					',',R3.ACSKEY,
					',',RTRIM(R3.DM_PRS_1),
					',',RTRIM(R3.DM_PRS_LST),
					',',RTRIM(R3.DX_STR_ADR_1),
					',',RTRIM(R3.DX_STR_ADR_2),
					',',RTRIM(R3.DM_CT),
					',',RTRIM(R3.[STATE]),
					',',RTRIM(R3.DF_ZIP_CDE),
					',',RTRIM(R3.DM_FGN_CNY),
					',',RTRIM(R3.PHONE),
					',',R3.DC_DOM_ST,
					',',R3.CCC
				) AS LetterData,
			R3.CCC AS CostCenter, 
			0 AS InValidAddress, 
			0 AS DoNotProcessEcorr, 
			0 AS OnEcorr, 
			0 AS ArcNeeded,  
			0 AS ImagingNeeded, 
			@NOW AS AddedAt, 
			@SASId AS AddedBy
		FROM
			#R3 R3
			INNER JOIN UDW.calc.EmailAddress EM
				ON R3.BF_SSN = EM.DF_PRS_ID
			LEFT JOIN ULS.[print].PrintProcessing ExistingData
				ON ExistingData.AccountNumber = R3.DF_SPE_ACC_ID
				AND ExistingData.EmailAddress = EM.EmailAddress
				AND ExistingData.ScriptDataId = @ScriptDataId
				AND ExistingData.LetterData = 
					CONCAT(--data to be inserted into letter
							R3.BF_SSN,
							',',R3.DF_SPE_ACC_ID,
							',',R3.ACSKEY,
							',',RTRIM(R3.DM_PRS_1),
							',',RTRIM(R3.DM_PRS_LST),
							',',RTRIM(R3.DX_STR_ADR_1),
							',',RTRIM(R3.DX_STR_ADR_2),
							',',RTRIM(R3.DM_CT),
							',',RTRIM(R3.[STATE]),
							',',RTRIM(R3.DF_ZIP_CDE),
							',',RTRIM(R3.DM_FGN_CNY),
							',',RTRIM(R3.PHONE),
							',',R3.DC_DOM_ST,
							',',R3.CCC
						)
				AND ExistingData.CostCenter = R3.CCC
				AND ExistingData.InValidAddress = 0
				AND ExistingData.DoNotProcessEcorr = 0
				AND ExistingData.OnEcorr = 0
				AND ExistingData.ArcNeeded = 0
				AND ExistingData.ImagingNeeded = 0
				AND (
						CONVERT(DATE,ExistingData.AddedAt) = @TODAY
						OR (
								ExistingData.EcorrDocumentCreatedAt IS NULL
								AND ExistingData.PrintedAt IS NULL
								AND ExistingData.DeletedAt IS NULL
							)
					)
		WHERE
			ExistingData.AccountNumber IS NULL
		;
		--select * from uls.[print].PrintProcessing where AddedBy = 'CMP30DYSKP'  --TEST
		
	COMMIT TRANSACTION
END TRY
BEGIN CATCH
	DECLARE @EM VARCHAR(4000) = @SASId + ' encountered an error.  Transaction not committed. Error: ' + (SELECT ERROR_MESSAGE());

	ROLLBACK TRANSACTION;

	DECLARE @ProcessLogId INT;
	DECLARE @ProcessNotificationId INT;
	DECLARE @NotificationTypeId INT = (SELECT TOP 1 NotificationTypeId FROM ProcessLogs..NotificationTypes WHERE NotificationTypeDescription = 'Error Report'); --Error report
	DECLARE @NotificationSeverityTypeId INT = (SELECT TOP 1 NotificationSeverityTypeId FROM ProcessLogs..NotificationSeverityTypes WHERE NotificationSeverityTypeDescription = 'Critical'); --Critical
		
	INSERT INTO ProcessLogs..ProcessLogs (StartedOn, EndedOn, ScriptId, Region, RunBy) VALUES(@NOW,@NOW,@SASId,'uheaa',SUSER_SNAME());
	SET @ProcessLogId = SCOPE_IDENTITY()

	INSERT INTO ProcessLogs..ProcessNotifications (NotificationTypeId,NotificationSeverityTypeId,ProcessLogId, ResolvedAt, ResolvedBy) VALUES(@NotificationTypeId,@NotificationSeverityTypeId,@ProcessLogId, NULL, NULL)
	SET @ProcessNotificationId = SCOPE_IDENTITY()

	INSERT INTO ULS.[log].ProcessLogMessages (ProcessNotificationId, LogMessage) VALUES(@ProcessNotificationId,@EM);

	THROW;
END CATCH;