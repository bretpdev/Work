
PROC IMPORT OUT= WORK.BORROWER
		DATAFILE= "T:\NH 23809.xlsx"
        DBMS=EXCEL REPLACE;
	SHEET="A$";
	GETNAMES=YES;
	MIXED=NO;
	SCANTEXT=YES;
	USEDATE=YES;
	SCANTIME=YES;
RUN;

%LET RPTLIB = T:\SAS;
FILENAME REPORTZ "&RPTLIB/ULWAB1.LWAB1RZ";
FILENAME REPORT2 "&RPTLIB/ULWAB1.LWAB1R2";

LIBNAME  DUSTER  REMOTE  SERVER=DUSTER SLIBREF=WORK;

DATA DUSTER.DATA; *Send data to Duster;
	SET BORROWER;
RUN;

RSUBMIT;
LIBNAME OLWHRM1 DB2 DATABASE=DLGSUTWH OWNER=OLWHRM1;

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
         RUN;
  %END  ;
%MEND  ;


PROC SQL;


	CREATE TABLE DEMO AS

					SELECT DISTINCT
						ACCOUNTNUMBER,
						DFDL1_DATE,
						DFDL1_ARC,
						REPAYMENT_ARRANGEMENT_DATE,
						ARC,
						CLAIM_STATUS,
						COLLECTION_COST_ACCRUED,
						COLLECTION_COST_PROJECTED,
						B.MAXDATE,
						C.LC_TRX_TYP
					FROM
						DATA
					JOIN OLWHRM1.PD01_PDM_INF PD01
						ON PD01.DF_SPE_ACC_ID = ACCOUNTNUMBER
					JOIN OLWHRM1.GA01_APP GA01
						ON PD01.DF_PRS_ID = GA01.DF_PRS_ID_BR
					INNER JOIN (
								SELECT
									AF_APL_ID, MAX(LD_TRX_EFF) AS MAXDATE
								FROM
									OLWHRM1.DC11_LON_FAT
								GROUP BY
									AF_APL_ID) B
						ON B.AF_APL_ID = GA01.AF_APL_ID
					INNER JOIN (
								SELECT
									AF_APL_ID, LD_TRX_EFF, LC_TRX_TYP
								FROM
									OLWHRM1.DC11_LON_FAT) C
						ON B.AF_APL_ID = C.AF_APL_ID
						AND B.MAXDATE = C.LD_TRX_EFF
						

;

	/*%PUT  SQLXRC= >>> &SQLXRC <<< ||| SQLXMSG= >>> &SQLXMSG >>> ;  ** INCLUDES ERROR MESSAGES TO SAS LOG  ;*/
	/*%SQLCHECK;*/
QUIT;

ENDRSUBMIT;

DATA DEMO;
	SET DUSTER.DEMO;
RUN;

/*export to Excel spreadsheet*/
PROC EXPORT DATA= WORK.DEMO 
            OUTFILE = "T:\SAS\NH 23809_NEW_BOR_LIST.xlsx" 
            DBMS = EXCEL
			REPLACE;
     SHEET="A"; 
RUN;
