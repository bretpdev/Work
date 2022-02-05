
CREATE Procedure BorrowerSelectByAccountIdentifier
	(
		@AccountIdentifier nvarchar(max)
	)
AS

declare @AccountNumber char(10)
if (len(@AccountIdentifier) = 9) select @AccountNumber = DF_SPE_ACC_ID from dbo.PD10_Borrower where BF_SSN = @AccountIdentifier
else set @AccountNumber = @AccountIdentifier

select borr.BF_SSN as SSN, 
       borr.DF_SPE_ACC_ID as AccountNumber, 
       borr.DM_PRS_1 as FirstName, 
       borr.DM_PRS_LST as LastName, 
       borr.DM_PRS_MID as MiddleInitial,  
       borr.DD_BRT as DOB
  from dbo.PD10_Borrower borr
 where borr.DF_SPE_ACC_ID = @AccountNumber
 
 exec BorrowerSelect_Demographics @AccountNumber
 exec BorrowerSelect_Info @AccountNumber