CREATE PROCEDURE [dbo].[DeleteFormFromLetter]
	@LetterId VARCHAR(10)
AS
	DELETE FROM	
		LetterFormMapping
	WHERE
		LetterId = @LetterId
RETURN 0
