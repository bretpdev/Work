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

--INSERT INTO ArcType(ArcTypeId, ArcType, ArcTypeDescription)
--VALUES(0, 'Atd22ByLoan', 'Add arc by sequence number')
--,(1, 'Atd22AllLoans', 'Add arc to all loans')
--,(2, 'Atd22ByBalance', 'Add arc for all loans with a balance')
--,(3, 'Atd22ByLoanProgram', 'Add arc by loan program')
--,(4, 'Atd22AllLoansRegards', 'Add arc to all loans with regards to information')
--,(5, 'Atd22ByLoanRegards', 'Add arc by sequence number with regards to information')

GRANT EXECUTE ON SCHEMA ::scra TO db_executor
GO