USE CDW

GO
SELECT
	*
FROM
	AYXX_BR_LON_ATY AYXX
WHERE
	AYXX.LD_ATY_REQ_RCV BETWEEN 'XXXX/XX/XX' AND 'XXXX/XX/XX'
	AND
	AYXX.PF_REQ_ACT IN ('IDRPR', 'IBRDF', 'CODCA', 'CODPA')
	AND
	AYXX.LC_STA_ACTYXX = 'A'

USE Income_Driven_Repayment
GO	

/*

	@StartDate DATE = 'XXXX-X-X',
	@EndDate DATE = 'XXXX-X-XX'

	@StartDate DATE = 'XXXX-X-X',
	@EndDate DATE = 'XXXX-X-XX'
	
	@StartDate DATE = 'XXXX-XX-XX',
	@EndDate DATE = 'XXXX-XX-XX'

*/

DECLARE
	@StartDate DATE = 'XXXX-X-X',
	@EndDate DATE = 'XXXX-X-XX'

SELECT CAST(@StartDate AS VARCHAR(XX)) + ' - ' + CAST(@EndDate AS VARCHAR(XX)) [DateRange]

-- total applications
SELECT
	COUNT(A.application_id) [ApplicationCount]
FROM
	Applications A
WHERE
	A.created_at BETWEEN DATEADD(DAY, -XX, @StartDate) AND @EndDate
	ANd
	A.Active = X

-- #X 
-- abandoned IDR applications
SELECT
	*
FROM
	(
		SELECT
			A.application_id,
			A.created_at,
			RPTSH.repayment_plan_type_status_mapping_id,
			ROW_NUMBER() OVER (PARTITION BY A.application_id ORDER BY RPTSH.created_at DESC) [seq]
		FROM
			Applications A
			INNER JOIN Repayment_Plan_Selected RPS ON RPS.application_id = A.application_id
			INNER JOIN Repayment_Plan_Type_Status_History RPTSH ON RPTSH.repayment_plan_type_id = RPS.repayment_plan_type_id
		WHERE
			A.Active = X
			AND
			A.created_at BETWEEN DATEADD(DAY, -XX, @StartDate) AND @EndDate
	) Apps 
	INNER JOIN Repayment_Plan_Type_Substatus RPTS ON RPTS.repayment_plan_type_substatus_id = Apps.repayment_plan_type_status_mapping_id and Apps.seq = X
WHERE
	( -- if an application is pending more than XX days after it is created consider it abandoned
		RPTS.repayment_plan_type_status_id = X
		AND
		Apps.created_at < DATEADD(DAY, -XX, GETDATE())
	)
	OR
	(  -- if deneid for any of these resonson consider the application denied
		RPTS.repayment_plan_type_status_id = X
		AND
		RPTS.repayment_plan_type_substatus_id IN (XX, XX, XX, XX)
	)

--january feb march
--jan X -> denied on april Xst 

--SELECT
--	*
--FROM
--	Repayment_Plan_Type_Substatus RPTS
--WHERE
--	RPTS.repayment_plan_type_substatus_id IN (XX, XX, XX, XX)


/*
application_source_id	application_source
X	Paper
X	Electronic
*/

-- #X borrower count by source, if both sources are occur count the electronic application
--#X XXXX-XX-XX - XXXX-XX-XX: UniqueBorrowersWithPaperApp:XXXX; UniqueBorrowersWithElectronicApp:XXXX
SELECT
	SUM(CASE WHEN BA.application_source_id = X THEN X ELSE X END) [UniqueBorrowersWithPaperApp],
	SUM(CASE WHEN BA.application_source_id = X THEN X ELSE X END) [UniqueBorrowersWithElectronicApp]
FROM
	( -- borrower applications
		SELECT
			Ln.borrower_id,
			A.application_source_id,
			ROW_NUMBER() OVER (PARTITION BY Ln.borrower_id ORDER BY A.application_source_id DESC) [source_precedent]
		FROM
			(
				SELECT DISTINCT
					borrower_id,
					application_id
				FROM
					Income_Driven_Repayment.dbo.Loans
			) Ln
			INNER JOIN Income_Driven_Repayment.dbo.Applications A ON A.application_id = Ln.application_id
		WHERE	
			A.created_at BETWEEN @StartDate AND @EndDate
			AND
			A.Active = X
	) BA
WHERE
	BA.source_precedent = X -- count only the highest precentent applicaiton

-- #X XXXX-XX-XX - XXXX-XX-XX: PaperApplications:XXXX; ElectronicApplicaitons:XXXX 
SELECT
	SUM(CASE WHEN A.application_source_id = X THEN X ELSE X END) [PaperApps],
	SUM(CASE WHEN A.application_source_id = X THEN X ELSE X END) [ElectronicApps]
FROM
	Applications A
WHERE
	A.created_at BETWEEN @StartDate AND @EndDate
	ANd
	A.Active = X