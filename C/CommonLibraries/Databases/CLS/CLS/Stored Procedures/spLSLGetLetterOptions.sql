-- =============================================
-- Author:		Jarom Ryan
-- Create date: 12/27/2012
-- Description:	this sp will get the Letter Options ofr a given Letter type.
-- =============================================
CREATE PROCEDURE [dbo].[spLSLGetLetterOptions]

	@LetterType As Varchar(50)
AS
BEGIN

	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT Distinct LetterOptions
	From dbo.LoanServicingLetters
	Where LetterType = @LetterType
END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[spLSLGetLetterOptions] TO [db_executor]
    AS [dbo];



