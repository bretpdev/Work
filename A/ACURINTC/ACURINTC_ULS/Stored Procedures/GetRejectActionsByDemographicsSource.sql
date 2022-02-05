CREATE PROCEDURE [acurintc].[GetRejectActionsByDemographicsSource]
	@DemographicsSourceId INT
AS

	SELECT
		[RejectActionId],
		[ActionCodeAddress],
		[ActionCodePhone],
		[ActionCodeEmail],
		[DemographicsSourceId],
		[RejectReasonId]
	FROM
		acurintc.RejectActions
	WHERE
		DemographicsSourceId = @DemographicsSourceId

RETURN 0
