/*%LET RPTLIB = %SYSGET(reportdir);*/
%LET RPTLIB = T:\SAS;
FILENAME REPORTZ "&RPTLIB/UNWSXX.NWSXXRZ";
FILENAME REPORTX "&RPTLIB/UNWSXX.NWSXXRX";

LIBNAME  LEGEND  REMOTE  SERVER=LEGEND SLIBREF=work  ;
RSUBMIT LEGEND;
/*%let DB = DNFPRQUT;  *This is test;*/
/*%let DB = DNFPRUUT;  *This is VUKX test;*/
%let DB = DNFPUTDL;  *This is live;

data _null_;
	runday = today() - X;
	call symput('eom',intnx('month',runday,X,'E'));
	call symput('eomt',"'"||put(intnx('month',runday,X,'E'),mmddyyXX.)||"'");
     CALL SYMPUT('BEGIN',"'"||PUT(INTNX('MONTH',TODAY(),X,'beginning'), MMDDYYDXX.)||"'");
run;

%put &eom;
%put &eomt;
%PUT &BEGIN;

LIBNAME PKUB DBX DATABASE=&DB OWNER=PKUB;

%MACRO SQLCHECK ;
  %IF  &SQLXRC NE X  %THEN  %DO  ;
    DATA _NULL_  ;
            FILE REPORTZ NOTITLES  ;
            PUT @XX " ********************************************************************* "
              / @XX " ****  THE SQL CODE ABOVE HAS EXPERIENCED AN ERROR.               **** "
              / @XX " ****  THE SAS SHOULD BE REVIEWED.                                **** "       
              / @XX " ********************************************************************* "
              / @XX " ****  THE SQL ERROR CODE IS  &SQLXRC  AND THE SQL ERROR MESSAGE  **** "
              / @XX " ****  &SQLXMSG   **** "
              / @XX " ********************************************************************* "
            ;
         RUN  ;
  %END  ;
%MEND  ;

PROC SQL;
	CONNECT TO DBX (DATABASE=&DB);

	CREATE TABLE DEMO AS
		SELECT	
			*
		FROM	
			CONNECTION TO DBX 
				(
					SELECT DISTINCT
						WQXX.WF_QUE
						,WQXX.WF_SUB_QUE
						,WQXX.WN_CTL_TSK
						,WQXX.PF_REQ_ACT
/*						,WQXX.BF_SSN*/
/*						WQXX.**/
/*						,PDXX.DF_SPE_ACC_ID*/
					FROM
						PKUB.WQXX_TSK_QUE WQXX
/*						JOIN PKUB.PDXX_PRS_NME PDXX*/
/*							ON WQXX.BF_SSN = PDXX.DF_PRS_ID*/
					WHERE
						WQXX.WF_QUE = 'RX'
						AND 
						WQXX.WF_SUB_QUE = 'XX'
/*						AND */
/*						WQXX.WC_STA_WQUEXX = 'C'*/
/*						AND */
/*						DAYS(WQXX.WF_LST_DTS_WQXX) BETWEEN DAYS(&BEGIN) AND DAYS(&EOMT)*/
/*						AND */
/*						PDXX.DF_SPE_ACC_ID = 'XXXXXXXXXX'*/

					FOR READ ONLY WITH UR
				)
	;

	DISCONNECT FROM DBX;

	/*%PUT  SQLXRC= >>> &SQLXRC <<< ||| SQLXMSG= >>> &SQLXMSG >>> ;  ** INCLUDES ERROR MESSAGES TO SAS LOG  ;*/
	/*%SQLCHECK;*/
QUIT;

ENDRSUBMIT;

DATA DEMO; SET LEGEND.DEMO; RUN;

/*export to Excel spreadsheet*/
PROC EXPORT DATA = WORK.DEMO 
            OUTFILE = "T:\SAS\NH XXXX.xlsx" 
            DBMS = EXCEL
			REPLACE;
     SHEET="A"; 
RUN;
