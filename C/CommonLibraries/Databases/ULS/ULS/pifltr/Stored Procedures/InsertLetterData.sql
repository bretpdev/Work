CREATE PROCEDURE pifltr.InsertLetterData
	@PifLetterId int,
	@FirstName varchar(100),
	@LastName varchar(100),
	@Address1 varchar(100),
	@Address2 varchar(100),
	@City varchar(50),
	@State varchar(15),
	@Zip char(5),
	@Country varchar(100),
	@EffectiveDate varchar(10),
	@ConsolPif bit,
	@CostCenterCode varchar(10),
	@GuarDate varchar(10),
	@GuarAmt varchar(20),
	@LoanPgm varchar(100),
	@LoanSeq int
AS
BEGIN

	INSERT INTO pifltr.PifLetterData(PifLetterId, FirstName, LastName, Address1, Address2, City, [State], Zip, Country, EffectiveDate, ConsolPif, CostCenterCode, GuarDate, GuarAmt, LoanPgm, LoanSeq)
	VALUES(@PifLetterId, @FirstName, @LastName, @Address1, @Address2, @City, @State, @Zip, @Country, @EffectiveDate, @ConsolPif, @CostCenterCode, @GuarDate, @GuarAmt, @LoanPgm, @LoanSeq)

END
GO
GRANT EXECUTE
    ON OBJECT::[pifltr].[InsertLetterData] TO [db_executor]
    AS [dbo];

