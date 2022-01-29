CREATE PROCEDURE [scra].[GetActiveDutyBeginDate]
	@BorrowerId int
AS
	SELECT
		AD.BeginDate
	FROM
		scra.ActiveDuty AD
		INNER JOIN
		(
			SELECT
				MAX(CreatedAt) AS CreatedAt,
				BorrowerId
			FROM
				scra.ActiveDuty
			WHERE
				BorrowerId = @BorrowerId
			GROUP BY
				BorrowerId
		) MaxCreate
			ON MaxCreate.BorrowerId = AD.BorrowerId
			AND MaxCreate.CreatedAt = AD.CreatedAt
	WHERE
		AD.BorrowerId = @BorrowerId