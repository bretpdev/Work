USE ULS
GO

BEGIN TRANSACTION

	DECLARE @ScriptdataId int = (SELECT ScriptDataId FROM [print].ScriptData WHERE SourceFile = 'ULWG96.LWG96R2.*')
	DECLARE @Comment VARCHAR(MAX) = 'TILP Graduation Letter sent to Borrower'
	DECLARE @ArcName VARCHAR(5) = 'TLPGR'
	DECLARE @CommentId int
	DECLARE @ArcId int
	DECLARE @ArcScriptDataMapping int
	DECLARE @Arc varchar(5)

--------------R2-----------------------------------R2--------------------------------------------R2-----------------------------------------
	----Add a comment if there is an ARC associated with letter--------------------------------------------
	IF NOT EXISTS (SELECT Comment FROM [ULS].[print].Comments WHERE Comment = @Comment)
	BEGIN
		INSERT INTO [ULS].[print].Comments(Comment)
		VALUES(@Comment)
	END

	SELECT @CommentId = (SELECT CommentId FROM [ULS].[print].Comments WHERE Comment = @Comment)
	PRINT CONCAT('Comment ID ',@CommentId)

	-----Add the ARC to the Arcs table if it does not exist-------------------------------------------------
	SET @Arc = @ArcName--Add the Arc ID here
	PRINT CONCAT('ARC ',@Arc)

	SELECT @ArcId = (SELECT ArcId FROM [ULS].[print].Arcs WHERE Arc = @Arc)

	IF (@ArcId = '' OR @ArcId IS NULL OR @ArcId = 0)
		BEGIN
			INSERT INTO [ULS].[print].Arcs(Arc)
			VALUES(@Arc)

			SELECT @ArcId = @@IDENTITY
		END
	PRINT CONCAT('Arc ID ',@ArcId)

	-----Add the Arc and Comment info to the ArcScriptDataMapping table-------------------------------------
	IF NOT EXISTS (SELECT * FROM [ULS].[print].ArcScriptDataMapping WHERE ScriptDataId = @ScriptdataId AND ArcId = @ArcId AND CommentId = @CommentId)
	BEGIN
		INSERT INTO [ULS].[print].ArcScriptDataMapping(ScriptDataId, ArcId, ArcTypeId, CommentId)
		VALUES(@ScriptdataId, @ArcId, 0, @CommentId)
	END

	SELECT @ArcScriptDataMapping = (SELECT ArcScriptDataMappingId FROM [ULS].[print].ArcScriptDataMapping WHERE ScriptDataId = @ScriptdataId AND ArcId = @ArcId AND CommentId = @CommentId)
	PRINT CONCAT('Arc Script Data Mapping ID ',@ArcScriptDataMapping)

--------------R3-----------------------------------R3--------------------------------------------R3-----------------------------------------	
	SET @ScriptdataId = (SELECT ScriptDataId FROM [print].ScriptData WHERE SourceFile = 'ULWG96.LWG96R3.*')
	SET @Comment = 'TILP Teaching License Not Received Letter sent to Borrower'
	SET @ArcName = 'TLPTL'
	---Add a comment if there is an ARC associated with letter--------------------------------------------
	IF NOT EXISTS (SELECT Comment FROM [ULS].[print].Comments WHERE Comment = @Comment)
	BEGIN
		INSERT INTO [ULS].[print].Comments(Comment)
		VALUES(@Comment)
	END

	SELECT @CommentId = (SELECT CommentId FROM [ULS].[print].Comments WHERE Comment = @Comment)
	PRINT CONCAT('Comment ID ',@CommentId)

	-----Add the ARC to the Arcs table if it does not exist-------------------------------------------------
	SET @Arc = @ArcName--Add the Arc ID here
	PRINT CONCAT('ARC ',@Arc)

	SELECT @ArcId = (SELECT ArcId FROM [ULS].[print].Arcs WHERE Arc = @Arc)

	IF (@ArcId = '' OR @ArcId IS NULL OR @ArcId = 0)
		BEGIN
			INSERT INTO [ULS].[print].Arcs(Arc)
			VALUES(@Arc)

			SELECT @ArcId = @@IDENTITY
		END
	PRINT CONCAT('Arc ID ',@ArcId)

	-----Add the Arc and Comment info to the ArcScriptDataMapping table-------------------------------------
	IF NOT EXISTS (SELECT * FROM [ULS].[print].ArcScriptDataMapping WHERE ScriptDataId = @ScriptdataId AND ArcId = @ArcId AND CommentId = @CommentId)
	BEGIN
		INSERT INTO [ULS].[print].ArcScriptDataMapping(ScriptDataId, ArcId, ArcTypeId, CommentId)
		VALUES(@ScriptdataId, @ArcId, 0, @CommentId)
	END

	SELECT @ArcScriptDataMapping = (SELECT ArcScriptDataMappingId FROM [ULS].[print].ArcScriptDataMapping WHERE ScriptDataId = @ScriptdataId AND ArcId = @ArcId AND CommentId = @CommentId)
	PRINT CONCAT('Arc Script Data Mapping ID ',@ArcScriptDataMapping)

--------------R4-----------------------------------R4--------------------------------------------R4-----------------------------------------
	SET @ScriptdataId = (SELECT ScriptDataId FROM [print].ScriptData WHERE SourceFile = 'ULWG96.LWG96R4.*')
	SET @Comment = 'TILP Leave of Absence Will Expire Letter sent to Borrower'
	SET @ArcName = 'TLLAE'
	---Add a comment if there is an ARC associated with letter--------------------------------------------
	IF NOT EXISTS (SELECT Comment FROM [ULS].[print].Comments WHERE Comment = @Comment)
	BEGIN
		INSERT INTO [ULS].[print].Comments(Comment)
		VALUES(@Comment)
	END

	SELECT @CommentId = (SELECT CommentId FROM [ULS].[print].Comments WHERE Comment = @Comment)
	PRINT CONCAT('Comment ID ',@CommentId)

	-----Add the ARC to the Arcs table if it does not exist-------------------------------------------------
	SET @Arc = @ArcName--Add the Arc ID here
	PRINT CONCAT('ARC ',@Arc)

	SELECT @ArcId = (SELECT ArcId FROM [ULS].[print].Arcs WHERE Arc = @Arc)

	IF (@ArcId = '' OR @ArcId IS NULL OR @ArcId = 0)
		BEGIN
			INSERT INTO [ULS].[print].Arcs(Arc)
			VALUES(@Arc)

			SELECT @ArcId = @@IDENTITY
		END
	PRINT CONCAT('Arc ID ',@ArcId)

	-----Add the Arc and Comment info to the ArcScriptDataMapping table-------------------------------------
	IF NOT EXISTS (SELECT * FROM [ULS].[print].ArcScriptDataMapping WHERE ScriptDataId = @ScriptdataId AND ArcId = @ArcId AND CommentId = @CommentId)
	BEGIN
		INSERT INTO [ULS].[print].ArcScriptDataMapping(ScriptDataId, ArcId, ArcTypeId, CommentId)
		VALUES(@ScriptdataId, @ArcId, 0, @CommentId)
	END

	SELECT @ArcScriptDataMapping = (SELECT ArcScriptDataMappingId FROM [ULS].[print].ArcScriptDataMapping WHERE ScriptDataId = @ScriptdataId AND ArcId = @ArcId AND CommentId = @CommentId)
	PRINT CONCAT('Arc Script Data Mapping ID ',@ArcScriptDataMapping)

--------------R5-----------------------------------R5--------------------------------------------R5-----------------------------------------
	SET @ScriptdataId = (SELECT ScriptDataId FROM [print].ScriptData WHERE SourceFile = 'ULWG96.LWG96R5.*')
	SET @Comment = 'TILP Leave of Absence Has Expired Letter sent to Borrower'
	SET @ArcName = 'TLLAX'
	---Add a comment if there is an ARC associated with letter--------------------------------------------
	IF NOT EXISTS (SELECT Comment FROM [ULS].[print].Comments WHERE Comment = @Comment)
	BEGIN
		INSERT INTO [ULS].[print].Comments(Comment)
		VALUES(@Comment)
	END

	SELECT @CommentId = (SELECT CommentId FROM [ULS].[print].Comments WHERE Comment = @Comment)
	PRINT CONCAT('Comment ID ',@CommentId)

	-----Add the ARC to the Arcs table if it does not exist-------------------------------------------------
	SET @Arc = @ArcName--Add the Arc ID here
	PRINT CONCAT('ARC ',@Arc)

	SELECT @ArcId = (SELECT ArcId FROM [ULS].[print].Arcs WHERE Arc = @Arc)

	IF (@ArcId = '' OR @ArcId IS NULL OR @ArcId = 0)
		BEGIN
			INSERT INTO [ULS].[print].Arcs(Arc)
			VALUES(@Arc)

			SELECT @ArcId = @@IDENTITY
		END
	PRINT CONCAT('Arc ID ',@ArcId)

	-----Add the Arc and Comment info to the ArcScriptDataMapping table-------------------------------------
	IF NOT EXISTS (SELECT * FROM [ULS].[print].ArcScriptDataMapping WHERE ScriptDataId = @ScriptdataId AND ArcId = @ArcId AND CommentId = @CommentId)
	BEGIN
		INSERT INTO [ULS].[print].ArcScriptDataMapping(ScriptDataId, ArcId, ArcTypeId, CommentId)
		VALUES(@ScriptdataId, @ArcId, 0, @CommentId)
	END

	SELECT @ArcScriptDataMapping = (SELECT ArcScriptDataMappingId FROM [ULS].[print].ArcScriptDataMapping WHERE ScriptDataId = @ScriptdataId AND ArcId = @ArcId AND CommentId = @CommentId)
	PRINT CONCAT('Arc Script Data Mapping ID ',@ArcScriptDataMapping)

--------------R6-----------------------------------R6--------------------------------------------R6-----------------------------------------
	SET @ScriptdataId = (SELECT ScriptDataId FROM [print].ScriptData WHERE SourceFile = 'ULWG96.LWG96R6.*')
	SET @Comment = 'Borrower Dropped from TILP Program Letter sent to Borrower'
	SET @ArcName = 'TLPDP'
	---Add a comment if there is an ARC associated with letter--------------------------------------------
	IF NOT EXISTS (SELECT Comment FROM [ULS].[print].Comments WHERE Comment = @Comment)
	BEGIN
		INSERT INTO [ULS].[print].Comments(Comment)
		VALUES(@Comment)
	END

	SELECT @CommentId = (SELECT CommentId FROM [ULS].[print].Comments WHERE Comment = @Comment)
	PRINT CONCAT('Comment ID ',@CommentId)

	-----Add the ARC to the Arcs table if it does not exist-------------------------------------------------
	SET @Arc = @ArcName--Add the Arc ID here
	PRINT CONCAT('ARC ',@Arc)

	SELECT @ArcId = (SELECT ArcId FROM [ULS].[print].Arcs WHERE Arc = @Arc)

	IF (@ArcId = '' OR @ArcId IS NULL OR @ArcId = 0)
		BEGIN
			INSERT INTO [ULS].[print].Arcs(Arc)
			VALUES(@Arc)

			SELECT @ArcId = @@IDENTITY
		END
	PRINT CONCAT('Arc ID ',@ArcId)

	-----Add the Arc and Comment info to the ArcScriptDataMapping table-------------------------------------
	IF NOT EXISTS (SELECT * FROM [ULS].[print].ArcScriptDataMapping WHERE ScriptDataId = @ScriptdataId AND ArcId = @ArcId AND CommentId = @CommentId)
	BEGIN
		INSERT INTO [ULS].[print].ArcScriptDataMapping(ScriptDataId, ArcId, ArcTypeId, CommentId)
		VALUES(@ScriptdataId, @ArcId, 0, @CommentId)
	END

	SELECT @ArcScriptDataMapping = (SELECT ArcScriptDataMappingId FROM [ULS].[print].ArcScriptDataMapping WHERE ScriptDataId = @ScriptdataId AND ArcId = @ArcId AND CommentId = @CommentId)
	PRINT CONCAT('Arc Script Data Mapping ID ',@ArcScriptDataMapping)

--------------R7-----------------------------------R7--------------------------------------------R7-----------------------------------------	
	SET @ScriptdataId = (SELECT ScriptDataId FROM [print].ScriptData WHERE SourceFile = 'ULWG96.LWG96R7.*')
	SET @Comment = 'Borrower Never Accepted to Teaching Program Letter sent to Borrower'
	SET @ArcName = 'TLBNA'
	---Add a comment if there is an ARC associated with letter--------------------------------------------
	IF NOT EXISTS (SELECT Comment FROM [ULS].[print].Comments WHERE Comment = @Comment)
	BEGIN
		INSERT INTO [ULS].[print].Comments(Comment)
		VALUES(@Comment)
	END

	SELECT @CommentId = (SELECT CommentId FROM [ULS].[print].Comments WHERE Comment = @Comment)
	PRINT CONCAT('Comment ID ',@CommentId)

	-----Add the ARC to the Arcs table if it does not exist-------------------------------------------------
	SET @Arc = @ArcName--Add the Arc ID here
	PRINT CONCAT('ARC ',@Arc)

	SELECT @ArcId = (SELECT ArcId FROM [ULS].[print].Arcs WHERE Arc = @Arc)

	IF (@ArcId = '' OR @ArcId IS NULL OR @ArcId = 0)
		BEGIN
			INSERT INTO [ULS].[print].Arcs(Arc)
			VALUES(@Arc)

			SELECT @ArcId = @@IDENTITY
		END
	PRINT CONCAT('Arc ID ',@ArcId)

	-----Add the Arc and Comment info to the ArcScriptDataMapping table-------------------------------------
	IF NOT EXISTS (SELECT * FROM [ULS].[print].ArcScriptDataMapping WHERE ScriptDataId = @ScriptdataId AND ArcId = @ArcId AND CommentId = @CommentId)
	BEGIN
		INSERT INTO [ULS].[print].ArcScriptDataMapping(ScriptDataId, ArcId, ArcTypeId, CommentId)
		VALUES(@ScriptdataId, @ArcId, 0, @CommentId)
	END

	SELECT @ArcScriptDataMapping = (SELECT ArcScriptDataMappingId FROM [ULS].[print].ArcScriptDataMapping WHERE ScriptDataId = @ScriptdataId AND ArcId = @ArcId AND CommentId = @CommentId)
	PRINT CONCAT('Arc Script Data Mapping ID ',@ArcScriptDataMapping)

--------------R8-----------------------------------R8--------------------------------------------R8-----------------------------------------
	SET @ScriptdataId = (SELECT ScriptDataId FROM [print].ScriptData WHERE SourceFile = 'ULWG96.LWG96R8.*')
	SET @Comment = 'One Year TILP Grace Used Letter sent to Borrower'
	SET @ArcName = 'TI1YR'
	---Add a comment if there is an ARC associated with letter--------------------------------------------
	IF NOT EXISTS (SELECT Comment FROM [ULS].[print].Comments WHERE Comment = @Comment)
	BEGIN
		INSERT INTO [ULS].[print].Comments(Comment)
		VALUES(@Comment)
	END

	SELECT @CommentId = (SELECT CommentId FROM [ULS].[print].Comments WHERE Comment = @Comment)
	PRINT CONCAT('Comment ID ',@CommentId)

	-----Add the ARC to the Arcs table if it does not exist-------------------------------------------------
	SET @Arc = @ArcName--Add the Arc ID here
	PRINT CONCAT('ARC ',@Arc)

	SELECT @ArcId = (SELECT ArcId FROM [ULS].[print].Arcs WHERE Arc = @Arc)

	IF (@ArcId = '' OR @ArcId IS NULL OR @ArcId = 0)
		BEGIN
			INSERT INTO [ULS].[print].Arcs(Arc)
			VALUES(@Arc)

			SELECT @ArcId = @@IDENTITY
		END
	PRINT CONCAT('Arc ID ',@ArcId)

	-----Add the Arc and Comment info to the ArcScriptDataMapping table-------------------------------------
	IF NOT EXISTS (SELECT * FROM [ULS].[print].ArcScriptDataMapping WHERE ScriptDataId = @ScriptdataId AND ArcId = @ArcId AND CommentId = @CommentId)
	BEGIN
		INSERT INTO [ULS].[print].ArcScriptDataMapping(ScriptDataId, ArcId, ArcTypeId, CommentId)
		VALUES(@ScriptdataId, @ArcId, 0, @CommentId)
	END

	SELECT @ArcScriptDataMapping = (SELECT ArcScriptDataMappingId FROM [ULS].[print].ArcScriptDataMapping WHERE ScriptDataId = @ScriptdataId AND ArcId = @ArcId AND CommentId = @CommentId)
	PRINT CONCAT('Arc Script Data Mapping ID ',@ArcScriptDataMapping)

--------------R9-----------------------------------R9--------------------------------------------R9-----------------------------------------
	SET @ScriptdataId = (SELECT ScriptDataId FROM [print].ScriptData WHERE SourceFile = 'ULWG96.LWG96R9.*')
	SET @Comment = 'TILP Maximum Semesters Allotted Used Letter sent to Borrower'
	SET @ArcName = 'TLMSA'
	---Add a comment if there is an ARC associated with letter--------------------------------------------
	IF NOT EXISTS (SELECT Comment FROM [ULS].[print].Comments WHERE Comment = @Comment)
	BEGIN
		INSERT INTO [ULS].[print].Comments(Comment)
		VALUES(@Comment)
	END

	SELECT @CommentId = (SELECT CommentId FROM [ULS].[print].Comments WHERE Comment = @Comment)
	PRINT CONCAT('Comment ID ',@CommentId)

	-----Add the ARC to the Arcs table if it does not exist-------------------------------------------------
	SET @Arc = @ArcName--Add the Arc ID here
	PRINT CONCAT('ARC ',@Arc)

	SELECT @ArcId = (SELECT ArcId FROM [ULS].[print].Arcs WHERE Arc = @Arc)

	IF (@ArcId = '' OR @ArcId IS NULL OR @ArcId = 0)
		BEGIN
			INSERT INTO [ULS].[print].Arcs(Arc)
			VALUES(@Arc)

			SELECT @ArcId = @@IDENTITY
		END
	PRINT CONCAT('Arc ID ',@ArcId)

	-----Add the Arc and Comment info to the ArcScriptDataMapping table-------------------------------------
	IF NOT EXISTS (SELECT * FROM [ULS].[print].ArcScriptDataMapping WHERE ScriptDataId = @ScriptdataId AND ArcId = @ArcId AND CommentId = @CommentId)
	BEGIN
		INSERT INTO [ULS].[print].ArcScriptDataMapping(ScriptDataId, ArcId, ArcTypeId, CommentId)
		VALUES(@ScriptdataId, @ArcId, 0, @CommentId)
	END

	SELECT @ArcScriptDataMapping = (SELECT ArcScriptDataMappingId FROM [ULS].[print].ArcScriptDataMapping WHERE ScriptDataId = @ScriptdataId AND ArcId = @ArcId AND CommentId = @CommentId)
	PRINT CONCAT('Arc Script Data Mapping ID ',@ArcScriptDataMapping)

--------------R10-----------------------------------R10--------------------------------------------R10----------------------------------------	
	SET @ScriptdataId = (SELECT ScriptDataId FROM [print].ScriptData WHERE SourceFile = 'ULWG96.LWG96R10*')
	SET @Comment = 'TILP Teaching Status Follow Up Letter sent to Borrower'
	SET @ArcName = 'TI1FU'
	---Add a comment if there is an ARC associated with letter--------------------------------------------
	IF NOT EXISTS (SELECT Comment FROM [ULS].[print].Comments WHERE Comment = @Comment)
	BEGIN
		INSERT INTO [ULS].[print].Comments(Comment)
		VALUES(@Comment)
	END

	SELECT @CommentId = (SELECT CommentId FROM [ULS].[print].Comments WHERE Comment = @Comment)
	PRINT CONCAT('Comment ID ',@CommentId)

	-----Add the ARC to the Arcs table if it does not exist-------------------------------------------------
	SET @Arc = @ArcName--Add the Arc ID here
	PRINT CONCAT('ARC ',@Arc)

	SELECT @ArcId = (SELECT ArcId FROM [ULS].[print].Arcs WHERE Arc = @Arc)

	IF (@ArcId = '' OR @ArcId IS NULL OR @ArcId = 0)
		BEGIN
			INSERT INTO [ULS].[print].Arcs(Arc)
			VALUES(@Arc)

			SELECT @ArcId = @@IDENTITY
		END
	PRINT CONCAT('Arc ID ',@ArcId)

	-----Add the Arc and Comment info to the ArcScriptDataMapping table-------------------------------------
	IF NOT EXISTS (SELECT * FROM [ULS].[print].ArcScriptDataMapping WHERE ScriptDataId = @ScriptdataId AND ArcId = @ArcId AND CommentId = @CommentId)
	BEGIN
		INSERT INTO [ULS].[print].ArcScriptDataMapping(ScriptDataId, ArcId, ArcTypeId, CommentId)
		VALUES(@ScriptdataId, @ArcId, 0, @CommentId)
	END

	SELECT @ArcScriptDataMapping = (SELECT ArcScriptDataMappingId FROM [ULS].[print].ArcScriptDataMapping WHERE ScriptDataId = @ScriptdataId AND ArcId = @ArcId AND CommentId = @CommentId)
	PRINT CONCAT('Arc Script Data Mapping ID ',@ArcScriptDataMapping)

COMMIT TRANSACTION
--ROLLBACK TRANSACTION