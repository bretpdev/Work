/*	Outstanding Supplemental Claims
Lists supplemental claims received but not processed.
*/

/*LIBNAME DLGSUTWH DB2 DATABASE=DLGSUTWH OWNER=OLWHRM1;
%LET RPTLIB = %SYSGET(reportdir);
FILENAME REPORT2 "&RPTLIB/ULWC02.LWC02R2";*/

LIBNAME  WORKLOCL  REMOTE  SERVER=CYPRUS  SLIBREF=WORK;
RSUBMIT;
PROC SQL;
CONNECT TO DB2 (DATABASE=DLGSUTWH);
CREATE TABLE SUPPCLM AS
SELECT *
FROM CONNECTION TO DB2 (
SELECT  INTEGER(B.bf_ssn)				AS      SSN,
RTRIM(RTRIM(D.DM_PRS_LST) ||', '||RTRIM(D.DM_PRS_1)||' '||RTRIM(D.DM_PRS_MID))
										AS 		NAME,
        B.AF_APL_ID||B.AF_APL_ID_SFX	AS      CLID,
        TIME(A.LF_CRT_DTS_DC10)			AS      TIMESTMP,
        TIME(A.LF_CRT_DTS_DC14)   		AS      TIME,
        A.LD_SPI_PKG_RCV            	AS      DATE,
        B.IF_OPS_LDR                	AS      HOLDER,
        B.LF_CUR_LON_SER_AGY        	AS      SERVIC,
        C.ac_lon_typ					AS		TYP
FROM    OLWHRM1.DC14_SUP_INT_CLM A INNER JOIN
		OLWHRM1.DC01_LON_CLM_INF B ON
			A.LF_CRT_DTS_DC10 = B.LF_CRT_DTS_DC10
		INNER JOIN OLWHRM1.GA10_LON_APP C ON
			A.af_apl_id = C.af_apl_id AND
			A.af_apl_id_sfx = C.af_apl_id_sfx
		INNER JOIN OLWHRM1.PD01_PDM_INF D
			ON B.BF_SSN = D.DF_PRS_ID
WHERE   A.LD_SPI_PKG_RCV is NOT NULL AND
		A.la_spi_pd = 0 AND
		A.la_spi_not_pd = 0 AND
		A.ld_ver_spi is NULL
		AND A.LD_SPI_PRC IS NULL
		AND A.LD_SPI_PKG_RTN IS NULL	/*ADD TO DW 7/11/02*/
ORDER BY A.LD_SPI_PKG_RCV);
DISCONNECT FROM DB2;
ENDRSUBMIT;
/*PROC PRINTTO PRINT=REPORT2;
RUN;*/
OPTIONS NODATE CENTER PAGENO=1 PS=52 LS=90 ERRORS=0 MISSING=0;
PROC PRINT DATA=WORKLOCL.SUPPCLM N = "TOTAL NUMBER OF SUPPLEMENTAL CLAIMS OUTSTANDING = " 
NOOBS SPLIT='/' WIDTH=UNIFORM WIDTH=MIN;
   VAR SSN NAME CLID TYP HOLDER SERVIC DATE;
   FORMAT DATE MMDDYY8.  TIME TIME14.6 SSN SSN11.;
   LABEL TYP='LOAN TYPE'
   		 CLID = 'UNIQUE ID'
         DATE='DATE/RECEIVED'
         NAME='NAME'
         HOLDER='HOLDER/CODE'
         SERVIC='SERVICER/CODE';
   TITLE1 'SUPPLEMENTALS CLAIMS RECEIVED AND NOT PROCESSED';
   TITLE2 "RUN DATE:  &SYSDATE9 ";
   FOOTNOTE	'JOB = UTLWC02     REPORT = ULWC02.LWC02R2';
RUN;