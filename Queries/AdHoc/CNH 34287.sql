SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
SELECT
	CASE WHEN MonthEndDelq.MAX_DAYS_DELINQUENT BETWEEN X AND X THEN 'Current Borrowers (X-X days)'
	     WHEN MonthEndDelq.MAX_DAYS_DELINQUENT BETWEEN X AND XX THEN 'Delinquent Borrowers (X-XX days)'
	     WHEN MonthEndDelq.MAX_DAYS_DELINQUENT BETWEEN XX AND XX THEN 'Delinquent Borrowers (XX-XX days)'
	     WHEN MonthEndDelq.MAX_DAYS_DELINQUENT BETWEEN XX AND XXX THEN 'Delinquent Borrowers (XX-XXX days)'
	     WHEN MonthEndDelq.MAX_DAYS_DELINQUENT BETWEEN XXX AND XXX THEN 'Delinquent Borrowers (XXX-XXX days)'
		 WHEN MonthEndDelq.MAX_DAYS_DELINQUENT BETWEEN XXX AND XXX THEN 'Delinquent Borrowers (XXX-XXX days)'
		 WHEN MonthEndDelq.MAX_DAYS_DELINQUENT > XXX THEN 'Delinquent Borrowers (XXX+ days)'
		 ELSE '' END AS DelqBucket,
		 MonthEndDelq.[Month],
		 MonthEndDelq.[Year],
		 SUM(COALESCE(Calls.CallCount,X)) + SUM(COALESCE(CallsX.CallCount,X)) AS Calls,
		 SUM(PhysicalMail.MailCount) AS Mail,
		 SUM(Emails.EmailCount) AS Email
FROM
	AuditCDW..UNWCXX_JanXXXX_MarXXXX MonthEndDelq
	INNER JOIN CDW..PDXX_PRS_NME PDXX
		ON PDXX.DF_PRS_ID = MonthEndDelq.BORROWER_SSN
	LEFT JOIN
	(
		SELECT DISTINCT
			COUNT(NCH.NobleCallHistoryId) AS CallCount,
			MONTH(NCH.ActivityDate) AS [Month],
			YEAR(NCH.ActivityDate) AS [Year],
			NCH.AccountIdentifier
		FROM
			NobleCalls..NobleCallHistory NCH
		WHERE
			NCH.RegionId = X --Cornerstone
			AND NCH.IsInbound = X --Outbound calls
			AND NCH.DeletedAt IS NULL
			AND CAST(NCH.ActivityDate AS DATE) BETWEEN 'XXXX-XX-XX' AND 'XXXX-XX-XX'
		GROUP BY
			NCH.AccountIdentifier,
			MONTH(NCH.ActivityDate),
			YEAR(NCH.ActivityDate)
	) Calls -- some SSN some account number
		ON Calls.[Month] = MonthEndDelq.[Month]
		AND Calls.[Year] = MonthEndDelq.[Year]
		AND	Calls.AccountIdentifier = PDXX.DF_SPE_ACC_ID
	LEFT JOIN
	(
		SELECT DISTINCT
			COUNT(NCH.NobleCallHistoryId) AS CallCount,
			MONTH(NCH.ActivityDate) AS [Month],
			YEAR(NCH.ActivityDate) AS [Year],
			NCH.AccountIdentifier
		FROM
			NobleCalls..NobleCallHistory NCH
		WHERE
			NCH.RegionId = X --Cornerstone
			AND NCH.IsInbound = X --Outbound calls
			AND NCH.DeletedAt IS NULL
			AND CAST(NCH.ActivityDate AS DATE) BETWEEN 'XXXX-XX-XX' AND 'XXXX-XX-XX'
		GROUP BY
			NCH.AccountIdentifier,
			MONTH(NCH.ActivityDate),
			YEAR(NCH.ActivityDate)
	) CallsX -- some SSN some account number
		ON CallsX.[Month] = MonthEndDelq.[Month]
		AND CallsX.[Year] = MonthEndDelq.[Year]
		AND	CallsX.AccountIdentifier = PDXX.DF_PRS_ID
	LEFT JOIN
	(
		SELECT DISTINCT
			COUNT(DD.DocumentDetailsId) AS MailCount,
			MONTH(DD.Printed) AS [Month],
			YEAR(DD.Printed) AS [Year],
			DD.Ssn
		FROM 
			ECorrFed..DocumentDetails DD
		WHERE 
			DD.CorrMethod = 'Printed' 
			AND DD.Active = X 
			AND CAST(DD.Printed AS DATE) BETWEEN 'XXXX-XX-XX' AND 'XXXX-XX-XX' 
		GROUP BY
			DD.Ssn,
			MONTH(DD.Printed),
			YEAR(DD.Printed)

		UNION ALL

		SELECT DISTINCT
			COUNT(*) AS MailCount,
			MONTH(AYXX.LD_ATY_REQ_RCV) AS [Month],
			YEAR(AYXX.LD_ATY_REQ_RCV) AS [Year],
			AYXX.BF_SSN AS Ssn
			
		FROM
			CDW..AYXX_BR_LON_ATY AYXX
		WHERE
			CAST(AYXX.LD_ATY_REQ_RCV AS DATE) BETWEEN 'XXXX-XX-XX' AND 'XXXX-XX-XX'
			AND AYXX.PF_REQ_ACT IN ('RXRFX','RADEN','ADUPR')
		GROUP BY
			AYXX.BF_SSN,
			MONTH(AYXX.LD_ATY_REQ_RCV),
			YEAR(AYXX.LD_ATY_REQ_RCV)
	) PhysicalMail
		ON PhysicalMail.Ssn = PDXX.DF_PRS_ID
		AND PhysicalMail.[Month] = MonthEndDelq.[Month]
		AND PhysicalMail.[Year] = MonthEndDelq.[Year]
	LEFT JOIN
	(
		SELECT DISTINCT
			PDXX.DF_SPE_ACC_ID AS AccountNumber,
			MONTH(AYXX.LD_ATY_REQ_RCV) AS [Month],
			YEAR(AYXX.LD_ATY_REQ_RCV) AS [Year],
			COUNT(*) AS EmailCount
		FROM
			CDW..AYXX_BR_LON_ATY AYXX
			INNER JOIN CLS.emailbtcf.EmailCampaigns EC
				ON AYXX.PF_REQ_ACT = EC.Arc
			INNER JOIN CDW..PDXX_PRS_NME PDXX
				ON PDXX.DF_PRS_ID = AYXX.BF_SSN
		WHERE
			CAST(AYXX.LD_ATY_REQ_RCV AS DATE) BETWEEN 'XXXX-XX-XX' AND 'XXXX-XX-XX'
		GROUP BY
			PDXX.DF_SPE_ACC_ID,
			MONTH(AYXX.LD_ATY_REQ_RCV),
			YEAR(AYXX.LD_ATY_REQ_RCV)

		UNION ALL 
		
		SELECT DISTINCT
			PDXX.DF_SPE_ACC_ID AS AccountNumber,
			MONTH(DD.EmailSent) AS [Month],
			YEAR(DD.EmailSent) AS [Year],
			COUNT(*) AS EmailCount
		FROM
			ECorrFed..DocumentDetails DD
			INNER JOIN ECorrFed..Letters L
				ON L.LetterId = DD.LetterId
			INNER JOIN CDW..PDXX_PRS_NME PDXX
				ON PDXX.DF_PRS_ID = DD.Ssn
		WHERE
			DD.CorrMethod = 'Email Notify'
			AND DD.Active = X
			AND CAST(DD.EmailSent AS DATE) BETWEEN 'XXXX-XX-XX' AND 'XXXX-XX-XX'
		GROUP BY
			PDXX.DF_SPE_ACC_ID,
			DD.AddresseeEmail,
			L.SubjectLine,
			MONTH(DD.EmailSent),
			YEAR(DD.EmailSent)
	) Emails
		ON Emails.AccountNumber = PDXX.DF_SPE_ACC_ID
		AND Emails.[Month] = MonthEndDelq.[Month]
		AND Emails.[Year] = MonthEndDelq.[Year]
GROUP BY
	MonthEndDelq.[Month],
	MonthEndDelq.[Year],
	CASE WHEN MonthEndDelq.MAX_DAYS_DELINQUENT BETWEEN X AND X THEN 'Current Borrowers (X-X days)'
	     WHEN MonthEndDelq.MAX_DAYS_DELINQUENT BETWEEN X AND XX THEN 'Delinquent Borrowers (X-XX days)'
	     WHEN MonthEndDelq.MAX_DAYS_DELINQUENT BETWEEN XX AND XX THEN 'Delinquent Borrowers (XX-XX days)'
	     WHEN MonthEndDelq.MAX_DAYS_DELINQUENT BETWEEN XX AND XXX THEN 'Delinquent Borrowers (XX-XXX days)'
	     WHEN MonthEndDelq.MAX_DAYS_DELINQUENT BETWEEN XXX AND XXX THEN 'Delinquent Borrowers (XXX-XXX days)'
		 WHEN MonthEndDelq.MAX_DAYS_DELINQUENT BETWEEN XXX AND XXX THEN 'Delinquent Borrowers (XXX-XXX days)'
		 WHEN MonthEndDelq.MAX_DAYS_DELINQUENT > XXX THEN 'Delinquent Borrowers (XXX+ days)'
		 ELSE '' END
ORDER BY
	DelqBucket,
	MonthEndDelq.[Year] ASC,
	MonthEndDelq.[Month] ASC
