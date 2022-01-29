CREATE PROCEDURE [payhistlpp].[GetAccounts]
	@UserAccessId INT,
	@RunId INT
AS
	SELECT DISTINCT
		A.AccountsId,
		A.Ssn,
		A.Lender AS [Lender],
		RTRIM(PD10.DM_PRS_1) AS [FirstName],
		RTRIM(PD10.DM_PRS_MID) AS [MiddleInitial],
		RTRIM(CONCAT(RTRIM(PD10.DM_PRS_LST),' ',RTRIM(PD10.DM_PRS_LST_SFX))) AS [LastName]
	FROM
		ULS.payhistlpp.Accounts A
		INNER JOIN ULS.payhistlpp.Run R
			ON R.RunId = A.RunId
		INNER JOIN UDW..PD10_PRS_NME PD10
			ON PD10.DF_PRS_ID = A.Ssn
	WHERE
		A.ProcessedAt IS NULL
		AND A.DeletedAt IS NULL
		AND A.UserAccessId = @UserAccessId
		AND A.RunId = @RunId
		AND R.DeletedAt IS NULL