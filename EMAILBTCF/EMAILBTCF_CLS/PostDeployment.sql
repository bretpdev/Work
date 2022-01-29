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

--Migrate dbo.BatchEmail data into emailbtcf.EmailCampaigns

INSERT INTO
	emailbtcf.EmailCampaigns (SasFile, LetterId, SendingAddress, SubjectLine, Arc, CommentText)
SELECT
	SasFile, LetterId, SendingAddress, SubjectLine, Arc, CommentText
FROM
	dbo.BatchEmail


--Delete old table
DROP TABLE dbo.BatchEmail