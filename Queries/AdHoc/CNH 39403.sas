PROC IMPORT OUT= WORK.Data
            DATAFILE= "T:\cnh XXXXX input.xlsx" 
            DBMS=EXCEL REPLACE;
     RANGE="SheetX$"; 
     GETNAMES=YES;
     MIXED=NO;
     SCANTEXT=YES;
     USEDATE=YES;
     SCANTIME=YES;
RUN;

LIBNAME  LEGEND  REMOTE  SERVER=LEGEND SLIBREF=WORK;
DATA LEGEND.DATA; *Send data to Duster;
SET DATA;
RUN;


RSUBMIT LEGEND;
LIBNAME PKUB DBX DATABASE=DNFPUTDL OWNER=PKUB;
PROC SQL;
	CREATE TABLE FINAL AS
	SELECT 
		LNXX.* 
	FROM 
		PKUB.LNXX_LON_CRB_RPT LNXX
		INNER JOIN DATA D
			ON D.SSN = LNXX.BF_SSN
	WHERE
		LNXX.LD_RPT_CRB >= D.CASEDATE
		AND LNXX.LC_RPT_STA_CRB IN ('XX','XX','XX','XX','XX')
;
QUIT;
ENDRSUBMIT;

PROC EXPORT 
	DATA= LEGEND.FINAL
	OUTFILE= "T:\CNH XXXXX.xlsx" 
	DBMS=XLSX /*this is the correct DBMS for EXCEL XXXX */
	REPLACE /*comment out or delete this line and change the name of the sheet below if you want to add the output to a new tab in an existing spreadsheet*/
	; /*NOTE everything up to this semi-colon is actually one command, it has just been broken up on separate lines for readability*/
	SHEET="SheetX"; 
RUN;
