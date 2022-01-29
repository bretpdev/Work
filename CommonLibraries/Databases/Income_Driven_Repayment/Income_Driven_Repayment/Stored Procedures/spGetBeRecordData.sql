
-- =============================================
-- Author:		JAROM RYAN
-- Create date: 06/21/2013
-- Description:	THIS SP WILL PULL DATA FOR THE BE RECORDS IN THE NSLDS FILE
-- =============================================
CREATE PROCEDURE [dbo].[spGetBeRecordData]

@StartDate DateTime = NULL,
@EndDate DateTime = NULL

AS
BEGIN

	SET NOCOUNT ON;

	SELECT DISTINCT
		BEHIS.application_id AS ApplicationId,
		BEHIS.award_id AS AwardId,
		APP.e_application_id AS EApplicationId,
		RT.repayment_type_code AS BERepaymentTypeProgram,
		BEHIS.requested_by_borrower AS BERequestedByBorrower,
		RTS.repayment_type_status AS BERepaymentPlanTypeStatus,
		BEHIS.repayment_application_status AS BERepaymentPlanTypeStausDate,
		BDHIS.family_size AS BEFamilySize,
		BEHIS.total_income AS TotalIncome,
		ISNULL(LNS.loan_seq, '    ') AS LoanSeq,
		App.RepaymentTypeProcessedNotSame as NotSame
		
	FROM
		dbo.BE_Data_History BEHIS
	LEFT JOIN BD_Data_History BDHIS
		ON BEHIS.application_id = BDHIS.application_id
	LEFT JOIN dbo.Repayment_Plan_Type_Substatus SUBSTA
		ON SUBSTA.repayment_plan_type_substatus_id = BEHIS.substatus_mapping_id
	LEFT JOIN dbo.Repayment_Type_Status RTS
		ON RTS.repayment_type_status_id = SUBSTA.repayment_type_status_id
	INNER JOIN Applications APP
		ON APP.application_id = BEHIS.application_id
	LEFT JOIN dbo.Repayment_Plan_Selected RPS
		ON RPS.application_id = APP.application_id
	LEFT JOIN dbo.Repayment_Type RT
		ON RT.repayment_type_id = RPS.repayment_type_id
	LEFT JOIN dbo.Loans LNS
		ON LNS.application_id = BEHIS.application_id
		AND lns.award_id = BEHIS.award_id
	WHERE
		BEHIS.created_at BETWEEN @StartDate AND @EndDate
		AND APP.Active = 1
		AND (APP.created_by != 'IDRXMLDATA' AND app.updated_by != 'IDRXMLDATA')--has been imported but not reviewed
		
END


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[spGetBeRecordData] TO [db_executor]
    AS [dbo];

