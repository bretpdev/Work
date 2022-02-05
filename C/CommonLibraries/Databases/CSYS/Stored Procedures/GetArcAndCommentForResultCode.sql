CREATE PROCEDURE [dbo].[GetArcAndCommentForResultCode]
		@ResultCode		varchar(2)
AS
	SELECT 
		Arc,
		Comment,
		ResponseCode
	FROM
		[DialerResponseCodeMapping]
	WHERE 
		ResultCode = @ResultCode
RETURN 0
