*------------------------------------------------------------*
| UTLWO89 PROCESSING TIME FOR DEFERMENTS AND OR FORBEARANCES |
*------------------------------------------------------------*;

*___________________________________________________________
| 				THIS JOB HAS BEEN RETIRED					|
*___________________________________________________________|

/*LIBNAME DLGSUTWH DB2 DATABASE=DLGSUTWH OWNER=OLWHRM1;*/
/*%LET RPTLIB = %SYSGET(reportdir);*/
%LET RPTLIB = T:\SAS;
FILENAME REPORT2 "&RPTLIB/ULWO89.LWO89R2";
FILENAME REPORT3 "&RPTLIB/ULWO89.LWO89R3";
FILENAME REPORT4 "&RPTLIB/ULWO89.LWO89R4";
FILENAME REPORT5 "&RPTLIB/ULWO89.LWO89R5";
FILENAME REPORT6 "&RPTLIB/ULWO89.LWO89R6";
FILENAME REPORT7 "&RPTLIB/ULWO89.LWO89R7";
FILENAME REPORT8 "&RPTLIB/ULWO89.LWO89R8";
FILENAME REPORT9 "&RPTLIB/ULWO89.LWO89R9";
FILENAME REPORT10 "&RPTLIB/ULWO89.LWO89R10";
FILENAME REPORT11 "&RPTLIB/ULWO89.LWO89R11";
FILENAME REPORT12 "&RPTLIB/ULWO89.LWO89R12";
FILENAME REPORT13 "&RPTLIB/ULWO89.LWO89R13";
FILENAME REPORT14 "&RPTLIB/ULWO89.LWO89R14";
FILENAME REPORT15 "&RPTLIB/ULWO89.LWO89R15";
FILENAME REPORT16 "&RPTLIB/ULWO89.LWO89R16";
FILENAME REPORT17 "&RPTLIB/ULWO89.LWO89R17";
FILENAME REPORT18 "&RPTLIB/ULWO89.LWO89R18";
FILENAME REPORT19 "&RPTLIB/ULWO89.LWO89R19";
FILENAME REPORT20 "&RPTLIB/ULWO89.LWO89R20";
FILENAME REPORT21 "&RPTLIB/ULWO89.LWO89R21";
FILENAME REPORTZ "&RPTLIB/ULWO89.LWO89RZ";
DATA _NULL_;
	CALL SYMPUT('MONTHS_AGO_3',"'"||PUT(INTNX('MONTH',TODAY(),-3,'BEGINNING'), MMDDYYD10.)||"'");
	CALL SYMPUT('MON_BEG',"'"||PUT(INTNX('MONTH',TODAY(),-1,'BEGINNING'), MMDDYYD10.)||"'");
	CALL SYMPUT('MON_END',"'"||PUT(INTNX('MONTH',TODAY(),-1,'END'), MMDDYYD10.)||"'");
	CALL SYMPUT('RUNDATE',PUT(INTNX('DAY',TODAY(),0,'beginning'), MMDDYY10.));
RUN;
%SYSLPUT MONTHS_AGO_3 = &MONTHS_AGO_3;
%SYSLPUT MON_BEG = &MON_BEG;
%SYSLPUT MON_END = &MON_END;
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
CREATE TABLE CODA AS
SELECT *
FROM CONNECTION TO DB2 (
SELECT DISTINCT A.BF_SSN
	,A.LF_LON_CUR_OWN
	,D.DM_PRS_MID
	,D.DM_PRS_1
	,D.DM_PRS_LST
	,C.LN_ATY_SEQ
	,C.PF_REQ_ACT
	,C.LD_ATY_REQ_RCV
FROM OLWHRM1.LN10_LON A
INNER JOIN OLWHRM1.LN85_LON_ATY B
	ON A.BF_SSN = B.BF_SSN
	AND A.LN_SEQ = B.LN_SEQ
INNER JOIN OLWHRM1.AY10_BR_LON_ATY C
	ON B.BF_SSN = C.BF_SSN
	AND B.LN_ATY_SEQ = C.LN_ATY_SEQ
INNER JOIN OLWHRM1.PD10_PRS_NME D
	ON A.BF_SSN = D.DF_PRS_ID
LEFT OUTER JOIN OLWHRM1.LN50_BR_DFR_APV LN50
	ON A.BF_SSN = LN50.BF_SSN
	AND A.LN_SEQ = LN50.LN_SEQ
LEFT OUTER JOIN OLWHRM1.DF10_BR_DFR_REQ DF10
    ON LN50.BF_SSN = DF10.BF_SSN
    AND LN50.LF_DFR_CTL_NUM = DF10.LF_DFR_CTL_NUM
LEFT OUTER JOIN OLWHRM1.LN60_BR_FOR_APV LN60
	ON A.BF_SSN = LN60.BF_SSN
	AND A.LN_SEQ = LN60.LN_SEQ
LEFT OUTER JOIN OLWHRM1.FB10_BR_FOR_REQ FB10
    ON FB10.BF_SSN = LN60.BF_SSN
    AND FB10.LF_FOR_CTL_NUM = LN60.LF_FOR_CTL_NUM
WHERE A.LA_CUR_PRI > 0
	AND A.LC_STA_LON10 = 'R'
	AND A.IC_LON_PGM != 'TILP'
	AND A.LI_CON_PAY_STP_PUR != 'Y'
	AND C.LD_ATY_REQ_RCV BETWEEN &MON_BEG AND &MON_END
	AND C.LC_STA_ACTY10 = 'A'
	AND 
	(
		(	
			C.PF_REQ_ACT = 'PRDEF' AND
			DF10.LC_STA_DFR10 = 'A' AND 
			DF10.LC_DFR_STA = 'A' AND 
			LN50.LC_STA_LON50 = 'A' 
		)
	OR 
		(
			C.PF_REQ_ACT = 'PRFOR' AND
			FB10.LC_STA_FOR10 = 'A' AND 
			FB10.LC_FOR_STA = 'A' AND 
			LN60.LC_STA_LON60 = 'A'
		)
	)
FOR READ ONLY WITH UR
);
CREATE TABLE OLDA AS
SELECT *
FROM CONNECTION TO DB2 (
SELECT A.DF_PRS_ID 
	,A.BF_CRT_DTS_AY01 AS AY01_ID
	,A.BD_ATY_PRF
	,CASE 
		WHEN SUBSTR(A.BX_CMT,1,1) = 'D' THEN SUBSTR(A.BX_CMT,1,9)
		ELSE SUBSTR(A.BX_CMT,1,12)
	 END AS OLTYP
FROM OLWHRM1.AY01_BR_ATY A
INNER JOIN OLWHRM1.PD01_PDM_INF B
	ON A.DF_PRS_ID = B.DF_PRS_ID
WHERE A.BD_ATY_PRF BETWEEN &MONTHS_AGO_3 AND &MON_END
	AND A.PF_ACT = 'MDCID'
FOR READ ONLY WITH UR
);

CREATE TABLE LDRS AS
SELECT *
FROM CONNECTION TO DB2 (
SELECT IF_DOE_LDR
	,IM_LDR_FUL
FROM OLWHRM1.LR10_LDR_DMO
FOR READ ONLY WITH UR
);
DISCONNECT FROM DB2;
/*%PUT  SQLXRC= >>> &SQLXRC <<< ||| SQLXMSG= >>> &SQLXMSG >>>  ;  * INCLUDES ERROR MESSAGES TO SAS LOG  ;*/
/*%SQLCHECK (SQLRPT=ULWO89.LWO89RZ);*/
/*QUIT;*/
PROC SORT DATA=OLDA OUT=OLDA (WHERE=(SUBSTR(OLTYP,1,12) = 'FORBEARANCE' OR
	SUBSTR(OLTYP,1,9) = 'DEFERMENT')) NODUPKEY;
	BY DF_PRS_ID;
RUN;
PROC SQL;
	PROC SQL;
	CREATE TABLE ALDA (DROP=DF_PRS_ID) AS 
			SELECT DISTINCT A.*
				,B.*
			FROM CODA A
			INNER JOIN OLDA B
				ON A.BF_SSN = B.DF_PRS_ID  
			WHERE A.PF_REQ_ACT = 'PRDEF'
				AND B.OLTYP = 'DEFERMENT'
				AND 
				(
					MONTH(B.BD_ATY_PRF) = MONTH(A.LD_ATY_REQ_RCV) OR
					MONTH(B.BD_ATY_PRF) = MONTH(A.LD_ATY_REQ_RCV) - 1 OR
					MONTH(B.BD_ATY_PRF) = 12 AND MONTH(A.LD_ATY_REQ_RCV) = 1 
				)
		UNION
			SELECT DISTINCT A.*
					,B.*
				FROM CODA A
				INNER JOIN OLDA B
					ON A.BF_SSN = B.DF_PRS_ID  
				WHERE A.PF_REQ_ACT = 'PRFOR'
					AND B.OLTYP = 'FORBEARANCE'
					AND 
					(
						MONTH(B.BD_ATY_PRF) = MONTH(A.LD_ATY_REQ_RCV) OR
						MONTH(B.BD_ATY_PRF) = MONTH(A.LD_ATY_REQ_RCV) - 1 OR
						MONTH(B.BD_ATY_PRF) = 12 AND MONTH(A.LD_ATY_REQ_RCV) = 1 
					)
	;
QUIT;
ENDRSUBMIT;
DATA ALDA;
	SET WORKLOCL.ALDA;
RUN;
DATA LDRS;
	SET WORKLOCL.LDRS;
RUN;
DATA ALDA REDA(KEEP=BF_SSN DM_PRS_MID DM_PRS_1 DM_PRS_LST LF_LON_CUR_OWN MASKED_SSN);
	LENGTH MASKED_SSN $ 11.;
	SET ALDA;
	MASKED_SSN = CATX('-','XXX','XX',SUBSTR(BF_SSN,6,4));
RUN;
PROC SORT DATA=REDA NODUPKEY;
	BY BF_SSN LF_LON_CUR_OWN;
RUN;
DATA ALDA(DROP=DM_PRS_MID DM_PRS_1 DM_PRS_LST LF_LON_CUR_OWN) ERR;
	SET ALDA;
	IF BD_ATY_PRF <= LD_ATY_REQ_RCV <= BD_ATY_PRF+10 THEN DO;
		IF OLTYP = 'FORBEARANCE' AND PF_REQ_ACT = 'PRFOR' THEN
			OUTPUT ALDA;
		ELSE IF OLTYP = 'DEFERMENT' AND PF_REQ_ACT = 'PRDEF' THEN
			OUTPUT ALDA;
	END;
	ELSE DO;
		OUTPUT ERR;
	END;
RUN;
/*ENSURE DATA RELATIONSHIPS*/
PROC SORT DATA=ALDA NODUPKEY;
	BY BF_SSN AY01_ID LN_ATY_SEQ;
RUN;
PROC SORT DATA=ERR NODUPKEY;
	BY LF_LON_CUR_OWN BF_SSN BD_ATY_PRF;
RUN;
DATA ALDA;
	SET ALDA;
	PROC_DAYS = ABS(BD_ATY_PRF - LD_ATY_REQ_RCV);
RUN;
DATA ALDA (DROP=I);
	SET ALDA;
	DEC_DAYS=0;
	DO I=BD_ATY_PRF TO LD_ATY_REQ_RCV;
/*		ACCOUNT FOR SATURDAYS AND SUNDAYS */
		IF WEEKDAY(I) IN (1,7) THEN 	
			DEC_DAYS = DEC_DAYS + 1;
/*		ACCOUNT FOR HOLIDAYS*/
		IF 
		(
			I = HOLIDAY('NEWYEAR',YEAR(LD_ATY_REQ_RCV)) OR /*THE ONLY HOLIDAY THAT USES A COMPASS DATE*/
			I = HOLIDAY('USINDEPENDENCE',YEAR(BD_ATY_PRF)) OR
			I = HOLIDAY('MLK',YEAR(BD_ATY_PRF)) OR
			I = HOLIDAY('USPRESIDENTS',YEAR(BD_ATY_PRF)) OR
			I = HOLIDAY('MEMORIAL',YEAR(BD_ATY_PRF)) OR
			I = HOLIDAY('LABOR',YEAR(BD_ATY_PRF)) OR
			I = HOLIDAY('THANKSGIVING',YEAR(BD_ATY_PRF)) OR
			I = HOLIDAY('THANKSGIVING',YEAR(BD_ATY_PRF))+1 OR /*DAY AFTER THANKSGIVING*/
			I = MDY(7,24,YEAR(BD_ATY_PRF)) 	/*JULY 24TH*/
		) 
			THEN
				DEC_DAYS = DEC_DAYS + 1;
	END;
	NET_PROC_DAYS = PROC_DAYS - DEC_DAYS + 1;
RUN;
/*REASSEMBLE DATA FOR FURTHER REPORT PROCESSING*/
PROC SQL;
	CREATE TABLE ALDA_REP AS
		SELECT DISTINCT A.BF_SSN
			,B.MASKED_SSN
			,A.DM_PRS_1 
			,A.DM_PRS_MID
			,A.DM_PRS_LST 
			,A.LF_LON_CUR_OWN
			,B.OLTYP
			,B.BD_ATY_PRF
			,B.LD_ATY_REQ_RCV
			,B.NET_PROC_DAYS
			,C.OL_COUNT
			,C.CO_COUNT
		FROM REDA A
		INNER JOIN ALDA B
			ON A.BF_SSN = B.BF_SSN
		INNER JOIN (
			SELECT BF_SSN
				,COUNT(DISTINCT BD_ATY_PRF) AS OL_COUNT
				,COUNT(DISTINCT LD_ATY_REQ_RCV) AS CO_COUNT
			FROM ALDA A
			GROUP BY BF_SSN
			) C
			ON B.BF_SSN = C.BF_SSN
	;
QUIT;
/*CREATE R4 DATA SET AND MASTER DATA SET FOR LENDER REPORTS*/
DATA ALDA_REP ERR2;
	SET ALDA_REP;
	IF OL_COUNT ^= CO_COUNT THEN
		OUTPUT ERR2;
	ELSE 
		OUTPUT ALDA_REP;
RUN;
PROC SORT DATA=ERR2;
	BY LF_LON_CUR_OWN BF_SSN BD_ATY_PRF; 
RUN;
/*R2 DATA SET*/
PROC SQL;
	CREATE TABLE DF_SUMRY AS
		SELECT OLTYP
			,COUNT(*) AS TYP_TOT
			,SUM(NET_PROC_DAYS) AS NUMER
			,ROUND(SUM(NET_PROC_DAYS) / COUNT(*),.01) AS AVE_PROC 
		FROM  ALDA_REP
		GROUP BY OLTYP
		;
QUIT;
/*CREATE REPORTS*/
PROC PRINTTO PRINT=REPORT2 NEW;
RUN;
OPTIONS ORIENTATION = LANDSCAPE;
OPTIONS PS=39 LS=127 NODATE CENTER PAGENO=1 ;
TITLE 'PROCESSING TIME FOR DEFERMENTS AND/OR FORBEARANCES - TOTALS ';
TITLE2 "&RUNDATE";
FOOTNOTE   'JOB = UTLWO89  	 REPORT = ULWO89.LWO89R2';
PROC PRINT NOOBS SPLIT='/' DATA=DF_SUMRY WIDTH=UNIFORM WIDTH=MIN LABEL;
FORMAT TYP_TOT COMMA6. AVE_PROC 10.2; 
VAR OLTYP TYP_TOT AVE_PROC;
LABEL OLTYP = '/' 
	TYP_TOT = 'TOTAL/NUMBER'
	AVE_PROC = 'AVERGE/PROCESSING/TIME'
	;
RUN;
PROC PRINTTO;
RUN;

PROC PRINTTO PRINT=REPORT3 NEW;
RUN;
OPTIONS ORIENTATION = LANDSCAPE;
OPTIONS PS=39 LS=127 NODATE CENTER PAGENO=1 ;
TITLE 'DOCUMENT RECEIVED BUT NOT PROCESSED ERROR REPORT';
TITLE2 "&RUNDATE";
FOOTNOTE   'JOB = UTLWO89  	 REPORT = ULWO89.LWO89R3';
PROC CONTENTS DATA=ERR OUT=EMPTYSET NOPRINT;
DATA _NULL_;
SET EMPTYSET;
FILE PRINT;
IF  NOBS=0 AND _N_ =1 THEN DO;
	PUT // 126*'-';
	PUT      ////
		@51 '**** NO OBSERVATIONS FOUND ****';
	PUT ////
		@57 '-- END OF REPORT --';
	PUT ////////////
		@46 "JOB = UTLWO89  	 REPORT = ULWO89.LWO89R3";
	END;
RETURN;
RUN;
PROC PRINT NOOBS SPLIT='~' DATA=ERR WIDTH=UNIFORM WIDTH=MIN LABEL;
FORMAT BD_ATY_PRF MMDDYY10.;
VAR LF_LON_CUR_OWN
	MASKED_SSN
	DM_PRS_1 
	DM_PRS_MID
	DM_PRS_LST
	BD_ATY_PRF;
LABEL LF_LON_CUR_OWN = 'LENDER'
	MASKED_SSN = 'SSN'
	DM_PRS_1 = 'FIRST~NAME' 
	DM_PRS_MID = 'MIDDLE'
	DM_PRS_LST = 'LAST~NAME'
	BD_ATY_PRF = 'DATE~DEF/FORB~DOC RECEIVED';
RUN;
PROC PRINTTO;
RUN;

PROC PRINTTO PRINT=REPORT4 NEW;
RUN;
OPTIONS ORIENTATION = LANDSCAPE;
OPTIONS PS=39 LS=127 NODATE CENTER PAGENO=1 ;
TITLE 'MULTIPLE DOCUMENTS RECEIVED ERROR REPORT ';
TITLE2 "&RUNDATE";
FOOTNOTE   'JOB = UTLWO89  	 REPORT = ULWO89.LWO89R4';
PROC CONTENTS DATA=ERR2 OUT=EMPTYSET NOPRINT;
DATA _NULL_;
SET EMPTYSET;
FILE PRINT;
IF  NOBS=0 AND _N_ =1 THEN DO;
	PUT // 126*'-';
	PUT      ////
		@51 '**** NO OBSERVATIONS FOUND ****';
	PUT ////
		@57 '-- END OF REPORT --';
	PUT ////////////
		@46 "JOB = UTLWO89  	 REPORT = ULWO89.LWO89R4";
	END;
RETURN;
RUN;
PROC PRINT NOOBS SPLIT='~' DATA=ERR2 WIDTH=UNIFORM WIDTH=MIN LABEL;
FORMAT BD_ATY_PRF MMDDYY10.;
VAR LF_LON_CUR_OWN
	MASKED_SSN
	DM_PRS_1 
	DM_PRS_MID
	DM_PRS_LST
	BD_ATY_PRF;
LABEL LF_LON_CUR_OWN = 'LENDER'
	MASKED_SSN = 'SSN'
	DM_PRS_1 = 'FIRST~NAME' 
	DM_PRS_MID = 'MIDDLE'
	DM_PRS_LST = 'LAST~NAME'
	BD_ATY_PRF = 'DATE~DEF/FORB~DOC RECEIVED';
RUN;
PROC PRINTTO;
RUN;
%MACRO UTLWO89_LENDER_REPORTS(RNO,LDR_ID);
DATA _NULL_;
	SET LDRS;
	IF IF_DOE_LDR = "&LDR_ID" ;
	CALL SYMPUT('LDR_NAME',TRIM(IM_LDR_FUL));
RUN;
PROC SQL;
	CREATE TABLE REPDS AS
		SELECT A.LF_LON_CUR_OWN
			,A.MASKED_SSN
			,A.DM_PRS_1 
			,A.DM_PRS_MID
			,A.DM_PRS_LST 
			,A.OLTYP
			,A.BD_ATY_PRF
			,A.LD_ATY_REQ_RCV
			,A.NET_PROC_DAYS
		FROM ALDA_REP A
		WHERE A.LF_LON_CUR_OWN = "&LDR_ID" 
		ORDER BY A.BD_ATY_PRF 
	;
QUIT;
PROC SQL; 
	CREATE TABLE LDR_DF_SUMRY AS
		SELECT OLTYP
			,COUNT(*) AS TYP_TOT
			,SUM(NET_PROC_DAYS) AS NUMER
			,ROUND(SUM(NET_PROC_DAYS) / COUNT(*),.01) AS AVE_PROC 
		FROM REPDS
		GROUP BY OLTYP
		;
QUIT;
PROC PRINTTO PRINT=REPORT&RNO NEW;
RUN;
OPTIONS ORIENTATION = LANDSCAPE;
OPTIONS PS=39 LS=127 NODATE CENTER PAGENO=1 ;
TITLE "PROCESSING TIME FOR DEFERMENTS AND/OR FORBEARANCES &LDR_NAME ";
TITLE2 "&RUNDATE";
FOOTNOTE   "JOB = UTLWO89  	 REPORT = ULWO89.LWO89R&RNO";
PROC CONTENTS DATA=REPDS OUT=EMPTYSET NOPRINT;
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
		@46 "JOB = UTLWO89  	 REPORT = ULWO89.LWO89R2";
	END;
RETURN;
RUN;
PROC PRINT NOOBS SPLIT='~' DATA=REPDS WIDTH=UNIFORM WIDTH=MIN LABEL;
FORMAT BD_ATY_PRF LD_ATY_REQ_RCV MMDDYY10.;
VAR LF_LON_CUR_OWN
	MASKED_SSN
	DM_PRS_1 
	DM_PRS_MID
	DM_PRS_LST
	BD_ATY_PRF
	LD_ATY_REQ_RCV
	NET_PROC_DAYS;
LABEL LF_LON_CUR_OWN = 'LENDER'
	MASKED_SSN = 'SSN'
	DM_PRS_1 = 'FIRST~NAME' 
	DM_PRS_MID = 'MIDDLE'
	DM_PRS_LST = 'LAST~NAME'
	BD_ATY_PRF = 'DATE~DEF/FORB~DOC RECEIVED'
	LD_ATY_REQ_RCV = 'DATE~DEF/FORB~DOC PROCESSED'
	NET_PROC_DAYS = 'PROCESSING TIME';
RUN;
TITLE "TOTALS FOR &LDR_NAME ";
PROC PRINT NOOBS SPLIT='~' DATA=LDR_DF_SUMRY WIDTH=UNIFORM WIDTH=MIN LABEL;
FORMAT AVE_PROC 10.2;
VAR OLTYP AVE_PROC;
LABEL OLTYP = '~'
	AVE_PROC = 'AVERAGE~PROCESSING~TIME';
RUN;
PROC PRINTTO;
RUN;
%MEND UTLWO89_LENDER_REPORTS;
%UTLWO89_LENDER_REPORTS(5,811698);
%UTLWO89_LENDER_REPORTS(6,817575);
%UTLWO89_LENDER_REPORTS(7,833577);
%UTLWO89_LENDER_REPORTS(8,828476);
%UTLWO89_LENDER_REPORTS(9,833828);
%UTLWO89_LENDER_REPORTS(10,817440);
%UTLWO89_LENDER_REPORTS(11,817545);
%UTLWO89_LENDER_REPORTS(12,830791);
%UTLWO89_LENDER_REPORTS(13,817546);
%UTLWO89_LENDER_REPORTS(14,820200);
%UTLWO89_LENDER_REPORTS(15,830132);
%UTLWO89_LENDER_REPORTS(16,829123);
%UTLWO89_LENDER_REPORTS(17,830146);
%UTLWO89_LENDER_REPORTS(18,829158);
%UTLWO89_LENDER_REPORTS(19,834396);
%UTLWO89_LENDER_REPORTS(20,834437);
%UTLWO89_LENDER_REPORTS(21,82847601);
