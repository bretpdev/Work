CREATE PROCEDURE [clschllnfd].[GetSchoolName]
	@SchoolCode VARCHAR(8)
AS
	SELECT
		IM_SCL_FUL [SchoolName]
	FROM
		SC10_SCH_DMO SC10
	WHERE
		SC10.IF_DOE_SCL = @SchoolCode
RETURN 0