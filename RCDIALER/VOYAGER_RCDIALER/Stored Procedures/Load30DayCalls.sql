CREATE PROCEDURE [rcdialer].[Load30DayCalls]
	@MONDAY DATETIME
AS

INSERT INTO voyager.rcdialer.OutboundCalls(RCID, BorrowerId, ServicerId, FirstName, LastName, Address1, address2, City, [State], Zip, Email, HomePhone, WorkPhone, CellPhone, MonthlyRepaymentAmount, SchoolCode, SchoolName, DaysDelinquent, DelinquentBucket)
SELECT DISTINCT
	CONCAT(SchoolId.school_id, '_', DQ.borrower_id) AS RCID,
	DQ.borrower_id AS BorrowerId,
	SchoolId.servicer_id AS ServicerId,
	RTRIM(DQ.first_name) AS FirstName,
	RTRIM(DQ.last_name) AS LastName,
	RTRIM(DEMO.address_1) AS Address1,
	RTRIM(DEMO.address_2) AS Address2,
	RTRIM(DEMO.city) AS City,
	RTRIM(DEMO.[state]) AS [State],
	RTRIM(DEMO.zip) AS Zip,
	RTRIM(Email.BorrowerEmail) AS Email,
	CASE
		WHEN DNCH.PhoneNumber IS NOT NULL THEN NULL ELSE DQ.home_phone
	END AS HomePhone,
	CASE
		WHEN DNCW.PhoneNumber IS NOT NULL THEN NULL ELSE DQ.business_phone
	END AS WorkPhone,
	CASE
		WHEN DNCC.PhoneNumber IS NOT NULL THEN NULL ELSE DQ.cell_phone
	END AS CellPhone,
	SUM(DQ.monthly_payment_amount) OVER (PARTITION BY DQ.borrower_id) AS MonthlyRepaymentAmount,
	SchoolId.school_id AS SchoolCode,
	SchoolId.school_name AS SchoolName,
	MAX(DQ.days_delinquent) OVER (PARTITION BY DQ.borrower_id) AS DaysDelinquent,
	30 AS DelinquentBucket
FROM
	voyager..delqs DQ
	INNER JOIN rcdialer.BucketMapping MAP
		ON DQ.days_delinquent BETWEEN MAP.BucketBegin AND MAP.BucketEnd
		AND MAP.Bucket = 30
		AND MAP.DeletedAt IS NULL
	INNER JOIN
	(
		SELECT DISTINCT
			D.borrower_id,
			D.servicer_id,
			MAX(D.school_id) OVER (PARTITION BY D.borrower_id) AS school_id,
			MAX(S.school_name) OVER (PARTITION BY D.borrower_id) AS school_name,
			D.loan_date
		FROM
			voyager..delqs D
			INNER JOIN 
			(
				SELECT DISTINCT
					DL.borrower_id,
					MAX(DL.loan_date) OVER (PARTITION BY DL.borrower_id) AS LoanDate
				FROM
					voyager..delqs DL
					INNER JOIN rcdialer.BucketMapping MAP
						ON DL.days_delinquent BETWEEN MAP.BucketBegin AND MAP.BucketEnd
						AND MAP.Bucket = 30
						AND MAP.DeletedAt IS NULL
				WHERE
					active_cohort = 1
			) MaxDate
				ON MaxDate.borrower_id = D.borrower_id
				AND MaxDate.LoanDate = D.loan_date
			LEFT JOIN voyager..schools S
				ON S.school_code = D.school_id
				AND S.service_30 = 1
				AND S.active = 1
		WHERE
			D.active_cohort = 1
	) SchoolId
		ON SchoolId.borrower_id = DQ.borrower_id
	LEFT JOIN
	(
		SELECT DISTINCT
			D.borrower_id,
			D.school_id,
			D.address_1,
			D.address_2,
			D.city,
			D.[state],
			D.zip
		FROM
			voyager..demos D
			INNER JOIN 
			(
				SELECT	
					borrower_id,
					MAX(add_eff_date) AS EffectiveDate
				FROM
					voyager..demos
				WHERE
					good_flag = 'Y'
					AND current_source_flag = 'Y'
				GROUP BY
					borrower_id
			) MaxDemo
				ON MaxDemo.borrower_id = D.borrower_id
				AND MaxDemo.EffectiveDate = D.add_eff_date
		WHERE
			good_flag = 'Y'
			AND current_source_flag = 'Y'
	) DEMO
		ON DEMO.borrower_id = SchoolId.borrower_id
		AND DEMO.school_id = SchoolId.school_id
	LEFT JOIN
	(
		SELECT DISTINCT
			E.borrower_id,
			E.email_address AS BorrowerEmail
		FROM
			[voyager].[dbo].[emails] E
			INNER JOIN
			(
				SELECT
					borrower_id,
					MAX(email_effective_date) AS email_effective_date
				FROM
					voyager..emails
				WHERE
					email_good_flag = 'Y'
				GROUP BY
					borrower_id
			) MaxEmail
				ON MaxEmail.borrower_id = E.borrower_id
				AND MaxEmail.email_effective_date = E.email_effective_date
		WHERE
			E.email_good_flag = 'Y'
	) Email
		ON Email.borrower_id = SchoolId.borrower_id
	LEFT JOIN 
	(
		SELECT DISTINCT
			borrower_id,
			loan_date
		FROM
			voyager..delqs 
	) DS --Gets the servicer with the most current loan date
		ON DS.borrower_id = DQ.borrower_id
		AND DS.loan_date = SchoolId.loan_date
	LEFT JOIN rcdialer.DoNotContact DNCH
		ON DNCH.BorrowerId = DQ.borrower_id
		AND DNCH.PhoneNumber = DQ.home_phone
		AND DNCH.DeletedAt IS NULL
	LEFT JOIN rcdialer.DoNotContact DNCW
		ON DNCW.BorrowerId = DQ.borrower_id
		AND DNCW.PhoneNumber = DQ.business_phone
		AND DNCW.DeletedAt IS NULL
	LEFT JOIN rcdialer.DoNotContact DNCC
		ON DNCC.BorrowerId = DQ.borrower_id
		AND DNCC.PhoneNumber = DQ.cell_phone
		AND DNCC.DeletedAt IS NULL
	LEFT JOIN voyager.rcdialer.OutboundCalls OC
		ON OC.BorrowerId = DQ.borrower_id
		AND OC.FirstName = DQ.first_name
		AND OC.LastName = DQ.last_name
		AND OC.ServicerId = SchoolId.servicer_id
		AND OC.SchoolCode = SchoolId.school_id
		AND OC.DeletedAt IS NULL
		AND CAST(OC.AddedAt AS DATE) = CAST(GETDATE() AS DATE)
WHERE
	DQ.active_cohort = 1
	AND OC.BorrowerId IS NULL
	AND DQ.created_on BETWEEN DATEADD(D, -7, @MONDAY) AND @MONDAY --Monday is the monday of the current week at 9:00 AM
	AND (DNCH.PhoneNumber IS NULL OR DNCW.PhoneNumber IS NULL OR DNCC.PhoneNumber IS NULL)