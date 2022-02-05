CREATE PROCEDURE [rtnemlinvf].[GetSsnFromEmail]
	@Email VARCHAR(56)
AS
	SELECT
		DF_PRS_ID [Ssn],
		'O' [EmailType]
	FROM
		PD03_PRS_ADR_PHN
	WHERE
		DX_EML_ADR = @Email
		AND DI_EML_ADR_VAL = 'Y'
RETURN 0