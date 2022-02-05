CREATE PROCEDURE [dbo].[InsertFormInformation]
	@LetterId varchar(10),
	@Path varchar(260),
	@Form varchar(255)
AS

IF NOT EXISTS(SELECT LetterId from LetterFormMapping where LetterId = @LetterId)
	BEGIN
		INSERT INTO LetterFormMapping(LetterId, FormPath, Form)
		VALUES(@LetterId, @Path, @Form)
	END
ELSE
	BEGIN
		UPDATE
			LetterFormMapping
		SET
			Form = @Form,
			FormPath = @Path
		WHERE
			LetterId = @LetterId
	END

RETURN 0
