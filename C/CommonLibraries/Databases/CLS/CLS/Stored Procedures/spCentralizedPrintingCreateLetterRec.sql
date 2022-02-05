
CREATE PROCEDURE [dbo].[spCentralizedPrintingCreateLetterRec]

@LetterID			VARCHAR(10),
@AcctNum			VARCHAR(20),
@BU					INT,
@DomInd				BIT,
@SpecialHandling	BIT,
@LetterIds			VARCHAR(MAX),
@NewRecNum			BIGINT OUTPUT,
@BarcodeSeqNum		BIGINT OUTPUT

AS

--insert print record
INSERT INTO dbo.CentralizedPrintingLetter (LetterID, AccountNumber, BusinessUnitId, IsDomestic, Requested) 
			VALUES (@LetterID, @AcctNum, @BU, @DomInd, GETDATE())

--get ID
SET @NewRecNum =  @@Identity

IF @SpecialHandling = 1
BEGIN
--SPECIAL HANDLING
	--return the number of recs that fit into letter's identified batch
	SET @BarcodeSeqNum = (
					SELECT COUNT(*) AS BarcodeSeqNum
					FROM dbo.CentralizedPrintingLetter  A
					WHERE A.SeqNum <= @NewRecNum 
						AND A.Printed IS NULL
						AND A.LetterID = @LetterID
				)
END
ELSE
BEGIN
--NON SPECIAL HANDLING
	SET @BarcodeSeqNum = (
							SELECT COUNT(*) AS BarcodeSeqNum
							FROM dbo.CentralizedPrintingLetter  A
							WHERE A.SeqNum <= @NewRecNum 
								AND A.Printed IS NULL
								AND A.IsDomestic = @DomInd
								AND A.LetterID IN (SELECT * FROM dbo.SplitString(',', @LetterID))
						  )
END

--update state mail barcode seq number in record just created
UPDATE dbo.CentralizedPrintingLetter  SET StateMailBatchSeq = @BarcodeSeqNum WHERE SeqNum = @NewRecNum


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[spCentralizedPrintingCreateLetterRec] TO [db_executor]
    AS [dbo];



