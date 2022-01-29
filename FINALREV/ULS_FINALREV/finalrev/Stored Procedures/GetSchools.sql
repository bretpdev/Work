CREATE PROCEDURE [finalrev].[GetSchools]
	@BorrowerRecordId INT
AS
	SELECT
		S.SchoolCode
	FROM
		finalrev.Schools S
		LEFT JOIN finalrev.BorrowerSchools BS
			ON BS.SchoolsId = S.SchoolsId
		LEFT JOIN finalrev.BorrowerRecord B
			ON B.BorrowerRecordID = BS.BorrowerRecordId
	WHERE
		B.BorrowerRecordID = @BorrowerRecordId
RETURN 0