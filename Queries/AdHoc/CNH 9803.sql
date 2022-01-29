/*for use in relation with SCRA CR XXXX (see CNH XXXX)*/
USE CLS
GO

DROP PROCEDURE [dbo].[PopulateScraTables_CRXXXX]

--DROP TABLE [scra].[Data_CRXXXX];
--DROP TABLE [scra].[Borrowers_CRXXXX];
--DROP TABLE [scra].[BenefitSource_CRXXXX];
--DROP TABLE [scra].[ActiveDuty_CRXXXX];

--BEGIN TRANSACTION
--	DECLARE @ERROR INT = X
--	DECLARE @ROWCOUNT INT = X

--	DELETE FROM  [scra].[Data_CRXXXX];
--	--X
	
--	-- Save/Set the row count and error number (if any) from the previously executed statement
--	SELECT @ROWCOUNT = @@ROWCOUNT, @ERROR = @@ERROR

--	DELETE FROM [scra].[Borrowers_CRXXXX]; 
--	--XXX
--	--XXX OPSDEV
		
--	-- Save/Set the row count and error number (if any) from the previously executed statement
--	SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR
	
--	DELETE FROM [CLS].[scra].[ActiveDuty_CRXXXX];
--	--XXX
--	--XXX OPSDEV

--	-- Save/Set the row count and error number (if any) from the previously executed statement
--	SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR

----IF @ROWCOUNT = XXXX AND @ERROR = X --OPSDEV
--IF @ROWCOUNT = XXX AND @ERROR = X
--	BEGIN
--		PRINT 'Transaction committed'
--		COMMIT TRANSACTION
--		--ROLLBACK TRANSACTION
--	END
--ELSE
--	BEGIN
--		PRINT 'ROWCOUNT:  ' + CAST(@ROWCOUNT as VARCHAR(XX))
--		PRINT 'ERROR:  ' + CAST(@ERROR as VARCHAR(XX))
--		PRINT 'Transaction NOT committed'
--		ROLLBACK TRANSACTION
--	END
