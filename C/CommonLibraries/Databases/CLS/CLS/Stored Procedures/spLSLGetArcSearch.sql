-- =============================================
-- Author:		Jarom Ryan
-- Create date: 01/28/2013
-- Description:	Will return the ArcSearch for a given letter type, option, and choice.
-- =============================================
CREATE PROCEDURE [dbo].[spLSLGetArcSearch]

	@LetterType As Varchar(300),
	@LetterOption As Varchar(300),
	@LetterChoice As Varchar(800)


AS
BEGIN

	SET NOCOUNT ON;

	Select ArcSearch
	From dbo.LoanServicingLetters
	Where LetterType = @LetterType And LetterOptions = @LetterOption and LetterChoices = @LetterChoice
END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[spLSLGetArcSearch] TO [db_executor]
    AS [dbo];



