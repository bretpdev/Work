CREATE PROCEDURE dbo.spPRNT_GetQCErrorConditions 

@SeqNum				BIGINT,
@WhatFor				VARCHAR(10) = 'Printing'

AS

DECLARE @LID			VARCHAR(20)

IF @WhatFor = 'Printing'
BEGIN
	SET @LID = (SELECT TOP 1 LetterID
			FROM dbo.PRNT_DAT_Print A
			INNER JOIN dbo.PRNT_DAT_PrintingErrors B
				ON A.SeqNum = B.PrintSeqNum
			WHERE A.SeqNum = @SeqNum) 
	
	SELECT COUNT(*) as TheCount
	FROM dbo.PRNT_DAT_Print A
	INNER JOIN dbo.PRNT_DAT_PrintingErrors B
		ON A.SeqNum = B.PrintSeqNum
	WHERE DATEDIFF(wk,A.RequestedDate,GetDate()) = 0 AND A.LetterID = @LID
END
ELSE
BEGIN
	SET @LID = (SELECT TOP 1 LetterID
			FROM dbo.PRNT_DAT_Fax A
			INNER JOIN dbo.PRNT_DAT_FaxingErrors B
				ON A.SeqNum = B.FaxSeqNum
			WHERE A.SeqNum = @SeqNum) 
	
	SELECT COUNT(*) as TheCount
	FROM dbo.PRNT_DAT_Fax A
	INNER JOIN dbo.PRNT_DAT_FaxingErrors B
		ON A.SeqNum = B.FaxSeqNum
	WHERE DATEDIFF(wk,A.RequestedDate,GetDate()) = 0 AND A.LetterID = @LID
END