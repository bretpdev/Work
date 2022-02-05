CREATE PROCEDURE [scra].[Get0MData]
	@BorrowerId int
AS
	SELECT
		CASE
			WHEN AD.BenefitSourceId = 2 OR AD.BenefitSourceId = 3 THEN BOR.EndorserAccountNumber
		END AS EndAccountNumber,
		AD.BeginDate,
		AD.EndDate,
		AD.IsReservist,
		AD.BenefitSourceId,
		AD.NotificationDate
	FROM
		scra.ActiveDuty AD
		JOIN scra.Borrowers BOR
			ON AD.BorrowerId = BOR.BorrowerId
	WHERE
		AD.BorrowerId = @BorrowerId

RETURN 0
