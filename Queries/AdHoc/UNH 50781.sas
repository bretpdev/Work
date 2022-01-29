/*TODO:  this job would be a cleaner if the data set could be updated directly on DUSTER (an encoding error is received by SS users who run the job)*/
%LET FILENAME = Range Assignments _ February 2017.xlsx;

*get data from Excel;
LIBNAME RANGES "T:\&FILENAME";

/*create library on DUSTER to progrevw directory*/
RSUBMIT;
LIBNAME PROGREVW V8 '/sas/whse/progrevw';
ENDRSUBMIT;

*copy data set to local work library, see TODO: above;
LIBNAME  DUSTER  REMOTE  SERVER=DUSTER  SLIBREF=PROGREVW;
DATA XREF; 
	SET DUSTER.XREF;
RUN;

*overwrite local dataset dropping unneeded columns and renaming a column from the spreadsheet;
DATA XREF (DROP=DEPT RENAME=(WINDOWS_USER_ID = WINDOWSUSERNAME));
	SET RANGES.'Sheet1$'n;
RUN;

*copy updated data set back out to DUSTER progrevw dir where it belongs;
DATA DUSTER.XREF; 
	SET XREF; 
RUN;
