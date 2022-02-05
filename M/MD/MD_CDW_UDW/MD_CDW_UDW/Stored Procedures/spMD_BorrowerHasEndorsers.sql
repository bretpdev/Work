CREATE PROCEDURE [dbo].[spMD_BorrowerHasEndorsers]
	@BorrowerAccountNumber char(10)
AS
	if (exists(select * from LN20_Endorser e where e.DF_SPE_ACC_ID = @BorrowerAccountNumber)) 
		select cast(1 as bit)
	else 
		select cast(0 as bit)
	
RETURN 0
