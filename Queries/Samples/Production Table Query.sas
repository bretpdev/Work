/*****************************************************/
/*	connect to CYPRUS the SAS AIX node (7)		     */
/*****************************************************/
/*filename rlink '!sasroot\connect\saslink\tcpunix.scr'  ;*/
/*options comamid=tcp remote=cyprus  ;*/
/*signon cyprus ;*/
/*********************************************************/
/*This query finds cases of Legal Address/Phone Disunion**/
/*********************************************************/
/**00000000000000000000000000000000***********************/
RSUBMIT  CYPRUS  ;
OPTIONS NOCENTER NODATE NONUMBER LS=132 mprint symbolgen  ;
%macro sortit (ds, byvars)  ;
 PROC SORT  DATA=&ds  ;
   BY &byvars  ;
 RUN  ;
%mend  ;
/*********************************************************/
PROC SQL;
CONNECT TO DB2 (DATABASE=DLGSUTOL  USER=dbuheaa  USING=XXXXXXXX) ;
CREATE TABLE DEMOL AS
SELECT *
FROM CONNECTION TO DB2 (
SELECT	distinct
		integer(D.bf_ssn)				as SSN,
		D.lc_pcl_rea					AS PCLREA,
		D.lc_sta_dc10					as CLMSTA,
		RTRIM(a.DM_PRS_LST)				as NAME,
		C.dc_skp_trc_sta				AS SKPSTA,
		C.dd_skp_trc_eff				AS SKPDT,
		CASE 
		WHEN C.dn_alt_phn = ' ' THEN C.dn_alt_phn
		ELSE SUBSTR(C.dn_alt_phn,1,3)||'-'||SUBSTR(C.dn_alt_phn,4,3)
			 ||'-'||SUBSTR(C.dn_alt_phn,7,4)
		END								AS ALTPHN,
		C.di_alt_phn_vld				AS ALTVAL,
		C.di_vld_adr					AS LADRVLD,
		C.di_phn_vld					AS LPHNVLD
FROM  dbuheaa.DC10_LON_CLM D 
		left outer join 
      dbuheaa.PD01_PDM A
	on D.bf_ssn = A.DF_PRS_ID
		join
	dbuheaa.PD03_PRS_ADR_PHN C
	on D.bf_ssn = c.DF_PRS_ID
WHERE C.dc_adr = 'L'
AND ((C.di_vld_adr = 'Y' AND C.di_phn_vld = 'N')
	OR (C.di_vld_adr = 'N' AND C.di_phn_vld = 'Y'))
AND D.ld_clm_asn_doe IS NULL
AND D.ld_pcl_rcv = (select max(p.ld_pcl_rcv)
					from dbuheaa.DC10_LON_CLM P
					where p.bf_ssn = d.bf_ssn)
);

CREATE TABLE DEMOA AS
SELECT *
FROM CONNECTION TO DB2(
SELECT DISTINCT
		integer(A.DF_PRS_ID)			as SSN,
		A.di_vld_adr					AS AADRVLD,
		A.di_phn_vld					AS APHNVLD
FROM  dbuheaa.PD03_PRS_ADR_PHN A
WHERE A.dc_adr = 'A'
AND EXISTS (SELECT *
			FROM  dbuheaa.DC10_LON_CLM Y
					LEFT OUTER JOIN 
				  dbuheaa.PD03_PRS_ADR_PHN X
			on Y.bf_ssn = X.DF_PRS_ID
			WHERE X.DF_PRS_ID = A.DF_PRS_ID
			AND X.dc_adr = 'L'
			AND ((X.di_vld_adr = 'Y' AND X.di_phn_vld = 'N')
				OR (X.di_vld_adr = 'N' AND X.di_phn_vld = 'Y'))
			AND Y.ld_clm_asn_doe IS NULL
			)			
);

CREATE TABLE DEMOT AS
SELECT *
FROM CONNECTION TO DB2(
SELECT DISTINCT
		integer(A.DF_PRS_ID)			as SSN,
		A.di_vld_adr					AS TADRVLD,
		A.di_phn_vld					AS TPHNVLD
FROM  dbuheaa.PD03_PRS_ADR_PHN A
WHERE A.dc_adr = 'T'
AND EXISTS (SELECT *
			FROM  dbuheaa.DC10_LON_CLM Y
					LEFT OUTER JOIN 
				  dbuheaa.PD03_PRS_ADR_PHN X
			on Y.bf_ssn = X.DF_PRS_ID
			WHERE X.DF_PRS_ID = A.DF_PRS_ID
			AND X.dc_adr = 'L'
			AND ((X.di_vld_adr = 'Y' AND X.di_phn_vld = 'N')
				OR (X.di_vld_adr = 'N' AND X.di_phn_vld = 'Y'))
			AND Y.ld_clm_asn_doe IS NULL
			)			
);
DISCONNECT FROM DB2;
QUIT  ;

%sortit (demol, ssn)  ;
%sortit (demoa, ssn)  ;
%sortit (demot, ssn)  ;

DATA DEMOALL;
MERGE DEMOL DEMOA DEMOT;
BY SSN;
RUN;

PROC SORT data=DEMOALL;
BY PCLREA CLMSTA SSN;
RUN;

/*********************************************************/
%macro report (r1,r2)  ;
filename  &r1  "/sas/whse/mctest&r1..lst"  ;  run  ;
filename  &r2  "/sas/whse/mctest&r2..lst"  ;  run  ;

OPTIONS NOCENTER NODATE NUMBER LS=120 PAGENO=1;
proc printto  print= &r1  ;  run  ;
PROC REPORT DATA = DEMOALL NOWD SPACING=2 HEADSKIP SPLIT='/' ; 
COLUMN SSN NAME PCLREA CLMSTA SKPSTA SKPDT ALTPHN ALTVAL 
AADRVLD APHNVLD TADRVLD TPHNVLD N CSUM ;
WHERE lADRVLD = 'Y' AND lPHNVLD = 'N' ;
BY PCLREA ;
DEFINE SSN / FORMAT=SSN11. DISPLAY LEFT;
DEFINE NAME / 'Last Name' WIDTH=15;
DEFINE PCLREA / 'PCL/Rea' WIDTH=3;
DEFINE CLMSTA / 'Claim/Status' WIDTH=6;
DEFINE SKPSTA / 'Skip/Status'WIDTH=6;
DEFINE SKPDT / 'Skip/Date' DISPLAY WIDTH=9;
DEFINE ALTPHN / 'Other Phone';
DEFINE ALTVAL / 'Other/Phone/Valid' WIDTH=5;
DEFINE AADRVLD / 'Alt/Phone/Valid' WIDTH=5;
DEFINE APHNVLD / 'Alt/Phone/Valid' WIDTH=5;
DEFINE TADRVLD / 'Temp/Phone/Valid' WIDTH=5;
DEFINE TPHNVLD / 'Temp/Phone/Valid' WIDTH=5;
DEFINE N /  NOPRINT WIDTH=5;
DEFINE CSUM / NOPRINT COMPUTED WIDTH = 5;
RBREAK AFTER / SUMMARIZE SKIP DOL;

COMPUTE CSUM;
CSUM = N;
ENDCOMP;

COMPUTE AFTER;
LINE "Count:  "CSUM COMMA6.;
ENDCOMP;
TITLE 'Valid Address/Phone Disunion - Valid Address w/ Invalid Phone';
FOOTNOTE;
RUN;

OPTIONS NOCENTER NODATE NUMBER LS=120 PAGENO=1;
proc printto  print= &r2  ;  run  ;
PROC REPORT DATA = DEMOALL NOWD SPACING=2 HEADSKIP SPLIT='/' ; 
COLUMN SSN NAME PCLREA CLMSTA SKPSTA SKPDT ALTPHN ALTVAL 
AADRVLD APHNVLD TADRVLD TPHNVLD N CSUM ;
WHERE lADRVLD = 'N' AND lPHNVLD = 'Y' ;
BY PCLREA ;
DEFINE SSN / FORMAT=SSN11. DISPLAY LEFT;
DEFINE NAME / 'Last Name' WIDTH=15;
DEFINE PCLREA / 'PCL/Rea' WIDTH=3;
DEFINE CLMSTA / 'Claim/Status' WIDTH=6;
DEFINE SKPSTA / 'Skip/Status'WIDTH=6;
DEFINE SKPDT / 'Skip/Date' DISPLAY WIDTH=9;
DEFINE ALTPHN / 'Other Phone';
DEFINE ALTVAL / 'Other/Phone/Valid' WIDTH=5;
DEFINE AADRVLD / 'Alt/Addr/Valid' WIDTH=5;
DEFINE APHNVLD / 'Alt/Phone/Valid' WIDTH=5;
DEFINE TADRVLD / 'Temp/Addr/Valid' WIDTH=5;
DEFINE TPHNVLD / 'Temp/Phone/Valid' WIDTH=5;
DEFINE N /  NOPRINT WIDTH=5;
DEFINE CSUM / NOPRINT COMPUTED WIDTH = 5;
RBREAK AFTER / SUMMARIZE SKIP DOL;

COMPUTE CSUM;
CSUM = N;
ENDCOMP;

COMPUTE AFTER;
LINE "Count:  "CSUM COMMA6.;
ENDCOMP;
TITLE 'Valid Address/Phone Disunion - Invalid Address w/ Valid Phone';
FOOTNOTE;
RUN;
%mend  ;
/*********************************************************/
%report (rep01,rep02) ;
/*********************************************************/
endrsubmit  ;

/*libname  cypruswk  remote  server=cyprus  slibref=work  ;*/
/**/
/*libname  hjh  'c:\temp\'  ;*/
/*proc copy  in=cypruswk  out=hjh  ;*/
/* select   demoa demoall demol demot  ;*/
/*run  ;*/

