CREATE PROCEDURE [acurintc].[GetDemographicsSources]
AS

	SELECT
		DemographicsSourceId,
		Name
	FROM
		acurintc.DemographicsSources


RETURN 0
