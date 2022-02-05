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

TRUNCATE TABLE aesrcvdial.DialerFiles

INSERT INTO aesrcvdial.DialerFiles([FileName], OutputFileName)
VALUES('rcvfile8*','LGP_OB_Auto_DFT')
,('rcvfile12*','LGP_OB_Auto_PRE')
,('rcvfile13*','LGP_OB_Auto_SKP')

GRANT EXECUTE ON SCHEMA::aesrcvdial TO db_executor