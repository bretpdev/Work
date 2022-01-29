--RUN ON UHEAASQLDB

BEGIN TRANSACTION
	DECLARE @ERROR INT = 0
	DECLARE @ROWCOUNT INT = 0
	DECLARE @ExpectedRowCount INT = 25
	
--new values set 1
	--select * from ULS.emailbatch.HTMLFiles
	INSERT INTO ULS.emailbatch.HTMLFiles (HTMLFile)
	VALUES
		 ('CUR1EMLUH.html')
		,('CUR2EMLUH.html')
		,('CUR3EMLUH.html')
		,('CUR4EMLUH.html')
		,('CUR5EMLUH.html')
	;--5
	-- Save/Set the row count and error number (if any) from the previously executed statement
	SELECT @ROWCOUNT = @@ROWCOUNT, @ERROR = @@ERROR

	DECLARE @HTML1 TINYINT = (SELECT HTMLFileId FROM ULS.emailbatch.HTMLFiles WHERE HTMLFile = 'CUR1EMLUH.html');
	DECLARE @HTML2 TINYINT = (SELECT HTMLFileId FROM ULS.emailbatch.HTMLFiles WHERE HTMLFile = 'CUR2EMLUH.html');
	DECLARE @HTML3 TINYINT = (SELECT HTMLFileId FROM ULS.emailbatch.HTMLFiles WHERE HTMLFile = 'CUR3EMLUH.html');
	DECLARE @HTML4 TINYINT = (SELECT HTMLFileId FROM ULS.emailbatch.HTMLFiles WHERE HTMLFile = 'CUR4EMLUH.html');
	DECLARE @HTML5 TINYINT = (SELECT HTMLFileId FROM ULS.emailbatch.HTMLFiles WHERE HTMLFile = 'CUR5EMLUH.html');

--new values set 2
	--select * from uls.emailbatch.SubjectLines
	INSERT INTO ULS.emailbatch.SubjectLines (SubjectLine)
	VALUES
		 ('Your Loans Qualify for the Fresh Start Program')
		,('Important message from UHEAA')
		,('Time is Running Out to Participate in the Fresh Start Program!')
		,('UHEAA needs to talk to you')
		,('Final notice from UHEAA')
	;--5
	-- Save/Set the row count and error number (if any) from the previously executed statement
	SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR

	DECLARE @SUBJECTLINE1 TINYINT = (SELECT SubjectLineId FROM ULS.emailbatch.SubjectLines WHERE SubjectLine = 'Your Loans Qualify for the Fresh Start Program');
	DECLARE @SUBJECTLINE2 TINYINT = (SELECT SubjectLineId FROM ULS.emailbatch.SubjectLines WHERE SubjectLine = 'Important message from UHEAA');
	DECLARE @SUBJECTLINE3 TINYINT = (SELECT SubjectLineId FROM ULS.emailbatch.SubjectLines WHERE SubjectLine = 'Time is Running Out to Participate in the Fresh Start Program!');
	DECLARE @SUBJECTLINE4 TINYINT = (SELECT SubjectLineId FROM ULS.emailbatch.SubjectLines WHERE SubjectLine = 'UHEAA needs to talk to you');
	DECLARE @SUBJECTLINE5 TINYINT = (SELECT SubjectLineId FROM ULS.emailbatch.SubjectLines WHERE SubjectLine = 'Final notice from UHEAA');

--new values set 3
	--select * from ULS.emailbatch.Arcs
	INSERT INTO ULS.emailbatch.Arcs (Arc)
	VALUES
		 ('CURE1')
		,('CURE2')
		,('CURE3')
		,('CURE4')
		,('CURE5')
	;--5
	-- Save/Set the row count and error number (if any) from the previously executed statement
	SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR

	DECLARE @ARC1 TINYINT = (SELECT ArcId FROM ULS.emailbatch.Arcs WHERE Arc = 'CURE1');
	DECLARE @ARC2 TINYINT = (SELECT ArcId FROM ULS.emailbatch.Arcs WHERE Arc = 'CURE2');
	DECLARE @ARC3 TINYINT = (SELECT ArcId FROM ULS.emailbatch.Arcs WHERE Arc = 'CURE3');
	DECLARE @ARC4 TINYINT = (SELECT ArcId FROM ULS.emailbatch.Arcs WHERE Arc = 'CURE4');
	DECLARE @ARC5 TINYINT = (SELECT ArcId FROM ULS.emailbatch.Arcs WHERE Arc = 'CURE5');

--new values set 4
	--select * from ULS.emailbatch.Comments
	INSERT INTO ULS.emailbatch.Comments (Comment)
	VALUES
		 ('Sent Cure Email 1')
		,('Sent Cure Email 2')
		,('Sent Cure Email 3')
		,('Sent Cure Email 4')
		,('Sent Cure Email 5')
	;--5
	-- Save/Set the row count and error number (if any) from the previously executed statement
	SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR
	
	DECLARE @COMMENT1 TINYINT = (SELECT CommentId FROM ULS.emailbatch.Comments WHERE Comment = 'Sent Cure Email 1');
	DECLARE @COMMENT2 TINYINT = (SELECT CommentId FROM ULS.emailbatch.Comments WHERE Comment = 'Sent Cure Email 2');
	DECLARE @COMMENT3 TINYINT = (SELECT CommentId FROM ULS.emailbatch.Comments WHERE Comment = 'Sent Cure Email 3');
	DECLARE @COMMENT4 TINYINT = (SELECT CommentId FROM ULS.emailbatch.Comments WHERE Comment = 'Sent Cure Email 4');
	DECLARE @COMMENT5 TINYINT = (SELECT CommentId FROM ULS.emailbatch.Comments WHERE Comment = 'Sent Cure Email 5');
	
	DECLARE @DTS DATETIME = GETDATE();

--new values set 5
	--select * from ULS.emailbatch.EmailCampaigns
	INSERT INTO ULS.emailbatch.EmailCampaigns
	(
		--EmailCampaignId,
		SourceFile
		,HTMLFileId
		,FromAddressId
		,SubjectLineId
		,ProcessAllFiles
		,OKIfMissing
		,OKIfEmpty
		,ArcId
		,CommentId
		,ActivityTypeId
		,ActivityContactId
		,AddedBy
		,AddedAt
		,DeletedBy
		,DeletedAt
	)
	VALUES
		('CureEmail.1*', @HTML1, 3, @SUBJECTLINE1, 1, 1, 1, @ARC1, @COMMENT1, 1, 1, 'SASR_4377', @DTS, NULL, NULL),
		('CureEmail.2*', @HTML2, 3, @SUBJECTLINE2, 1, 1, 1, @ARC2, @COMMENT2, 1, 1, 'SASR_4377', @DTS, NULL, NULL),
		('CureEmail.3*', @HTML3, 3, @SUBJECTLINE3, 1, 1, 1, @ARC3, @COMMENT3, 1, 1, 'SASR_4377', @DTS, NULL, NULL),
		('CureEmail.4*', @HTML4, 3, @SUBJECTLINE4, 1, 1, 1, @ARC4, @COMMENT4, 1, 1, 'SASR_4377', @DTS, NULL, NULL),
		('CureEmail.5*', @HTML5, 3, @SUBJECTLINE5, 1, 1, 1, @ARC5, @COMMENT5, 1, 1, 'SASR_4377', @DTS, NULL, NULL)
	;--5
	-- Save/Set the row count and error number (if any) from the previously executed statement
	SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR

IF @ROWCOUNT = @ExpectedRowCount AND @ERROR = 0
	BEGIN
		PRINT 'Transaction committed'
		COMMIT TRANSACTION
		--ROLLBACK TRANSACTION
	END
ELSE
	BEGIN
		PRINT 'ROWCOUNT:  ' + CAST(@ROWCOUNT as VARCHAR(10))
		PRINT 'ERROR:  ' + CAST(@ERROR as VARCHAR(10))
		PRINT 'Transaction NOT committed'
		ROLLBACK TRANSACTION
	END
