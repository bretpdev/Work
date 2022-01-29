
CREATE PROCEDURE [dbo].[spMD_GetACHData]
	@AccountNumber				VARCHAR(10)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	--get all ACH information
	SELECT BF_EFT_ABA as RoutingNumber,
		BF_EFT_ACC as AccountNumber,
		BC_EFT_STA as DirectDebit,
		BA_EFT_ADD_WDR as AdditionalWithdrawalAmount,
		BN_EFT_NSF_CTR as NSFCounter,
		BC_EFT_DNL_REA as DenialReason,
		BD_EFT_STA  as StatusDate,
		BN_EFT_SEQ as SequenceNumber
	FROM dbo.BR30_Autopay BR30
	WHERE DF_SPE_ACC_ID = @AccountNumber
END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[spMD_GetACHData] TO [UHEAA\Imaging Users]
    AS [dbo];

