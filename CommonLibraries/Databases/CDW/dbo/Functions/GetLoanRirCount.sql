
CREATE  FUNCTION [dbo].[GetLoanRirCount]
(
	@AccountNumber char(10),
	@LoanSeq int
)
RETURNS decimal(9, 2)
AS
BEGIN

declare @Ssn char(9)
SELECT @Ssn = BF_SSN FROM dbo.PD10_Borrower WHERE DF_SPE_ACC_ID = @AccountNumber

declare @RirType nvarchar(max)
IF (EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'LN54_LON_BBS'))
BEGIN
	declare @PgmCode char(3)
	select @PgmCode = PM_BBS_PGM from LN54_LON_BBS where BF_SSN = @Ssn and LN_SEQ = @LoanSeq
	if @PgmCode in ('N48','O42','R48','U36', 'U48', 'W24')
	begin
		select 
			@RirType = cast(isnull(LN_LON_BBT_PAY, 0) as nvarchar(max)) + '/' +  isnull(rp02.PN_BBT_PAY_ICV, 0)
		from 
			LN55_LON_BBS_TIR  ln55
		join
			RP02_BBS_PGM_TIR rp02 on rp02.PM_BBS_PGM = ln55.PM_BBS_PGM
		where BF_SSN = @Ssn and LN_SEQ = @LoanSeq
	end
	else if @PgmCode in ('BI1', 'BI2', 'BI4','BI5', 'BI6','BI7','BI8', 'BI9', 'BIA', 'BIB', 'BIC', 'BIE', 'BR1', 'BT1', 'BT2', 'BT3')
	begin
		select 
			@RirType = cast(isnull(LN_BBS_STS_PCV_PAY, 0) as nvarchar(max)) + '/' +  isnull(rp02.PN_BBT_PAY_ICV, 0)
		from 
			LN54_LON_BBS  ln54
		join
			RP02_BBS_PGM_TIR rp02 on rp02.PM_BBS_PGM = ln54.PM_BBS_PGM
		where BF_SSN = @Ssn and LN_SEQ = @LoanSeq
	end 
END

	RETURN @RirType
END