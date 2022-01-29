BEGIN TRANSACTION
	DECLARE @ERROR INT = X
	DECLARE @ROWCOUNT INT = X

INSERT INTO [CLS]..ArcAddProcessing
           ([ArcTypeId]
           ,[ArcResponseCodeId]
           ,[AccountNumber]
           ,[RecipientId]
           ,[ARC]
           ,[ScriptId]
           ,[ProcessOn]
           ,[Comment]
           ,[IsReference]
           ,[IsEndorser]
           ,[ProcessFrom]
           ,[ProcessTo]
           ,[NeededBy]
           ,[RegardsTo]
           ,[RegardsCode]
           ,[LN_ATY_SEQ]
           ,[ProcessingAttempts]
           ,[CreatedAt]
           ,[CreatedBy]
           ,[ProcessedAt])
SELECT
	X, --All loans
	NULL,
	PDXX.DF_SPE_ACC_ID,
	NULL,
	'MXXXX', --ARC
	'CNH XXXXX', -- Script Id
	GETDATE(), -- Process On
	'The pymt you submitted XX/X-XX/X is experiencing extended processing times We are working diligently to post the payment', -- Comment
	X,
	X,
	NULL, --Process From
	NULL,
	NULL,
	NULL,
	NULL,
	X, --LN_ATY_SEQ
	X, --Processing Attempts
	GETDATE(), --Created At
	SUSER_SNAME(), --Created By
	NULL
FROM
	CDW..PDXX_PRS_NME PDXX
WHERE
	PDXX.DF_PRS_ID IN ('XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX')

SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR

INSERT INTO [CLS]..ArcAddProcessing
           ([ArcTypeId]
           ,[ArcResponseCodeId]
           ,[AccountNumber]
           ,[RecipientId]
           ,[ARC]
           ,[ScriptId]
           ,[ProcessOn]
           ,[Comment]
           ,[IsReference]
           ,[IsEndorser]
           ,[ProcessFrom]
           ,[ProcessTo]
           ,[NeededBy]
           ,[RegardsTo]
           ,[RegardsCode]
           ,[LN_ATY_SEQ]
           ,[ProcessingAttempts]
           ,[CreatedAt]
           ,[CreatedBy]
           ,[ProcessedAt])
SELECT
	X, --All loans
	NULL,
	PDXX.DF_SPE_ACC_ID,
	NULL,
	'MXXXX', --ARC
	'CNH XXXXX', -- Script Id
	GETDATE(), -- Process On
	'The pymt you submitted XX/X-XX/X is experiencing extended processing times We are working diligently to post the payment', -- Comment
	X,
	X,
	NULL, --Process From
	NULL,
	NULL,
	NULL,
	NULL,
	X, --LN_ATY_SEQ
	X, --Processing Attempts
	GETDATE(), --Created At
	SUSER_SNAME(), --Created By
	NULL
FROM
	CDW..PDXX_PRS_NME PDXX
WHERE
	PDXX.DF_PRS_ID IN ('XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX')

SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR

INSERT INTO [CLS]..ArcAddProcessing
           ([ArcTypeId]
           ,[ArcResponseCodeId]
           ,[AccountNumber]
           ,[RecipientId]
           ,[ARC]
           ,[ScriptId]
           ,[ProcessOn]
           ,[Comment]
           ,[IsReference]
           ,[IsEndorser]
           ,[ProcessFrom]
           ,[ProcessTo]
           ,[NeededBy]
           ,[RegardsTo]
           ,[RegardsCode]
           ,[LN_ATY_SEQ]
           ,[ProcessingAttempts]
           ,[CreatedAt]
           ,[CreatedBy]
           ,[ProcessedAt])
SELECT
	X, --All loans
	NULL,
	PDXX.DF_SPE_ACC_ID,
	NULL,
	'MXXXX', --ARC
	'CNH XXXXX', -- Script Id
	GETDATE(), -- Process On
	'The pymt you submitted XX/X-XX/X is experiencing extended processing times We are working diligently to post the payment', -- Comment
	X,
	X,
	NULL, --Process From
	NULL,
	NULL,
	NULL,
	NULL,
	X, --LN_ATY_SEQ
	X, --Processing Attempts
	GETDATE(), --Created At
	SUSER_SNAME(), --Created By
	NULL
FROM
	CDW..PDXX_PRS_NME PDXX
WHERE
	PDXX.DF_PRS_ID IN ('XXXXXXXXX','XXXXXXXXX')

SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR

IF @ROWCOUNT = XXXX AND @ERROR = X
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
