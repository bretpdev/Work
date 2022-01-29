PROC IMPORT OUT= WORK.OCT_DATA
            DATAFILE= "T:/Oct.xlsx" 
            DBMS=EXCEL REPLACE;
     RANGE="Oct XX corX$"; 
     GETNAMES=YES;
     MIXED=NO;
     SCANTEXT=YES;
     USEDATE=YES;
     SCANTIME=YES;
RUN;

DATA SAUCE;
	SET OCT_DATA;
	KEEP SSN BF_SSN;
	FORMAT BF_SSN zX.;
	BF_SSN = SSN;
RUN;

/*%LET RPTLIB = %SYSGET(reportdir);*/
%LET RPTLIB = T:\SAS;
FILENAME REPORTZ "&RPTLIB/UNWSXX.NWSXXRZ";
FILENAME REPORTX "&RPTLIB/UNWSXX.NWSXXRX";

LIBNAME  LEGEND  REMOTE  SERVER=LEGEND SLIBREF=work  ;

DATA LEGEND.SOURCE;
	SET SAUCE;
	BF_SSNX = PUT(BF_SSN, $ZX.);
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

	CREATE TABLE DEMO AS
		SELECT DISTINCT
			S.*
			,MAX(FBR.TOT_FBR) AS FBR_USED
		FROM
			SOURCE S
			JOIN PKUB.DWXX_DW_CLC_CLU DWXX
				ON S.BF_SSNX = DWXX.BF_SSN
				AND DWXX.WC_DW_LON_STA = 'XX'
			LEFT JOIN PKUB.AYXX_BR_LON_ATY AYXX
				ON S.BF_SSNX = AYXX.BF_SSN
				AND AYXX.PF_REQ_ACT = 'SPHAN'
			JOIN PKUB.LNXX_LON_RPS LNXX
				ON S.BF_SSNX = LNXX.BF_SSN
				AND DWXX.LN_SEQ = LNXX.LN_SEQ
				AND LNXX.LC_TYP_SCH_DIS NOT IN ('CA','CP','CQ','CX','CX','CX','IA','IB','IL','IP','IX','IX','FS')
			LEFT JOIN PKUB.BRXX_BR_EFT BRXX
				ON S.BF_SSNX = BRXX.BF_SSN
				AND BRXX.BC_EFT_STA = 'A'
			LEFT JOIN (
					SELECT
						FBR.*
						,SUM(INTCK('DAY',FBR.LD_BEG,FBR.LD_END)) AS TOT_FBR
					FROM
						PKUB.LNXX_BR_FOR_APV LNXX
						JOIN (
							SELECT DISTINCT 
								LNXX.BF_SSN
								,LNXX.LN_SEQ
								,LNXX.LF_FOR_CTL_NUM 	AS CTL_NUM
								,LNXX.LN_FOR_OCC_SEQ 	AS OCC_SEQ
								,FBXX.LC_FOR_TYP 		AS TYP
								,'F' 					AS DEF_FOR
								,CASE
									WHEN LNXX.LD_FOR_BEG <= TODAY() THEN LD_FOR_BEG
									ELSE TODAY()
								 END AS LD_BEG
								,CASE
									WHEN LNXX.LD_FOR_END <= TODAY() THEN LNXX.LD_FOR_END
									ELSE TODAY()
								 END AS LD_END
							FROM PKUB.FBXX_BR_FOR_REQ FBXX
							INNER JOIN PKUB.LNXX_BR_FOR_APV LNXX  
								ON FBXX.BF_SSN = LNXX.BF_SSN
								AND FBXX.LF_FOR_CTL_NUM = LNXX.LF_FOR_CTL_NUM 
							WHERE FBXX.LC_FOR_STA = 'A'
								AND FBXX.LC_STA_FORXX = 'A'
								AND LNXX.LC_STA_LONXX = 'A'
								) FBR
							ON LNXX.BF_SSN = FBR.BF_SSN
							AND LNXX.LN_SEQ = FBR.LN_SEQ
							AND LNXX.LF_FOR_CTL_NUM = FBR.CTL_NUM
					GROUP BY
						FBR.BF_SSN
						,FBR.LN_SEQ
						) FBR
				ON S.BF_SSNX = FBR.BF_SSN
			WHERE 
				AYXX.BF_SSN IS NULL
				AND
				BRXX.BF_SSN IS NULL
			GROUP BY S.BF_SSNX
	;

	DISCONNECT FROM DBX;

	/*%PUT  SQLXRC= >>> &SQLXRC <<< ||| SQLXMSG= >>> &SQLXMSG >>> ;  ** INCLUDES ERROR MESSAGES TO SAS LOG  ;*/
	/*%SQLCHECK;*/
QUIT;

ENDRSUBMIT;

DATA DEMO; SET LEGEND.DEMO; RUN;

DATA MNTHS;
	SET DEMO;
	MONTHS_FBR_USED = (COALESCE(FBR_USED,X)/XXX) * XX;
	KEEP SSN MONTHS_FBR_USED;
RUN;

PROC SQL;
	CREATE TABLE OUTPT AS
		SELECT
			OD.*
			,MONTHS_FBR_USED
		FROM OCT_DATA OD
			JOIN MNTHS D
				ON OD.SSN = D.SSN
;
QUIT;

DATA UNDER_XX OVER_XX;
	SET OUTPT;
	IF MONTHS_FBR_USED < XX THEN OUTPUT UNDER_XX;
	IF MONTHS_FBR_USED GE XX THEN OUTPUT OVER_XX;
RUN;

/*export to Excel spreadsheet*/
PROC EXPORT DATA = WORK.UNDER_XX 
            OUTFILE = "T:\SAS\NH XXXX.xlsx" 
            DBMS = EXCEL
			REPLACE;
     SHEET="UNDER_XX"; 
RUN;

PROC EXPORT DATA = WORK.OVER_XX 
            OUTFILE = "T:\SAS\NH XXXX.xlsx" 
            DBMS = EXCEL
			REPLACE;
     SHEET="OVER_XX"; 
RUN;
