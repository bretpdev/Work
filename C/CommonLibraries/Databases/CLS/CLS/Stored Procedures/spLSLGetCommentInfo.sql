-- =============================================
-- Author:		Jarom Ryan
-- Create date: 01/15/2012
-- Description:	This sp will get the information needed to add a comment with the loan servicing letters script
-- =============================================
CREATE PROCEDURE [dbo].[spLSLGetCommentInfo]

@LetterType As Varchar (50),
@LetterOption As Varchar (100),
@LetterChoice As Varchar (1500),
@LetterChoice2 As Varchar (1500) = NULL,
@LetterChoice3 As Varchar (1500) = NULL,
@LetterChoice4 As Varchar (1500) = NULL,
@LetterChoice5 As Varchar (1500) = NULL

AS
BEGIN

	SET NOCOUNT ON;

	SELECT DISTINCT
	 ArcSearch,
	 Arc,
	 MIN(Hierarchy) As Hierarchy,
	 Tx2xSearch,
	 Note
	FROM dbo.LoanServicingLetters
	WHERE LetterType = @LetterType And LetterOptions = @LetterOption And LetterChoices in (@LetterChoice, @LetterChoice2, @LetterChoice3, @LetterChoice4, @LetterChoice5)
	GROUP BY ArcSearch, Arc, Hierarchy, Tx2xSearch, Note
END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[spLSLGetCommentInfo] TO [db_executor]
    AS [dbo];



