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

--DELETE FROM batchesp.ESPEnrollments
--DELETE FROM batchesp.TS01Enrollments
--DELETE FROM batchesp.TS26LoanInformation
--DELETE FROM batchesp.TSAYDefermentForbearances
--DELETE FROM batchesp.Ts2hPendingDisbursements


--DELETE FROM batchesp.NonSelectionReasons

--INSERT INTO batchesp.NonSelectionReasons (Reason, Course) VALUES ('YOUR LOAN IS NOT ELIGIBLE FOR THIS TYPE', 'A')
--INSERT INTO batchesp.NonSelectionReasons (Reason, Course) VALUES ('SSN IS NOT THE STUDENT FOR THIS LOAN', 'A')
--INSERT INTO batchesp.NonSelectionReasons (Reason, Course) VALUES ('LOAN HAS BEEN FULLY CANCELLED', 'A')
--INSERT INTO batchesp.NonSelectionReasons (Reason, Course) VALUES ('COMAKER NOT ELIGIBLE FOR DEFEREMENT TYPE', 'A')
--INSERT INTO batchesp.NonSelectionReasons (Reason, Course) VALUES ('COMAKER NOT ELIGIBLE FOR FORBEARANE TYPE', 'A')
--INSERT INTO batchesp.NonSelectionReasons (Reason, Course) VALUES ('LOAN IS PAID IN FULL', 'A')
--INSERT INTO batchesp.NonSelectionReasons (Reason, Course) VALUES ('BORR DENIED POST ENROLLMENT DEFERMENT', 'A')
--INSERT INTO batchesp.NonSelectionReasons (Reason, Course) VALUES ('TRANSITION MUST FOLLOW F/T SCH DEFERMENT', 'A')
--INSERT INTO batchesp.NonSelectionReasons (Reason, Course) VALUES ('YOUR LOAN IS NOT ELIGIBLE FOR THIS FORB', 'A')
--INSERT INTO batchesp.NonSelectionReasons (Reason, Course) VALUES ('OVERLAPS EXISTING PERIOD', 'B')
--INSERT INTO batchesp.NonSelectionReasons (Reason, Course) VALUES ('YOU ARE PRESENTLY ON A DEFER/FORB', 'B')
--INSERT INTO batchesp.NonSelectionReasons (Reason, Course) VALUES ('BORROWER IN DISABILITY STATUS', 'C')
--INSERT INTO batchesp.NonSelectionReasons (Reason, Course) VALUES ('BORROWER IN BANKRUPTCY STATUS', 'D')
--INSERT INTO batchesp.NonSelectionReasons (Reason, Course) VALUES ('DEFERMENT IND = N', 'E')
--INSERT INTO batchesp.NonSelectionReasons (Reason, Course) VALUES ('BORROWER IN DEATH STATUS', 'F')

grant execute on schema::batchesp to [Uheaa\UheaaUsers]