CREATE PROCEDURE [dbo].[GetIVRResponseCode]
	@AccountNumber VARCHAR(10)
AS
	SELECT
		ResponseCode
	FROM
		IVRDemographicResponses DR
		INNER JOIN
		(
			SELECT
				MAX(CallDate) AS MaxCall
			FROM
				IVRDemographicResponses DR
			WHERE
				DR.AccountNumber = @AccountNumber
		) MaxResponse
			ON DR.CallDate = MaxResponse.MaxCall
		INNER JOIN ResponseTypes RT
			ON DR.ResponseTypeId = RT.ResponseTypeId
	WHERE
		DR.AccountNumber = @AccountNumber
RETURN 0
