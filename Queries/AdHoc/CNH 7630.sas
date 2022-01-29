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
						RTRIM(PDXX.DM_PRS_X) || ' ' || PDXX.DM_PRS_LST AS NAME,
						PDXX.DF_PRS_ID,
						SDXX.LF_DOE_SCL_ENR_CUR
					FROM
						PKUB.SDXX_STU_SPR SDXX
						INNER JOIN PKUB.PDXX_PRS_NME PDXX
							ON PDXX.DF_PRS_ID = SDXX.LF_STU_SSN
						JOIN PKUB.LNXX_LON LNXX
							ON PDXX.DF_PRS_ID = LNXX.BF_SSN
					WHERE
						SDXX.LC_STA_STUXX = 'A'
						and SDXX.LC_REA_SCL_SPR = 'XX'
						and SDXX.LD_SCL_SPR >= 'X/XX/XXXX'
						and SDXX.LF_DOE_SCL_ENR_CUR IN ('XXXXXXXX','XXXXXXXX','XXXXXXXX','XXXXXXXX','XXXXXXXX','XXXXXXXX','XXXXXXXX','XXXXXXXX','XXXXXXXX','XXXXXXXX','XXXXXXXX','XXXXXXXX','XXXXXXXX','XXXXXXXX','XXXXXXXX','XXXXXXXX','XXXXXXXX','XXXXXXXX','XXXXXXXX','XXXXXXXX','XXXXXXXX','XXXXXXXX','XXXXXXXX','XXXXXXXX','XXXXXXXX','XXXXXXXX','XXXXXXXX','XXXXXXXX')
						AND LNXX.LA_CUR_PRI > X

			FOR READ ONLY WITH UR
				)
	;

	DISCONNECT FROM DBX;

	/*%PUT  SQLXRC= >>> &SQLXRC <<< ||| SQLXMSG= >>> &SQLXMSG >>> ;  ** INCLUDES ERROR MESSAGES TO SAS LOG  ;*/
	/*%SQLCHECK;*/
QUIT;

ENDRSUBMIT;

DATA DEMO; SET LEGEND.DEMO; RUN;

/*write to comma delimited file*/
DATA _NULL_;
	SET		WORK.DEMO;
	FILE
		'T:\SAS\NH XXXX.txt'
		DELIMITER = ','
		DSD
		DROPOVER
		LRECL = XXXXX
	;

/*	FORMAT*/
/*		DF_SPE_ACC_ID $XX.*/
/*		LD_LON_GTR YYMMDDXX.*/
/*	;*/

	/* write column names, remove this to create a file without a header row */
	IF _N_ = X THEN
		DO;
			PUT	
				'Borrower Name'
				','
				'SSN'
				','
				'OPE ID'
			;
		END;

	/* write data*/	
	DO;
		PUT NAME $ @;
		PUT DF_PRS_ID $ @;
		PUT LF_DOE_SCL_ENR_CUR $;
		;
	END;
RUN;


/*/*export to Excel spreadsheet*/*/
/*PROC EXPORT DATA = WORK.DEMO */
/*            OUTFILE = "T:\NH XXXX.xlsx" */
/*            DBMS = EXCEL*/
/*			REPLACE;*/
/*     SHEET="SheetX"; */
/*RUN;*/

