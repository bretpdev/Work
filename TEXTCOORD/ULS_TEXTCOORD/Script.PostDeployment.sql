/*
Post-Deployment Script Template							
--------------------------------------------------------------------------------------
 This file contains SQL statements that will be appended to the build script.		
 Use SQLCMD syntax to include a file in the post-deployment script.			
 Example:      :r .\myfile.sql								
 Use SQLCMD syntax to reference a variable in the post-deployment script.		
 Example:      :setvar TableName MyTable							
               SELECT * FROM [$(TableName)]					
--------------------------------------------------------------------------------------
*/
GRANT EXECUTE ON SCHEMA::textcoord TO db_executor

DECLARE @LowerDelinquencyId INT = 1
DECLARE @UpperDelinquencyId INT = 2
DECLARE @SegmentId INT = 3
DECLARE @PerformanceCategoryId INT = 4
DECLARE @NumberToSendId INT = 5

MERGE textcoord.UiFields AS t USING
(
	SELECT @LowerDelinquencyId [UiFieldId], 'LowerDelinquency' [FieldName]
	UNION
	SELECT @UpperDelinquencyId, 'UpperDelinquency'
	UNION
	SELECT @SegmentId, 'Segment'
	UNION
	SELECT @PerformanceCategoryId, 'PerformanceCategory'
	UNION
	SELECT @NumberToSendId, 'NumberToSend'

) AS s
ON t.UiFieldId = s.UiFieldId
WHEN NOT MATCHED BY target THEN
	INSERT (UiFieldId, FieldName)
	VALUES (UiFieldId, FieldName)
WHEN NOT MATCHED BY source THEN
	DELETE
WHEN MATCHED THEN
	UPDATE SET FieldName = s.FieldName
;

DECLARE @DelinquentCampaignId INT = 1
DECLARE @IdrCampaignId INT = 2
DECLARE @Delinquent1CampaignId INT = 3
DECLARE @Delinquent2CampaignId INT = 4
DECLARE @Delinquent3CampaignId INT = 5

SET IDENTITY_INSERT textcoord.Campaigns ON

IF NOT EXISTS(SELECT * FROM textcoord.Campaigns WHERE CampaignId = @DelinquentCampaignId)
	BEGIN
		INSERT INTO textcoord.Campaigns (CampaignId, Campaign, Sproc) VALUES (@DelinquentCampaignId, 'DELINQUENT','textcoord.SearchDelinquency')
	END

IF NOT EXISTS(SELECT * FROM textcoord.Campaigns WHERE CampaignId = @IdrCampaignId)
	BEGIN
		INSERT INTO textcoord.Campaigns (CampaignId, Campaign, Sproc) VALUES (@IdrCampaignId, 'IDR','textcoord.SearchIDR')
		INSERT INTO textcoord.CampaignDisabledUiFields (CampaignId, UiFieldId) 
		VALUES
		(@IdrCampaignId, @LowerDelinquencyId),
		(@IdrCampaignId, @UpperDelinquencyId)
	END

IF NOT EXISTS(SELECT * FROM textcoord.Campaigns WHERE CampaignID = @Delinquent1CampaignId)
	BEGIN
		INSERT INTO textcoord.Campaigns (CampaignId, Campaign, CampaignCode, Sproc) VALUES (@Delinquent1CampaignId, 'DELINQUENT (LOW-RANGE)', 'delinquency1', 'dbo.SearchDelinquency')
	END
IF NOT EXISTS(SELECT * FROM textcoord.Campaigns WHERE CampaignID = @Delinquent2CampaignId)
	BEGIN
		INSERT INTO textcoord.Campaigns (CampaignId, Campaign, CampaignCode, Sproc) VALUES (@Delinquent2CampaignId, 'DELINQUENT (MID-RANGE)', 'delinquency2', 'dbo.SearchDelinquency')
	END
IF NOT EXISTS(SELECT * FROM textcoord.Campaigns WHERE CampaignID = @Delinquent3CampaignId)
	BEGIN
		INSERT INTO textcoord.Campaigns (CampaignId, Campaign, CampaignCode, Sproc) VALUES (@Delinquent3CampaignId, 'DELINQUENT (HIGH-RANGE)', 'delinquency3', 'dbo.SearchDelinquency')
	END

SET IDENTITY_INSERT textcoord.Campaigns OFF

UPDATE
	textcoord.Campaigns
SET
	Sproc = REPLACE(Sproc, 'dbo.', 'textcoord.'),
	CampaignCode = ISNULL(CampaignCode, LOWER(Campaign))