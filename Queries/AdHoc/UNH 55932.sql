USE TLP
GO

BEGIN TRANSACTION
	DECLARE @ERROR INT = 0
	DECLARE @ROWCOUNT INT = 0
	DECLARE @Expected INT = 2

INSERT INTO TLP..ReferenceDat (RefLastName, RefFirstName, RefMiddleInit, RefAdd1, RefAdd2, RefCity, RefState, RefZip, RefAddValidity, RefHomePhone, RefHomePhoneValidity, SSN)
VALUES
	('Gibbens','Bette','','12254 Stephens View Cir','','Draper','UT','84020',1,'8014952902',1,'528965213'),
	('Smith','Dana','','2822 W Warner Way','','Riverton','UT','84065',1,'8018916603',1,'528965213')

	-- Save/Set the row count and error number (if any) from the previously executed statement
	SELECT @ROWCOUNT = @@ROWCOUNT, @ERROR = @@ERROR

IF @ROWCOUNT = @Expected AND @ERROR = 0
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