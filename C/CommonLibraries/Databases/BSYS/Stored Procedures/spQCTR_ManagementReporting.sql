CREATE PROCEDURE [dbo].[spQCTR_ManagementReporting]


AS


--Create Results Table to collect recipients temporarily
DECLARE	@Results		TABLE 	(
					RecNum		INT IDENTITY (1,1),
					WID		VARCHAR(50) 
					)
--Create ReturnableResults to return complied data
DECLARE @ReturnableResults	TABLE	(
					Recipients	VARCHAR(7000),
					Subject		VARCHAR(100),
					Body		VARCHAR(100)
					)
--Create Applicable BU table to return BUs that had issues in a certain time frame
DECLARE	@ApplicableBUs		TABLE 	(
				BU		VARCHAR(50) 
				)
DECLARE @RecNumIterator		INT
DECLARE @RecipStr		VARCHAR(7000)
DECLARE @DailyReportDateSent	DATETIME

SET @RecNumIterator = 1
SET @RecipStr = ''

--Monthly report email 
IF (SELECT COUNT(*) FROM QCTR_DAT_EmailSentForManagementReporting WHERE ReportType = 'Monthly' AND DATEDIFF(mm,DateSent,GETDATE()) = 0) = 0
BEGIN
INSERT INTO @Results (WID) (
					--Monthly Email Recips
					SELECT DISTINCT A.WindowsUserID + '@utahsbr.edu' as WindowsUserID
					FROM GENR_REF_BU_Agent_Xref A 
					--only send email to Management that have authorized QC access 
					--(this is to keep managment that don't know about QC from getting the email)
					JOIN (
						SELECT DISTINCT WindowsUserID
						FROM GENR_REF_BU_Agent_Xref
						WHERE ROLE = 'Authorized QC'
						) B ON A.WindowsUserID = B.WindowsUserID
					WHERE A.Role = 'Manager' OR A.Role = 'Supervisor'
			)

--iterate through to create recipient string
WHILE @RecNumIterator <= (SELECT MAX(RecNum) FROM @Results)
BEGIN				
	IF @RecipStr = ''
	BEGIN
		SET @RecipStr = (SELECT WID FROM @Results WHERE RecNum = @RecNumIterator)
	END
	ELSE
	BEGIN
		SET @RecipStr = @RecipStr + ';' + (SELECT WID FROM @Results WHERE RecNum = @RecNumIterator)
	END
	SET @RecNumIterator = @RecNumIterator + 1
END

--add record to returnableresults table
INSERT INTO @ReturnableResults (Subject, Body, Recipients) VALUES ('Monthly QC Report', 'http://paweb/QCTest/ManagementReporting.jsp?cbReport=Monthly', @RecipStr)

--update table that report was sent
INSERT INTO QCTR_DAT_EmailSentForManagementReporting (ReportType) VALUES ('Monthly')
END



SET @RecipStr = ''

--Weekly Report Email
IF (SELECT COUNT(*) FROM QCTR_DAT_EmailSentForManagementReporting WHERE ReportType = 'Weekly' AND DATEDIFF(ww,DateSent,GETDATE()) = 0) = 0
BEGIN
INSERT INTO @Results (WID) (
				--Weekly Email Recips
				SELECT DISTINCT A.WindowsUserID + '@utahsbr.edu' as WindowsUserID 
				FROM GENR_REF_BU_Agent_Xref A 
				--only send email to Management that have authorized QC access 
				--(this is to keep managment that don't know about QC from getting the email)
				JOIN (
					SELECT DISTINCT WindowsUserID
					FROM GENR_REF_BU_Agent_Xref
					WHERE ROLE = 'Authorized QC'
					) B ON A.WindowsUserID = B.WindowsUserID
				WHERE (A.Role = 'Manager' AND A.BusinessUnit LIKE '%Subbranch%') OR
						((A.Role = 'Manager' OR A.Role = 'Supervisor') AND A.BusinessUnit = 'Account Services') OR
						((A.Role = 'Manager' OR A.Role = 'Supervisor') AND A.BusinessUnit = 'Auxiliary Services') OR
						((A.Role = 'Manager' OR A.Role = 'Supervisor') AND A.BusinessUnit = 'Borrower Services') OR
						((A.Role = 'Manager' OR A.Role = 'Supervisor') AND A.BusinessUnit = 'Loan Management') OR
						((A.Role = 'Manager' OR A.Role = 'Supervisor') AND A.BusinessUnit = 'Operational Accounting') OR
						((A.Role = 'Manager' OR A.Role = 'Supervisor') AND A.BusinessUnit = 'Document Services') OR
						((A.Role = 'Manager' OR A.Role = 'Supervisor') AND A.BusinessUnit = 'School Services')
			)

--iterate through to create recipient string
WHILE @RecNumIterator <= (SELECT MAX(RecNum) FROM @Results)
BEGIN				
	IF @RecipStr = ''
	BEGIN
		SET @RecipStr = (SELECT WID FROM @Results WHERE RecNum = @RecNumIterator)
	END
	ELSE
	BEGIN
		SET @RecipStr = @RecipStr + ';' + (SELECT WID FROM @Results WHERE RecNum = @RecNumIterator)
	END
	SET @RecNumIterator = @RecNumIterator + 1
END

--add record to returnableresults table
INSERT INTO @ReturnableResults (Subject, Body, Recipients) VALUES ('Weekly QC Report', 'http://paweb/QCTest/ManagementReporting.jsp?cbReport=Weekly', @RecipStr)

--update table that report was sent
INSERT INTO QCTR_DAT_EmailSentForManagementReporting (ReportType) VALUES ('Weekly')
END

SET @RecipStr = ''

--Quarterly Report Email
IF (SELECT COUNT(*) FROM QCTR_DAT_EmailSentForManagementReporting WHERE ReportType = 'Quarterly' AND DATEDIFF(qq,DateSent,GETDATE()) = 0) = 0
BEGIN
INSERT INTO @Results (WID) (
				--Quarterly Email Recips
				SELECT DISTINCT A.WindowsUserID + '@utahsbr.edu' as WindowsUserID
				FROM GENR_REF_BU_Agent_Xref A 
				--only send email to Management that have authorized QC access 
				--(this is to keep managment that don't know about QC from getting the email)
				JOIN (
					SELECT DISTINCT WindowsUserID
					FROM GENR_REF_BU_Agent_Xref
					WHERE ROLE = 'Authorized QC'
					) B ON A.WindowsUserID = B.WindowsUserID
				WHERE A.Role = 'Manager' AND 
					(A.BusinessUnit LIKE '%Subbranch%' OR
					A.BusinessUnit = 'Business Operations Branch' OR
					A.BusinessUnit = 'Operations Division')
			)

--iterate through to create recipient string
WHILE @RecNumIterator <= (SELECT MAX(RecNum) FROM @Results)
BEGIN				
	IF @RecipStr = ''
	BEGIN
		SET @RecipStr = (SELECT WID FROM @Results WHERE RecNum = @RecNumIterator)
	END
	ELSE
	BEGIN
		SET @RecipStr = @RecipStr + ';' + (SELECT WID FROM @Results WHERE RecNum = @RecNumIterator)
	END
	SET @RecNumIterator = @RecNumIterator + 1
END

--add record to returnableresults table
INSERT INTO @ReturnableResults (Subject, Body, Recipients) VALUES ('Quarterly QC Report', 'http://paweb/QCTest/ManagementReporting.jsp?cbReport=Quarterly', @RecipStr)

--update table that report was sent
INSERT INTO QCTR_DAT_EmailSentForManagementReporting (ReportType) VALUES ('Quarterly')
END

SET @RecipStr = ''

--Daily Report Email
--DateSent turns into what date the report was sent for last more than what date the report was last sent when it comes to the daily report.  
--if it wasn't done this way then management would get duplicate reports if a day went by that didn't have a QC issue logged on it.
SET @DailyReportDateSent = (SELECT MAX(Requested) FROM dbo.QCTR_DAT_Issue WHERE DATEDIFF(dd,Requested,GETDATE()) <> 0)
--do figuring from data gathered above
IF (SELECT COUNT(*) FROM QCTR_DAT_EmailSentForManagementReporting WHERE ReportType = 'Daily' AND DATEDIFF(dd,DateSent,@DailyReportDateSent) = 0) = 0
BEGIN
	--get applicable BUs
	INSERT INTO @ApplicableBUs (BU) (
									SELECT DISTINCT B.BU
									FROM dbo.QCTR_DAT_Issue A
									INNER JOIN dbo.QCTR_DAT_BU B
										ON A.ID = B.IssueID
									WHERE DATEDIFF(dd,A.Requested,@DailyReportDateSent) = 0
									)
	IF (SELECT COUNT(*) FROM @ApplicableBUs) > 0 --only process if there is something in the table
	BEGIN
		--adds records for managers and 2nds
		INSERT INTO @Results (WID) (
						--Daily Email Recips
						SELECT DISTINCT A.WindowsUserID + '@utahsbr.edu' as WindowsUserID
						FROM GENR_REF_BU_Agent_Xref A 
						--only send email to Management that have authorized QC access 
						--(this is to keep managment that don't know about QC from getting the email)
						JOIN (
							SELECT DISTINCT WindowsUserID
							FROM GENR_REF_BU_Agent_Xref
							WHERE ROLE = 'Authorized QC'
							) B ON A.WindowsUserID = B.WindowsUserID
						WHERE ((A.Role = 'Manager' OR A.Role = 'Supervisor') AND A.BusinessUnit IN (SELECT BU FROM @ApplicableBUs))
							
					)
		--adds Assistant Directors
		INSERT INTO @Results (WID) (
						--Daily Email Recips
						SELECT DISTINCT A.WindowsUserID + '@utahsbr.edu' as WindowsUserID
						FROM GENR_REF_BU_Agent_Xref A 
						--only send email to Management that have authorized QC access 
						--(this is to keep managment that don't know about QC from getting the email)
						JOIN (
							SELECT DISTINCT WindowsUserID
							FROM GENR_REF_BU_Agent_Xref
							WHERE ROLE = 'Authorized QC'
							) B ON A.WindowsUserID = B.WindowsUserID
						JOIN (
							SELECT DISTINCT Parent
							FROM dbo.GENR_LST_BusinessUnits
							WHERE BusinessUnit IN (SELECT BU FROM @ApplicableBUs) AND
								Parent LIKE '%Subbranch%'
							) C ON A.BusinessUnit = C.Parent
						WHERE A.Role = 'Manager'
					)

		--iterate through to create recipient string
		WHILE @RecNumIterator <= (SELECT MAX(RecNum) FROM @Results)
		BEGIN				
			IF @RecipStr = ''
			BEGIN
				SET @RecipStr = (SELECT WID FROM @Results WHERE RecNum = @RecNumIterator)
			END
			ELSE
			BEGIN
				SET @RecipStr = @RecipStr + ';' + (SELECT WID FROM @Results WHERE RecNum = @RecNumIterator)
			END
			SET @RecNumIterator = @RecNumIterator + 1
		END

		--add record to returnableresults table
		INSERT INTO @ReturnableResults (Subject, Body, Recipients) VALUES ('Daily QC Report', 'http://paweb/QCTest/ManagementReporting.jsp?cbReport=Daily', @RecipStr)

		--update table that report was sent
		INSERT INTO QCTR_DAT_EmailSentForManagementReporting (ReportType, DateSent) VALUES ('Daily', @DailyReportDateSent)
	END
END


SELECT Recipients, Subject, Body FROM @ReturnableResults