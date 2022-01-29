USE NeedHelpCornerStone
GO

BEGIN TRANSACTION
	DECLARE @ERROR INT = 0
	DECLARE @ROWCOUNT INT = 0

	UPDATE
		DAT_Ticket
	SET
		History = 'J Refugio Nolasco - 08/05/2016 08:08 AM - In Progress

This borrower is being picked up because he has two other loans with valid disqualification dates:

Borr_SSN      Borr_Benfit_Cde   Disqual_Date    Loan_Ident 
XXXXX8443        0800                  083011          ''LOA001144327 
XXXXX8443        0800                  083011          ''LOA001632229 
XXXXX8443        2565                  000000          ''LOA002218539 
XXXXX8443        2580                  000000          ''LOA002951582 

The query picks up his social and matches it to the whatever data exists in Duster. What query parameters should be used to exclude this type of borrower?

Jesse Gutierrez - 08/04/2016 03:26 PM - In Progress

The query is listing the following loans for account 5119685826:
Ln Seq 3 (Sup File''s Loan Ident / LN10.LF_GTR_RFR_XTN = LOA002218539)
Ln Seq 4 (Sup File''s Loan Ident / LN10.LF_GTR_RFR_XTN = LOA002951582) 

Both have an LC_BBS_ELG = ''Y'', however both loans have a Disqualifcation Date of 0 in the supplemental file and should be excluded.  

J Refugio Nolasco - 08/04/2016 01:23 PM - In Progress

Updated query as requested. Please see UNH_27479_Population_12.csv for updated output for Population 1.  Please note that only Population 1 returned any results. Populations 2 & 3 did not yield any hits, so there is no attachment for them.

Jesse Gutierrez - 08/04/2016 01:01 PM - In Progress

The population is displaying loans that shouldn''t be displayed.  After further review, the query is selecting both active and inactive LN54 rows.  Please update it so it only uses the active rows (LC_STA_LN54 = ''A'').  



J Refugio Nolasco - 08/03/2016 02:57 PM - In Progress

See updated CSV file UNH_27249_Popultion_11.csv for updated output for Population 1. Please note that only Population 1 returned any results. Populations 2 & 3 did not yield any hits, so there is no attachment for them.

Marc Titcomb - 08/03/2016 11:41 AM - In Progress

Hey JR, I missed an extra requirement. Please see "Updated Quick Query - BANA BBP.docx" and run it again.

Thanks!

J Refugio Nolasco - 08/01/2016 02:40 PM - In Progress

Attached please find the three requested output data sets. Please note that the Population 2 query yielded no hits.

Jarom Ryan - 07/29/2016 02:12 PM - In Progress

Assigning to JR

Wendy Hack - 07/29/2016 02:07 PM - In Progress

Moving to Jarom to assign. 

Wendy Hack - 07/29/2016 02:07 PM - In Progress

Marc Titcomb - 07/20/2016 12:13 PM - Review

Issue:
We need to identify three populations associated with SR 4493 in cleaning up tiered borrower benefits on BANA accounts. Please see the attached requirements and use the following path in viewing the input files that are to be used: X:\Archive\BANA\UNH 27479.

Thanks!

'
	WHERE
		Ticket = 27479

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
