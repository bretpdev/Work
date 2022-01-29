USE ULS
GO

IF NOT EXISTS(SELECT * FROM	emailbatch.HTMLFiles WHERE HTMLFile = 'NDSTDU.html')
	BEGIN
		INSERT INTO emailbatch.HTMLFiles(HTMLFile)
		VALUES('NDSTDU.html')
	END

DECLARE
	@ArcId INT,
	@HtmlFileId INT,
	@CommentId INT,
	@FromAddressId INT,
	@SubjectLineId INT

SET @ArcId = (SELECT ArcId FROM emailbatch.Arcs WHERE Arc = 'NDSNU')
SET @HtmlFileId = (SELECT HTMLFileId FROM emailbatch.HTMLFiles WHERE HTMLFile = 'NDSTDU.html')
SET @CommentId = (SELECT CommentId FROM emailbatch.Comments WHERE Comment = 'Disaster email notice sent to borrower')
SET @FromAddressId = (SELECT FromAddressId FROM emailbatch.FromAddresses WHERE FromAddress = 'uheaa@utahsbr.edu')
SET @SubjectLineId = (SELECT SubjectLineId FROM emailbatch.SubjectLines WHERE SubjectLine = 'Federal Disaster Area Relief')

IF NOT EXISTS(SELECT * FROM emailbatch.EmailCampaigns WHERE HTMLFileId = @HtmlFileId AND FromAddressId = @FromAddressId AND SubjectLineId = @SubjectLineId AND ArcId = @ArcId AND CommentId = @CommentId)
	BEGIN
		INSERT INTO emailbatch.EmailCampaigns(SourceFile, HTMLFileId, FromAddressId, SubjectLineId, ProcessAllFiles, OKIfMissing, OKIfEmpty, ArcId, CommentId, ActivityTypeId, ActivityContactId, AddedBy, AddedAt)
		VALUES(NULL, @HtmlFileId, @FromAddressId, @SubjectLineId, 0, 1, 0, @ArcId, @CommentId, NULL, NULL, 'DCR', GETDATE())
	END