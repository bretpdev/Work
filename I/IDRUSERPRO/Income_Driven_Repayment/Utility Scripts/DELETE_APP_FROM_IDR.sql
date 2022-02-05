BEGIN TRAN
GO

USE [Income_Driven_Repayment]
GO

DECLARE @Row  FLOAT
DECLARE @NumberShouldBeAffected  INT
DECLARE @Repayment_Plan_Type_Id INT
DECLARE @ApplicationId INT
DECLARE @BorrowerId INT

--NOTE--
--uncomment out the tables that you need to delete data from.  You may have to go look in each table to find out if there is data within the table.

--set application id you need to delete
SET @ApplicationId = 

--SET THE ACCOUNT NUMBER IF YOU NEED TO DELETE THE BORROWER RECORD
SET @BorrowerId = 

--number of row you need to delete.  You will need to look in each table to get the count.
SET @NumberShouldBeAffected = 

SET @Repayment_Plan_Type_Id = (SELECT repayment_plan_type_id FROM Repayment_Plan_Selected WHERE application_id = @ApplicationId)

--DELETE FROM dbo.BD_Data_History
--WHERE application_id = @ApplicationId
--SET @Row = @@ROWCOUNT

--DELETE FROM dbo.BE_Data_History
--WHERE application_id = @ApplicationId
--SET @Row = @@ROWCOUNT + @Row

--DELETE FROM dbo.BF_Data_History
--WHERE application_id = @ApplicationId
--SET @Row = @@ROWCOUNT + @Row

--DELETE FROM Loans
--WHERE application_id = @ApplicationId
--SET @Row = @@ROWCOUNT + @Row

--DELETE FROM dbo.Repayment_Plan_Type_Status_History
--WHERE repayment_plan_type_id = @Repayment_Plan_Type_Id
--SET @Row = @@ROWCOUNT + @Row

--DELETE FROM dbo.Repayment_Plan_Selected
--WHERE application_id = @ApplicationId
--SET @Row = @@ROWCOUNT + @Row

--DELETE FROM Other_Loans
--WHERE application_id = @ApplicationId
--SET @Row = @@ROWCOUNT + @Row

--DELETE FROM dbo.Applications
--WHERE application_id = @ApplicationId
--SET @Row = @@ROWCOUNT + @Row

--DELETE FROM dbo.Borrowers
--WHERE borrower_id = @BorrowerId
--SET @Row = @@ROWCOUNT + @Row


--This should never change
IF @Row <> @NumberShouldBeAffected
	BEGIN
		ROLLBACK
		PRINT 'Looking for ' + CAST(@NumberShouldBeAffected AS VARCHAR(5)) + ' rows. Rolled Back because there are ' + CAST(@Row as varchar(5)) + ' rows affected'
	END
ELSE
	BEGIN
		COMMIT
		PRINT 'Committed ' + CAST(@Row AS VARCHAR(5)) + ' rows'
	END