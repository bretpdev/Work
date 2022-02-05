-- =============================================
-- Author:		Jarom Ryan
-- Create date: 01/14/2013
-- Description:	Will get the System information for a given letter
-- =============================================
CREATE PROCEDURE [dbo].[spLSLGetLetterInformation] 

@LetterType As Varchar (50),
@LetterOption As Varchar (100),
@LetterChoice As Varchar (1500)

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT ArcSearch,Arc,LetterId,Hierarchy,Tx2xSearch,Note
	From dbo.LoanServicingLetters
	Where LetterType = @LetterType And LetterOptions = @LetterOption And LetterChoices = @LetterChoice
END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[spLSLGetLetterInformation] TO [db_executor]
    AS [dbo];



