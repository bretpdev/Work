CREATE PROCEDURE dbo.spPRNT_CreateFaxRec

@FaxNum			VARCHAR(20),
@AcctNum			VARCHAR(20),
@BU				VARCHAR(50),
@LID				VARCHAR(10),
@CommentsAddedTo		VARCHAR(10),
@NewRecNum			BIGINT OUTPUT


AS

DECLARE @RecNum		BIGINT
DECLARE @PageNumber	NUMERIC
DECLARE @CostCenter		VARCHAR(6)

--insert print record
INSERT INTO PRNT_DAT_Fax(FaxNumber, AccountNumber, BusinessUnit, LetterID, CommentsAddedTo) 
			VALUES (@FaxNum, @AcctNum, @BU, @LID, @CommentsAddedTo)

--get ID
SET @NewRecNum =  @@Identity