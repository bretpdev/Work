CREATE PROCEDURE [dbo].[GetOnelinkActionCodeForDispositionCode]
	@Disposition VARCHAR(2)

AS
	SELECT 
		ActionCode,
		ActivityType,
		ActivityContactType,
		Comment
	FROM
		OnelinkDialerResponseMapping
	WHERE
		DispositionCode = @Disposition
RETURN 0
