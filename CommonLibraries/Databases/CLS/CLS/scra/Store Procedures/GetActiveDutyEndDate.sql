
CREATE PROCEDURE [scra].[GetActiveDutyEndDate]
	@BorrowerId int
AS
	SELECT
		EndDate
	FROM
		scra.ActiveDuty
	WHERE
		BorrowerId = @BorrowerId
		AND CreatedAt = (SELECT MAX(CreatedAt) FROM scra.ActiveDuty WHERE BorrowerId = @BorrowerId)
RETURN 0
GO
GRANT EXECUTE
    ON OBJECT::[scra].[GetActiveDutyEndDate] TO [db_executor]
    AS [dbo];

