CREATE PROCEDURE dbo.spPRNT_CreateLetterRec

@LetterID			VARCHAR(10),
@AcctNum			VARCHAR(20),
@BU				VARCHAR(50),
@Dom				VARCHAR(2),
@NewRecNum			BIGINT OUTPUT,
@BarcodeSeqNum		BIGINT OUTPUT


AS

DECLARE @DomInd			CHAR(1)
DECLARE @RecNum			BIGINT
DECLARE @PageNumber		NUMERIC
DECLARE @CostCenter			VARCHAR(6)
DECLARE @Duplex			BIT
--Testing only
--SET @LetterID = '1098E'
--SET @AcctNum = '123456789'
--SET @BU = 'Account Services'
--SET @Dom = 'UT'

--get state indicator value from state table
IF (SELECT COUNT(*) FROM GENR_LST_States WHERE CODE = @Dom) > 0
BEGIN
	SET @DomInd = (SELECT TOP 1 Domestic FROM GENR_LST_States WHERE CODE = @Dom)
END
ELSE
BEGIN
	SET @DomInd = NULL
END

--insert print record
INSERT INTO PRNT_DAT_Print (LetterID, AccountNumber, BusinessUnit, Domestic) 
			VALUES (@LetterID, @AcctNum, @BU, @DomInd)

--get ID
SET @NewRecNum =  @@Identity

--check if letter is special handling or not
IF (SELECT TOP 1 CASE 
		WHEN Instructions IS NULL or CAST(Instructions AS VARCHAR(50)) = '' THEN 'N' 
		ELSE 'Y' END as SH 
	FROM dbo.LTDB_DAT_CentralPrintingDocData
	WHERE ID = @LetterID) = 'Y'
BEGIN
--SPECIAL HANDLING
	--return the number of recs that fit into letter's identified batch
	SET @BarcodeSeqNum = (
					SELECT COUNT(*) AS BarcodeSeqNum
					FROM PRNT_DAT_Print A
					WHERE A.SeqNum <= @NewRecNum 
						AND A.PrintDate IS NULL
						AND A.LetterID = @LetterID
				)
END
ELSE
BEGIN
--NON SPECIAL HANDLING
	--get number of pages it will take to print the letter
	SET @PageNumber = (SELECT TOP 1 CASE
					WHEN Duplex = 1 THEN CEILING(Pages / 2)
					ELSE Pages
				     END AS PageNumber
			FROM LTDB_DAT_CentralPrintingDocData
			WHERE [ID] = @LetterID)
	
	--get cost center of letter
	SET @CostCenter = (SELECT TOP 1 
				     UHEAACostCenter AS CostCenter	
			FROM LTDB_DAT_CentralPrintingDocData
			WHERE [ID] = @LetterID)

	--check if letter is duplex or not
	SET @Duplex =  (SELECT TOP 1 Duplex	
			FROM LTDB_DAT_CentralPrintingDocData
			WHERE [ID] = @LetterID)
	
	IF @Duplex = 1 
	BEGIN
		--if the letter is duplex then
		--return the number of recs that fit into letter's identified batch
		SET @BarcodeSeqNum = (
						SELECT COUNT(*) AS BarcodeSeqNum
						FROM PRNT_DAT_Print A
						INNER JOIN (
								SELECT [ID]
								FROM LTDB_DAT_CentralPrintingDocData
								WHERE UHEAACostCenter = @CostCenter 
									AND Duplex = 1 
									AND CEILING(Pages / 2) = @PageNumber 
									AND (Instructions IS NULL OR CAST(Instructions AS VARCHAR(50)) = '') 
								) B ON A.LetterID = B.[ID]
						WHERE A.SeqNum <= @NewRecNum 
							AND A.PrintDate IS NULL
							AND A.Domestic = @DomInd
					)
	END
	ELSE
	BEGIN
		--if the letter is not duplex
		--return the number of recs that fit into letter's identified batch
		SET @BarcodeSeqNum = (
						SELECT COUNT(*) AS BarcodeSeqNum
						FROM PRNT_DAT_Print A
						INNER JOIN (
								SELECT [ID]
								FROM LTDB_DAT_CentralPrintingDocData
								WHERE UHEAACostCenter = @CostCenter
									AND Duplex = 0 
									AND Pages = @PageNumber
									AND (Instructions IS NULL OR CAST(Instructions AS VARCHAR(50)) = '') 
								) B ON A.LetterID = B.[ID]
						WHERE A.SeqNum <= @NewRecNum 
							AND A.PrintDate IS NULL
							AND A.Domestic = @DomInd
					)
	END
END

--update state mail barcode seq number in record just created
UPDATE PRNT_DAT_Print SET StateMail2DDocSeqNum = @BarcodeSeqNum WHERE SeqNum = @NewRecNum