﻿-- =============================================
-- Author:		JAROM RYAN
-- Create date: 06/03/2013
-- Description: WILL GET THE TYPE OF APPLICATION PROCESSED FOR AN EXISTING APP
-- =============================================
CREATE PROCEDURE [dbo].[spGetExistingAppTypeProcessed]
	
	@AppId INT
	
	
AS
BEGIN

	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT 
		RPT.repayment_plan
	FROM
		dbo.Repayment_Plan_Selected RPS
	INNER JOIN dbo.Repayment_Type RT
		ON RT.repayment_type_id = RPS.repayment_type_id
	INNER JOIN dbo.Repayment_Plan_Type RPT
		ON RPT.repayment_plan_type_id = RT.repayment_plan_type_id
	WHERE 
		RPS.application_id = @AppId
END
