/*MASTER JOB*/
%LET RPTLIB = Y:\Batch\FTP;
%LET LOGLIB = Y:\Batch\Logs;
%LET CODELIB = Y:\Codebase\SAS\CDW;

x """C:\Program Files\SASHome\x86\SASFoundation\9.3\sas.exe"" -icon 
	-CONFIG ""&CODELIB\win7config.CFG""
	-sysin ""&CODELIB\SLAVE.sas"" 
	-LOG ""&LOGLIB\SLAVE.LOG""
	-SYSPARM ""&CODELIB &RPTLIB"""; /*command line args to pass values to slave job*/


/*SLAVE JOB*/
/*put this step in the slave job to parse the values of the command line args out of the SYSPARM system macro variable*/
DATA _NULL_;
	CALL SYMPUT('CODELIB',SCAN("&SYSPARM",1));
	CALL SYMPUT('RPTLIB',SCAN("&SYSPARM",2));
RUN;
