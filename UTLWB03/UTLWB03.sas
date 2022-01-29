*------------------------------------------*
| UTLWB03 TILP CHAPTER 7 BANKRUPTCY REVIEW |
*------------------------------------------*;
/*LIBNAME DLGSUTWH DB2 DATABASE=DLGSUTWH OWNER=OLWHRM1;*/
/*%LET RPTLIB = %SYSGET(reportdir);*/
%LET RPTLIB = T:\SAS;
FILENAME REPORT2 "&RPTLIB/ULWB03.LWB03R2";
FILENAME REPORTZ "&RPTLIB/ULWB03.LWB03RZ";
DATA _NULL_;
     CALL SYMPUT('DAYS_AGO_30',"'"||PUT(INTNX('DAY',TODAY(),-30,'BEGINNING'), MMDDYYD10.)||"'");
RUN;
%SYSLPUT DAYS_AGO_30 = &DAYS_AGO_30;
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
CREATE TABLE TLP07 AS
SELECT *
FROM CONNECTION TO DB2 (
SELECT DISTINCT A.BF_SSN AS TARGET_ID
	,A.LN_SEQ 
	,'TLP07' AS ARC_NAME
	,'' AS FROM_DATE
	,'' AS TO_DATE
	,'' AS NEEDED_BY_DATE
	,'' AS RECIP
	,'' AS REGARDS_CODE
	,'' AS REGARDS_ID
	,'Chapter 07 Bankruptcy Review for TILP loan(s)' AS COMMENTS
FROM OLWHRM1.LN10_LON A
INNER JOIN OLWHRM1.PD24_PRS_BKR B
	ON A.BF_SSN = B.DF_PRS_ID
LEFT OUTER JOIN (
	SELECT BF_SSN
		,'X' AS TLP07_ARC
	FROM OLWHRM1.AY10_BR_LON_ATY
	WHERE PF_REQ_ACT = 'TLP07'
		AND LD_ATY_RSP >= &DAYS_AGO_30
	) C
	ON A.BF_SSN = C.BF_SSN
LEFT OUTER JOIN (
	SELECT BF_SSN
		,'X' AS QUE_TSK
	FROM OLWHRM1.WQ20_TSK_QUE
	WHERE WF_QUE = 'TB'
		AND WF_SUB_QUE = '07'
	) D
	ON A.BF_SSN = D.BF_SSN
WHERE A.LC_STA_LON10 = 'R'
	AND A.LA_CUR_PRI > 0
	AND A.IC_LON_PGM = 'TILP'
	AND B.DC_BKR_STA = '06'
	AND B.DC_BKR_TYP = '07'
	AND NOT EXISTS (
		SELECT *
		FROM OLWHRM1.LN10_LON X
		WHERE X.BF_SSN = A.BF_SSN
		AND X.IC_LON_PGM != 'TILP'
		)
	AND C.TLP07_ARC IS NULL
	AND D.QUE_TSK IS NULL
FOR READ ONLY WITH UR
);
DISCONNECT FROM DB2;
/*%PUT  SQLXRC= >>> &SQLXRC <<< ||| SQLXMSG= >>> &SQLXMSG >>>  ;  * INCLUDES ERROR MESSAGES TO SAS LOG  ;*/
/*%SQLCHECK (SQLRPT=ULWB03.LWB03RZ);*/
/*QUIT;*/
ENDRSUBMIT;

DATA TLP07;
SET WORKLOCL.TLP07;
RUN;

PROC SORT DATA=TLP07 NODUPKEY;
BY TARGET_ID LN_SEQ;
RUN;

DATA _NULL_;
SET  WORK.TLP07;
FILE REPORT2 DELIMITER=',' DSD DROPOVER LRECL=32767;
   FORMAT TARGET_ID $9. ;
   FORMAT ARC_NAME $5. ;
   FORMAT FROM_DATE $1. ;
   FORMAT NEEDED_BY_DATE $1. ;
   FORMAT RECIP $1. ;
   FORMAT REGARDS_CODE $1. ;
   FORMAT REGARDS_ID $1. ;
   FORMAT COMMENTS $45. ;
   FORMAT LN_SEQ 6.;
   FORMAT TO_DATE $1.;
   FORMAT LN_SEQ z3.;
 DO;
   PUT TARGET_ID $ @;
   PUT ARC_NAME $ @;
   PUT FROM_DATE $ @;
   PUT TO_DATE $ @;
   PUT NEEDED_BY_DATE $ @;
   PUT RECIP $ @;
   PUT REGARDS_CODE $ @;
   PUT REGARDS_ID $ @;
   PUT LN_SEQ  @;
   PUT COMMENTS $ ;
  
 END;
RUN;