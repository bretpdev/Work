CREATE PROCEDURE [dbo].[spGetBFRecordData]

@StartDate DateTime = NULL,
@EndDate DateTime = NULL,
@AppId INT = NULL

AS
BEGIN

	SET NOCOUNT ON;

	IF(@AppId = -1) --Uses this when selecting records by date range and not application ID
		BEGIN
			SELECT DISTINCT
				BFHIS.application_id AS ApplicationId,
				BFHIS.award_id AS AwardId,
				APP.e_application_id AS EApplicationId,
				BFHIS.disclosure_date AS BFDisclosureDate,
				ISNULL(LNS.loan_seq, '    ') AS LoanSeq
			FROM
				dbo.BF_Data_History BFHIS
			INNER JOIN Applications APP
				ON APP.application_id = BFHIS.application_id
			LEFT JOIN dbo.Loans LNS
				ON LNS.application_id = BFHIS.application_id
				AND lns.award_id = BFHIS.award_id
			WHERE
				BFHIS.created_at BETWEEN @StartDate AND @EndDate
				AND APP.Active = 1
				AND app.updated_by != 'IDRXMLDATA'--has been imported but not reviewed
		END
	ELSE
		BEGIN
			SELECT DISTINCT
					BFHIS.application_id AS ApplicationId,
					BFHIS.award_id AS AwardId,
					APP.e_application_id AS EApplicationId,
					BFHIS.disclosure_date AS BFDisclosureDate,
					ISNULL(LNS.loan_seq, '    ') AS LoanSeq
				FROM
					dbo.BF_Data_History BFHIS
				INNER JOIN Applications APP
					ON APP.application_id = BFHIS.application_id
				LEFT JOIN dbo.Loans LNS
					ON LNS.application_id = BFHIS.application_id
					AND lns.award_id = BFHIS.award_id
				WHERE
					BFHIS.application_id = @AppId
					AND APP.Active = 1
					AND app.updated_by != 'IDRXMLDATA'--has been imported but not reviewed
		END
END