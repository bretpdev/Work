*-------------------------------------------*
| UTLWD35 VIP And Special Handling Accounts |
*-------------------------------------------*;
/*LIBNAME DLGSUTWH DB2 DATABASE=DLGSUTWH OWNER=OLWHRM1;*/
/*%LET RPTLIB = %SYSGET(reportdir);*/
%LET RPTLIB = T:\SAS;
FILENAME REPORT2 "&RPTLIB/ULWD35.LWD35R2";
FILENAME REPORT3 "&RPTLIB/ULWD35.LWD35R3";
FILENAME REPORT4 "&RPTLIB/ULWD35.LWD35R4";
FILENAME REPORTZ "&RPTLIB/ULWD35.LWD35RZ";
DATA _NULL_;
	CALL SYMPUT('RUNDATE',PUT(INTNX('DAY',TODAY(),0,'beginning'), MMDDYY10.));
RUN;
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
CREATE TABLE AVIP AS
SELECT *
FROM CONNECTION TO DB2 (
SELECT DISTINCT B.DF_SPE_ACC_ID
	,A.PF_ACT
	,A.BX_CMT
FROM OLWHRM1.AY01_BR_ATY A
INNER JOIN OLWHRM1.PD01_PDM_INF B
	ON A.DF_PRS_ID = B.DF_PRS_ID
LEFT OUTER JOIN OLWHRM1.DC01_LON_CLM_INF C
	ON A.DF_PRS_ID = C.BF_SSN
LEFT OUTER JOIN OLWHRM1.LN10_LON D
	ON A.DF_PRS_ID = D.BF_SSN
WHERE A.PF_ACT IN ('VIPSS','SPHAN','RESPH')
	AND
	(
		(
			C.LC_STA_DC10 = '03'
			AND C.LD_CLM_ASN_DOE IS NULL
		)
	OR 
		(
			D.LA_CUR_PRI > 0
			AND D.LC_STA_LON10 = 'R'
		)
	)
FOR READ ONLY WITH UR
);
DISCONNECT FROM DB2;
/*%PUT  SQLXRC= >>> &SQLXRC <<< ||| SQLXMSG= >>> &SQLXMSG >>>  ;  * INCLUDES ERROR MESSAGES TO SAS LOG  ;*/
/*%SQLCHECK (SQLRPT=ULWD35.LWD35RZ);*/
/*QUIT;*/
ENDRSUBMIT;
DATA AVIP ;
	SET WORKLOCL.AVIP;
RUN;
PROC SQL;
	CREATE TABLE SUMDAT AS 
		SELECT DISTINCT 'TOTAL SPECIAL HANDLING' AS PF_ACT 
			,COUNT(DISTINCT A.DF_SPE_ACC_ID) AS BOR_NUM
			,1 AS OV
			,'1' AS DV
		FROM AVIP A
		LEFT OUTER JOIN AVIP B
			ON A.DF_SPE_ACC_ID = B.DF_SPE_ACC_ID
			AND B.PF_ACT = 'RESPH'
		WHERE A.PF_ACT = 'SPHAN'
			AND B.PF_ACT IS NULL	
		GROUP BY A.PF_ACT
	UNION 
		SELECT DISTINCT 'TOTAL VIP' AS PF_ACT
			,COUNT(DISTINCT DF_SPE_ACC_ID) AS BOR_NUM
			,2 AS OV
			,'1' AS DV
		FROM AVIP
		WHERE PF_ACT = 'VIPSS'		
	UNION 
		SELECT COALESCE(X.PF_ACT,'TOTAL RESOLVED') AS PF_ACT
			,X.BOR_NUM
			,COALESCE(X.OV,3) AS OV
			,COALESCE(X.DV,'1') AS DV
		FROM (
			SELECT DISTINCT 'TOTAL RESOLVED' AS  PF_ACT 
				,COALESCE(COUNT(DISTINCT A.DF_SPE_ACC_ID),0) AS BOR_NUM
				,3 AS OV
				,'1' AS DV
			FROM AVIP A
			INNER JOIN AVIP B
				ON A.DF_SPE_ACC_ID = B.DF_SPE_ACC_ID
			WHERE A.PF_ACT = 'SPHAN'
				AND B.PF_ACT = 'RESPH'
			) X
;
QUIT;
PROC SORT DATA=AVIP OUT=D35R2(WHERE=(PF_ACT = 'VIPSS'));
	BY PF_ACT DF_SPE_ACC_ID;
RUN;
PROC PRINTTO PRINT=REPORT2 NEW;
RUN;
OPTIONS ORIENTATION = LANDSCAPE;
OPTIONS PS=39 LS=127 CENTER NODATE PAGENO=1;
TITLE "VIP ACCOUNTS";
TITLE2 "&RUNDATE";
FOOTNOTE   "JOB = UTLWD35  	 REPORT = ULWD35.LWD35R2";
PROC CONTENTS DATA=D35R2 OUT=EMPTYSET NOPRINT;
DATA _NULL_;
SET EMPTYSET;
FILE PRINT;
IF  NOBS=0 AND _N_ =1 THEN DO;
	PUT // 126*'-';
	PUT      //////
		@51 '**** NO OBSERVATIONS FOUND ****';
	PUT //////
		@57 '-- END OF REPORT --';
	PUT //////////////
		@46 "JOB = UTLWD35  	 REPORT = ULWD35.LWD35R2";
	END;
RETURN;
RUN;
PROC PRINT NOOBS SPLIT='~' DATA=D35R2 WIDTH=UNIFORM WIDTH=MIN LABEL;
	VAR DF_SPE_ACC_ID PF_ACT;
	LABEL DF_SPE_ACC_ID = 'ACCOUNT #'
		PF_ACT = 'ACTION CODE/ARC';
RUN;
PROC PRINTTO;
RUN;
PROC SORT DATA=AVIP OUT=D35R3(WHERE=(PF_ACT = 'SPHAN'));
	BY PF_ACT DF_SPE_ACC_ID;
RUN;
PROC PRINTTO PRINT=REPORT3 NEW;
RUN;
OPTIONS ORIENTATION = LANDSCAPE;
OPTIONS PS=39 LS=127 CENTER NODATE PAGENO=1;
TITLE "SPECIAL HANDLING ACCOUNTS";
TITLE2 "&RUNDATE";
FOOTNOTE   "JOB = UTLWD35  	 REPORT = ULWD35.LWD35R3";
PROC CONTENTS DATA=D35R3 OUT=EMPTYSET NOPRINT;
DATA _NULL_;
SET EMPTYSET;
FILE PRINT;
IF  NOBS=0 AND _N_ =1 THEN DO;
	PUT // 126*'-';
	PUT      //////
		@51 '**** NO OBSERVATIONS FOUND ****';
	PUT //////
		@57 '-- END OF REPORT --';
	PUT //////////////
		@46 "JOB = UTLWD35  	 REPORT = ULWD35.LWD35R3";
	END;
RETURN;
RUN;
PROC REPORT DATA=D35R3 NOWD HEADSKIP SPLIT='/';
	COLUMN DF_SPE_ACC_ID PF_ACT BX_CMT;
	DEFINE DF_SPE_ACC_ID / GROUP 'ACCOUNT #' WIDTH=10;
	DEFINE PF_ACT / GROUP 'ACTION CODE/ARC' WIDTH=11;
	DEFINE BX_CMT / GROUP 'COMMENT' WIDTH=100 FLOW;
RUN;
PROC PRINTTO;
RUN;
PROC SORT DATA=SUMDAT;
	BY OV;
RUN;
PROC PRINTTO PRINT=REPORT4 NEW;
RUN;
OPTIONS ORIENTATION = LANDSCAPE;
OPTIONS PS=39 LS=127 CENTER NODATE PAGENO=1;
TITLE "SUMMARY REPORT";
TITLE2 "&RUNDATE";
FOOTNOTE   "JOB = UTLWD35  	 REPORT = ULWD35.LWD35R4";
PROC CONTENTS DATA=SUMDAT OUT=EMPTYSET NOPRINT;
DATA _NULL_;
SET EMPTYSET;
FILE PRINT;
IF  NOBS=0 AND _N_ =1 THEN DO;
	PUT // 126*'-';
	PUT      //////
		@51 '**** NO OBSERVATIONS FOUND ****';
	PUT //////
		@57 '-- END OF REPORT --';
	PUT //////////////
		@46 "JOB = UTLWD35  	 REPORT = ULWD35.LWD35R4";
	END;
RETURN;
RUN;
PROC REPORT DATA=SUMDAT NOWD HEADSKIP SPLIT='/';
	COLUMN DV OV PF_ACT BOR_NUM ;
	DEFINE DV / ORDER NOPRINT;
	DEFINE OV / ORDER NOPRINT;
	DEFINE PF_ACT / ORDER '/' WIDTH=25;
	DEFINE BOR_NUM / ANALYSIS '/' FORMAT=COMMA6.;
	BREAK AFTER DV / SKIP OL SUMMARIZE SUPPRESS;
	
RUN;
PROC PRINTTO;
RUN;

