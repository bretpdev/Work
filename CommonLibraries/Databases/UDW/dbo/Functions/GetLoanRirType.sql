CREATE FUNCTION [dbo].[GetLoanRirType]
(
	@AccountNumber char(10),
	@LoanSeq int
)
RETURNS nvarchar(max)
AS
BEGIN

declare @Ssn char(9)
SELECT @Ssn = BF_SSN FROM dbo.PD10_Borrower WHERE DF_SPE_ACC_ID = @AccountNumber
	
declare @RirType nvarchar(max)
IF (EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'LN54_LON_BBS'))
BEGIN
	declare @PgmCode char(3)
	select @PgmCode = PM_BBS_PGM from LN54_LON_BBS where BF_SSN = @Ssn and LN_SEQ = @LoanSeq
	select @RirType = case @PgmCode
		when 'BI1' then 'Interest Rate'
		when 'BI2' then 'Interest Rate'
		when 'BI4' then 'Interest Rate'
		when 'BI5' then 'Interest Rate'
		when 'BI6' then 'Interest Rate'
		when 'BI7' then 'Interest Rate'
		when 'BI8' then 'Interest Rate'
		when 'BI9' then 'Interest Rate'
		when 'BIA' then 'Interest Rate'
		when 'BIB' then 'Interest Rate'
		when 'BIC' then 'Interest Rate'
		when 'BID' then 'Interest Rate'
		when 'BIE' then 'Interest Rate'
		when 'BR1' then 'Rebate'
		when 'BT1' then 'Rebate'
		when 'BT2' then 'Rebate'
		when 'BT3' then 'Interest Rate'
		when 'GRD' then 'Interest Rate'
		when 'G48' then 'Interest Rate'
		when 'N48' then 'Interest Rate'
		when 'O24' then 'Interest Rate'
		when 'R48' then 'Rebate'
		when 'U36' then 'Interest Rate'
		when 'U48' then 'Interest Rate'
		when 'W24' then 'Interest Rate'
		when 'WY2' then 'Interest Rate'
		when 'WY3' then 'Interest Rate'
		else NULL
	end 
END

	RETURN @RirType
END