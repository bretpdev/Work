-- =============================================
-- Author:		Jarom Ryan
-- Create date: 02/28/2012
-- Description:	This sp will Affiliation based upon the LenderId sent in
-- =============================================
CREATE PROCEDURE [dbo].[spGENRGetLenderAffiliation]
	-- Add the parameters for the stored procedure here
	@LenderId	Varchar
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT Affiliation
	FROM GENR_REF_LenderAffiliation
	WHERE LenderID = @LenderID
END