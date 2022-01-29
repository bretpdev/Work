PROC IMPORT OUT= WORK.DATA
            DATAFILE= "T:\CNH XXXXX_DATA.xlsx" 
            DBMS=EXCEL REPLACE;
     RANGE="SheetX$"; 
     GETNAMES=YES;
     MIXED=NO;
     SCANTEXT=YES;
     USEDATE=YES;
     SCANTIME=YES;
RUN;


LIBNAME  LEGEND  REMOTE  SERVER=LEGEND SLIBREF=work  ;
DATA LEGEND.DATA; *Send data to Duster;
SET DATA;
RUN;

RSUBMIT LEGEND;
/*%let DB = DNFPRQUT;  *This is test;*/
/*%let DB = DNFPRUUT;  *This is VUKX test;*/
%let DB = DNFPUTDL;  *This is live;

LIBNAME PKUB DBX DATABASE=&DB OWNER=PKUB;


PROC SQL;
CREATE TABLE TEST AS 
	SELECT
		*
	FROM
		DATA D
		INNER JOIN PKUB.LNXX_LON_CRB_RPT LNXX
			ON D.BF_SSN = LNXX.BF_SSN
			AND D.LN_SEQ = LNXX.LN_SEQ
			AND LNXX.LD_RPT_CRB BETWEEN INPUT('XX/XX/XXXX',MMDDYYXX.) AND INPUT('XX/XX/XXXX',MMDDYYXX.) 
;
QUIT;


ENDRSUBMIT;


DATA TEST;
SET LEGEND.TEST;
RUN;

PROC EXPORT 
	DATA= WORK.TEST
	OUTFILE= "T:\CNH XXXXX CREDIT REPORTING.XLSX" 
	DBMS=XLSX /*this is the correct DBMS for EXCEL XXXX */
	REPLACE /*comment out or delete this line and change the name of the sheet below if you want to add the output to a new tab in an existing spreadsheet*/
	; /*NOTE everything up to this semi-colon is actually one command, it has just been broken up on separate lines for readability*/
	SHEET="SHEETX"; 
RUN;
