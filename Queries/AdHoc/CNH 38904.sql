USE CDW
GO

SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED

SELECT DISTINCT
	NonCap.BF_SSN,
	NonCap.LN_SEQ,
	FSXX.LF_FED_AWD + RIGHT('XXX' + CAST(FSXX.LN_FED_AWD_SEQ AS VARCHAR),X) AS AwardId,
	CAST(Cap.LD_FOR_BEG AS DATE) AS CapForbBegin,
	CAST(Cap.LD_FOR_END AS DATE) AS CapForbEnd,
	CAST(Cap.LD_FOR_APL AS DATE) AS CapForbApplied,
	CAST(NonCap.LD_FOR_BEG AS DATE) AS NonCapForbBegin,
	CAST(NonCap.LD_FOR_END AS DATE) AS NonCapForbEnd,
	CAST(NonCap.LD_FOR_APL AS DATE) AS NonCapForbApplied,
	DWXX.WC_DW_LON_STA AS LoanStatus,
	CASE WHEN RS.BF_SSN IS NOT NULL THEN RS.LC_TYP_SCH_DIS ELSE '' END AS RepaymentStatus
FROM
	CDW..FSXX_DL_LON FSXX
	INNER JOIN
	(
		SELECT
			LNXX.BF_SSN,
			LNXX.LN_SEQ,
			LNXX.LD_FOR_BEG,
			LNXX.LD_FOR_END,
			LNXX.LD_FOR_APL
		FROM
			CDW..LNXX_BR_FOR_APV LNXX				
			INNER JOIN CDW..FBXX_BR_FOR_REQ FBXX
				ON FBXX.BF_SSN = LNXX.BF_SSN
				AND FBXX.LF_FOR_CTL_NUM = LNXX.LF_FOR_CTL_NUM
				AND FBXX.LC_FOR_TYP = 'XX'
				AND FBXX.LC_STA_FORXX = 'A'
				AND FBXX.LC_FOR_STA = 'A'
				AND FBXX.LI_CAP_FOR_INT_REQ = 'C'
		WHERE
			LNXX.LC_STA_LONXX = 'A'
			AND LNXX.LC_FOR_RSP != 'XXX'
			AND CAST(LNXX.LD_FOR_BEG AS DATE) >= 'XXXX-XX-XX'
	) Cap
		ON Cap.BF_SSN = FSXX.BF_SSN
		AND Cap.LN_SEQ = FSXX.LN_SEQ
	INNER JOIN
	(
		SELECT
			LNXXNon.BF_SSN,
			LNXXNon.LN_SEQ,
			LNXXNon.LD_FOR_BEG,
			LNXXNon.LD_FOR_END,
			LNXXNon.LD_FOR_APL
		FROM
			CDW..LNXX_BR_FOR_APV LNXXNon				
			INNER JOIN CDW..FBXX_BR_FOR_REQ FBXX
				ON FBXX.BF_SSN = LNXXNon.BF_SSN
				AND FBXX.LF_FOR_CTL_NUM = LNXXNon.LF_FOR_CTL_NUM
				AND FBXX.LC_FOR_TYP = 'XX'
				AND FBXX.LC_STA_FORXX = 'A'
				AND FBXX.LC_FOR_STA = 'A'
				AND FBXX.LI_CAP_FOR_INT_REQ != 'C'
		WHERE
			LNXXNon.LC_STA_LONXX = 'A'
			AND LNXXNon.LC_FOR_RSP != 'XXX'
			AND CAST(LNXXNon.LD_FOR_BEG AS DATE) >= 'XXXX-XX-XX'
			AND DATEDIFF(MONTH,LNXXNon.LD_FOR_BEG,LNXXNon.LD_FOR_END) >= X
	) NonCap
		ON NonCap.BF_SSN = FSXX.BF_SSN
		AND NonCap.LN_SEQ = FSXX.LN_SEQ
		AND CAST(NonCap.LD_FOR_BEG AS DATE) >= CAST(Cap.LD_FOR_END AS DATE)
	INNER JOIN CDW..DWXX_DW_CLC_CLU DWXX
		ON DWXX.BF_SSN = NonCap.BF_SSN
		AND DWXX.LN_SEQ = NonCap.LN_SEQ
	LEFT JOIN CDW.calc.RepaymentSchedules RS
		ON RS.BF_SSN = NonCap.BF_SSN
		AND RS.LN_SEQ = NonCap.LN_SEQ
		AND RS.CurrentGradation = X