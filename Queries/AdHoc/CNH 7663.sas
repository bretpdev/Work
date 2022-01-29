PROC IMPORT OUT= WORK.POP
            DATAFILE= "T:\Item X Population Borrowers.xlsx" 
            DBMS=EXCEL REPLACE;
     RANGE="SheetX$"; 
     GETNAMES=YES;
     MIXED=NO;
     SCANTEXT=YES;
     USEDATE=YES;
     SCANTIME=YES;
RUN;

/*%LET RPTLIB = %SYSGET(reportdir);*/
%LET RPTLIB = T:\SAS;
FILENAME REPORTZ "&RPTLIB/UNWSXX.NWSXXRZ";
FILENAME REPORTX "&RPTLIB/UNWSXX.NWSXXRX";

LIBNAME  LEGEND  REMOTE  SERVER=LEGEND SLIBREF=work  ;

DATA LEGEND.POP;
	SET POP;
	SSN = PUT(BF_SSN, $ZX.);
	DROP BF_SSN;
RUN;

RSUBMIT LEGEND;
/*%let DB = DNFPRQUT;  *This is test;*/
/*%let DB = DNFPRUUT;  *This is VUKX test;*/
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

PROC SQL ;
	CONNECT TO DBX (DATABASE=&DB);

	CREATE TABLE DISC AS
		SELECT	
			POP.SSN AS BF_SSN
			,LNXX.LN_SEQ
			,LNXX_MIN.LN_RPS_SEQ
			,LNXX.LD_CRT_LONXX
			,LNXX.LA_ACR_INT_RPD
			,LNXX.LC_STA_LONXX
		FROM 
			POP
			JOIN( 
					SELECT
						POP.SSN AS BF_SSN
						,LNXX.LN_SEQ
						,MIN(LNXX.LN_RPS_SEQ) AS LN_RPS_SEQ
					FROM 
						POP
						JOIN PKUB.LNXX_LON_RPS LNXX
							ON POP.SSN = LNXX.BF_SSN
					WHERE LNXX.LD_CRT_LONXX GE 'XXJUNXXXX'd
					GROUP BY 
						POP.SSN
						,LNXX.LN_SEQ
					) LNXX_MIN
				ON POP.SSN = LNXX_MIN.BF_SSN
			JOIN 
				PKUB.LNXX_LON_RPS LNXX
				ON POP.SSN = LNXX.BF_SSN
				AND LNXX_MIN.LN_SEQ = LNXX.LN_SEQ
				AND LNXX_MIN.LN_RPS_SEQ = LNXX.LN_RPS_SEQ
			JOIN 
				PKUB.LNXX_LON LNXX
				ON POP.SSN = LNXX.BF_SSN
				AND LNXX.LN_SEQ = LNXX.LN_SEQ
/*			JOIN */
/*				PKUB.DWXX_DW_CLC_CLU DWXX*/
/*				ON POP.SSN = DWXX.BF_SSN*/
/*		WHERE LNXX.LD_CRT_LONXX < 'X/XX/XXXX'*/
	;

	CREATE TABLE NON_DISC AS
		SELECT
			POP.SSN AS BF_SSN
			,DWXX.LN_SEQ
			,DWXX.WA_TOT_BRI_OTS
			,LNXX.LC_STA_LONXX
		FROM 
			POP
			LEFT JOIN 
				DISC
				ON POP.SSN = DISC.BF_SSN
			JOIN 
				PKUB.DWXX_DW_CLC_CLU DWXX
				ON POP.SSN = DWXX.BF_SSN
			JOIN 
				PKUB.LNXX_LON LNXX
				ON POP.SSN = LNXX.BF_SSN
				AND DWXX.LN_SEQ = LNXX.LN_SEQ
		WHERE 
			DISC.BF_SSN IS NULL
	;
	
	DISCONNECT FROM DBX;

	/*%PUT  SQLXRC= >>> &SQLXRC <<< ||| SQLXMSG= >>> &SQLXMSG >>> ;  ** INCLUDES ERROR MESSAGES TO SAS LOG  ;*/
	/*%SQLCHECK;*/
QUIT;

ENDRSUBMIT;

DATA DISC; SET LEGEND.DISC; RUN;
DATA NON_DISC; SET LEGEND.NON_DISC; RUN;

/*PROC SQL;*/
/*	CREATE TABLE TOT_DSC AS*/
/*		SELECT*/
/*			COUNT(DISINCT BF_SSN) AS TOT_BOR*/
/*			,SUM(LA_ACR_INT_RPD) AS TOT_INT*/
/*		FROM */
/*			DISC */
/*	;*/
/*QUIT;*/

/*export to Excel spreadsheet*/
PROC EXPORT DATA = WORK.NON_DISC
            OUTFILE = "T:\SAS\NH XXXX TEST.xlsx" 
            DBMS = EXCEL
			REPLACE;
     SHEET="RX"; 
RUN;

/*export to Excel spreadsheet*/
PROC EXPORT DATA = WORK.DISC 
            OUTFILE = "T:\SAS\NH XXXX TEST.xlsx" 
            DBMS = EXCEL
			REPLACE;
     SHEET="RX"; 
RUN;


