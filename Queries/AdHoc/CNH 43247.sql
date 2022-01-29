USE CDW
GO
--IF OBJECT_ID (N'#CREDIT', N'U') IS NULL 
--BEGIN
--SELECT * INTO #CREDIT FROM OPENQUERY(LEGEND,
--'
--SELECT DISTINCT
--	LNXX.BF_SSN,
--	MAX(LD_RPT_CRB) AS LD_RPT_CRB
--FROM
--	PKUB.LNXX_LON_CRB_RPT LNXX
--	INNER JOIN PKUB.LNXX_LON LNXX
--		ON LNXX.BF_SSN = LNXX.BF_SSN
--		AND LNXX.LN_SEQ = LNXX.LN_SEQ
--		AND LNXX.LA_CUR_PRI > X
--		AND LNXX.LC_STA_LONXX = ''R''
--WHERE
--	LC_RPT_STA_CRB IN (''XX'',''XX'',''XX'',''XX'',''XX'')
--GROUP BY
--	LNXX.BF_SSN
--'
--)
--END



declare @optout table (bf_ssn char(X), LABEL VARCHAR(XX))
insert into @optout select df_prs_id, 'OPT OUT' from cdw..PDXX_PRS_NME where DF_SPE_ACC_ID in ('XXXXXXXXXX',
'XXXXXXXXXX',
'XXXXXXXXXX',
'XXXXXXXXXX',
'XXXXXXXXXX')
----!!!!!!!!!! MAKE SURE TO CHANGE ALL OF THE TABLE REFERENCES TO THE nh TICKET IMPORTED TABLE!!!!!!
DECLARE @Date DATE = 'XXXX-XX-XX'

SELECT DISTINCT
	CNH.[#] AS [X],
	CNH.[Servicer completing this data file] AS [X],
	CNH.[X Digit SSN] AS [X],
	CASE WHEN LNXX.BF_SSN IS NOT NULL THEN 'Y' ELSE 'N' END AS [X],
	CASE WHEN SUM(LNXX.LA_CUR_PRI) OVER (PARTITION BY LNXX.BF_SSN) > X.XX AND LNXX.BF_SSN IS NOT NULL  THEN 'Y' ELSE 'N' END AS [X],
	CASE
		WHEN FBXX.BF_SSN IS NOT NULL AND SUM(LNXX.LA_CUR_PRI) OVER (PARTITION BY LNXX.BF_SSN) > X.XX  THEN 'Y' ELSE 'N' 
	END AS [X],
	CASE
		WHEN CNH.[If borrower was not in forbearance / stopped collections, have y] IS NOT NULL THEN CNH.[If borrower was not in forbearance / stopped collections, have y] 
		else ' ' end
	[X],
	CNH.[Borrower has been notified of approval or denial__X = Yes (do no] [X],
	CASE 
		WHEN [If the borrower is now in forbearance - provide the current end ] IS NOT NULL THEN [If the borrower is now in forbearance - provide the current end ]
		WHEN SUM(LNXX.LA_CUR_PRI) OVER (PARTITION BY LNXX.BF_SSN) > X.XX THEN ISNULL(CONVERT(VARCHAR(XX),FBXX.LD_FOR_END,XXX),'') ELSE '' 
	END AS [X],
	CASE 
		WHEN CNH.[If borrower was not in forbearance / stopped collections, have y] IS NOT NULL THEN CNH.[If borrower was not in forbearance / stopped collections, have y] 
		WHEN SUM(LNXX.LA_CUR_PRI) OVER (PARTITION BY LNXX.BF_SSN) > X.XX and FBXX.BF_SSN IS NULL  then COALESCE(OP.LABEL, AYXX.PF_REQ_ACT, LOAN_STATUS.PX_DSC_MDM, IDR.LABEL, LNXX.TRANSFER_TYPE, DISCHARGES.DISCHARGE_TYPE, '' ) 
		else '' 
		end AS [XX],
	CASE WHEN BillNotices.Ssn IS NOT NULL and BillNotices.CreateDate > @Date THEN 'Y' ELSE 'N' END AS [XX],
	ISNULL(CONVERT(VARCHAR(XX),BillNotices.CreateDate, XXX),'')AS [XX],
	CASE WHEN Payments.BF_SSN IS NOT NULL and Payments.LD_FAT_EFF > @date THEN 'Y' ELSE 'N' END AS [XX],
	ISNULL(CONVERT(VARCHAR(XX),Payments.LD_FAT_EFF,XXX),'')  AS [XX],
	--'' [XX],
	--'' [XX],
	CASE WHEN c.BF_SSN IS NOT NULL and c.LD_RPT_CRB > @date THEN 'Y' ELSE 'N' END AS [XX],
	ISNULL(CONVERT(VARCHAR(XX),c.LD_RPT_CRB,XXX),'') AS [XX],
	'' AS [XX],
	'' AS [XX],
	CASE WHEN HAYXX.BF_SSN IS NOT NULL THEN 'Y' ELSE 'N' END AS HAS_BDINT
	--cnh.[Borrower has recently been deemed ineligible/denied_ _X = Yes (D] as [XX]
FROM
	CDW..[CNHXXXXX] CNH
	INNER JOIN CDW..LNXX_LON LNXX
		ON LNXX.BF_SSN = CNH.[X digit ssn]
		AND
		(
			( --Released and either paid off AFTER X/XX/XXXX or not paid
				LNXX.LC_STA_LONXX = 'R'
				AND ISNULL(LNXX.LD_PIF_RPT,'XXXX-XX-XX') >= @Date
			)
			OR --Deconverted AFTER X/XX/XXXX
			(
				LNXX.LC_STA_LONXX = 'D'
				AND LNXX.LA_CUR_PRI = X.XX
				AND LNXX.LD_STA_LONXX >= @Date
			)
		)
	LEFT JOIN
	(
		SELECT DISTINCT
			DD.Ssn,
			MAX(DD.CreateDate) AS CreateDate
		FROM
			ECorrFed..DocumentDetails DD
			INNER JOIN CDW..[CNHXXXXX] CNH
				ON CNH.[X digit SSN] = DD.Ssn
			INNER JOIN ECorrFed..Letters L
				ON L.LetterId = DD.LetterId
				AND L.Letter IN('EBILLFED','INTBILFED','BILSTFED')
		WHERE
			CAST(DD.TotalDue AS DECIMAL(XX,X)) > X.XX
		GROUP BY
			DD.Ssn
	) BillNotices
		ON BillNotices.Ssn = CNH.[X digit ssn]
	LEFT JOIN
	(
		SELECT DISTINCT
			LNXX.BF_SSN,
			MAX(LD_FAT_EFF) AS LD_FAT_EFF
		FROM
			CDW..LNXX_FIN_ATY LNXX
			INNER JOIN CDW..[CNHXXXXX] CNH
				ON CNH.[X digit SSN] = LNXX.BF_SSN
		WHERE
			LNXX.PC_FAT_TYP = 'XX'
			AND LNXX.PC_FAT_SUB_TYP = 'XX'
		GROUP BY
			LNXX.BF_SSN
	) Payments
		ON Payments.BF_SSN = CNH.[X digit ssn]
	LEFT JOIN  #Credit c
		ON c.BF_SSN = CNH.[X digit ssn]
	LEFT JOIN 
	(
		SELECT DISTINCT
			FBXX.BF_SSN,
			MIN(LNXX.LD_FOR_END) AS LD_FOR_END
		FROM
			CDW..FBXX_BR_FOR_REQ FBXX
			INNER JOIN CDW..[CNHXXXXX] CNH
				ON CNH.[X digit SSN] = FBXX.BF_SSN
			INNER JOIN CDW..LNXX_BR_FOR_APV LNXX
				ON FBXX.BF_SSN = LNXX.BF_SSN
				AND FBXX.LF_FOR_CTL_NUM = LNXX.LF_FOR_CTL_NUM
				AND FBXX.LC_FOR_STA = 'A'
				AND FBXX.LC_STA_FORXX = 'A'
				AND LNXX.LC_STA_LONXX = 'A'
		WHERE
			LNXX.LD_FOR_END > CAST(GETDATE() AS DATE)
			AND  lnXX.LD_FOR_BEG <= CAST(GETDATE() AS DATE)
		GROUP BY
			FBXX.BF_SSN
	) FBXX
		ON FBXX.BF_SSN = CNH.[X DIGIT SSN]
	LEFT JOIN 
	(
		SELECT DISTINCT
			PDXX.DF_PRS_ID,
			MAX(LKXX.PX_DSC_MDM) AS PX_DSC_MDM
		FROM
			CDW..PDXX_PRS_NME PDXX
			INNER JOIN CDW..LNXX_LON LNXX
				ON LNXX.BF_SSN = PDXX.DF_PRS_ID
			INNER JOIN CDW..DWXX_DW_CLC_CLU DWXX
				ON DWXX.BF_SSN = LNXX.BF_SSN
				AND DWXX.LN_SEQ = LNXX.LN_SEQ
			INNER JOIN CDW..LKXX_LS_CDE_LKP LKXX
				ON LKXX.PM_ATR = 'WC-DW-LON-STA'
				AND LKXX.PX_ATR_VAL = DWXX.WC_DW_LON_STA
		WHERE
			DWXX.WC_DW_LON_STA NOT IN ('XX','XX', 'XX')
		group by
			PDXX.DF_PRS_ID
	) LOAN_STATUS
		ON LOAN_STATUS.DF_PRS_ID = CNH.[X digit SSN]
	LEFT JOIN
	(
		SELECT
			BF_SSN,
			'$X IDR' AS LABEL
		FROM
			CDW.calc.RepaymentSchedules RS
			INNER JOIN CDW..[CNHXXXXX] CNH
				ON CNH.[X digit SSN] = RS.BF_SSN
		WHERE
			CurrentGradation = X
		GROUP BY
			BF_SSN
		HAVING SUM(LA_RPS_ISL) = X

	) IDR
		ON IDR.BF_SSN = CNH.[X digit SSN]
	LEFT JOIN
	(
		SELECT
			BF_SSN,
			MAX(CASE 
				WHEN LNXX.PC_FAT_SUB_TYP = 'XX' THEN 'OTHER PSLF'
				WHEN LNXX.PC_FAT_SUB_TYP = 'XX' THEN 'OTHER TPD'
				WHEN LNXX.PC_FAT_SUB_TYP = 'XX' THEN 'OTHER DMCS'
				ELSE NULL
			END) AS TRANSFER_TYPE
		FROM
			CDW..LNXX_FIN_ATY LNXX
		WHERE	
			LNXX.PC_FAT_TYP = 'XX'
			AND LNXX.PC_FAT_SUB_TYP IN ('XX','XX','XX')
			AND ISNULL(LNXX.LC_FAT_REV_REA,'') = ''
			AND LNXX.LC_STA_LONXX = 'A'
		GROUP BY
			BF_SSN
	) LNXX
		ON LNXX.BF_SSN = CNH.[X digit SSN]
	LEFT JOIN
    (
		SELECT DISTINCT
			LNXX.BF_SSN,
			CASE 
				WHEN AYXX.PF_REQ_ACT = 'ADDTH' THEN 'OTHER Death'
				WHEN AYXX.PF_REQ_ACT = 'ADCSH' THEN 'OTHER Closed School'
				WHEN AYXX.PF_REQ_ACT = 'ADTLF' THEN 'OTHER Teacher Loan Forgiveness'
				WHEN AYXX.PF_REQ_ACT = 'ADFCR' THEN 'OTHER False Certification'
				WHEN AYXX.PF_REQ_ACT = 'ADUPR' THEN 'OTHER Unpaid Refund'
				WHEN AYXX.PF_REQ_ACT = 'IDAPB' THEN 'OTHER ID Theft'
				WHEN AYXX.PF_REQ_ACT = 'TPDAP' THEN 'OTHER Total Permanent Disability'
				ELSE NULL
			END AS DISCHARGE_TYPE
		FROM
			CDW..LNXX_LON LNXX
			INNER JOIN CDW..LNXX_FIN_ATY LNXX
				ON LNXX.BF_SSN = LNXX.BF_SSN
				AND LNXX.LN_SEQ = LNXX.LN_SEQ
				AND LNXX.PC_FAT_TYP = 'XX' --discharge
				AND LNXX.PC_FAT_SUB_TYP  = 'XX' --discharge
				AND ISNULL(LNXX.LC_FAT_REV_REA,'') = ''
				AND LNXX.LC_STA_LONXX = 'A'
			INNER JOIN CDW..LNXX_LON_ATY LNXX
				ON LNXX.BF_SSN = LNXX.BF_SSN
				AND LNXX.LN_SEQ = LNXX.LN_SEQ
			INNER JOIN CDW..AYXX_BR_LON_ATY AYXX
				ON AYXX.BF_SSN = LNXX.BF_SSN
				AND AYXX.LN_ATY_SEQ = LNXX.LN_ATY_SEQ
		WHERE
			LNXX.LA_CUR_PRI = X.XX -- zero balance
			AND AYXX.PF_REQ_ACT IN ('ADDTH','ADCSH','ADTLF','ADFCR','ADUPR','IDAPB','TPDAP')
    ) DISCHARGES
		on DISCHARGES.BF_SSN = CNH.[X digit SSN]
	LEFT JOIN 
	(
		SELECT 
			BF_SSN,
			'OPT OUT' AS PF_REQ_ACT
		FROM 
			CDW..AYXX_BR_LON_ATY AYXX
		WHERE
			AYXX.PF_REQ_ACT in ('BDDNY', 'BDOPT')
			AND AYXX.LC_STA_ACTYXX = 'A'
	)AYXX
		ON AYXX.BF_SSN = CNH.[X digit SSN]
		
	LEFT JOIN @optout OP
		 ON OP.bf_ssn = CNH.[X digit SSN]
	LEFT JOIN CDW..AYXX_BR_LON_ATY HAYXX
		ON HAYXX.BF_SSN = CNH.[X digit ssn]
		AND HAYXX.PF_REQ_ACT = 'BDINT'
ORDER BY CNH.#






