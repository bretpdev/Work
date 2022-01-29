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

	CREATE TABLE DEMO AS
		SELECT	
			*
		FROM	
			CONNECTION TO DBX 
				(
					SELECT DISTINCT
						LNXX.BF_SSN
						,CASE
							WHEN AYXX.PF_REQ_ACT = 'PXXXA' THEN 'OUTBOUND'
							WHEN AYXX.PF_REQ_ACT = 'PXXXC' THEN 'INBOUND'
							ELSE '        '
						END AS CALL_TYPE 
						,AYXX.LD_STA_ACTYXX
					FROM
						PKUB.LNXX_LON LNXX
						LEFT JOIN PKUB.AYXX_BR_LON_ATY AYXX
							ON LNXX.BF_SSN = AYXX.BF_SSN
							AND AYXX.LD_STA_ACTYXX >= 'XX/XX/XXXX'
							AND AYXX.PF_REQ_ACT IN ('PXXXC','PXXXA')
							AND AYXX.PF_RSP_ACT = 'CNTCT'
/*						INNER JOIN*/
/*						(*/
/*							SELECT DISTINCT*/
/*								BF_SSN*/
/*							FROM*/
/*								PKUB.AYXX_BR_LON_ATY*/
/*							WHERE*/
/*								PF_RSP_ACT = 'CNTCT'*/
/*								AND PF_REQ_ACT IN ('PXXXC','PXXXA')*/
/*						) AYXX_C*/
/*							ON AYXX_C.BF_SSN = LNXX.BF_SSN*/
					WHERE
						LNXX.LA_CUR_PRI > X
						AND LNXX.LC_STA_LONXX = 'R'

					FOR READ ONLY WITH UR
				)
	;

CREATE TABLE TOG AS
	SELECT
		LNXX.BF_SSN
		,D.CALL_TYPE
		,D.LD_STA_ACTYXX
	FROM SOURCEX S
		INNER JOIN PKUB.FSXX_DL_LON FSXX
			ON S.LF_FED_AWD = FSXX.LF_FED_AWD
			AND S.LN_FED_AWD_SEQ = FSXX.LN_FED_AWD_SEQ
		INNER JOIN PKUB.LNXX_LON LNXX
			ON FSXX.BF_SSN = LNXX.BF_SSN
			AND FSXX.LN_SEQ = LNXX.LN_SEQ
			AND LNXX.LA_CUR_PRI > X
			AND LNXX.LC_STA_LONXX = 'R'
		LEFT JOIN DEMO D
			ON LNXX.BF_SSN = D.BF_SSN
;

	DISCONNECT FROM DBX;

	/*%PUT  SQLXRC= >>> &SQLXRC <<< ||| SQLXMSG= >>> &SQLXMSG >>> ;  ** INCLUDES ERROR MESSAGES TO SAS LOG  ;*/
	/*%SQLCHECK;*/
QUIT;

ENDRSUBMIT;

DATA DEMO; SET LEGEND.TOG; RUN;

/*export to Excel spreadsheet*/
PROC EXPORT DATA = WORK.DEMO 
            OUTFILE = "T:\SAS\NH XXXX Query X.xlsx" 
            DBMS = EXCEL
			REPLACE;
     SHEET="A"; 
RUN;
