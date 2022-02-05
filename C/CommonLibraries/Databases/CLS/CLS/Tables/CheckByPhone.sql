CREATE TABLE [dbo].[CheckByPhone] (
    [RecNo]                      INT             IDENTITY (1, 1) NOT NULL,
    [AccountNumber]              NCHAR (10)      NOT NULL,
    [RoutingNumber]              CHAR (9)        NULL,
    [BankAccountNumber]          VARCHAR (25)    NULL,
    [AccountType]                NCHAR (1)       NOT NULL,
    [PaymentAmount]              DECIMAL (9, 2)  NOT NULL,
    [AccountHolderName]          VARCHAR (50)    NOT NULL,
    [EffectiveDate]              DATE            NOT NULL,
    [ProcessedDate]              DATE            NULL,
    [EncryptedRoutingNumber]     VARBINARY (128) NULL,
    [EncryptedBankAccountNumber] VARBINARY (128) NULL,
    [BankAccountLast4]           VARCHAR (4)     NULL,
    [UserAuthorization]          BIT             NULL,
    [AcctStoreAuthorization]     BIT             NULL,
    [TTSDate]                    VARCHAR (10)    NULL,
    [DataSource]                 VARCHAR (10)    CONSTRAINT [DF_CheckByPhone_DataSource] DEFAULT ('IVR') NULL,
    [FileName]                   VARCHAR (200)   NULL,
    [Deleted]                    BIT             CONSTRAINT [DF_CheckByPhone_Deleted] DEFAULT ((0)) NOT NULL,
    [DeletedAt]                  DATETIME        NULL,
    [DeletedBy]                  VARCHAR (50)    NULL,
    [CreatedAt]                  DATETIME        CONSTRAINT [DF_CheckByPhone_CreatedAt] DEFAULT (getdate()) NULL,
    [CreatedBy]                  VARCHAR (50)    CONSTRAINT [DF_CheckByPhone_CreatedBy] DEFAULT (suser_name()) NULL,
    CONSTRAINT [PK_CheckByPhone] PRIMARY KEY CLUSTERED ([RecNo] ASC) WITH (FILLFACTOR = 95)
);






GO

CREATE TRIGGER [dbo].[EncryptInformation] ON [dbo].[CheckByPhone]
INSTEAD OF INSERT
AS
BEGIN

--first open the key
OPEN SYMMETRIC KEY USHE_Financial_Data_Key DECRYPTION BY CERTIFICATE USHE_Financial_Encryption_Certificate;

--see if we are inserting already encrypted information
--declare variables
DECLARE @enBankAcctNum varbinary(128) = CONVERT(varbinary(128), (SELECT [EncryptedBankAccountNumber] FROM INSERTED), 1)
DECLARE @enRoutingNum varbinary(128) = CONVERT(varbinary(128), (SELECT [EncryptedRoutingNumber] FROM INSERTED), 1)
DECLARE @BankAcctNum varchar(25) = (SELECT [BankAccountNumber] FROM INSERTED)
DECLARE @RoutingNum char(9) = (SELECT [RoutingNumber] FROM INSERTED)
DECLARE @Last4 varchar(4) = (SELECT [BankAccountLast4] FROM INSERTED)
DECLARE @Auth bit = (SELECT [AcctStoreAuthorization] FROM INSERTED)
DECLARE @SpanishDate varchar(10) = (SELECT SUBSTRING(CONVERT(varchar(10), [EffectiveDate], 1), 4, 2) + '-' + SUBSTRING(CONVERT(varchar(10), [EffectiveDate], 1), 1, 2) + '-20' + SUBSTRING(CONVERT(varchar(10), [EffectiveDate], 1), 7, 2) FROM INSERTED)

--we are doing a "standard" insert. passing in clear text acct info
IF @enBankAcctNum IS NULL AND @enRoutingNum IS NULL
BEGIN
	SET @enBankAcctNum = ENCRYPTBYKEY(Key_GUID('USHE_Financial_Data_Key'), @BankAcctNum)
	SET @enRoutingNum = ENCRYPTBYKEY(Key_GUID('USHE_Financial_Data_Key'), @RoutingNum)
	IF @Last4 IS NULL
	SET @Last4 = CASE @Auth WHEN 1 THEN
			RIGHT(@BankAcctNum, 4)
		ELSE NULL
		END
END
ELSE --we are re-using previously encrypted info
BEGIN
	IF @Last4 IS NULL AND @Auth = 1 --check last 4 on the bank account
	BEGIN
		--by getting here, we don't have the last 4 digits, we want to save it, and we are re-using 
		--an encrypted account, so decrypt
		SET @BankAcctNum = CONVERT(VARCHAR(25), DECRYPTBYKEY(@enBankAcctNum))
		SET @Last4 = RIGHT(@BankAcctNum, 4)
	END
END

--clear out the unecrypted information
SET @BankAcctNum = NULL
SET @RoutingNum = NULL

--do the actual insert
INSERT INTO CheckByPhone(
	[AccountNumber] ,[AccountType] ,[PaymentAmount] ,[AccountHolderName] ,[EffectiveDate] ,[ProcessedDate]
	,[EncryptedRoutingNumber] ,[EncryptedBankAccountNumber]
	,[BankAccountLast4]
	,[BankAccountNumber], [RoutingNumber]
	,[UserAuthorization], [AcctStoreAuthorization], [TTSDate], DataSource)
SELECT AccountNumber
	, AccountType
	, PaymentAmount
	, AccountHolderName
	, EffectiveDate
	, ProcessedDate
	, @enRoutingNum
	, @enBankAcctNum
	, @Last4
	, @BankAcctNum
	, @RoutingNum
	, 1
	, AcctStoreAuthorization
	, @SpanishDate
	,DataSource
	FROM INSERTED

--close the key
CLOSE SYMMETRIC KEY USHE_Financial_Data_Key;
END

