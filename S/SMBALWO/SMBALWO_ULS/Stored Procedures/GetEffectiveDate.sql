CREATE PROCEDURE [smbalwo].[GetEffectiveDate]
	@AccountNumber VARCHAR(10),
	@LoanWriteOffId INT,
	@LN_SEQ INT
AS

	DECLARE @IsTilp BIT = (SELECT IsTilp FROM smbalwo.LoanWriteOffs WHERE LoanWriteOffId = @LoanWriteOffId)

	UPDATE
		smbalwo.LoanWriteOffs
	SET
		EffectiveDate = 
		(
			CASE
				WHEN @IsTilp = 1 THEN 
				(
					SELECT
						DATEADD(DAY, 1, MAX(LN90.LD_FAT_EFF))
					FROM
						UDW..LN90_FIN_ATY LN90
						INNER JOIN UDW..PD10_PRS_NME PD10
							ON LN90.BF_SSN = PD10.DF_PRS_ID
						INNER JOIN UDW..LN10_LON LN10
							ON LN90.BF_SSN = LN10.BF_SSN
							AND LN90.LN_SEQ = LN10.LN_SEQ
					WHERE
						LN90.LC_STA_LON90 = 'A'
						AND ISNULL(LN90.LC_FAT_REV_REA, '') = ''
						AND PD10.DF_SPE_ACC_ID = @AccountNumber
						AND LN10.IC_LON_PGM = 'TILP'
						AND LN90.LN_SEQ = @LN_SEQ
				)
				ELSE
				(
					SELECT
						DATEADD(DAY, 1, MAX(LN90.LD_FAT_EFF))
					FROM
						UDW..LN90_FIN_ATY LN90
						INNER JOIN UDW..PD10_PRS_NME PD10
							ON LN90.BF_SSN = PD10.DF_PRS_ID
					WHERE
						LN90.LC_STA_LON90 = 'A'
						AND ISNULL(LN90.LC_FAT_REV_REA, '') = ''
						AND PD10.DF_SPE_ACC_ID = @AccountNumber
						AND LN90.LN_SEQ = @LN_SEQ
				)
			END
		)
	WHERE
		LoanWriteOffId = @LoanWriteOffId

	SELECT
		EffectiveDate
	FROM
		smbalwo.LoanWriteOffs
	WHERE
		LoanWriteOffId = @LoanWriteOffId
RETURN 0