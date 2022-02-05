CREATE PROCEDURE [dbo].[GetAllBorrowersByLenderCode]
	@LenderCode varchar(6)
AS
	SELECT
		B.[br_ssn]
	FROM
		BorrowerLenderCodeMapping B
	WHERE
		lender_code = @LenderCode 
	
RETURN 0
