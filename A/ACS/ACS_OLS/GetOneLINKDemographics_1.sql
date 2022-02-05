CREATE PROCEDURE [acs].[GetOneLINKDemographics]
	-- Add the parameters for the stored procedure here
	@BfSSN VARCHAR(9)
AS
BEGIN
    -- Insert statements for procedure here
	SELECT 
		RTRIM(DM_PRS_1) DM_PRS_1, 
		RTRIM(DM_PRS_LST) DM_PRS_LST, 
		DF_SPE_ACC_ID
	FROM
		ODW..PD01_PDM_INF
	WHERE
		 DF_PRS_ID = @BfSSN
END
