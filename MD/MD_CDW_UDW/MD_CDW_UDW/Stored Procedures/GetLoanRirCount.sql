CREATE FUNCTION [dbo].[GetLoanRirCount]
(
	@AccountNumber char(10),
	@LoanSeq int
)
RETURNS NVARCHAR(MAX)
AS
BEGIN

declare @Ssn char(9)
SELECT @Ssn = BF_SSN FROM dbo.PD10_Borrower WHERE DF_SPE_ACC_ID = @AccountNumber

declare @Result nvarchar(MAX)

SELECT 
	@Result =
	CAST(ISNULL(LN_LON_BBT_PAY, 0) as VARCHAR(MAX)) 
	+ '/' + 
	CASE PM_BBS_PGM
		WHEN 'G48' THEN '48'
		WHEN 'N48' THEN '48'
		WHEN '024' THEN '24'
		WHEN 'R48' THEN '48'
		WHEN 'U36' THEN '36'
		WHEN 'U48' THEN '48'
		WHEN 'W24' THEN '24'
		WHEN 'BI5' THEN '36'
		WHEN 'BI8' THEN '12'
		WHEN 'BI9' THEN '36'
		WHEN 'BIA' THEN '36'
		WHEN 'BIB' THEN '36'
		WHEN 'BIE' THEN '48'
		WHEN 'BR1' THEN '1'
		WHEN 'BT1' THEN '1'
		WHEN 'BT2' THEN '12'
		WHEN 'BT3' THEN '6'
		ELSE '0'
	END
FROM 
	[LN55_LON_BBS_TIR]
WHERE 
	LF_LON_BBS_TIR = '01'
	AND
	BF_SSN = @Ssn
	AND
	LN_SEQ = @LoanSeq

RETURN @Result


END
