%LET BEGIN 	= 'XX/XX/XXXX';

%SYSLPUT BEGIN = &BEGIN;
%LET RPTLIB = T:\SAS;
LIBNAME  LEGEND  REMOTE  SERVER=LEGEND SLIBREF=WORK;
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
						BF_SSN
						,LN_SEQ
						,PC_FAT_TYP
						,PC_FAT_SUB_TYP
						,LD_FAT_APL
						,LA_FAT_CUR_PRI
						,LA_FAT_NSI
						,LC_FAT_REV_REA
					FROM
						PKUB.LNXX_FIN_ATY
					WHERE
						PC_FAT_TYP = 'XX'
						AND PC_FAT_SUB_TYP IN ('XX','XX')
						AND DAYS(LD_FAT_APL) >= DAYS(&BEGIN)
						AND LC_FAT_REV_REA ^= ''
						AND LC_FAT_REV_REA IS NOT NULL

					FOR READ ONLY WITH UR
				)
	;
	DISCONNECT FROM DBX;
QUIT;
ENDRSUBMIT;

PROC EXPORT 
	DATA= LEGEND.DEMO 
	OUTFILE= "&RPTLIB\NH XXXXX.xlsx" 
	DBMS=EXCEL REPLACE;
	SHEET="Refunds"; 
RUN;