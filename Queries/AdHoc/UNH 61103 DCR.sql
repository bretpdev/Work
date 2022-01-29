USE ULS

GO

BEGIN TRANSACTION
	DECLARE @ERROR INT = 0
	DECLARE @ROWCOUNT INT = 0

UPDATE ULS.[print].Letters SET PagesPerDocument = 2	WHERE letter IN ('IBRDN ','NOTCHLS','PIFCLLTR','PIFLTR','REQRCVDUH','US06BDMP','US08B48TPD','US08B48TPQ','US09B160')
	SELECT @ROWCOUNT = @@ROWCOUNT, @ERROR = @@ERROR
UPDATE ULS.[print].Letters SET PagesPerDocument = 4	WHERE letter IN ('BILLSTDEL1','BILLSTDEL2','BILLSTDEL3','BILLSTDEL4','BILLSTDEL5','BILLSTDEL6','BILLSTDEL7','BILLSTDEL8','BILLTILP','LPPBILLST','RPFBILL','SEPLTR','TCHSTCVLT', 'US06BCAP', 'US06BD101',   'US06BF101',  'US06BF601',  'US06BFRPA',  'US08BDSBA7',   'US09B10CP', 'US09B10P')
	SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR
UPDATE ULS.[print].Letters SET PagesPerDocument = 6	WHERE letter IN ('CNGRTS','ECONDEF','RPDISCUH','TMPHRDFORB','UNEMPDEF' )
	SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR
UPDATE ULS.[print].Letters SET PagesPerDocument = 12 WHERE letter IN ('IBRCOVLTR')
	SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR
INSERT INTO ULS.[print].Letters(Letter,PagesPerDocument) VALUES ('US10OTCUR3', 6)
	SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR

	

IF @ROWCOUNT = 37 AND @ERROR = 0
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





  
  
 
  
