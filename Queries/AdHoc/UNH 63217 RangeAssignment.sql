BEGIN TRANSACTION
	DECLARE @ERROR INT = 0
	DECLARE @ROWCOUNT INT = 0

	TRUNCATE TABLE BSYS.dbo.MABC_SSN_User_XRef
	--doesn't return a row count

	INSERT INTO BSYS.dbo.MABC_SSN_User_Xref(RangeID, UserID, WindowsUserName, SSNRangeBegin, SSNRangeEnd, Dept, UserID2) Values(1, 'UT02000','mpixton'   ,0,10,'Account Resolution','UT02000')
	SELECT @ROWCOUNT = @@ROWCOUNT, @ERROR = @@ERROR
	INSERT INTO BSYS.dbo.MABC_SSN_User_Xref(RangeID, UserID, WindowsUserName, SSNRangeBegin, SSNRangeEnd, Dept, UserID2) Values(2, 'UT01639','ejacobsen',11,20,'Account Resolution','UT01639')
	SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR
	INSERT INTO BSYS.dbo.MABC_SSN_User_Xref(RangeID, UserID, WindowsUserName, SSNRangeBegin, SSNRangeEnd, Dept, UserID2) Values(3, 'UT02087','dmathews' ,21,30,'Account Resolution','UT02087')
	SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR
	INSERT INTO BSYS.dbo.MABC_SSN_User_Xref(RangeID, UserID, WindowsUserName, SSNRangeBegin, SSNRangeEnd, Dept, UserID2) Values(4, 'UT01020','dking'    ,31,40,'Account Resolution','UT01020')
	SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR
	INSERT INTO BSYS.dbo.MABC_SSN_User_Xref(RangeID, UserID, WindowsUserName, SSNRangeBegin, SSNRangeEnd, Dept, UserID2) Values(5, 'UT02685','jclarke'  ,41,50,'Account Resolution','UT02685')
	SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR
	INSERT INTO BSYS.dbo.MABC_SSN_User_Xref(RangeID, UserID, WindowsUserName, SSNRangeBegin, SSNRangeEnd, Dept, UserID2) Values(6, 'UT00258','dburnham' ,51,60,'Account Resolution','UT00258')
	SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR
	INSERT INTO BSYS.dbo.MABC_SSN_User_Xref(RangeID, UserID, WindowsUserName, SSNRangeBegin, SSNRangeEnd, Dept, UserID2) Values(7, 'UT01602','dhalaliku',61,70,'Account Resolution','UT01602')
	SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR
	INSERT INTO BSYS.dbo.MABC_SSN_User_Xref(RangeID, UserID, WindowsUserName, SSNRangeBegin, SSNRangeEnd, Dept, UserID2) Values(8, 'UT01641','emorris'  ,71,80,'Account Resolution','UT01641')
	SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR
	INSERT INTO BSYS.dbo.MABC_SSN_User_Xref(RangeID, UserID, WindowsUserName, SSNRangeBegin, SSNRangeEnd, Dept, UserID2) Values(9, 'UT02442','bsmeltzer',81,90,'Account Resolution','UT02442')
	SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR
	INSERT INTO BSYS.dbo.MABC_SSN_User_Xref(RangeID, UserID, WindowsUserName, SSNRangeBegin, SSNRangeEnd, Dept, UserID2) Values(10,'UT02635','mbryant'  ,91,99,'Account Resolution','UT02635')
	SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR

IF @ROWCOUNT = 10 AND @ERROR = 0
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