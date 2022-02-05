CREATE PROCEDURE dbo.spPRNT_GetCostCenterCvrShtDat 

@CostCenter					VARCHAR(10),
@Standard					BIGINT,
@Foreign					BIGINT,
@PagesPerDoc					INT,
@SpecialInstructions				VARCHAR(1000),
@LetterID					VARCHAR(50) = 'NA'

AS

--FOR TESTING
/*
DECLARE		@CostCenter		VARCHAR(10)
DECLARE 		@Standard 		BIGINT
DECLARE 		@Foreign		BIGINT	
DECLARE		@PagesPerDoc		INT
DECLARE		@SpecialInstructions	VARCHAR(1000)


SET @CostCenter = 'MA2324'
SET @Standard = 300
SET @Foreign = 0
SET @PagesPerDoc = 2
SET @SpecialInstructions = 'just mail the stuff'
*/
--END OF FOR TESTING

--LOCAL VARS
DECLARE		@IDCounter		BIGINT
DECLARE		@MissingSeqNums	VARCHAR(8000)
DECLARE		@Dat			TABLE (
							[ID]		BIGINT IDENTITY(1,1),
							SeqNum	BIGINT
							)

--GET RECORDS FROM ERROR TABLE
INSERT INTO @Dat (SeqNum) SELECT DISTINCT B.StateMail2DDocSeqNum 
			  FROM dbo.PRNT_DAT_PrintingErrors A
			  JOIN dbo.PRNT_DAT_Print B
				ON B.SeqNum = A.PrintSeqNum
			  WHERE A.ErrorPrinted IS NULL

--INIT VAR TO BLANK STRING
SET @MissingSeqNums = ''

--CALCULATE ACTUAL NUMBER OF DOCS (ORIGINAL TOTAL - ERRORED DOCUMENTS)
IF @Standard > 0 
BEGIN
	SET @Standard = @Standard - (SELECT COALESCE(MAX([ID]),0) FROM @Dat)
END
ELSE
BEGIN
	SET @Foreign = @Foreign - (SELECT COALESCE(MAX([ID]),0) FROM @Dat)
END

IF (SELECT COUNT(*) FROM @Dat) > 0 
BEGIN
	SET @IDCounter = 1
	--CREATE COMMA DELIMITED STRING OF ERRORED DOCS  
	WHILE @IDCounter <= (SELECT MAX([ID]) FROM @Dat)
	BEGIN
		IF @MissingSeqNums = ''
		BEGIN
			SET @MissingSeqNums = CAST((SELECT SeqNum FROM @DAT WHERE [ID] = @IDCounter) AS VARCHAR(10)) 		
		END
		ELSE
		BEGIN
			SET @MissingSeqNums = @MissingSeqNums + ',' + CAST((SELECT SeqNum FROM @DAT WHERE [ID] = @IDCounter) AS VARCHAR(10))		
		END
		SET @IDCounter = @IDCounter + 1
	END
END 

SELECT @CostCenter as CostCenter, 
	@Standard as Standard,
	@Foreign as 'Foreign',
	@PagesPerDoc as PagesPerDoc,
	@SpecialInstructions as SpecialInstructions,
	@MissingSeqNums as MissingSeqNums,
	@LetterID as LetterID