--RS05, RS10, and LN65 for account 08 7275 5820.

USE UDW
GO

DECLARE @BF_SSN VARCHAR(9) = (SELECT DF_PRS_ID FROM PD10_PRS_NME WHERE DF_SPE_ACC_ID = '0872755820')

SELECT * FROM RS05_IBR_RPS	WHERE BF_SSN = @BF_SSN
SELECT * FROM RS10_BR_RPD	WHERE BF_SSN = @BF_SSN
SELECT * FROM LN65_LON_RPS	WHERE BF_SSN = @BF_SSN