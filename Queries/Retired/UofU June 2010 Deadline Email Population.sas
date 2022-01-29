************************************************
*** UofU June 2010 Deadline Email Population ***
***********************************************;
%LET RPTLIB = T:\SAS;
FILENAME REPORTZ "&RPTLIB/ULWEMAILPOP.LWEMAILPOPRZ";
FILENAME REPORT2 "&RPTLIB/ULWEMAILPOP.LWEMAILPOPR2";
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
CREATE TABLE DEMO AS
SELECT *
FROM CONNECTION TO DB2 (
SELECT DISTINCT A.DF_SPE_ACC_ID
		,TRIM(A.DM_PRS_1) || ' ' || TRIM(A.DM_PRS_LST) AS BOR_NAME
		,B.DX_EML_ADR
FROM OLWHRM1.PD01_PDM_INF A
INNER JOIN OLWHRM1.PD03_PRS_ADR_PHN B
	ON A.DF_PRS_ID = B.DF_PRS_ID
INNER JOIN OLWHRM1.SD02_STU_ENR D
	ON A.DF_PRS_ID = D.DF_PRS_ID_STU
INNER JOIN OLWHRM1.LN10_LON C
	ON A.DF_PRS_ID = C.BF_SSN
INNER JOIN OLWHRM1.GA01_APP E
	ON A.DF_PRS_ID = E.DF_PRS_ID_BR
WHERE B.DI_EML_ADR_VAL = 'Y'	 
	AND D.LC_STU_ENR_TYP IN ('F','H')
	AND D.LD_XPC_GRD > CURRENT DATE
	AND D.IF_OPS_SCL_RPT = '00367500'
	AND E.AF_APL_OPS_SCL = '00367500'
	AND E.AF_CUR_APL_OPS_LDR = '828476'
	AND B.DC_ADR != 'T'
	AND D.LF_CRT_DTS_SD02 = (
		SELECT MAX(Z.LF_CRT_DTS_SD02)
		FROM OLWHRM1.SD02_STU_ENR Z
		WHERE Z.DF_PRS_ID_STU = D.DF_PRS_ID_STU
		)

FOR READ ONLY WITH UR
);
DISCONNECT FROM DB2;

/*%PUT  SQLXRC= >>> &SQLXRC <<< ||| SQLXMSG= >>> &SQLXMSG >>> ;  ** INCLUDES ERROR MESSAGES TO SAS LOG  ;*/
/*%SQLCHECK;*/
/*QUIT;*/

ENDRSUBMIT;
DATA DEMO;
	SET WORKLOCL.DEMO;
RUN;

PROC SORT DATA=DEMO ;
	BY DF_SPE_ACC_ID;
RUN;
DATA _NULL_;
SET DEMO ;
FILE REPORT2 DELIMITER=',' DSD DROPOVER LRECL=32767;
IF _N_ = 1 THEN DO;
	PUT "ACCOUNT NUMBER,BORROWER NAME,E-MAIL ADDRESS";
END;
DO;
   PUT DF_SPE_ACC_ID @;
   PUT BOR_NAME @;
   PUT DX_EML_ADR $ ;
END;
RUN;

