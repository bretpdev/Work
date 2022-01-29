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
GRANT EXECUTE ON SCHEMA::aesxmtdial TO db_executor

TRUNCATE TABLE aesxmtdial.OnelinkDialerQueues

INSERT INTO aesxmtdial.OnelinkDialerQueues(Region,[Type],[Queue],[Name],Alias)
VALUES('DFT','C','30DAYNTC','COLLECTIONS 30 DAY N','COL 30 DAY NOTICE'),
('DFT','C','ACHFOLLO','DFLT ACH FOLLOW','ACH FOLLOW'),
('DFT','C','GRNREHAB','REHAB ELIGIBLE GARNI','RHB ELGBL GARNISH'),
('DFT','C','NOTELGRH','NOT ELGIBLE FOR REHA','NOT ELGIBLE FOR RHB'),
('DFT','C','PAYELRHB','PAYING ELIGBLE NOT Y','PAYING ELIGIBL'),
('DFT','C','REHABDLQ','REHAB PTNL DELQ','REHAABDLQ'),
('DFT','C','REHABLTR','REHAB SELECTION LTR','REHABSEL'),
('DFT','C','WG000002','WG000002 6 MNTH ACCT','DEFAULT NO PAY 6 MIN'),
('DFT','C','WG000003','WG000003 PAID LESS T','DFT PD LESS THAN DUE'),
('DFT','C','WG000004','WG000004 NO DFLT 4 L','DEFAULT 36 TO 90'),
('DFT','C','WG000005','WG000005 36-90 DFLT','DEFT 36 TO 90 LAC'),
('DFT','C','WG000006','WG000006 NO DFLT 4 L','DEFT NO PAY 90 PLUS'),
('DFT','C','WG000007','WG000007 90+ DFLT 4','DTT NO PY 90 PLS LAC'),
('DFT','C','WG000008','WG000008 DFLT 4 LETT','DFT NO PAY 6 PLUS'),
('DFT','C','WG000009','WG000009 6= DFLT 4 L','DFT NO PY 6 PLUS LAC'),
('DFT','C','WG000010','WG000010 6 MO+ NO PA','DFT 6 MONTH REVIEW'),
('DFT','C','WG000011','WG000011 90+ (OLD)','DFT 90 PLUS'),
('DFT','C','WG000012','WG000012 6+OLD','DFT NO PAY 6 PLUS'),
('DFT','C','WG000024','SPLIT','DEFT SPLIT'),
('DFT','C','WG000031','REJECT','DEFT REJECT'),
('DFT','C','WG000034','WG000034 6- DFLT 4 L','DEFT NO PY 6 MIN LAC'),
('DFT','C','WG000035','CATCH ALL','DEFT CATCH ALL'),
('DFT','S','AMNESTY','DFT BORROWERS OFFERR','COMPROMISE'),
('DFT','S','DEF10107','DFT LNS DISB','DEFT100107'),
('DFT','S','DRHBAWG','SOLICIT VOL PAY TO A','REHAB AWG BORROWERS'),
('DFT','S','REHABCAN','3 TO 8 REHABILITATIO','REHAB CANDIDATES'),
('DFT','S','REHABNOW','OVER 9 PMTS BUT NO R','REHAB READY'),
('DFT','S','RHBCAN6','BORROWER MADE AT LEA','6 REHAB PAYMENTS'),
('DFT','S','RHBCAN7','BORROWER MADE AT LEA','7 REHAB PAYMENTS'),
('DFT','S','RHBCAN8','BORROWER MADE AT LEA','8 REHAB PAYMENTS'),
('DFT','S','RHBCANJD','BORROWER MADE RHB PY','JD REHAB PYMNTS'),
('PRE','C','OVER18MO','OVER 18 MOS','OVER 18 MOS'),
('PRE','C','WG000100','CONVERSN','CONVERSION QUEUE'),
('PRE','C','WG000107','P SKPFND','WG000107'),
('PRE','C','WG000108','S SKPFND','WG000108'),
('PRE','C','WG000109','E SKPFND','WG000109'),
('PRE','C','WG000110','WG000110 31-60','WG000110'),
('PRE','C','WG000111','WG000111 60-95','WG000111'),
('PRE','C','WG000112','WG000112 60-95 C','WG000112'),
('PRE','C','WG000113','WG000113 96-119','WG000113'),
('PRE','C','WG000114','WG000114 96-119 C','WG000114'),
('PRE','C','WG000115','WG000115 120-240','WG000115'),
('PRE','C','WG000116','WG000116 120-240C','WG000116'),
('PRE','C','WG000117','WG000117 241-270','WG000117'),
('PRE','C','WG000118','WG000118 241-270C','WG000118'),
('PRE','C','WG000119','WG000119 180+ LS','WG000119'),
('PRE','C','WG000120','WG000120 271-330','WG000120'),
('PRE','C','WG000121','WG000121 331-360','WG000121'),
('PRE','C','WG000122','WG000122 271-360','WG000122'),
('PRE','C','WG000123','WG00123 360+','WG000123'),
('PRE','C','WG000125','WG000125 360+ DND','WG000125'),
('PRE','C','WG000126','CATCHALL','QUEUE ASSGN CATCHALL'),
('PRE','S','CCOHORTS','BORROWERS IN THE CUR','CURRENT COHORTS'),
('PRE','S','CUMEMPHN','BORROWERS WITH CU HO','BORRS WITH CU HOLDS'),
('PRE','S','CUPHNEML','BORROWERS WITH CU HO','BORRS CU HOLD NO EML'),
('PRE','S','FCOHORTS','BORROWERS IN THE FUT','FUTURE COHORTS'),
('PRE','S','P3YCHRT','NEW 3 YR COHORT SELE','P3YR COHORTS'),
('PRE','S','PCHRT300','CHRT TASKS GREATER T','PCHRT300'),
('PRE','S','PRECO120','PRECLAIM CONDUIT 120','PRECON120'),
('PRE','S','PRECO150','PRECLAIM CONDUIT 150','PRECON150'),
('PRE','S','PRECO180','PRECLAIM CONDUIT 180','PRECON180'),
('PRE','S','PRECON60','PRECLAIM CONDUIT 60-','PRECON60'),
('PRE','S','PRECON90','PRECLAIM CONDUIT 90-','PRECON90'),
('PRE','S','PREREHAB','REHABED ACCOUNT W AC','REHAB PRECLAIM'),
('PRE','S','ZEROFEE','ZERO FEE BORROWER CA','ZERO FEE CALLS'),
('SKP','C','BRWRCAL1','BORROWER FIRST CALL','BRWR FIRST CALL'),
('SKP','C','BRWRCAL2','BORROWER SECOND CALL','BRWR SECOND CALL'),
('SKP','C','BRWRCAL3','BORROWER THIRD CALL','BRWR THIRD CALL'),
('SKP','C','ENDRCAL1','ENDORSER FIRST CALL','ENDORSER FIRST CALL'),
('SKP','C','ENDRCAL2','ENDORSER SECOND CALL','ENDORSER SECOND CALL'),
('SKP','C','ENDRCAL3','ENDORSER THIRD CALL','ENDORSER THIRD CALL'),
('SKP','C','REFRCAL1','REFERENCE FIRST CALL','REFERENCE FIRST CALL'),
('SKP','C','REFRCAL2','REFERENCE SECOND CAL','REFERENCE SECOND CAL'),
('SKP','C','REFRCAL4','REFERENCE CALL 4TH A','4TH ATTEMPT REFRCALL'),
('SKP','C','RNEARBY1','REQUEST FOR NEARBYS','REQUEST FOR NEARBYS')


UPDATE DispositionCodeMapping SET OnelinkDisposition = 38 WHERE DispositionCode = 'Not In Service'
UPDATE DispositionCodeMapping SET OnelinkDisposition = 38 WHERE DispositionCode = 'Operator Intercept'
UPDATE DispositionCodeMapping SET OnelinkDisposition = 38 WHERE DispositionCode = 'Invalid'
UPDATE DispositionCodeMapping SET OnelinkDisposition = 16 WHERE DispositionCode = 'No Answer'
UPDATE DispositionCodeMapping SET OnelinkDisposition = 11 WHERE DispositionCode = 'Busy'
UPDATE DispositionCodeMapping SET OnelinkDisposition = 61 WHERE DispositionCode = 'Success'
UPDATE DispositionCodeMapping SET OnelinkDisposition = 11 WHERE DispositionCode = 'Fax'
UPDATE DispositionCodeMapping SET OnelinkDisposition = 13 WHERE DispositionCode = 'Answering Machine'
UPDATE DispositionCodeMapping SET OnelinkDisposition = 61 WHERE DispositionCode = 'Callback'
UPDATE DispositionCodeMapping SET OnelinkDisposition = 60 WHERE DispositionCode = 'Wrong Number'
UPDATE DispositionCodeMapping SET OnelinkDisposition = 82 WHERE DispositionCode = 'Wrong Person'
UPDATE DispositionCodeMapping SET OnelinkDisposition = 61 WHERE DispositionCode = 'Promise To Pay'
UPDATE DispositionCodeMapping SET OnelinkDisposition = 61 WHERE DispositionCode = 'Borrower Made A Payment'
UPDATE DispositionCodeMapping SET OnelinkDisposition = 61 WHERE DispositionCode = 'Borrower Hung Up'
UPDATE DispositionCodeMapping SET OnelinkDisposition = 61 WHERE DispositionCode = 'Forms Sent'
UPDATE DispositionCodeMapping SET OnelinkDisposition = 82 WHERE DispositionCode = 'Left Message with 3rd party'
UPDATE DispositionCodeMapping SET OnelinkDisposition = 82 WHERE DispositionCode = 'Not Available To Contact'
UPDATE DispositionCodeMapping SET OnelinkDisposition = 61 WHERE DispositionCode = 'Borrower promise to return form'
UPDATE DispositionCodeMapping SET OnelinkDisposition = 61 WHERE DispositionCode = 'Call Transferred to Servicer'
UPDATE DispositionCodeMapping SET OnelinkDisposition = 61 WHERE DispositionCode = 'Call Escalated to TL or Supervisor'
UPDATE DispositionCodeMapping SET OnelinkDisposition = 61 WHERE DispositionCode = 'Reference Contact - No demographic info verified or provided'
UPDATE DispositionCodeMapping SET OnelinkDisposition = 61 WHERE DispositionCode = 'Reference Contact - Demographic info verified and/or new info provided'
UPDATE DispositionCodeMapping SET OnelinkDisposition = 61 WHERE DispositionCode = 'Borrower Contact - No demographic info verified or provided'
UPDATE DispositionCodeMapping SET OnelinkDisposition = 61 WHERE DispositionCode = 'Borrower Contact - Demographic Info Verified and/or New Info Provided'
UPDATE DispositionCodeMapping SET OnelinkDisposition = 82 WHERE DispositionCode = 'Person Not Reference/Borrower - No Demographic Info Verified or Provided'
UPDATE DispositionCodeMapping SET OnelinkDisposition = 82 WHERE DispositionCode = 'Person Not Reference/Borrower - Demographic Info Verified and/or New Info Provided'