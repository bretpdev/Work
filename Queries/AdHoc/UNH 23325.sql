USE EmailTracking

GO


BEGIN TRANSACTION
       DECLARE @ERROR INT = 0
       DECLARE @ROWCOUNT INT = 0

              UPDATE EmailGroups set copied2 ='stgilmore@utahsbr.edu', copied5 ='rslagowski@utahsbr.edu'
              where Number = 9

       -- Save/Set the row count and error number (if any) from the previously executed statement
       SELECT @ROWCOUNT = @@ROWCOUNT, @ERROR = @@ERROR              
              
              UPDATE EmailGroups set copied4 ='Nothing'
              where Number = 25              

       -- Save/Set the row count and error number (if any) from the previously executed statement
       SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR

IF @ROWCOUNT = 2 AND @ERROR = 0
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


