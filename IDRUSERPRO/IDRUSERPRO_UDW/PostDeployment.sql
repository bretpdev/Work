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
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_GRSP_NDS_LON_RSP_IdrIneligible]') AND type = 'D')
BEGIN
    ALTER TABLE [GRSP_NDS_LON_RSP] DROP CONSTRAINT [DF_GRSP_NDS_LON_RSP_IdrIneligible]
END