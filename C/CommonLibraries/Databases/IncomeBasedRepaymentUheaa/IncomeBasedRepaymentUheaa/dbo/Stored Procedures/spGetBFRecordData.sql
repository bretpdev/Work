
-- =============================================
-- Author:		JAROM RYAN
-- Create date: 06/21/2013
-- Description:	WILL PULL ALL OF THE DATA NEEDED FOR THE BF RECORDS IN THE NSLDS FILE
-- =============================================
CREATE PROCEDURE [dbo].[spGetBFRecordData] 

@StartDate DateTime = NULL,
@EndDate DateTime = NULL

AS
BEGIN

	SET NOCOUNT ON;

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
END
