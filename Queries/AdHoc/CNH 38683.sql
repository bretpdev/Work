USE NeedHelpCornerStone
GO

BEGIN TRANSACTION
	DECLARE @ERROR INT = X
	DECLARE @ROWCOUNT INT = X

	UPDATE
		NeedHelpCornerStone..DAT_Ticket
	SET
		PreviousStatus = 'Submitting',
		[Status] = 'Discussion',
		History = 'Ty Jensen - XX/XX/XXXX XX:XX AM - Discussion

	The update forms for Rachel, Kryshelle and Elizabeth have been uploaded.

	Nghia Nguyen - XX/XX/XXXX XX:XX PM - Discussion

	Davis, Please review the following account

	RACHEL Q REYES                 	XX XXXX XXXX
	I added seqn X, Seqn X is giving me an error. Forgiveness was given before last disbursement date. Is this correct? 

	KRYSHELLE S COTHRAN      	XX XXXX XXXX
	The forgiveness for this is $XXXXX, but the borrower only teach regular secondary school. Please review. 

	ELIZABETH A SIEMERS         	XX XXXX XXXX
	I added seqn X, Seqn X is giving me an error. Forgiveness was given before last disbursement date. Is this correct? 

	Nghia Nguyen - XX/XX/XXXX XX:XX PM - Discussion

	Updated NSLDS for these users but a few exceptions.

	Candice Cole - XX/XX/XXXX XX:XX PM - Discussion

	Moving to Nghia, we''ll train on this tomorrow. 

	Davis Moon - XX/XX/XXXX XX:XX AM - Discussion

	Supervisor approved. 

	Issue:
	Please update the NSLDS TLF forgiveness amount for the following borrowers:

	AMBER N OVERSTREET       	XX XXXX XXXX
	BRANDON S KAISER             	XX XXXX XXXX
	CHAD C HARTER                  	XX XXXX XXXX
	DAVID W STOVER                	XX XXXX XXXX
	DEAN J IESUE                      	XX XXXX XXXX
	ELIZABETH A SIEMERS         	XX XXXX XXXX
	ERICA R BRADLEY                	XX XXXX XXXX
	ERIKA RODRIGUEZ               	XX XXXX XXXX
	EVALINDA ESTRADA             	XX XXXX XXXX
	EVER E LOPEZ                     	XX XXXX XXXX
	GAYLENE M EWING              	XX XXXX XXXX
	HEATHER D WILLIAMS         	XX XXXX XXXX
	JASON M BAGWELL              	XX XXXX XXXX
	JEREMY E LACY                   	XX XXXX XXXX
	JESSICA M PARFITT            	XX XXXX XXXX
	KRYSHELLE S COTHRAN      	XX XXXX XXXX
	MERLY SONI                        	XX XXXX XXXX
	MICHAEL J NECHUTA            	XX XXXX XXXX
	RACHEL Q REYES                 	XX XXXX XXXX
	RENEE J DEAL                     	XX XXXX XXXX
	ROBERT D VANDER LINDEN	XX XXXX XXXX
	SARA E DEFRANZA STILES  	XX XXXX XXXX
	SARAH K AMOS                   	XX XXXX XXXX
	STACY L MILLER                 	XX XXXX XXXX



	Ty Jensen - XX/XX/XXXX XX:XX PM - Submitting

	Court changed from Ty Jensen to Davis Moon'
	WHERE
		Ticket = XXXXX

		-- Save/Set the row count and error number (if any) from the previously executed statement
		SELECT @ROWCOUNT = @@ROWCOUNT, @ERROR = @@ERROR

	UPDATE
		NeedHelpCornerStone..DAT_Ticket
	SET
		PreviousStatus = 'Submitting',
		[Status] = 'Discussion',
		History = 'Candice Cole - XX/XX/XXXX XX:XX AM - Discussion

	Moving to Nghia to update.

	William Marsh - XX/XX/XXXX XX:XX PM - Discussion

	Issue:
	Please update the NSLDS TLF forgiveness amount for the following borrowers:

	AMY K BRAWLEY	XX XXXX XXXX


	Ty Jensen - XX/XX/XXXX XX:XX PM - Submitting

	Court changed from Ty Jensen to Davis Moon'
	WHERE
		Ticket = XXXXX

	SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR

IF @ROWCOUNT = X AND @ERROR = X
	BEGIN
		PRINT 'Transaction committed'
		COMMIT TRANSACTION
		--ROLLBACK TRANSACTION
	END
ELSE
	BEGIN
		PRINT 'ROWCOUNT:  ' + CAST(@ROWCOUNT as VARCHAR(XX))
		PRINT 'ERROR:  ' + CAST(@ERROR as VARCHAR(XX))
		PRINT 'Transaction NOT committed'
		ROLLBACK TRANSACTION
	END