CREATE PROCEDURE [dbo].[spGENRHierarchyEmailCopying] 

@BU			VARCHAR(50),
@Lvl			VARCHAR(50)

AS

DECLARE @NextSearch		VARCHAR(50)
DECLARE @Recips			VARCHAR(8000)
DECLARE @TempRecip			VARCHAR(8000)
DECLARE @ReTbl			TABLE(Recips     	VARCHAR(8000))
DECLARE @LvlCheck			VARCHAR(50)
DECLARE @SkipLastLookUp	bit

/* init vars */
SET @NextSearch = @BU
SET @Recips = ''
SET @SkipLastLookUp = 0

WHILE (SELECT Type FROM dbo.GENR_LST_BusinessUnits WHERE BusinessUnit = @NextSearch) <> @Lvl
BEGIN
	SET @TempRecip = (SELECT TOP 1 WindowsUserID  + '@utahsbr.edu' AS EmailAddr 
				FROM dbo.GENR_REF_BU_Agent_Xref
				WHERE BusinessUnit = @NextSearch AND Role = 'Manager')
	/* put together return string */
	IF @Recips = ''
	BEGIN
		IF @TempRecip <> '' AND @TempRecip IS NOT NULL 
		BEGIN
			SET @Recips = @TempRecip 
		END
	END
	ELSE
	BEGIN
		IF @TempRecip <> '' AND @TempRecip IS NOT NULL 
		BEGIN
			SET @Recips = @Recips + ',' + @TempRecip
		END
	END
	print @NextSearch
	SET @NextSearch = (SELECT Parent FROM dbo.GENR_LST_BusinessUnits WHERE BusinessUnit = @NextSearch)
	--this is in case layer is skipped
	SET @LvlCheck = (SELECT Type FROM dbo.GENR_LST_BusinessUnits WHERE BusinessUnit = @NextSearch)
	IF @Lvl = 'Group'
	BEGIN
		IF (@LvlCheck = 'Subbranch' OR @LvlCheck = 'Branch' OR @LvlCheck = 'Division')
		BEGIN
			SET @SkipLastLookUp = 1
			BREAK 
		END
	END
	ELSE IF @Lvl = 'Subbranch'
	BEGIN
		IF (@LvlCheck = 'Branch' OR @LvlCheck = 'Division')
		BEGIN
			SET @SkipLastLookUp = 1
			BREAK 
		END
	END
	ELSE IF @Lvl = 'Branch'
	BEGIN
		IF (@LvlCheck = 'Division')
		BEGIN
			SET @SkipLastLookUp = 1
			BREAK 
		END
	END
END

IF @SkipLastLookUp = 0
BEGIN
	SET @TempRecip = (SELECT TOP 1 WindowsUserID  + '@utahsbr.edu' AS EmailAddr 
					FROM dbo.GENR_REF_BU_Agent_Xref
					WHERE BusinessUnit = @NextSearch AND Role = 'Manager')
	/* put together last part of return string */
	IF @Recips = ''
	BEGIN
		IF @TempRecip <> '' AND @TempRecip IS NOT NULL 
		BEGIN
			SET @Recips = @TempRecip
		END
	END
	ELSE
	BEGIN
		IF @TempRecip <> '' AND @TempRecip IS NOT NULL 
		BEGIN
			SET @Recips = @Recips + ',' + @TempRecip
		END
	END
END

INSERT INTO @ReTbl VALUES (@Recips) 

SELECT * FROM @ReTbl