CREATE FUNCTION [dbo].[GetLoanRirInt]
(
	@AccountNumber char(10),
	@LoanSeq int,
	@LoanProgram nvarchar(20)
)
RETURNS decimal(9, 2)
AS
BEGIN

declare @Ssn char(9)
SELECT @Ssn = BF_SSN FROM dbo.PD10_Borrower WHERE DF_SPE_ACC_ID = @AccountNumber
	
declare @RirInt decimal (9, 2) = NULL
IF (EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'LN54_LON_BBS'))
BEGIN
	declare @PgmCode char(3)
	select @PgmCode = PM_BBS_PGM from LN54_LON_BBS where BF_SSN = @Ssn and LN_SEQ = @LoanSeq
	select @RirInt = case @PgmCode
		when 'BI1' then 0.25
		when 'BI2' then 0.25
		when 'BI3' then 0.50
		when 'BI4' then 1.00
		when 'BI5' then 1.00
		when 'BI6' then 1.00
		when 'BI7' then 1.00
		when 'BI8' then 1.50
		when 'BI9' then 1.50
		when 'BIA' then 1.75
		when 'BIB' then 2.00
		when 'BIC' then 1.75
		WHEN 'BID' THEN 2.00
		when 'BIE' then 2.00
		when 'BR1' then 1.00
		when 'GRD' then 1.00
		when 'G48' then 1.00
		when 'N48' then 2.00
		when 'O24' then 1.00
		when 'R48' then 2.00
		when 'U36' then 1.00
		when 'W24' then 1.00
		when 'WY2' THEN 2.00
		when 'WY3' THEN 2.00
		when 'U48' then case when @LoanProgram in ('PLUS', 'STFFRD', 'UNSTFD', 'PLUSGB', 'SLS') then 2.00 else 1.00 end
		else NULL
	end 

	if @RirInt IS NULL and EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'LN55_LON_BBS_TIR')
	begin
		if @PgmCode = 'BT1'
		begin
			set @RirInt = 0
			select @RirInt = @RirInt + 1 from LN55_LON_BBS_TIR where LC_STA_LN55 = 'A' and LC_LON_BBT_STA = 'Q' and BF_SSN = @Ssn and LN_SEQ = @LoanSeq AND LF_LON_BBS_TIR = '01'
		end
		else if @PgmCode = 'BT2'
		begin
			set @RirInt = 0
			select @RirInt = @RirInt + 1 from LN55_LON_BBS_TIR where LC_STA_LN55 = 'A' and LC_LON_BBT_STA = 'Q' and BF_SSN = @Ssn and LN_SEQ = @LoanSeq AND LF_LON_BBS_TIR = '01'
		end
		else if @PgmCode = 'BT3'
		begin
			set @RirInt = 0
			select @RirInt = @RirInt + 0.375 from LN55_LON_BBS_TIR where LC_STA_LN55 = 'A' and LC_LON_BBT_STA = 'Q' and BF_SSN = @Ssn and LN_SEQ = @LoanSeq and LF_LON_BBS_TIR = '01'
		end
	end
END

	RETURN @RirInt
END
