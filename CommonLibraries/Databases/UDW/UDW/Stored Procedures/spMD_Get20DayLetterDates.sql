
CREATE PROCEDURE [dbo].[spMD_Get20DayLetterDates]
	@AccountNumber		varchar(10)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	SELECT	LD_ATY_REQ_RCV AS SentDate
	FROM	dbo.AY10_20DayLetter
	WHERE	DF_SPE_ACC_ID = @AccountNumber
	ORDER BY CONVERT(DATETIME, LD_ATY_REQ_RCV) DESC

END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[spMD_Get20DayLetterDates] TO [UHEAA\Imaging Users]
    AS [dbo];

