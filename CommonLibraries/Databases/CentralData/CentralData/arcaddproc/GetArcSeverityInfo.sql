CREATE PROCEDURE [arcaddproc].[GetArcSeverityInfo]

AS
	SELECT
		ERR.ErrorCode,
		ERR.ProcessingAttempts,
		ERR.NotificationSeverityTypeId,
		ERR.RequeueHours
	FROM
		arcaddproc.ErrorCodeSeverityMapping ERR
		
RETURN 0
