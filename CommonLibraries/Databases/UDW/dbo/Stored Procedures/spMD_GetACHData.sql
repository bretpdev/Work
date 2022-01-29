CREATE PROCEDURE [dbo].[spMD_GetACHData]
	@AccountNumber				VARCHAR(10)
AS
BEGIN
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	--get all ACH information
	SELECT 
		dbo.Decryptor(BF_EFT_ABA) AS RoutingNumber,
		dbo.Decryptor(BF_EFT_ACC) AS AccountNumber,
		BC_EFT_STA AS DirectDebit,
		BA_EFT_ADD_WDR AS AdditionalWithdrawalAmount,
		cast(BN_EFT_NSF_CTR as smallint) AS NSFCounter,
		BC_EFT_DNL_REA AS DenialReason,
		CONVERT(VARCHAR, BD_EFT_STA, 101)  AS StatusDate,
		cast(BN_EFT_SEQ as int) AS SequenceNumber
	FROM 
		BR30_BR_EFT BR30
		left JOIN PD10_PRS_NME PD10
			ON PD10.DF_PRS_ID = BR30.BF_SSN
	WHERE 
		DF_SPE_ACC_ID = @AccountNumber
		AND BR30.BC_EFT_STA = 'A'
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[spMD_GetACHData] TO [UHEAA\Imaging Users]
    AS [dbo];

