CREATE PROCEDURE [dbo].[GetPovertyIncome]
	@FamilySize int
AS
	DECLARE @MAX_FAMILY INT = (SELECT MAX(FamilySize) FROM PovertyGuidelines)
	IF @FamilySize > @MAX_FAMILY
		BEGIN
			DECLARE @DIFFERENCE INT  = @FamilySize - @MAX_FAMILY
			SELECT
				(PG.Income + (@DIFFERENCE * 6240)) AS Income
			FROM	
				PovertyGuidelines PG
			WHERE
				FamilySize = @MAX_FAMILY
		END
	ELSE
		BEGIN
			SELECT
				PG.Income
			FROM
				PovertyGuidelines PG
			WHERE 
				FamilySize = @FamilySize
		END
RETURN 0
