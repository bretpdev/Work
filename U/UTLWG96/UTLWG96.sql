BEGIN TRY
	BEGIN TRANSACTION

		DECLARE @NOW DATETIME = GETDATE();
		DECLARE	@TODAY DATE = @NOW,
				@30DAYS DATE = DATEADD(DAY,30,@NOW),
				@JobId VARCHAR(10) = 'UTLWG96'		
		;
		--select @NOW,@TODAY,@30DAYS,@JobId --TEST

		DROP TABLE IF EXISTS #BASE_POP;

		SELECT DISTINCT
			REPORT,
			BF_SSN,
			DF_SPE_ACC_ID,
			RTRIM(DM_PRS_1) AS DM_PRS_1,
			CONCAT(RTRIM(DM_PRS_LST),' ',RTRIM(DM_PRS_LST_SFX)) AS DM_PRS_LST,
			RTRIM(DX_STR_ADR_1) AS DX_STR_ADR_1,
			RTRIM(DX_STR_ADR_2) AS DX_STR_ADR_2,
			RTRIM(DM_CT) AS DM_CT,
			RTRIM(DF_ZIP_CDE) AS DF_ZIP_CDE,
			RTRIM(DM_FGN_CNY) AS DM_FGN_CNY,
			DC_DOM_ST,
			IIF(D_DATE IS NOT NULL, CONCAT('"',FORMAT(D_DATE, 'MMMM d, yyyy'),'"'), NULL) AS D_DATE, --date info to be received
			CentralData.dbo.CreateACSKeyline(ALLPOP.BF_SSN, 'B', ALLPOP.DC_ADR) AS ACSKEY,
			STATE_CODE,
			'MA2324' AS CCC
		INTO
			#BASE_POP
		FROM
		(
			--R2: GRADUATION LETTER
			SELECT
				LN10.BF_SSN
				,PD10.DF_SPE_ACC_ID
				,PD10.DM_PRS_1
				,PD10.DM_PRS_LST
				,PD10.DM_PRS_LST_SFX
				,PD30.DX_STR_ADR_1
				,PD30.DX_STR_ADR_2
				,PD30.DM_CT
				,PD30.DC_DOM_ST
				,PD30.DF_ZIP_CDE
				,PD30.DM_FGN_CNY
				,PD30.DC_DOM_ST AS STATE_CODE
				,PD30.DC_ADR
				,NULL AS D_DATE
				,'R2' AS REPORT
			FROM
				UDW..LN10_LON LN10
				INNER JOIN 
				(--get records within past 5 days
					SELECT 
						LF_STU_SSN, 
						MAX(LD_STA_STU10) AS LD_STA_STU10
					FROM 
						UDW..SD10_STU_SPR SD10
					WHERE 
						LC_STA_STU10 = 'A'
						AND LC_REA_SCL_SPR = '01'
						AND LD_STA_STU10 BETWEEN DATEADD(DAY,-5,@TODAY) AND @TODAY
					GROUP BY
						LF_STU_SSN
				) SD10
					ON SD10.LF_STU_SSN = LN10.LF_STU_SSN
				INNER JOIN UDW..PD10_PRS_NME PD10
					ON LN10.BF_SSN = PD10.DF_PRS_ID
				INNER JOIN UDW..PD30_PRS_ADR PD30
					ON LN10.BF_SSN = PD30.DF_PRS_ID
				LEFT JOIN 
				(--exclude TLPGR
					SELECT 
						BF_SSN, 
						LD_ATY_REQ_RCV
					FROM 
						UDW..AY10_BR_LON_ATY 
					WHERE 
						PF_REQ_ACT = 'TLPGR'
						AND LC_STA_ACTY10 = 'A'
				) EXCLU
					ON LN10.BF_SSN = EXCLU.BF_SSN
					AND EXCLU.LD_ATY_REQ_RCV >= SD10.LD_STA_STU10
			WHERE 
				LN10.IC_LON_PGM = 'TILP'
				AND LN10.LA_CUR_PRI > 0.00
				AND LN10.LC_STA_LON10 = 'R'
				AND PD30.DC_ADR = 'L'
				AND PD30.DI_VLD_ADR = 'Y' 
				AND PD10.DF_PRS_ID LIKE ('[0-9]%')
				AND EXCLU.BF_SSN IS NULL

			UNION ALL

			--R3: TEACHING LICENSE NOT RECEIVED
			SELECT
				LN10.BF_SSN
				,PD10.DF_SPE_ACC_ID
				,PD10.DM_PRS_1
				,PD10.DM_PRS_LST
				,PD10.DM_PRS_LST_SFX
				,PD30.DX_STR_ADR_1
				,PD30.DX_STR_ADR_2
				,PD30.DM_CT
				,PD30.DC_DOM_ST
				,PD30.DF_ZIP_CDE
				,PD30.DM_FGN_CNY
				,PD30.DC_DOM_ST AS STATE_CODE
				,PD30.DC_ADR
				,@30DAYS AS D_DATE
				,'R3' AS REPORT
			FROM
				UDW..LN10_LON LN10
				INNER JOIN UDW..PD10_PRS_NME PD10
					ON LN10.BF_SSN = PD10.DF_PRS_ID
				INNER JOIN UDW..PD30_PRS_ADR PD30
					ON LN10.BF_SSN = PD30.DF_PRS_ID
				INNER JOIN 
				(--get records 61+ days old
					SELECT 
						LF_STU_SSN, 
						MAX(LD_STA_STU10) AS LD_STA_STU10
					FROM 
						UDW..SD10_STU_SPR 
					WHERE 
						LC_STA_STU10 = 'A'
						AND LC_REA_SCL_SPR = '01'
						AND LD_SCL_SPR < DATEADD(DAY,-60,@TODAY)
					GROUP BY
						LF_STU_SSN
				) SD10
					ON SD10.LF_STU_SSN = LN10.LF_STU_SSN
				LEFT JOIN
				(--exclude these arcs
					SELECT
						BF_SSN
					FROM
						UDW..AY10_BR_LON_ATY
					WHERE
						PF_REQ_ACT IN ('TLPTL','TLCNO','TLTCR','OTLTC')
						AND LC_STA_ACTY10 = 'A' 
				) EXCLU_ARC
					ON LN10.BF_SSN = EXCLU_ARC.BF_SSN
				LEFT JOIN
				(--excludes financial trans of 1054
					SELECT
						BF_SSN
					FROM
						UDW..LN90_FIN_ATY
					WHERE
						PC_FAT_TYP = '10' 
						AND PC_FAT_SUB_TYP = '54'
						AND LC_STA_LON90 = 'A'
						AND ISNULL(LC_FAT_REV_REA,'') = '' 
				) EXCLU_FIN
					ON LN10.BF_SSN = EXCLU_FIN.BF_SSN
				LEFT JOIN 
				(--excludes deferment types of 06 OR 33
					SELECT
						DF10.BF_SSN
					FROM
						UDW..DF10_BR_DFR_REQ DF10
						INNER JOIN UDW..LN50_BR_DFR_APV LN50
							ON DF10.BF_SSN = LN50.BF_SSN
					WHERE 
						DF10.LC_DFR_TYP IN ('06','33') 
						AND DF10.LC_DFR_STA = 'A'
						AND DF10.LC_STA_DFR10 = 'A' 
						AND LN50.LC_STA_LON50 = 'A'
						AND LN50.LC_DFR_RSP IN ('000','001','039','040','041','042','043','061','070','094') --!= '003'
				) EXCLU_DEF
					ON LN10.BF_SSN = EXCLU_DEF.BF_SSN
			WHERE 
				LN10.IC_LON_PGM = 'TILP'
				AND LN10.LA_CUR_PRI > 0.00
				AND LN10.LC_STA_LON10 = 'R'
				AND PD30.DC_ADR = 'L'
				AND PD30.DI_VLD_ADR = 'Y' 
				AND PD10.DF_PRS_ID LIKE ('[0-9]%')
				AND EXCLU_ARC.BF_SSN IS NULL
				AND EXCLU_FIN.BF_SSN IS NULL
				AND EXCLU_DEF.BF_SSN IS NULL

			UNION ALL

			--R4: LEAVE OF ABSENCE WILL EXPIRE
			SELECT
				LN10.BF_SSN
				,PD10.DF_SPE_ACC_ID
				,PD10.DM_PRS_1
				,PD10.DM_PRS_LST
				,PD10.DM_PRS_LST_SFX
				,PD30.DX_STR_ADR_1
				,PD30.DX_STR_ADR_2
				,PD30.DM_CT
				,PD30.DC_DOM_ST
				,PD30.DF_ZIP_CDE
				,PD30.DM_FGN_CNY
				,PD30.DC_DOM_ST AS STATE_CODE
				,PD30.DC_ADR
				,SD10.LD_SCL_SPR AS D_DATE
				,'R4' AS REPORT
			FROM
				UDW..LN10_LON LN10
				INNER JOIN UDW..PD10_PRS_NME PD10
					ON LN10.BF_SSN = PD10.DF_PRS_ID
				INNER JOIN UDW..PD30_PRS_ADR PD30
					ON LN10.BF_SSN = PD30.DF_PRS_ID
				INNER JOIN 
				(--get records 30+ days in future
					SELECT 
						SD10.LF_STU_SSN, 
						SD10.LD_STA_STU10, 
						SD10.LD_SCL_SPR
					FROM 
						UDW..SD10_STU_SPR SD10
						INNER JOIN
						(
							SELECT
								LF_STU_SSN,
								MAX(LD_STA_STU10) AS LD_STA_STU10
							FROM
								UDW..SD10_STU_SPR
							WHERE
								LC_STA_STU10 = 'A'
								AND LC_REA_SCL_SPR = '05'
								AND LD_SCL_SPR BETWEEN @TODAY AND @30DAYS
							GROUP BY
								LF_STU_SSN
						) MaxSD10
							ON MaxSD10.LF_STU_SSN = SD10.LF_STU_SSN
							AND MaxSD10.LD_STA_STU10 = SD10.LD_STA_STU10
					WHERE 
						SD10.LC_STA_STU10 = 'A'
						AND SD10.LC_REA_SCL_SPR = '05'
						AND SD10.LD_SCL_SPR BETWEEN @TODAY AND @30DAYS
				) SD10
					ON SD10.LF_STU_SSN = LN10.LF_STU_SSN
				LEFT JOIN 
				(--exclude TLLAE
					SELECT 
						BF_SSN, 
						LD_ATY_REQ_RCV
					FROM 
						UDW..AY10_BR_LON_ATY
					WHERE
						PF_REQ_ACT = 'TLLAE'
						AND LC_STA_ACTY10 = 'A' 
				) EXCLU
					ON LN10.BF_SSN = EXCLU.BF_SSN
					AND DATEDIFF(DAY, EXCLU.LD_ATY_REQ_RCV, SD10.LD_SCL_SPR) <= 30
			WHERE 
				LN10.IC_LON_PGM = 'TILP'
				AND LN10.LA_CUR_PRI > 0.00
				AND LN10.LC_STA_LON10 = 'R'
				AND PD30.DC_ADR = 'L'
				AND PD30.DI_VLD_ADR = 'Y'
				AND PD10.DF_PRS_ID LIKE ('[0-9]%')
				AND EXCLU.BF_SSN IS NULL

			UNION ALL

			--R5: LEAVE OF ABSENCE HAS EXPIRED
			SELECT
				LN10.BF_SSN
				,PD10.DF_SPE_ACC_ID
				,PD10.DM_PRS_1
				,PD10.DM_PRS_LST
				,PD10.DM_PRS_LST_SFX
				,PD30.DX_STR_ADR_1
				,PD30.DX_STR_ADR_2
				,PD30.DM_CT
				,PD30.DC_DOM_ST
				,PD30.DF_ZIP_CDE
				,PD30.DM_FGN_CNY
				,PD30.DC_DOM_ST AS STATE_CODE
				,PD30.DC_ADR
				,SD10.LD_SCL_SPR AS D_DATE
				,'R5' AS REPORT
			FROM
				UDW..LN10_LON LN10
				INNER JOIN UDW..PD10_PRS_NME PD10
					ON LN10.BF_SSN = PD10.DF_PRS_ID
				INNER JOIN UDW..PD30_PRS_ADR PD30
					ON LN10.BF_SSN = PD30.DF_PRS_ID
				INNER JOIN 
				(--get records 5+ days old
					SELECT 
						SD10.LF_STU_SSN, 
						SD10.LD_STA_STU10, 
						SD10.LD_SCL_SPR
					FROM 
						UDW..SD10_STU_SPR SD10
						INNER JOIN
						(
							SELECT
								LF_STU_SSN,
								MAX(LD_STA_STU10) AS LD_STA_STU10
							FROM
								UDW..SD10_STU_SPR
							WHERE
								LC_STA_STU10 = 'A'
								AND LC_REA_SCL_SPR = '05'
								AND LD_SCL_SPR <= DATEADD(DAY,-5,@TODAY)
							GROUP BY
								LF_STU_SSN
						) MaxSD10
							ON MaxSD10.LF_STU_SSN = SD10.LF_STU_SSN
							AND MaxSD10.LD_STA_STU10 = SD10.LD_STA_STU10
					WHERE
						SD10.LC_STA_STU10 = 'A'
						AND SD10.LC_REA_SCL_SPR = '05'
						AND SD10.LD_SCL_SPR <= DATEADD(DAY,-5,@TODAY)
				) SD10
					ON SD10.LF_STU_SSN = LN10.LF_STU_SSN
				LEFT JOIN 
				(--exclude TLLAX
					SELECT 
						BF_SSN, 
						LD_ATY_REQ_RCV
					FROM 
						UDW..AY10_BR_LON_ATY
					WHERE
						PF_REQ_ACT = 'TLLAX'
						AND LC_STA_ACTY10 = 'A' 
				) EXCLU
					ON LN10.BF_SSN = EXCLU.BF_SSN
					AND EXCLU.LD_ATY_REQ_RCV >= SD10.LD_SCL_SPR
			WHERE 
				LN10.IC_LON_PGM = 'TILP'
				AND LN10.LA_CUR_PRI > 0.00
				AND LN10.LC_STA_LON10 = 'R'
				AND PD30.DC_ADR = 'L'
				AND PD30.DI_VLD_ADR = 'Y' 
				AND PD10.DF_PRS_ID LIKE ('[0-9]%')
				AND EXCLU.BF_SSN IS NULL

			UNION ALL

			--R6: BORROWER DROPPED FROM TILP
			SELECT
				LN10.BF_SSN
				,PD10.DF_SPE_ACC_ID
				,PD10.DM_PRS_1
				,PD10.DM_PRS_LST
				,PD10.DM_PRS_LST_SFX
				,PD30.DX_STR_ADR_1
				,PD30.DX_STR_ADR_2
				,PD30.DM_CT
				,PD30.DC_DOM_ST
				,PD30.DF_ZIP_CDE
				,PD30.DM_FGN_CNY
				,PD30.DC_DOM_ST AS STATE_CODE
				,PD30.DC_ADR
				,@30DAYS AS D_DATE
				,'R6' AS REPORT
			FROM 
				UDW..LN10_LON LN10
				INNER JOIN UDW..AY10_BR_LON_ATY AY10
					ON AY10.BF_SSN = LN10.BF_SSN
				INNER JOIN UDW..PD10_PRS_NME PD10
					ON LN10.BF_SSN = PD10.DF_PRS_ID
				INNER JOIN UDW..PD30_PRS_ADR PD30
					ON LN10.BF_SSN = PD30.DF_PRS_ID
				LEFT JOIN 
				(--exclude TLPDP
					SELECT 
						BF_SSN, 
						LD_ATY_REQ_RCV
					FROM 
						UDW..AY10_BR_LON_ATY AY10 
					WHERE
						PF_REQ_ACT = 'TLPDP'
						AND LC_STA_ACTY10 = 'A' 
				) EXCLU
					ON LN10.BF_SSN = EXCLU.BF_SSN
					AND EXCLU.LD_ATY_REQ_RCV >= AY10.LD_ATY_REQ_RCV
			WHERE 
				LN10.IC_LON_PGM = 'TILP'
				AND LN10.LA_CUR_PRI > 0.00
				AND LN10.LC_STA_LON10 = 'R'
				AND PD30.DC_ADR = 'L'
				AND PD30.DI_VLD_ADR = 'Y' 
				AND PD10.DF_PRS_ID LIKE ('[0-9]%')
				AND AY10.PF_REQ_ACT = 'TLPDR'
				AND AY10.LC_STA_ACTY10 = 'A' 
				AND AY10.LD_ATY_REQ_RCV <= @TODAY
				AND EXCLU.BF_SSN IS NULL

			UNION ALL

			--R7: BORROWER NEVER ACCEPTED TILP
			SELECT
				LN10.BF_SSN
				,PD10.DF_SPE_ACC_ID
				,PD10.DM_PRS_1
				,PD10.DM_PRS_LST
				,PD10.DM_PRS_LST_SFX
				,PD30.DX_STR_ADR_1
				,PD30.DX_STR_ADR_2
				,PD30.DM_CT
				,PD30.DC_DOM_ST
				,PD30.DF_ZIP_CDE
				,PD30.DM_FGN_CNY
				,PD30.DC_DOM_ST AS STATE_CODE
				,PD30.DC_ADR
				,@30DAYS AS D_DATE
				,'R7' AS REPORT
			FROM 
				UDW..LN10_LON LN10
				INNER JOIN UDW..AY10_BR_LON_ATY AY10
					ON AY10.BF_SSN = LN10.BF_SSN
				INNER JOIN UDW..PD10_PRS_NME PD10
					ON LN10.BF_SSN = PD10.DF_PRS_ID
				INNER JOIN UDW..PD30_PRS_ADR PD30
					ON LN10.BF_SSN = PD30.DF_PRS_ID
				INNER JOIN 
				(--gets 6+ TILP 02 records
					SELECT
						LN10.BF_SSN 
					FROM
						UDW..LN10_LON LN10
						INNER JOIN UDW..DW01_DW_CLC_CLU DW01
							ON LN10.BF_SSN = DW01.BF_SSN
							AND LN10.LN_SEQ = DW01.LN_SEQ
					WHERE
						LN10.IC_LON_PGM = 'TILP'
						AND DW01.WC_DW_LON_STA = '02' --in school
						AND LN10.LC_STA_LON10 = 'R'
						AND LN10.LA_CUR_PRI > 0.00 
					GROUP BY 
						LN10.BF_SSN
					HAVING 
						COUNT(*) >= 6
				) InSchool
					ON InSchool.BF_SSN = LN10.BF_SSN
				LEFT JOIN 
				(--exclude TLBNA
					SELECT 
						BF_SSN, 
						LD_ATY_REQ_RCV
					FROM
						UDW..AY10_BR_LON_ATY AY10 
					WHERE
						PF_REQ_ACT = 'TLBNA'
						AND LC_STA_ACTY10 = 'A' 
				) EXCLU
					ON LN10.BF_SSN = EXCLU.BF_SSN
					AND EXCLU.LD_ATY_REQ_RCV >= AY10.LD_ATY_REQ_RCV
			WHERE 
				LN10.IC_LON_PGM = 'TILP'
				AND LN10.LA_CUR_PRI > 0.00
				AND LN10.LC_STA_LON10 = 'R'
				AND PD30.DC_ADR = 'L'
				AND PD30.DI_VLD_ADR = 'Y' 
				AND PD10.DF_PRS_ID LIKE ('[0-9]%')
				AND AY10.PF_REQ_ACT = 'TLPNA'
				AND AY10.LC_STA_ACTY10 = 'A' 
				AND AY10.LD_ATY_REQ_RCV <= @TODAY
				AND EXCLU.BF_SSN IS NULL

			UNION ALL

			--R8: ONE YEAR GRACE USED
			SELECT
				LN10.BF_SSN
				,PD10.DF_SPE_ACC_ID
				,PD10.DM_PRS_1
				,PD10.DM_PRS_LST
				,PD10.DM_PRS_LST_SFX
				,PD30.DX_STR_ADR_1
				,PD30.DX_STR_ADR_2
				,PD30.DM_CT
				,PD30.DC_DOM_ST
				,PD30.DF_ZIP_CDE
				,PD30.DM_FGN_CNY
				,PD30.DC_DOM_ST AS STATE_CODE
				,PD30.DC_ADR
				,DATEADD(DAY,60,@TODAY) AS D_DATE
				,'R8' AS REPORT
			FROM
				UDW..LN10_LON LN10
				INNER JOIN UDW..PD10_PRS_NME PD10
					ON LN10.BF_SSN = PD10.DF_PRS_ID
				INNER JOIN UDW..PD30_PRS_ADR PD30
					ON LN10.BF_SSN = PD30.DF_PRS_ID
				INNER JOIN UDW..DW01_DW_CLC_CLU DW01
					ON DW01.BF_SSN = LN10.BF_SSN
					AND DW01.LN_SEQ = LN10.LN_SEQ
				INNER JOIN UDW..DF10_BR_DFR_REQ DF10
					ON DF10.BF_SSN = LN10.BF_SSN
				INNER JOIN UDW..LN50_BR_DFR_APV LN50
					ON DF10.BF_SSN = LN50.BF_SSN
					AND DF10.LF_DFR_CTL_NUM = LN50.LF_DFR_CTL_NUM
				INNER JOIN 
				(--gets A 01 records
					SELECT 
						LF_STU_SSN, 
						MAX(LD_STA_STU10) AS LD_STA_STU10
					FROM 
						UDW..SD10_STU_SPR 
					WHERE
						LC_STA_STU10 = 'A'
						AND LC_REA_SCL_SPR = '01'
					GROUP BY
						LF_STU_SSN
				) SD10
					ON SD10.LF_STU_SSN = LN10.LF_STU_SSN
				LEFT JOIN 
				(--exclude TI1YR
					SELECT 
						BF_SSN
					FROM 
						UDW..AY10_BR_LON_ATY 
					WHERE
						PF_REQ_ACT = 'TI1YR'
						AND LC_STA_ACTY10 = 'A'
				) EXCLU
					ON LN10.BF_SSN = EXCLU.BF_SSN
			WHERE 
				LN10.IC_LON_PGM = 'TILP'
				AND LN10.LA_CUR_PRI > 0.00
				AND LN10.LC_STA_LON10 = 'R'
				AND PD30.DC_ADR = 'L'
				AND PD30.DI_VLD_ADR = 'Y'
				AND PD10.DF_PRS_ID LIKE ('[0-9]%')
				AND DW01.WC_DW_LON_STA = '04'
				AND DF10.LC_DFR_TYP = '33'
				AND DF10.LC_STA_DFR10 = 'A'
				AND DF10.LC_DFR_STA = 'A' 
				AND LN50.LC_STA_LON50 = 'A'
				AND LN50.LC_DFR_RSP != '003'
				AND LN50.LD_DFR_BEG < DATEADD(DAY,-365,@TODAY)
				AND EXCLU.BF_SSN IS NULL

			UNION ALL

			--R9: MAXIMUM SEMESTERS ALLOTTED
			SELECT
				LN10.BF_SSN
				,PD10.DF_SPE_ACC_ID
				,PD10.DM_PRS_1
				,PD10.DM_PRS_LST
				,PD10.DM_PRS_LST_SFX
				,PD30.DX_STR_ADR_1
				,PD30.DX_STR_ADR_2
				,PD30.DM_CT
				,PD30.DC_DOM_ST
				,PD30.DF_ZIP_CDE
				,PD30.DM_FGN_CNY
				,PD30.DC_DOM_ST AS STATE_CODE
				,PD30.DC_ADR
				,@30DAYS AS D_DATE
				,'R9' AS REPORT
			FROM 
				UDW..LN10_LON LN10
				INNER JOIN UDW..PD10_PRS_NME PD10
					ON LN10.BF_SSN = PD10.DF_PRS_ID
				INNER JOIN UDW..PD30_PRS_ADR PD30
					ON LN10.BF_SSN = PD30.DF_PRS_ID
				INNER JOIN UDW..DW01_DW_CLC_CLU DW01
					ON DW01.BF_SSN = LN10.BF_SSN
					AND DW01.LN_SEQ = LN10.LN_SEQ
				INNER JOIN 
				(--get records 4+ months in future
					SELECT
						SD10.LF_STU_SSN, 
						SD10.LD_STA_STU10, 
						SD10.LD_SCL_SPR
					FROM 
						UDW..SD10_STU_SPR SD10
						INNER JOIN
						(
							SELECT
								LF_STU_SSN,
								MAX(LD_STA_STU10) AS LD_STA_STU10
							FROM
								UDW..SD10_STU_SPR
							WHERE
								LC_STA_STU10 = 'A'
								AND LC_REA_SCL_SPR = '05'
								AND LD_SCL_SPR >= DATEADD(MONTH,4,@TODAY)
							GROUP BY
								LF_STU_SSN
						) MaxSD10
							ON MaxSD10.LF_STU_SSN = SD10.LF_STU_SSN
							AND MaxSD10.LD_STA_STU10 = SD10.LD_STA_STU10
					WHERE
						SD10.LC_STA_STU10 = 'A'
						AND SD10.LC_REA_SCL_SPR = '05'
						AND SD10.LD_SCL_SPR >= DATEADD(MONTH,4,@TODAY)
				) SD10
					ON SD10.LF_STU_SSN = LN10.LF_STU_SSN
				INNER JOIN 
				(--gets 8+ TILP records
					SELECT 
						BF_SSN 
					FROM 
						UDW..LN10_LON 
					WHERE
						 IC_LON_PGM = 'TILP'
						 AND LA_CUR_PRI > 0.00	
						 AND LC_STA_LON10 = 'R' 
					GROUP BY 
						BF_SSN
					HAVING
						COUNT(*) >= 8
				) TILP8
					ON LN10.BF_SSN = TILP8.BF_SSN
				LEFT JOIN 
				(--exclude TLMSA
					SELECT 
						BF_SSN, 
						LD_ATY_REQ_RCV
					FROM 
						UDW..AY10_BR_LON_ATY
					WHERE
						PF_REQ_ACT = 'TLMSA'
						AND LC_STA_ACTY10 = 'A' 
				) EXCLU
					ON LN10.BF_SSN = EXCLU.BF_SSN
			WHERE 
				LN10.IC_LON_PGM = 'TILP'
				AND LN10.LA_CUR_PRI > 0.00
				AND LN10.LC_STA_LON10 = 'R'
				AND PD30.DC_ADR = 'L'
				AND PD30.DI_VLD_ADR = 'Y' 
				AND PD10.DF_PRS_ID LIKE ('[0-9]%')
				AND DW01.WC_DW_LON_STA = '02'
				AND EXCLU.BF_SSN IS NULL

			UNION ALL

			--R10: TEACHING STATUS FOLLOW-UP LETTER
			SELECT
				LN10.BF_SSN
				,PD10.DF_SPE_ACC_ID
				,PD10.DM_PRS_1
				,PD10.DM_PRS_LST
				,PD10.DM_PRS_LST_SFX
				,PD30.DX_STR_ADR_1
				,PD30.DX_STR_ADR_2
				,PD30.DM_CT
				,PD30.DC_DOM_ST
				,PD30.DF_ZIP_CDE
				,PD30.DM_FGN_CNY
				,PD30.DC_DOM_ST AS STATE_CODE
				,PD30.DC_ADR
				,NULL AS D_DATE
				,'R10' AS REPORT
			FROM
				UDW..LN10_LON LN10
				INNER JOIN UDW..PD10_PRS_NME PD10
					ON LN10.BF_SSN = PD10.DF_PRS_ID
				INNER JOIN UDW..PD30_PRS_ADR PD30
					ON LN10.BF_SSN = PD30.DF_PRS_ID
				INNER JOIN 
				(--get TI1YR 60+ days old
					SELECT
						BF_SSN 
					FROM 
						UDW..AY10_BR_LON_ATY 
					WHERE
						PF_REQ_ACT = 'TI1YR'
						AND LC_STA_ACTY10 = 'A' 
						AND LD_ATY_REQ_RCV <= DATEADD(DAY,-60,@TODAY)
				) AY10
					ON LN10.BF_SSN = AY10.BF_SSN
				LEFT JOIN 
				(--exclude these arcs
					SELECT 
						BF_SSN, 
						LD_ATY_REQ_RCV
					FROM
						UDW..AY10_BR_LON_ATY 
					WHERE
						PF_REQ_ACT IN ('TI1RT','TI1FU')
						AND LC_STA_ACTY10 = 'A' 
				) EXCLU
					ON LN10.BF_SSN = EXCLU.BF_SSN
			WHERE 
				LN10.IC_LON_PGM = 'TILP'
				AND LN10.LA_CUR_PRI > 0.00
				AND LN10.LC_STA_LON10 = 'R'
				AND PD30.DC_ADR = 'L'
				AND PD30.DI_VLD_ADR = 'Y' 
				AND PD10.DF_PRS_ID LIKE ('[0-9]%')
				AND EXCLU.BF_SSN IS NULL
		) ALLPOP
		;
		--select * from #BASE_POP order by DF_SPE_ACC_ID--TEST

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
			BP.DF_SPE_ACC_ID AS AccountNumber,
			COALESCE(EM.EmailAddress,'Ecorr@uheaa.org') AS EmailAddress,
			SD.ScriptDataId,
			CASE
				WHEN BP.REPORT IN ('R2','R10')
				THEN CONCAT(
								BF_SSN
								,',',DF_SPE_ACC_ID
								,',',DM_PRS_1
								,',',RTRIM(DM_PRS_LST)
								,',',DX_STR_ADR_1
								,',',DX_STR_ADR_2
								,',',DM_CT
								,',',RTRIM(DC_DOM_ST)
								,',',DF_ZIP_CDE
								,',',DM_FGN_CNY
								,',',ACSKEY
								,',',STATE_CODE
								,',',BP.CCC
							)
				ELSE CONCAT(
								BF_SSN
								,',',DF_SPE_ACC_ID
								,',',DM_PRS_1
								,',',RTRIM(DM_PRS_LST)
								,',',DX_STR_ADR_1
								,',',DX_STR_ADR_2
								,',',DM_CT
								,',',RTRIM(DC_DOM_ST)
								,',',DF_ZIP_CDE
								,',',DM_FGN_CNY
								,',',D_DATE
								,',',ACSKEY
								,',',STATE_CODE
								,',',BP.CCC
						)
			END AS LetterData,
			BP.CCC AS CostCenter,
			0 AS InValidAddress,
			0 AS DoNotProcessEcorr,
			0 AS OnEcorr,
			1 AS ArcNeeded,
			0 AS ImagingNeeded,
			@NOW AS AddedAt,
			@JobId AS AddedBy
		FROM
			#BASE_POP BP
			INNER JOIN 
			(--get ScriptDataId
				SELECT
					SD.ScriptDataId,
					CASE L.Letter
						WHEN 'STATNTRCV'  THEN 'R10'
						WHEN 'CNGRTS'	  THEN 'R2'
						WHEN 'NOTCHLS'	  THEN 'R3'
						WHEN 'BELOAWLEXP' THEN 'R4'
						WHEN 'BELOAEXPAY' THEN 'R5'
						WHEN 'TILPSTATUS' THEN 'R6'
						WHEN 'TILPSIXSEM' THEN 'R7'
						WHEN 'TCHSTCVLT'  THEN 'R8'
						WHEN 'TILPMAXSEM' THEN 'R9'
					END AS REPORT
				FROM 
					ULS.[print].ScriptData SD
					INNER JOIN ULS.[print].Letters L
						ON SD.LetterId = L.LetterId
				WHERE 
					SD.ScriptId = 'TILPLTRS'
					AND COALESCE(SD.SourceFile,'') NOT LIKE 'ULWS26%'
			) SD
				ON SD.REPORT = BP.REPORT
			LEFT JOIN UDW.calc.EmailAddress EM
				ON BP.BF_SSN = EM.DF_PRS_ID
			LEFT JOIN ULS.[print].PrintProcessing ExistingData
				ON ExistingData.AccountNumber = BP.DF_SPE_ACC_ID
				AND ExistingData.ScriptDataId = SD.ScriptDataId
				AND CONVERT(DATE,ExistingData.AddedAt) = @TODAY
				AND 
				(
					ExistingData.EcorrDocumentCreatedAt IS NULL
					OR CAST(ExistingData.EcorrDocumentCreatedAt AS DATE) = @TODAY
				)
				AND
				(
					ExistingData.PrintedAt IS NULL
					OR CAST(ExistingData.PrintedAt AS DATE) = @TODAY
				)
				AND ExistingData.DeletedAt IS NULL
		WHERE
			ExistingData.AccountNumber IS NULL
		;
		--select * from uls.[print].PrintProcessing where AddedBy = 'UTLWG96' --TEST
		--select * from #BASE_POP --TEST
	COMMIT TRANSACTION
END TRY
BEGIN CATCH
	DECLARE @EM VARCHAR(4000) = @JobId + ' encountered an error.  Transaction not committed. Error: ' + (SELECT ERROR_MESSAGE());

	ROLLBACK TRANSACTION;

	DECLARE @ProcessLogId INT;
	DECLARE @ProcessNotificationId INT;
	DECLARE @NotificationTypeId INT = (SELECT TOP 1 NotificationTypeId FROM ProcessLogs..NotificationTypes WHERE NotificationTypeDescription = 'Error Report'); --Error report
	DECLARE @NotificationSeverityTypeId INT = (SELECT TOP 1 NotificationSeverityTypeId FROM ProcessLogs..NotificationSeverityTypes WHERE NotificationSeverityTypeDescription = 'Critical'); --Critical
		
	INSERT INTO ProcessLogs..ProcessLogs (StartedOn, EndedOn, ScriptId, Region, RunBy) VALUES(@NOW,@NOW,@JobId,'uheaa',SUSER_SNAME());
	SET @ProcessLogId = SCOPE_IDENTITY()

	INSERT INTO ProcessLogs..ProcessNotifications (NotificationTypeId,NotificationSeverityTypeId,ProcessLogId, ResolvedAt, ResolvedBy) VALUES(@NotificationTypeId,@NotificationSeverityTypeId,@ProcessLogId, NULL, NULL)
	SET @ProcessNotificationId = SCOPE_IDENTITY()

	INSERT INTO ULS.[log].ProcessLogMessages (ProcessNotificationId, LogMessage) VALUES(@ProcessNotificationId,@EM);

	THROW;
END CATCH;