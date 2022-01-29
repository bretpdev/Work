USE NobleCalls

GO

BEGIN TRANSACTION
	DECLARE @ERROR INT = 0
	DECLARE @ROWCOUNT INT = 0
	DECLARE @ExpectedCount INT = 11099

UPDATE
	AAP
SET
	AAP.ArcResponseCodeId = 1
FROM
	NobleCalls..NobleCallHistory NCH
	INNER JOIN ULS..ArcAddProcessing AAP
		ON AAP.ArcAddProcessingId = NCH.ArcAddProcessingId
	INNER JOIN UDW..PD10_PRS_NME PD10
		ON PD10.DF_SPE_ACC_ID = AAP.AccountNumber
	INNER JOIN UDW..AY10_BR_LON_ATY AY10
		ON AY10.LN_ATY_SEQ = AAP.LN_ATY_SEQ
		AND AY10.BF_SSN = PD10.DF_PRS_ID
	INNER JOIN UDW..AY20_ATY_TXT AY20
		ON AY20.BF_SSN = AY10.BF_SSN
		AND AY20.LN_ATY_SEQ = AY10.LN_ATY_SEQ
WHERE
	NCH.DispositionCode = 'RP'
	AND NCH.RegionId = 2 --UHEAA
	AND AY10.PF_REQ_ACT = 'DDPHN'
	AND AY10.PF_RSP_ACT = 'CNTCT'
	AND AY20.LX_ATY LIKE '%contact-refuse%'

	SELECT @ROWCOUNT = @@ROWCOUNT, @ERROR = @@ERROR

IF @ROWCOUNT = @ExpectedCount AND @ERROR = 0
	BEGIN
		PRINT 'Transaction committed'
		COMMIT TRANSACTION
		--ROLLBACK TRANSACTION
	END
ELSE
	BEGIN
		PRINT 'ROWCOUNT:  ' + CAST(@ROWCOUNT as VARCHAR(10))
		PRINT 'ERROR:  ' + CAST(@ERROR as VARCHAR(10))
		PRINT '!!!  Transaction NOT committed.  Contact a member of Application Development to have this error corrected.'
		ROLLBACK TRANSACTION
	END





  
  
 
  
