-- =============================================
-- Author:		Jarom Ryan
-- Create date: 01/14/2013
-- Description:	Will get the System information for a given letter
-- =============================================
Create PROCEDURE [dbo].[spLSLGetLetterId] 

@LetterType As Varchar (50),
@LetterOption As Varchar (100),
@LetterChoice As Varchar (1500)

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT LetterId
	From dbo.LoanServicingLetters
	Where LetterType = @LetterType And LetterOptions = @LetterOption And LetterChoices = @LetterChoice
END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[spLSLGetLetterId] TO [db_executor]
    AS [dbo];



