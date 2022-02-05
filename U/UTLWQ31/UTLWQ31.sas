*-----------------------------------*
| UTLWQ31 Cancel/Refund Volume FYTD |
*-----------------------------------*;
/*LIBNAME DLGSUTWH DB2 DATABASE=DLGSUTWH OWNER=OLWHRM1;*/
/*%LET RPTLIB = %SYSGET(reportdir);*/
%LET RPTLIB = T:\SAS;
FILENAME REPORT2 "&RPTLIB/ULWQ31.LWQ31R2";
FILENAME REPORT3 "&RPTLIB/ULWQ31.LWQ31R3";
FILENAME REPORTZ "&RPTLIB/ULWQ31.LWQ31RZ";
DATA _NULL_;
 	CALL SYMPUT('BEGIN',"'"||PUT(INTNX('YEAR.7',TODAY(),0,'BEGINNING'), MMDDYY10.)||"'");
	CALL SYMPUT('END',"'"||PUT(INTNX('YEAR.7',TODAY(),0,'END'), MMDDYY10.)||"'");
RUN;
%SYSLPUT BEGIN = &BEGIN;
%SYSLPUT END = &END;
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
CREATE TABLE CRVFYTD AS
SELECT *
FROM CONNECTION TO DB2 (
SELECT A.DF_PRS_ID_BR
	,B.AF_APL_ID||B.AF_APL_ID_SFX AS CLUID
	,B.AD_PRC
	,B.AC_PRC_STA
	,C.AN_DSB_SEQ
	,COALESCE(C.AA_DSB_ADJ,0) AS AA_DSB_ADJ
	,C.AD_DSB_ADJ
	,C.AC_DSB_ADJ_STA
	,C.AC_DSB_ADJ 
	,MONTH(C.AD_DSB_ADJ) AS MO
	,RTRIM(UCASE(MONTHNAME(C.AD_DSB_ADJ))) AS MONAME
	,CASE
		WHEN B.AD_PRC BETWEEN &BEGIN AND &END THEN 'CFY'
		ELSE 'PFY'
	 END AS FY
FROM OLWHRM1.GA01_APP A
INNER JOIN OLWHRM1.GA10_LON_APP B
	ON A.AF_APL_ID = B.AF_APL_ID
INNER JOIN OLWHRM1.GA11_LON_DSB_ATY C
	ON B.AF_APL_ID = C.AF_APL_ID
	AND B.AF_APL_ID_SFX = C.AF_APL_ID_SFX
WHERE C.AD_DSB_ADJ BETWEEN &BEGIN AND &END
	AND C.AC_DSB_ADJ_STA = 'A'
	AND C.AC_DSB_ADJ IN ('C','S','U')
FOR READ ONLY WITH UR
);
DISCONNECT FROM DB2;
/*%PUT  SQLXRC= >>> &SQLXRC <<< ||| SQLXMSG= >>> &SQLXMSG >>>  ;  * INCLUDES ERROR MESSAGES TO SAS LOG  ;*/
/*%SQLCHECK (SQLRPT=ULWQ31.LWQ31RZ);*/
/*QUIT;*/
ENDRSUBMIT;
DATA CRVFYTD;
	SET WORKLOCL.CRVFYTD;
RUN;
PROC SQL;
CREATE TABLE FYDAT AS 
SELECT FY
	,CASE 
		WHEN MO >= 7 THEN MO - 6
		ELSE MO + 6
	 END AS MO
	,MONAME
	,SUM(AA_DSB_ADJ) AS AMT
	,COUNT(DISTINCT CLUID) AS LONS
	,COUNT(DISTINCT DF_PRS_ID_BR) AS BORS
FROM CRVFYTD
GROUP BY FY
	,MO
	,MONAME 
ORDER BY FY
	,MO
;
QUIT;
%MACRO Q31_REP(FYI,RNO,RTITLE);
PROC SORT DATA=FYDAT OUT=REPDS(WHERE=(FY="&FYI"));
	BY MO;
RUN;
PROC PRINTTO PRINT=REPORT&RNO NEW;
RUN;
OPTIONS ORIENTATION = LANDSCAPE;
OPTIONS PS=39 LS=127;
TITLE "CANCEL/REFUND VOLUME FYTD";
TITLE2 "&RTITLE";
FOOTNOTE   "JOB = UTLWQ31  	 REPORT = ULWQ31.LWQ31R&RNO";
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
		@46 "JOB = UTLWQ31  	 REPORT = ULWQ31.LWQ31R2";
	END;
RETURN;
RUN;
PROC PRINT NOOBS SPLIT='/' DATA=REPDS WIDTH=UNIFORM WIDTH=MIN LABEL;
FORMAT AMT DOLLAR18.2;
VAR MONAME AMT LONS BORS;
LABEL MONAME = 'MONTH' AMT = '$/AMOUNT' LONS = '#/LOANS' BORS = '#/BORROWERS'
;
RUN;
PROC PRINTTO;
RUN;
%MEND Q31_REP;
%Q31_REP(CFY,2,CURRENT YEAR LOANS);
%Q31_REP(PFY,3,PAST FISCAL YEAR LOANS);

/*DETAIL FILE*/
DATA _NULL_;
FILE 'T:\SAS\SASR.2598.Detail.txt' DELIMITER=',' DSD DROPOVER LRECL=32767;
IF _N_ = 1 THEN DO;
PUT
      "DF_PRS_ID_BR"
   ','
      "CLUID"
   ','
      "AD_PRC"
   ','
      "AC_PRC_STA"
   ','
      "AN_DSB_SEQ"
   ','
      "AA_DSB_ADJ"
   ','
      "AD_DSB_ADJ"
   ','
      "AC_DSB_ADJ_STA"
   ','
      "AC_DSB_ADJ"
   ;
END;
SET  WORK.CRVFYTD;
   FORMAT DF_PRS_ID_BR $9. ;
   FORMAT CLUID $19. ;
   FORMAT AD_PRC mmddyy10. ;
   FORMAT AC_PRC_STA $1. ;
   FORMAT AN_DSB_SEQ 6. ;
   FORMAT AA_DSB_ADJ 15.2 ;
   FORMAT AD_DSB_ADJ mmddyy10. ;
   FORMAT AC_DSB_ADJ_STA $1. ;
   FORMAT AC_DSB_ADJ $1. ;
DO;
   PUT DF_PRS_ID_BR $ @;
   PUT CLUID $ @;
   PUT AD_PRC @;
   PUT AC_PRC_STA $ @;
   PUT AN_DSB_SEQ @;
   PUT AA_DSB_ADJ @;
   PUT AD_DSB_ADJ @;
   PUT AC_DSB_ADJ_STA $ @;
   PUT AC_DSB_ADJ $;
END;
RUN;