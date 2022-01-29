*--------------------------------------------------------*
| UTLWO86 - Identify Borrowers to Redisclose Information |
*--------------------------------------------------------*;
/*LIBNAME DLGSUTWH DB2 DATABASE=DLGSUTWH OWNER=OLWHRM1;*/
/*%LET RPTLIB = %SYSGET(reportdir);*/
%LET RPTLIB = T:\SAS;
FILENAME REPORT2 "&RPTLIB/ULWO86.LWO86R2";
FILENAME REPORTZ "&RPTLIB/ULWO86.LWO86RZ";
DATA _NULL_;
	CALL SYMPUT('DAYS_AGO_0',"'"||PUT(INTNX('DAY',TODAY(),0,'BEGINNING'), MMDDYYD10.)||"'");
	CALL SYMPUT('RUNDATE',PUT(INTNX('DAY',TODAY(),0,'beginning'), MMDDYY10.));
RUN;
%SYSLPUT DAYS_AGO_0 = &DAYS_AGO_0;
LIBNAME  WORKLOCL  REMOTE  SERVER=CYPRUS  SLIBREF=WORK;
RSUBMIT;
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
CREATE TABLE IBTRI AS
SELECT *
FROM CONNECTION TO DB2 (
SELECT PD10.DF_SPE_ACC_ID
FROM OLWHRM1.LN10_LON LN10
INNER JOIN OLWHRM1.DW01_DW_CLC_CLU DW01
	ON LN10.BF_SSN = DW01.BF_SSN
	AND LN10.LN_SEQ = DW01.LN_SEQ
INNER JOIN OLWHRM1.PD10_PRS_NME PD10
	ON LN10.BF_SSN = PD10.DF_PRS_ID
INNER JOIN OLWHRM1.LN65_LON_RPS LN65
	ON LN10.BF_SSN = LN65.BF_SSN
	AND LN65.LN_SEQ = LN65.LN_SEQ
INNER JOIN OLWHRM1.RS10_BR_RPD RS10
	ON LN65.BF_SSN = RS10.BF_SSN
	AND LN65.LN_RPS_SEQ = RS10.LN_RPS_SEQ
LEFT OUTER JOIN (
	SELECT DF10.BF_SSN
		,LN50.LN_SEQ
		,DF10.LF_DFR_CTL_NUM
		,DF10.LC_DFR_TYP
		,LN50.LD_DFR_BEG
		,LN50.LD_DFR_END
		,DF10.LC_STA_DFR10
		,DF10.LC_DFR_STA
		,LN50.LC_STA_LON50
	FROM OLWHRM1.DF10_BR_DFR_REQ DF10
	INNER JOIN OLWHRM1.LN50_BR_DFR_APV LN50
		ON DF10.BF_SSN = LN50.BF_SSN
		AND DF10.LF_DFR_CTL_NUM = LN50.LF_DFR_CTL_NUM
	WHERE DF10.LC_STA_DFR10 = 'A'  
		AND DF10.LC_DFR_STA = 'A'
		AND LN50.LC_STA_LON50 = 'A'
		AND LN50.LC_DFR_RSP != '003'
		AND LN50.LD_DFR_BEG <= &DAYS_AGO_0
		AND LN50.LD_DFR_END > &DAYS_AGO_0
	) DEFR
	ON LN10.BF_SSN = DEFR.BF_SSN
	AND LN10.LN_SEQ = DEFR.LN_SEQ 
LEFT OUTER JOIN (
	SELECT FB10.BF_SSN
		,LN60.LN_SEQ
		,FB10.LC_FOR_TYP
		,LN60.LD_FOR_BEG
		,LN60.LD_FOR_END
		,FB10.LC_STA_FOR10 
		,FB10.LC_FOR_STA
		,LN60.LC_STA_LON60 
	FROM OLWHRM1.FB10_BR_FOR_REQ FB10
	INNER JOIN OLWHRM1.LN60_BR_FOR_APV LN60
		ON FB10.BF_SSN = LN60.BF_SSN
		AND FB10.LF_FOR_CTL_NUM = LN60.LF_FOR_CTL_NUM 
	WHERE FB10.LC_STA_FOR10 = 'A'
		AND FB10.LC_FOR_STA = 'A'
		AND FB10.LC_FOR_TYP NOT IN ('28','10')
		AND LN60.LC_STA_LON60 = 'A'
		AND LN60.LC_FOR_RSP != '003'
		AND LN60.LD_FOR_BEG <= &DAYS_AGO_0
		AND LN60.LD_FOR_END > &DAYS_AGO_0
	) FORB
	ON LN10.BF_SSN = FORB.BF_SSN
	AND LN10.LN_SEQ = FORB.LN_SEQ 
WHERE RS10.LC_STA_RPST10 = 'A'
	AND LN10.LC_STA_LON10 = 'R'
	AND LN10.LA_CUR_PRI > 0 
	AND 
	(
		DEFR.BF_SSN IS NOT NULL 
	OR	
		FORB.BF_SSN IS NOT NULL
	)
FOR READ ONLY WITH UR
);
DISCONNECT FROM DB2;
/*%PUT  SQLXRC= >>> &SQLXRC <<< ||| SQLXMSG= >>> &SQLXMSG >>>  ;  * INCLUDES ERROR MESSAGES TO SAS LOG  ;*/
/*%SQLCHECK (SQLRPT=ULWO86.LWO86RZ);*/
/*QUIT;*/
PROC SORT DATA=IBTRI NODUPKEY;
	BY DF_SPE_ACC_ID;
RUN;
ENDRSUBMIT;
DATA IBTRI ;
	SET WORKLOCL.IBTRI;
RUN;
DATA _NULL_;
SET IBTRI;
FILE REPORT2 DELIMITER=',' DSD DROPOVER LRECL=32767;
DO;
   PUT DF_SPE_ACC_ID $ ;
END;
RUN;

