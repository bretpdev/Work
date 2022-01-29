--RUN ON UHEAASQLDB

USE CLS;
GO

DECLARE @TICKET VARCHAR(XX) = 'CNH_XXXXX';

BEGIN TRY
	BEGIN TRANSACTION @TICKET

		DELETE FROM Phone_Consent_Arcs
		WHERE   (
					arc = 'PHNCO'
					AND endorser = X
				)
			OR (
					arc = 'PHNCE'
					AND endorser = X
				)
		;

		INSERT INTO	Phone_Consent_Arcs 
			(
				arc,endorser
			)
		VALUES ('PHNUP',X), ('EWEBC',X)
		;

	COMMIT TRANSACTION
	--ROLLBACK TRANSACTION
END TRY
BEGIN CATCH;
	PRINT @TICKET + '.sql not committed.';
	THROW;
END CATCH;