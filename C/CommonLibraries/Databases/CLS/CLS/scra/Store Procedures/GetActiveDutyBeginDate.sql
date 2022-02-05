CREATE PROCEDURE [scra].[GetActiveDutyBeginDate]
	@BorrowerId int
AS
	SELECT
		BeginDate
	FROM
		scra.ActiveDuty AD
	WHERE
		BorrowerId = @BorrowerId
		AND CreatedAt = (SELECT MAX(CreatedAt) FROM scra.ActiveDuty WHERE BorrowerId = @BorrowerId)
RETURN 0
GO
GRANT EXECUTE
    ON OBJECT::[scra].[GetActiveDutyBeginDate] TO [db_executor]
    AS [dbo];

