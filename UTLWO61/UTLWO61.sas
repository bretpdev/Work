*----------------------------------------*
| UTLWO61 PIF ON COMPASS AND NOT ONELINK |
*----------------------------------------*;
/*LIBNAME DLGSUTWH DB2 DATABASE=DLGSUTWH OWNER=OLWHRM1;*/
/*%LET RPTLIB = %SYSGET(reportdir);*/
%LET RPTLIB = T:\SAS;
FILENAME REPORT2 "&RPTLIB/ULWO61.LWO61R2";
FILENAME REPORT3 "&RPTLIB/ULWO61.LWO61R3";
FILENAME REPORTZ "&RPTLIB/ULWO61.LWO61RZ";
DATA _NULL_;
     CALL SYMPUT('RUNDT',PUT(INTNX('DAY',TODAY(),0,'BEGINNING'), MMDDYYD10.));
	 CALL SYMPUT('RUNTIME',PUT(TIME(), TIME.));
RUN;
LIBNAME  WORKLOCL  REMOTE  SERVER=DUSTER  SLIBREF=WORK;
RSUBMIT;
DATA LOAN_TYPES;
	FORMAT LN_TYP LN_PGM $50.;
	INFILE "/sas/whse/progrevw/GENR_REF_LoanTypes.txt" DLM=',' MISSOVER DSD;
	INFORMAT LN_TYP LN_PGM $50.;
	INPUT LN_TYP LN_PGM ;
	LN_PGM = UPCASE(LN_PGM);
RUN;
/*CREATE MACRO VARIALBE LISTS OF LOAN PROGRAMS(FFEL AND PRIVATE LOANS)*/
PROC SQL NOPRINT;
	SELECT "'"||TRIM(LN_TYP)||"'" 
		INTO :PRIVATE_LIST SEPARATED BY "," /*PRIVATE LOAN LIST*/
	FROM LOAN_TYPES
	WHERE LN_PGM ^= 'FFEL';
QUIT;
%MACRO SQLCHECK (SQLRPT= );
%IF &SQLXRC NE 0 %THEN %DO;
	DATA _NULL_;
    FILE REPORTZ NOTITLES;
    PUT @01 " ********************************************************************* "
      / @01 " ****  THE SQL CODE ABOVE HAS EXPERIENCED AN ERROR.               **** "
      / @01 " ****  THE SAS LOG IN &SQLRPT SHOULD BE REVIEWED.          **** "       
      / @01 " ********************************************************************* "
      / @01 " ****  THE SQL ERROR CODE IS  &SQLXRC  AND THE SQL ERROR MESSAGE  **** "
      / @01 " ****  &SQLXMSG   **** "
      / @01 " ********************************************************************* ";
	RUN;
%END;
%MEND;
PROC SQL;
CONNECT TO DB2 (DATABASE=DLGSUTWH);
CREATE TABLE LPOCANO AS
SELECT *
FROM CONNECTION TO DB2 (
SELECT DISTINCT A.LD_PIF_RPT
	,D.DF_SPE_ACC_ID
	,E.AF_APL_ID||E.AF_APL_ID_SFX AS CLUID
	,A.LA_CUR_PRI
	,E.AD_PRC
	,A.LD_STA_LON10
	,A.LC_STA_LON10
	,B.WC_DW_LON_STA
FROM OLWHRM1.LN10_LON A
INNER JOIN OLWHRM1.DW01_DW_CLC_CLU B
	ON A.BF_SSN = B.BF_SSN
	AND A.LN_SEQ = B.LN_SEQ
INNER JOIN OLWHRM1.GA14_LON_STA C
	ON A.LF_LON_ALT = C.AF_APL_ID
	AND A.LN_LON_ALT_SEQ = INT(C.AF_APL_ID_SFX)
INNER JOIN OLWHRM1.PD10_PRS_NME D
	ON A.BF_SSN = D.DF_PRS_ID
INNER JOIN OLWHRM1.GA10_LON_APP E
	ON C.AF_APL_ID = E.AF_APL_ID
	AND C.AF_APL_ID_SFX = E.AF_APL_ID_SFX
WHERE A.IC_LON_PGM NOT IN (&PRIVATE_LIST)
	AND A.BF_SSN IN (SELECT BF_SSN
				FROM OLWHRM1.LN10_LON 
				WHERE LC_STA_LON10 ^= 'D')
	AND B.BF_SSN IN (SELECT BF_SSN
				FROM OLWHRM1.DW01_DW_CLC_CLU 
				WHERE WC_DW_LON_STA IN ('22','09'))
	AND C.AC_STA_GA14 = 'A'
	AND C.AC_LON_STA_TYP NOT IN ('PF','PN','CA')
 /*LETTER 'O' NUMBER 1*/
	AND E.AC_APL_REJ_REA_1 != 'O1'
	AND E.AC_APL_REJ_REA_2 != 'O1'
	AND E.AC_APL_REJ_REA_3 != 'O1'
	AND E.AC_APL_REJ_REA_4 != 'O1'
	AND E.AC_APL_REJ_REA_5 != 'O1'
	AND A.LC_STA_LON10 = 'R'
FOR READ ONLY WITH UR
);
DISCONNECT FROM DB2;
/*%PUT  SQLXRC= >>> &SQLXRC <<< ||| SQLXMSG= >>> &SQLXMSG >>>  ;  * INCLUDES ERROR MESSAGES TO SAS LOG  ;*/
/*%SQLCHECK (SQLRPT=ULWO61.LWO61RZ);*/
/*QUIT;*/

PROC SORT DATA=LPOCANO ;
BY CLUID DESCENDING LD_STA_LON10;
RUN;
DATA LPOCANO (DROP=LC_STA_LON10 WC_DW_LON_STA A LD_STA_LON10 LA_CUR_PRI);
SET LPOCANO;
BY CLUID DESCENDING LD_STA_LON10;
RETAIN A;
IF FIRST.CLUID THEN DO;
	A = 0;
	IF LC_STA_LON10 ^= 'D' AND WC_DW_LON_STA IN ('22','09') AND LA_CUR_PRI NOT > 0 THEN A=1;
END;
IF A=1 AND LC_STA_LON10 ^= 'D' AND WC_DW_LON_STA IN ('22','09') THEN OUTPUT;
RUN;

ENDRSUBMIT;
DATA LPOCANO;
	SET WORKLOCL.LPOCANO;
RUN;

DATA LPOCANO2 LPOCANO_R3;
SET LPOCANO;
IF LD_PIF_RPT <= TODAY() - 14 THEN OUTPUT LPOCANO_R3;
IF LD_PIF_RPT >= TODAY() - 10 THEN OUTPUT LPOCANO2;
RUN;
PROC PRINTTO PRINT=REPORT2 NEW;
RUN;
OPTIONS ORIENTATION = PORTRAIT;
OPTIONS PS=52 LS=96 NODATE CENTER PAGENO=1;
TITLE 'PIF on Compass and Not OneLINK';
TITLE2 "&RUNDT - &RUNTIME";
FOOTNOTE 'JOB = UTLWO61  	 REPORT = ULWO61.LWO61R2';
PROC CONTENTS DATA=LPOCANO2 OUT=EMPTYSET NOPRINT;
DATA _NULL_;
SET EMPTYSET;
FILE PRINT;
IF  NOBS=0 AND _N_ =1 THEN DO;
	PUT // 95*'-';
	PUT      //////////
		@40 '**** NO OBSERVATIONS FOUND ****';
	PUT //////////
		@46 '-- END OF REPORT --';
	PUT ///////////////////
		@36 "JOB = UTLWO61  	 REPORT = ULWO61.LWO61R2";
	END;
RETURN;
RUN;
PROC PRINT NOOBS SPLIT='/' DATA=LPOCANO2 WIDTH=UNIFORM WIDTH=MIN;
FORMAT AD_PRC MMDDYY10.;
VAR DF_SPE_ACC_ID CLUID AD_PRC;
LABEL DF_SPE_ACC_ID = 'Account Number' CLUID = 'UNIQUE ID' AD_PRC = 'GUARANTEE DATE';
RUN;
PROC PRINTTO;
RUN; 
PROC SORT DATA=LPOCANO_R3 NODUPKEY;
	BY DF_SPE_ACC_ID AD_PRC CLUID;
RUN;
DATA LPOCANO_R3 ;
SET LPOCANO_R3;
	BY DF_SPE_ACC_ID AD_PRC CLUID;
LENGTH A $600;
RETAIN A;
IF FIRST.AD_PRC THEN A = CLUID;
ELSE IF FIRST.CLUID THEN A = CATX(' ',A,CLUID);
IF LAST.AD_PRC THEN OUTPUT;
RUN;

DATA _NULL_;
SET LPOCANO_R3 ;
LENGTH DESCRIPTION $600.;
USER = ' ';
FORMAT ACT_DT MMDDYY10. ;
ACT_DT = LD_PIF_RPT ;
DESCRIPTION = CATX(',',
	"ACCOUNT NUMBER " || DF_SPE_ACC_ID
	,"UNIQUE ID " || A
	,"GUARANTEE DATE " ||PUT(AD_PRC, MMDDYY10.)
);
FILE REPORT3 DELIMITER=',' DSD DROPOVER LRECL=32767;
FORMAT USER $10. ;
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
