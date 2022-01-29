/*UTLWO57 - Align Forb QC Report*/
/*LIBNAME DLGSUTWH DB2 DATABASE=DLGSUTWH OWNER=OLWHRM1;*/
/*%LET RPTLIB = %SYSGET(reportdir);*/
%LET RPTLIB = T:\SAS;
FILENAME REPORTZ "&RPTLIB/ULWO57.LWO57RZ";
FILENAME REPORT2 "&RPTLIB/ULWO57.LWO57R2";
LIBNAME  WORKLOCL  REMOTE  SERVER=DUSTER  SLIBREF=WORK;
RSUBMIT;

%macro sqlcheck ;
  %if  &sqlxrc ne 0  %then  %do  ;
    data _null_  ;
            file reportz notitles  ;
            put @01 " ********************************************************************* "
              / @01 " ****  The SQL code above has experienced an error.               **** "
              / @01 " ****  The SAS should be reviewed.                                **** "       
              / @01 " ********************************************************************* "
              / @01 " ****  The SQL error code is  &sqlxrc  and the SQL error message  **** "
              / @01 " ****  &sqlxmsg   **** "
              / @01 " ********************************************************************* "
            ;
         run  ;
  %end  ;
%mend  ;

PROC SQL;
CONNECT TO DB2 (DATABASE=DLGSUTWH);
CREATE TABLE FORB AS
SELECT *
FROM CONNECTION TO DB2 (
SELECT DISTINCT
		D.DF_SPE_ACC_ID
		,A.LN_SEQ
		,A.LD_FOR_BEG
		,A.LD_FOR_END
		,A.LD_FOR_APL
		,B.LC_FOR_TYP

FROM	OLWHRM1.LN60_BR_FOR_APV A
		INNER JOIN OLWHRM1.FB10_BR_FOR_REQ B ON
			A.BF_SSN = B.BF_SSN
			AND A.LF_FOR_CTL_NUM = B.LF_FOR_CTL_NUM
			AND B.LC_FOR_TYP = '15'
			AND B.LC_FOR_STA = 'A'
			AND A.LC_STA_LON60 = 'A'
			AND A.LD_FOR_BEG >= '2006-07-01'
		INNER JOIN OLWHRM1.LN10_LON C ON
			A.BF_SSN = C.BF_SSN
			AND A.LN_SEQ = C.LN_SEQ
			AND C.IC_LON_PGM IN ('SUBCNS','UNCNS','SUBSPC','UNSPC','PLUS','PLUSGB')
			AND C.LA_CUR_PRI > 0
			AND C.LC_STA_LON10 = 'R'
		INNER JOIN OLWHRM1.PD10_PRS_NME D ON
			A.BF_SSN = D.DF_PRS_ID

ORDER BY DF_SPE_ACC_ID, LN_SEQ

FOR READ ONLY WITH UR
);
DISCONNECT FROM DB2;

/*%put  sqlxrc= >>> &sqlxrc <<< ||| sqlxmsg= >>> &sqlxmsg >>> ;  ** includes error messages to SAS log  ;*/
/*%sqlcheck;*/
/*quit;*/

ENDRSUBMIT;
DATA FORB; SET WORKLOCL.FORB; RUN;
DATA _NULL_;
SET FORB ;
LENGTH DESCRIPTION $600.;
USER = ' ' ;
ACT_DT = LD_FOR_APL ;
DESCRIPTION = CATX(',',
		DF_SPE_ACC_ID 
		,LN_SEQ 
		,PUT(LD_FOR_BEG, MMDDYY10.) 
		,PUT(LD_FOR_END, MMDDYY10.) 
);
FILE REPORT2 DELIMITER=',' DSD DROPOVER LRECL=32767;
FORMAT USER $10. ;
FORMAT ACT_DT MMDDYY10. ;
FORMAT DESCRIPTION $600. ;
IF _N_ = 1 THEN DO;
	PUT "USER,ACT_DT,DESCRIPTION";
END;
DO;
   PUT USER $ @;
   PUT ACT_DT @;
   PUT DESCRIPTION $ ;
END;
RUN;
