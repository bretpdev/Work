USE NeedHelpUheaa
GO

BEGIN TRANSACTION
	DECLARE @ERROR INT = 0
	DECLARE @ROWCOUNT INT = 0

	UPDATE
		DAT_Ticket
	SET
		History = 'David Halladay - 08/05/2016 09:49 AM - In Progress

Josh, I''m not sure how the table looks, but it seems like there may be historical data. Is it possible to limit the output to the most recent LC_GRN record? Or remove all borrowers who have a LC_GRN = 03 after they had a status of LC_GRN = 06 or 07?

Rose Amador - 08/04/2016 03:33 PM - In Progress

David, we have been reviewing the results and the query returned more accounts than we were expecting. Would it be possible to have it rerun, but exclude accounts that have a 03 transfer status (subrogation) on LC05? 

Ross Westwater - 08/01/2016 10:59 AM - In Progress

It looks like this (like the SCRA query) pulled all accounts that have ever had a judgement filed against them. I''ve filtered it to 3161 individual accounts AFTER removing those that were on Becky/Amanda''s list. We may need to be more specific. It was also scanned on an individual loan basis, so it may be handy if those identical acct nums for individual loans be merged together to form a single balance number. Most have been subrogated, from what I can tell (just from 5 random samples).

David Halladay - 07/22/2016 04:15 PM - In Progress

Moving to Rose to review. Rose, please see UNH 27921.xlsx

Joshua Wright - 07/22/2016 04:09 PM - In Progress

See attached.

Eric Barnes - 07/22/2016 11:51 AM - In Progress

Assigning to Josh Wright to complete.

David Halladay - 07/22/2016 11:16 AM - In Progress

Let''s try searching for that field on DC01. It says it''s a field on DC01, but that it references DC10. 

Please use the same information except Judgement status
DC01_LON_CLM_INF
DC01.LC_GRN = 06 or 07

If that doesn''t work we''ll have to add DC10 to our onelink DW.

Rose Amador - 07/22/2016 09:25 AM - In Progress

David, I was just wondering if you know what the next steps would be on this? Not sure if there would be another way to query, or if we need to request the access? Thanks!

Eric Barnes - 07/19/2016 09:55 AM - In Progress

We don''t have access to query the DC10 table.  A request will need to be put in to grant access, we''ll need to get that data from a different field, or else we can just exclude that portion from the query.

David Halladay - 07/19/2016 09:24 AM - In Progress

Defaulted loans
DC01_LON_CLM_INF
DC01.LC_STA_DC10 = 03

Judgement status
DC10_LON_CLM
DC10.LC_GARN = 06 or 07

Account Number
PD01_PDM_INF
PD01.DF_SPE_ACC_ID

Balance
GA10_LON_APP
GA10.AA_CUR_PRI 


Rose Amador - 07/12/2016 01:32 PM - Review

Good afternoon. I just wanted to see if I could get an update on this ticket? Thanks!

Rose Amador - 06/28/2016 04:46 PM - Review

Issue:
Collections needs to have a quick query run to identify any defaulted accounts with a judgment status. I believe the fields we need to query appear on Page 2 of 3 in LC05, and the statuses we are looking for are 06 (Judicial Loan) and 07 (Judicial Garnishment). If we could get the borrower''s account number and loan balance in the query that would be helpful. We do want to include accounts with $0.00 balances as well. I have attached screen shots of the fields we are looking for. 

',
		[Status] = 'In Progress',
		PreviousStatus = 'Review'
	WHERE
		Ticket = 27921

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
