/*%LET RPTLIB = %SYSGET(reportdir);*/
%LET RPTLIB = T:\SAS;
FILENAME REPORT2 "&RPTLIB/UNWIS2.NWIS2R2";

LIBNAME  LEGEND  REMOTE  SERVER=LEGEND SLIBREF=WORK  ;
RSUBMIT;
PROC SQL;
CONNECT TO DB2 (DATABASE=DNFPUTDL);
CREATE TABLE DEMO AS
SELECT *
	FROM CONNECTION TO DB2 (
	SELECT A.PF_USR
	,B.XM_USER
	,A.PF_REQ_ACT
		FROM PKUB.US50_USR_ACT_VLD A
			LEFT OUTER JOIN PKUB.APEX25 B
			ON A.PF_USR = B.XF_USR
FOR READ ONLY WITH UR
);
DISCONNECT FROM DB2;
/*%PUT  SQLXRC= >>> &SQLXRC <<< ||| SQLXMSG= >>> &SQLXMSG >>> ;  ** INCLUDES ERROR MESSAGES TO SAS LOG  ;*/
/*%SQLCHECK;*/
QUIT;
PROC SORT 
	DATA=DEMO; 
	BY PF_USR; 
RUN;

ENDRSUBMIT;
DATA DEMO;
	SET LEGEND.DEMO;
RUN;

DATA _NULL_;
	SET DEMO ;
	FILE REPORT2 DELIMITER=',' DSD DROPOVER LRECL=32767;
	IF _N_ = 1 THEN PUT "USER ID,USER NAME,ARC";
	PUT PF_USR XM_USER PF_REQ_ACT;
RUN;
