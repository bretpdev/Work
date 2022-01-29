*query based on "CNH XXXX XXXX and XXXX transactions.sas";

LIBNAME  LEGEND  REMOTE  SERVER=LEGEND SLIBREF=WORK ;

%LET START_DATE = 'XX/XX/XXXX';
%LET END_DATE = 'XX/XX/XXXX';

%SYSLPUT START_DATE = &START_DATE;
%SYSLPUT END_DATE = &END_DATE;
RSUBMIT LEGEND;
%LET DB = DNFPUTDL;
LIBNAME PKUB DBX DATABASE=&DB OWNER=PKUB;

PROC SQL;
	CONNECT TO DBX (DATABASE=&DB);

	CREATE TABLE DEMO AS
		SELECT	
			*
		FROM	
			CONNECTION TO DBX 
				(
					SELECT DISTINCT
						PDXX.DF_SPE_ACC_ID
						,PDXX.DM_PRS_X||' '||PDXX.DM_PRS_MID||' '||PDXX.DM_PRS_LST AS NAME
						,FSXX.LF_FED_AWD||FSXX.LN_FED_AWD_SEQ AS AWARD_ID
						,LNXX.LD_FAT_APL
						,LNXX.LD_FAT_EFF
						,LNXX.PC_FAT_SUB_TYP
						,LNXX.LA_FAT_CUR_PRI
						,LNXX.LA_FAT_NSI
					FROM
						PKUB.PDXX_PRS_NME PDXX
						INNER JOIN PKUB.LNXX_FIN_ATY LNXX
							ON PDXX.DF_PRS_ID = LNXX.BF_SSN
						INNER JOIN PKUB.FSXX_DL_LON FSXX
							ON LNXX.BF_SSN = FSXX.BF_SSN
							AND LNXX.LN_SEQ = FSXX.LN_SEQ
					WHERE
						LNXX.PC_FAT_TYP = 'XX'
						AND (
								LNXX.PC_FAT_SUB_TYP = 'XX' 
								OR LNXX.PC_FAT_SUB_TYP = 'XX'
							)
						AND DAYS(LNXX.LD_FAT_APL) BETWEEN DAYS(&START_DATE) AND DAYS(&END_DATE)

					FOR READ ONLY WITH UR
				)
	;

	DISCONNECT FROM DBX;
QUIT;
ENDRSUBMIT;
DATA DEMO; 
	SET LEGEND.DEMO; 
RUN;

/*export to comma delimited file*/
PROC EXPORT 
	DATA= WORK.DEMO 
	OUTFILE= "T:\SAS\SSAE XX Adjustment Query.csv" 
	DBMS=CSV REPLACE;
	PUTNAMES=YES;
RUN;