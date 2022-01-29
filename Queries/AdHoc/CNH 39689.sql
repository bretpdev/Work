USE CDW
GO

DECLARE @Date DATE = 'XXXX-XX-XX'

SELECT DISTINCT
	CNH.[#],
	CNH.SERVICER,
	CNH.[X Digit SSN],
	CASE WHEN LNXX.BF_SSN IS NOT NULL THEN 'Y' ELSE 'N' END AS OnSystem,
	CASE WHEN BillNotices.Ssn IS NOT NULL THEN 'Y' ELSE 'N' END AS BillSent,
	CASE WHEN Payments.BF_SSN IS NOT NULL THEN 'Y' ELSE 'N' END AS MadePayment,
	'' AS PaymentRefunded,
	CASE WHEN Credit.BF_SSN IS NOT NULL THEN 'Y' ELSE 'N' END AS CreditReported,
	'' AS CorrectedCredit,
	'' AS DMCSPayments,
	'' AS DMCSRefunds
FROM
	CDW..[CNH XXXXX] CNH
	LEFT JOIN CDW..LNXX_LON LNXX
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
			DD.Ssn
		FROM
			ECorrFed..DocumentDetails DD
			INNER JOIN ECorrFed..Letters L
				ON L.LetterId = DD.LetterId
				AND L.Letter IN('EBILLFED','INTBILFED','BILSTFED')
		WHERE
			CAST(DD.TotalDue AS DECIMAL(XX,X)) > X.XX
			AND DD.CreateDate >= @Date
	) BillNotices
		ON BillNotices.Ssn = LNXX.BF_SSN
	LEFT JOIN
	(
		SELECT DISTINCT
			LNXX.BF_SSN
		FROM
			CDW..LNXX_FIN_ATY LNXX
		WHERE
			LNXX.PC_FAT_TYP = 'XX'
			AND LNXX.PC_FAT_SUB_TYP = 'XX'
			AND CAST(LNXX.LD_FAT_EFF AS DATE) >= @Date
	) Payments
		ON Payments.BF_SSN = LNXX.BF_SSN
	LEFT JOIN OPENQUERY
	(LEGEND,
	'
		SELECT DISTINCT
			BF_SSN
		FROM
			PKUB.LNXX_LON_CRB_RPT
		WHERE
			LC_RPT_STA_CRB IN (''XX'',''XX'',''XX'',''XX'',''XX'')
			AND LD_RPT_CRB >= ''XXXX-XX-XX''
			AND LD_RPT_CRB <= ''XXXX-XX-XX''
	'
	) Credit
		ON Credit.BF_SSN = LNXX.BF_SSN
ORDER BY
	OnSystem DESC,
	MadePayment DESC,
	BillSent DESC

