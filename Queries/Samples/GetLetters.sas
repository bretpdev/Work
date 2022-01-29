/*%LET RPTLIB = %SYSGET(reportdir);*/
%LET RPTLIB = T:\SAS;
FILENAME REPORTZ "&RPTLIB/UNWS46.NWS46RZ";
FILENAME REPORT2 "&RPTLIB/UNWS46.NWS46R2";

LIBNAME  LEGEND  REMOTE  SERVER=LEGEND SLIBREF=work  ;
RSUBMIT LEGEND;
/*%let DB = DNFPRQUT;  *This is test;*/
/*%let DB = DNFPRUUT;  *This is VUK3 test;*/
%let DB = DNFPUTDL;  *This is live;

LIBNAME PKUB DB2 DATABASE=&DB OWNER=PKUB;

%MACRO SQLCHECK ;
  %IF  &SQLXRC NE 0  %THEN  %DO  ;
    DATA _NULL_  ;
            FILE REPORTZ NOTITLES  ;
            PUT @01 " ********************************************************************* "
              / @01 " ****  THE SQL CODE ABOVE HAS EXPERIENCED AN ERROR.               **** "
              / @01 " ****  THE SAS SHOULD BE REVIEWED.                                **** "       
              / @01 " ********************************************************************* "
              / @01 " ****  THE SQL ERROR CODE IS  &SQLXRC  AND THE SQL ERROR MESSAGE  **** "
              / @01 " ****  &SQLXMSG   **** "
              / @01 " ********************************************************************* "
            ;
         RUN  ;
  %END  ;
%MEND  ;

PROC SQL;
	CONNECT TO DB2 (DATABASE=&DB);

	CREATE TABLE DEMO AS
		SELECT	
			*
		FROM	
			CONNECTION TO DB2 
				(
					SELECT 
						AC11.PF_LTR,
						AC11.PF_REQ_ACT,
						AY10.BF_SSN AS BWR,
						AY10.LF_ATY_RCP AS RECIP,
						LD_ATY_REQ_RCV AS REQUESTED

					FROM
						PKUB.AC11_ACT_REQ_LTR AC11
						JOIN PKUB.AY10_BR_LON_ATY AY10
							ON AY10.PF_REQ_ACT = AC11.PF_REQ_ACT
					WHERE
						AY10.BF_SSN ^= AY10.LF_ATY_RCP

					FOR READ ONLY WITH UR
				)
	;

	DISCONNECT FROM DB2;

	/*%PUT  SQLXRC= >>> &SQLXRC <<< ||| SQLXMSG= >>> &SQLXMSG >>> ;  ** INCLUDES ERROR MESSAGES TO SAS LOG  ;*/
	/*%SQLCHECK;*/
QUIT;

ENDRSUBMIT;


/*export to Excel spreadsheet*/
/*PROC EXPORT DATA = LEGEND.DEMO */
/*            OUTFILE = "T:\SAS\EXCEL OUTPUT1.xls" */
/*            DBMS = EXCEL*/
/*			REPLACE;*/
/*     SHEET="A"; */
/*RUN;*/

