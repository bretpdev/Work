-- =============================================
-- Author:		Jarom Ryan
-- Create date: 03/06/2012
-- Description:	This sp will insert bwr loan level data into CompassPifLoanLevel Table
-- =============================================
CREATE PROCEDURE [dbo].[spCompassPifInsertLoanLevelData]

	@AccountNumber  VARCHAR(10),
	@UniqueId		VARCHAR(50),
	@GuarDate		Date,
	@GuarAmt		VARCHAR(20),
	@LoanPgm		VARCHAR(100),
	@LoanSeq		VARCHAR(6)
	
AS
BEGIN

	SET NOCOUNT ON;
	
	Insert Into CompassPifLoanLevel (AccountNumber,UniqueId,GuarDate,GuarAmt,LoanPgm, LoanSeq)
	Values (@AccountNumber, @UniqueId, @GuarDate, @GuarAmt, @LoanPgm, @LoanSeq)
END
