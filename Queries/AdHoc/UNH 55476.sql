USE IncomeBasedRepaymentUheaa
GO

BEGIN TRANSACTION
	DECLARE @ERROR INT = 0
	DECLARE @ROWCOUNT INT = 0
	DECLARE @ExpectedRowCount INT = 2--2 records to be updated.

--Issue:
--Letter IBRAPRVUH was promoted on 01-31-2018, however it has not generated at all.  
--In order to remedy this, this DCR needs to: 

--1) Update the mapping in IncomeBasedRepaymentUheaa.dbo.Arc_Letter_Data in UHEAASQLDB so that the records with a mapping_id of 1 
--and records with a mapping_id of 22 have the letter_id set to 'IBRAPRVUH'.  

UPDATE IncomeBasedRepaymentUheaa..Arc_Letter_Data SET letter_id = 'IBRAPRVUH' WHERE mapping_id IN (1,22)

SELECT @ROWCOUNT = @@ROWCOUNT, @ERROR = @@ERROR --2


--2) A query needs to be made to identify all borrowers who were placed on an IBR plan who were placed on the plan due to the reasons entailed by mapping_id 1 or mapping_id 22. 
--Once all borrowers have been identified, we will send the appropriate correspondence to them. 

--SELECT DISTINCT BF_SSN FROM UDW..AY10_BR_LON_ATY WHERE PF_REQ_ACT = 'IBAPV' AND LD_ATY_REQ_RCV >= '01/31/18'


IF @ROWCOUNT = @ExpectedRowCount AND @ERROR = 0
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
