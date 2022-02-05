CREATE PROCEDURE [acurintc].[GetRejectReasons]
AS

	SELECT
		RejectReasonId,
		RejectReason
	FROM
		acurintc.RejectReasons

RETURN 0
