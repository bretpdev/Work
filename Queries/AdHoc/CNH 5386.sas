PROC IMPORT OUT= WORK.SOURCE
            DATAFILE= "T:\CornerStone Awards - IDR email campaign.xlsx" 
            DBMS=EXCEL REPLACE;
     RANGE="SheetX$"; 
     GETNAMES=YES;
     MIXED=NO;
     SCANTEXT=YES;
     USEDATE=YES;
     SCANTIME=YES;
RUN;

DATA SOURCEX;	
	SET SOURCE;
	LF_FED_AWD = SUBSTR(AWARD_ID,X,XX);
	LN_FED_AWD_SEQ = INPUT(SUBSTR(AWARD_ID,XX,X), BESTXX.);
RUN;

/*%LET RPTLIB = %SYSGET(reportdir);*/
%LET RPTLIB = T:\SAS;
FILENAME REPORTZ "&RPTLIB/UNWSXX.NWSXXRZ";
FILENAME REPORTX "&RPTLIB/UNWSXX.NWSXXRX";

LIBNAME  LEGEND  REMOTE  SERVER=LEGEND SLIBREF=work  ;

DATA LEGEND.SOURCEX;
	SET SOURCEX;
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

PROC SQL;
	CONNECT TO DBX (DATABASE=&DB);

	CREATE TABLE PMTS AS
		SELECT DISTINCT	
			S.*
			,FSXX.BF_SSN
			,FSXX.LN_SEQ
			,SUM(LNXX.LA_FAT_NSI,LNXX.LA_FAT_CUR_PRI) AS TOT_PAYMENT
			,LNXX.LD_FAT_EFF
		FROM SOURCEX S
			INNER JOIN PKUB.FSXX_DL_LON FSXX
				ON S.LF_FED_AWD = FSXX.LF_FED_AWD
				AND S.LN_FED_AWD_SEQ = FSXX.LN_FED_AWD_SEQ
			LEFT JOIN PKUB.LNXX_FIN_ATY LNXX
				ON FSXX.BF_SSN = LNXX.BF_SSN
				AND FSXX.LN_SEQ = LNXX.LN_SEQ
				AND LNXX.LC_FAT_REV_REA = ' '
				AND LNXX.PC_FAT_TYP = 'XX'
				AND LNXX.PC_FAT_SUB_TYP = 'XX'
				AND LNXX.LD_FAT_EFF >= INPUT('XXNOVXXXX',DATEX.)
			
	;

	DISCONNECT FROM DBX;

	/*%PUT  SQLXRC= >>> &SQLXRC <<< ||| SQLXMSG= >>> &SQLXMSG >>> ;  ** INCLUDES ERROR MESSAGES TO SAS LOG  ;*/
	/*%SQLCHECK;*/
QUIT;

ENDRSUBMIT;

DATA PMTS; SET LEGEND.PMTS; RUN;

/*export to Excel spreadsheet*/
PROC EXPORT DATA = WORK.PMTS 
            OUTFILE = "T:\SAS\NH XXXX Query X.xlsx" 
            DBMS = EXCEL
			REPLACE;
     SHEET="A"; 
RUN;
