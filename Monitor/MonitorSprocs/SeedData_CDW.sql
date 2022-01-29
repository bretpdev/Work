﻿--/*
--Post-Deployment Script Template							
--------------------------------------------------------------------------------------
-- This file contains SQL statements that will be appended to the build script.		
-- Use SQLCMD syntax to include a file in the post-deployment script.			
-- Example:      :r .\myfile.sql								
-- Use SQLCMD syntax to reference a variable in the post-deployment script.		
-- Example:      :setvar TableName MyTable							
--               SELECT * FROM [$(TableName)]					
--------------------------------------------------------------------------------------
--*/

--DELETE FROM monitor.ExemptLoanStatuses
--DBCC CHECKIDENT ('monitor.ExemptLoanStatuses', RESEED, 0)
--GO
--INSERT INTO monitor.ExemptLoanStatuses (LoanStatusCode, LoanStatusDescription) VALUES ('04', 'Defer')
--INSERT INTO monitor.ExemptLoanStatuses (LoanStatusCode, LoanStatusDescription) VALUES ('12', 'Claim Paid')
--INSERT INTO monitor.ExemptLoanStatuses (LoanStatusCode, LoanStatusDescription) VALUES ('16', 'Death Alleged')
--INSERT INTO monitor.ExemptLoanStatuses (LoanStatusCode, LoanStatusDescription) VALUES ('17', 'Death Verified')
--INSERT INTO monitor.ExemptLoanStatuses (LoanStatusCode, LoanStatusDescription) VALUES ('18', 'Disability Alleged')
--INSERT INTO monitor.ExemptLoanStatuses (LoanStatusCode, LoanStatusDescription) VALUES ('19', 'Disability Verified')
--INSERT INTO monitor.ExemptLoanStatuses (LoanStatusCode, LoanStatusDescription) VALUES ('20', 'Bankruptcy Alleged')
--INSERT INTO monitor.ExemptLoanStatuses (LoanStatusCode, LoanStatusDescription) VALUES ('21', 'Bankruptcy Verified')

--DELETE FROM monitor.ExemptForbearanceTypes
--DBCC CHECKIDENT ('monitor.ExemptForbearanceTypes', RESEED, 0)
--GO
--INSERT INTO monitor.ExemptForbearanceTypes (ForbearanceTypeCode, ForbearanceTypeDescription) VALUES ('10', 'Bankruptcy')
--INSERT INTO monitor.ExemptForbearanceTypes (ForbearanceTypeCode, ForbearanceTypeDescription) VALUES ('14', 'Disability')

--DELETE FROM monitor.ExemptSetupArcs
--DBCC CHECKIDENT ('monitor.ExemptSetupArcs', RESEED, 0)
--GO
--INSERT INTO monitor.ExemptSetupArcs (ARC, Queue) VALUES ('GRSEL', 'GP')
--INSERT INTO monitor.ExemptSetupArcs (ARC, Queue) VALUES ('MONTR', 'MO')
--INSERT INTO monitor.ExemptSetupArcs (ARC, Queue) VALUES ('IDRPN', '2P')
--INSERT INTO monitor.ExemptSetupArcs (ARC, Queue) VALUES ('CODCA', '2A')
--INSERT INTO monitor.ExemptSetupArcs (ARC, Queue) VALUES ('CODPA', '2A')
--INSERT INTO monitor.ExemptSetupArcs (ARC, Queue) VALUES ('CODRV', '2A')
--INSERT INTO monitor.ExemptSetupArcs (ARC, Queue) VALUES ('G119I', '2A')
--INSERT INTO monitor.ExemptSetupArcs (ARC, Queue) VALUES ('IBADI', '2A')
--INSERT INTO monitor.ExemptSetupArcs (ARC, Queue) VALUES ('IBALN', '2A')
--INSERT INTO monitor.ExemptSetupArcs (ARC, Queue) VALUES ('IBRAF', '2A')
--INSERT INTO monitor.ExemptSetupArcs (ARC, Queue) VALUES ('IBRDF', '2A')
--INSERT INTO monitor.ExemptSetupArcs (ARC, Queue) VALUES ('IBRRW', '2A')
--INSERT INTO monitor.ExemptSetupArcs (ARC, Queue) VALUES ('IBRWB', '2A')
--INSERT INTO monitor.ExemptSetupArcs (ARC, Queue) VALUES ('IBRWV', '2A')
--INSERT INTO monitor.ExemptSetupArcs (ARC, Queue) VALUES ('ICRAL', '2A')
--INSERT INTO monitor.ExemptSetupArcs (ARC, Queue) VALUES ('IDRPR', '2A')
--INSERT INTO monitor.ExemptSetupArcs (ARC, Queue) VALUES ('RPYNL', '2A')
--INSERT INTO monitor.ExemptSetupArcs (ARC, Queue) VALUES ('T4506', '2A')
--INSERT INTO monitor.ExemptSetupArcs (ARC, Queue) VALUES ('WRIDR', '2A')

--DELETE FROM monitor.ExemptScheduleTypes
--DBCC CHECKIDENT ('monitor.ExemptScheduleTypes', RESEED, 0)
--GO
--INSERT INTO monitor.ExemptScheduleTypes (ScheduleName, ScheduleCode, SchedulePlanName) VALUES ('PERMANENT STANDARD', 'IL', 'IBR')
--INSERT INTO monitor.ExemptScheduleTypes (ScheduleName, ScheduleCode, SchedulePlanName) VALUES ('INCOME SENSITIVE', 'IS', 'ISR')
--INSERT INTO monitor.ExemptScheduleTypes (ScheduleName, ScheduleCode, SchedulePlanName) VALUES ('REDUCED PAYMENTS', 'RP', 'Work out')

--INSERT INTO monitor.ExemptScheduleTypes (ScheduleName, ScheduleCode, SchedulePlanName) VALUES ('PFH', 'IB', 'IBR')
--INSERT INTO monitor.ExemptScheduleTypes (ScheduleName, ScheduleCode, SchedulePlanName) VALUES ('IBR2014 PARTIAL FINANCIAL HARDSHIP', 'I3', 'IBR 2014')
--INSERT INTO monitor.ExemptScheduleTypes (ScheduleName, ScheduleCode, SchedulePlanName) VALUES ('IBR2014 PERMANENT STANDARD', 'IP', 'IBR 2014')
--INSERT INTO monitor.ExemptScheduleTypes (ScheduleName, ScheduleCode, SchedulePlanName) VALUES ('ICR1', 'C1', 'ICR')
--INSERT INTO monitor.ExemptScheduleTypes (ScheduleName, ScheduleCode, SchedulePlanName) VALUES ('ICR2', 'C2', 'ICR')
--INSERT INTO monitor.ExemptScheduleTypes (ScheduleName, ScheduleCode, SchedulePlanName) VALUES ('ICR3', 'C3', 'ICR')
--INSERT INTO monitor.ExemptScheduleTypes (ScheduleName, ScheduleCode, SchedulePlanName) VALUES ('CONTINGENT LEVEL', 'CL', 'ICR')
--INSERT INTO monitor.ExemptScheduleTypes (ScheduleName, ScheduleCode, SchedulePlanName) VALUES ('ICR PERMANENT STANDARD', 'CQ', 'ICR')
--INSERT INTO monitor.ExemptScheduleTypes (ScheduleName, ScheduleCode, SchedulePlanName) VALUES ('PAYE-PFH', 'CA', 'PAYE')
--INSERT INTO monitor.ExemptScheduleTypes (ScheduleName, ScheduleCode, SchedulePlanName) VALUES ('PAYE PERMANENT STANDARD', 'CP', 'PAYE')
--INSERT INTO monitor.ExemptScheduleTypes (ScheduleName, ScheduleCode, SchedulePlanName) VALUES ('REPAYE ALTERNATIVE', 'IA', 'REPAYE')
--INSERT INTO monitor.ExemptScheduleTypes (ScheduleName, ScheduleCode, SchedulePlanName) VALUES ('REVISED PAY AS YOU EARN', 'I5', 'REPAYE')
--INSERT INTO monitor.ExemptScheduleTypes (ScheduleName, ScheduleCode, SchedulePlanName) VALUES ('EXIT COUNSELING - REPAYE', 'RE', 'REPAYE')

--DELETE FROM monitor.EojReports
--INSERT INTO monitor.EojReports (EojReportId, ReportName) VALUES(1, 'Redisclosures')
--INSERT INTO monitor.EojReports (EojReportId, ReportName) VALUES(2, 'Payments too high - Prenotifications')
--INSERT INTO monitor.EojReports (EojReportId, ReportName) VALUES(3, 'Tasks Skipped - Max Limit')
--INSERT INTO monitor.EojReports (EojReportId, ReportName) VALUES(4, 'Tasks Skipped - Exempt Condition (no manual task created)')
--INSERT INTO monitor.EojReports (EojReportId, ReportName) VALUES(5, 'Tasks Forwarded - Task skipped and a manual task created')
--INSERT INTO monitor.EojReports (EojReportId, ReportName) VALUES(6, 'Tasks Cancelled')