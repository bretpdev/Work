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

:r "CSYS\spMD_UserIsFaqAdmin.sql"

use [CDW]
go
:r "CDW_UDW_ODW\BorrowersSearch.sql"

use [UDW]
go
:r "CDW_UDW_ODW\BorrowersSearch.sql"

use [ODW]
go
:r "CDW_UDW_ODW\ODW_BorrowersSearch.sql"