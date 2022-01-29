/*
 Pre-Deployment Script Template							
--------------------------------------------------------------------------------------
 This file contains SQL statements that will be executed before the build script.	
 Use SQLCMD syntax to include a file in the pre-deployment script.			
 Example:      :r .\myfile.sql								
 Use SQLCMD syntax to reference a variable in the pre-deployment script.		
 Example:      :setvar TableName MyTable							
               SELECT * FROM [$(TableName)]					
--------------------------------------------------------------------------------------
*/

INSERT INTO CSYS..GENR_DAT_EnterpriseFileSystem([Key], TestMode, Region, [Path])
VALUES
	('Barcode Font', 0, 'UHEAA', 'C:\Windows\Fonts'),
	('Barcode Font', 1, 'UHEAA', 'C:\Windows\Fonts'),
	('Barcode Font', 0, 'CornerStone', 'C:\Windows\Fonts'),
	('Barcode Font', 1, 'CornerStone', 'C:\Windows\Fonts')