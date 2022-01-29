




CREATE PROCEDURE [dbo].[spNDHP_OnecADayEmailAlert] 


AS

DECLARE @QCManager		VARCHAR(100)
DECLARE @BSManager		VARCHAR(100)
DECLARE @BOBManager		VARCHAR(100)
DECLARE @ASerSubManager		VARCHAR(100)
DECLARE @BSSubManager		VARCHAR(100)
DECLARE @ComplianceAll		VARCHAR(200)
DECLARE @SSManager		VARCHAR(100)
DECLARE @FinStaffOL			VARCHAR(8000)
DECLARE @FinStaffCO			VARCHAR(8000)
DECLARE @FullBody			VARCHAR(8000)
DECLARE @FullListOfEmailRecips	VARCHAR(8000)
DECLARE @Iterator			INT
DECLARE @Iterator2			INT
DECLARE @TotalTicketCount		INT
DECLARE @TotalCount			INT
DECLARE @SecondaryKey		VARCHAR(100)
DECLARE @AuthCompAgents		TABLE ([ID]			INT IDENTITY (1, 1),
						Agent		NVARCHAR(50))
DECLARE @BUTable			TABLE ([ID]			INT IDENTITY (1, 1),
						BU 			NVARCHAR(50))
DECLARE @SSStaffTable		TABLE ([ID]			INT IDENTITY (1, 1),
						WINID 			NVARCHAR(50))
DECLARE @EmailDat			TABLE ( [ID]			INT IDENTITY (1, 1),
						BodyPart 		VARCHAR(500), 
						TicketCode		CHAR(3),
						Manager		VARCHAR(100),
						Requester		VARCHAR(100),
						QCAnalyst		VARCHAR(100),
						SSAnalyst		VARCHAR(100),
						TheSystem		VARCHAR(100),
						Unit			NVARCHAR(50))
DECLARE @TempEmailDat		TABLE ( [ID]			INT IDENTITY (1, 1),
						BodyPart 		VARCHAR(500), 
						TicketCode		CHAR(3),
						Manager		VARCHAR(100),
						Requester		VARCHAR(100),
						QCAnalyst		VARCHAR(100),
						SSAnalyst		VARCHAR(100),
						TheSystem		VARCHAR(100),
						Unit			NVARCHAR(50))
DECLARE @Results			TABLE ( [ID]			INT IDENTITY (1, 1),
						Recipients		VARCHAR(8000),
						Subject			VARCHAR(500),
						Body			TEXT)

CREATE TABLE #RecipTemp (RecipString		VARCHAR(8000))

/* check if email reports have been sent out already */
IF (SELECT DATEDIFF(d,PriorityUpdated,GETDATE()) AS RE FROM dbo.NDHP_LST_PriorityUpdated) <> 0
BEGIN

	/* create data set if reports haven't been sent out */
	
	/*get ticket level info*/
	INSERT INTO @EmailDat
	SELECT '- HP ' + CAST(A.Ticket AS VARCHAR(10)) + ' ' + A.Subject AS BodyPart, 
		A.TicketCode, 
		B.WindowsUserID + '@utahsbr.edu' AS Manager,
		';' + CC.UserID + '@utahsbr.edu' AS Requester, 
		';' + C.WindowsUserID + '@utahsbr.edu' AS QCAnalyst, 
		CASE WHEN BB.UserID = '' OR BB.UserID IS NULL THEN '' ELSE ';' + BB.UserID + '@utahsbr.edu' END AS SSAnalyst, 
		CASE WHEN D.[System] = '' OR D.[System] IS NULL THEN '' ELSE D.[System] END TheSystem,
		A.UNIT
	FROM dbo.NDHP_DAT_Tickets A
	INNER JOIN (
			SELECT UserID, Ticket
			FROM dbo.NDHP_DAT_UpdateTicketUserIDs
			WHERE Role = 'AssignedTo' 
			) BB ON BB.Ticket = A.Ticket
	INNER JOIN (
			SELECT UserID, Ticket
			FROM dbo.NDHP_DAT_UpdateTicketUserIDs
			WHERE Role = 'Requester' 
			) CC ON CC.Ticket = A.Ticket
	JOIN (	SELECT WindowsUserID, BusinessUnit
		FROM dbo.GENR_REF_BU_Agent_Xref
		WHERE Role = 'Manager'
		) B ON A.Unit = B.BusinessUnit
	JOIN (	SELECT WindowsUserID, BusinessUnit
		FROM dbo.GENR_REF_BU_Agent_Xref
		WHERE Role = 'QC Analysis'
		) C ON A.Unit = C.BusinessUnit
	LEFT JOIN (
		/* only need one system result because fin adjustment tickets can only have one system and that is the purpose of this field addition */
		SELECT Ticket, MAX([System]) AS [System]
		FROM dbo.NDHP_REF_Systems
		GROUP BY Ticket
		) D ON A.Ticket = D.Ticket
	WHERE A.Priority IN (SELECT Priority FROM dbo.GENR_LST_Priorities WHERE Urgency = 'High') 
		AND Required <= GETDATE()
		AND Status NOT IN ('Resolved','Withdrawn')
	
	--for testing
	--SELECT * FROM @EmailDat
	
	  --get bs manager since that role is emailed for every ticket 
		--SET @BSManager = (SELECT WindowsUserID + '@utahsbr.edu' AS BSManager FROM dbo.GENR_REF_BU_Agent_Xref WHERE Role = 'Manager' AND BusinessUnit = 'BS') 
	
	 --get Business Operations Branch manager since that role is emailed for every ticket 
		--SET @BOBManager = (SELECT WindowsUserID + '@utahsbr.edu' AS BOBManager FROM dbo.GENR_REF_BU_Agent_Xref WHERE Role = 'Manager' AND BusinessUnit = 'Business Operations Branch') 
	
	 --get Account Services Subbranch Manager since that role is emailed for every ticket 
		--SET @ASerSubManager = (SELECT WindowsUserID + '@utahsbr.edu' AS ASerSubManager FROM dbo.GENR_REF_BU_Agent_Xref WHERE Role = 'Manager' AND BusinessUnit = 'Account Services Subbranch') 
	
	 --get Borrower Services Subbranch Manager since that role is emailed for every ticket 
		--SET @BSSubManager = (SELECT WindowsUserID + '@utahsbr.edu' AS BSSubManager FROM dbo.GENR_REF_BU_Agent_Xref WHERE Role = 'Manager' AND BusinessUnit = 'Borrower Services Subbranch') 
	

	 	/*NOTE: The first if statement can use 0 as the starting point for the iterators because the table
		//has nothing in it, but all other if statements delete what was in the table before hand and 
		//starts the iterators at the min of the table because the ID auto number continues to go up*/
	 
	
	
	/* check for Policy/Regulatory/Compliance tickets */
	IF (SELECT COUNT(*) FROM @EmailDat WHERE TicketCode = 'POL') > 0
	BEGIN
		/*get qc manager email address*/
		SET @QCManager = (SELECT WindowsUserID + '@utahsbr.edu' AS QCManager FROM dbo.GENR_REF_BU_Agent_Xref WHERE Role = 'Manager' AND BusinessUnit = 'Compliance') /*Quality Control*/
		

		/* create a table with a list of all BU's with a 'POL' ticket */ 

        /*Gets all agents of compliance on email list*/
		INSERT INTO @AuthCompAgents (Agent) (SELECT DISTINCT WindowsUserID FROM dbo.GENR_REF_BU_Agent_Xref WHERE BusinessUnit = 'Compliance' AND Role = 'Member Of')
		SET @TotalCount = (SELECT MAX([ID]) FROM @AuthCompAgents)
		SET @Iterator = 0
		SET @ComplianceAll = ''
		WHILE @Iterator < @TotalCount
		BEGIN
			SET @Iterator = @Iterator + 1
			IF (@Iterator = 1)
			BEGIN
				SET @ComplianceAll = (SELECT Agent FROM @AuthCompAgents WHERE [ID] = @Iterator) + '@utahsbr.edu' 
			END
			ELSE
			BEGIN
				SET @ComplianceAll = @ComplianceAll + ';' + (SELECT Agent FROM @AuthCompAgents WHERE [ID] = @Iterator) + '@utahsbr.edu' 
			END
		END	

		INSERT INTO @BUTable (BU) (SELECT DISTINCT Unit FROM @EmailDat WHERE TicketCode = 'POL')
		INSERT INTO @TempEmailDat (BodyPart, TicketCode, Manager, Requester, QCAnalyst, SSAnalyst, TheSystem, Unit) SELECT BodyPart, TicketCode, Manager, Requester, QCAnalyst, SSAnalyst, TheSystem, Unit FROM @EmailDat WHERE TicketCode = 'POL'
		/*get record totals for iterations*/
		SET @TotalTicketCount = (SELECT MAX([ID]) FROM @TempEmailDat)
		SET @TotalCount = (SELECT MAX([ID]) FROM @BUTable)
		SET @Iterator = 0
		/* iterate through records and create result record */
		WHILE @Iterator < @TotalCount
		BEGIN
			SET @Iterator = @Iterator + 1
			/* set up vars for inner loop */
			SET @FullBody = ''
			SET @FullListOfEmailRecips = ''
			SET @Iterator2 = 0
			SET @SecondaryKey = (SELECT BU FROM @BUTable WHERE [ID] = @Iterator)
			WHILE @Iterator2 < @TotalTicketCount
			BEGIN
				SET @Iterator2 = @Iterator2 + 1
				/*if the business units match then add data to working strings*/
				IF (SELECT UNIT FROM @TempEmailDat WHERE [ID] = @Iterator2) = @SecondaryKey
				BEGIN
					IF @FullBody = '' 
					BEGIN
						SET @FullBody = (SELECT BodyPart FROM @TempEmailDat WHERE [ID] = @Iterator2)
						SET @FullListOfEmailRecips = @BSSubManager + ';' + @ASerSubManager + ';' + @BOBManager + ';' + @BSManager + ';' + @QCManager + ';' + @ComplianceAll /* + (SELECT QCAnalyst FROM @TempEmailDat WHERE [ID] = @Iterator2)*/
					END
					ELSE
					BEGIN
						SET @FullBody = @FullBody + CHAR(13)  + CHAR(10) + (SELECT BodyPart FROM @TempEmailDat WHERE [ID] = @Iterator2)
					END
				END
			END
			/* add record to results table */
			INSERT INTO @Results (Subject, Body, Recipients) VALUES ('Emergency Policy/Regulatory/Compliance ticket(s) for ' + @SecondaryKey, @FullBody, @FullListOfEmailRecips)
		END
	END
	
	/* check for Special handling tickets */
	IF (SELECT COUNT(*) FROM @EmailDat WHERE TicketCode = 'SPH') > 0
	BEGIN
		/* create a table with a list of all BU's with a 'SPH' ticket */
		DELETE FROM @BUTable WHERE [ID] > 0
		INSERT INTO @BUTable (BU) (SELECT DISTINCT Unit FROM @EmailDat WHERE TicketCode = 'SPH')
		DELETE FROM @TempEmailDat WHERE [ID] > 0
		INSERT INTO @TempEmailDat (BodyPart, TicketCode, Manager, Requester, QCAnalyst, SSAnalyst, TheSystem, Unit) SELECT BodyPart, TicketCode, Manager, Requester, QCAnalyst, SSAnalyst, TheSystem, Unit FROM @EmailDat WHERE TicketCode = 'SPH'
		/*get record totals for iterations*/
		SET @TotalTicketCount = (SELECT MAX([ID]) FROM @TempEmailDat)
		SET @TotalCount = (SELECT MAX([ID]) FROM @BUTable)
		SET @Iterator = (SELECT MIN([ID]) FROM @BUTable)
		IF @Iterator > 0 BEGIN SET @Iterator = @Iterator - 1 END
		/* iterate through records and create result record */
		WHILE @Iterator < @TotalCount
		BEGIN
			SET @Iterator = @Iterator + 1
			/* set up vars for inner loop */
			SET @FullBody = ''
			SET @FullListOfEmailRecips = ''
			SET @Iterator2 = (SELECT MIN([ID]) FROM @TempEmailDat)
			IF @Iterator2 > 0 BEGIN SET @Iterator2 = @Iterator2 - 1 END
			SET @SecondaryKey = (SELECT BU FROM @BUTable WHERE [ID] = @Iterator)
			WHILE @Iterator2 < @TotalTicketCount
			BEGIN
				SET @Iterator2 = @Iterator2 + 1
				/*if the business units match then add data to working strings*/
				IF (SELECT UNIT FROM @TempEmailDat WHERE [ID] = @Iterator2) = @SecondaryKey
				BEGIN
					IF @FullBody = '' 
					BEGIN
						SET @FullBody = (SELECT BodyPart FROM @TempEmailDat WHERE [ID] = @Iterator2)
						SET @FullListOfEmailRecips = @BSSubManager + ';' + @ASerSubManager + ';' + @BOBManager + ';' + @BSManager + (SELECT Requester AS Requester FROM @TempEmailDat WHERE [ID] = @Iterator2) + (SELECT Manager AS Manager FROM @TempEmailDat WHERE [ID] = @Iterator2)
					END
					ELSE
					BEGIN
						SET @FullBody = @FullBody + CHAR(13)  + CHAR(10) + (SELECT BodyPart FROM @TempEmailDat WHERE [ID] = @Iterator2)
					END
				END
			END
			/* add record to results table */
			INSERT INTO @Results (Subject, Body, Recipients) VALUES ('Emergency Special Handling ticket(s) ' + @SecondaryKey, @FullBody, @FullListOfEmailRecips)
		END
	END
	
	/* check for system tickets */
	IF (SELECT COUNT(*) FROM @EmailDat WHERE TicketCode IN ('OTH','FNC','PRB')) > 0
	BEGIN
		/*get system support manager email address*/
		SET @SSManager = (SELECT WindowsUserID + '@utahsbr.edu' AS QCManager FROM dbo.GENR_REF_BU_Agent_Xref WHERE Role = 'Manager' AND BusinessUnit = 'Systems Support')
		/* create a table with a list of all BU's with a 'OTH' or 'FNC' or 'PRB' ticket */
		INSERT INTO @SSStaffTable (WINID) (SELECT DISTINCT SSAnalyst FROM @EmailDat WHERE TicketCode IN ('OTH','FNC','PRB'))
		DELETE FROM @TempEmailDat WHERE [ID] > 0	
		INSERT INTO @TempEmailDat (BodyPart, TicketCode, Manager, Requester, QCAnalyst, SSAnalyst, TheSystem, Unit) SELECT BodyPart, TicketCode, Manager, Requester, QCAnalyst, SSAnalyst, TheSystem, Unit FROM @EmailDat WHERE TicketCode IN ('OTH','FNC','PRB')
		/*get record totals for iterations*/
		SET @TotalTicketCount = (SELECT MAX([ID]) FROM @TempEmailDat)
		SET @TotalCount = (SELECT MAX([ID]) FROM @SSStaffTable)
		SET @Iterator = 0
		/* iterate through records and create result record */
		WHILE @Iterator < @TotalCount
		BEGIN
			SET @Iterator = @Iterator + 1
			/* set up vars for inner loop */
			SET @FullBody = ''
			SET @FullListOfEmailRecips = ''
			SET @Iterator2 = (SELECT MIN([ID]) FROM @TempEmailDat)
			IF @Iterator2 > 0 BEGIN SET @Iterator2 = @Iterator2 - 1 END
			SET @SecondaryKey = (SELECT WINID FROM @SSStaffTable WHERE [ID] = @Iterator)
			WHILE @Iterator2 < @TotalTicketCount
			BEGIN
				SET @Iterator2 = @Iterator2 + 1
				/*if the SS analyst match then add data to working strings*/
				IF (SELECT SSAnalyst FROM @TempEmailDat WHERE [ID] = @Iterator2) = @SecondaryKey
				BEGIN
					IF @FullBody = '' 
					BEGIN
						SET @FullBody = (SELECT BodyPart FROM @TempEmailDat WHERE [ID] = @Iterator2)
						SET @FullListOfEmailRecips = @BSSubManager + ';' + @ASerSubManager + ';' + @BOBManager + ';' + @BSManager + ';' + @SSManager + (SELECT SSAnalyst FROM @TempEmailDat WHERE [ID] = @Iterator2)
					END
					ELSE
					BEGIN
						SET @FullBody = @FullBody +CHAR(13)  + CHAR(10) + (SELECT BodyPart FROM @TempEmailDat WHERE [ID] = @Iterator2)
					END
				END
			END
			/* add record to results table */
			INSERT INTO @Results (Subject, Body, Recipients) VALUES ('Emergency System Issue ticket(s)', @FullBody, @FullListOfEmailRecips)
		END
	END
	
	/* check for COMPASS financial adjustment tickets */
	IF (SELECT COUNT(*) FROM @EmailDat WHERE TicketCode = 'FAR' AND TheSystem = 'Compass') > 0
	BEGIN
		/*get Operation Accounting staff email addresses for COMPASS */
		INSERT INTO #RecipTemp EXEC dbo.spGENRRecipientString 'HPFinAdjCompass' 
		SET @FinStaffCO = (SELECT * FROM #RecipTemp)
		DELETE FROM @TempEmailDat WHERE [ID] > 0	
		INSERT INTO @TempEmailDat (BodyPart, TicketCode, Manager, Requester, QCAnalyst, SSAnalyst, TheSystem, Unit) SELECT BodyPart, TicketCode, Manager, Requester, QCAnalyst, SSAnalyst, TheSystem, Unit FROM @EmailDat WHERE TicketCode = 'FAR' AND TheSystem = 'Compass'
		/*get record totals for iterations*/
		SET @TotalTicketCount = (SELECT MAX([ID]) FROM @TempEmailDat)
		SET @Iterator = (SELECT MIN([ID]) FROM @TempEmailDat)
		IF @Iterator > 0 BEGIN SET @Iterator = @Iterator - 1 END
		SET @FullBody = ''
		SET @FullListOfEmailRecips = ''
		/* iterate through records and create result record */
		WHILE @Iterator < @TotalTicketCount
		BEGIN
			SET @Iterator = @Iterator + 1
			IF @FullBody = '' 
			BEGIN
				SET @FullBody = (SELECT BodyPart FROM @TempEmailDat WHERE [ID] = @Iterator)
				SET @FullListOfEmailRecips = @BSSubManager + ';' + @ASerSubManager + ';' + @BOBManager + ';' + @BSManager + ';' + @FinStaffCO
			END
			ELSE
			BEGIN
				SET @FullBody = @FullBody + CHAR(13)  + CHAR(10) + (SELECT BodyPart FROM @TempEmailDat WHERE [ID] = @Iterator2)
			END
		END
		/* add record to results table */
		INSERT INTO @Results (Subject, Body, Recipients) VALUES ('Emergency Compass Financial Adjustment ticket(s)', @FullBody, @FullListOfEmailRecips)
	END
	
	/* check for OneLINK financial adjustment tickets */
	IF (SELECT COUNT(*) FROM @EmailDat WHERE TicketCode = 'FAR' AND TheSystem = 'OneLINK') > 0
	BEGIN
		/*get Operation Accounting staff email addresses for COMPASS */
		DELETE FROM #RecipTemp WHERE RecipString IS NOT NULL
		INSERT INTO #RecipTemp EXEC dbo.spGENRRecipientString 'HPFinAdjOneLink'
		SET @FinStaffOL = (SELECT * FROM #RecipTemp)
		DELETE FROM @TempEmailDat WHERE [ID] > 0	
		INSERT INTO @TempEmailDat (BodyPart, TicketCode, Manager, Requester, QCAnalyst, SSAnalyst, TheSystem, Unit) SELECT BodyPart, TicketCode, Manager, Requester, QCAnalyst, SSAnalyst, TheSystem, Unit FROM @EmailDat WHERE TicketCode = 'FAR' AND TheSystem = 'OneLINK'
		/*get record totals for iterations*/
		SET @TotalTicketCount = (SELECT MAX([ID]) FROM @TempEmailDat)
		SET @Iterator = (SELECT MIN([ID]) FROM @TempEmailDat)
		IF @Iterator > 0 BEGIN SET @Iterator = @Iterator - 1 END
		SET @FullBody = ''
		SET @FullListOfEmailRecips = ''
		/* iterate through records and create result record */
		WHILE @Iterator < @TotalTicketCount
		BEGIN
			SET @Iterator = @Iterator + 1
			IF @FullBody = '' 
			BEGIN
				SET @FullBody = (SELECT BodyPart FROM @TempEmailDat WHERE [ID] = @Iterator)
				SET @FullListOfEmailRecips = @BSSubManager + ';' + @ASerSubManager + ';' + @BOBManager + ';' + @BSManager + ';' + @FinStaffOL
			END
			ELSE
			BEGIN
				SET @FullBody = @FullBody + CHAR(13)  + CHAR(10) + (SELECT BodyPart FROM @TempEmailDat WHERE [ID] = @Iterator2)
			END
		END
		/* add record to results table */
		INSERT INTO @Results (Subject, Body, Recipients) VALUES ('Emergency OneLINK Financial Adjustment ticket(s)', @FullBody, @FullListOfEmailRecips)
	END
	DELETE FROM dbo.NDHP_LST_PriorityUpdated WHERE PriorityUpdated IS NOT NULL
	INSERT INTO dbo.NDHP_LST_PriorityUpdated VALUES (GETDATE())
END

DROP TABLE #RecipTemp

SELECT Recipients, Subject, Body FROM @Results