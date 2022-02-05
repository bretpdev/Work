-- =============================================
-- Author:		Jarom Ryan
-- Create date: 12/27/2012
-- Description:	This sp will get the Letter Choices for a given Letter Option
-- =============================================
CREATE PROCEDURE [dbo].[spLSLGetLetterChoices] 
	
	@LetterType As Varchar(300),
	@LetterOption As Varchar (300)
	
AS
BEGIN

	SET NOCOUNT ON;

    Select Distinct LetterChoices
    From dbo.LoanServicingLetters
    Where LetterOptions = @LetterOption And LetterType = @LetterType
END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[spLSLGetLetterChoices] TO [db_executor]
    AS [dbo];



