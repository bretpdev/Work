


CREATE PROCEDURE [dbo].[GetLettersForHomePage]
	@HomePage VARCHAR(50)
AS
BEGIN
	
	SET NOCOUNT ON;

    SELECT
		DisplayName AS [Description],
		DataForFunctionCall AS Arc
	FROM 
		[MenuOptionsScriptsAndServices]
	WHERE 
		SubToBeCalled = 'AddLetterARC'
		AND HomePage = @HomePage
END


