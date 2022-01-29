﻿-- =============================================
-- Author:		JAROM RYAN
-- Create date: 06/21/2013
-- Description:	THIS WILL GET THE repayment_plan_type_requested_id FOR A GIVEN APP ID
-- =============================================
CREATE PROCEDURE [dbo].[spGetRepaymentPlanRequested]

@AppId INT

AS
BEGIN

	SET NOCOUNT ON;

	SELECT 
		repayment_plan_type_requested_id
	FROM
		dbo.Applications
	WHERE 
		application_id = @AppId
		
END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[spGetRepaymentPlanRequested] TO [db_executor]
    AS [dbo];

