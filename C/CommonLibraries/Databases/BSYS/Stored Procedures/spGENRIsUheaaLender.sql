

CREATE PROCEDURE [dbo].[spGENRIsUheaaLender]
	@LenderId VARCHAR(15)
AS
	Select COUNT(*) FROM GENR_REF_LenderAffiliation WHERE LenderID = @LenderId AND Affiliation = 'UHEAA'
RETURN 0
GO


