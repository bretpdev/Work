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
IF EXISTS ( SELECT * 
            FROM   sysobjects 
            WHERE  id = object_id(N'[bcsretmail].[CheckBorrowerRegion]') 
                   and OBJECTPROPERTY(id, N'IsProcedure') = 1 )
BEGIN
    DROP PROCEDURE [bcsretmail].[CheckBorrowerRegion]
END


IF EXISTS ( SELECT * 
            FROM   sysobjects 
            WHERE  id = object_id(N'[bcsretmail].[GetCaliforniaBorForEnd]') 
                   and OBJECTPROPERTY(id, N'IsProcedure') = 1 )
BEGIN
    DROP PROCEDURE [bcsretmail].[GetCaliforniaBorForEnd]
END


IF EXISTS ( SELECT * 
            FROM   sysobjects 
            WHERE  id = object_id(N'[bcsretmail].[GetCaliforniaBorrower]') 
                   and OBJECTPROPERTY(id, N'IsProcedure') = 1 )
BEGIN
    DROP PROCEDURE [bcsretmail].[GetCaliforniaBorrower]
END


IF EXISTS ( SELECT * 
            FROM   sysobjects 
            WHERE  id = object_id(N'[bcsretmail].[GetCaliforniaEndForBor]') 
                   and OBJECTPROPERTY(id, N'IsProcedure') = 1 )
BEGIN
    DROP PROCEDURE[bcsretmail].[GetCaliforniaEndForBor]
END


IF EXISTS ( SELECT * 
            FROM   sysobjects 
            WHERE  id = object_id(N'[bcsretmail].[GetCaliforniaEndorser]') 
                   and OBJECTPROPERTY(id, N'IsProcedure') = 1 )
BEGIN
    DROP PROCEDURE [bcsretmail].[GetCaliforniaEndorser]
END


IF EXISTS (SELECT * FROM sys.schemas WHERE name = 'bcsretmail')
BEGIN
    DROP SCHEMA bcsretmail
END


GRANT EXECUTE ON SCHEMA:: rtrnmailuh TO db_executor