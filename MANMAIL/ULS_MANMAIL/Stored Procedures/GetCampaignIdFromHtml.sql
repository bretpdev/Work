CREATE PROCEDURE [manmail].[GetCampaignIdFromHtml]
	@HtmlFile varchar(50),
	@IsEndorser bit
AS
	SELECT
		EmailCampaignId
	FROM
		emailbatch.EmailCampaigns EC
		LEFT JOIN emailbatch.HTMLFiles H
			ON EC.HTMLFileId = H.HTMLFileId
		LEFT JOIN emailbatch.Arcs A
			ON EC.ArcId = A.ArcId
	WHERE
		(
			(H.HTMLFile = @HtmlFile AND @IsEndorser = 1 AND A.ARC = 'D030A')
			OR
			(H.HTMLFile = @HtmlFile AND @IsEndorser = 0 AND A.Arc = 'D030P')
		)
		OR
		(
			(H.HTMLFile = @HtmlFile AND @IsEndorser = 1 AND A.ARC = 'CAEEM')
			OR
			(H.HTMLFile = @HtmlFile AND @IsEndorser = 0 AND A.Arc = 'CABEM')
		)
RETURN 0