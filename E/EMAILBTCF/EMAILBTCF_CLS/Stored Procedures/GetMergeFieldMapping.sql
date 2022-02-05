CREATE PROCEDURE [emailbtcf].[GetMergeFieldMapping]
	@EmailCampaignId INT
AS

	SELECT
		MF.MergeFieldId,
		MF.MergeFieldName,
		LDMFM.LineDataIndex
	FROM
		[emailbtcf].LineDataMergeFieldMapping LDMFM
		INNER JOIN [emailbtcf].MergeFields MF
			ON MF.MergeFieldId = LDMFM.MergeFieldId
	WHERE
		LDMFM.EmailCampaignId = @EmailCampaignId
		AND MF.Active = 1