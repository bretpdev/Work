USE NeedHelpUheaa
GO

BEGIN TRANSACTION
	DECLARE @ERROR INT = 0
	DECLARE @ROWCOUNT INT = 0

	UPDATE
		NeedHelpUheaa..DAT_Ticket
	SET
		History = 'Candice Cole - 08/09/2021 10:08 AM - Hold

AES was able to make a modification  to add the Sunday Datawarehouse refresh. The changes however, caused the Saturday morning run to be out of sync. The Warehouse ran 10 minutes prior to COMPASS finishing. 
AES believes this most likely would cause our data for Saturday to be incomplete. 

Jenny is was advised this should be orrected by the time the job runs again this weekend.

Candice Cole - 08/05/2021 03:30 PM - Hold

AES advised end of month could in fact have playe da part in the issue with volume. Their production support team is reviewing to compare what they found to be missing.

Candice Cole - 08/03/2021 08:54 AM - Hold

Sent file and update to AES.

Jeremy Blair - 08/03/2021 07:50 AM - Hold

Update from Jarom:
Saved another spreadsheet called Missing LT20 Records.xlsx in Q:\Support Services\Jarom\LT20 Issues\.  From yesterday there are 2,595 records that would have been missing if we did not have our reconciliation process.  The number actually seems really high so I will check the numbers again later today and maybe tomorrow.  Maybe month end is causing some slowness in AES loading LT20

Debbie Phillips - 08/02/2021 02:34 PM - Hold

Update from AES:

AES has identified the issue, in order to solve it, AES will be implementing a Sunday night Datawarehouse refresh. This change is scheduled to be implemented prior to this weekend. Would you like AES to refresh your DW today or have you already ran your processes and new data won''t be necessary?

@Deb responded that a warehouse update is not necessary because we already ran our work-around process.  We will look to see if the weekend changes help next Monday.

Debbie Phillips - 08/02/2021 08:25 AM - Hold

Just a reminder there is a daily work around to capture these and get them sent anyway.  We are just trying to trouble shoot with AES so we can remove the workaround.

Brenda Adams - 08/02/2021 08:16 AM - Hold

All Operations managers should be copied on this as they are affected by these missing letters more than Cindy.  Adding in the rest of the managers.  

Jeremy Blair - 08/02/2021 07:55 AM - Hold

CCC has been updated with this information.

Jeremy Blair - 08/02/2021 07:53 AM - Hold

Update from Jarom:
We had 3,228 letters missing today usually it is around 1,000 on Monday.  I will compare the missing ones today with what comes in tomorrow and then we can get a list of the missing letters for AES.  If you would like to see the missing letters for today I saved a spreadsheet to Q:\Support Services\Jarom\LT20 Issues\

Candice Cole - 07/30/2021 09:00 AM - Hold

Per email chain received from Jacob Zellers, AES is having DTS monitor our LT20 starting Monday 8/2/2021.  They us to make them aware ASAP if we see any issue w/the LT20 table.

AES would like us to provide the job and run time w/each issue that is found. We can reach out to Jenny/Jacob in the CCC 80416.

Candice Cole - 07/27/2021 10:10 AM - Hold

Ticket Placed On Hold: 

In our meeting w/Jenny Pottieger and Kate at AES they advised this issue has been escalated.  They have pulled all the data they need and it is being reviewed by the architects.  AES wanted to call out this is likely occuring for their other clients and it listed as a high priorty and while it seems like its not be reviewed its actively been worked on.  They wanted to call out that yes the issue would be resolved if all letters were on smart comm but as we have scripted letters that may never be included in smart comm they''ll continue to look into the issue until it has been resolved.  They do not have the specific ETA however, Jenny called out she is hoping to have something for us in the next week or two weeks.

Placing on hold.

Candice Cole - 07/16/2021 10:41 AM - Discussion

AES advised they''re still pulling together data from the weekend to analyze.

Candice Cole - 07/12/2021 08:11 AM - Discussion

Jenny conifrmed the pull of data from the LT20 and DW have matched for the past two days(which is what we have seen each week). She  expects to have a variance today or tomorrow. Once this occurs she said AES should then be able to doig deeper into the issue.

Candice Cole - 07/08/2021 08:20 AM - Discussion

AES was not ble to compare between the UT and DW data.They advised this occured becaye the LT20 load in our warehouse was a load/replace, however, as of yesterday AES will be saving the load file and recording the number of rows added to the DW to compare with the LT20 file from COMPASS. 

This should allow AES to find any discrepancies and review to determine the issue.

Jeremy Blair - 06/16/2021 10:54 AM - Discussion

Attached the records Jarom sent to the CCC.

Jarom Ryan - 06/14/2021 07:52 AM - Discussion

Attached are the records from last week

Candice Cole - 06/10/2021 08:24 AM - Discussion

Today jenny asked again for an updated report as of 6/10/21.  Can we please get an updated copy? Moving to Jarom to assign to a programmer.

Candice Cole - 06/04/2021 01:11 PM - Discussion

Per the call Jenny wanted to how cancel and how far back the report goes, and where data is pulling from which is the warehouse, So AES could start verifying the data daily and try to find out what is happening. What will show what''s happening by eliminating issues. I will follow up with her monday.



Candice Cole - 06/03/2021 12:55 PM - Discussion

Jenney responded to me and the meeting time on friday works for her, she has requested I add Zachary Munsch to the meeting.

Candice Cole - 06/02/2021 03:24 PM - Discussion

I''ve sent out a meeting invite for Friday and sent the updated reporting to AES. If we need to move the meeting time. Please let me know and I can reach out to AES.

Jarom Ryan - 06/02/2021 02:46 PM - Discussion

New V2 file attached

Jarom Ryan - 06/02/2021 08:41 AM - Discussion

Sorry for the delay, I will get the population attached today.

Candice Cole - 06/01/2021 12:36 PM - Discussion

Jarom AES would like us to provide an updated report like the one you provided last week, but for this week? We''re trying to schedule a call for Thursday or Friday to discuss.

Candice Cole - 06/01/2021 10:48 AM - Discussion

I''ve replied to Jenny asking for times for a quick call.

David Halladay - 05/25/2021 02:52 PM - Discussion

The CCC has been updated with your response and the attachment. 

Jarom Ryan - 05/25/2021 02:26 PM - Discussion

I do not think so, the vast majority have the printed response.  Attached is the details

Candice Cole - 05/25/2021 10:48 AM - Discussion

Jarom,  AES responded w/the following:

There does seem to be a day delay due to the weekend but usually catches up by Tuesday(except for this week) We have noted there are letters that are triggered, error, and then cancelled by what appears to be a script. Are those letters in your letter reconciliation report or do you exclude them?

Jeremy Blair - 05/25/2021 08:35 AM - Discussion

CCC has been updated and today''s Letter Reconciliation report has been sent to AES.

Jeremy Blair - 05/25/2021 08:30 AM - Discussion

Received an update from Jarom regariding LT20 this week:
"I noticed yesterday there was a large number of records in the letter reconciliation yesterday, it had been a while since there were that many (over 1500) but in the past it was not abnormal to have a higher volume on Monday.  For today’s report I noticed that we also had a high number again (4400) so I am wondering if we want to reach out to AES before we run the DCR to see if there is something on their end that is failing.  It seems like they might be behind because the records we added yesterday that were missing are now in the table today.  The ARC for yesterday’s missing population were left on 05/23 so it seems like they are 2 days behind.  Let me know if you have any questions or concerns, or if we want to move forward with the DCR without reaching out to AES."

Jeremy Blair - 05/25/2021 08:29 AM - Discussion

Releasing from hold: 



Candice Cole - 05/17/2021 08:59 AM - Hold

AES submitted a new SDT 801330 on 5/11. While the SDT is being worked, Jenny is continuing to research in OCS. Zach (their Datawarehouse BSSA) has been assisting her with some compares ofthe data. She believes at this point the issue is around the weekend runs and letters that error and are subsequently cancelled. I''ve asked for another update.

Debbie Phillips - 05/11/2021 12:38 PM - Hold

Please provide an update.

Debbie Phillips - 05/05/2021 08:19 AM - Hold

Please provide an update.

Candice Cole - 04/27/2021 01:21 PM - Hold

Received response from Jenny at AES she is going to do more research and get back to us. 

Jarom Ryan - 04/26/2021 11:59 AM - Hold

Candice I can confirm that letters were impacted on 04/19.  It appears to be most letters that were triggered on 04/18.  In some cases we had letters that did get added on 04/20 but not all of them did.  I have attached a spreadsheet of a few letters that were not added on 04/19, or 04/20.  I am not sure if it could be a scheduling issues because I have also attached a screenshot of the JAMS log indicating that the job ran multiple times each day with no errors.

Jarom Ryan - 04/26/2021 08:16 AM - Hold

Court changed from Jared Kieschnick to Jarom Ryan

Candice Cole - 04/23/2021 08:21 AM - Hold

AES finally found a discrepency, A query against the LT20s in the PUT region is looking for anything greater than 4/17 has both 4/17 and 4/19 LT20s(total of 5895 rows) while the same query against the warehouse has results of 4564. The difference between the two results is 1331, the number of LT20 records in PUT for 4/19. So, the 4/17 LT20s have loaded to the warehouse but not the 4/19. 

AES is working with their datawarehouse BSSA and they advised the warehouse updates are based on the completion of certain jobs and not a set schedule. The last time the warehouse refreshed was Sat Apr 17, so it would not have any of the 4/19 records.

Can we confirm the 4/19 is what was missing in our process? If so, this all appears to be a timing issue based on when the warehouse loads. 

I have attached the various query results for your reference.  

Also AES is also reviewing to see if the saw the 4/19 data populate on the 4/20 run.


Debbie Phillips - 04/23/2021 07:48 AM - Hold

Where are we with this?  Please provide an update.

Wendy Hack - 04/15/2021 10:25 AM - Hold

AES updated the CCC issue on 4/6 with the following. 

We have pulled a number of rows for the LT20 from The PUT region and your datawarehouse. All days have matched including Monday of this week.


Candice Cole - 04/06/2021 12:05 PM - Hold

Requested an update

Candice Cole - 04/01/2021 03:17 PM - Hold

Ticket Placed On Hold: 

AES has compared the rows daily and all are matching up they are waiting for our weekend to hit since this seems to be when we see this issue occur.  They will let me know what they determine on Monday .

Candice Cole - 03/25/2021 12:41 PM - Discussion

AES is still follow up with production support I''ll follow up on Monday if nothing received by tomorrow. 

Debbie Phillips - 03/25/2021 11:01 AM - Discussion

Please provide an update.

Debbie Phillips - 03/17/2021 02:21 PM - Discussion

Updates from the CCC - 

[03/17/21 03:58 PM] Jenny Potteiger - Candice, I have asked for a compare to be completed on Sunday''s data from this upcoming weekend. I will follow up on Monday with the team to see what was found.

[03/17/21 10:19 AM] Andrea Gaylor - Adding email for updates.

[03/16/21 08:25 PM] Jenny Potteiger - Thank you Candice, that is exactly what I was trying to find out. Can you tell me any accounts in group of letter still missing from Sunday? Production support is unable to perform much research without newer examples.

[03/16/21 05:56 PM] UT02991 - Candice Cole -- They''re not invalid address accounts. As of yesterday we were missing 1300+ letters for the past week. 1302 of those were for Sunday. Only 342 letters are missing for Sunday (3-14) now. We don''t know if this is a large group of those accounts were late to make it into your remote servers possibly or not. This seems to happen almost every week. Several are missing as of Monday, then by the next day only a subset of those are missing?

[03/16/21 05:37 PM] Jenny Potteiger - Candice, do all of the 1314 letters you have as missing have INVAD in the activity for that letter?

[03/15/21 08:37 PM] Jenny Potteiger - Hi Candice, my supervisor just reached out to me on this, she will be escalating it again tomorrow.


Candice Cole - 03/15/2021 04:46 PM - Discussion

I requested an update from AES.

Debbie Phillips - 03/15/2021 09:13 AM - Discussion

Please provide an update

Candice Cole - 03/04/2021 10:28 AM - Discussion

Court changed from Jeremy Blair to Candice Cole

Candice Cole - 03/04/2021 10:27 AM - Discussion

Issue:
The LT20 table is having an issue where the data is not loading, it appears to occur on Monday''s where the Sunday data is not making into the system. I''m updating CCC 80416 with the new examples. I think AES is confusing the invalid address with this issue, however, the entire tables worth of data is usually missing which wouldn''t be linked to an address. 

Once the latest examples are attached I will update the ccc.'
	WHERE
		Ticket = 70755

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