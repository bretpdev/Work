/*NOTE:  Clear out the Y:\Development\SAS Test Files\School Reports folder before running this job*/


%LET RPTLIB = T:\SAS;
LIBNAME  LEGEND  REMOTE  SERVER=LEGEND SLIBREF=work  ;
RSUBMIT LEGEND;
PROC SQL;
CONNECT TO DBX (DATABASE=DNFPUTDL);
CREATE TABLE GEN AS
SELECT *
FROM CONNECTION TO DBX (
SELECT A.BF_SSN
	,A.LN_SEQ
	,A.IC_LON_PGM
	,SUBSTR(A.LF_DOE_SCL_ORG,X,X) AS LF_DOE_SCL_ORG_PRE
	,SUBSTR(A.LF_DOE_SCL_ORG,X,X) AS LF_DOE_SCL_ORG_BNCH
	,A.LF_DOE_SCL_ORG
	,A.LA_CUR_PRI
	,B.WC_DW_LON_STA
	,B.WD_LON_RPD_SR
	,B.WA_TOT_BRI_OTS
	,C.DM_PRS_X
	,C.DM_PRS_LST
	,D.DX_STR_ADR_X
	,D.DX_STR_ADR_X
	,D.DM_CT
	,COALESCE(D.DC_DOM_ST,D.DM_FGN_ST) AS DC_DOM_ST
	,D.DF_ZIP_CDE
	,PHN_H.DN_DOM_PHN_ARA||PHN_H.DN_DOM_PHN_XCH||PHN_H.DN_DOM_PHN_LCL AS HPHN
	,PHN_W.DN_DOM_PHN_ARA||PHN_W.DN_DOM_PHN_XCH||PHN_W.DN_DOM_PHN_LCL AS WPHN
	,PHN_A.DN_DOM_PHN_ARA||PHN_A.DN_DOM_PHN_XCH||PHN_A.DN_DOM_PHN_LCL AS APHN
	,PHN_M.DN_DOM_PHN_ARA||PHN_M.DN_DOM_PHN_XCH||PHN_M.DN_DOM_PHN_LCL AS MPHN
	,PHN_H.DI_PHN_VLD AS DI_PHN_VLD_H
	,PHN_W.DI_PHN_VLD AS DI_PHN_VLD_W
	,PHN_A.DI_PHN_VLD AS DI_PHN_VLD_A
	,PHN_M.DI_PHN_VLD AS DI_PHN_VLD_M
	,EML.DX_ADR_EML
	,EML.DI_VLD_ADR_EML
	,COALESCE(E.LN_DLQ_MAX,X) AS DYS_DLQ
	,E.LD_DLQ_OCC AS DLQ_DT
	,SDXX.LD_NTF_SCL_SPR
	,F.LC_TYP_SCH_DIS 
	,DAY(G.LD_RPS_X_PAY_DU) AS LD_RPS_X_PAY_DU
	,SCXX.IM_SCL_FUL
FROM PKUB.LNXX_LON A
INNER JOIN PKUB.DWXX_DW_CLC_CLU B
	ON A.BF_SSN = B.BF_SSN
	AND A.LN_SEQ = B.LN_SEQ
/*DEMOS*/
INNER JOIN PKUB.PDXX_PRS_NME C
	ON B.BF_SSN = C.DF_PRS_ID
LEFT OUTER JOIN PKUB.PDXX_PRS_ADR D
	ON C.DF_PRS_ID = D.DF_PRS_ID
	AND D.DC_ADR = 'L'
LEFT OUTER JOIN PKUB.PDXX_PRS_PHN PHN_H
	ON C.DF_PRS_ID = PHN_H.DF_PRS_ID
	AND PHN_H.DC_PHN = 'H'
LEFT OUTER JOIN PKUB.PDXX_PRS_PHN PHN_W
	ON C.DF_PRS_ID = PHN_W.DF_PRS_ID
	AND PHN_W.DC_PHN = 'W'
LEFT OUTER JOIN PKUB.PDXX_PRS_PHN PHN_A
	ON C.DF_PRS_ID = PHN_A.DF_PRS_ID
	AND PHN_A.DC_PHN = 'A'
LEFT OUTER JOIN PKUB.PDXX_PRS_PHN PHN_M
	ON C.DF_PRS_ID = PHN_M.DF_PRS_ID
	AND PHN_M.DC_PHN = 'M'
LEFT OUTER JOIN PKUB.PDXX_PRS_ADR_EML EML
	ON C.DF_PRS_ID = EML.DF_PRS_ID
	AND EML.DC_STA_PDXX = 'A'
	AND EML.DC_ADR_EML = 'H'
/*DELINQUENCY*/
LEFT OUTER JOIN PKUB.LNXX_LON_DLQ_HST E
	ON A.BF_SSN = E.BF_SSN
	AND A.LN_SEQ = E.LN_SEQ
	AND E.LC_STA_LONXX = 'X'
/*SCHOOL SEPARATION AND SCHOOL INFO*/
LEFT OUTER JOIN PKUB.LNXX_LON_STU_OSD LNXX
	ON A.BF_SSN = LNXX.BF_SSN
	AND A.LN_SEQ = LNXX.LN_SEQ
	AND A.LF_STU_SSN = LNXX.LF_STU_SSN
	AND LNXX.LC_STA_LONXX = 'A'
LEFT OUTER JOIN PKUB.SDXX_STU_SPR SDXX
	ON LNXX.LF_STU_SSN = SDXX.LF_STU_SSN
	AND LNXX.LN_STU_SPR_SEQ = SDXX.LN_STU_SPR_SEQ
	AND SDXX.LC_STA_STUXX = 'A'
LEFT OUTER JOIN PKUB.SCXX_SCH_DMO SCXX
	ON A.LF_DOE_SCL_ORG = SCXX.IF_DOE_SCL
/*REAPYMENT SCHEDULE*/
LEFT OUTER  JOIN PKUB.LNXX_LON_RPS F
	ON A.BF_SSN = F.BF_SSN
	AND A.LN_SEQ = F.LN_SEQ
	AND F.LC_STA_LONXX = 'A'
LEFT OUTER JOIN PKUB.RSXX_BR_RPD G
	ON F.BF_SSN = G.BF_SSN
	AND F.LN_RPS_SEQ = G.LN_RPS_SEQ
	AND G.LC_STA_RPSTXX = 'A'
WHERE A.LA_CUR_PRI > X 
	AND A.LC_STA_LONXX = 'R' 
	AND A.LF_DOE_SCL_ORG != 'XXXXXXXX'
/*if given a subset of schools, use code below*/
/*	AND SUBSTR(A.LF_DOE_SCL_ORG,X,X) IN ('XXXXXX',*/
/*'XXXXXX',*/
/*'XXXXXX',*/
/*'XXXXXX',*/
/*'XXXXXX',*/
/*'XXXXXX',*/
/*'XXXXXX',*/
/*'XXXXXX',*/
/*'XXXXXX',*/
/*'XXXXXX',*/
/*'XXXXXX',*/
/*'XXXXXX',*/
/*'XXXXXX',*/
/*'XXXXXX',*/
/*'XXXXXX',*/
/*'XXXXXX',*/
/*'XXXXXX',*/
/*'XXXXXX',*/
/*'XXXXXX',*/
/*'XXXXXX',*/
/*'XXXXXX',*/
/*'XXXXXX',*/
/*'XXXXXX',*/
/*'XXXXXX',*/
/*'XXXXXX',*/
/*'XXXXXX',*/
/*'XXXXXX',*/
/*'XXXXXX',*/
/*'XXXXXX',*/
/*'XXXXXX',*/
/*'XXXXXX',*/
/*'XXXXXX',*/
/*'XXXXXX',*/
/*'XXXXXX',*/
/*'XXXXXX',*/
/*'XXXXXX',*/
/*'XXXXXX',*/
/*'XXXXXX',*/
/*'XXXXXX',*/
/*'XXXXXX',*/
/*'XXXXXX',*/
/*'XXXXXX',*/
/*'XXXXXX',*/
/*'XXXXXX',*/
/*'XXXXXX',*/
/*'XXXXXX',*/
/*'XXXXXX',*/
/*'XXXXXX',*/
/*'XXXXXX',*/
/*'XXXXXX',*/
/*'XXXXXX',*/
/*'XXXXXX',*/
/*'XXXXXX',*/
/*'XXXXXX',*/
/*'XXXXXX',*/
/*'XXXXXX',*/
/*'XXXXXX',*/
/*'XXXXXX',*/
/*'XXXXXX',*/
/*'XXXXXX',*/
/*'XXXXXX')*/

ORDER BY A.BF_SSN
	,A.LF_DOE_SCL_ORG
	,A.LN_SEQ
FOR READ ONLY WITH UR
);

CREATE TABLE SCHL_DEMO AS
SELECT *
FROM CONNECTION TO DBX (
SELECT IF_DOE_SCL
	,IC_SCL_DPT 
	,IM_SCL_CT
	,IC_SCL_DOM_ST
FROM pkub.scXX_sch_dpt
FOR READ ONLY WITH UR
);

DISCONNECT FROM DBX;
QUIT;
PROC SORT DATA=SCHL_DEMO NODUPKEY; 
BY IF_DOE_SCL IC_SCL_DPT;
RUN;

PROC TRANSPOSE DATA=GEN OUT=SCL_LOANS(DROP=_NAME_ _LABEL_) PREFIX=LOAN;
	VAR IC_LON_PGM;
	BY BF_SSN LF_DOE_SCL_ORG_PRE;
RUN;

PROC TRANSPOSE DATA=GEN OUT=LOAN_STA(DROP=_NAME_ _LABEL_) PREFIX=LNSTA;
	VAR WC_DW_LON_STA;
	BY BF_SSN LF_DOE_SCL_ORG_PRE;
RUN;
/*RSUBMIT;*/
/*CREATE BORROWER/SCHOOL LEVEL DATA SET*/
PROC SQL;
CREATE TABLE BOR_SUM_DAT AS 
SELECT DISTINCT  A.BF_SSN
	,CASE
		WHEN DYS_DLQ > XX THEN 'DELINQUENT'
		ELSE 'CURRENT'
	 END AS BOR_STAT
	,A.DM_PRS_X
	,A.DM_PRS_LST
	,A.DX_STR_ADR_X
	,A.DX_STR_ADR_X
	,A.DM_CT
	,A.DC_DOM_ST
	,A.DF_ZIP_CDE
	,A.HPHN
	,A.DI_PHN_VLD_H
	,A.WPHN
	,A.DI_PHN_VLD_W
	,A.APHN
	,A.DI_PHN_VLD_A
	,A.MPHN
	,A.DI_PHN_VLD_M
	,A.DX_ADR_EML
	,A.DI_VLD_ADR_EML
	,C.LA_CUR_PRI
	,C.WA_TOT_BRI_OTS
	,C.LA_CUR_PRI + C.WA_TOT_BRI_OTS AS TOT_BAL
	,B.LOANX
	,B.LOANX
	,B.LOANX
	,B.LOANX
	,B.LOANX
	,B.LOANX
	,D.LNSTAX
	,D.LNSTAX
	,D.LNSTAX
	,D.LNSTAX
	,D.LNSTAX
	,D.LNSTAX
	/*,A.WC_DW_LON_STA */ /*LOAN LEVEL - NEED TO DECIDE HOW TO ROLL TO BORROWER LEVEL*/
	,A.DYS_DLQ
	,A.DLQ_DT
	,A.LD_NTF_SCL_SPR
	,A.LC_TYP_SCH_DIS /*POSSIBLE LOAN LEVEL TOO*/
	,A.LD_RPS_X_PAY_DU
	,C.LN_CT
	,C.WD_LON_RPD_SR
	,YEAR(INTNX('MONTH',C.WD_LON_RPD_SR,X)) AS CHRT_YR
/*	,A.LF_DOE_SCL_ORG*/
	,A.IM_SCL_FUL
	,A.LF_DOE_SCL_ORG_PRE
	,A.LF_DOE_SCL_ORG_BNCH
	,E.IM_SCL_CT
	,E.IC_SCL_DOM_ST
FROM GEN A
INNER JOIN SCL_LOANS B
	ON A.BF_SSN = B.BF_SSN
	AND A.LF_DOE_SCL_ORG_PRE = B.LF_DOE_SCL_ORG_PRE
INNER JOIN LOAN_STA D
	ON A.BF_SSN = D.BF_SSN
	AND A.LF_DOE_SCL_ORG_PRE = D.LF_DOE_SCL_ORG_PRE
INNER JOIN (
	SELECT BF_SSN
		,LF_DOE_SCL_ORG_PRE
		,SUM(LA_CUR_PRI) AS LA_CUR_PRI
		,SUM(WA_TOT_BRI_OTS) AS WA_TOT_BRI_OTS
		,MIN(WD_LON_RPD_SR) AS WD_LON_RPD_SR
		,COUNT(*) AS LN_CT
	FROM GEN
	GROUP BY BF_SSN
		,LF_DOE_SCL_ORG_PRE
	) C
	ON A.BF_SSN = C.BF_SSN
	AND A.LF_DOE_SCL_ORG_PRE = C.LF_DOE_SCL_ORG_PRE
LEFT OUTER JOIN SCHL_DEMO E
	ON A.LF_DOE_SCL_ORG = E.IF_DOE_SCL
/*GROUP BY A.BF_SSN*/
/*	,A.LF_DOE_SCL_ORG_PRE*/
;
QUIT;

ENDRSUBMIT;

options nonotes nosource nosourceX errors=X;
libname LEGEND 'T:\';

DATA BOR_SUM_DAT;
	SET LEGEND.BOR_SUM_DAT;
RUN;

/*PROC EXPORT DATA= BOR_SUM_DAT*/
/*            OUTFILE= "T:\SAS\RawDataSchoolsWithSchoolCityandST.xls" */
/*            DBMS=EXCEL REPLACE;*/
/*RUN;*/


/*proc sort data=BOR_SUM_DAT; by lf_doe_scl_org dm_prs_lst dm_prs_X; run;*/

/*data BOR_SUM_DATX;*/ 
/*set BOR_SUM_DAT;*/
/*im_scl_ful = tranwrd(im_scl_ful, '/','-');*/
/*im_scl_ful = tranwrd(im_scl_ful, '\','-');*/
/*im_scl_ful = tranwrd(im_scl_ful, ',','-');*/
/*im_scl_ful = tranwrd(im_scl_ful, "'",' ');*/
/*run;*/

options nonotes nosource nosourceX errors=X;
data bakup;
set BOR_SUM_DAT;
run;

proc sql;
      create TABLE unique_schools as
            select distinct
                  LF_DOE_SCL_ORG_PRE||LF_DOE_SCL_ORG_BNCH AS LF_DOE_SCL_ORG
            from
                  bor_sum_dat
      ;
quit;

data schools;
      set unique_schools;
      pk = _n_;

/*    ttlct = _n_;*/
run;


proc sql noprint;
select count(distinct LF_DOE_SCL_ORG) into : ttlct
from schools;
quit;

%LET CT = X;
%put &ttlct;

%macro OneRpt(cde);
      libname ex excel "Y:\Development\SAS Test Files\School Reports\&cde - %sysfunc(today(),mmddyyX.).xlsx";

      data ex.SchoolReport(DBLABEL=YES);
            set BOR_SUM_DAT;
            if LF_DOE_SCL_ORG_PRE||LF_DOE_SCL_ORG_BNCH = "&cde" then output ex.SchoolReport;
      run;

      libname ex clear;
%mend;

%macro SchRep();
%do i=&CT %to &ttlct;
      proc sql noprint;
            select LF_DOE_SCL_ORG into : SchNm 
            from schools
            where pk = &ct;
      quit;
      %put &SchNm;

      %OneRpt(&SchNm);
      
	  %LET CT = &CT + X;
	  %PUT &CT;

%end;
%mend;

%SchRep;
