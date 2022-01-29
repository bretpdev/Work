USE [NORAD]
GO
/****** Object:  StoredProcedure [dbo].[spCKPH_AddCheckByPhoneEntry]    Script Date: 10/11/2016 1:36:05 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[spCKPH_AddCheckByPhoneEntry]

@SSN					VARCHAR(9),
@NAME					VARCHAR(200),
@DOB					VARCHAR(10),
@ABA					VARCHAR(50),
@BANKACCOUNTNUMBER		VARCHAR(200),
@ACCOUNTTYPE			VARCHAR(50),
@AMOUNT					VARCHAR(50),
@EFFECTIVEDATE			VARCHAR(50),
@ACCOUNTHOLDERNAME		VARCHAR(200),
@DataSource				VARCHAR(10)

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	INSERT INTO dbo.CKPH_DAT_OPSCheckByPhone 
	(SSN, Name, DOB, ABA, BankAccountNumber, AccountType, Amount, EffectiveDate, AccountHolderName, DataSource) 
	VALUES 
	(@SSN, @NAME, @DOB, @ABA, @BANKACCOUNTNUMBER, @ACCOUNTTYPE, @AMOUNT, @EFFECTIVEDATE, @ACCOUNTHOLDERNAME, @DataSource)											

END
