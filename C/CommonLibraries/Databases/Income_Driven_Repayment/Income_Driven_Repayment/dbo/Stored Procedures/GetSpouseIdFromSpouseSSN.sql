CREATE PROCEDURE [dbo].[GetSpouseIdFromSpouseSSN]
	@SSN CHAR(9)
AS
	
	SELECT
		spouse_id AS SpouseId 
	FROM
		[dbo].Spouses
	WHERE
		SSN = @SSN
RETURN 0