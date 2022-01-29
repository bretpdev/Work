USE ULS
GO

BEGIN TRANSACTION

	DECLARE @FileHeaderId INT
	DECLARE @ScriptdataId INT
	DECLARE @LetterId INT
	DECLARE @CommentId INT
	DECLARE @Comment VARCHAR(MAX) =''
	DECLARE @ArcId INT
	DECLARE @HeaderId INT
	DECLARE @ArcScriptDataMapping INT
	DECLARE @Priority INT
	DECLARE @SourceFile VARCHAR(50) = null --can be null if sas inserts directly into db
	DECLARE @ScriptId VARCHAR(50) = 'SCLATESTG' --previous scriptId
	DECLARE @LetterName VARCHAR(20) = 'DANOPHND1' --LetterId from letter tracking
	DECLARE @Pages INT = 2
	DECLARE @FileHeaderText VARCHAR(MAX) = 'BF_SSN,DF_SPE_ACC_ID,DM_PRS_1,DM_PRS_LST,DX_STR_ADR_1,DX_STR_ADR_2,DM_CT,DC_DOM_ST,DF_ZIP,DM_FGN_CNY,DAYS_DLQ,ACSKEY,STATE_IND,COST_CENTER_CODE'
	DECLARE @FileHeaderTextEnd VARCHAR(MAX) = 'BF_SSN,DF_SPE_ACC_ID,DM_PRS_1,DM_PRS_LST,DX_STR_ADR_1,DX_STR_ADR_2,DM_CT,DC_DOM_ST,DF_ZIP,DM_FGN_CNY,DAYS_DLQ,ACSKEY,STATE_IND,COST_CENTER_CODE,CoborrowerAccountNumber'
	DECLARE @FileHeaderState INT = 7
	DECLARE @FileHeaderAcc INT = 1
	DECLARE @FileHeaderCost INT = 13 --If your letter doesnt have a cost center field, use 0
	DECLARE @Arc VARCHAR(5) = 'ALTS1'--Add the Arc ID here
	DECLARE @ActivityType VARCHAR(2) = 'LT'
	DECLARE @ActivityContact VARCHAR(2) = '03'
	-----Add letter to Letters table------------------------------------------------------------------------
	SELECT @LetterId = (SELECT LetterId FROM ULS.[print].Letters WHERE Letter = @LetterName)
	IF @LetterId IS NULL
		BEGIN
			INSERT INTO ULS.[print].[Letters](Letter, PagesPerDocument)
			VALUES(@LetterName, @Pages)

			SELECT @LetterId = @@IDENTITY
		END

	PRINT CONCAT('Letter ID ',@LetterId)

	-----Add a file header----------------------------------------------------------------------------------

	SELECT @FileHeaderId = (SELECT FileHeaderId FROM ULS.[print].FileHeaders WHERE FileHeader = @FileHeaderText)
	IF @FileHeaderId IS NULL
		BEGIN
			INSERT INTO ULS.[print].FileHeaders(FileHeader, StateIndex, AccountNumberIndex, CostCenterCodeIndex)
			VALUES(@FileHeaderText, @FileHeaderState, @FileHeaderAcc, @FileHeaderCost)

			SELECT @FileHeaderId = @@IDENTITY
		END
	PRINT CONCAT('File Header ID ',@FileHeaderId)

	-----Set the priority by getting the max priority and adding 1------------------------------------------
	SELECT @Priority = ((SELECT MAX([Priority]) FROM ULS.[print].ScriptData) + 1)
	PRINT CONCAT('Priority ',@Priority)

	-----Add the letter to the ScriptData table-------------------------------------------------------------
	SELECT @ScriptdataId = (SELECT ScriptDataId FROM ULS.[print].ScriptData WHERE ScriptId = @ScriptId AND SourceFile = @SourceFile AND LetterId = @LetterId AND FileHeaderId = @FileHeaderId AND IsEndorser = 0)
	IF @ScriptdataId IS NULL
		BEGIN
			INSERT INTO ULS.[print].[ScriptData](ScriptID, SourceFile, LetterId, FileHeaderId, ProcessAllFiles, IsEndorser, [Priority], AddBarCodes, DoNotProcessEcorr, AddedBy, AddedAt, Active)
			VALUES(@ScriptId, @SourceFile, @LetterId, @FileHeaderId, 0, 0, @Priority, 1, 0, SUSER_SNAME(), GETDATE(), 1)

			SELECT @ScriptdataId = @@IDENTITY
		END
	PRINT CONCAT('Script Data ID ',@ScriptdataId)

	-----Add a comment if there is an ARC associated with letter--------------------------------------------
	IF @Comment != ''
	BEGIN
		SELECT @CommentId = (SELECT CommentId FROM ULS.[print].Comments WHERE Comment = @Comment)
		IF @CommentId IS NULL
			BEGIN
				INSERT INTO ULS.[print].Comments(Comment)
				VALUES(@Comment)

				SELECT @CommentId = @@IDENTITY
			END
		PRINT CONCAT('Comment ID ',@CommentId)
	END

	-----Add the ARC to the Arcs table if it does not exist-------------------------------------------------
	IF @Arc != ''
	BEGIN
		PRINT CONCAT('ARC ',@Arc)
		SELECT @ArcId = (SELECT ArcId FROM ULS.[print].Arcs WHERE Arc = @Arc)

		IF @ArcId IS NULL
			BEGIN
				INSERT INTO ULS.[print].Arcs(Arc,ActivityType,ActivityContact)
				VALUES(@Arc,@ActivityType,@ActivityContact)

				SELECT @ArcId = @@IDENTITY
			END
		PRINT CONCAT('Arc ID ',@ArcId)
	END

	IF @Arc != ''
	BEGIN
		-----Add the Arc and Comment info to the ArcScriptDataMapping table-------------------------------------
		SELECT @ArcScriptDataMapping = (SELECT ArcScriptDataMappingId FROM ULS.[print].ArcScriptDataMapping WHERE ScriptDataId = @ScriptdataId AND ArcId = @ArcId)
		IF @ArcScriptDataMapping IS NULL
			BEGIN
				INSERT INTO ULS.[print].ArcScriptDataMapping(ScriptDataId, ArcId, ArcTypeId, CommentId)
				VALUES(@ScriptdataId, @ArcId, 6, null)

				SELECT @ArcScriptDataMapping = @@IDENTITY
			END
		PRINT CONCAT('Arc Script Data Mapping ID ',@ArcScriptDataMapping)
	END

	SELECT @FileHeaderId = (SELECT FileHeaderId FROM ULS.[print].FileHeaders WHERE FileHeader = @FileHeaderTextEnd)
	IF @FileHeaderId IS NULL
		BEGIN
			INSERT INTO ULS.[print].FileHeaders(FileHeader, StateIndex, AccountNumberIndex, CostCenterCodeIndex)
			VALUES(@FileHeaderTextEnd, @FileHeaderState, @FileHeaderAcc, @FileHeaderCost)

			SELECT @FileHeaderId = @@IDENTITY
		END
	PRINT CONCAT('File Header ID ',@FileHeaderId)

	-----Set the priority by getting the max priority and adding 1------------------------------------------
	SELECT @Priority = ((SELECT MAX([Priority]) FROM ULS.[print].ScriptData) + 1)
	PRINT CONCAT('Priority ',@Priority)

	SELECT @ScriptdataId = (SELECT ScriptDataId FROM ULS.[print].ScriptData WHERE ScriptId = @ScriptId AND SourceFile = @SourceFile AND LetterId = @LetterId AND FileHeaderId = @FileHeaderId AND IsEndorser = 1)
	IF @ScriptdataId IS NULL
		BEGIN
			INSERT INTO ULS.[print].[ScriptData](ScriptID, SourceFile, LetterId, FileHeaderId, ProcessAllFiles, IsEndorser, [Priority], AddBarCodes, DoNotProcessEcorr, AddedBy, AddedAt, Active, EndorsersBorrowerSSNIndex)
			VALUES(@ScriptId, @SourceFile, @LetterId, @FileHeaderId, 0, 1, @Priority, 1, 0, SUSER_SNAME(), GETDATE(), 1, 14)

			SELECT @ScriptdataId = @@IDENTITY
		END
	PRINT CONCAT('Script Data ID ',@ScriptdataId)

	IF @Arc != ''
	BEGIN
		-----Add the Arc and Comment info to the ArcScriptDataMapping table-------------------------------------
		SELECT @ArcScriptDataMapping = (SELECT ArcScriptDataMappingId FROM ULS.[print].ArcScriptDataMapping WHERE ScriptDataId = @ScriptdataId AND ArcId = @ArcId)
		IF @ArcScriptDataMapping IS NULL
			BEGIN
				INSERT INTO ULS.[print].ArcScriptDataMapping(ScriptDataId, ArcId, ArcTypeId, CommentId)
				VALUES(@ScriptdataId, @ArcId, 6, null)

				SELECT @ArcScriptDataMapping = @@IDENTITY
			END
		PRINT CONCAT('Arc Script Data Mapping ID ',@ArcScriptDataMapping)
	END

COMMIT TRANSACTION
--ROLLBACK TRANSACTION


BEGIN TRANSACTION

	DECLARE @FileHeaderId2 INT
	DECLARE @ScriptdataId2 INT
	DECLARE @LetterId2 INT
	DECLARE @CommentId2 INT
	DECLARE @Comment2 VARCHAR(MAX) =''
	DECLARE @ArcId2 INT
	DECLARE @HeaderId2 INT
	DECLARE @ArcScriptDataMapping2 INT
	DECLARE @Priority2 INT
	DECLARE @SourceFile2 VARCHAR(50) = null --can be null if sas inserts directly into db
	DECLARE @ScriptId2 VARCHAR(50) = 'SCLATESTG' --previous scriptId
	DECLARE @LetterName2 VARCHAR(20) = 'DANOPHND2' --LetterId from letter tracking
	DECLARE @Pages2 INT = 2
	DECLARE @FileHeaderText2 VARCHAR(MAX) = 'BF_SSN,DF_SPE_ACC_ID,DM_PRS_1,DM_PRS_LST,DX_STR_ADR_1,DX_STR_ADR_2,DM_CT,DC_DOM_ST,DF_ZIP,DM_FGN_CNY,DAYS_DLQ,ACSKEY,STATE_IND,COST_CENTER_CODE'
	DECLARE @FileHeaderTextEnd2 VARCHAR(MAX) = 'BF_SSN,DF_SPE_ACC_ID,DM_PRS_1,DM_PRS_LST,DX_STR_ADR_1,DX_STR_ADR_2,DM_CT,DC_DOM_ST,DF_ZIP,DM_FGN_CNY,DAYS_DLQ,ACSKEY,STATE_IND,COST_CENTER_CODE,CoborrowerAccountNumber'
	DECLARE @FileHeaderState2 INT = 7
	DECLARE @FileHeaderAcc2 INT = 1
	DECLARE @FileHeaderCost2 INT = 13 --If your letter doesnt have a cost center field, use 0
	DECLARE @Arc2 VARCHAR(5) = 'ALTS2'--Add the Arc ID here
	DECLARE @ActivityType2 VARCHAR(2) = 'LT'
	DECLARE @ActivityContact2 VARCHAR(2) = '03'
	-----Add letter to Letters table------------------------------------------------------------------------
	SELECT @LetterId2 = (SELECT LetterId FROM ULS.[print].Letters WHERE Letter = @LetterName2)
	IF @LetterId2 IS NULL
		BEGIN
			INSERT INTO ULS.[print].[Letters](Letter, PagesPerDocument)
			VALUES(@LetterName2, @Pages2)

			SELECT @LetterId2 = @@IDENTITY
		END

	PRINT CONCAT('Letter ID ',@LetterId2)

	-----Add a file header----------------------------------------------------------------------------------

	SELECT @FileHeaderId2 = (SELECT FileHeaderId FROM ULS.[print].FileHeaders WHERE FileHeader = @FileHeaderText2)
	IF @FileHeaderId2 IS NULL
		BEGIN
			INSERT INTO ULS.[print].FileHeaders(FileHeader, StateIndex, AccountNumberIndex, CostCenterCodeIndex)
			VALUES(@FileHeaderText2, @FileHeaderState2, @FileHeaderAcc2, @FileHeaderCost2)

			SELECT @FileHeaderId2 = @@IDENTITY
		END
	PRINT CONCAT('File Header ID ',@FileHeaderId2)

	-----Set the priority by getting the max priority and adding 1------------------------------------------
	SELECT @Priority2 = ((SELECT MAX([Priority]) FROM ULS.[print].ScriptData) + 1)
	PRINT CONCAT('Priority ',@Priority2)

	-----Add the letter to the ScriptData table-------------------------------------------------------------
	SELECT @ScriptdataId2 = (SELECT ScriptDataId FROM ULS.[print].ScriptData WHERE ScriptId = @ScriptId2 AND SourceFile = @SourceFile2 AND LetterId = @LetterId2 AND FileHeaderId = @FileHeaderId2 AND IsEndorser = 0)
	IF @ScriptdataId2 IS NULL
		BEGIN
			INSERT INTO ULS.[print].[ScriptData](ScriptID, SourceFile, LetterId, FileHeaderId, ProcessAllFiles, IsEndorser, [Priority], AddBarCodes, DoNotProcessEcorr, AddedBy, AddedAt, Active)
			VALUES(@ScriptId2, @SourceFile2, @LetterId2, @FileHeaderId2, 0, 0, @Priority2, 1, 0, SUSER_SNAME(), GETDATE(), 1)

			SELECT @ScriptdataId2 = @@IDENTITY
		END
	PRINT CONCAT('Script Data ID ',@ScriptdataId2)

	-----Add a comment if there is an ARC associated with letter--------------------------------------------
	IF @Comment2 != ''
	BEGIN
		SELECT @CommentId2 = (SELECT CommentId FROM ULS.[print].Comments WHERE Comment = @Comment2)
		IF @CommentId2 IS NULL
			BEGIN
				INSERT INTO ULS.[print].Comments(Comment)
				VALUES(@Comment2)

				SELECT @CommentId2 = @@IDENTITY
			END
		PRINT CONCAT('Comment ID ',@CommentId2)
	END

	-----Add the ARC to the Arcs table if it does not exist-------------------------------------------------
	IF @Arc2 != ''
	BEGIN
		PRINT CONCAT('ARC ',@Arc2)
		SELECT @ArcId2 = (SELECT ArcId FROM ULS.[print].Arcs WHERE Arc = @Arc2)

		IF @ArcId2 IS NULL
			BEGIN
				INSERT INTO ULS.[print].Arcs(Arc,ActivityType,ActivityContact)
				VALUES(@Arc2,@ActivityType2,@ActivityContact2)

				SELECT @ArcId2 = @@IDENTITY
			END
		PRINT CONCAT('Arc ID ',@ArcId2)
	END

	IF @Arc2 != ''
	BEGIN
		-----Add the Arc and Comment info to the ArcScriptDataMapping table-------------------------------------
		SELECT @ArcScriptDataMapping2 = (SELECT ArcScriptDataMappingId FROM ULS.[print].ArcScriptDataMapping WHERE ScriptDataId = @ScriptdataId2 AND ArcId = @ArcId2)
		IF @ArcScriptDataMapping2 IS NULL
			BEGIN
				INSERT INTO ULS.[print].ArcScriptDataMapping(ScriptDataId, ArcId, ArcTypeId, CommentId)
				VALUES(@ScriptdataId2, @ArcId2, 6, null)

				SELECT @ArcScriptDataMapping2 = @@IDENTITY
			END
		PRINT CONCAT('Arc Script Data Mapping ID ',@ArcScriptDataMapping2)
	END

	SELECT @FileHeaderId2 = (SELECT FileHeaderId FROM ULS.[print].FileHeaders WHERE FileHeader = @FileHeaderTextEnd2)
	IF @FileHeaderId2 IS NULL
		BEGIN
			INSERT INTO ULS.[print].FileHeaders(FileHeader, StateIndex, AccountNumberIndex, CostCenterCodeIndex)
			VALUES(@FileHeaderTextEnd2, @FileHeaderState2, @FileHeaderAcc2, @FileHeaderCost2)

			SELECT @FileHeaderId2 = @@IDENTITY
		END
	PRINT CONCAT('File Header ID ',@FileHeaderId2)

	-----Set the priority by getting the max priority and adding 1------------------------------------------
	SELECT @Priority2 = ((SELECT MAX([Priority]) FROM ULS.[print].ScriptData) + 1)
	PRINT CONCAT('Priority ',@Priority2)

	SELECT @ScriptdataId2 = (SELECT ScriptDataId FROM ULS.[print].ScriptData WHERE ScriptId = @ScriptId2 AND SourceFile = @SourceFile2 AND LetterId = @LetterId2 AND FileHeaderId = @FileHeaderId2 AND IsEndorser = 1)
	IF @ScriptdataId2 IS NULL
		BEGIN
			INSERT INTO ULS.[print].[ScriptData](ScriptID, SourceFile, LetterId, FileHeaderId, ProcessAllFiles, IsEndorser, [Priority], AddBarCodes, DoNotProcessEcorr, AddedBy, AddedAt, Active, EndorsersBorrowerSSNIndex)
			VALUES(@ScriptId2, @SourceFile2, @LetterId2, @FileHeaderId2, 0, 1, @Priority2, 1, 0, SUSER_SNAME(), GETDATE(), 1, 14)

			SELECT @ScriptdataId2 = @@IDENTITY
		END
	PRINT CONCAT('Script Data ID ',@ScriptdataId2)

	IF @Arc2 != ''
	BEGIN
		-----Add the Arc and Comment info to the ArcScriptDataMapping table-------------------------------------
		SELECT @ArcScriptDataMapping2 = (SELECT ArcScriptDataMappingId FROM ULS.[print].ArcScriptDataMapping WHERE ScriptDataId = @ScriptdataId2 AND ArcId = @ArcId2)
		IF @ArcScriptDataMapping2 IS NULL
			BEGIN
				INSERT INTO ULS.[print].ArcScriptDataMapping(ScriptDataId, ArcId, ArcTypeId, CommentId)
				VALUES(@ScriptdataId2, @ArcId2, 6, null)

				SELECT @ArcScriptDataMapping2 = @@IDENTITY
			END
		PRINT CONCAT('Arc Script Data Mapping ID ',@ArcScriptDataMapping2)
	END

COMMIT TRANSACTION
--ROLLBACK TRANSACTION


BEGIN TRANSACTION

	DECLARE @FileHeaderId3 INT
	DECLARE @ScriptdataId3 INT
	DECLARE @LetterId3 INT
	DECLARE @CommentId3 INT
	DECLARE @Comment3 VARCHAR(MAX) =''
	DECLARE @ArcId3 INT
	DECLARE @HeaderId3 INT
	DECLARE @ArcScriptDataMapping3 INT
	DECLARE @Priority3 INT
	DECLARE @SourceFile3 VARCHAR(50) = null --can be null if sas inserts directly into db
	DECLARE @ScriptId3 VARCHAR(50) = 'SCLATESTG' --previous scriptId
	DECLARE @LetterName3 VARCHAR(20) = 'DANOPHND3' --LetterId from letter tracking
	DECLARE @Pages3 INT = 2
	DECLARE @FileHeaderText3 VARCHAR(MAX) = 'BF_SSN,DF_SPE_ACC_ID,DM_PRS_1,DM_PRS_LST,DX_STR_ADR_1,DX_STR_ADR_2,DM_CT,DC_DOM_ST,DF_ZIP,DM_FGN_CNY,DAYS_DLQ,ACSKEY,STATE_IND,COST_CENTER_CODE'
	DECLARE @FileHeaderTextEnd3 VARCHAR(MAX) = 'BF_SSN,DF_SPE_ACC_ID,DM_PRS_1,DM_PRS_LST,DX_STR_ADR_1,DX_STR_ADR_2,DM_CT,DC_DOM_ST,DF_ZIP,DM_FGN_CNY,DAYS_DLQ,ACSKEY,STATE_IND,COST_CENTER_CODE,CoborrowerAccountNumber'
	DECLARE @FileHeaderState3 INT = 7
	DECLARE @FileHeaderAcc3 INT = 1
	DECLARE @FileHeaderCost3 INT = 13 --If your letter doesnt have a cost center field, use 0
	DECLARE @Arc3 VARCHAR(5) = 'ALTT1'--Add the Arc ID here
	DECLARE @ActivityType3 VARCHAR(2) = 'LT'
	DECLARE @ActivityContact3 VARCHAR(2) = '03'
	-----Add letter to Letters table------------------------------------------------------------------------
	SELECT @LetterId3 = (SELECT LetterId FROM ULS.[print].Letters WHERE Letter = @LetterName3)
	IF @LetterId3 IS NULL
		BEGIN
			INSERT INTO ULS.[print].[Letters](Letter, PagesPerDocument)
			VALUES(@LetterName3, @Pages3)

			SELECT @LetterId3 = @@IDENTITY
		END

	PRINT CONCAT('Letter ID ',@LetterId2)

	-----Add a file header----------------------------------------------------------------------------------

	SELECT @FileHeaderId3 = (SELECT FileHeaderId FROM ULS.[print].FileHeaders WHERE FileHeader = @FileHeaderText3)
	IF @FileHeaderId3 IS NULL
		BEGIN
			INSERT INTO ULS.[print].FileHeaders(FileHeader, StateIndex, AccountNumberIndex, CostCenterCodeIndex)
			VALUES(@FileHeaderText3, @FileHeaderState3, @FileHeaderAcc3, @FileHeaderCost3)

			SELECT @FileHeaderId3 = @@IDENTITY
		END
	PRINT CONCAT('File Header ID ',@FileHeaderId3)

	-----Set the priority by getting the max priority and adding 1------------------------------------------
	SELECT @Priority3 = ((SELECT MAX([Priority]) FROM ULS.[print].ScriptData) + 1)
	PRINT CONCAT('Priority ',@Priority3)

	-----Add the letter to the ScriptData table-------------------------------------------------------------
	SELECT @ScriptdataId3 = (SELECT ScriptDataId FROM ULS.[print].ScriptData WHERE ScriptId = @ScriptId3 AND SourceFile = @SourceFile3 AND LetterId = @LetterId3 AND FileHeaderId = @FileHeaderId3 AND IsEndorser = 0)
	IF @ScriptdataId3 IS NULL
		BEGIN
			INSERT INTO ULS.[print].[ScriptData](ScriptID, SourceFile, LetterId, FileHeaderId, ProcessAllFiles, IsEndorser, [Priority], AddBarCodes, DoNotProcessEcorr, AddedBy, AddedAt, Active)
			VALUES(@ScriptId3, @SourceFile3, @LetterId3, @FileHeaderId3, 0, 0, @Priority3, 1, 0, SUSER_SNAME(), GETDATE(), 1)

			SELECT @ScriptdataId3 = @@IDENTITY
		END
	PRINT CONCAT('Script Data ID ',@ScriptdataId3)

	-----Add a comment if there is an ARC associated with letter--------------------------------------------
	IF @Comment3 != ''
	BEGIN
		SELECT @CommentId3 = (SELECT CommentId FROM ULS.[print].Comments WHERE Comment = @Comment3)
		IF @CommentId3 IS NULL
			BEGIN
				INSERT INTO ULS.[print].Comments(Comment)
				VALUES(@Comment3)

				SELECT @CommentId3 = @@IDENTITY
			END
		PRINT CONCAT('Comment ID ',@CommentId3)
	END

	-----Add the ARC to the Arcs table if it does not exist-------------------------------------------------
	IF @Arc3 != ''
	BEGIN
		PRINT CONCAT('ARC ',@Arc3)
		SELECT @ArcId3 = (SELECT ArcId FROM ULS.[print].Arcs WHERE Arc = @Arc3)

		IF @ArcId3 IS NULL
			BEGIN
				INSERT INTO ULS.[print].Arcs(Arc,ActivityType,ActivityContact)
				VALUES(@Arc3,@ActivityType3,@ActivityContact3)

				SELECT @ArcId3 = @@IDENTITY
			END
		PRINT CONCAT('Arc ID ',@ArcId3)
	END

	IF @Arc3 != ''
	BEGIN
		-----Add the Arc and Comment info to the ArcScriptDataMapping table-------------------------------------
		SELECT @ArcScriptDataMapping3 = (SELECT ArcScriptDataMappingId FROM ULS.[print].ArcScriptDataMapping WHERE ScriptDataId = @ScriptdataId3 AND ArcId = @ArcId3)
		IF @ArcScriptDataMapping3 IS NULL
			BEGIN
				INSERT INTO ULS.[print].ArcScriptDataMapping(ScriptDataId, ArcId, ArcTypeId, CommentId)
				VALUES(@ScriptdataId3, @ArcId3, 6, null)

				SELECT @ArcScriptDataMapping3 = @@IDENTITY
			END
		PRINT CONCAT('Arc Script Data Mapping ID ',@ArcScriptDataMapping3)
	END

	SELECT @FileHeaderId3 = (SELECT FileHeaderId FROM ULS.[print].FileHeaders WHERE FileHeader = @FileHeaderTextEnd3)
	IF @FileHeaderId3 IS NULL
		BEGIN
			INSERT INTO ULS.[print].FileHeaders(FileHeader, StateIndex, AccountNumberIndex, CostCenterCodeIndex)
			VALUES(@FileHeaderTextEnd3, @FileHeaderState3, @FileHeaderAcc3, @FileHeaderCost3)

			SELECT @FileHeaderId3 = @@IDENTITY
		END
	PRINT CONCAT('File Header ID ',@FileHeaderId3)

	-----Set the priority by getting the max priority and adding 1------------------------------------------
	SELECT @Priority3 = ((SELECT MAX([Priority]) FROM ULS.[print].ScriptData) + 1)
	PRINT CONCAT('Priority ',@Priority3)

	SELECT @ScriptdataId3 = (SELECT ScriptDataId FROM ULS.[print].ScriptData WHERE ScriptId = @ScriptId3 AND SourceFile = @SourceFile3 AND LetterId = @LetterId3 AND FileHeaderId = @FileHeaderId3 AND IsEndorser = 1)
	IF @ScriptdataId3 IS NULL
		BEGIN
			INSERT INTO ULS.[print].[ScriptData](ScriptID, SourceFile, LetterId, FileHeaderId, ProcessAllFiles, IsEndorser, [Priority], AddBarCodes, DoNotProcessEcorr, AddedBy, AddedAt, Active, EndorsersBorrowerSSNIndex)
			VALUES(@ScriptId3, @SourceFile3, @LetterId3, @FileHeaderId3, 0, 1, @Priority3, 1, 0, SUSER_SNAME(), GETDATE(), 1, 14)

			SELECT @ScriptdataId3 = @@IDENTITY
		END
	PRINT CONCAT('Script Data ID ',@ScriptdataId3)

	IF @Arc3 != ''
	BEGIN
		-----Add the Arc and Comment info to the ArcScriptDataMapping table-------------------------------------
		SELECT @ArcScriptDataMapping3 = (SELECT ArcScriptDataMappingId FROM ULS.[print].ArcScriptDataMapping WHERE ScriptDataId = @ScriptdataId3 AND ArcId = @ArcId3)
		IF @ArcScriptDataMapping3 IS NULL
			BEGIN
				INSERT INTO ULS.[print].ArcScriptDataMapping(ScriptDataId, ArcId, ArcTypeId, CommentId)
				VALUES(@ScriptdataId3, @ArcId3, 6, null)

				SELECT @ArcScriptDataMapping3 = @@IDENTITY
			END
		PRINT CONCAT('Arc Script Data Mapping ID ',@ArcScriptDataMapping3)
	END

COMMIT TRANSACTION
--ROLLBACK TRANSACTION

BEGIN TRANSACTION

	DECLARE @FileHeaderId4 INT
	DECLARE @ScriptdataId4 INT
	DECLARE @LetterId4 INT
	DECLARE @CommentId4 INT
	DECLARE @Comment4 VARCHAR(MAX) =''
	DECLARE @ArcId4 INT
	DECLARE @HeaderId4 INT
	DECLARE @ArcScriptDataMapping4 INT
	DECLARE @Priority4 INT
	DECLARE @SourceFile4 VARCHAR(50) = null --can be null if sas inserts directly into db
	DECLARE @ScriptId4 VARCHAR(50) = 'SCLATESTG' --previous scriptId
	DECLARE @LetterName4 VARCHAR(20) = 'DANOPHND4' --LetterId from letter tracking
	DECLARE @Pages4 INT = 2
	DECLARE @FileHeaderText4 VARCHAR(MAX) = 'BF_SSN,DF_SPE_ACC_ID,DM_PRS_1,DM_PRS_LST,DX_STR_ADR_1,DX_STR_ADR_2,DM_CT,DC_DOM_ST,DF_ZIP,DM_FGN_CNY,DAYS_DLQ,ACSKEY,STATE_IND,COST_CENTER_CODE'
	DECLARE @FileHeaderTextEnd4 VARCHAR(MAX) = 'BF_SSN,DF_SPE_ACC_ID,DM_PRS_1,DM_PRS_LST,DX_STR_ADR_1,DX_STR_ADR_2,DM_CT,DC_DOM_ST,DF_ZIP,DM_FGN_CNY,DAYS_DLQ,ACSKEY,STATE_IND,COST_CENTER_CODE,CoborrowerAccountNumber'
	DECLARE @FileHeaderState4 INT = 7
	DECLARE @FileHeaderAcc4 INT = 1
	DECLARE @FileHeaderCost4 INT = 13 --If your letter doesnt have a cost center field, use 0
	DECLARE @Arc4 VARCHAR(5) = 'ALTT2'--Add the Arc ID here
	DECLARE @ActivityType4 VARCHAR(2) = 'LT'
	DECLARE @ActivityContact4 VARCHAR(2) = '03'
	-----Add letter to Letters table------------------------------------------------------------------------
	SELECT @LetterId4 = (SELECT LetterId FROM ULS.[print].Letters WHERE Letter = @LetterName4)
	IF @LetterId4 IS NULL
		BEGIN
			INSERT INTO ULS.[print].[Letters](Letter, PagesPerDocument)
			VALUES(@LetterName4, @Pages4)

			SELECT @LetterId4 = @@IDENTITY
		END

	PRINT CONCAT('Letter ID ',@LetterId4)

	-----Add a file header----------------------------------------------------------------------------------

	SELECT @FileHeaderId4 = (SELECT FileHeaderId FROM ULS.[print].FileHeaders WHERE FileHeader = @FileHeaderText4)
	IF @FileHeaderId4 IS NULL
		BEGIN
			INSERT INTO ULS.[print].FileHeaders(FileHeader, StateIndex, AccountNumberIndex, CostCenterCodeIndex)
			VALUES(@FileHeaderText4, @FileHeaderState4, @FileHeaderAcc4, @FileHeaderCost4)

			SELECT @FileHeaderId4 = @@IDENTITY
		END
	PRINT CONCAT('File Header ID ',@FileHeaderId4)

	-----Set the priority by getting the max priority and adding 1------------------------------------------
	SELECT @Priority4 = ((SELECT MAX([Priority]) FROM ULS.[print].ScriptData) + 1)
	PRINT CONCAT('Priority ',@Priority4)

	-----Add the letter to the ScriptData table-------------------------------------------------------------
	SELECT @ScriptdataId4 = (SELECT ScriptDataId FROM ULS.[print].ScriptData WHERE ScriptId = @ScriptId4 AND SourceFile = @SourceFile4 AND LetterId = @LetterId4 AND FileHeaderId = @FileHeaderId4 AND IsEndorser = 0)
	IF @ScriptdataId4 IS NULL
		BEGIN
			INSERT INTO ULS.[print].[ScriptData](ScriptID, SourceFile, LetterId, FileHeaderId, ProcessAllFiles, IsEndorser, [Priority], AddBarCodes, DoNotProcessEcorr, AddedBy, AddedAt, Active)
			VALUES(@ScriptId4, @SourceFile4, @LetterId4, @FileHeaderId4, 0, 0, @Priority4, 1, 0, SUSER_SNAME(), GETDATE(), 1)

			SELECT @ScriptdataId4 = @@IDENTITY
		END
	PRINT CONCAT('Script Data ID ',@ScriptdataId4)

	-----Add a comment if there is an ARC associated with letter--------------------------------------------
	IF @Comment4 != ''
	BEGIN
		SELECT @CommentId4 = (SELECT CommentId FROM ULS.[print].Comments WHERE Comment = @Comment4)
		IF @CommentId4 IS NULL
			BEGIN
				INSERT INTO ULS.[print].Comments(Comment)
				VALUES(@Comment4)

				SELECT @CommentId4 = @@IDENTITY
			END
		PRINT CONCAT('Comment ID ',@CommentId4)
	END

	-----Add the ARC to the Arcs table if it does not exist-------------------------------------------------
	IF @Arc4 != ''
	BEGIN
		PRINT CONCAT('ARC ',@Arc4)
		SELECT @ArcId4 = (SELECT ArcId FROM ULS.[print].Arcs WHERE Arc = @Arc4)

		IF @ArcId4 IS NULL
			BEGIN
				INSERT INTO ULS.[print].Arcs(Arc,ActivityType,ActivityContact)
				VALUES(@Arc4,@ActivityType4,@ActivityContact4)

				SELECT @ArcId4 = @@IDENTITY
			END
		PRINT CONCAT('Arc ID ',@ArcId4)
	END

	IF @Arc4 != ''
	BEGIN
		-----Add the Arc and Comment info to the ArcScriptDataMapping table-------------------------------------
		SELECT @ArcScriptDataMapping4 = (SELECT ArcScriptDataMappingId FROM ULS.[print].ArcScriptDataMapping WHERE ScriptDataId = @ScriptdataId4 AND ArcId = @ArcId4)
		IF @ArcScriptDataMapping4 IS NULL
			BEGIN
				INSERT INTO ULS.[print].ArcScriptDataMapping(ScriptDataId, ArcId, ArcTypeId, CommentId)
				VALUES(@ScriptdataId4, @ArcId4, 6, null)

				SELECT @ArcScriptDataMapping4 = @@IDENTITY
			END
		PRINT CONCAT('Arc Script Data Mapping ID ',@ArcScriptDataMapping4)
	END

	SELECT @FileHeaderId4 = (SELECT FileHeaderId FROM ULS.[print].FileHeaders WHERE FileHeader = @FileHeaderTextEnd4)
	IF @FileHeaderId4 IS NULL
		BEGIN
			INSERT INTO ULS.[print].FileHeaders(FileHeader, StateIndex, AccountNumberIndex, CostCenterCodeIndex)
			VALUES(@FileHeaderTextEnd4, @FileHeaderState4, @FileHeaderAcc4, @FileHeaderCost4)

			SELECT @FileHeaderId4 = @@IDENTITY
		END
	PRINT CONCAT('File Header ID ',@FileHeaderId4)

	-----Set the priority by getting the max priority and adding 1------------------------------------------
	SELECT @Priority4 = ((SELECT MAX([Priority]) FROM ULS.[print].ScriptData) + 1)
	PRINT CONCAT('Priority ',@Priority4)

	SELECT @ScriptdataId4 = (SELECT ScriptDataId FROM ULS.[print].ScriptData WHERE ScriptId = @ScriptId4 AND SourceFile = @SourceFile4 AND LetterId = @LetterId4 AND FileHeaderId = @FileHeaderId4 AND IsEndorser = 1)
	IF @ScriptdataId4 IS NULL
		BEGIN
			INSERT INTO ULS.[print].[ScriptData](ScriptID, SourceFile, LetterId, FileHeaderId, ProcessAllFiles, IsEndorser, [Priority], AddBarCodes, DoNotProcessEcorr, AddedBy, AddedAt, Active, EndorsersBorrowerSSNIndex)
			VALUES(@ScriptId4, @SourceFile4, @LetterId4, @FileHeaderId4, 0, 1, @Priority4, 1, 0, SUSER_SNAME(), GETDATE(), 1, 14)

			SELECT @ScriptdataId4 = @@IDENTITY
		END
	PRINT CONCAT('Script Data ID ',@ScriptdataId4)

	IF @Arc4 != ''
	BEGIN
		-----Add the Arc and Comment info to the ArcScriptDataMapping table-------------------------------------
		SELECT @ArcScriptDataMapping4 = (SELECT ArcScriptDataMappingId FROM ULS.[print].ArcScriptDataMapping WHERE ScriptDataId = @ScriptdataId4 AND ArcId = @ArcId4)
		IF @ArcScriptDataMapping4 IS NULL
			BEGIN
				INSERT INTO ULS.[print].ArcScriptDataMapping(ScriptDataId, ArcId, ArcTypeId, CommentId)
				VALUES(@ScriptdataId4, @ArcId4, 6, null)

				SELECT @ArcScriptDataMapping4 = @@IDENTITY
			END
		PRINT CONCAT('Arc Script Data Mapping ID ',@ArcScriptDataMapping4)
	END

COMMIT TRANSACTION
--ROLLBACK TRANSACTION


BEGIN TRANSACTION

	DECLARE @FileHeaderId5 INT
	DECLARE @ScriptdataId5 INT
	DECLARE @LetterId5 INT
	DECLARE @CommentId5 INT
	DECLARE @Comment5 VARCHAR(MAX) =''
	DECLARE @ArcId5 INT
	DECLARE @HeaderId5 INT
	DECLARE @ArcScriptDataMapping5 INT
	DECLARE @Priority5 INT
	DECLARE @SourceFile5 VARCHAR(50) = null --can be null if sas inserts directly into db
	DECLARE @ScriptId5 VARCHAR(50) = 'SCLATESTG' --previous scriptId
	DECLARE @LetterName5 VARCHAR(20) = 'DANOPHND5' --LetterId from letter tracking
	DECLARE @Pages5 INT = 2
	DECLARE @FileHeaderText5 VARCHAR(MAX) = 'BF_SSN,DF_SPE_ACC_ID,DM_PRS_1,DM_PRS_LST,DX_STR_ADR_1,DX_STR_ADR_2,DM_CT,DC_DOM_ST,DF_ZIP,DM_FGN_CNY,DAYS_DLQ,ACSKEY,STATE_IND,COST_CENTER_CODE'
	DECLARE @FileHeaderTextEnd5 VARCHAR(MAX) = 'BF_SSN,DF_SPE_ACC_ID,DM_PRS_1,DM_PRS_LST,DX_STR_ADR_1,DX_STR_ADR_2,DM_CT,DC_DOM_ST,DF_ZIP,DM_FGN_CNY,DAYS_DLQ,ACSKEY,STATE_IND,COST_CENTER_CODE,CoborrowerAccountNumber'
	DECLARE @FileHeaderState5 INT = 7
	DECLARE @FileHeaderAcc5 INT = 1
	DECLARE @FileHeaderCost5 INT = 13 --If your letter doesnt have a cost center field, use 0
	DECLARE @Arc5 VARCHAR(5) = 'ALTV1'--Add the Arc ID here
	DECLARE @ActivityType5 VARCHAR(2) = 'LT'
	DECLARE @ActivityContact5 VARCHAR(2) = '03'
	-----Add letter to Letters table------------------------------------------------------------------------
	SELECT @LetterId5 = (SELECT LetterId FROM ULS.[print].Letters WHERE Letter = @LetterName5)
	IF @LetterId5 IS NULL
		BEGIN
			INSERT INTO ULS.[print].[Letters](Letter, PagesPerDocument)
			VALUES(@LetterName5, @Pages5)

			SELECT @LetterId5 = @@IDENTITY
		END

	PRINT CONCAT('Letter ID ',@LetterId2)

	-----Add a file header----------------------------------------------------------------------------------

	SELECT @FileHeaderId5 = (SELECT FileHeaderId FROM ULS.[print].FileHeaders WHERE FileHeader = @FileHeaderText5)
	IF @FileHeaderId5 IS NULL
		BEGIN
			INSERT INTO ULS.[print].FileHeaders(FileHeader, StateIndex, AccountNumberIndex, CostCenterCodeIndex)
			VALUES(@FileHeaderText5, @FileHeaderState5, @FileHeaderAcc5, @FileHeaderCost5)

			SELECT @FileHeaderId5 = @@IDENTITY
		END
	PRINT CONCAT('File Header ID ',@FileHeaderId5)

	-----Set the priority by getting the max priority and adding 1------------------------------------------
	SELECT @Priority5 = ((SELECT MAX([Priority]) FROM ULS.[print].ScriptData) + 1)
	PRINT CONCAT('Priority ',@Priority5)

	-----Add the letter to the ScriptData table-------------------------------------------------------------
	SELECT @ScriptdataId5 = (SELECT ScriptDataId FROM ULS.[print].ScriptData WHERE ScriptId = @ScriptId5 AND SourceFile = @SourceFile5 AND LetterId = @LetterId5 AND FileHeaderId = @FileHeaderId5 AND IsEndorser = 0)
	IF @ScriptdataId5 IS NULL
		BEGIN
			INSERT INTO ULS.[print].[ScriptData](ScriptID, SourceFile, LetterId, FileHeaderId, ProcessAllFiles, IsEndorser, [Priority], AddBarCodes, DoNotProcessEcorr, AddedBy, AddedAt, Active)
			VALUES(@ScriptId5, @SourceFile5, @LetterId5, @FileHeaderId5, 0, 0, @Priority5, 1, 0, SUSER_SNAME(), GETDATE(), 1)

			SELECT @ScriptdataId5 = @@IDENTITY
		END
	PRINT CONCAT('Script Data ID ',@ScriptdataId5)

	-----Add a comment if there is an ARC associated with letter--------------------------------------------
	IF @Comment5 != ''
	BEGIN
		SELECT @CommentId5 = (SELECT CommentId FROM ULS.[print].Comments WHERE Comment = @Comment5)
		IF @CommentId5 IS NULL
			BEGIN
				INSERT INTO ULS.[print].Comments(Comment)
				VALUES(@Comment5)

				SELECT @CommentId5 = @@IDENTITY
			END
		PRINT CONCAT('Comment ID ',@CommentId5)
	END

	-----Add the ARC to the Arcs table if it does not exist-------------------------------------------------
	IF @Arc5 != ''
	BEGIN
		PRINT CONCAT('ARC ',@Arc5)
		SELECT @ArcId5 = (SELECT ArcId FROM ULS.[print].Arcs WHERE Arc = @Arc5)

		IF @ArcId5 IS NULL
			BEGIN
				INSERT INTO ULS.[print].Arcs(Arc,ActivityType,ActivityContact)
				VALUES(@Arc5,@ActivityType5,@ActivityContact5)

				SELECT @ArcId5 = @@IDENTITY
			END
		PRINT CONCAT('Arc ID ',@ArcId5)
	END

	IF @Arc5 != ''
	BEGIN
		-----Add the Arc and Comment info to the ArcScriptDataMapping table-------------------------------------
		SELECT @ArcScriptDataMapping5 = (SELECT ArcScriptDataMappingId FROM ULS.[print].ArcScriptDataMapping WHERE ScriptDataId = @ScriptdataId5 AND ArcId = @ArcId5)
		IF @ArcScriptDataMapping5 IS NULL
			BEGIN
				INSERT INTO ULS.[print].ArcScriptDataMapping(ScriptDataId, ArcId, ArcTypeId, CommentId)
				VALUES(@ScriptdataId5, @ArcId5, 6, null)

				SELECT @ArcScriptDataMapping5 = @@IDENTITY
			END
		PRINT CONCAT('Arc Script Data Mapping ID ',@ArcScriptDataMapping5)
	END

	SELECT @FileHeaderId5 = (SELECT FileHeaderId FROM ULS.[print].FileHeaders WHERE FileHeader = @FileHeaderTextEnd5)
	IF @FileHeaderId5 IS NULL
		BEGIN
			INSERT INTO ULS.[print].FileHeaders(FileHeader, StateIndex, AccountNumberIndex, CostCenterCodeIndex)
			VALUES(@FileHeaderTextEnd5, @FileHeaderState5, @FileHeaderAcc5, @FileHeaderCost5)

			SELECT @FileHeaderId5 = @@IDENTITY
		END
	PRINT CONCAT('File Header ID ',@FileHeaderId5)

	-----Set the priority by getting the max priority and adding 1------------------------------------------
	SELECT @Priority5 = ((SELECT MAX([Priority]) FROM ULS.[print].ScriptData) + 1)
	PRINT CONCAT('Priority ',@Priority5)

	SELECT @ScriptdataId5 = (SELECT ScriptDataId FROM ULS.[print].ScriptData WHERE ScriptId = @ScriptId5 AND SourceFile = @SourceFile5 AND LetterId = @LetterId5 AND FileHeaderId = @FileHeaderId5 AND IsEndorser = 1)
	IF @ScriptdataId5 IS NULL
		BEGIN
			INSERT INTO ULS.[print].[ScriptData](ScriptID, SourceFile, LetterId, FileHeaderId, ProcessAllFiles, IsEndorser, [Priority], AddBarCodes, DoNotProcessEcorr, AddedBy, AddedAt, Active, EndorsersBorrowerSSNIndex)
			VALUES(@ScriptId5, @SourceFile5, @LetterId5, @FileHeaderId5, 0, 1, @Priority5, 1, 0, SUSER_SNAME(), GETDATE(), 1, 14)

			SELECT @ScriptdataId5 = @@IDENTITY
		END
	PRINT CONCAT('Script Data ID ',@ScriptdataId5)

	IF @Arc5 != ''
	BEGIN
		-----Add the Arc and Comment info to the ArcScriptDataMapping table-------------------------------------
		SELECT @ArcScriptDataMapping5 = (SELECT ArcScriptDataMappingId FROM ULS.[print].ArcScriptDataMapping WHERE ScriptDataId = @ScriptdataId5 AND ArcId = @ArcId5)
		IF @ArcScriptDataMapping5 IS NULL
			BEGIN
				INSERT INTO ULS.[print].ArcScriptDataMapping(ScriptDataId, ArcId, ArcTypeId, CommentId)
				VALUES(@ScriptdataId5, @ArcId5, 6, null)

				SELECT @ArcScriptDataMapping5 = @@IDENTITY
			END
		PRINT CONCAT('Arc Script Data Mapping ID ',@ArcScriptDataMapping5)
	END

COMMIT TRANSACTION
--ROLLBACK TRANSACTION


BEGIN TRANSACTION

	DECLARE @FileHeaderId6 INT
	DECLARE @ScriptdataId6 INT
	DECLARE @LetterId6 INT
	DECLARE @CommentId6 INT
	DECLARE @Comment6 VARCHAR(MAX) =''
	DECLARE @ArcId6 INT
	DECLARE @HeaderId6 INT
	DECLARE @ArcScriptDataMapping6 INT
	DECLARE @Priority6 INT
	DECLARE @SourceFile6 VARCHAR(50) = null --can be null if sas inserts directly into db
	DECLARE @ScriptId6 VARCHAR(50) = 'SCLATESTG' --previous scriptId
	DECLARE @LetterName6 VARCHAR(20) = 'DANOPHND6' --LetterId from letter tracking
	DECLARE @Pages6 INT = 2
	DECLARE @FileHeaderText6 VARCHAR(MAX) = 'BF_SSN,DF_SPE_ACC_ID,DM_PRS_1,DM_PRS_LST,DX_STR_ADR_1,DX_STR_ADR_2,DM_CT,DC_DOM_ST,DF_ZIP,DM_FGN_CNY,DAYS_DLQ,ACSKEY,STATE_IND,COST_CENTER_CODE'
	DECLARE @FileHeaderTextEnd6 VARCHAR(MAX) = 'BF_SSN,DF_SPE_ACC_ID,DM_PRS_1,DM_PRS_LST,DX_STR_ADR_1,DX_STR_ADR_2,DM_CT,DC_DOM_ST,DF_ZIP,DM_FGN_CNY,DAYS_DLQ,ACSKEY,STATE_IND,COST_CENTER_CODE,CoborrowerAccountNumber'
	DECLARE @FileHeaderState6 INT = 7
	DECLARE @FileHeaderAcc6 INT = 1
	DECLARE @FileHeaderCost6 INT = 13 --If your letter doesnt have a cost center field, use 0
	DECLARE @Arc6 VARCHAR(5) = 'ALTV2'--Add the Arc ID here
	DECLARE @ActivityType6 VARCHAR(2) = 'LT'
	DECLARE @ActivityContact6 VARCHAR(2) = '03'
	-----Add letter to Letters table------------------------------------------------------------------------
	SELECT @LetterId6 = (SELECT LetterId FROM ULS.[print].Letters WHERE Letter = @LetterName6)
	IF @LetterId6 IS NULL
		BEGIN
			INSERT INTO ULS.[print].[Letters](Letter, PagesPerDocument)
			VALUES(@LetterName6, @Pages6)

			SELECT @LetterId6 = @@IDENTITY
		END

	PRINT CONCAT('Letter ID ',@LetterId2)

	-----Add a file header----------------------------------------------------------------------------------

	SELECT @FileHeaderId6 = (SELECT FileHeaderId FROM ULS.[print].FileHeaders WHERE FileHeader = @FileHeaderText6)
	IF @FileHeaderId6 IS NULL
		BEGIN
			INSERT INTO ULS.[print].FileHeaders(FileHeader, StateIndex, AccountNumberIndex, CostCenterCodeIndex)
			VALUES(@FileHeaderText6, @FileHeaderState6, @FileHeaderAcc6, @FileHeaderCost6)

			SELECT @FileHeaderId6 = @@IDENTITY
		END
	PRINT CONCAT('File Header ID ',@FileHeaderId6)

	-----Set the priority by getting the max priority and adding 1------------------------------------------
	SELECT @Priority6 = ((SELECT MAX([Priority]) FROM ULS.[print].ScriptData) + 1)
	PRINT CONCAT('Priority ',@Priority6)

	-----Add the letter to the ScriptData table-------------------------------------------------------------
	SELECT @ScriptdataId6 = (SELECT ScriptDataId FROM ULS.[print].ScriptData WHERE ScriptId = @ScriptId6 AND SourceFile = @SourceFile6 AND LetterId = @LetterId6 AND FileHeaderId = @FileHeaderId6 AND IsEndorser = 0)
	IF @ScriptdataId6 IS NULL
		BEGIN
			INSERT INTO ULS.[print].[ScriptData](ScriptID, SourceFile, LetterId, FileHeaderId, ProcessAllFiles, IsEndorser, [Priority], AddBarCodes, DoNotProcessEcorr, AddedBy, AddedAt, Active)
			VALUES(@ScriptId6, @SourceFile6, @LetterId6, @FileHeaderId6, 0, 0, @Priority6, 1, 0, SUSER_SNAME(), GETDATE(), 1)

			SELECT @ScriptdataId6 = @@IDENTITY
		END
	PRINT CONCAT('Script Data ID ',@ScriptdataId6)

	-----Add a comment if there is an ARC associated with letter--------------------------------------------
	IF @Comment6 != ''
	BEGIN
		SELECT @CommentId6 = (SELECT CommentId FROM ULS.[print].Comments WHERE Comment = @Comment6)
		IF @CommentId6 IS NULL
			BEGIN
				INSERT INTO ULS.[print].Comments(Comment)
				VALUES(@Comment6)

				SELECT @CommentId6 = @@IDENTITY
			END
		PRINT CONCAT('Comment ID ',@CommentId6)
	END

	-----Add the ARC to the Arcs table if it does not exist-------------------------------------------------
	IF @Arc6 != ''
	BEGIN
		PRINT CONCAT('ARC ',@Arc6)
		SELECT @ArcId6 = (SELECT ArcId FROM ULS.[print].Arcs WHERE Arc = @Arc6)

		IF @ArcId6 IS NULL
			BEGIN
				INSERT INTO ULS.[print].Arcs(Arc,ActivityType,ActivityContact)
				VALUES(@Arc6,@ActivityType6,@ActivityContact6)

				SELECT @ArcId6 = @@IDENTITY
			END
		PRINT CONCAT('Arc ID ',@ArcId6)
	END

	IF @Arc6 != ''
	BEGIN
		-----Add the Arc and Comment info to the ArcScriptDataMapping table-------------------------------------
		SELECT @ArcScriptDataMapping6 = (SELECT ArcScriptDataMappingId FROM ULS.[print].ArcScriptDataMapping WHERE ScriptDataId = @ScriptdataId6 AND ArcId = @ArcId6)
		IF @ArcScriptDataMapping6 IS NULL
			BEGIN
				INSERT INTO ULS.[print].ArcScriptDataMapping(ScriptDataId, ArcId, ArcTypeId, CommentId)
				VALUES(@ScriptdataId6, @ArcId6, 6, null)

				SELECT @ArcScriptDataMapping6 = @@IDENTITY
			END
		PRINT CONCAT('Arc Script Data Mapping ID ',@ArcScriptDataMapping6)
	END

	SELECT @FileHeaderId6 = (SELECT FileHeaderId FROM ULS.[print].FileHeaders WHERE FileHeader = @FileHeaderTextEnd6)
	IF @FileHeaderId6 IS NULL
		BEGIN
			INSERT INTO ULS.[print].FileHeaders(FileHeader, StateIndex, AccountNumberIndex, CostCenterCodeIndex)
			VALUES(@FileHeaderTextEnd6, @FileHeaderState6, @FileHeaderAcc6, @FileHeaderCost6)

			SELECT @FileHeaderId6 = @@IDENTITY
		END
	PRINT CONCAT('File Header ID ',@FileHeaderId6)

	-----Set the priority by getting the max priority and adding 1------------------------------------------
	SELECT @Priority6 = ((SELECT MAX([Priority]) FROM ULS.[print].ScriptData) + 1)
	PRINT CONCAT('Priority ',@Priority6)

	SELECT @ScriptdataId6 = (SELECT ScriptDataId FROM ULS.[print].ScriptData WHERE ScriptId = @ScriptId6 AND SourceFile = @SourceFile6 AND LetterId = @LetterId6 AND FileHeaderId = @FileHeaderId6 AND IsEndorser = 1)
	IF @ScriptdataId6 IS NULL
		BEGIN
			INSERT INTO ULS.[print].[ScriptData](ScriptID, SourceFile, LetterId, FileHeaderId, ProcessAllFiles, IsEndorser, [Priority], AddBarCodes, DoNotProcessEcorr, AddedBy, AddedAt, Active, EndorsersBorrowerSSNIndex)
			VALUES(@ScriptId6, @SourceFile6, @LetterId6, @FileHeaderId6, 0, 1, @Priority6, 1, 0, SUSER_SNAME(), GETDATE(), 1, 14)

			SELECT @ScriptdataId6 = @@IDENTITY
		END
	PRINT CONCAT('Script Data ID ',@ScriptdataId6)

	IF @Arc6 != ''
	BEGIN
		-----Add the Arc and Comment info to the ArcScriptDataMapping table-------------------------------------
		SELECT @ArcScriptDataMapping6 = (SELECT ArcScriptDataMappingId FROM ULS.[print].ArcScriptDataMapping WHERE ScriptDataId = @ScriptdataId6 AND ArcId = @ArcId6)
		IF @ArcScriptDataMapping6 IS NULL
			BEGIN
				INSERT INTO ULS.[print].ArcScriptDataMapping(ScriptDataId, ArcId, ArcTypeId, CommentId)
				VALUES(@ScriptdataId6, @ArcId6, 6, null)

				SELECT @ArcScriptDataMapping6 = @@IDENTITY
			END
		PRINT CONCAT('Arc Script Data Mapping ID ',@ArcScriptDataMapping6)
	END

COMMIT TRANSACTION
--ROLLBACK TRANSACTION