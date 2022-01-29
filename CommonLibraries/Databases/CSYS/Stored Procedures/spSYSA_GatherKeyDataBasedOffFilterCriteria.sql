CREATE PROCEDURE [dbo].[spSYSA_GatherKeyDataBasedOffFilterCriteria]
	-- Add the parameters for the stored procedure here
	@Application		VARCHAR(100) = '', 
	@Type				VARCHAR(20) = '',
	@KeyWord			VARCHAR(100) = '',
	@KeyWordFieldFilter	VARCHAR(20) = ''
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE	@Temp		TABLE (	[Application]	VARCHAR(100),
								Name			VARCHAR(100),
								[Type]			VARCHAR(20),
								[Description]	VARCHAR(8000))	
	DECLARE	@Temp2		TABLE (	[Application]	VARCHAR(100),
								Name			VARCHAR(100),
								[Type]			VARCHAR(20),
								[Description]	VARCHAR(8000))		

	INSERT INTO @Temp
	SELECT [Application],
			UserKey as Name,
			[Type],
			[Description]
	FROM dbo.SYSA_LST_UserKeys

	--apply filtering criteria

	--Application criteria
	IF (LEN(@Application) <> 0)
	BEGIN
		INSERT INTO @Temp2 SELECT * FROM @Temp WHERE [Application] = @Application
		DELETE  @Temp WHERE [Application] IS NOT NULL
		INSERT INTO @Temp SELECT * FROM @Temp2
		DELETE  @Temp2 WHERE [Application] IS NOT NULL
	END

	--Type criteria
	IF (LEN(@Type) <> 0)
	BEGIN
		INSERT INTO @Temp2 SELECT * FROM @Temp WHERE [Type] = @Type
		DELETE  @Temp WHERE [Application] IS NOT NULL
		INSERT INTO @Temp SELECT * FROM @Temp2
		DELETE  @Temp2 WHERE [Application] IS NOT NULL
	END

	--Key Word criteria
	IF (LEN(@KeyWord) > 0)
	BEGIN
		IF (@KeyWordFieldFilter = 'Key And Description')
		BEGIN
			INSERT INTO @Temp2 SELECT * FROM @Temp WHERE Name LIKE '%' + @KeyWord + '%' OR [Description] LIKE '%' + @KeyWord + '%'
		END
		ELSE IF (@KeyWordFieldFilter = 'Key')
		BEGIN
			INSERT INTO @Temp2 SELECT * FROM @Temp WHERE Name LIKE '%' + @KeyWord + '%'
		END
		ELSE --description
		BEGIN
			INSERT INTO @Temp2 SELECT * FROM @Temp WHERE [Description] LIKE '%' + @KeyWord + '%'
		END
		DELETE  @Temp WHERE [Application] IS NOT NULL
		INSERT INTO @Temp SELECT * FROM @Temp2
	END

	SELECT * FROM @Temp ORDER BY Name
	
END
