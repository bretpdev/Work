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
INSERT INTO Arguments(Argument, ArgumentDescription)
VALUES('Mode (int)', 'Sets the mode that the application will start in.'),
('SqlUserId', 'Sets the ID of the user running the application.'),
('Role', 'Sets the role for the user that is using the application.'),
('User Roles', 'Sets the list of available user roles.'),
('Mode (string)', 'Sets the mode that the application will start in.')
GO

INSERT INTO Applications(ApplicationName, AccessKey, AddedBy, SourcePath, StartingDll, StartingClass)
VALUES('Application Settings', 'Application Settings', 0, '\\cs1\Standards\Desktop Software\Need Help\{0}\Application Settings\', 'ApplicationSettings.dll', 'LibraryStarter')

INSERT INTO ApplicationArguments(ApplicationId, ArgumentId, ArgumentOrder)
VALUES(1, 1, 1)
, (1, 2, 2)