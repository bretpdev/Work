-- =============================================
-- Author:		Bret Pehrson
-- Create date: 8/14/2013
-- Description:	Returns text to be merged in letters
-- =============================================
CREATE PROCEDURE [dbo].[GetLetterText] 
	-- Add the parameters for the stored procedure here
	@MergeFieldName varchar(50),
	@Condition varchar(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT
		[Text]
	FROM
		GENR_DAT_LetterConditionalText
	WHERE
		LetterID = 'BLNGSTMNT'
		AND MergeFieldName = @MergeFieldName
		AND Condition = @Condition
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetLetterText] TO [db_executor]
    AS [dbo];

