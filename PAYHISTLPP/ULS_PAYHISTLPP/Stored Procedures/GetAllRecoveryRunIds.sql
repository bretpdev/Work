CREATE PROCEDURE [payhistlpp].[GetAllRecoveryRunIds]
	@UserAccessId INT,
	@Lender VARCHAR(6)
AS
	SELECT DISTINCT
		R.RunId,
		R.FileDirectory,
		R.Tilp
	FROM
		ULS.payhistlpp.Run R
		INNER JOIN ULS.payhistlpp.Accounts A
			ON A.RunId = R.RunId
	WHERE
		R.UserAccessId = @UserAccessId
		AND A.Lender = @Lender
		AND R.CompletedAt IS NULL
		And R.DeletedAt IS NULL