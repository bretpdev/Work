CREATE PROCEDURE [dbo].[LoanTypesGetByKey]
	@TypeKey varchar(max)
AS
	SELECT LoanType FROM GENR_REF_LoanTypes WHERE TypeKey = @TypeKey
RETURN 0
