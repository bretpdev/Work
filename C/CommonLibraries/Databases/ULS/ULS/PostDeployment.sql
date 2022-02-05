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

--DELETE FROM CPP_FileTypes

--INSERT INTO CPP_FileTypes(FileType)
--VALUES('Comma')
--,('Direct')
--,('Flat')

GRANT EXECUTE ON [dbo].[CPP_DeletePaymentSource] TO db_executor
GRANT EXECUTE ON [dbo].[CPP_GetFileTypeForPaymentSource] TO db_executor
GRANT EXECUTE ON [dbo].[CPP_GetFileTypes] TO db_executor
GRANT EXECUTE ON [dbo].[CPP_GetPaymentSources] TO db_executor
GRANT EXECUTE ON [dbo].[CPP_GetPaymentTypes] TO db_executor
GRANT EXECUTE ON [dbo].[CPP_InsertPaymentSource] TO db_executor
GRANT EXECUTE ON [dbo].[CPP_SetOverpaymentTransmittal] TO db_executor
GRANT EXECUTE ON [dbo].[CPP_UpdatePaymentSource] TO db_executor
GRANT EXECUTE ON [dbo].[CPP_DeletePaymentType] TO db_executor
GRANT EXECUTE ON [dbo].[CPP_InsertPaymentType] TO db_executor
GRANT EXECUTE ON [dbo].[CPP_UpdatePaymentType] TO db_executor

GRANT EXECUTE ON SCHEMA ::[print] to db_executor
GO

GRANT EXECUTE ON SCHEMA ::[dbo] to db_executor
GO