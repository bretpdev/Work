# UTLWS75

SCRA-UHEAA: This job will read the DOD return file that is downloaded from the SCRA/DOD website, insert information directly into the SCRA tables, and will track borrowers active duty status.

Details: First, the UTLWS75.dtsx package checks X:\PADD\FTP (live) or X:\PADD\FTP\Test (test) for a complete set of DOD return files (SCRA___ULWOO3). If no files exist, it produces a message stating so. If at least one file exists, it clears out ULS.scra._ DODReturnFile and ULS.scra._ DODReturnFileErrors. Next, it reads in each file from X:\PADD\FTP (live) or X:\PADD\FTP\Test (test), re-populates the two aforementioned tables, and moves the read files to X:\Archive\SCRA\ (live) or X:\Archive\SCRA\Test\ (test). Afterwards, it executes the SQL code found in UTLWS75.sql, which is the collective SQL code from the many Execute SQL Tasks in the UTLWS75.dtsx package. Finally, if all of the Execute SQL Tasks run successfully, it produces a message stating so.
