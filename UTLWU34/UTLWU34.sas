/*LIBNAME DLGSUTWH DB2 DATABASE=DLGSUTWH OWNER=OLWHRM1;*/
/*%LET RPTLIB = %SYSGET(reportdir);*/
%LET RPTLIB = T:\SAS;
FILENAME REPORTZ "&RPTLIB/ULWU34.LWU34RZ";
FILENAME REPORT2 "&RPTLIB/ULWU34.LWU34R2";
LIBNAME  WORKLOCL  REMOTE  SERVER=CYPRUS  SLIBREF=WORK;
RSUBMIT;
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
CONNECT TO DB2 (DATABASE=DLGSUTWH);
CREATE TABLE FBRHELP AS
SELECT *
FROM CONNECTION TO DB2 (
SELECT DISTINCT A.DF_SPE_ACC_ID
	,A.DM_PRS_LST 	
	,A.DM_PRS_1
	,C.DX_STR_ADR_1
	,C.DX_STR_ADR_2
	,C.DM_CT
	,C.DC_DOM_ST
	,C.DF_ZIP_CDE
FROM OLWHRM1.PD10_PRS_NME A
INNER JOIN OLWHRM1.PD30_PRS_ADR C
	ON A.DF_PRS_ID = C.DF_PRS_ID
INNER JOIN OLWHRM1.LN10_LON B
	ON A.DF_PRS_ID = B.BF_SSN
INNER JOIN OLWHRM1.LN16_LON_DLQ_HST D
	ON B.BF_SSN = D.BF_SSN
	AND B.LN_SEQ = D.LN_SEQ
INNER JOIN OLWHRM1.DW01_DW_CLC_CLU E
	ON B.BF_SSN = E.BF_SSN
	AND B.LN_SEQ = E.LN_SEQ
WHERE B.LC_STA_LON10 = 'R'
	AND B.LA_CUR_PRI > 25
	AND DAYS(D.LD_DLQ_OCC) BETWEEN DAYS(CURRENT DATE) - 240 AND DAYS(CURRENT DATE) - 210
	AND D.LC_STA_LON16 = '1'
	AND E.WC_DW_LON_STA IN ('03','14')
	AND B.LI_CON_PAY_STP_PUR != 'Y'
	AND C.DI_VLD_ADR = 'Y'
	AND C.DC_ADR = 'L'
FOR READ ONLY WITH UR
);
DISCONNECT FROM DB2;
/*%PUT  SQLXRC= >>> &SQLXRC <<< ||| SQLXMSG= >>> &SQLXMSG >>> ;  ** INCLUDES ERROR MESSAGES TO SAS LOG  ;*/
/*%SQLCHECK;*/
/*QUIT;*/
ENDRSUBMIT;
DATA FBRHELP;
	SET WORKLOCL.FBRHELP;
RUN;
PROC SORT DATA=FBRHELP;
	BY DF_SPE_ACC_ID;
RUN;

DATA _NULL_;
SET FBRHELP ;
FILE REPORT2 DELIMITER=',' DSD DROPOVER LRECL=32767;
NAME = TRIM(DM_PRS_LST) || ', ' || TRIM(DM_PRS_1);
CTSTZIP = TRIM(DM_CT) || ', ' || TRIM(DC_DOM_ST) || ' ' || TRIM(DF_ZIP_CDE); 
DO;
   PUT DF_SPE_ACC_ID ;
   PUT NAME ;
   PUT DX_STR_ADR_1  ;
IF DX_STR_ADR_2 ^= '' THEN DO;
   PUT DX_STR_ADR_2  ;
END;
   PUT CTSTZIP $ ;
END;
RUN;