USE ULS
GO

DECLARE @HtmlId INT;
DECLARE @SubjectId INT;
DECLARE @ArcId INT;
DECLARE @FromId INT;
DECLARE @EmailCampaignId INT;

IF NOT EXISTS(SELECT TOP 1 HtmlFileId FROM ULS.emailbatch.HTMLFiles WHERE HTMLFile = 'NTDSFBEXUH.html')
BEGIN
	INSERT INTO ULS.emailbatch.HtmlFiles(HTMLFile)
	VALUES ('NTDSFBEXUH.html')
END
SET @HtmlId = (SELECT TOP 1 HtmlFileId from ULS.emailbatch.HTMLFiles WHERE HTMLFile = 'NTDSFBEXUH.html')

IF NOT EXISTS(SELECT TOP 1 SubjectLineId FROM ULS.emailbatch.SubjectLines WHERE SubjectLine = 'Your Natural Disaster Forbearance is Ending')
BEGIN
	INSERT INTO ULS.emailbatch.SubjectLines(SubjectLine)
	VALUES ('Your Natural Disaster Forbearance is Ending')
END
SET @SubjectId = (SELECT TOP 1 SubjectLineId FROM ULS.emailbatch.SubjectLines WHERE SubjectLine = 'Your Natural Disaster Forbearance is Ending')

IF NOT EXISTS(SELECT TOP 1 ArcId FROM ULS.emailbatch.Arcs WHERE Arc = 'CVDEX')
BEGIN
	INSERT INTO ULS.emailbatch.Arcs(Arc)
	VALUES ('CVDEX')
END
SET @ArcId = (SELECT TOP 1 ArcId FROM ULS.emailbatch.Arcs WHERE Arc = 'CVDEX')

IF NOT EXISTS(SELECT TOP 1 FromAddressId FROM ULS.emailbatch.FromAddresses WHERE FromAddress = 'noreply@utahsbr.edu')
BEGIN
	INSERT INTO ULS.emailbatch.FromAddresses(FromAddress)
	VALUES ('noreply@utahsbr.edu')
END
SET @FromId = (SELECT TOP 1 FromAddressId FROM ULS.emailbatch.FromAddresses WHERE FromAddress = 'noreply@utahsbr.edu')

IF NOT EXISTS(SELECT TOP 1 EmailCampaignId FROM ULS.emailbatch.EmailCampaigns WHERE HTMLFileId = @HtmlId AND SubjectLineId = @SubjectId AND ArcId = @ArcId AND FromAddressId = @FromId)
BEGIN
	INSERT INTO ULS.emailbatch.EmailCampaigns(SourceFile, HTMLFileId, FromAddressId, SubjectLineId, ProcessAllFiles, OKIfMissing, OKIfEmpty, ArcId, AddedBy, AddedAt)
	VALUES('',@HtmlId,1,@SubjectId,1,1,1,@ArcId,SUSER_SNAME(),GETDATE())
END
SET @EmailCampaignId = (SELECT TOP 1 EmailCampaignId FROM ULS.emailbatch.EmailCampaigns WHERE HTMLFileId = @HtmlId AND SubjectLineId = @SubjectId AND ArcId = @ArcId AND FromAddressId = @FromId)

SELECT
	@HtmlId,
	@SubjectId,
	@ArcId,
	@FromId,
	@EmailCampaignId