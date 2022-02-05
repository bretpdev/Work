CREATE PROCEDURE dbo.spNDHP_CheckForDirectorLvlEmailCopy 

@BU				nvarchar(50),
@spRe				varchar(8000) OUTPUT

AS

SET @spRe = ''

DECLARE @TempRecip		nvarchar(50)
DECLARE DirectorEmailRecip CURSOR FOR 
	/* director email list */
	(SELECT * FROM NDHP_REF_DirectorEmailCopying)

/*check for level director email copying */
OPEN DirectorEmailRecip

/* iterate through query results */
FETCH NEXT FROM DirectorEmailRecip
INTO @TempRecip
WHILE @@FETCH_STATUS = 0 
BEGIN
	CREATE TABLE #TempBUs (BU nvarchar(50)) /* create temporary table */
	INSERT INTO #TempBUs exec spGENRWhoIsInChargeOfWho @TempRecip, 'N','Y'
	IF (SELECT COUNT(*) FROM #TempBUs WHERE BU = @BU) > 0
	BEGIN
		
		/* Create Recipient string */
		IF @spRe = ''
		BEGIN
			SET @spRe = @TempRecip + '@utahsbr.edu'
		END
		ELSE
		BEGIN
			SET @spRe = @spRe + ';' + @TempRecip + '@utahsbr.edu'
		END
	END
	/* get next record */
	FETCH NEXT FROM DirectorEmailRecip
	INTO @TempRecip
	DROP TABLE #TempBUs /* delete temporary table */
END
/* close cursor */
CLOSE DirectorEmailRecip
/* release memory */
DEALLOCATE DirectorEmailRecip