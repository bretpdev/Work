CREATE PROCEDURE [rtrnmailuh].[GetCampaignIdFromHtml]
	@HtmlFile varchar(50),
	@IsEndorser BIT
AS
	SELECT
		EmailCampaignId
	FROM
		emailbatch.EmailCampaigns EC
		INNER JOIN emailbatch.HTMLFiles H
			ON EC.HTMLFileId = H.HTMLFileId
		LEFT JOIN emailbatch.Arcs A
			ON EC.ArcId = A.ArcId
	WHERE
		EC.DeletedAt IS NULL
		AND
		(
			(H.HTMLFile = @HtmlFile AND @IsEndorser = 1 AND EC.ArcId IS NULL AND EC.SourceFile = 'RTRNMAILUH')
			OR
			(H.HTMLFile = @HtmlFile AND @IsEndorser = 0 AND A.Arc = 'CABEM' AND EC.SourceFile = 'RTRNMAILUH')
		)