CREATE PROCEDURE [dbo].[LoanDetail_Forb]
	@LetterId varchar(10),
	@BF_SSN  CHAR(9)
AS


SELECT
	'Add' AS LetterAction,
	'Forb' AS LetterTypeCode,
	'05/27/2017' AS BeginDate,
	'05/27/2017' AS EndDate,
	1 as LN_SEQ,
	'Stafford' AS LoanProgram
FROM
	UDW..LN10_LON LN10
	
WHERE
	LN10.BF_SSN = @BF_SSN
ORDER BY 
	LN10.LN_SEQ,
	BeginDate