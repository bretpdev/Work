--1. INSERT new systems
--2. INSERT systems replacing old systems
--3. UPDATE REF_Systems with new values
--4. DELETE deleted systems from UPDATE REF_Systems
--5. DELETE deleted and renamed systems from LST_System


USE [NeedHelpUheaa]

GO

BEGIN TRANSACTION
       DECLARE @ERROR INT = 0
       DECLARE @ROWCOUNT INT = 0

	--1. INSERT new systems (6)
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


	--2. INSERT systems replacing old systems (6)
		INSERT 
			[dbo].[LST_System] ([System], [ValidForTicketType])
		VALUES 
			(N'Noble Composer', N'GEN'),
			(N'Compass - UHEAA', N'FAR'),
			(N'Compass - UHEAA', N'GEN'),
			(N' Live Agent', N'GEN'),
			(N'IVR – UHEAA', N'GEN'),
			(N'MD', N'GEN')
	   -- Update the row count and error number (if any) from the previously executed statement
       SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR

	   
	--3. UPDATE REF_Systems with new values
		UPDATE [dbo].[REF_System] SET System = 'Noble Composer' WHERE System = 'Autodialer'
		SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR
		UPDATE [dbo].[REF_System] SET System = 'Compass - UHEAA' WHERE System = 'Compass'
		SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR
		UPDATE [dbo].[REF_System] SET System = 'Live Agent' WHERE System = 'Email Tracking'
		SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR
		UPDATE [dbo].[REF_System] SET System = 'IVR - UHEAA' WHERE System = 'IVR'
		SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR
		UPDATE [dbo].[REF_System] SET System = 'MD' WHERE	System = 'Maui DUDE'	
       SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR


	--4. DELETE deleted systems from REF_Systems
		DELETE FROM [dbo].[REF_System] WHERE System IN (
			'LCO'
			,'Letterwriter'
			,'New Century'
			)
       -- Update the row count and error number (if any) from the previously executed statement
       SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR


	--5. DELETE deleted and renamed systems from LST_System
		DELETE FROM [dbo].[LST_System] WHERE System IN (
			'Autodialer'
			,'Compass'
			,'Email Tracking'
			,'IVR'
			,'LCO'
			,'Letterwriter'
			,'Maui DUDE'
			,'New Century'
			)
       -- Update the row count and error number (if any) from the previously executed statement
       SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR


IF @ROWCOUNT = 6689 AND @ERROR = 0
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
