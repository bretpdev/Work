*---------------------------------------*
| UTLWS29 Ray of Hope - E-mail campaign |
*---------------------------------------*;
LIBNAME DLGSUTWH DB2 DATABASE=DLGSUTWH OWNER=OLWHRM1;
%LET RPTLIB = %SYSGET(reportdir);
/*%LET RPTLIB = T:\SAS;*/
FILENAME REPORT2 "&RPTLIB/ULWS29.LWS29R2";
FILENAME REPORTZ "&RPTLIB/ULWS29.LWS29RZ";
/*LIBNAME  WORKLOCL  REMOTE  SERVER=CYPRUS  SLIBREF=WORK;*/
/*RSUBMIT;*/
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
CREATE TABLE ROHEC AS
SELECT *
FROM CONNECTION TO DB2 (
SELECT DISTINCT A.BF_SSN
	,RTRIM(D.DM_PRS_1)||' '||DM_PRS_LST AS NAME
	,C.DX_ADR_EML
	,D.DF_SPE_ACC_ID
FROM OLWHRM1.LN10_LON A
INNER JOIN OLWHRM1.DW01_DW_CLC_CLU B
	ON A.BF_SSN = B.BF_SSN
	AND A.LN_SEQ = B.LN_SEQ
INNER JOIN OLWHRM1.PD32_PRS_ADR_EML C 
	ON B.BF_SSN = C.DF_PRS_ID
INNER JOIN OLWHRM1.PD10_PRS_NME D
	ON C.DF_PRS_ID = D.DF_PRS_ID
WHERE A.LA_CUR_PRI > 0
	AND A.LC_STA_LON10 = 'R'
	AND B.WC_DW_LON_STA = '03'
	AND C.DI_VLD_ADR_EML = 'Y'

FOR READ ONLY WITH UR
);
DISCONNECT FROM DB2;
%PUT  SQLXRC= >>> &SQLXRC <<< ||| SQLXMSG= >>> &SQLXMSG >>>  ;  * INCLUDES ERROR MESSAGES TO SAS LOG  ;
%SQLCHECK (SQLRPT=ULWS29.LWS29RZ);
QUIT;
/*ENDRSUBMIT;*/
/*DATA ROHEC ;*/
/*	SET WORKLOCL.ROHEC;*/
/*RUN;*/
PROC SORT DATA=ROHEC NODUPKEY;
	BY DF_SPE_ACC_ID;
RUN;
DATA _NULL_;
	SET WORK.ROHEC;
	FILE REPORT2 DELIMITER=',' DSD DROPOVER LRECL=32767;
	IF _N_ = 1 THEN DO;
	PUT
		"BF_SSN"
		','
		"NAME"
		','
		"DX_ADR_EML"
		;
	END;
	FORMAT BF_SSN $9. ;
	FORMAT NAME $37. ;
	FORMAT DX_ADR_EML $254. ;
	DO;
		PUT BF_SSN $ @;
		PUT NAME $ @;
		PUT DX_ADR_EML $;
	END;
RUN;
