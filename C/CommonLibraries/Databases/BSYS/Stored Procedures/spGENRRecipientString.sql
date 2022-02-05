CREATE PROCEDURE [dbo].[spGENRRecipientString] 

@EmailID			NVARCHAR(100)

 AS

/*testing only will be passed in*/
/*
DECLARE @EmailID		NVARCHAR(100)

SET @EmailID = 'HPFinAdjCompass'
*/

/*local vars*/
DECLARE @ResultsParam	VARCHAR(8000)
DECLARE @ResultsTlb		TABLE	(	RecipString	VARCHAR(8000))
DECLARE @TempResultsTlb	TABLE	(	[ID]	INT IDENTITY (1,1),
						Recip	NVARCHAR(50))
DECLARE @Iterator		INT	

SET @Iterator = 0
SET @ResultsParam = ''

INSERT INTO @TempResultsTlb (Recip) 
		SELECT WinUName + '@utahsbr.edu' as Recip 
		FROM dbo.GENR_REF_MiscEmailNotif	
		WHERE TypeKey = @EmailID

/* build output parameter */
WHILE @Iterator < (SELECT COUNT(*) FROM @TempResultsTlb)
BEGIN
	SET @Iterator = @Iterator + 1
	IF @ResultsParam = ''
	BEGIN
		SET @ResultsParam = (SELECT Recip FROM @TempResultsTlb WHERE [ID] = @Iterator)
	END
	ELSE
	BEGIN
		SET @ResultsParam = @ResultsParam + ';' + (SELECT Recip FROM @TempResultsTlb WHERE [ID] = @Iterator)
	END
END

INSERT INTO @ResultsTlb VALUES (@ResultsParam)

/* Return data set */
SELECT RecipString FROM @ResultsTlb