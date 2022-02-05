
CREATE PROCEDURE [dbo].[spBillingText]
	@FileName		CHAR(3)
AS
BEGIN

	SELECT * FROM BillingStatements WHERE FileName = @FileName

END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[spBillingText] TO [db_executor]
    AS [dbo];



