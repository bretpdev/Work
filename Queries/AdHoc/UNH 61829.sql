--RUN ON UHEAASQLDB

USE ULS;
GO

DECLARE @TICKET VARCHAR(10) = 'UNH_61829';

BEGIN TRY
	BEGIN TRANSACTION @TICKET

		DELETE FROM Phone_Consent_Arcs
		WHERE   (
					arc = 'PHNCO'
					AND endorser = 0
					AND compass = 1
				)
			OR (
					arc = 'PHNCE'
					AND endorser = 1
					AND compass = 1
				)
		;

		INSERT INTO	Phone_Consent_Arcs 
			(
				arc,endorser,compass
			)
		VALUES ('PHNUP',0,1), ('EWEBC',1,1)
		;

	COMMIT TRANSACTION
	--ROLLBACK TRANSACTION
END TRY
BEGIN CATCH;
	PRINT @TICKET + '.sql not committed.';
	THROW;
END CATCH;