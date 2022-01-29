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

	CREATE TABLE PreJulyXXXX AS
		SELECT	
			*
		FROM	
			CONNECTION TO DBX 
				(
					SELECT DISTINCT
						AYXX.*
					FROM
						PKUB.AYXX_BR_LON_ATY AYXX
					WHERE
						AYXX.LD_ATY_REQ_RCV BETWEEN 'XX/XX/XXXX' AND 'XX/XX/XXXX'
						AND AYXX.PF_REQ_ACT in('TLFSA','CSFSA','DEFSA','UPFSA','FCFSA','SSFSA','BDISC','TLDNY','CSDNY','DEDNY','UPDNY','FCDNY','SSDNY','BKDNY','BDISM')
						
					FOR READ ONLY WITH UR
				)
	;
	
	/*CREATE TABLE PreJulyXXXXDenials AS
		SELECT
			*
		FROM
			CONNECTION TO DBX
				(
					SELECT DISTINCT
						AYXX.*,
						CONCAT(CONCAT(AYXX.LX_ATY, COALESCE(AYXXB.LX_ATY,' ')), COALESCE(AYXXC.LX_ATY,' ')) AS Comment
					FROM 
						PKUB.AYXX_BR_LON_ATY AYXX
						INNER JOIN PKUB.AYXX_BR_LON_ATY AYXXP
							ON AYXX.BF_SSN = AYXXP.BF_SSN
							AND AYXXP.PF_REQ_ACT = 'PXXXA'
							AND AYXXP.LD_ATY_REQ_RCV >= AYXX.LD_ATY_REQ_RCV
							AND AYXXP.LD_ATY_REQ_RCV <= 'XX/X/XXXX'
						LEFT JOIN PKUB.AYXX_ATY_TXT AYXX
							ON AYXXP.BF_SSN = AYXX.BF_SSN
							AND AYXXP.LN_ATY_SEQ = AYXX.LN_ATY_SEQ
							AND AYXX.LN_ATY_TXT_SEQ = 'X'
						LEFT JOIN PKUB.AYXX_ATY_TXT AYXXB
							ON AYXXP.BF_SSN = AYXXB.BF_SSN
							AND AYXXP.LN_ATY_SEQ = AYXXB.LN_ATY_SEQ
							AND AYXXB.LN_ATY_TXT_SEQ = 'X'
						LEFT JOIN PKUB.AYXX_ATY_TXT AYXXC
							ON AYXXP.BF_SSN = AYXXC.BF_SSN
							AND AYXXP.LN_ATY_SEQ = AYXXC.LN_ATY_SEQ
							AND AYXXC.LN_ATY_TXT_SEQ = 'X'
					WHERE
						AYXX.LD_ATY_REQ_RCV BETWEEN 'XX/XX/XXXX' AND 'XX/XX/XXXX'
						AND AYXX.PF_REQ_ACT = 'DITPD'
				)
		
	;	*/
	CREATE TABLE PostJulyXXXX AS
		SELECT 
			*
		FROM
			CONNECTION TO DBX
				(
					SELECT DISTINCT 
						AYXX.*
					FROM
						PKUB.AYXX_BR_LON_ATY AYXX
					WHERE
						AYXX.LD_ATY_REQ_RCV BETWEEN 'XX/XX/XXXX' AND 'XX/XX/XXXX'
						AND AYXX.PF_REQ_ACT in('TLFSA','CSFSA','DEFSA','UPFSA','FCFSA','SSFSA','BDISC','TLDNY','CSDNY','DEDNY','UPDNY','FCDNY','SSDNY','BKDNY','BDISM')
						
					FOR READ ONLY WITH UR
				)
	;
	DISCONNECT FROM DBX;

	/*%PUT  SQLXRC= >>> &SQLXRC <<< ||| SQLXMSG= >>> &SQLXMSG >>> ;  ** INCLUDES ERROR MESSAGES TO SAS LOG  ;*/
	/*%SQLCHECK;*/
QUIT;

ENDRSUBMIT;

DATA PreJulyXXXX; SET LEGEND.PreJulyXXXX; RUN;
/*DATA PreJulyXXXXDenials; SET LEGEND.PreJulyXXXXDenials; RUN;*/
DATA PostJulyXXXX; SET LEGEND.PostJulyXXXX; RUN;



/*export to Excel spreadsheet*/
PROC EXPORT DATA = WORK.PreJulyXXXX 
            OUTFILE = "T:\NH XXXX.xlsx" 
            DBMS = EXCEL
			REPLACE;
     SHEET="PreJulyXXXX"; 
RUN;

/*export to Excel spreadsheet*/
PROC EXPORT DATA = WORK.PostJulyXXXX 
            OUTFILE = "T:\NH XXXX.xlsx" 
            DBMS = EXCEL
			REPLACE;
     SHEET="PostJulyXXXX"; 
RUN;

/*export to Excel spreadsheet*/
/*PROC EXPORT DATA = WORK.PreJulyXXXXDenials 
            OUTFILE = "T:\NH XXXX.xlsx" 
            DBMS = EXCEL
			REPLACE;
     SHEET="PreJulyXXXXDenials"; 
RUN;*/
