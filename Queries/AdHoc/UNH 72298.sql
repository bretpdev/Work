USE NeedHelpUheaa
GO

BEGIN TRANSACTION
	DECLARE @ERROR INT = 0
	DECLARE @ROWCOUNT INT = 0

	UPDATE
		NeedHelpUheaa..DAT_Ticket
	SET
		[Status] = 'In Progress',
		PreviousStatus = 'Review',
		History = 'David Halladay - 08/12/2021 02:06 PM - In Progress

	There was no response to the CCC. I have requested a follow-up from AES. 

	David Halladay - 08/04/2021 02:03 PM - In Progress

	This account has been added to the CCC.

	Cindy Oman - 07/30/2021 04:04 PM - In Progress

	Account 52 7482 1502 is a consolidation loan that was consolidated back on 10/14/16.  This adjustment will re-open the loans with a total balance of $60.48.  

	David Halladay - 07/21/2021 02:23 PM - In Progress

	Moving to Cindy. Please see AES update from 7/13. Are there any specific account we would like to have AES review?

	Wendy Hack - 07/13/2021 06:43 AM - In Progress

	AES updated the CCC issue. When you insert a backdated adjustment there are a variety of reasons that the balance can increase/decrease. A loan level review will be necessary to determine the exact cause of the increase. Some examples of why a loan balance can increase/decrease would be LPD setup changes, interest capping, etc.

	David Halladay - 07/07/2021 01:45 PM - In Progress

	CCC has been updated. 

	Wendy Hack - 07/02/2021 05:54 AM - In Progress

	Moving back to David to follow up with AES. 

	Cindy Oman - 07/01/2021 02:45 PM - In Progress

	Sorry for the delay.  In looking at the before/after there are 20 loans that increased.  If we are removing late fees why would a balance increase? 

	Wendy Hack - 06/30/2021 06:37 AM - In Progress

	AES is asking if you have an update on this review. 

	Jeremy Blair - 06/16/2021 11:46 AM - In Progress

	AES would like to know if we can close CCC 80834?

	Thank you!

	David Halladay - 05/25/2021 11:06 AM - In Progress

	AES provided the following response: 

	The before & after workbook shows the full difference between all the interest accrued and late fees outstanding between production and test, not just one transaction. This can be a little misleading when reviewing. If you utilize the population document, which includes the fee assessment date and fee amount that is being removed.

	David Halladay - 05/21/2021 09:23 AM - In Progress

	I have updated the CCC. 

	Cindy Oman - 05/20/2021 04:49 PM - In Progress

	I may be coming in on this late since I was not part of the cleanup in October 2020 or the promotion of LSFIN 3381 on 1/7/21.  So can you tell me if this is a late fee cleanup, why is all the 272 borrowers on the before late fee as 0 and after late fee as 0, but still have an adjustment on their loan balance?  

	David Halladay - 05/20/2021 12:10 PM - In Progress

	Moving to Cindy for review. 

	David Halladay - 05/20/2021 12:10 PM - In Progress

	David Halladay - 05/20/2021 12:10 PM - Review

	Issue:
	Received the following through CCC:

	As you are aware the Demand LSFIN-3381 was promoted 1/7/2021 to correct the coding issue where an adjustment is not created to remove a late fee when a fee is assessed to a loan where the remittance is subsequently applied having the same effective date as the late fee– the steps to resolve the impacted population are to insert a type 34 adjustment beginning the late effective date to remove the fee. We have tested the one time remediation similar to the reoccurring cleanup that started in October 2020 through the implementation of the code however UHEAA did not have loans impacted during the reoccurring cleanup period. We have completed the testing effort for the one-time UT population in quality region V2CH and I have attached the Before & After results for your review. I’ve also attached the impacted population document for assistance. Could you please review and provide a response prior to 5/31 if you agree with the results? Please let me know if there are any questions or concerns during your review. Thanks Rainie!'
	WHERE
		Ticket = 71506

	-- Save/Set the row count and error number (if any) from the previously executed statement
	SELECT @ROWCOUNT = @@ROWCOUNT, @ERROR = @@ERROR

	UPDATE
		NeedHelpUheaa..DAT_TicketsAssociatedUserID
	SET
		SqlUserId = 1519
	WHERE
		Ticket = 71506
		AND [Role] = 'Court'

	SELECT @ROWCOUNT += @@ROWCOUNT, @ERROR += @@ERROR

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