USE PLS

GO

BEGIN TRANSACTION
	DECLARE @ERROR INT = 0
	DECLARE @ROWCOUNT INT = 0
	DECLARE @ExpectedCount INT = 10

DELETE FROM PLS.crpqassign.Users WHERE UserName IN ('P616008','P613885')

SELECT @ROWCOUNT = @@ROWCOUNT, @ERROR = @@ERROR

INSERT INTO PLS.crpqassign.Users(UserName, AgentName)
VALUES
('P618557','Jaden Crane'), 
('P619140','Ashley Ogle'), 
('P618554','Chayanne Thomson'),
('P621240','Devin Bryner'),
('P616366','Jessica Tu'),
('P619139','Joy Radford'),
('P619013','Melissa Stettler'),
('P621388','Samantha Forrest')

SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR

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





  
  
 
  
