PROC IMPORT OUT = WORK.SOURCE
            DATAFILE = "Y:\Batch\FTP\Jeremy\CornerStone\June XXXX\DMCS SSNs.xlsx" 
            DBMS = xlsx REPLACE;
   			SHEET = 'SheetX'; 
RUN;

LIBNAME  LEGEND  REMOTE  SERVER=LEGEND SLIBREF=WORK;
DATA LEGEND.SOURCE; *Send data to Duster;
SET SOURCE;
RUN;


RSUBMIT LEGEND;
LIBNAME PKUB DBX DATABASE=DNFPUTDL OWNER=PKUB;
PROC SQL;
	CREATE TABLE DEMO AS
		SELECT	
			LNXX.BF_SSN,
			LNXX.IC_LON_PGM,
			LNXX.LA_FAT_NSI,
			LNXX.LA_FAT_NSI_ACR
			
		FROM
			 SOURCE S
		INNER JOIN PKUB.LNXX_LON LNXX
			ON input(LNXX.BF_SSN, BESTXX.) = S.SSNS
		inner join PKUB.LNXX_FIN_ATY LNXX
			ON LNXX.BF_SSN = LNXX.BF_SSN
			AND LNXX.LN_SEQ = LNXX.LN_SEQ
		WHERE LNXX.PC_FAT_TYP = 'XX' AND LNXX.PC_FAT_SUB_TYP = 'XX'
			
;
QUIT;
ENDRSUBMIT;

PROC EXPORT DATA= LEGEND.DEMO 
            OUTFILE= "Y:\Batch\FTP\Jeremy\CornerStone\June XXXX\DMCS SSNs NH XXXX.xls" 
            DBMS=EXCEL REPLACE;
     SHEET="SheetX"; 
RUN;