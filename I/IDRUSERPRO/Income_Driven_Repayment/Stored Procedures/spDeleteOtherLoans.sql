﻿-- =============================================
-- Author:		JAROM RYAN
-- Create date: 06/27/2013
-- Description:	WILL DELETE ALL OTHER LOANS FOR A GIVEN APP ID
-- =============================================
CREATE PROCEDURE [dbo].[spDeleteOtherLoans] 

@AppId INT

AS
BEGIN

	DELETE
		dbo.Other_Loans
	WHERE
		application_id = @AppId
END