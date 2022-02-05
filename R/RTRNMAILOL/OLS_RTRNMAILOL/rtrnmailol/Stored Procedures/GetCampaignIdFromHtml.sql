CREATE PROCEDURE [rtrnmailol].[GetCampaignIdFromHtml]
	@HtmlFile VARCHAR(300),
	@IsEndorser BIT
AS
	SELECT
		EmailCampaignId
	FROM
		ULS.emailbatch.EmailCampaigns EC
		INNER JOIN ULS.emailbatch.HTMLFiles H
			ON EC.HTMLFileId = H.HTMLFileId
		LEFT JOIN ULS.emailbatch.Arcs A
			ON EC.ArcId = A.ArcId
	WHERE
		EC.DeletedAt IS NULL
		AND
		(
			(H.HTMLFile = @HtmlFile AND @IsEndorser = 1 AND EC.ArcId IS NULL AND EC.SourceFile = 'RTRNMAILOL')
			OR					    
			(H.HTMLFile = @HtmlFile AND @IsEndorser = 0 AND A.Arc = 'D030P' AND EC.SourceFile = 'RTRNMAILOL')
		)