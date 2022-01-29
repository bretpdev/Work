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

IF ((SELECT COUNT(*) FROM payhistlpp.UserAccess) = 0)
	BEGIN
		INSERT INTO payhistlpp.UserAccess(UserName)
		VALUES('')
		,('Candice')
		,('David')
		,('Jeremy')
		,('Wendy')
	END

GRANT EXECUTE ON SCHEMA:: payhistlpp TO db_executor