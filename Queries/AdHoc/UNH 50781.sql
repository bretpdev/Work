USE BSYS
GO

BEGIN TRANSACTION
	DECLARE @ERROR INT = 0
	DECLARE @ROWCOUNT INT = 0
	DECLARE @ExpectedRowCount INT = 28

	--SELECT * FROM BSYS.dbo.MABC_SSN_User_XRef;--14
	
	DELETE FROM dbo.MABC_SSN_User_XRef;--14
	
	-- Save/Set the row count and error number (if any) from the previously executed statement
	SELECT @ROWCOUNT = @@ROWCOUNT, @ERROR = @@ERROR

	INSERT INTO dbo.MABC_SSN_User_XRef 
		(RangeID,UserID,WindowsUserName,SSNRangeBegin,SSNRangeEnd,Dept,UserID2)
	VALUES	
		 (1,'UT01487','jisom',0,6,'Account Resolution','UT01488')
		,(2,'UT01173','wwoodard',7,13,'Account Resolution','UT01174')
		,(3,'UT01020','cking',14,21,'Account Resolution','UT01021')
		,(4,'UT01602','dlino',22,28,'Account Resolution','UT01603')
		,(5,'UT01100','jbarrett',29,35,'Account Resolution','UT01101')
		,(6,'UT01606','cvillasmil',36,42,'Account Resolution','UT01607')
		,(7,'UT00685','ekamibayashi',43,49,'Account Resolution','UT00686')
		,(8,'UT01641','emorris',50,57,'Account Resolution','UT01642')
		,(9,'UT01603','dlino',58,65,'Account Resolution','UT01602')
		,(10,'UT01276','ekunz',66,72,'Account Resolution','UT01277')
		,(11,'UT01639','ejacobsen',73,79,'Account Resolution','UT01640')
		,(12,'UT00258','dburnham',80,86,'Account Resolution','UT00538')
		,(13,'UT01604','rnebeker',87,92,'Account Resolution','UT01605')
		,(14,'UT00434','pellis',93,99,'Account Resolution','UT00663')
		
	-- Save/Set the row count and error number (if any) from the previously executed statement
	SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR

IF @ROWCOUNT = @ExpectedRowCount AND @ERROR = 0
	BEGIN
		PRINT 'Transaction committed'
		COMMIT TRANSACTION
		--ROLLBACK TRANSACTION
	END
ELSE
	BEGIN
		PRINT 'ROWCOUNT:  ' + CAST(@ROWCOUNT as VARCHAR(10))
		PRINT 'ERROR:  ' + CAST(@ERROR as VARCHAR(10))
		PRINT 'Transaction NOT committed'
		ROLLBACK TRANSACTION
	END


