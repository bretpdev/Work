
CREATE PROCEDURE [dbo].[spCheckByPhoneGetPendingRecords]
AS
BEGIN	
	SET NOCOUNT ON;
	
	--open the encryption key to decrypt
    OPEN SYMMETRIC KEY USHE_Financial_Data_Key
	DECRYPTION BY CERTIFICATE USHE_Financial_Encryption_Certificate;

	SELECT

		RecNo AS ID,
		EffectiveDate,
		AccountType,
		CONVERT(CHAR(9), DECRYPTBYKEY(EncryptedRoutingNumber)) as ABA, --decrypt
		CONVERT(VARCHAR(25), DECRYPTBYKEY(EncryptedBankAccountNumber)) as BankAccountNumber, --decrypt
		CAST(ROUND(PaymentAmount * 100, 0) AS INT) AS Amount,
		AccountNumber AS AccountNumber,
		AccountHolderName AS Name,
		AccountHolderName
	FROM 
		CheckByPhone
	WHERE 
		ProcessedDate IS NULL 
		AND EffectiveDate <= GETDATE()
		AND ISNULL(Deleted,0) != 1
	ORDER BY AccountNumber
	
	--close the key when we're done
	CLOSE SYMMETRIC KEY USHE_Financial_Data_Key;
END


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[spCheckByPhoneGetPendingRecords] TO [db_executor]
    AS [dbo];




GO
GRANT EXECUTE
    ON OBJECT::[dbo].[spCheckByPhoneGetPendingRecords] TO [UHEAA\BatchScripts]
    AS [dbo];



