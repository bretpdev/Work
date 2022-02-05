CREATE TRIGGER [dbo].[EncryptInformation] ON [dbo].[CKPH_DAT_OPSCheckByPhone]
INSTEAD OF INSERT
AS 
BEGIN

--first open the key
	OPEN SYMMETRIC KEY USHE_Financial_Data_Key DECRYPTION BY CERTIFICATE USHE_Financial_Encryption_Certificate;

--see if we are inserting already encrypted information
--declare variables
	DECLARE @BankAcctNum varchar(25) = (SELECT [BankAccountNumber] FROM INSERTED)
	DECLARE @RoutingNum char(9) = (SELECT [ABA] FROM INSERTED)
	DECLARE @enBankAcctNum varbinary(128) = CONVERT(varbinary(128), (SELECT [EncryptedBankAccountNumber] FROM INSERTED), 1)
	DECLARE @enRoutingNum varbinary(128) = CONVERT(varbinary(128), (SELECT [EncryptedRoutingNumber] FROM INSERTED), 1)

IF @enBankAcctNum IS NULL AND @enRoutingNum IS NULL
BEGIN
	SET @enBankAcctNum = ENCRYPTBYKEY(Key_GUID('USHE_Financial_Data_Key'), @BankAcctNum)
	SET @enRoutingNum = ENCRYPTBYKEY(Key_GUID('USHE_Financial_Data_Key'), @RoutingNum)
END
	SET @BankAcctNum = ''
	SET @RoutingNum = ''
	
	INSERT INTO [CKPH_DAT_OPSCheckByPhone] (SSN,Name,DOB,ABA,BankAccountNumber,AccountType,Amount,EffectiveDate,AccountHolderName,ProcessedDate,EncryptedBankAccountNumber,EncryptedRoutingNumber)
	SELECT
		SSN,
		Name,
		DOB,
		@RoutingNum,
		@BankAcctNum,
		AccountType,
		Amount,
		EffectiveDate,
		AccountHolderName,
		ProcessedDate,
		@enBankAcctNum,
		@enRoutingNum
	FROM
		INSERTED

	CLOSE SYMMETRIC KEY USHE_Financial_Data_Key
END
