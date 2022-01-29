DECLARE @OnelinkRegion INT = (SELECT RegionId FROM Regions WHERE UPPER(Region) = 'ONELINK')
DECLARE @CompassRegion INT = (SELECT RegionId FROM Regions WHERE UPPER(Region) = 'COMPASSUHEAA')
DECLARE @RepayCentsiblyRegion INT = (SELECT RegionId FROM Regions WHERE UPPER(Region) = 'REPAYCENTSIBLY')
IF @RepayCentsiblyRegion IS NULL
BEGIN
	INSERT INTO dbo.Regions(Region)
	VALUES
		('RepayCentsibly')

	SET @RepayCentsiblyRegion = (SELECT RegionId FROM Regions WHERE UPPER(Region) = 'REPAYCENTSIBLY')
END

INSERT INTO [dbo].CampaignPrefixes(CampaignPrefix, ScriptId, RegionId, Active)
VALUES
	('LPP', 'DIACTCMTS', @CompassRegion, 1),
	('LGP', 'DIACTCMTS', @OnelinkRegion, 1),
	('RC', 'RCCALLHIST', @RepayCentsiblyRegion, 1)
