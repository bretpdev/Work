DATA LNS;
	INFILE 'T:\SAS\NHXXXX.CSV' DLM=',';
	INPUT BF_SSN $X. LN_SEQ;
RUN;


LIBNAME  LEGEND  REMOTE  SERVER=LEGEND SLIBREF=work  ;

DATA LEGEND.LNS;
	SET LNS;
RUN;

RSUBMIT LEGEND;
%let DB = DNFPUTDL;  *This is live;

LIBNAME PKUB DBX DATABASE=&DB OWNER=PKUB;

PROC SQL;
	CONNECT TO DBX (DATABASE=&DB);

	CREATE TABLE FSXX AS
		SELECT	
			FSXX.*
		FROM
			CONNECTION TO DBX 
				(
					SELECT	
						*
					FROM	PKUB.FSXX_DL_LON

					FOR READ ONLY WITH UR
				) FSXX
			INNER JOIN LNS L
				ON FSXX.BF_SSN = L.BF_SSN
				AND FSXX.LN_SEQ = L.LN_SEQ
	;

	DISCONNECT FROM DBX;

QUIT;

ENDRSUBMIT;

PROC EXPORT DATA= LEGEND.FSXX
            OUTFILE= "T:\SAS\FSXX DUMP.xls" 
            DBMS=EXCEL REPLACE;
RUN;