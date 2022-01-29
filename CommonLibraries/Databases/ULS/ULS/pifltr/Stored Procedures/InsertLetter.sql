CREATE PROCEDURE pifltr.InsertLetter
	@AccountNumber char(10),
	@UniqueId varchar(20)
AS
BEGIN

	INSERT INTO pifltr.PifLetter(AccountNumber, UniqueId)
	VALUES(@AccountNumber, @UniqueId)

END
GO
GRANT EXECUTE
    ON OBJECT::[pifltr].[InsertLetter] TO [db_executor]
    AS [dbo];

