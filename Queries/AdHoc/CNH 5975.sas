PROC IMPORT OUT= WORK.INPOP
            DATAFILE= "T:\Nov Transfers - CS.xlsx" 
            DBMS=EXCEL REPLACE;
     GETNAMES=YES;
     MIXED=NO;
     SCANTEXT=YES;
     USEDATE=YES;
     SCANTIME=YES;
RUN;

DATA INPOP;
	SET INPOP;
	BF_SSN = STRIP(COMPRESS(BORROWER_SSN, '-'));
RUN;


LIBNAME LEGEND REMOTE SERVER=LEGEND SLIBREF=WORK;

DATA LEGEND.INPOP; SET INPOP; RUN;

RSUBMIT;
LIBNAME PKUB DBX DATABASE=DNFPUTDL OWNER=PKUB;

PROC SQL;
	CREATE TABLE IN_QUE AS
		SELECT
			I.*
		FROM
			INPOP I
			JOIN PKUB.WQXX_TSK_QUE WQXX
				ON I.BF_SSN = WQXX.BF_SSN
				AND WQXX.WF_QUE = 'XX'
				AND WQXX.WF_SUB_QUE = 'XX'
				AND WQXX.WC_STA_WQUEXX NOT IN ('C','X')
	;

	CREATE TABLE WQXX AS
		SELECT
			WQXX.*
		FROM
			PKUB.WQXX_TSK_QUE WQXX
		WHERE
			WQXX.WF_QUE = 'XX'
			AND WQXX.WF_SUB_QUE = 'XX'
			AND WQXX.WC_STA_WQUEXX NOT IN ('C','X')
	;
QUIT;

ENDRSUBMIT;

PROC EXPORT
		DATA=LEGEND.IN_QUE
		OUTFILE='T:\NH XXXX XXXX QUEUE.XLSX'
		REPLACE;
RUN;
		
PROC EXPORT
		DATA=LEGEND.WQXX
		OUTFILE='T:\NH XXXX XXXX QUEUE.XLSX'
		REPLACE;
RUN;
