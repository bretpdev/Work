--X. INSERT new systems
--X. INSERT systems replacing old systems
--X. UPDATE REF_Systems with new values
--X. DELETE deleted systems from UPDATE REF_Systems
--X. DELETE deleted and renamed systems from LST_System


USE [NeedHelpCornerStone]

GO

BEGIN TRANSACTION
       DECLARE @ERROR INT = X
       DECLARE @ROWCOUNT INT = X

	--X. INSERT new systems (X)
		INSERT 
			[dbo].[LST_System] ([System], [ValidForTicketType]) 
		VALUES 
			(N'Noble Harmony', N'GEN'),
			(N'Noble Maestro', N'GEN'),
			(N'Noble ShiftTrack', N'GEN'),
			(N'NeedHelp', N'GEN'),
			(N'JAMS', N'GEN'),
			(N'Imaging', N'GEN')
       -- Save/Set the row count and error number (if any) from the previously executed statement
       SELECT @ROWCOUNT = @@ROWCOUNT, @ERROR = @@ERROR


	--X. INSERT systems replacing old systems (X)
		INSERT 
			[dbo].[LST_System] ([System], [ValidForTicketType])
		VALUES 
			(N'Noble Composer', N'GEN'),
			(N'Compass - FED', N'FAR'),
			(N'Compass - FED', N'GEN'),
			(N' Live Agent', N'GEN'),
			(N'IVR � FED', N'GEN'),
			(N'MD', N'GEN')
	   -- Update the row count and error number (if any) from the previously executed statement
       SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR

	   
	--X. UPDATE REF_System with new values
		UPDATE [dbo].[REF_System] SET System = 'Noble Composer' WHERE System = 'Autodialer'
		SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR
		UPDATE [dbo].[REF_System] SET System = 'Compass - FED' WHERE System = 'Compass'
		SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR
		UPDATE [dbo].[REF_System] SET System = 'Live Agent' WHERE System = 'Email Tracking'
		SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR
		UPDATE [dbo].[REF_System] SET System = 'IVR - FED' WHERE System = 'IVR'
		SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR
		UPDATE [dbo].[REF_System] SET System = 'MD' WHERE	System = 'Maui DUDE'	
       SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR


	--X. DELETE deleted systems from REF_Systems
		DELETE FROM [dbo].[REF_System] WHERE System IN (
			'LCO'
			,'Letterwriter'
			,'New Century'
			,'OneLINK'
			)
       -- Update the row count and error number (if any) from the previously executed statement
       SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR


	--X. DELETE deleted and renamed systems from LST_System
		DELETE FROM [dbo].[LST_System] WHERE System IN (
			'Autodialer'
			,'Compass'
			,'Email Tracking'
			,'IVR'
			,'LCO'
			,'Letterwriter'
			,'Maui DUDE'
			,'New Century'
			,'OneLINK'
			)
       -- Update the row count and error number (if any) from the previously executed statement
       SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR


IF @ROWCOUNT = XXXX AND @ERROR = X
       BEGIN
              PRINT 'Transaction committed'
              COMMIT TRANSACTION
       END
ELSE
       BEGIN
              PRINT 'ROWCOUNT:  ' + CAST(@ROWCOUNT as VARCHAR(XX))
              PRINT 'ERROR:  ' + CAST(@ERROR as VARCHAR(XX))
              PRINT '!!!  Transaction NOT committed.  Contact a member of Application Development to have this error corrected.'
              ROLLBACK TRANSACTION
       END
