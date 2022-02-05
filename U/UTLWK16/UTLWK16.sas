/*LIBNAME DLGSUTWH DB2 DATABASE=DLGSUTWH OWNER=OLWHRM1;*/
/*%LET RPTLIB = %SYSGET(reportdir);*/
FILENAME REPORT2 "T:\SAS\ULWK16.LWK16R2";
/*FILENAME REPORT2 "&RPTLIB/ULWK16.LWK16R2";*/
/*FILENAME REPORTZ "&RPTLIB/ULWK16.LWK16RZ";*/
LIBNAME  WORKLOCL  REMOTE  SERVER=CYPRUS  SLIBREF=WORK;
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
CREATE TABLE SKIP AS
SELECT *
FROM CONNECTION TO DB2 (

SELECT DISTINCT A.BF_SSN AS SSN
	,MAX(D.LD_ATY_REQ_RCV) AS LD_ATY_REQ_RCV
	,C.DI_PHN_VLD
	,B.DI_VLD_ADR
	,B.DD_VER_ADR
	,C.DD_PHN_VER 
	,E.DF_SPE_ACC_ID

FROM OLWHRM1.LN10_LON A
INNER JOIN OLWHRM1.PD30_PRS_ADR B
	ON A.BF_SSN = B.DF_PRS_ID
	AND B.DC_ADR = 'L'
INNER JOIN OLWHRM1.PD42_PRS_PHN C
	ON A.BF_SSN = C.DF_PRS_ID 
	AND C.DC_PHN = 'H'
INNER JOIN OLWHRM1.AY10_BR_LON_ATY D
	ON A.BF_SSN = D.BF_SSN
INNER JOIN OLWHRM1.PD01_PDM_INF E
	ON D.BF_SSN = E.DF_PRS_ID

WHERE A.LA_CUR_PRI > 0
AND A.LC_STA_LON10 = 'R'
AND (C.DI_PHN_VLD = 'N' OR B.DI_VLD_ADR = 'N')
AND D.PF_REQ_ACT = 'SKPME'
AND A.BF_SSN NOT IN (SELECT DISTINCT ZZ.BF_SSN 
				FROM OLWHRM1.AY10_BR_LON_ATY ZZ
				INNER JOIN OLWHRM1.AC10_ACT_REQ YY
					ON YY.PF_REQ_ACT = ZZ.PF_REQ_ACT
					AND YY.PC_CCI_CLM_COL_ATY IN ('SD', 'SO', 'SS', 'YY')
				WHERE DAYS(ZZ.LD_ATY_RSP) >= DAYS(CURRENT DATE) - 40)
GROUP BY A.BF_SSN, C.DI_PHN_VLD, B.DI_VLD_ADR, B.DD_VER_ADR, C.DD_PHN_VER,E.DF_SPE_ACC_ID

FOR READ ONLY WITH UR
);

DISCONNECT FROM DB2;

/*%put  sqlxrc= >>> &sqlxrc <<< ||| sqlxmsg= >>> &sqlxmsg >>> ;  ** includes error messages to SAS log  ;*/
/*%sqlcheck;*/
/*quit;*/

ENDRSUBMIT;

DATA SKIP; SET WORKLOCL.SKIP; RUN;

DATA SKIP; 
SET SKIP;
FORMAT CRTB MMDDYY10.;
IF LD_ATY_REQ_RCV > TODAY() - 40 THEN CRTA = 'X';
IF DI_PHN_VLD = 'N' AND 
	DI_VLD_ADR = 'N' AND 
	DD_PHN_VER >= DD_VER_ADR 
	THEN CRTB = DD_PHN_VER;
ELSE IF DI_PHN_VLD = 'N' AND 
	DI_VLD_ADR = 'N' AND 
	DD_PHN_VER < DD_VER_ADR 
	THEN CRTB = DD_VER_ADR;
ELSE IF DI_PHN_VLD = 'N' AND 
	DI_VLD_ADR = 'Y' 
	THEN CRTB = DD_PHN_VER;
ELSE IF DI_PHN_VLD = 'Y' AND 
	DI_VLD_ADR = 'N' 
	THEN CRTB = DD_VER_ADR;
RUN;

DATA SKIP (DROP=CRTA CRTB);
SET SKIP;
IF CRTA ^= 'X' AND CRTB < TODAY() - 30 THEN OUTPUT;
RUN;
	
PROC SORT DATA=SKIP;
BY SSN;
RUN;
DATA _NULL_;
SET SKIP ;
LENGTH DESCRIPTION $600.;
USER = ' ';
ACT_DT = LD_ATY_REQ_RCV ;
DESCRIPTION = TRIM(DF_SPE_ACC_ID) || 
		',' || TRIM(PUT(LD_ATY_REQ_RCV, MMDDYY10.)) || 
		',' || TRIM(DI_VLD_ADR) ||
		',' || TRIM(DI_PHN_VLD) 
;
FILE REPORT2 DELIMITER=',' DSD DROPOVER LRECL=32767;
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
