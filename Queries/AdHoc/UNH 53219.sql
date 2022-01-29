USE ULS
GO

IF NOT EXISTS(SELECT * FROM emailbatch.Arcs WHERE Arc = 'NDSNU')
	BEGIN
		INSERT INTO emailbatch.Arcs(Arc)
		VALUES('NDSNU')
	END

IF NOT EXISTS(SELECT * FROM	emailbatch.HTMLFiles WHERE HTMLFile = 'NDSTRU.html')
	BEGIN
		INSERT INTO emailbatch.HTMLFiles(HTMLFile)
		VALUES('NDSTRU.html')
	END

IF NOT EXISTS(SELECT * FROM emailbatch.Comments WHERE Comment = 'Disaster email notice sent to borrower')
	BEGIN
		INSERT INTO emailbatch.Comments(Comment)
		VALUES('Disaster email notice sent to borrower')
	END

DECLARE
	@ArcId INT,
	@HtmlFileId INT,
	@CommentId INT,
	@FromAddressId INT,
	@SubjectLineId INT

SET @ArcId = (SELECT ArcId FROM emailbatch.Arcs WHERE Arc = 'NDSNU')
SET @HtmlFileId = (SELECT HTMLFileId FROM emailbatch.HTMLFiles WHERE HTMLFile = 'NDSTRU.html')
SET @CommentId = (SELECT CommentId FROM emailbatch.Comments WHERE Comment = 'Disaster email notice sent to borrower')
SET @FromAddressId = (SELECT FromAddressId FROM emailbatch.FromAddresses WHERE FromAddress = 'uheaa@utahsbr.edu')
SET @SubjectLineId = (SELECT SubjectLineId FROM emailbatch.SubjectLines WHERE SubjectLine = 'Federal Disaster Area Relief')

IF NOT EXISTS(SELECT * FROM emailbatch.EmailCampaigns WHERE HTMLFileId = @HtmlFileId AND FromAddressId = @FromAddressId AND SubjectLineId = @SubjectLineId AND ArcId = @ArcId AND CommentId = @CommentId)
	BEGIN
		INSERT INTO emailbatch.EmailCampaigns(SourceFile, HTMLFileId, FromAddressId, SubjectLineId, ProcessAllFiles, OKIfMissing, OKIfEmpty, ArcId, CommentId, ActivityTypeId, ActivityContactId, AddedBy, AddedAt)
		VALUES(NULL, @HtmlFileId, @FromAddressId, @SubjectLineId, 0, 1, 0, @ArcId, @CommentId, NULL, NULL, 'DCR', GETDATE())
	END