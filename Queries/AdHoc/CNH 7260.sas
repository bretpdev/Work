/*%LET RPTLIB = %SYSGET(reportdir);*/
%LET RPTLIB = T:\SAS;
FILENAME REPORTZ "&RPTLIB/UNWSXX.NWSXXRZ";
FILENAME REPORTX "&RPTLIB/UNWSXX.NWSXXRX";

LIBNAME  LEGEND  REMOTE  SERVER=LEGEND SLIBREF=work  ;
RSUBMIT LEGEND;
/*%let DB = DNFPRQUT;  *This is test;*/
/*%let DB = DNFPRUUT;  *This is VUKX test;*/
%let DB = DNFPUTDL;  *This is live;

LIBNAME PKUB DBX DATABASE=&DB OWNER=PKUB;

%MACRO SQLCHECK ;
  %IF  &SQLXRC NE X  %THEN  %DO  ;
    DATA _NULL_  ;
            FILE REPORTZ NOTITLES  ;
            PUT @XX " ********************************************************************* "
              / @XX " ****  THE SQL CODE ABOVE HAS EXPERIENCED AN ERROR.               **** "
              / @XX " ****  THE SAS SHOULD BE REVIEWED.                                **** "       
              / @XX " ********************************************************************* "
              / @XX " ****  THE SQL ERROR CODE IS  &SQLXRC  AND THE SQL ERROR MESSAGE  **** "
              / @XX " ****  &SQLXMSG   **** "
              / @XX " ********************************************************************* "
            ;
         RUN  ;
  %END  ;
%MEND  ;

PROC SQL;
	CONNECT TO DBX (DATABASE=DNFPUTDL);

	CREATE TABLE REMOTE_DATA AS
		SELECT
			*
		FROM
			CONNECTION TO DBX
				(
					SELECT DISTINCT
						PDXX.DM_PRS_LST AS LAST_NAME,
						PDXX.DF_SPE_ACC_ID AS ACCOUNT_NUMBER,
						LNXX.LN_IBR_FGV_MTH_CTR AS IBR_COUNTER,
						LNXX.LN_ICR_FGV_MTH_CTR AS ICR_COUNTER,
						LNXX.LN_PSV_FGV_PAY_CTR AS PSLF
					FROM	
						PKUB.PDXX_PRS_NME PDXX
						INNER JOIN PKUB.LNXX_FIN_ATY LNXX
							ON LNXX.BF_SSN = PDXX.DF_PRS_ID
							AND (LNXX.PC_FAT_TYP = 'XX' AND LNXX.PC_FAT_SUB_TYP = 'XX' AND LNXX.LC_FAT_REV_REA = '' AND LNXX.LC_STA_LONXX = 'A') 
						INNER JOIN PKUB.LNXX_LON LNXX
							ON LNXX.BF_SSN = LNXX.BF_SSN
							AND LNXX.LN_SEQ = LNXX.LN_SEQ
						INNER JOIN PKUB.LNXX_RPD_PIO_CVN LNXX
							ON LNXX.BF_SSN = LNXX.BF_SSN
							AND LNXX.LN_SEQ = LNXX.LN_SEQ
						WHERE
							LNXX.LN_IBR_FGV_MTH_CTR IS NOT NULL
							OR LNXX.LN_ICR_FGV_MTH_CTR IS NOT NULL
							OR LNXX.LN_PSV_FGV_PAY_CTR IS NOT NULL
				)
	;

	DISCONNECT FROM DBX;
QUIT;

ENDRSUBMIT;

DATA DEMO; SET LEGEND.REMOTE_DATA; RUN;



/*export to Excel spreadsheet*/
PROC EXPORT DATA = WORK.DEMO 
            OUTFILE = "T:\NH XXXX.xlsx" 
            DBMS = EXCEL
			REPLACE;
     SHEET="SheetX"; 
RUN;
