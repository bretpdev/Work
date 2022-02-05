-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[spLSLGetDefForbOptions] 
	-- Add the parameters for the stored procedure here
	@Type As Varchar (50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT Distinct  LetterOptions
	From dbo.LoanServicingLetters
	Where LetterType = @Type
END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[spLSLGetDefForbOptions] TO [db_executor]
    AS [dbo];



