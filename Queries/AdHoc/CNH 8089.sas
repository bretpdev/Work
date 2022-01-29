/*%LET RPTLIB = %SYSGET(reportdir);*/
%LET RPTLIB = T:\SAS;
FILENAME REPORTZ "&RPTLIB/UNWSXX.NWSXXRZ";
FILENAME REPORTX "&RPTLIB/UNWSXX.NWSXXRX";

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
		SELECT	
			*
		FROM	
			CONNECTION TO DBX 
				(
					SELECT	
						DISTINCT
						PDXX.DF_SPE_ACC_ID
						,PDXX.DM_PRS_X||' '||PDXX.DM_PRS_MID||' '||PDXX.DM_PRS_LST AS NAME
						,FSXX.LF_FED_AWD||FSXX.LN_FED_AWD_SEQ AS AWARD_ID
						,LNXX.LD_FAT_APL
						,LNXX.LD_FAT_EFF
						,LNXX.PC_FAT_SUB_TYP
						,coalesce(LNXX.LA_FAT_CUR_PRI,X) as LA_FAT_CUR_PRI
						,coalesce(LNXX.LA_FAT_NSI, X) as LA_FAT_NSI
						
					FROM
						PKUB.PDXX_PRS_NME PDXX
						INNER JOIN PKUB.LNXX_FIN_ATY LNXX
							ON PDXX.DF_PRS_ID = LNXX.BF_SSN
						INNER JOIN PKUB.FSXX_DL_LON FSXX
							ON LNXX.BF_SSN = FSXX.BF_SSN
							AND LNXX.LN_SEQ = FSXX.LN_SEQ
					WHERE
						(LNXX.PC_FAT_TYP = 'XX'	AND (LNXX.PC_FAT_SUB_TYP = 'XX'  OR LNXX.PC_FAT_SUB_TYP = 'XX'))
						AND DAYS(LNXX.LD_FAT_APL) >= DAYS('XXXX-XX-XX')
	

					FOR READ ONLY WITH UR
				)
	;
	DISCONNECT FROM DBX;
QUIT;

ENDRSUBMIT;
DATA DEMO; SET LEGEND.DEMO; RUN;


PROC EXPORT DATA = WORK.DEMO 
            OUTFILE = "T:\SSAE XX Adjustment Query_VX.xlsx"
            DBMS = EXCEL
			REPLACE;
     SHEET="SheetX"; 
RUN;
