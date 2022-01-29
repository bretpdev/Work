CREATE FUNCTION [unexdascrb].[VerifyCallId]
(
	@CallId VARCHAR(50)
)
RETURNS BIT
AS
BEGIN
	IF EXISTS(
		SELECT      
			*
		FROM
			dbo.RPT_CALLACTIVITY
		WHERE
			CallId = @CallId
	) RETURN 1
	RETURN 0
END
