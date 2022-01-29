/*%LET RPTLIB = %SYSGET(reportdir);*/
%LET RPTLIB = T:\SAS;
FILENAME REPORTZ "&RPTLIB/UNWS25.NWS25RZ";
FILENAME REPORT2 "&RPTLIB/UNWS25.NWS25R2";

DATA _NULL_;
     CALL SYMPUT('BEGIN',"'"||PUT(INTNX('MONTH',TODAY(),-1,'beginning'), MMDDYYD10.)||"'");
     CALL SYMPUT('END',"'"||PUT(INTNX('MONTH',TODAY(),-1,'end'), MMDDYYD10.)||"'");
     CALL SYMPUT('EFFDATE',TRIM(LEFT(UPCASE(
		PUT(INTNX('MONTH',TODAY(),-1), MONNAME9.)||' '||
		PUT(INTNX('MONTH',TODAY(),-1), YEAR4.)))));

/*	PUT &BEGIN;*/
/*	PUT &END;*/
RUN;
%SYSLPUT BEGIN = &BEGIN;
%SYSLPUT END = &END;

LIBNAME  LEGEND  REMOTE  SERVER=LEGEND SLIBREF=work  ;
RSUBMIT LEGEND;
%let DB = DNFPRQUT;  *This is test;
/*%let DB = DNFPUTDL;  *This is live;*/

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
CREATE TABLE ALL AS
	SELECT	*
	FROM	CONNECTION TO DB2 (
				SELECT	DISTINCT
						A.BF_SSN
						,A.PF_REQ_ACT
						,A.LD_ATY_REQ_RCV
						,CASE
							WHEN A.LD_ATY_REQ_RCV BETWEEN &BEGIN AND &END THEN 1
							ELSE 0
						 END AS LST_MON
				FROM	PKUB.AY10_BR_LON_ATY A
				WHERE	A.PF_REQ_ACT IN ('VIPSS','SPHAN')
						AND A.LC_STA_ACTY10 <> 'E'
						AND A.LD_ATY_REQ_RCV <= &END
				FOR READ ONLY WITH UR
			);

DISCONNECT FROM DB2;

CREATE TABLE CNTS AS
	SELECT	*
	FROM	(

				SELECT	1 AS ORD
						,'Total VIP Borrowers' AS LABL
						,COUNT(DISTINCT BF_SSN) AS CNT
				FROM	ALL
				WHERE	PF_REQ_ACT IN ('VIPSS')

			UNION

				SELECT	2 AS ORD
						,'VIPs added during the previous month' AS LABL
						,COUNT(DISTINCT BF_SSN) AS CNT
				FROM	ALL
				WHERE	PF_REQ_ACT IN ('VIPSS')
						AND LST_MON = 1
									
			UNION

				SELECT	3 AS ORD
						,'Total Special Handling Borrowers' AS LABL
						,COUNT(DISTINCT BF_SSN) AS CNT
				FROM	ALL
				WHERE	PF_REQ_ACT IN ('SPHAN')
			
			UNION

				SELECT	4 AS ORD
						,'Special Handling Borrowers added during the previous month' AS LABL
						,COUNT(DISTINCT BF_SSN) AS CNT
				FROM	ALL
				WHERE	PF_REQ_ACT IN ('SPHAN')
						AND LST_MON = 1
			)
	ORDER BY ORD
		;

/*%PUT  SQLXRC= >>> &SQLXRC <<< ||| SQLXMSG= >>> &SQLXMSG >>> ;  ** INCLUDES ERROR MESSAGES TO SAS LOG  ;*/
/*%SQLCHECK;*/
QUIT;

ENDRSUBMIT;
/*DATA ALL; SET LEGEND.ALL; RUN;*/
DATA CNTS; SET LEGEND.CNTS; RUN;


/*create printed report*/
PROC PRINTTO PRINT=REPORT2 NEW; RUN;
OPTIONS ORIENTATION = LANDSCAPE;
OPTIONS PS=39 LS=127;
TITLE 		'VIP/Special Handling Tracking - FED';
FOOTNOTE1  	"THIS DOCUMENT MAY CONTAIN BORROWERS' SENSITIVE INFORMATION THAT UHEAA HAS PLEDGED TO PROTECT.";
FOOTNOTE2	'PLEASE TAKE APPROPRIATE PRECAUTIONS TO SAFEGUARD THIS INFORMATION.';
FOOTNOTE3	;
FOOTNOTE4   'JOB = UTNWS25  	 REPORT = UNWS25.NWS25R2';

PROC PRINT NOOBS SPLIT='/' DATA=CNTS WIDTH=UNIFORM WIDTH=MIN LABEL;
VAR 	LABL CNT;
LABEL	LABL = 'Description'
		CNT = 'Count';
RUN;

PROC PRINTTO;
RUN;

/*PROC EXPORT DATA= WORK.ALL */
/*            OUTFILE= "T:\SAS\CSV OUTPUT.TXT" */
/*            DBMS=CSV REPLACE;*/
/*     PUTNAMES=YES;*/
/*RUN;*/
