-- =============================================
-- Author:		JAROM RYAN
-- Create date: 05/30/2013
-- Description:	WILL INSERT THE REPAYMENT PLAN SELECTED
-- =============================================
CREATE PROCEDURE [dbo].[spInsertSelectedPlan]

@AppId int,
@RepaymentTypeId int

AS
BEGIN

	SET NOCOUNT ON;

INSERT INTO dbo.Repayment_Plan_Selected(application_id,repayment_type_id)
VALUES(@AppId, @RepaymentTypeId)

SELECT SCOPE_IDENTITY()

END
