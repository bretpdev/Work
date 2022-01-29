*import data set from spreadsheet;
LIBNAME XL 'T:\LNXX June Loan Split.xlsx';

DATA WORK.SOURCE (DROP=FX);
	LENGTH TAB $ X.;
	SET XL.'To MOHELA XXX$'N (IN=A)
		XL.'To ESA XXX$'N (IN=B)
		XL.'To Granite State XXX$'N (IN=C)
		XL.'To OSLA XXX$'N (IN=D)
		XL.'To PHEAA XXX$'N (IN=E)
	;
	IF A=X THEN TAB='MOHELA';
	IF B=X THEN TAB='ESA';
	IF C=X THEN TAB='GRANITE';
	IF D=X THEN TAB='OSLA';
	IF E=X THEN TAB='PHEAA';
RUN;

LIBNAME XL CLEAR;

%LET RPTLIB = T:\SAS;
LIBNAME  LEGEND  REMOTE  SERVER=LEGEND SLIBREF=WORK;
DATA LEGEND.SOURCE; *Send data to legend;
	SET SOURCE;
RUN;

RSUBMIT LEGEND;
LIBNAME PKUB DBX DATABASE=DNFPUTDL OWNER=PKUB;
LIBNAME PKUS DBX DATABASE=DNFPUTDL OWNER=PKUS;

PROC SQL;
	CREATE TABLE LoanDetail AS
		SELECT DISTINCT
			S.TAB
			,S.BF_SSN
			,S.LN_SEQ
			,S.LD_LON_GTR
			,S.IC_LON_PGM
			,S.LC_FED_PGM_YR
			,PDXX.DM_PRS_X AS First_Name
			,PDXX.DM_PRS_LST AS Last_Name
			,FSXX.LF_FED_AWD
			,FSXX.LN_FED_AWD_SEQ
			,S.Deal_IDs
		FROM
			SOURCE S
			LEFT JOIN PKUB.PDXX_PRS_NME PDXX
				ON S.BF_SSN = PDXX.DF_PRS_ID
			LEFT JOIN PKUS.FSXX_DL_LON FSXX
				ON S.BF_SSN = FSXX.BF_SSN
				AND S.LN_SEQ = FSXX.LN_SEQ
		;
QUIT;
ENDRSUBMIT;

DATA LoanDetail;
	SET LEGEND.LoanDetail;
RUN;

*direct output to separate data sets;
DATA MOHELA 
	 ESA
	 GRANITE 
	 OSLA 
	 PHEAA
	;
	SET LoanDetail;
	IF TAB='MOHELA' THEN OUTPUT MOHELA;
	IF TAB='ESA' THEN OUTPUT ESA;
	IF TAB='GRANITE' THEN OUTPUT GRANITE;
	IF TAB='OSLA' THEN OUTPUT OSLA;
	IF TAB='PHEAA' THEN OUTPUT PHEAA;
	DROP TAB;
RUN;

*export to different tabs within same workbook;
PROC EXPORT
		DATA=MOHELA
		OUTFILE="&RPTLIB\CNH XXXXX.xlsx"
		DBMS = EXCEL
		REPLACE;
		SHEET="To MOHELA XXX";
RUN;
PROC EXPORT
		DATA=ESA
		OUTFILE="&RPTLIB\CNH XXXXX.xlsx"
		DBMS = EXCEL
		REPLACE;
		SHEET="To ESA XXX";
RUN;
PROC EXPORT
		DATA=GRANITE
		OUTFILE="&RPTLIB\CNH XXXXX.xlsx"
		DBMS = EXCEL
		REPLACE;
		SHEET="To Granite State XXX";
RUN;
PROC EXPORT
		DATA=OSLA
		OUTFILE="&RPTLIB\CNH XXXXX.xlsx"
		DBMS = EXCEL
		REPLACE;
		SHEET="To OSLA XXX";
RUN;
PROC EXPORT
		DATA=PHEAA
		OUTFILE="&RPTLIB\CNH XXXXX.xlsx"
		DBMS = EXCEL
		REPLACE;
		SHEET="To PHEAA XXX";
RUN;