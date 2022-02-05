CREATE PROCEDURE GetCallCampaigns
(
	@Region VARCHAR(20)
)
AS
BEGIN
	SELECT C.CallCampaign
	FROM CallCampaigns C INNER JOIN Region R on R.RegionId = C.RegionId
	WHERE R.Region = @Region
END;
