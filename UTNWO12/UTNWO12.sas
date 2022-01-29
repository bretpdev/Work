/*%LET RPTLIB = %SYSGET(reportdir);*/
%LET RPTLIB = T:\SAS;
FILENAME REPORTZ "&RPTLIB/UNWO12.NWO12RZ";
FILENAME REPORT2 "&RPTLIB/UNWO12.NWO12R2";

LIBNAME  LEGEND  REMOTE  SERVER=LEGEND SLIBREF=work  ;
RSUBMIT LEGEND;
/*%let DB = DNFPRQUT;  *This is test;*/
%let DB = DNFPUTDL;  *This is live;

LIBNAME PKUB DB2 DATABASE=&DB OWNER=PKUB;

%MACRO SQLCHECK ;
  %IF  &SQLXRC NE 0  %THEN  %DO  ;
    DATA _NULL_  ;
            FILE REPORTZ NOTITLES  ;
            PUT @01 " ********************************************************************* "
              / @01 " ****  THE SQL CODE ABOVE HAS EXPERIENCED AN ERROR.               **** "
              / @01 " ****  THE SAS SHOULD BE REVIEWED.                                **** "       
              / @01 " ********************************************************************* "
              / @01 " ****  THE SQL ERROR CODE IS  &SQLXRC  AND THE SQL ERROR MESSAGE  **** "
              / @01 " ****  &SQLXMSG   **** "
              / @01 " ********************************************************************* "
            ;
         RUN  ;
  %END  ;
%MEND  ;

PROC SQL;
CONNECT TO DB2 (DATABASE=&DB);
CREATE TABLE TLFFORB_DET AS
	SELECT	*
	FROM	CONNECTION TO DB2 (
				SELECT	DISTINCT
						A.DF_SPE_ACC_ID
						,B.LN_SEQ
						,B.IC_LON_PGM
						,C.LF_FOR_CTL_NUM
				FROM	PKUB.PD10_PRS_NME A
						INNER JOIN PKUB.LN10_LON B
							ON A.DF_PRS_ID = B.BF_SSN
						INNER JOIN PKUB.FB10_BR_FOR_REQ C
							ON A.DF_PRS_ID = C.BF_SSN
						INNER JOIN PKUB.LN60_BR_FOR_APV D
							ON A.DF_PRS_ID = D.BF_SSN
							AND B.LN_SEQ = D.LN_SEQ
							AND C.LF_FOR_CTL_NUM = D.LF_FOR_CTL_NUM
				WHERE	B.LA_CUR_PRI > 0
						AND B.LC_STA_LON10 = 'R'
						AND B.IC_LON_PGM IN ('DGPLUS', 'DLPCNS', 'DLPLGB', 'DLPLUS', 'DLSCPG', 'DLSCPL', 'DPLUS', 'PLUS', 'PLUSGB')
						AND C.LC_FOR_STA = 'A'
						AND C.LC_STA_FOR10 = 'A'
						AND D.LC_STA_LON60 = 'A'
						AND C.LC_FOR_TYP = '21'
				FOR READ ONLY WITH UR
			);
DISCONNECT FROM DB2;

CREATE TABLE TLFFORB AS
	SELECT	DISTINCT DF_SPE_ACC_ID
	FROM 	TLFFORB_DET
	ORDER BY DF_SPE_ACC_ID;

/*%PUT  SQLXRC= >>> &SQLXRC <<< ||| SQLXMSG= >>> &SQLXMSG >>> ;  ** INCLUDES ERROR MESSAGES TO SAS LOG  ;*/
/*%SQLCHECK;*/
QUIT;

ENDRSUBMIT;
DATA TLFFORB_DET; SET LEGEND.TLFFORB_DET; RUN;
DATA TLFFORB; SET LEGEND.TLFFORB; RUN;

/*export to queue builder (fed) file*/
DATA _NULL_;
SET TLFFORB ;
FILE REPORT2 DELIMITER=',' DSD DROPOVER LRECL=32767;
PUT DF_SPE_ACC_ID 'TLFFP,,,,,,,ALL,' ;
RUN;
