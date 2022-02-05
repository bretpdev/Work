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

--DELETE FROM dashcache.DashboardCache
--DELETE FROM dashcache.DashboardItems

--INSERT INTO dashcache.DashboardItems (ItemName, UheaaSprocName, UheaaDatabase, CornerstoneSprocName, CornerstoneDatabase) 
--VALUES 
--('LT20 Letters', 'LT20_UHEAA', 'UDW', 'LT20_CS', 'CDW'),
--('ARC Add Processing', 'ARCADD_UHEAA', 'ULS', 'ARCADD_CS', 'CLS'),
--('E-Bill', 'EBILL_UHEAA', 'ULS', NULL, NULL),
--('DTX7L', 'DTX7L_UHEAA', 'ULS', NULL, NULL),
--('E-Corr XML Generator', 'ECORR_UHEAA', 'ECorrUheaa', 'ECORR_CS', 'ECorrFed'),
--('Repayment Disclosures', 'RPMTDISC_UHEAA', 'ULS', 'RPMTDISC_CS', 'CLS'),
--('SCRA Active Duty', 'SCRA_UHEAA', 'ULS', 'SCRA_CS', 'CLS'),
--('Check By Phone', 'CHECKBYPHONE_UHEAA', 'Norad', 'CHECKBYPHONE_CS', 'CLS'),
--('Automate Demographics', 'AUTODEMO_UHEAA', 'ULS', NULL, NULL),
--('Alternate Format Sent to NTIS', NULL, NULL, 'ALTNTIS_CS', 'ECorrFed'),
--('Centralized Printing', 'CNTRPRNT_UHEAA', 'BSYS', 'CNTRPRNT_CS', 'CLS')
