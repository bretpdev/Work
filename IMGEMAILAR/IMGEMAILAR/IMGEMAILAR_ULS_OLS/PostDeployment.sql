/*
Post-Deployment Script Template							
--------------------------------------------------------------------------------------
 This file contains SQL statements that will be appended to the build script.		
 Use SQLCMD syntax to include a file in the post-deployment script.			
 Example:      :r .\myfile.sql								
 Use SQLCMD syntax to reference a variable in the post-deployment script.		
 Example:      :setvar TableName MyTable							
               SELECT * FROM [$(TableName)]					
--------------------------------------------------------------------------------------
*/

	TRUNCATE TABLE imgemailar.AvailableDocuments

	IF (DB_NAME() = 'OLS')
		INSERT INTO imgemailar.AvailableDocuments(LetterId, OverrideDescription) VALUES
		('LSARC', 'UH Emails'),
		('PCARC', NULL),
		('PCAWG', NULL),
		('PCCMP', NULL),
		('PCCOR', NULL),
		('PCDDB', NULL),
		('PCDPA', NULL),
		('PCFBS', NULL),
		('PCLVC', NULL),
		('PCLGL', NULL),
		('PCMGR', NULL),
		('PCRAP', NULL),
		('PCRHB', NULL),
		('PCRHN', NULL),
		('PCRST', NULL),
		('PCTAX', NULL),
		('PCTPD', NULL),
		('DACOR', NULL),
		('DADEF', NULL),
		('DAFOR', NULL)

	IF (DB_NAME() = 'CLS')
	BEGIN
		INSERT INTO imgemailar.AvailableDocuments(LetterId, OverrideDescription) VALUES
		('CREML', 'CS Emails'),
		('CRARC', NULL),
		('CCONA', NULL),
		('CPOCA', NULL),
		('CMOCA', NULL)

		GRANT EXECUTE ON schema::imgemailar to [UHEAA\CornerStoneUsers]
	END
	ELSE
	BEGIN
		GRANT EXECUTE ON schema::imgemailar to [UHEAA\UheaaUsers]
	END

	IF (DB_NAME() = 'ULS')
		INSERT INTO imgemailar.AvailableDocuments(LetterId, OverrideDescription) VALUES
		('LSARC', 'UH Emails')
