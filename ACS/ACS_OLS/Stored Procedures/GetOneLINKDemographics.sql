﻿CREATE PROCEDURE [acs].[GetOneLINKDemographics]
	@Ssn VARCHAR(9)
AS
	
	SELECT 
		RTRIM(DM_PRS_1) AS DM_PRS_1, 
		RTRIM(DM_PRS_LST) AS DM_PRS_LST, 
		DF_SPE_ACC_ID
	FROM
		ODW..PD01_PDM_INF
	WHERE
		 DF_PRS_ID = @Ssn
