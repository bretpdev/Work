CREATE PROCEDURE [deskaudits].[GetCommonFailReasons]

AS

	SELECT 
		CommonFailReasonId AS FailReasonId,
		FailReasonDescription
	FROM
		deskaudits.CommonFailReasons

RETURN 0
