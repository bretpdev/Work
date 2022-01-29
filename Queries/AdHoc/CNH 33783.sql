use cdw
go
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
IF OBJECT_ID('tempdb..#BASE_POP') IS NOT NULL 
	DROP TABLE #BASE_POP

SELECT DISTINCT
	LNXX.BF_SSN AS Ssn,
	PDXX.DF_SPE_ACC_ID AS AccountNumber,
	XX AS DueDate,
	GETDATE() AS AddedAt
INTO #BASE_POP
FROM
	CDW..PDXX_PRS_NME PDXX
	INNER JOIN CDW..LNXX_LON LNXX
		ON PDXX.DF_PRS_ID = LNXX.BF_SSN
	INNER JOIN CDW..RSXX_BR_RPD RSXX
		ON RSXX.BF_SSN = LNXX.BF_SSN
		AND RSXX.LC_STA_RPSTXX = 'A'
	INNER JOIN CDW..LNXX_LON_RPS LNXX
		ON LNXX.BF_SSN = LNXX.BF_SSN
		AND LNXX.LN_SEQ = LNXX.LN_SEQ
		AND LNXX.LN_RPS_SEQ = RSXX.LN_RPS_SEQ
	INNER JOIN CDW..DWXX_DW_CLC_CLU DWXXX
		ON DWXXX.BF_SSN = LNXX.BF_SSN
		AND DWXXX.LN_SEQ = LNXX.LN_SEQ
		AND DWXXX.WC_DW_LON_STA = 'XX'
	LEFT JOIN
	(
		SELECT	
			BF_SSN
		FROM
			CDW..DWXX_DW_CLC_CLU DWXX
		WHERE
			DWXX.WC_DW_LON_STA NOT IN ('XX', 'XX')
	) DWXX
		ON DWXX.BF_SSN = LNXX.BF_SSN
	INNER JOIN
	(
		SELECT DISTINCT
			BLXX.BF_SSN,
			BLXX.LD_BIL_DU
		FROM
			CDW..BLXX_BR_BIL BLXX
		INNER JOIN
		(
			SELECT
				B.BF_SSN,
				MAX(B.LD_BIL_DU) AS LD_BIL_DU 
			FROM
				CDW..BLXX_BR_BIL B
			WHERE
				B.LC_STA_BILXX = 'A'
				AND B.LC_BIL_TYP = 'P'
			GROUP BY 
				BF_SSN
		)MAX_BILL
			ON MAX_BILL.BF_SSN = BLXX.BF_SSN
			AND MAX_BILL.LD_BIL_DU = BLXX.LD_BIL_DU
		WHERE
			DAY(BLXX.LD_BIL_DU) BETWEEN XX AND XX 
			AND BLXX.LD_BIL_DU <= DATEADD(DAY, XX, CAST(GETDATE() AS DATE))
	)BLXX
		ON BLXX.BF_SSN = LNXX.BF_SSN
	LEFT JOIN
	(
		SELECT	
			BF_SSN
		FROM
			CDW..AYXX_BR_LON_ATY AYXX
		WHERE
			AYXX.PF_REQ_ACT = 'OVRPS'
            AND AYXX.LC_STA_ACTYXX = 'A'
            AND CAST(AYXX.LD_ATY_REQ_RCV AS DATE) > CAST(DATEADD(DAY, -XX, GETDATE()) AS DATE)
	)AYXX
		ON AYXX.BF_SSN = PDXX.DF_PRS_ID
	LEFT JOIN
	(
		SELECT
			BF_SSN
		FROM
			CDW..LNXX_LON_DLQ_HST
		WHERE
			LC_STA_LONXX = 'X' 
	)LNXX_CUR
		ON LNXX_CUR.BF_SSN = LNXX.BF_SSN
	LEFT JOIN
	(
		SELECT
			BRXX.BF_SSN
		FROM
			CDW..BRXX_BR_EFT BRXX
		WHERE
			BRXX.BC_EFT_STA = 'A'
	)BRXX
		ON BRXX.BF_SSN = LNXX.BF_SSN
	LEFT JOIN
	(
		SELECT
			BF_SSN
		FROM
			CDW..LNXX_LON_RPS
		WHERE
			LC_TYP_SCH_DIS IN ('CQ','CX','CX','CX','IB','IL','IS','IX','CA','CP','IP','PG','PL','FS','FG', 'IX','IA') 
			AND LC_STA_LONXX = 'A'
	)IDR
		ON IDR.BF_SSN = LNXX.BF_SSN
	 
WHERE
	LNXX.LA_CUR_PRI > X
	AND LNXX.LC_STA_LONXX = 'R'
	AND LNXX.LC_STA_LONXX = 'A'
	AND LNXX.LC_TYP_SCH_DIS NOT IN ('CQ','CX','CX','CX','IB','IL','IS','IX','CA','CP','IP','PG','PL','FS','FG')
	AND (BRXX.BF_SSN IS NULL)
	AND LNXX_CUR.BF_SSN IS NULL
	AND IDR.BF_SSN IS NULL
	AND AYXX.BF_SSN IS NULL
	AND DWXX.BF_SSN IS NULL
	
--COMMENT THIS PEICE OUT WHEN READY TO RUN IN LIVE						
SELECT * FROM #BASE_POP



--COMMENT OUT WHEN READY TO RUN IN LIVE
--INSERT INTO CLS..DueDateChange(Ssn, AccountNumber, DueDate, AddedAt)
--SELECT 
--	BP.*
--FROM 
--	#BASE_POP BP
--	LEFT JOIN /*PREVENTS DUPLICATE RECORDS FROM BEING ADDED*/
--	(
--		SELECT 
--			Ssn
--		FROM 
--			CLS..DueDateChange 
--		WHERE 
--			ProcessedAt IS NULL
--	) UNPROCESSED_POP
--		ON BP.Ssn = UNPROCESSED_POP.Ssn
--WHERE
--	UNPROCESSED_POP.Ssn IS NULL