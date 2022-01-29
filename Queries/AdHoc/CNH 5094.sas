/*%LET RPTLIB = %SYSGET(reportdir);*/
%LET RPTLIB = T:\SAS;
FILENAME REPORTZ "&RPTLIB/UNWSXX.NWSXXRZ";
FILENAME REPORTX "&RPTLIB/UNWSXX.NWSXXRX";

%LET BEGIN 	= 'XX/XX/XXXX';
%SYSLPUT BEGIN = &begin;


LIBNAME  LEGEND  REMOTE  SERVER=LEGEND SLIBREF=work  ;
RSUBMIT LEGEND;
/*%let DB = DNFPRQUT;  *This is test;*/
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
CONNECT TO DBX (DATABASE=&DB);
CREATE TABLE DEMO AS
	SELECT	*
	FROM	CONNECTION TO DBX (
				SELECT	DISTINCT
						PDXX.DF_SPE_ACC_ID
						,TRIM(PDXX.DM_PRS_X) || ' ' || TRIM(PDXX.DM_PRS_LST) AS NAME
						,LNXX.LD_FAT_PST
						,LNXX.LD_FAT_EFF
						,ADXX.LC_WOF_WUP_REA
						,LKXX.PX_DSC_XXL_XC
						,LNXX.LA_FAT_CUR_PRI + LNXX.LA_FAT_NSI AS WRITE_OFF_AMOUNT
				FROM	PKUB.PDXX_PRS_NME PDXX
						INNER JOIN PKUB.LNXX_FIN_ATY LNXX
							ON PDXX.DF_PRS_ID = LNXX.BF_SSN
						INNER JOIN PKUB.ADXX_PCV_ATY_ADJ ADXX
							ON PDXX.DF_PRS_ID = ADXX.BF_SSN
						INNER JOIN PKUB.LKXX_LS_CDE_LKP LKXX
							ON ADXX.LC_WOF_WUP_REA = LKXX.PX_ATR_VAL
							AND LKXX.PM_ATR = 'LC-WOF-WUP-REA'
				WHERE	LNXX.PC_FAT_TYP = 'XX'
						AND LNXX.PC_FAT_SUB_TYP = 'XX'
						AND LNXX.LD_FAT_PST >= &BEGIN
						
				FOR READ ONLY WITH UR
			);

DISCONNECT FROM DBX;

/*%PUT  SQLXRC= >>> &SQLXRC <<< ||| SQLXMSG= >>> &SQLXMSG >>> ;  ** INCLUDES ERROR MESSAGES TO SAS LOG  ;*/
/*%SQLCHECK;*/
QUIT;

ENDRSUBMIT;
DATA DEMO; SET LEGEND.DEMO; RUN;

PROC EXPORT DATA= WORK.DEMO 
            OUTFILE= "T:\SAS\NHCS XXXX.xlsx" 
            DBMS=EXCEL REPLACE;
RUN;
