USE NeedHelpUheaa
GO

BEGIN TRANSACTION
	DECLARE @ERROR INT = 0
	DECLARE @ROWCOUNT INT = 0

	UPDATE 
		NeedHelpUheaa.dbo.DAT_Ticket
	SET 
		Issue = 'NAME: ROBERT W HALLMAN     ACCT #: 47 9711 4377  This borrower had their ACH removed because the bank came back saying that they couldn''t find the account. He came over from ACS so there''s no form on file. Could we have your help looking into the files from ACS to grab the ACH info again and compare it with this email he sent saying that his bank info was correct and shouldn''t have been removed?   Routing: xxxxxxxxx  Acct: xxxxxx  He does not want to send in a new form and says that we should be able to fix this. If we can compare this to his ACH info that was transferred over and it''s right, can we re apply it? If it''s not right i''ll let him know.' 
		,History = 'Carlos Brandaris - 05/19/2016 11:08 AM - Discussion    Ticket Withdrawn:     Thank you. Withdrawing.    Debbie Phillips - 05/19/2016 10:59 AM - Discussion    The ACH info received upon transfer matches what is currently listed in the ticket.  However, a DCR will be submitted to remove that confidential information out of the ticket and attached as a separate file instead.  You will need to work with Brad if you wish to have the ACH re-added to the account.    Debbie Phillips - 05/19/2016 10:43 AM - Discussion    Reviewing.    Carlos Brandaris - 05/19/2016 10:40 AM - Discussion    Please is there any update?    Carlos Brandaris - 05/16/2016 03:51 PM - Discussion    Update?    Lynn Guymon - 05/06/2016 02:53 PM - Discussion    Changing Priority as borrower is an Escalation and we need to follow up with him.    Carlos Brandaris - 05/06/2016 09:29 AM - Discussion    Is there an update? Please. I need to call the borrower back.    Carlos Brandaris - 04/27/2016 09:52 AM - Discussion    Any update?    Darin Tea - 04/19/2016 12:27 PM - Discussion    Issue:  NAME: ROBERT W HALLMAN     ACCT #: 47 9711 4377  This borrower had their ACH removed because the bank came back saying that they couldn''t find the account. He came over from ACS so there''s no form on file. Could we have your help looking into the files from ACS to grab the ACH info again and compare it with this email he sent saying that his bank info was correct and shouldn''t have been removed?   Routing: XXXXXXXXX  Acct: XXXXXX  He does not want to send in a new form and says that we should be able to fix this. If we can compare this to his ACH info that was transferred over and it''s right, can we re apply it? If it''s not right i''ll let him know.'     
	WHERE 
		Ticket = 26959
	
	-- Save/Set the row count and error number (if any) from the previously executed statement
	SELECT @ROWCOUNT = @@ROWCOUNT, @ERROR = @@ERROR

IF @ROWCOUNT = 1 AND @ERROR = 0
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


