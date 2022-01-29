USE [CLS]
GO
/****** Object:  StoredProcedure [dbo].[spAddCheckByPhoneEntry]    Script Date: 2/14/2017 12:13:01 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER PROCEDURE [dbo].[spAddCheckByPhoneEntry]
	@AccountNumber				nchar(10),
	@RoutingNumber				char(9),
	@BankAccountNumber			varchar(25),
	@AccountType				nchar(1),
	@PaymentAmount				decimal(9,2),
	@AccountHolderName			varchar(50),
	@EffectiveDate				date,
	@AcctStoreAuthorization		bit = 0,
	@DataSource					VARCHAR(10)

	/*NOTE: "IVR" is the default value for DataSource in the table, 
	as we are unable to modify the IVR insert at this time.  
	We are overriding that with our scriptId*/
AS
BEGIN	
	SET NOCOUNT ON;

    INSERT INTO CheckByPhone (AccountNumber,RoutingNumber,BankAccountNumber,AccountType,PaymentAmount,
		AccountHolderName,EffectiveDate,AcctStoreAuthorization, DataSource)
    VALUES (@AccountNumber,@RoutingNumber,@BankAccountNumber,@AccountType,@PaymentAmount,
		@AccountHolderName,@EffectiveDate,@AcctStoreAuthorization,@DataSource)
END


