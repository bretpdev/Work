USE [EcorrUheaa]
GO

BEGIN TRANSACTION

DECLARE @Letter VARCHAR(10) = 'US06BFADJ'
IF NOT EXISTS(SELECT Letter FROM [EcorrUheaa]..[Letters] WHERE Letter = @Letter)
	BEGIN
		DECLARE @SubjectLine VARCHAR(50) = 'Your Forbearance Has Been Adjusted'
		DECLARE @DocComment VARCHAR(255) = 'Attention is Needed on Your UHEAA Online Account'
		INSERT INTO [EcorrUheaa]..[Letters](Letter, LetterTypeId, DocId, Viewable, ReportDescription, ReportName, Viewed, MainframeRegion, SubjectLine, DocSource, DocComment, WorkFlow, DocDelete, Active)
		VALUES(@Letter, 1, 'XELT', 'Y', 'Description','Name','N','V3CB',@SubjectLine,'IMPORT',@DocComment,'N','N',1)
		PRINT 'Record added to .Letters'
	END


SET @Letter = 'US06BFAPRL'
IF NOT EXISTS(SELECT Letter FROM [EcorrUheaa]..[Letters] WHERE Letter = @Letter)
	BEGIN
		SET @SubjectLine = 'Your Forbearance Has Been Approved'
		SET @DocComment = 'Attention is Needed on Your UHEAA Online Account'
		INSERT INTO [EcorrUheaa]..[Letters](Letter, LetterTypeId, DocId, Viewable, ReportDescription, ReportName, Viewed, MainframeRegion, SubjectLine, DocSource, DocComment, WorkFlow, DocDelete, Active)
		VALUES(@Letter, 1, 'XELT', 'Y', 'Description','Name','N','V3CB',@SubjectLine,'IMPORT',@DocComment,'N','N',1)
		PRINT 'Record added to .Letters'
	END

COMMIT TRANSACTION