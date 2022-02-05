# UTNW021

This job will read the file that is downloaded from the SCRA/DOD website for active duty borrowers and provide the borrowers/loans with an interest rate > 6%. The job will feed into ArcAddProcessing table to create a queue for Loan Servicing Script to work.

Details:
First, the UTNW021.dtsx package checks Z:\Batch\FTP (live) or Y:\Batch\FTP (test) for a complete set of DOD return files (SCRA___UTNWS81). If no files exist, it produces a message stating so.  If at least one file exists, it clears out CLS.scra._ DODReturnFile and CLS.scra._ DODReturnFileErrors. Next, it reads in each file from Z:\Batch\FTP (live) or Y:\Batch\FTP (test), re-populates the two aforementioned tables, and moves the read files to  Z:\Archive\SCRA\ (live) or Y:\Archive\SCRA\ (test). Afterwards, it executes the SQL code found in UTNW021.sql, which is the collective SQL code from the many Execute SQL Tasks in the UTNW021.dtsx package. Finally, if all of the Execute SQL Tasks run successfully, it produces a message stating so.
