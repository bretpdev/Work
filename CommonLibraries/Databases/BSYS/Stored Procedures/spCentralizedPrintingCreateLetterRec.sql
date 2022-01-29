CREATE PROCEDURE [dbo].[spCentralizedPrintingCreateLetterRec]

@LetterID			VARCHAR(10),
@AcctNum			VARCHAR(20),
@BU					VARCHAR(50),
@DomInd				CHAR(1),
@SpecialHandling	BIT,
@LetterIds			VARCHAR(MAX),
@NewRecNum			BIGINT OUTPUT,
@BarcodeSeqNum		BIGINT OUTPUT

AS

--insert print record
INSERT INTO PRNT_DAT_Print (LetterID, AccountNumber, BusinessUnit, Domestic) 
			VALUES (@LetterID, @AcctNum, @BU, @DomInd)

--get ID
SET @NewRecNum =  @@Identity

IF @SpecialHandling = 1
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
	SET @BarcodeSeqNum = (
							SELECT COUNT(*) AS BarcodeSeqNum
							FROM PRNT_DAT_Print A
							WHERE A.SeqNum <= @NewRecNum 
								AND A.PrintDate IS NULL
								AND A.Domestic = @DomInd
								AND A.LetterID IN ('1098E','AMNSTYLBAL','BRKDWN','CLAIMPD','REJECT','RETURN','RECALL','UNATTFU','DATREDEF','DFNOPHN1','DFNOPHN2','DFNOPHN3','DFNOPHN4','DFNOPHN5','DFNOPHN6','DPACONF','DPACANO','DPACANP','ULDSBDLCO3','ULDSBDLDF3','FBSDOC','DFORBDEN','AMNHIBAL','AMNSTYJD','SOINJSPREL','SOISFR','SOISPR','PIF','ULDSBDLCO2','ULDSBDLDF2','ULDSBDLCO4','ULDSBDLDF4','ULDSBDLPIF','ULDSBOFF1','MANUAL','DIJS','PIFTC','DLBILL','AWGRA','AWGRAR','CURRENT','ULGSBPRIV','ULDSBDLPFJ','RHB7PMTS','RHB5PMTS','RHB6PMTS','RHVERSC','RHVERCR','RHVERFA','REINAPVB','REINAPVS','REINDENB','REINDENS','REINREVB','OTHNOTSAT','VWANOTSAT','NOTSAT','PAYOFF','NSFDPA','SOFR','ENRVER','DSOP','SUPREJ','TLFDENNEL','TLFDOC','TLFTRANS','PMTINC','DELQ150','FRGN','DS01SCR','EMPVER','EMPVWA','AMNSTYRB','AMNSTY060','AMNSTY240','AMNSTY420','AMNSTY600')
						  )
END

--update state mail barcode seq number in record just created
UPDATE PRNT_DAT_Print SET StateMail2DDocSeqNum = @BarcodeSeqNum WHERE SeqNum = @NewRecNum