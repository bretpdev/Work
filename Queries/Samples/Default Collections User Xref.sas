/*FOR THE UTLWP01 JOB, users are assigned based on the last 2 digits*/
/*of the borrowers ssn.  The assignments are stored in our local */
/*tables.  The data is copied to duster so AES can run the report.*/
/*This job should be run every time we have had a change to the local*/
/*tables.*/
options remote= cyprus ;
signon ;
LIBNAME BSYS ODBC USER="sqlserver;" PASSWORD="procauto;" REQUIRED="FILEDSN=X:\PADR\ODBC\BSYS.dsn;" ;
DATA XREF(DROP=DEPT);
SET BSYS.MABC_SSN_USER_XREF;
WHERE DEPT = 'Account Resolution';
RUN;
LIBNAME  WORKLOCL  REMOTE  SERVER=CYPRUS  SLIBREF=WORK;
DATA WORKLOCL.XREF;
SET WORK.XREF;
RUN;
RSUBMIT; 
LIBNAME SASTAB '/sas/whse/progrevw';
DATA SASTAB.XREF;
SET WORK.XREF;
RUN;
 
ENDRSUBMIT;
