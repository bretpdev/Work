
CREATE FUNCTION dbo.CountBorrowers() RETURNS INT AS
BEGIN 
	DECLARE @NumberOfBorrowers AS INT
	SET @NumberOfBorrowers = 0;
	
	SELECT @NumberOfBorrowers = COUNT(DISTINCT BF_SSN)
	FROM PD10_Borrower
	
	 RETURN @NumberOfBorrowers;
	
END