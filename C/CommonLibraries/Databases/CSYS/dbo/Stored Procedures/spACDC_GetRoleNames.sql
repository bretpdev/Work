
-- =============================================
-- Author:		Bret Pehrson
-- Create date: 9/19/2012
-- Description:	Returns a list of Roles
-- =============================================
CREATE PROCEDURE [dbo].[spACDC_GetRoleNames]
	-- Add the parameters for the stored procedure here
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    SELECT RoleName FROM SYSA_LST_Role
    WHERE EndDate IS NULL
END