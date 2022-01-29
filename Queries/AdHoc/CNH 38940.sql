USE CDW;
GO

DECLARE @TODAY DATE = GETDATE();

SELECT DISTINCT
	--AYXX.BF_SSN,
	PDXX.DF_SPE_ACC_ID
	,AYXX.PF_REQ_ACT
	,CONVERT(DATE, AYXX.LD_ATY_REQ_RCV) AS LD_ATY_REQ_RCV
FROM
	LNXX_LON LNXX
	INNER JOIN PDXX_PRS_NME PDXX
		ON LNXX.BF_SSN = PDXX.DF_PRS_ID
	INNER JOIN AYXX_BR_LON_ATY AYXX
		ON LNXX.BF_SSN = AYXX.BF_SSN
	INNER JOIN LNXX_LON_ATY LNXX
		ON AYXX.BF_SSN = LNXX.BF_SSN
		AND AYXX.LN_ATY_SEQ = LNXX.LN_ATY_SEQ
	INNER JOIN DWXX_DW_CLC_CLU DWXX
		ON LNXX.BF_SSN = DWXX.BF_SSN
		AND LNXX.LN_SEQ = DWXX.LN_SEQ
		AND DWXX.WC_DW_LON_STA <> 'XX' --<>not in school
	LEFT JOIN 
	(--deferment exclusions:
		SELECT DISTINCT
			LNXXX.BF_SSN
			,LNXXX.LN_SEQ
			,LNXXX.LD_DFR_BEG
		FROM
			LNXX_BR_DFR_APV LNXXX
			INNER JOIN DFXX_BR_DFR_REQ DFXXX
				ON LNXXX.BF_SSN = DFXXX.BF_SSN
				AND LNXXX.LF_DFR_CTL_NUM = DFXXX.LF_DFR_CTL_NUM
		WHERE
			LNXXX.LC_DFR_RSP != 'XXX' --denials
			AND LNXXX.LC_STA_LONXX = 'A'
			AND DFXXX.LC_STA_DFRXX = 'A'
			AND DFXXX.LC_DFR_STA = 'A'
	) LNXXX
		ON LNXX.BF_SSN = LNXXX.BF_SSN
		AND LNXX.LN_SEQ = LNXXX.LN_SEQ
		AND LNXXX.LD_DFR_BEG = DATEADD(DAY, X, LNXX.LD_END_GRC_PRD) --forb start date
	LEFT JOIN
	(--forbearance exclusions:
		SELECT DISTINCT
			LNXXX.BF_SSN
			,LNXXX.LN_SEQ
			,LNXXX.LD_FOR_BEG
		FROM 
			LNXX_BR_FOR_APV LNXXX
			INNER JOIN FBXX_BR_FOR_REQ FBXXX
				ON LNXXX.BF_SSN = FBXXX.BF_SSN
				AND LNXXX.LF_FOR_CTL_NUM = FBXXX.LF_FOR_CTL_NUM
		WHERE
			LNXXX.LC_FOR_RSP != 'XXX' --denials
			AND LNXXX.LC_STA_LONXX = 'A'
			AND FBXXX.LC_STA_FORXX = 'A'
			AND FBXXX.LC_FOR_STA = 'A'
	) LNXXX
		ON LNXX.BF_SSN = LNXXX.BF_SSN
		AND LNXX.LN_SEQ = LNXXX.LN_SEQ
		AND LNXXX.LD_FOR_BEG = DATEADD(DAY, X, LNXX.LD_END_GRC_PRD) --forb start date
	LEFT JOIN
	(--exclude active IDR with $X payment
		SELECT DISTINCT
			BF_SSN
			,SUM(LA_RPS_ISL) OVER(PARTITION BY BF_SSN) AS sum_LA_RPS_ISL
			,DATEADD(MONTH, LN_RPS_TRM, LD_RPS_X_PAY_DU) AS RepaymentEndDate
		FROM
			calc.RepaymentSchedules 
		WHERE
			LC_TYP_SCH_DIS IN ('IB','CX','CX','CX','CA','IA','IX','IX') --PFH
			AND CurrentGradation = X
	) IDRX
		ON AYXX.BF_SSN = IDRX.BF_SSN
		AND IDRX.sum_LA_RPS_ISL = X.XX
		AND IDRX.RepaymentEndDate > DATEADD(DAY, XX, AYXX.LD_ATY_REQ_RCV)
	LEFT JOIN
	(--exclude collection suspension already applied >= CSREQX DATE
		SELECT
			LNXX.BF_SSN,
			MAX(LNXX.LD_FOR_APL) AS max_LD_FOR_APL
		FROM
			LNXX_BR_FOR_APV LNXX
			INNER JOIN FBXX_BR_FOR_REQ FBXX
				ON LNXX.BF_SSN = FBXX.BF_SSN
				AND LNXX.LF_FOR_CTL_NUM = FBXX.LF_FOR_CTL_NUM
		WHERE
			LNXX.LC_STA_LONXX = 'A'
			AND FBXX.LC_FOR_STA = 'A'
			AND FBXX.LC_STA_FORXX = 'A'
			AND FBXX.LC_FOR_TYP = XX --Collection Suspension
		GROUP BY
			LNXX.BF_SSN
	) COL_SUSP
		ON LNXX.BF_SSN = COL_SUSP.BF_SSN
	LEFT JOIN
	(--exclude if ARC date is GT LNXX grace date
		SELECT
			BF_SSN,
			MAX(LD_END_GRC_PRD) AS max_LD_END_GRC_PRD
		FROM
			LNXX_LON
		WHERE
			LA_CUR_PRI > X.XX
			AND LC_STA_LONXX = 'R'
		GROUP BY
			BF_SSN
	) GRACE
		ON LNXX.BF_SSN = GRACE.BF_SSN
WHERE
	AYXX.PF_REQ_ACT = 'CSRQX'
	AND AYXX.LC_STA_ACTYXX = 'A'
	AND LNXX.LA_CUR_PRI > X.XX
	AND LNXX.LC_STA_LONXX = 'R'
	AND LNXXX.BF_SSN IS NULL
	AND LNXXX.BF_SSN IS NULL
	AND IDRX.BF_SSN IS NULL
	AND COL_SUSP.max_LD_FOR_APL < AYXX.LD_ATY_REQ_RCV
	AND GRACE.max_LD_END_GRC_PRD <= DATEADD(DAY,XX,AYXX.LD_ATY_REQ_RCV)
;




----CSFOLFED.sql: CLOSED SCHOOL FOLLOW UP - FED
--USE CDW;
--GO

--DROP TABLE IF EXISTS #BASE_POP, #CSE_BASE_POP, #CSE_DATASETS, #FORBPROCESSING;

----BEGIN TRY
----	BEGIN TRANSACTION
		
--		DECLARE @TODAY DATE = GETDATE();
		
--		--base population:
--		SELECT DISTINCT
--			 PDXX.DF_SPE_ACC_ID
--			,LNXX.BF_SSN
--			,LNXX.LN_SEQ
--			,MIN_CSREQ.min_LN_ATY_SEQ
--			,MIN_CSREQ.min_LD_ATY_REQ_RCV
--			,IIF(LNXX.IC_LON_PGM IN ('DPLUS','DLPLUS','DLPCNS'), X, X) AS SelectAllLoans
--		--INTO
--		--	#BASE_POP
--		FROM
--			LNXX_LON LNXX
--			INNER JOIN PDXX_PRS_NME PDXX
--				ON LNXX.BF_SSN = PDXX.DF_PRS_ID
--			INNER JOIN LNXX_LON_ATY LNXX
--				ON LNXX.BF_SSN = LNXX.BF_SSN
--				AND LNXX.LN_SEQ = LNXX.LN_SEQ
--			INNER JOIN 
--			(--get min CSREQ arc
--				SELECT DISTINCT
--					 AYXX.BF_SSN
--					,LNXX.LN_SEQ
--					,MIN(AYXX.LN_ATY_SEQ) AS min_LN_ATY_SEQ
--					,CONVERT(DATE, MIN(AYXX.LD_ATY_REQ_RCV)) AS min_LD_ATY_REQ_RCV
--				FROM
--					AYXX_BR_LON_ATY AYXX
--					INNER JOIN LNXX_LON_ATY LNXX
--						ON AYXX.BF_SSN = LNXX.BF_SSN
--						AND AYXX.LN_ATY_SEQ = LNXX.LN_ATY_SEQ
--				WHERE
--					AYXX.PF_REQ_ACT = 'CSREQ'
--					AND AYXX.LC_STA_ACTYXX = 'A'
--					AND AYXX.LD_ATY_REQ_RCV >= CONVERT(DATETIME,'XXXXXXXX') --exclude CSREQ before XX/XX/XXXX
--				GROUP BY
--					 AYXX.BF_SSN
--					,LNXX.LN_SEQ
--			) MIN_CSREQ
--				ON LNXX.BF_SSN = MIN_CSREQ.BF_SSN
--				AND LNXX.LN_SEQ = MIN_CSREQ.LN_SEQ
--				AND LNXX.LN_ATY_SEQ = MIN_CSREQ.min_LN_ATY_SEQ
--			LEFT JOIN 
--			(--exclude CSRQX
--				SELECT DISTINCT
--					 AYXX.BF_SSN
--					,LNXX.LN_SEQ
--					,AYXX.LN_ATY_SEQ
--					,CONVERT(DATE, AYXX.LD_ATY_REQ_RCV) AS LD_ATY_REQ_RCV
--				FROM
--					AYXX_BR_LON_ATY AYXX
--					INNER JOIN LNXX_LON_ATY LNXX
--						ON AYXX.BF_SSN = LNXX.BF_SSN
--						AND AYXX.LN_ATY_SEQ = LNXX.LN_ATY_SEQ
--				WHERE
--					AYXX.PF_REQ_ACT = 'CSRQX'
--					AND AYXX.LC_STA_ACTYXX = 'A'

--				and ayXX.BF_SSN in ('XXXXXXXXX','XXXXXXXXX')

--			) X_CSRQX
--				ON MIN_CSREQ.BF_SSN = X_CSRQX.BF_SSN
--				AND MIN_CSREQ.LN_SEQ = X_CSRQX.LN_SEQ
--				AND X_CSRQX.LD_ATY_REQ_RCV >= MIN_CSREQ.min_LD_ATY_REQ_RCV
--				AND X_CSRQX.LD_ATY_REQ_RCV <= @TODAY
--			LEFT JOIN
--			(--exclude ADCSH/DICSK
--				SELECT DISTINCT
--					 BF_SSN
--					,CONVERT(DATE, LD_ATY_REQ_RCV) AS LD_ATY_REQ_RCV
--				FROM
--					AYXX_BR_LON_ATY
--				WHERE
--					PF_REQ_ACT IN ('ADCSH','DICSK')
--			) X_ADCSHDICSK
--				ON LNXX.BF_SSN = X_ADCSHDICSK.BF_SSN
--				AND X_ADCSHDICSK.LD_ATY_REQ_RCV >= MIN_CSREQ.min_LD_ATY_REQ_RCV
--				AND X_ADCSHDICSK.LD_ATY_REQ_RCV <= @TODAY
--		WHERE
--			LNXX.LA_CUR_PRI > X.XX
--			AND LNXX.LC_STA_LONXX = 'R'
--			AND MIN_CSREQ.min_LD_ATY_REQ_RCV <= DATEADD(DAY, -XX, @TODAY)
--			--AND X_CSRQX.BF_SSN IS NULL --exclude CSRQX
--			AND X_ADCSHDICSK.BF_SSN IS NULL --exclude ADCSH/DICSK

--			and pdXX.DF_SPE_ACC_ID in ('XXXXXXXXXX','XXXXXXXXXX')
--		;
--		--select * from #BASE_POP order by min_LD_ATY_REQ_RCV --TEST
		
--		--Follow up letter data set:
--		DECLARE @ScriptId VARCHAR(XX) = 'CLSCNOTFED'; --letter ID
--		DECLARE @ScriptDataId INT = (SELECT ScriptDataId FROM CLS.[print].ScriptData WHERE ScriptId = @ScriptId)
--				,@AddedBy VARCHAR(X) = 'CSFOLFED'; --Sacker SAS ID

--		INSERT INTO CLS.[print].PrintProcessing
--		(
--			 AccountNumber
--			,EmailAddress
--			,ScriptDataId
--			,SourceFile
--			,LetterData
--			,CostCenter
--			,DoNotProcessEcorr
--			,OnEcorr
--			,ArcNeeded
--			,ImagingNeeded
--			,AddedBy
--			,AddedAt
--		)
--		SELECT DISTINCT
--			 BP.DF_SPE_ACC_ID AS AccountNumber
--			,COALESCE(PHXX.DX_CNC_EML_ADR,'Ecorr@MyCornerStoneLoan.org') AS EmailAddress
--			,@ScriptDataId AS ScriptDataId
--			,'' AS SourceFile
--			,COALESCE(LTRIM(RTRIM(CentralData.dbo.CreateACSKeyline(BP.BF_SSN, 'B', PDXX.DC_ADR))),'')
--			+ ',' + COALESCE(LTRIM(RTRIM(PDXX.DM_PRS_X)),'')
--			+ ' ' + COALESCE(LTRIM(RTRIM(PDXX.DM_PRS_LST)),'')
--			+ ',' + COALESCE(LTRIM(RTRIM(PDXX.DX_STR_ADR_X)),'')
--			+ ',' + COALESCE(LTRIM(RTRIM(PDXX.DX_STR_ADR_X)),'')
--			+ ',' + COALESCE(LTRIM(RTRIM(PDXX.DM_CT)),'')
--			+ ',' + COALESCE(LTRIM(RTRIM(PDXX.DC_DOM_ST)),'')
--			+ ',' + COALESCE(LTRIM(RTRIM(PDXX.DF_ZIP_CDE)),'')
--			+ ',' + COALESCE(LTRIM(RTRIM(PDXX.DM_FGN_ST)),'')
--			+ ',' + COALESCE(LTRIM(RTRIM(PDXX.DM_FGN_CNY)),'')
--			+ ',' + COALESCE(LTRIM(RTRIM(PDXX.DF_SPE_ACC_ID)),'')
--			+ ',' + 'MAXXXX' AS LetterData
--			,'MAXXXX' AS CostCenter
--			,SD.DoNotProcessEcorr
--			,IIF(PHXX.DI_CNC_ELT_OPI = 'Y' AND PDXX.DI_VLD_ADR = 'Y', X, X) AS OnEcorr
--			,IIF(SDM.ArcId IS NULL, X, X) AS ArcNeeded
--			,IIF(SD.DocIdId IS NULL, X, X) AS ImagingNeeded
--			,@AddedBy
--			,GETDATE() AS AddedAt
--		FROM
--			#BASE_POP BP
--			INNER JOIN CDW..PDXX_PRS_NME PDXX
--				ON PDXX.DF_PRS_ID = BP.BF_SSN
--			INNER JOIN CDW..PDXX_PRS_ADR PDXX
--				ON PDXX.DF_PRS_ID = PDXX.DF_PRS_ID
--				AND PDXX.DC_ADR = 'L' --legal address
--			INNER JOIN CLS.[print].ScriptData SD
--				ON SD.ScriptId = @ScriptId
--				AND SD.ScriptDataId = @ScriptDataId
--			INNER JOIN CLS.[print].ArcScriptDataMapping SDM
--				ON SDM.ScriptDataId = SD.ScriptDataId
--			LEFT JOIN CDW..PHXX_CNC_EML PHXX 
--				ON PHXX.DF_SPE_ID = BP.DF_SPE_ACC_ID
--				AND PHXX.DI_VLD_CNC_EML_ADR = 'Y'
--			LEFT JOIN CLS.[print].PrintProcessing EXISTING_DATA
--				ON  EXISTING_DATA.AccountNumber = BP.DF_SPE_ACC_ID
--				AND EXISTING_DATA.AddedBy = @AddedBy
--				AND EXISTING_DATA.AddedAt >= @TODAY
--				AND EXISTING_DATA.AddedAt < DATEADD(DAY, X, @TODAY)
--		WHERE
--			EXISTING_DATA.AccountNumber IS NULL
--		;
--		--SELECT * FROM  CLS.[print].PrintProcessing WHERE AddedBy = 'CLSCNOTFED' --TEST

--		--Collection Suspension Extension base pop
--		SELECT DISTINCT
--			 BP.DF_SPE_ACC_ID
--			,BP.BF_SSN
--			,BP.LN_SEQ
--			,BP.min_LN_ATY_SEQ
--			,BP.min_LD_ATY_REQ_RCV
--			,BP.SelectAllLoans
--		INTO
--			#CSE_BASE_POP
--		FROM
--			#BASE_POP BP
--			LEFT JOIN
--			(--exclude FBRMD between CSREQ and today
--				SELECT
--					BF_SSN
--					,LD_ATY_REQ_RCV
--				FROM
--					AYXX_BR_LON_ATY 
--				WHERE
--					PF_REQ_ACT = 'FBRMD'
--					AND LC_STA_ACTYXX = 'A'
--			) AYXX_FBRMD
--				ON BP.BF_SSN = AYXX_FBRMD.BF_SSN
--				AND AYXX_FBRMD.LD_ATY_REQ_RCV >= BP.min_LD_ATY_REQ_RCV
--				AND AYXX_FBRMD.LD_ATY_REQ_RCV <= @TODAY
--			LEFT JOIN
--			(--exclude active IDR with $X payment
--				SELECT DISTINCT
--					 BF_SSN
--					,SUM(LA_RPS_ISL) OVER(PARTITION BY BF_SSN) AS sum_LA_RPS_ISL
--					,DATEADD(MONTH, LN_RPS_TRM, LD_RPS_X_PAY_DU) AS RepaymentEndDate
--				FROM
--					calc.RepaymentSchedules 
--				WHERE
--					LC_TYP_SCH_DIS IN ('IB','CX','CX','CX','CA','IA','IX','IX') --PFH
--					AND CurrentGradation = X
--			) IDRX
--				ON BP.BF_SSN = IDRX.BF_SSN
--				AND IDRX.sum_LA_RPS_ISL = X.XX
--				AND IDRX.RepaymentEndDate > DATEADD(DAY, XX, @TODAY)
--			LEFT JOIN
--			(--exclude borrowers with PLUS and NONPLUS loans
--				SELECT DISTINCT
--					BF_SSN
--					,COUNT (DISTINCT SelectAllLoans) AS count_SelectAllLoans
--				FROM
--					#BASE_POP
--				GROUP BY
--					BF_SSN
--				HAVING
--					COUNT (DISTINCT SelectAllLoans) > X
--			) BOTH_XX
--				ON BP.BF_SSN = BOTH_XX.BF_SSN
--		WHERE
--			AYXX_FBRMD.BF_SSN IS NULL --exclude FBRMD between CSREQ and today
--			AND IDRX.BF_SSN IS NULL --exclude active IDR with $X payment
--			AND BOTH_XX.BF_SSN IS NULL --exclude borrowers with PLUS and NONPLUS loans
--		;
--		--select * from #CSE_BASE_POP --TEST

--		--Collection Suspension Extension breakdown into distinct datasets
--		SELECT DISTINCT
--			 CSE_DATASETS.DF_SPE_ACC_ID
--			,CSE_DATASETS.BF_SSN
--			,CSE_DATASETS.LN_SEQ
--			,CSE_DATASETS.StartDate
--			,CSE_DATASETS.SelectAllLoans
--			,CSE_DATASETS.[Status]
--			,CSE_DATASETS.ProcessOn
--		INTO
--			#CSE_DATASETS
--		FROM
--			(--IDR/non-IDR repayment greater than X
--				SELECT DISTINCT
--					 NONIDR.DF_SPE_ACC_ID
--					,NONIDR.BF_SSN
--					,NONIDR.LN_SEQ
--					,@TODAY AS StartDate
--					,NONIDR.SelectAllLoans
--					,'IDR/non-IDR > X' AS [Status]
--					,@TODAY AS ProcessOn
--				FROM
--					(--non IDR repayment
--						SELECT DISTINCT
--							 CSE.DF_SPE_ACC_ID
--							,CSE.BF_SSN
--							,CSE.LN_SEQ
--							,CSE.min_LN_ATY_SEQ
--							,CSE.min_LD_ATY_REQ_RCV
--							,CSE.SelectAllLoans
--							,SUM(RS.LA_RPS_ISL) OVER(PARTITION BY RS.BF_SSN, RS.LN_SEQ) AS sum_LA_RPS_ISL
--						FROM
--							#CSE_BASE_POP CSE
--							INNER JOIN DWXX_DW_CLC_CLU DWXX
--								ON CSE.BF_SSN = DWXX.BF_SSN
--								AND CSE.LN_SEQ = DWXX.LN_SEQ
--							INNER JOIN calc.RepaymentSchedules RS
--								ON DWXX.BF_SSN = RS.BF_SSN
--								AND DWXX.LN_SEQ = RS.LN_SEQ
--						WHERE
--							DWXX.WC_DW_LON_STA = 'XX'
--							AND RS.LC_TYP_SCH_DIS NOT IN ('IB','CX','CX','CX','CA','IA','IX','IX') --not PFH
--							AND RS.CurrentGradation = X
--					) NONIDR
--				WHERE
--					NONIDR.sum_LA_RPS_ISL > X.XX
			
--				UNION ALL

--				--IDR repayment equal to X
--				SELECT DISTINCT
--					 IDRX.DF_SPE_ACC_ID
--					,IDRX.BF_SSN
--					,IDRX.LN_SEQ
--					,IDRX.StartDate
--					,IDRX.SelectAllLoans
--					,'IDR = X' AS [Status]
--					,CASE
--						WHEN IDRX.StartDate < DATEADD(DAY, XX, @TODAY)
--						THEN @TODAY
--						ELSE IDRX.StartDate
--					END AS ProcessOn
--				FROM
--					(--IDR repayment
--						SELECT DISTINCT
--							CSE.*
--							,SUM(RS.LA_RPS_ISL) OVER(PARTITION BY RS.BF_SSN) AS sum_LA_RPS_ISL
--							,DATEADD(DAY,X,(DATEADD(MONTH, LN_RPS_TRM, RS.LD_RPS_X_PAY_DU))) AS StartDate --forb start date
--						FROM
--							#CSE_BASE_POP CSE
--							INNER JOIN DWXX_DW_CLC_CLU DWXX
--								ON CSE.BF_SSN = DWXX.BF_SSN
--								AND CSE.LN_SEQ = DWXX.LN_SEQ
--							INNER JOIN calc.RepaymentSchedules RS
--								ON DWXX.BF_SSN = RS.BF_SSN
--								AND DWXX.LN_SEQ = RS.LN_SEQ
--							LEFT JOIN 
--							(--deferment exclusions:
--								SELECT DISTINCT
--									 LNXXX.BF_SSN
--									,LNXXX.LN_SEQ
--									,LNXXX.LD_DFR_BEG
--								FROM
--									LNXX_BR_DFR_APV LNXXX
--									INNER JOIN DFXX_BR_DFR_REQ DFXXX
--										ON LNXXX.BF_SSN = DFXXX.BF_SSN
--										AND LNXXX.LF_DFR_CTL_NUM = DFXXX.LF_DFR_CTL_NUM
--								WHERE
--									LNXXX.LC_DFR_RSP != 'XXX' --denials
--									AND LNXXX.LC_STA_LONXX = 'A'
--									AND DFXXX.LC_STA_DFRXX = 'A'
--									AND DFXXX.LC_DFR_STA = 'A'
--							) LNXXX
--								ON CSE.BF_SSN = LNXXX.BF_SSN
--								AND CSE.LN_SEQ = LNXXX.LN_SEQ
--								AND LNXXX.LD_DFR_BEG = DATEADD(DAY,X,(DATEADD(MONTH, LN_RPS_TRM, RS.LD_RPS_X_PAY_DU))) --forb start date
--							LEFT JOIN
--							(--forbearance exclusions:
--								SELECT DISTINCT
--									 LNXXX.BF_SSN
--									,LNXXX.LN_SEQ
--									,LNXXX.LD_FOR_BEG
--								FROM 
--									LNXX_BR_FOR_APV LNXXX
--									INNER JOIN FBXX_BR_FOR_REQ FBXXX
--										ON LNXXX.BF_SSN = FBXXX.BF_SSN
--										AND LNXXX.LF_FOR_CTL_NUM = FBXXX.LF_FOR_CTL_NUM
--								WHERE
--									LNXXX.LC_FOR_RSP != 'XXX' --denials
--									AND LNXXX.LC_STA_LONXX = 'A'
--									AND FBXXX.LC_STA_FORXX = 'A'
--									AND FBXXX.LC_FOR_STA = 'A'
--							) LNXXX
--								ON CSE.BF_SSN = LNXXX.BF_SSN
--								AND CSE.LN_SEQ = LNXXX.LN_SEQ
--								AND LNXXX.LD_FOR_BEG = DATEADD(DAY,X,(DATEADD(MONTH, LN_RPS_TRM, RS.LD_RPS_X_PAY_DU))) --forb start date
--						WHERE
--							DWXX.WC_DW_LON_STA = 'XX'
--							AND RS.LC_TYP_SCH_DIS IN ('IB','CX','CX','CX','CA','IA','IX','IX') --PFH
--							AND DATEADD(MONTH, LN_RPS_TRM, RS.LD_RPS_X_PAY_DU) < DATEADD(DAY, XX, @TODAY)
--							AND RS.CurrentGradation = X
--							AND LNXXX.BF_SSN IS NULL --remove active deferments
--							AND LNXXX.BF_SSN IS NULL --remove active forbearances
--					) IDRX
--				WHERE
--					IDRX.sum_LA_RPS_ISL = X.XX
			
--				UNION ALL
				
--				--In Grace
--				SELECT DISTINCT
--					 CSE.DF_SPE_ACC_ID
--					,CSE.BF_SSN
--					,CSE.LN_SEQ
--					,DATEADD(DAY, X, LNXX.LD_END_GRC_PRD) AS StartDate --forb start date
--					,CSE.SelectAllLoans
--					,'In Grace' AS [Status]
--					,CASE
--						WHEN DATEADD(DAY, X, LNXX.LD_END_GRC_PRD) < DATEADD(DAY, XX, @TODAY)
--						THEN @TODAY
--						ELSE DATEADD(DAY, X, LNXX.LD_END_GRC_PRD)
--					END AS ProcessOn
--				FROM
--					#CSE_BASE_POP CSE
--					INNER JOIN DWXX_DW_CLC_CLU DWXX
--						ON CSE.BF_SSN = DWXX.BF_SSN
--						AND CSE.LN_SEQ = DWXX.LN_SEQ
--					INNER JOIN LNXX_LON LNXX
--						ON CSE.BF_SSN = LNXX.BF_SSN
--						AND CSE.LN_SEQ = LNXX.LN_SEQ
--					LEFT JOIN 
--					(--deferment exclusions:
--						SELECT DISTINCT
--							 LNXXX.BF_SSN
--							,LNXXX.LN_SEQ
--							,LNXXX.LD_DFR_BEG
--						FROM
--							LNXX_BR_DFR_APV LNXXX
--							INNER JOIN DFXX_BR_DFR_REQ DFXXX
--								ON LNXXX.BF_SSN = DFXXX.BF_SSN
--								AND LNXXX.LF_DFR_CTL_NUM = DFXXX.LF_DFR_CTL_NUM
--						WHERE
--							LNXXX.LC_DFR_RSP != 'XXX' --denials
--							AND LNXXX.LC_STA_LONXX = 'A'
--							AND DFXXX.LC_STA_DFRXX = 'A'
--							AND DFXXX.LC_DFR_STA = 'A'
--					) LNXXX
--						ON CSE.BF_SSN = LNXXX.BF_SSN
--						AND CSE.LN_SEQ = LNXXX.LN_SEQ
--						AND LNXXX.LD_DFR_BEG = DATEADD(DAY, X, LNXX.LD_END_GRC_PRD) --forb start date
--					LEFT JOIN
--					(--forbearance exclusions:
--						SELECT DISTINCT
--							 LNXXX.BF_SSN
--							,LNXXX.LN_SEQ
--							,LNXXX.LD_FOR_BEG
--						FROM 
--							LNXX_BR_FOR_APV LNXXX
--							INNER JOIN FBXX_BR_FOR_REQ FBXXX
--								ON LNXXX.BF_SSN = FBXXX.BF_SSN
--								AND LNXXX.LF_FOR_CTL_NUM = FBXXX.LF_FOR_CTL_NUM
--						WHERE
--							LNXXX.LC_FOR_RSP != 'XXX' --denials
--							AND LNXXX.LC_STA_LONXX = 'A'
--							AND FBXXX.LC_STA_FORXX = 'A'
--							AND FBXXX.LC_FOR_STA = 'A'
--					) LNXXX
--						ON CSE.BF_SSN = LNXXX.BF_SSN
--						AND CSE.LN_SEQ = LNXXX.LN_SEQ
--						AND LNXXX.LD_FOR_BEG = DATEADD(DAY, X, LNXX.LD_END_GRC_PRD) --forb start date
--				WHERE
--					DWXX.WC_DW_LON_STA = 'XX' --in grace
--					AND DATEDIFF(DAY, @TODAY, LNXX.LD_END_GRC_PRD) < XX
--					AND LNXXX.BF_SSN IS NULL --remove active deferments
--					AND LNXXX.BF_SSN IS NULL --remove active forbearances
				
--				UNION ALL

--				--deferment
--				SELECT DISTINCT
--					 CSE.DF_SPE_ACC_ID
--					,CSE.BF_SSN
--					,CSE.LN_SEQ
--					,DATEADD(DAY, X, LNXX.LD_DFR_END) AS StartDate --forb start date
--					,CSE.SelectAllLoans
--					,'Deferment' AS [Status]
--					,CASE
--						WHEN DATEADD(DAY, X, LNXX.LD_DFR_END) < DATEADD(DAY, XX, @TODAY)
--						THEN @TODAY
--						ELSE DATEADD(DAY, X, LNXX.LD_DFR_END)
--					END AS ProcessOn
--				FROM
--					#CSE_BASE_POP CSE
--					INNER JOIN DWXX_DW_CLC_CLU DWXX
--						ON CSE.BF_SSN = DWXX.BF_SSN
--						AND CSE.LN_SEQ = DWXX.LN_SEQ
--					INNER JOIN LNXX_BR_DFR_APV LNXX
--						ON CSE.BF_SSN = LNXX.BF_SSN
--						AND CSE.LN_SEQ = LNXX.LN_SEQ
--					INNER JOIN DFXX_BR_DFR_REQ DFXX
--						ON LNXX.BF_SSN = DFXX.BF_SSN
--						AND LNXX.LF_DFR_CTL_NUM = DFXX.LF_DFR_CTL_NUM
--					LEFT JOIN 
--					(--deferment exclusions:
--						SELECT DISTINCT
--							 LNXXX.BF_SSN
--							,LNXXX.LN_SEQ
--							,LNXXX.LD_DFR_BEG
--						FROM
--							LNXX_BR_DFR_APV LNXXX
--							INNER JOIN DFXX_BR_DFR_REQ DFXXX
--								ON LNXXX.BF_SSN = DFXXX.BF_SSN
--								AND LNXXX.LF_DFR_CTL_NUM = DFXXX.LF_DFR_CTL_NUM
--						WHERE
--							LNXXX.LC_DFR_RSP != 'XXX' --denials
--							AND LNXXX.LC_STA_LONXX = 'A'
--							AND DFXXX.LC_STA_DFRXX = 'A'
--							AND DFXXX.LC_DFR_STA = 'A'
--					) LNXXX
--						ON CSE.BF_SSN = LNXXX.BF_SSN
--						AND CSE.LN_SEQ = LNXXX.LN_SEQ
--						AND LNXXX.LD_DFR_BEG = DATEADD(DAY, X, LNXX.LD_DFR_END) --forb start date
--					LEFT JOIN
--					(--forbearance exclusions:
--						SELECT DISTINCT
--							 LNXXX.BF_SSN
--							,LNXXX.LN_SEQ
--							,LNXXX.LD_FOR_BEG
--						FROM 
--							LNXX_BR_FOR_APV LNXXX
--							INNER JOIN FBXX_BR_FOR_REQ FBXXX
--								ON LNXXX.BF_SSN = FBXXX.BF_SSN
--								AND LNXXX.LF_FOR_CTL_NUM = FBXXX.LF_FOR_CTL_NUM
--						WHERE
--							LNXXX.LC_FOR_RSP != 'XXX' --denials
--							AND LNXXX.LC_STA_LONXX = 'A'
--							AND FBXXX.LC_STA_FORXX = 'A'
--							AND FBXXX.LC_FOR_STA = 'A'
--					) LNXXX
--						ON CSE.BF_SSN = LNXXX.BF_SSN
--						AND CSE.LN_SEQ = LNXXX.LN_SEQ
--						AND LNXXX.LD_FOR_BEG = DATEADD(DAY, X, LNXX.LD_DFR_END) --forb start date
--				WHERE
--					DWXX.WC_DW_LON_STA = 'XX'
--					AND LNXX.LC_DFR_RSP != 'XXX' --denials
--					AND LNXX.LC_STA_LONXX = 'A'
--					AND DFXX.LC_STA_DFRXX = 'A'
--					AND DFXX.LC_DFR_STA = 'A'
--					AND LNXX.LD_DFR_BEG <= @TODAY
--					AND LNXX.LD_DFR_END >= @TODAY 
--					AND DATEDIFF(DAY, @TODAY, LNXX.LD_DFR_END) < XX
--					AND LNXXX.BF_SSN IS NULL --remove active deferments
--					AND LNXXX.BF_SSN IS NULL --remove active forbearances

--				UNION ALL

--				--forbearance
--				SELECT DISTINCT
--					 CSE.DF_SPE_ACC_ID
--					,CSE.BF_SSN
--					,CSE.LN_SEQ
--					,DATEADD(DAY, X, LNXX.LD_FOR_END) AS StartDate --forb start date
--					,CSE.SelectAllLoans
--					,'Forbearance' AS [Status]
--					,CASE
--						WHEN DATEADD(DAY, X, LNXX.LD_FOR_END) < DATEADD(DAY, XX, @TODAY)
--						THEN @TODAY
--						ELSE DATEADD(DAY, X, LNXX.LD_FOR_END)
--					END AS ProcessOn
--				FROM
--					#CSE_BASE_POP CSE
--					INNER JOIN DWXX_DW_CLC_CLU DWXX
--						ON CSE.BF_SSN = DWXX.BF_SSN
--						AND CSE.LN_SEQ = DWXX.LN_SEQ
--					INNER JOIN LNXX_BR_FOR_APV LNXX
--						ON CSE.BF_SSN = LNXX.BF_SSN
--						AND CSE.LN_SEQ = LNXX.LN_SEQ
--					INNER JOIN FBXX_BR_FOR_REQ FBXX
--						ON LNXX.BF_SSN = FBXX.BF_SSN
--						AND LNXX.LF_FOR_CTL_NUM = FBXX.LF_FOR_CTL_NUM
--					LEFT JOIN 
--					(--deferment exclusions:
--						SELECT DISTINCT
--							 LNXXX.BF_SSN
--							,LNXXX.LN_SEQ
--							,LNXXX.LD_DFR_BEG
--						FROM
--							LNXX_BR_DFR_APV LNXXX
--							INNER JOIN DFXX_BR_DFR_REQ DFXXX
--								ON LNXXX.BF_SSN = DFXXX.BF_SSN
--								AND LNXXX.LF_DFR_CTL_NUM = DFXXX.LF_DFR_CTL_NUM
--						WHERE
--							LNXXX.LC_DFR_RSP != 'XXX' --denials
--							AND LNXXX.LC_STA_LONXX = 'A'
--							AND DFXXX.LC_STA_DFRXX = 'A'
--							AND DFXXX.LC_DFR_STA = 'A'
--					) LNXXX
--						ON CSE.BF_SSN = LNXXX.BF_SSN
--						AND CSE.LN_SEQ = LNXXX.LN_SEQ
--						AND LNXXX.LD_DFR_BEG = DATEADD(DAY, X, LNXX.LD_FOR_END) --forb start date
--					LEFT JOIN
--					(--forbearance exclusions:
--						SELECT DISTINCT
--							 LNXXX.BF_SSN
--							,LNXXX.LN_SEQ
--							,LNXXX.LD_FOR_BEG
--						FROM 
--							LNXX_BR_FOR_APV LNXXX
--							INNER JOIN FBXX_BR_FOR_REQ FBXXX
--								ON LNXXX.BF_SSN = FBXXX.BF_SSN
--								AND LNXXX.LF_FOR_CTL_NUM = FBXXX.LF_FOR_CTL_NUM
--						WHERE
--							LNXXX.LC_FOR_RSP != 'XXX' --denials
--							AND LNXXX.LC_STA_LONXX = 'A'
--							AND FBXXX.LC_STA_FORXX = 'A'
--							AND FBXXX.LC_FOR_STA = 'A'
--					) LNXXX
--						ON CSE.BF_SSN = LNXXX.BF_SSN
--						AND CSE.LN_SEQ = LNXXX.LN_SEQ
--						AND LNXXX.LD_FOR_BEG = DATEADD(DAY, X, LNXX.LD_FOR_END) --forb start date
--				WHERE
--					DWXX.WC_DW_LON_STA = 'XX'
--					AND LNXX.LC_FOR_RSP != 'XXX' --denials
--					AND LNXX.LC_STA_LONXX = 'A'
--					AND FBXX.LC_STA_FORXX = 'A'
--					AND FBXX.LC_FOR_STA = 'A'
--					AND LNXX.LD_FOR_BEG <= @TODAY
--					AND LNXX.LD_FOR_END >= @TODAY 
--					AND DATEDIFF(DAY, @TODAY, LNXX.LD_FOR_END) < XX
--					AND LNXXX.BF_SSN IS NULL --remove active deferments
--					AND LNXXX.BF_SSN IS NULL --remove active forbearances
--			) CSE_DATASETS				
--		;
--		--select * from #CSE_DATASETS --TEST

--		--insert to forb processing table
--		DECLARE @BusinessUnitId BIGINT = (SELECT BusinessUnitId FROM CLS.forb.BusinessUnits WHERE BusinessUnit = 'Account Services'),
--				@ForbCode VARCHAR(X) = 'F',
--				@ForbearanceType VARCHAR(X) = 'XX',
--				@ForbToClearDelq VARCHAR(X) = 'Y';

--		SELECT DISTINCT
--			 CSED.DF_SPE_ACC_ID AS AccountNumber
--			,@ForbCode AS ForbCode
--			,@TODAY AS DateRequested
--			,@ForbearanceType AS ForbearanceType
--			,CASE
--				WHEN MultiNonPlus.min_StartDate IS NOT NULL
--				THEN MultiNonPlus.min_StartDate
--				ELSE CONVERT(DATE, CSED.StartDate)
--			END AS StartDate
--			,DATEADD(DAY, XX, @TODAY) AS EndDate
--			,@TODAY AS DateCertified
--			,IIF(LNXX.LF_EDS IS NOT NULL, 'Y', NULL) AS CoMakerEligibility
--			,@ForbToClearDelq AS ForbToClearDelq
--			,CSED.SelectAllLoans
--			,@BusinessUnitId AS BusinessUnitId
--			,@AddedBy AS AddedBy
--			,GETDATE() AS AddedAt
--			,CASE
--				WHEN MultiNonPlus.min_StartDate IS NOT NULL
--				THEN MultiNonPlus.min_StartDate
--				ELSE CONVERT(DATE, CSED.ProcessOn)
--			END AS ProcessOn
--		INTO
--			#FORBPROCESSING
--		FROM
--			#CSE_DATASETS CSED
--			LEFT JOIN LNXX_EDS LNXX
--				ON CSED.BF_SSN = LNXX.BF_SSN
--				AND CSED.LN_SEQ = LNXX.LN_SEQ
--				AND LNXX.LC_STA_LONXX = 'A'
--				AND LNXX.LC_EDS_TYP = 'M' --comaker flag
--			LEFT JOIN
--			(--use min StartDate for brwrs w/multiple loans w/different start dates
--				SELECT
--					DF_SPE_ACC_ID
--					,MIN(StartDate) OVER(PARTITION BY DF_SPE_ACC_ID) AS min_StartDate
--				FROM
--					#CSE_DATASETS
--				WHERE
--					[Status] IN ('In Grace','Deferment','Forbearance')
--			) MultiNonPlus
--				ON CSED.DF_SPE_ACC_ID = MultiNonPlus.DF_SPE_ACC_ID
--		;
--		--select * from #FORBPROCESSING

--		INSERT INTO CLS.forb.ForbearanceProcessing
--		(
--			 AccountNumber
--			,ForbCode
--			,DateRequested
--			,ForbearanceType
--			,StartDate
--			,EndDate
--			,DateCertified
--			,CoMakerEligibility
--			,ForbToClearDelq
--			,SelectAllLoans
--			,BusinessUnitId
--			,AddedBy
--			,AddedAt
--			,ProcessOn
--		)
--		SELECT
--			 FP.AccountNumber
--			,FP.ForbCode
--			,FP.DateRequested
--			,FP.ForbearanceType
--			,FP.StartDate
--			,FP.EndDate
--			,FP.DateCertified
--			,FP.CoMakerEligibility
--			,FP.ForbToClearDelq
--			,FP.SelectAllLoans
--			,FP.BusinessUnitId
--			,FP.AddedBy
--			,FP.AddedAt
--			,FP.ProcessOn
--		FROM
--			#FORBPROCESSING FP
--			LEFT JOIN CLS.forb.ForbearanceProcessing EXISTING_DATA
--				 ON EXISTING_DATA.AccountNumber		= FP.AccountNumber
--				AND EXISTING_DATA.ForbCode			= FP.ForbCode
--				AND EXISTING_DATA.DateRequested		= FP.DateRequested
--				AND EXISTING_DATA.ForbearanceType	= FP.ForbearanceType
--				AND EXISTING_DATA.StartDate			= FP.StartDate
--				AND EXISTING_DATA.EndDate			= FP.EndDate
--				AND EXISTING_DATA.DateCertified		= FP.DateCertified
--				AND EXISTING_DATA.CoMakerEligibility= FP.CoMakerEligibility
--				AND EXISTING_DATA.ForbToClearDelq	= FP.ForbToClearDelq
--				AND EXISTING_DATA.SelectAllLoans	= FP.SelectAllLoans
--				AND EXISTING_DATA.BusinessUnitId	= FP.BusinessUnitId
--				AND EXISTING_DATA.AddedBy			= FP.AddedBy
--				AND EXISTING_DATA.ProcessOn			= FP.ProcessOn
--				AND EXISTING_DATA.AddedAt >= @TODAY
--				AND EXISTING_DATA.AddedAt < DATEADD(DAY, X, @TODAY)
--		WHERE
--			EXISTING_DATA.AccountNumber IS NULL
--		;
--		--select * from CLS.forb.ForbearanceProcessing where AddedBy = 'CSFOLFED' --TEST

--		--insert PLUS loans into forb selection table
--		INSERT INTO CLS.forb.ForbLoanSequenceSelection
--		(
--			 ForbearanceProcessingId
--			,LoanSequence
--		)
--		SELECT
--			 FP.ForbearanceProcessingId
--			,CSED.LN_SEQ
--		FROM
--			#FORBPROCESSING F
--			INNER JOIN #CSE_DATASETS CSED
--				ON F.AccountNumber = CSED.DF_SPE_ACC_ID
--			INNER JOIN CLS.forb.ForbearanceProcessing FP
--				ON F.AccountNumber = FP.AccountNumber
--				AND F.ForbCode = FP.ForbCode
--				AND F.DateRequested = FP.DateRequested
--				AND F.ForbearanceType = FP.ForbearanceType
--				AND F.StartDate = FP.StartDate
--				AND F.EndDate = FP.EndDate
--				AND F.DateCertified = FP.DateCertified
--				AND F.ProcessOn = FP.ProcessOn
--			LEFT JOIN CLS.forb.ForbLoanSequenceSelection EXISTING_DATA
--				ON EXISTING_DATA.ForbearanceProcessingId = FP.ForbearanceProcessingId
--		WHERE
--			F.SelectAllLoans = X --PLUS loans
--			AND CSED.SelectAllLoans = X --PLUS loans
--			AND FP.SelectAllLoans = X --PLUS loans
--			AND EXISTING_DATA.ForbearanceProcessingId IS NULL
--		;
--		--select * from CLS.forb.ForbLoanSequenceSelection --test
--		--select * from #FORBPROCESSING where SelectAllLoans = X --test
--		--select * from #CSE_DATASETS where SelectAllLoans = X --test
--		--select * from cls.forb.ForbearanceProcessing where SelectAllLoans = X --test
		

--		--send borrowers with PLUS and NONPLUS loans to AAP
--		DECLARE @ArcTypeId TINYINT = X, @ARC_AAP VARCHAR(X) = 'CSFRV', @ScriptID_AAP VARCHAR(X) = 'CSFOLFED';
				
--		INSERT INTO CLS..ArcAddProcessing
--		(
--			ArcTypeId
--			,AccountNumber
--			,RecipientId
--			,ARC
--			,ScriptId
--			,ProcessOn
--			,Comment
--			,IsReference
--			,IsEndorser
--			,RegardsCode
--			,LN_ATY_SEQ
--			,ProcessingAttempts
--			,CreatedAt
--			,CreatedBy
--		)
--		SELECT DISTINCT
--			 @ArcTypeId AS ArcTypeId
--			,BP.DF_SPE_ACC_ID AS AccountNumber
--			,'' AS RecipientId
--			,@ARC_AAP AS ARC
--			,@ScriptID_AAP AS ScriptId
--			,GETDATE() AS ProcessOn
--			,'Borrower has PLUS and Non-Plus Loans. Unable to determine loans to process. Please review.' AS Comment
--			,X AS IsReference
--			,X AS IsEndorser
--			,X AS RegardsCode
--			,X AS LN_ATY_SEQ
--			,X AS ProcessingAttempts
--			,GETDATE() AS CreatedAt
--			,@ScriptID_AAP AS CreatedBy
--		FROM
--			#BASE_POP BP
--			LEFT JOIN CLS..ArcAddProcessing EXISTING_DATA
--				ON EXISTING_DATA.ArcTypeId = @ArcTypeId
--				AND EXISTING_DATA.AccountNumber = BP.DF_SPE_ACC_ID
--				AND EXISTING_DATA.ARC = @ARC_AAP
--				AND EXISTING_DATA.ScriptId = @ScriptID_AAP
--				AND EXISTING_DATA.CreatedBy = @ScriptID_AAP
--				AND EXISTING_DATA.CreatedAt >= @TODAY
--				AND EXISTING_DATA.CreatedAt < DATEADD(DAY, X, @TODAY)
--		WHERE
--			EXISTING_DATA.AccountNumber IS NULL
--		GROUP BY
--			BP.DF_SPE_ACC_ID
--		HAVING
--			COUNT (DISTINCT BP.SelectAllLoans) > X
--	;
--	--select * from CLS..ArcAddProcessing where ARC = 'CSFRV' and ScriptId = 'CSFOLFED' --TEST

----	COMMIT TRANSACTION;
----END TRY
----BEGIN CATCH
----	PRINT 'CSFOLFED.sql transaction NOT committed.';
----	ROLLBACK TRANSACTION;
----	THROW;
----END CATCH;