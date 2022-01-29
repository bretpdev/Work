*----------------------------------------------------------*
| UTLWG98 Year to Date Guaranty Volume by School by Lender |
*----------------------------------------------------------*;
/*LIBNAME DLGSUTWH DB2 DATABASE=DLGSUTWH OWNER=OLWHRM1;*/
/*%LET RPTLIB = %SYSGET(reportdir);*/
%LET RPTLIB = T:\SAS;
FILENAME REPORT2 "&RPTLIB/ULWG98.LWG98R2";
FILENAME REPORTZ "&RPTLIB/ULWG98.LWG98RZ";
/*CALCULATE FISCAL YEAR DATES*/
DATA _NULL_;
	CDATE = TODAY();
	IF MONTH(CDATE) < 7 THEN DO;
		BEG = "'"||CATS('07/01/',YEAR(CDATE)-1)||"'";
		END = "'"||CATS('06/30/',YEAR(CDATE))||"'";
	END;
	ELSE DO;
		BEG = "'"||CATS('07/01/',YEAR(CDATE))||"'";
		END = "'"||CATS('06/30/',YEAR(CDATE)+1)||"'";
	END;
	CALL SYMPUT('FY_BEG',TRIM(BEG));
	CALL SYMPUT('FY_END',TRIM(END));
RUN;
%SYSLPUT FY_BEG = &FY_BEG;
%SYSLPUT FY_END = &FY_END;
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
CREATE TABLE YTDGVS AS
SELECT *
FROM CONNECTION TO DB2 (
SELECT DISTINCT B.AF_APL_ID||B.AF_APL_ID_SFX AS CLUID
	,B.AA_GTE_LON_AMT - COALESCE(B.AA_TOT_RFD,0) - COALESCE(B.AA_TOT_CAN,0) AS GTE_AMT
	,A.AF_APL_OPS_SCL
	,SCL.IM_IST_FUL AS SCL_NM
	,A.AF_CUR_APL_OPS_LDR
	,LDR.IM_IST_FUL AS LDR_NM
FROM OLWHRM1.GA01_APP A
INNER JOIN OLWHRM1.GA10_LON_APP B
	ON A.AF_APL_ID = B.AF_APL_ID
LEFT OUTER JOIN OLWHRM1.IN01_LGS_IDM_MST LDR
	ON A.AF_CUR_APL_OPS_LDR = LDR.IF_IST
LEFT OUTER JOIN OLWHRM1.SC01_LGS_SCL_INF SCL
	ON A.AF_APL_OPS_SCL = SCL.IF_IST
WHERE B.AC_PRC_STA = 'A'
	AND B.AD_PRC BETWEEN &FY_BEG AND &FY_END
	AND B.AA_GTE_LON_AMT - COALESCE(B.AA_TOT_RFD,0) - COALESCE(B.AA_TOT_CAN,0) <> 0
FOR READ ONLY WITH UR
);
DISCONNECT FROM DB2;
/*%PUT  SQLXRC= >>> &SQLXRC <<< ||| SQLXMSG= >>> &SQLXMSG >>>  ;  * INCLUDES ERROR MESSAGES TO SAS LOG  ;*/
/*%SQLCHECK (SQLRPT=ULWG98.LWG98RZ);*/
/*QUIT;*/
ENDRSUBMIT;
DATA YTDGVS ;
	SET WORKLOCL.YTDGVS;
RUN;
PROC SORT DATA=YTDGVS;
	BY SCL_NM LDR_NM;
RUN;
PROC SQL;
	CREATE TABLE Y2D_TOT AS 
		SELECT AF_APL_OPS_SCL
			,SCL_NM
			,AF_CUR_APL_OPS_LDR
			,LDR_NM
			,COUNT(*) AS LN_CT
			,SUM(GTE_AMT) AS GTE_SUM
		FROM YTDGVS
		WHERE SCL_NM ^= ''
		GROUP BY AF_APL_OPS_SCL
			,SCL_NM
			,AF_CUR_APL_OPS_LDR
			,LDR_NM
		ORDER BY AF_APL_OPS_SCL
			,SCL_NM
			,AF_CUR_APL_OPS_LDR
			,LDR_NM
	;
QUIT;
PROC PRINTTO PRINT=REPORT2 NEW;
RUN;
OPTIONS ORIENTATION = LANDSCAPE;
OPTIONS PS=39 LS=127 DATE CENTER PAGENO=1;
TITLE 'YEAR TO DATE GUARANTY VOLUME';
TITLE2	'GROUPED BY SCHOOL AND SORTED BY LENDER';
FOOTNOTE   'JOB = UTLWG98  	 REPORT = ULWG98.LWG98R2';
PROC CONTENTS DATA=Y2D_TOT OUT=EMPTYSET NOPRINT;
DATA _NULL_;
SET EMPTYSET;
FILE PRINT;
IF  NOBS=0 AND _N_ =1 THEN DO;
	PUT // 132*'-';
	PUT      //////
		@51 '**** NO OBSERVATIONS FOUND ****';
	PUT //////
		@57 '-- END OF REPORT --';
	PUT //////////////
		@46 "JOB = UTLWG98  	 REPORT = ULWG98.LWG98R2";
	END;
RETURN;
RUN;
PROC PRINT NOOBS SPLIT='/' DATA=Y2D_TOT WIDTH=UNIFORM WIDTH=MIN LABEL;
FORMAT LN_CT COMMA8. GTE_SUM DOLLAR18.2;
VAR AF_APL_OPS_SCL SCL_NM AF_CUR_APL_OPS_LDR LDR_NM LN_CT GTE_SUM;
LABEL AF_APL_OPS_SCL = 'SCHOOL/ID'
	SCL_NM = 'SCHOOL/NAME' 
	AF_CUR_APL_OPS_LDR = 'LENDER/ID' 
	LDR_NM = 'LENDER/NAME' 
	LN_CT = '#/LOANS' 
	GTE_SUM = '$/LOANS';
RUN;
PROC PRINTTO;
RUN;
/*DETAIL FILE*/
DATA _NULL_;
SET  WORK.YTDGVS;
FILE 'T:\SAS\UTLWG98.Detail.txt' DELIMITER=',' DSD DROPOVER LRECL=32767;
IF _N_ = 1 THEN DO;
   PUT
	   'CLUID'
	   ','
	   'GTE_AMT'
	   ','
	   'AF_APL_OPS_SCL'
	   ','
	   'SCL_NM'
	   ','
	   'AF_CUR_APL_OPS_LDR'
	   ','
	   'LDR_NM'
	   ;
END;
DO;
   PUT CLUID $ @;
   PUT GTE_AMT @;
   PUT AF_APL_OPS_SCL $ @;
   PUT SCL_NM $ @;
   PUT AF_CUR_APL_OPS_LDR @ ;
   PUT LDR_NM ; 
END;
RUN;






