# (RETIRED) CONADJFED
Consolidation Adjustment - FED

the purpose of this request is to create queue task for all loans in a file received from Navient

1.truncate CLS..ConsolAdjustmentFed; 
2.process each NAtoCSTLC% file in Q:\Operational Accounting\Federal Servicing\DCRs\Consolidation Loan Adjs\: 
   2a.get beginning count of CLS..ConsolAdjustmentFed records, 
   2b. rename NAtoCSTLC% file InProcess.xlsx so the Excel connection manager will connect to it (attempts to create a excel connection manager with a variable file name were unsuccessfule), 
   2c.load data from InProcess.xlsx range D3..D10,000 (column D contains the SSN which is the only data needed, rows 1 and 2 contain titles which confuse SSIS and have to be skipped, row 10,000 used as end of range to ensure all records are included even though most files only contain a few hundred records) into CLS..ConsolAdjustmentFed, 
   2d.rename InProcess.xlsx to restore the original file name with "Processed" at the beginning so it no longer matches the NAtoCSTLC% pattern (the users need the file for processing the queue tasks so it can't be deleted but renaming it prevents it from being processed again). 
   2e.get ending count of CLS..ConsolAdjustmentFed records, 
   2f.send an e-mail to warn users if 9,997 or more records were added (ending count - beginning count) as there may be records beyond row 10,000 in the file that did not get processed; 
3.create ArAddProcessing records for each record added to CLS..ConsolAdjustmentFed.
