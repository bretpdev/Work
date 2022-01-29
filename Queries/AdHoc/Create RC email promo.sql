USE [voyager]
GO

/****** Object:  StoredProcedure [rcemailpop].[CovidForb2]    Script Date: 11/30/2021 8:37:21 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [rcemailpop].[CovidForb2]
	
AS
	DECLARE @SendGrid varchar(100) = 'd-9d435b4e5ba444a4a39575f67855a859'

DROP TABLE IF EXISTS #TwoServicers
SELECT DISTINCT
	P.borrower_id,
	current_ga_ed_servicer_code AS ServicerCode,
	s.servicer_name
INTO #TwoServicers
FROM
(
SELECT DISTINCT
	borrower_id,
	COUNT(DISTINCT current_ga_ed_servicer_code) AS SER_COUNT
FROM
	voyager..[ports] P
WHERE
	current_opb_amount > 0.00
	AND current_loan_status = 'FB'
	AND SUBSTRING(loan_type, 1,1) = 'D'
GROUP BY
	borrower_id
HAVING 
	COUNT(DISTINCT current_ga_ed_servicer_code) > 1
) P
INNER JOIN voyager..[ports] POR
	ON POR.borrower_id = P.borrower_id
INNER JOIN voyager..servicers S
		ON S.id = POR.current_ga_ed_servicer_code
WHERE
	current_opb_amount > 0.00
	AND current_loan_status = 'FB'
	AND SUBSTRING(loan_type, 1,1) = 'D'

INSERT INTO voyager.rcemailpop.EmailPopulation(BorrowerId, SchoolId, ServicerCode, Email, FirstName, CoBrandedLogo, SchoolLogo, SchoolName, SchoolEmail, SchoolPhone,
SchoolWebsite, ServicerName, ServicerWebsite, ServicerLogo, ServicerContactPage, ServicerDelayPage, ServicerRepaymentPage,
ServicerPhone, Bucket, SendGridTemplate, DaysDelinquent, FromEmailAddress)
SELECT DISTINCT 
	P.borrower_id,
	P.school_id,
	S.id,
	EMAIL.BorrowerEmail,
	UPPER(LEFT(lower(first_name),1))+LOWER(SUBSTRING(lower(first_name),2,LEN(lower(first_name)))) AS FirstName,
	CAST(SCH.rc_school_logo  AS VARCHAR(500)) AS CoBrandedLogo,
	CAST(SCH.school_logo AS VARCHAR(500)) AS SchoolLogo,
	sch.school_name,
	SCH.school_email,
	SCH.school_phone,
	CAST(SCH.website AS VARCHAR(500)) as SchoolWebsite,
	S.servicer_name,
	S.website,
	S.servicer_logo,
	S.contact,
	S.[delay],
	S.repay,
	S.phone,
	0 as bucket,
	@SendGrid AS SendGridTemplate,
	0 as DaysDelinquent,
	'info@repaycentsibly.org' AS FromEmailAddress
	
FROM
	Voyager..[Ports] P
	INNER JOIN voyager..servicers S
		ON P.current_ga_ed_servicer_code = S.id
	INNER JOIN voyager..schools SCH
		ON SCH.id = P.school_id
	INNER JOIN
	(
		SELECT DISTINCT
			E.borrower_id,
			E.email_address AS BorrowerEmail
		FROM 
			[voyager].[dbo].[emails] e
			INNER JOIN 
			(
				SELECT
					borrower_id, 
					MAX(email_effective_date) as email_effective_date
				FROM
					voyager..emails E
					LEFT JOIN [voyager].[rcemailpul].[UnsubscribedEmails] UNSUB
						ON UNSUB.EmailAddress = E.email_address
				WHERE
					email_good_flag = 'Y'
					AND UNSUB.EmailAddress IS NULL
				GROUP BY
					borrower_id
			)me
				on me.borrower_id = e.borrower_id
				and me.email_effective_date = e.email_effective_date
		WHERE
			E.email_good_flag = 'Y'
	) EMAIL
		 ON EMAIL.borrower_id = P.borrower_id
	LEFT JOIN voyager.rcemailpop.EmailPopulation EP
		ON EP.BorrowerId = P.borrower_id
		AND EP.SendGridTemplate = @SendGrid
		AND EP.DeletedAt IS NULL
		AND EP.AddedAt > DATEADD(DAY, -7, GETDATE())
	LEFT JOIN #TwoServicers TS
		ON TS.borrower_id = P.borrower_id
WHERE
	current_opb_amount > 0.00
	AND current_loan_status = 'FB'
	AND SUBSTRING(loan_type, 1,1) = 'D'
	AND EP.BorrowerId IS NULL
	AND TS.borrower_id IS NULL

RETURN 0

GO

CREATE PROCEDURE [rcemailpop].[CovidForb3]
	
AS
	DECLARE @SendGrid varchar(100) = 'd-4fdccac7e88444c5aa6e7c431e6e6963'

DROP TABLE IF EXISTS #TwoServicers
SELECT DISTINCT
	P.borrower_id,
	current_ga_ed_servicer_code AS ServicerCode,
	s.servicer_name
INTO #TwoServicers
FROM
(
SELECT DISTINCT
	borrower_id,
	COUNT(DISTINCT current_ga_ed_servicer_code) AS SER_COUNT
FROM
	voyager..[ports] P
WHERE
	current_opb_amount > 0.00
	AND current_loan_status = 'FB'
	AND SUBSTRING(loan_type, 1,1) = 'D'
GROUP BY
	borrower_id
HAVING 
	COUNT(DISTINCT current_ga_ed_servicer_code) > 1
) P
INNER JOIN voyager..[ports] POR
	ON POR.borrower_id = P.borrower_id
INNER JOIN voyager..servicers S
		ON S.id = POR.current_ga_ed_servicer_code
WHERE
	current_opb_amount > 0.00
	AND current_loan_status = 'FB'
	AND SUBSTRING(loan_type, 1,1) = 'D'

INSERT INTO voyager.rcemailpop.EmailPopulation(BorrowerId, SchoolId, ServicerCode, Email, FirstName, CoBrandedLogo, SchoolLogo, SchoolName, SchoolEmail, SchoolPhone,
SchoolWebsite, ServicerName, ServicerWebsite, ServicerLogo, ServicerContactPage, ServicerDelayPage, ServicerRepaymentPage,
ServicerPhone, Bucket, SendGridTemplate, DaysDelinquent, FromEmailAddress)
SELECT DISTINCT 
	P.borrower_id,
	P.school_id,
	S.id,
	EMAIL.BorrowerEmail,
	UPPER(LEFT(lower(first_name),1))+LOWER(SUBSTRING(lower(first_name),2,LEN(lower(first_name)))) AS FirstName,
	CAST(SCH.rc_school_logo  AS VARCHAR(500)) AS CoBrandedLogo,
	CAST(SCH.school_logo AS VARCHAR(500)) AS SchoolLogo,
	sch.school_name,
	SCH.school_email,
	SCH.school_phone,
	CAST(SCH.website AS VARCHAR(500)) as SchoolWebsite,
	S.servicer_name,
	S.website,
	S.servicer_logo,
	S.contact,
	S.[delay],
	S.repay,
	S.phone,
	0 as bucket,
	@SendGrid AS SendGridTemplate,
	0 as DaysDelinquent,
	'info@repaycentsibly.org' AS FromEmailAddress
	
FROM
	Voyager..[Ports] P
	INNER JOIN voyager..servicers S
		ON P.current_ga_ed_servicer_code = S.id
	INNER JOIN voyager..schools SCH
		ON SCH.id = P.school_id
	INNER JOIN
	(
		SELECT DISTINCT
			E.borrower_id,
			E.email_address AS BorrowerEmail
		FROM 
			[voyager].[dbo].[emails] e
			INNER JOIN 
			(
				SELECT
					borrower_id, 
					MAX(email_effective_date) as email_effective_date
				FROM
					voyager..emails E
					LEFT JOIN [voyager].[rcemailpul].[UnsubscribedEmails] UNSUB
						ON UNSUB.EmailAddress = E.email_address
				WHERE
					email_good_flag = 'Y'
					AND UNSUB.EmailAddress IS NULL
				GROUP BY
					borrower_id
			)me
				on me.borrower_id = e.borrower_id
				and me.email_effective_date = e.email_effective_date
		WHERE
			E.email_good_flag = 'Y'
	) EMAIL
		 ON EMAIL.borrower_id = P.borrower_id
	LEFT JOIN voyager.rcemailpop.EmailPopulation EP
		ON EP.BorrowerId = P.borrower_id
		AND EP.SendGridTemplate = @SendGrid
		AND EP.DeletedAt IS NULL
		AND EP.AddedAt > DATEADD(DAY, -7, GETDATE())
	LEFT JOIN #TwoServicers TS
		ON TS.borrower_id = P.borrower_id
WHERE
	current_opb_amount > 0.00
	AND current_loan_status = 'FB'
	AND SUBSTRING(loan_type, 1,1) = 'D'
	AND EP.BorrowerId IS NULL
	AND TS.borrower_id IS NULL

RETURN 0

GO

CREATE PROCEDURE [rcemailpop].[CovidForb4]
	
AS
	DECLARE @SendGrid varchar(100) = 'd-03022550dd8b4103813be2cbfc9d67ce'

DROP TABLE IF EXISTS #TwoServicers
SELECT DISTINCT
	P.borrower_id,
	current_ga_ed_servicer_code AS ServicerCode,
	s.servicer_name
INTO #TwoServicers
FROM
(
SELECT DISTINCT
	borrower_id,
	COUNT(DISTINCT current_ga_ed_servicer_code) AS SER_COUNT
FROM
	voyager..[ports] P
WHERE
	current_opb_amount > 0.00
	AND current_loan_status = 'FB'
	AND SUBSTRING(loan_type, 1,1) = 'D'
GROUP BY
	borrower_id
HAVING 
	COUNT(DISTINCT current_ga_ed_servicer_code) > 1
) P
INNER JOIN voyager..[ports] POR
	ON POR.borrower_id = P.borrower_id
INNER JOIN voyager..servicers S
		ON S.id = POR.current_ga_ed_servicer_code
WHERE
	current_opb_amount > 0.00
	AND current_loan_status = 'FB'
	AND SUBSTRING(loan_type, 1,1) = 'D'

INSERT INTO voyager.rcemailpop.EmailPopulation(BorrowerId, SchoolId, ServicerCode, Email, FirstName, CoBrandedLogo, SchoolLogo, SchoolName, SchoolEmail, SchoolPhone,
SchoolWebsite, ServicerName, ServicerWebsite, ServicerLogo, ServicerContactPage, ServicerDelayPage, ServicerRepaymentPage,
ServicerPhone, Bucket, SendGridTemplate, DaysDelinquent, FromEmailAddress)
SELECT DISTINCT 
	P.borrower_id,
	P.school_id,
	S.id,
	EMAIL.BorrowerEmail,
	UPPER(LEFT(lower(first_name),1))+LOWER(SUBSTRING(lower(first_name),2,LEN(lower(first_name)))) AS FirstName,
	CAST(SCH.rc_school_logo  AS VARCHAR(500)) AS CoBrandedLogo,
	CAST(SCH.school_logo AS VARCHAR(500)) AS SchoolLogo,
	sch.school_name,
	SCH.school_email,
	SCH.school_phone,
	CAST(SCH.website AS VARCHAR(500)) as SchoolWebsite,
	S.servicer_name,
	S.website,
	S.servicer_logo,
	S.contact,
	S.[delay],
	S.repay,
	S.phone,
	0 as bucket,
	@SendGrid AS SendGridTemplate,
	0 as DaysDelinquent,
	'info@repaycentsibly.org' AS FromEmailAddress
	
FROM
	Voyager..[Ports] P
	INNER JOIN voyager..servicers S
		ON P.current_ga_ed_servicer_code = S.id
	INNER JOIN voyager..schools SCH
		ON SCH.id = P.school_id
	INNER JOIN
	(
		SELECT DISTINCT
			E.borrower_id,
			E.email_address AS BorrowerEmail
		FROM 
			[voyager].[dbo].[emails] e
			INNER JOIN 
			(
				SELECT
					borrower_id, 
					MAX(email_effective_date) as email_effective_date
				FROM
					voyager..emails E
					LEFT JOIN [voyager].[rcemailpul].[UnsubscribedEmails] UNSUB
						ON UNSUB.EmailAddress = E.email_address
				WHERE
					email_good_flag = 'Y'
					AND UNSUB.EmailAddress IS NULL
				GROUP BY
					borrower_id
			)me
				on me.borrower_id = e.borrower_id
				and me.email_effective_date = e.email_effective_date
		WHERE
			E.email_good_flag = 'Y'
	) EMAIL
		 ON EMAIL.borrower_id = P.borrower_id
	LEFT JOIN voyager.rcemailpop.EmailPopulation EP
		ON EP.BorrowerId = P.borrower_id
		AND EP.SendGridTemplate = @SendGrid
		AND EP.DeletedAt IS NULL
		AND EP.AddedAt > DATEADD(DAY, -7, GETDATE())
	LEFT JOIN #TwoServicers TS
		ON TS.borrower_id = P.borrower_id
WHERE
	current_opb_amount > 0.00
	AND current_loan_status = 'FB'
	AND SUBSTRING(loan_type, 1,1) = 'D'
	AND EP.BorrowerId IS NULL
	AND TS.borrower_id IS NULL

RETURN 0

GO

CREATE PROCEDURE [rcemailpop].[CovidForb5]
	
AS
	DECLARE @SendGrid varchar(100) = 'd-87d4d19940314e5bae51ef1ca4200eaf'

DROP TABLE IF EXISTS #TwoServicers
SELECT DISTINCT
	P.borrower_id,
	current_ga_ed_servicer_code AS ServicerCode,
	s.servicer_name
INTO #TwoServicers
FROM
(
SELECT DISTINCT
	borrower_id,
	COUNT(DISTINCT current_ga_ed_servicer_code) AS SER_COUNT
FROM
	voyager..[ports] P
WHERE
	current_opb_amount > 0.00
	AND current_loan_status = 'FB'
	AND SUBSTRING(loan_type, 1,1) = 'D'
GROUP BY
	borrower_id
HAVING 
	COUNT(DISTINCT current_ga_ed_servicer_code) > 1
) P
INNER JOIN voyager..[ports] POR
	ON POR.borrower_id = P.borrower_id
INNER JOIN voyager..servicers S
		ON S.id = POR.current_ga_ed_servicer_code
WHERE
	current_opb_amount > 0.00
	AND current_loan_status = 'FB'
	AND SUBSTRING(loan_type, 1,1) = 'D'

INSERT INTO voyager.rcemailpop.EmailPopulation(BorrowerId, SchoolId, ServicerCode, Email, FirstName, CoBrandedLogo, SchoolLogo, SchoolName, SchoolEmail, SchoolPhone,
SchoolWebsite, ServicerName, ServicerWebsite, ServicerLogo, ServicerContactPage, ServicerDelayPage, ServicerRepaymentPage,
ServicerPhone, Bucket, SendGridTemplate, DaysDelinquent, FromEmailAddress)
SELECT DISTINCT 
	P.borrower_id,
	P.school_id,
	S.id,
	EMAIL.BorrowerEmail,
	UPPER(LEFT(lower(first_name),1))+LOWER(SUBSTRING(lower(first_name),2,LEN(lower(first_name)))) AS FirstName,
	CAST(SCH.rc_school_logo  AS VARCHAR(500)) AS CoBrandedLogo,
	CAST(SCH.school_logo AS VARCHAR(500)) AS SchoolLogo,
	sch.school_name,
	SCH.school_email,
	SCH.school_phone,
	CAST(SCH.website AS VARCHAR(500)) as SchoolWebsite,
	S.servicer_name,
	S.website,
	S.servicer_logo,
	S.contact,
	S.[delay],
	S.repay,
	S.phone,
	0 as bucket,
	@SendGrid AS SendGridTemplate,
	0 as DaysDelinquent,
	'info@repaycentsibly.org' AS FromEmailAddress
	
FROM
	Voyager..[Ports] P
	INNER JOIN voyager..servicers S
		ON P.current_ga_ed_servicer_code = S.id
	INNER JOIN voyager..schools SCH
		ON SCH.id = P.school_id
	INNER JOIN
	(
		SELECT DISTINCT
			E.borrower_id,
			E.email_address AS BorrowerEmail
		FROM 
			[voyager].[dbo].[emails] e
			INNER JOIN 
			(
				SELECT
					borrower_id, 
					MAX(email_effective_date) as email_effective_date
				FROM
					voyager..emails E
					LEFT JOIN [voyager].[rcemailpul].[UnsubscribedEmails] UNSUB
						ON UNSUB.EmailAddress = E.email_address
				WHERE
					email_good_flag = 'Y'
					AND UNSUB.EmailAddress IS NULL
				GROUP BY
					borrower_id
			)me
				on me.borrower_id = e.borrower_id
				and me.email_effective_date = e.email_effective_date
		WHERE
			E.email_good_flag = 'Y'
	) EMAIL
		 ON EMAIL.borrower_id = P.borrower_id
	LEFT JOIN voyager.rcemailpop.EmailPopulation EP
		ON EP.BorrowerId = P.borrower_id
		AND EP.SendGridTemplate = @SendGrid
		AND EP.DeletedAt IS NULL
		AND EP.AddedAt > DATEADD(DAY, -7, GETDATE())
	LEFT JOIN #TwoServicers TS
		ON TS.borrower_id = P.borrower_id
WHERE
	current_opb_amount > 0.00
	AND current_loan_status = 'FB'
	AND SUBSTRING(loan_type, 1,1) = 'D'
	AND EP.BorrowerId IS NULL
	AND TS.borrower_id IS NULL

RETURN 0

GO
