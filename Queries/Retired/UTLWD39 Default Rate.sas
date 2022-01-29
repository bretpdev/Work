/*LIBNAME DLGSUTWH DB2 DATABASE=DLGSUTWH OWNER=OLWHRM1;*/
/*%LET RPTLIB = %SYSGET(reportdir);*/
%LET RPTLIB = T:\SAS;
FILENAME REPORTZ "&RPTLIB/ULWD39.LWD39RZ";
FILENAME REPORT2 "&RPTLIB/ULWD39.LWD39R2";
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
CREATE TABLE DEF_RT AS
SELECT *
FROM CONNECTION TO DB2 (
SELECT DISTINCT A.BF_SSN
	,A.LN_SEQ
	,A.LF_LON_ALT || '0' || A.LN_LON_ALT_SEQ AS CLUID 
	,CASE
		WHEN C.WC_DW_LON_STA IN ('03','04','05','06','07','08','09'
			,'10','11','13','14','15','16','18','20') THEN 'RP'
		WHEN C.WC_DW_LON_STA IN ('12') AND D.LC_REA_CLM_PCL IN ('06','09')
			THEN 'DF'
	END AS LN_STATUS
	,B.LC_STA_LON15
	,COALESCE(A.LA_CUR_PRI,0) + COALESCE(A.LA_NSI_OTS,0) AS CUR_BAL
	,COALESCE(F.TRANS_1030,0) AS CLM_PD

FROM OLWHRM1.LN10_LON A
INNER JOIN OLWHRM1.DW01_DW_CLC_CLU C
	ON A.BF_SSN = C.BF_SSN
	AND A.LN_SEQ = C.LN_SEQ
LEFT OUTER JOIN OLWHRM1.LN15_DSB B
	ON A.BF_SSN = B.BF_SSN
	AND A.LN_SEQ = B.LN_SEQ
	AND B.LA_DSB = B.LA_DSB_CAN
	AND B.LC_STA_LON15 IN ('1','3') 
LEFT OUTER JOIN OLWHRM1.CL10_CLM_PCL D
	ON C.BF_SSN = D.BF_SSN
	AND C.WC_DW_LON_STA = '12'
	AND D.LC_REA_CLM_PCL IN ('06','09')
LEFT OUTER JOIN (SELECT BF_SSN
					,LN_SEQ
					,SUM(COALESCE(LA_FAT_NSI,0) + COALESCE(LA_FAT_LTE_FEE,0) + COALESCE(LA_FAT_CUR_PRI,0)) AS TRANS_1030
			FROM OLWHRM1.LN90_FIN_ATY 
			WHERE PC_FAT_TYP = '10'
				AND PC_FAT_SUB_TYP = '30'
			GROUP BY BF_SSN, LN_SEQ) F
	ON A.BF_SSN = F.BF_SSN
	AND A.LN_SEQ = F.LN_SEQ
WHERE A.LC_STA_LON10 = 'R'
	AND ((C.WC_DW_LON_STA IN ('03','04','05','06','07','08','09'
			,'10','11','13','14','15','16','18','20') AND A.LA_CUR_PRI > 0)
			OR (C.WC_DW_LON_STA IN ('12') AND D.LC_REA_CLM_PCL IN ('06','09') AND A.LA_CUR_PRI = 0))

FOR READ ONLY WITH UR
);
DISCONNECT FROM DB2;

/*%PUT  SQLXRC= >>> &SQLXRC <<< ||| SQLXMSG= >>> &SQLXMSG >>> ;  ** INCLUDES ERROR MESSAGES TO SAS LOG  ;*/
/*%SQLCHECK;*/
/*QUIT;*/

PROC SORT DATA=DEF_RT;
BY BF_SSN LN_SEQ;
where LC_STA_LON15 = '';
RUN;
 
DATA DEF_SUMMARY(KEEP=DEF_LON LON DEF_DOL TOT_DOL DEF_RT DEF_DOL_RT);
SET DEF_RT END=LAST;
BY BF_SSN LN_SEQ;
RETAIN DEF_LON LON DEF_DOL TOT_DOL A B 0;
FORMAT TOT_DOL DEF_DOL 16.2;
IF FIRST.BF_SSN THEN DO;
	A=0;
	B=0;
END;
IF LN_STATUS = 'DF' THEN DO;
	DEF_DOL = DEF_DOL + CLM_PD*(-1);
	TOT_DOL = TOT_DOL - CLM_PD;
	IF A = 0 THEN DO;
		DEF_LON = DEF_LON + 1;
		A = 1;
	END;
END;
IF LN_STATUS = 'RP' THEN DO;
	TOT_DOL = TOT_DOL + CUR_BAL;
	IF B = 0 THEN DO;
		LON = LON + 1;
		B = 1;
	END;
END;
IF LAST THEN DO;
	DEF_RT = DEF_LON/LON;
	DEF_DOL_RT = DEF_DOL / TOT_DOL ;
	OUTPUT;
END;
RUN;
ENDRSUBMIT;
DATA DEF_SUMMARY;
SET WORKLOCL.DEF_SUMMARY;
RUN;

PROC PRINTTO PRINT=REPORT2 NEW;
RUN;
/*FOR LANDSCAPE REPORTS:*/
OPTIONS ORIENTATION = LANDSCAPE;
OPTIONS PS=15 LS=127;
TITLE 'DEFAULT RATE';

PROC PRINT NOOBS SPLIT='/' DATA=DEF_SUMMARY WIDTH=UNIFORM WIDTH=MIN LABEL;
FORMAT DEF_RT percent10.2;
FORMAT DEF_LON LON COMMA12.0;
VAR DEF_LON LON DEF_RT ;
LABEL DEF_LON = 'DEFAULTED BORROWERS'
	LON = 'BORROWERS'
	DEF_RT = '# DEFAULT RATE';
RUN;
TITLE;
FOOTNOTE   'JOB = UTLWD39  	 REPORT = ULWD39.LWD39R2';
PROC PRINT NOOBS SPLIT='/' DATA=DEF_SUMMARY WIDTH=UNIFORM WIDTH=MIN LABEL;
FORMAT  DEF_DOL_RT percent10.2;
FORMAT TOT_DOL DEF_DOL DOLLAR18.2;
VAR  DEF_DOL TOT_DOL  DEF_DOL_RT;
LABEL DEF_DOL = '$ DEFAULTED'
	TOT_DOL = '$ TOTAL'
	DEF_DOL_RT = '$ DEFAULT RATE';
RUN;

PROC PRINTTO;
RUN;
 
/*PROC EXPORT DATA= WORK.DEF_RT */
/*            OUTFILE= "T:\SAS\d39_detail.xls" */
/*            DBMS=EXCEL REPLACE;*/
/*     RANGE="1"; */
/*RUN;*/