--Some letters that are marked as printed on CS doc processing for X/XX-X/X do not seem to have been sent to the printer. 
--Please null the printedAt date for all letters matching the following date-plus-letter combinations. They are all from either fedecorprt or ecorrslfed.

USE CDW
GO

BEGIN TRANSACTION
	DECLARE @ERROR INT = X
	DECLARE @ROWCOUNT INT = X
	DECLARE @ExpectedRowCount INT = XXXX

UPDATE
	CDW..LTXX_LTR_REQ_PRC
SET
	PrintedAt = NULL
WHERE
	--X/XX
	(
		RM_DSC_LTR_PRC IN ('TSXXBFXXXC','TSXXBDXXX','TSXXBIBR','TSXXBDDXX','TSXXBGLBX')
		AND DATEDIFF(DAY,PrintedAt,'XXXX-XX-XX') = X
	)
	OR --X/XX
	(
		RM_DSC_LTR_PRC IN ('TSXXBDDXX')
		AND DATEDIFF(DAY,PrintedAt,'XXXX-XX-XX') = X
	)
	OR --X/X
	(
		RM_DSC_LTR_PRC IN ('TSXXBGLBX')
		AND DATEDIFF(DAY,PrintedAt,'XXXX-XX-XX') = X
	)
--XXXX

	SELECT @ROWCOUNT = @@ROWCOUNT, @ERROR = @@ERROR


UPDATE 
	PP
SET 
	PP.PrintedAt = NULL
FROM
	CLS.[print].Letters L
	INNER JOIN CLS.[print].ScriptData SD
		ON L.LetterId = SD.LetterId
	INNER JOIN CLS.[print].PrintProcessing PP
		ON PP.ScriptDataId = SD.ScriptDataId
WHERE
	--X/XX
	(
		L.Letter IN ('LDAFCXFED','PLRPYMTFED','RPDISCFED')
		AND DATEDIFF(DAY,PP.PrintedAt,'XXXX-XX-XX') = X
	)
	OR --X/XX
	(
		L.Letter IN ('PLRPYMTFED')
		AND DATEDIFF(DAY,PP.PrintedAt,'XXXX-XX-XX') = X
	)
	OR --X/X
	(
		L.Letter IN ('CBPCNFFED','FORAPPREC','RPDISCFED')
		AND DATEDIFF(DAY,PP.PrintedAt,'XXXX-XX-XX') = X
	)
--XXX

	SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR


IF @ROWCOUNT = @ExpectedRowCount AND @ERROR = X
	BEGIN
		PRINT 'Transaction committed'
		COMMIT TRANSACTION
		--ROLLBACK TRANSACTION
	END
ELSE
	BEGIN
		PRINT 'ROWCOUNT:  ' + CAST(@ROWCOUNT AS VARCHAR(XX))
		PRINT 'ERROR:  ' + CAST(@ERROR AS VARCHAR(XX))
		PRINT 'Transaction NOT committed'
		ROLLBACK TRANSACTION
	END
