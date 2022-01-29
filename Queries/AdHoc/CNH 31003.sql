USE [CLS]
GO
/****** Object:  StoredProcedure [dbo].[spCentralizedPrintingCreateLetterRec]    Script Date: X/X/XXXX X:XX:XX PM ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO

ALTER PROCEDURE [dbo].[spCentralizedPrintingCreateLetterRec]

@LetterID			VARCHAR(XX),
@AcctNum			VARCHAR(XX),
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

IF @SpecialHandling = X
BEGIN
--SPECIAL HANDLING
	--return the number of recs that fit into letter's identified batch
	SET @BarcodeSeqNum = (
					SELECT COUNT(*) AS BarcodeSeqNum
					FROM dbo.CentralizedPrintingLetter  A
					WHERE A.SeqNum <= @NewRecNum 
						AND A.PrintedAt IS NULL
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
								AND A.PrintedAt IS NULL
								AND A.IsDomestic = @DomInd
								AND A.LetterID IN (SELECT * FROM dbo.SplitString(',', @LetterID))
						  )
END

--update state mail barcode seq number in record just created
UPDATE dbo.CentralizedPrintingLetter  SET StateMailBatchSeq = @BarcodeSeqNum WHERE SeqNum = @NewRecNum

