-- START PROMOTION SCRIPT --

USE CLS
GO
IF EXISTS(SELECT NULL FROM SYS.TABLES WHERE name = 'ServicerVolume')
BEGIN
	DROP TABLE REPAYE.ServicerVolume
END
GO
IF EXISTS(SELECT NULL FROM SYS.TABLES WHERE name = 'TopDenialReasons')
BEGIN
	DROP TABLE REPAYE.TopDenialReasons
END
GO
IF EXISTS(SELECT NULL FROM SYS.objects WHERE name = 'START_OF_WEEK')
BEGIN
	DROP FUNCTION START_OF_WEEK
END
GO
IF EXISTS(SELECT NULL FROM SYS.schemas WHERE name LIKE '%REPAYE%')
BEGIN
	DROP SCHEMA REPAYE
END
GO
CREATE SCHEMA REPAYE
GO
GRANT SELECT ON SCHEMA::[REPAYE] TO [!UHEAASSRS]
GO
GRANT SELECT ON SCHEMA::[REPAYE] TO [UHEAA\SystemAnalysts]
GO
GRANT SELECT ON SCHEMA::[REPAYE] TO [UHEAA\Developers]
GO
GRANT VIEW DEFINITION ON SCHEMA::[REPAYE] TO [UHEAA\Developers]
GO
GRANT EXECUTE ON CLS.DBO.START_OF_WEEK TO [!UHEAASSRS]
GO
CREATE TABLE REPAYE.ServicerVolume
(
	ServicerVolumeId INT IDENTITY(X,X),
	DueDate DATE NOT NULL,
	BeginDate DATE NOT NULL,
	EndDate DATE NOT NULL,
	PhoneInquiries INT,
	WebInquiries INT,
	EmailInquiries INT,
	OtherInquiries INT,
	ApplicationsReceived INT,
	ApplicationsApproved INT,
	ApplicationsDenied INT,
	ApplicationsInProgress INT,
	OldestProgressDate DATE,
	ApplicationTypeOnline INT,
	ApplicationTypePaper INT
)

GO

CREATE TABLE REPAYE.TopDenialReasons
(
	EndDate DATE NOT NULL,
	DenialReasonX VARCHAR(XXX),
	DenialReasonX VARCHAR(XXX),
	DenialReasonX VARCHAR(XXX)
)

GO

CREATE FUNCTION dbo.START_OF_WEEK
(
    @DATE DATETIME,
    -- Sun = X, Mon = X, Tue = X, Wed = X
    -- Thu = X, Fri = X, Sat = X
    -- Default to Sunday
    @WEEK_START_DAY INT = X 
)
/*
Find the fisrt date on or before @DATE that matches 
day of week of @WEEK_START_DAY.
*/
RETURNS DATETIME
AS
BEGIN
	DECLARE
		@START_OF_WEEK_DATE DATETIME,
		@FIRST_BOW DATETIME

	-- Check for valid day of week
	IF @WEEK_START_DAY between X and X
		BEGIN
		-- Find first day on or after XXXX/X/X (-XXXXX)
		-- matching day of week of @WEEK_START_DAY
		-- XXXX/X/X is earliest possible SQL Server date.
		SELECT @FIRST_BOW = CONVERT(DATETIME,-XXXXX+((@WEEK_START_DAY+X)%X))
		-- Verify beginning of week not before XXXX/X/X
		IF @DATE >= @FIRST_BOW
			BEGIN
				SELECT @START_OF_WEEK_DATE = 
				DATEADD(dd,(DATEDIFF(dd,@FIRST_BOW,@DATE)/X)*X,@FIRST_BOW)
			END
		END

	RETURN @START_OF_WEEK_DATE
END
GO

-- (SEED DATA) insert historical date records --
INSERT INTO
	REPAYE.ServicerVolume
	(
		DueDate,
		BeginDate,
		EndDate
	)
VALUES
	('X/XX/XXXX', 'XX/XX/XXXX', 'X/X/XXXX')

INSERT INTO
	REPAYE.TopDenialReasons
	(
		EndDate
	)
VALUES
	('X/X/XXXX')
-- END PROMOTION SCRIPT --


-- START SCHEDULED SCRIPT --
DECLARE	@beginDate DATE = CLS.dbo.START_OF_WEEK(GETDATE(), X) -- X = consider Saturday the first day of the week
DECLARE @endDate DATE = DATEADD(DAY, X, @beginDate)


-- insert weekly date information if it doesn't already exist
IF NOT EXISTS(SELECT NULL FROM REPAYE.ServicerVolume WHERE BeginDate = @beginDate)
BEGIN
	INSERT INTO	REPAYE.ServicerVolume
	(
		DueDate,
		BeginDate,
		EndDate
	)
	SELECT
		DATEADD(DAY, XX, @beginDate) [DueDate],
		@beginDate,
		@endDate
	FROM
		REPAYE.ServicerVolume SV
END

IF NOT EXISTS(SELECT NULL FROM REPAYE.TopDenialReasons WHERE EndDate = @EndDate)
BEGIN
	INSERT INTO	REPAYE.TopDenialReasons
	(
		EndDate
	)
	SELECT
		@endDate
	FROM
		REPAYE.TopDenialReasons TDR
END



-- ACCOUNTS --
SELECT
	B.SSN
FROM
	[Income_Driven_Repayment].[dbo].Repayment_Plan_selected RPS
	INNER JOIN
	(
		SELECT
			RPS.application_id,
			HIS.repayment_plan_type_status_history_id as repayment_plan_type_status_history_id
		FROM 
			[Income_Driven_Repayment].[dbo].[Repayment_Plan_Type_Status_History] HIS
			INNER JOIN [Income_Driven_Repayment].[dbo].Repayment_Plan_selected RPS ON HIS.repayment_plan_type_id = RPS.repayment_plan_type_id
		WHERE
			RPS.repayment_type_id = X -- REPAYE
	) max_val ON max_val.application_id = RPS.application_id
	INNER JOIN [Income_Driven_Repayment].dbo.Repayment_Plan_Type_Status_History HIS
		ON HIS.repayment_plan_type_status_history_id = max_val.repayment_plan_type_status_history_id
	INNER JOIN [Income_Driven_Repayment].[dbo].[Repayment_Plan_Type_Substatus] SUB
		ON SUB.repayment_plan_type_substatus_id = HIS.repayment_plan_type_status_mapping_id
	INNER JOIN [Income_Driven_Repayment].dbo.Applications app
		ON app.application_id = RPS.application_id
	INNER JOIN [Income_Driven_Repayment].dbo.Loans L
		ON l.application_id = app.application_id
	INNER JOIN [Income_Driven_Repayment].dbo.Borrowers B
		ON B.borrower_id = L.borrower_id
WHERE 
	HIS.created_at BETWEEN @beginDate AND @endDate

UNION

SELECT
	B.BF_SSN [SSN]
FROM
	CDW.dbo.PDXX_Borrower B
	INNER JOIN CDW.dbo.AYXX_History AYXX ON AYXX.DF_SPE_ACC_ID = B.DF_SPE_ACC_ID
WHERE
	CAST(AYXX.LD_ATY_REQ_RCV AS DATE) BETWEEN @beginDate AND @endDate
    AND 
	AYXX.PF_REQ_ACT IN ('RPAYE','REPEM') -- ?REPAYE Inquiry?, Email Inquiry

-- *************  RUN ONE TIME ONLY THEN DELETE *************
--SET	@beginDate = 'XX/XX/XXXX'
--SET	@endDate = 'XX/XX/XXXX'
-- **********************  END DELETE  **********************


-- INQUIRIES --
UPDATE
	REPAYE.ServicerVolume
SET
	PhoneInquiries = SUMS.PhoneInquiries,
	WebInquiries = SUMS.WebInquiries,
	EmailInquiries = SUMS.EmailInquiries,
	OtherInquiries = SUMS.OtherInquiries
FROM
	(
		SELECT
			SUM(CASE AYXX.PF_REQ_ACT WHEN 'RPAYE' THEN X ELSE X END) [PhONeInquiries],
			SUM(CASE AYXX.PF_REQ_ACT WHEN '????' THEN X ELSE X END) [WebInquiries],
			SUM(CASE AYXX.PF_REQ_ACT WHEN 'REPEM' THEN X ELSE X END) [EmailInquiries],
			SUM(CASE AYXX.PF_REQ_ACT WHEN '????' THEN X ELSE X END) [OtherInquiries]
		FROM
			CDW.dbo.AYXX_History AYXX
		WHERE
			CAST(AYXX.LD_ATY_REQ_RCV as date) BETWEEN @beginDate AND @endDate
	) SUMS
WHERE
	BeginDate = @beginDate
	

--APPLICATIONS RECEIVED--
UPDATE
	REPAYE.ServicerVolume
SET
	ApplicationsReceived = AR.ApplicationsReceived
FROM
	(
		SELECT
			COUNT(*) [ApplicationsReceived]
		FROM
			CDW.dbo.AYXX_History AYXX
		WHERE
			CAST(AYXX.LD_ATY_REQ_RCV as date) BETWEEN @beginDate AND @endDate
			AND 
			AYXX.PF_REQ_ACT in ('CODPA','IDRPR', 'CODCA') -- COD IDR, paper IDR, COD IDR
	) AR
WHERE
	BeginDate = @beginDate


--APPLICATIONS APPROVED--
UPDATE
	REPAYE.ServicerVolume
SET
	ApplicationsApproved = AA.ApplicationsApproved
FROM
	(
		SELECT
			COUNT(*) [ApplicationsApproved]
		FROM
			[Income_Driven_Repayment].[dbo].Repayment_Plan_selected RPS
			INNER JOIN
			(
				SELECT
					RPS.application_id,
					HIS.repayment_plan_type_status_history_id [repayment_plan_type_status_history_id]
				FROM 
					[Income_Driven_Repayment].[dbo].[Repayment_Plan_Type_Status_History] HIS
					INNER JOIN [Income_Driven_Repayment].[dbo].Repayment_Plan_selected RPS ON HIS.repayment_plan_type_id = RPS.repayment_plan_type_id
				WHERE
					RPS.repayment_type_id = X -- REPAYE
			) max_val ON max_val.application_id = RPS.application_id
			INNER JOIN [Income_Driven_Repayment].[dbo].Repayment_Plan_Type_Status_History HIS ON HIS.repayment_plan_type_status_history_id = max_val.repayment_plan_type_status_history_id
			INNER JOIN [Income_Driven_Repayment].[dbo].[Repayment_Plan_Type_Substatus] SUB ON SUB.repayment_plan_type_substatus_id = HIS.repayment_plan_type_status_mapping_id
			INNER JOIN [Income_Driven_Repayment].[dbo].Applications APP ON APP.application_id = RPS.application_id
		WHERE 
			HIS.created_at BETWEEN @beginDate AND @endDate
			AND 
			SUB.repayment_plan_type_status_id = X -- APPROVED
	) AA
WHERE
	BeginDate = @beginDate


--APPLICATIONS DENIED--
UPDATE
	REPAYE.ServicerVolume
SET
	ApplicationsDenied = AD.ApplicationsDenied
FROM
	(
		SELECT
			COUNT(*) [ApplicationsDenied]
		FROM
			[Income_Driven_Repayment].[dbo].Repayment_Plan_selected RPS
			INNER JOIN
			(
				SELECT
					RPS.application_id,
					HIS.repayment_plan_type_status_history_id as repayment_plan_type_status_history_id
				FROM 
					[Income_Driven_Repayment].[dbo].[Repayment_Plan_Type_Status_History] HIS
					INNER JOIN [Income_Driven_Repayment].[dbo].Repayment_Plan_selected RPS ON HIS.repayment_plan_type_id = RPS.repayment_plan_type_id
				WHERE
					RPS.repayment_type_id = X -- REPAYE
			) max_val ON max_val.application_id = RPS.application_id
			INNER JOIN [Income_Driven_Repayment].[dbo].Repayment_Plan_Type_Status_History HIS
				ON HIS.repayment_plan_type_status_history_id = max_val.repayment_plan_type_status_history_id
			INNER JOIN [Income_Driven_Repayment].[dbo].[Repayment_Plan_Type_Substatus] SUB
				ON SUB.repayment_plan_type_substatus_id = HIS.repayment_plan_type_status_mapping_id
			INNER JOIN [Income_Driven_Repayment].[dbo].Applications APP
				ON APP.application_id = RPS.application_id
		WHERE 
			HIS.created_at BETWEEN @beginDate AND @endDate
			AND SUB.repayment_plan_type_status_id = X -- DENIED
	) AD
WHERE
	BeginDate = @beginDate


--APPLICATIONS IN PROGRESS--
DECLARE 
	@ApplicationsReceived INT,
	@ApplicationsProcessed INT

	-- set applications received
	SELECT
		@ApplicationsReceived = COUNT(*)
	FROM
		CDW.dbo.AYXX_History AYXX
	WHERE
		CAST(AYXX.LD_ATY_REQ_RCV as DATE) BETWEEN @beginDate AND @endDate
		AND 
		AYXX.PF_REQ_ACT in ('CODPA', 'IDRPR', 'CODCA') -- COD IDR, paper IDR, COD IDR

	-- set applications processed
	SELECT
		@ApplicationsProcessed = COUNT(*)
	FROM
		[Income_Driven_Repayment].[dbo].Repayment_Plan_selected RPS
		INNER JOIN
		(
			SELECT
				RPS.application_id,
				HIS.repayment_plan_type_status_history_id as repayment_plan_type_status_history_id
			FROM 
				[Income_Driven_Repayment].[dbo].[Repayment_Plan_Type_Status_History] HIS
				INNER JOIN [Income_Driven_Repayment].[dbo].Repayment_Plan_selected RPS ON HIS.repayment_plan_type_id = RPS.repayment_plan_type_id
			WHERE
				RPS.repayment_type_id = X -- REPAYE
		) max_val ON max_val.application_id = RPS.application_id
		INNER JOIN [Income_Driven_Repayment].[dbo].Repayment_Plan_Type_Status_History HIS
			ON HIS.repayment_plan_type_status_history_id = max_val.repayment_plan_type_status_history_id
		INNER JOIN [Income_Driven_Repayment].[dbo].[Repayment_Plan_Type_Substatus] SUB
			ON SUB.repayment_plan_type_substatus_id = HIS.repayment_plan_type_status_mapping_id
		INNER JOIN [Income_Driven_Repayment].[dbo].Applications APP
			ON APP.application_id = RPS.application_id
	WHERE 
		HIS.created_at BETWEEN @beginDate AND @endDate
		AND 
		SUB.repayment_plan_type_status_id in (X, X, X) -- (APPROVED, DENIED, PENDING)

UPDATE
	REPAYE.ServicerVolume
SET
	ApplicationsInProgress = @ApplicationsReceived - @ApplicationsProcessed
WHERE
	BeginDate = @beginDate


--OLDEST DATE OF APPLICATION--
UPDATE
	REPAYE.ServicerVolume
SET 
	OldestProgressDate = OPD.OldestProgressDate
FROM
	(
		SELECT
			MAX(CAST(AYXX.LD_ATY_REQ_RCV as DATE)) [OldestProgressDate]
		FROM
			CDW.dbo.AYXX_History AYXX
			LEFT JOIN
			(
				SELECT
					B.account_number
				FROM
					[Income_Driven_Repayment].[dbo].Repayment_Plan_selected RPS
					INNER JOIN
					(
						SELECT
							RPS.application_id,
							HIS.repayment_plan_type_status_history_id as repayment_plan_type_status_history_id
						FROM 
							[Income_Driven_Repayment].[dbo].[Repayment_Plan_Type_Status_History] HIS
							INNER JOIN [Income_Driven_Repayment].[dbo].Repayment_Plan_selected RPS ON HIS.repayment_plan_type_id = RPS.repayment_plan_type_id
						WHERE
							RPS.repayment_type_id = X -- REPAYE
					) max_val ON max_val.application_id = RPS.application_id
					INNER JOIN [Income_Driven_Repayment].dbo.Repayment_Plan_Type_Status_History HIS
						ON HIS.repayment_plan_type_status_history_id = max_val.repayment_plan_type_status_history_id
					INNER JOIN [Income_Driven_Repayment].[dbo].[Repayment_Plan_Type_Substatus] SUB
						ON SUB.repayment_plan_type_substatus_id = HIS.repayment_plan_type_status_mapping_id
					INNER JOIN [Income_Driven_Repayment].dbo.Applications app
						ON app.application_id = RPS.application_id
					INNER JOIN [Income_Driven_Repayment].dbo.Loans L
						ON L.application_id = app.application_id
					INNER JOIN [Income_Driven_Repayment].dbo.Borrowers B
						ON B.borrower_id = L.borrower_id
				WHERE 
					HIS.created_at BETWEEN @beginDate AND @endDate
			) processed ON processed.account_number = AYXX.DF_SPE_ACC_ID
		WHERE
			CAST(AYXX.LD_ATY_REQ_RCV as DATE) BETWEEN @beginDate AND @endDate
			AND 
			AYXX.PF_REQ_ACT in ('CODPA','IDRPR', 'CODCA') -- COD IDR, paper IDR, COD IDR
			AND
			processed.account_number is NULL
	) OPD
WHERE
	BeginDate = @beginDate


--ONLINE APPS--
UPDATE
	REPAYE.ServicerVolume
SET
	ApplicationTypeOnline = OA.OnlineApps
FROM
	(
		SELECT
			COUNT(*) [OnlineApps]
		FROM
			CDW.dbo.AYXX_History AYXX
		WHERE
			CAST(AYXX.LD_ATY_REQ_RCV as DATE) BETWEEN @beginDate AND @endDate
			AND 
			AYXX.PF_REQ_ACT in ('CODPA', 'CODCA') -- COD IDR, COD IDR
	) OA
WHERE
	BeginDate = @beginDate


--PAPER APPS--
UPDATE
	REPAYE.ServicerVolume
SET
	ApplicationTypePaper = PA.ApplicationTypePaper
FROM
	(
		SELECT
			COUNT(*) [ApplicationTypePaper]
		FROM
			CDW.dbo.AYXX_History AYXX
		WHERE
			CAST(AYXX.LD_ATY_REQ_RCV as DATE) BETWEEN @beginDate AND @endDate
		AND 
			AYXX.PF_REQ_ACT = 'IDRPR' -- Paper IDR
	) PA
WHERE
	BeginDate = @beginDate


-- TOP X DENIAL REASONS--
UPDATE 
	REPAYE.TopDenialReasons
SET
	DenialReasonX = DR.DenialReasonX,
	DenialReasonX = DR.DenialReasonX,
	DenialReasonX = DR.DenialReasonX
FROM
	(
		SELECT
			MAX(CASE WHEN RNK.row_num = X THEN RNK.DenialReason END) [DenialReasonX], 
			MAX(CASE WHEN RNK.row_num = X THEN RNK.DenialReason END) [DenialReasonX],
			MAX(CASE WHEN RNK.row_num = X THEN RNK.DenialReason END) [DenialReasonX]
		FROM
			(
				SELECT
					CNT.DenialReason,
					CNT.DenialReasonCount,
					ROW_NUMBER() OVER(ORDER BY CNT.DenialReasonCount DESC) [row_num]  --largest count will have row_num value of X
				FROM
					(
						SELECT
							SUB.repayment_plan_type_substatus [DenialReason],
							COUNT(*) [DenialReasonCount]
						FROM
							[Income_Driven_Repayment].[dbo].Repayment_Plan_selected RPS
							INNER JOIN
							(
								SELECT
									RPS.application_id,
									HIS.repayment_plan_type_status_history_id as repayment_plan_type_status_history_id
								FROM 
									[Income_Driven_Repayment].[dbo].[Repayment_Plan_Type_Status_History] HIS
									INNER JOIN [Income_Driven_Repayment].dbo.Repayment_Plan_selected RPS
										on HIS.repayment_plan_type_id = RPS.repayment_plan_type_id
								WHERE
									RPS.repayment_type_id = X -- REPAYE
							) max_val on max_val.application_id = RPS.application_id
							INNER JOIN [Income_Driven_Repayment].dbo.Repayment_Plan_Type_Status_History HIS
								on HIS.repayment_plan_type_status_history_id = max_val.repayment_plan_type_status_history_id
							INNER JOIN [Income_Driven_Repayment].dbo.[Repayment_Plan_Type_Substatus] SUB
								on SUB.repayment_plan_type_substatus_id = his.repayment_plan_type_status_mapping_id
							INNER JOIN [Income_Driven_Repayment].dbo.Applications APP
								on APP.application_id = RPS.application_id
						WHERE 
							HIS.created_at BETWEEN @beginDate AND @endDate
							AND 
							SUB.repayment_plan_type_status_id = X --DENIED
						GROUP BY
							SUB.repayment_plan_type_substatus
					) CNT
			) RNK
		WHERE
			RNK.row_num <= X -- limit to the three with the largest count
	) DR
WHERE
	EndDate = @endDate


-- ACCOUNTS --
SELECT
	B.SSN
FROM
	[Income_Driven_Repayment].[dbo].Repayment_Plan_selected RPS
	INNER JOIN
	(
		SELECT
			RPS.application_id,
			HIS.repayment_plan_type_status_history_id as repayment_plan_type_status_history_id
		FROM 
			[Income_Driven_Repayment].[dbo].[Repayment_Plan_Type_Status_History] HIS
			INNER JOIN [Income_Driven_Repayment].[dbo].Repayment_Plan_selected RPS ON HIS.repayment_plan_type_id = RPS.repayment_plan_type_id
		WHERE
			RPS.repayment_type_id = X -- REPAYE
	) max_val ON max_val.application_id = RPS.application_id
	INNER JOIN [Income_Driven_Repayment].dbo.Repayment_Plan_Type_Status_History HIS
		ON HIS.repayment_plan_type_status_history_id = max_val.repayment_plan_type_status_history_id
	INNER JOIN [Income_Driven_Repayment].[dbo].[Repayment_Plan_Type_Substatus] SUB
		ON SUB.repayment_plan_type_substatus_id = HIS.repayment_plan_type_status_mapping_id
	INNER JOIN [Income_Driven_Repayment].dbo.Applications app
		ON app.application_id = RPS.application_id
	INNER JOIN [Income_Driven_Repayment].dbo.Loans L
		ON l.application_id = app.application_id
	INNER JOIN [Income_Driven_Repayment].dbo.Borrowers B
		ON B.borrower_id = L.borrower_id
WHERE 
	HIS.created_at BETWEEN @beginDate AND @endDate

UNION

SELECT
	B.BF_SSN [SSN]
FROM
	CDW.dbo.PDXX_Borrower B
	INNER JOIN CDW.dbo.AYXX_History AYXX ON AYXX.DF_SPE_ACC_ID = B.DF_SPE_ACC_ID
WHERE
	CAST(AYXX.LD_ATY_REQ_RCV AS DATE) BETWEEN @beginDate AND @endDate
    AND 
	AYXX.PF_REQ_ACT IN ('RPAYE','REPEM') -- ?REPAYE Inquiry?, Email Inquiry



