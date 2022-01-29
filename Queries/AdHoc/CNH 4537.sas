/*%LET RPTLIB = %SYSGET(reportdir);*/
%LET RPTLIB = T:\SAS;
FILENAME REPORTZ "&RPTLIB/UNWSXX.NWSXXRZ";
FILENAME REPORTX "&RPTLIB/UNWSXX.NWSXXRX";

LIBNAME  LEGEND  REMOTE  SERVER=LEGEND SLIBREF=work  ;
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

	CREATE TABLE DEMO AS
		SELECT	
			*
		FROM	
			CONNECTION TO DBX 
				(
					SELECT DISTINCT
						A.BF_SSN
						,CASE
							WHEN YEAR(A.LD_FAT_EFF) = XXXX THEN 'XXXX'
							WHEN YEAR(A.LD_FAT_EFF) = XXXX THEN 'XXXX'
							WHEN YEAR(A.LD_FAT_EFF) = XXXX THEN 'XXXX'
						ELSE 'XXXX' END AS YR,
						A.LD_FAT_EFF,
						SUM(A.LA_FAT_ILG_PRI) AS LA_FAT_ILG_PRI,
						SUM(A.LA_FAT_CUR_PRI) AS LA_FAT_CUR_PRI
					FROM
						PKUB.LNXX_FIN_ATY A
					WHERE
						A.PC_FAT_TYP = 'XX'
						AND A.PC_FAT_SUB_TYP = 'XX'
						AND A.LC_FAT_REV_REA = ' '
						AND A.LD_FAT_EFF > 'XX/XX/XXXX'
						AND A.LC_STA_LONXX = 'A'
					GROUP BY
						A.BF_SSN, A.LD_FAT_EFF

					FOR READ ONLY WITH UR
				)
	;

	DISCONNECT FROM DBX;

	/*%PUT  SQLXRC= >>> &SQLXRC <<< ||| SQLXMSG= >>> &SQLXMSG >>> ;  ** INCLUDES ERROR MESSAGES TO SAS LOG  ;*/
	/*%SQLCHECK;*/
QUIT;

ENDRSUBMIT;

DATA DEMO; SET LEGEND.DEMO; RUN;

DATA YRXXXX YRXXXX YRXXXX YRXXXX;
	SET DEMO;
	IF YR = 'XXXX' THEN OUTPUT YRXXXX;
	IF YR = 'XXXX' THEN OUTPUT YRXXXX;
	IF YR = 'XXXX' THEN OUTPUT YRXXXX;
	IF YR = 'XXXX' THEN OUTPUT YRXXXX;
RUN;

%MACRO WRITE (FILE,SHEET);
/*export to Excel spreadsheet*/
PROC EXPORT DATA = WORK.&FILE
            OUTFILE = "T:\SAS\XX XX Transactions.xlsx" 
            DBMS = EXCEL
			REPLACE;
     SHEET="&SHEET"; 
RUN;
%MEND WRITE;
/*%WRITE (YRXXXX,'XXXX');*/
%WRITE (YRXXXX,'YRXXXX');
%WRITE (YRXXXX,'YRXXXX');
%WRITE (YRXXXX,'YRXXXX');
