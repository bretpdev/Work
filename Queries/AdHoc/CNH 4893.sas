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

%MACRO DTE (DTE,TITLEA,TITLEP,TITLEB,TITLEAX,TITLEPX,TITLEBX,TITLE_ALL);
PROC SQL ;
	CONNECT TO DBX (DATABASE=&DB);

	CREATE TABLE &TITLEA AS
		SELECT	
			*
		FROM	
			CONNECTION TO DBX 
				(
					SELECT DISTINCT
						PDXX.DF_SPE_ACC_ID
					FROM
						PKUB.PDXX_PRS_NME PDXX
						LEFT JOIN PKUB.PDXX_PRS_ADR PDXX
							ON PDXX.DF_PRS_ID = PDXX.DF_PRS_ID
							AND PDXX.DI_VLD_ADR = 'N'
							AND DAYS(PDXX.DD_VER_ADR) BETWEEN (DAYS(&DTE) - X) AND (DAYS(&DTE) - X) 
						LEFT JOIN PKUB.PDXX_PRS_INA PDXX
							ON PDXX.DF_PRS_ID = PDXX.DF_PRS_ID
							AND PDXX.DI_VLD_ADR_HST = 'N'
							AND DAYS(PDXX.DD_VER_ADR_HST) BETWEEN (DAYS(&DTE) - X) AND (DAYS(&DTE) - X) 
					WHERE
						(PDXX.DF_PRS_ID IS NOT NULL OR PDXX.DF_PRS_ID IS NOT NULL)

					FOR READ ONLY WITH UR
				)
	;
	
	CREATE TABLE &TITLEP AS
		SELECT
			*
		FROM 
			CONNECTION TO DBX
				(
					SELECT DISTINCT
						PDXX.DF_SPE_ACC_ID
					FROM 
						PKUB.PDXX_PRS_NME PDXX	
						LEFT JOIN PKUB.PDXX_PRS_PHN PDXX
							ON PDXX.DF_PRS_ID = PDXX.DF_PRS_ID
							AND PDXX.DI_PHN_VLD = 'N'
							AND DAYS(PDXX.DD_PHN_VER) BETWEEN (DAYS(&DTE) - X) AND (DAYS(&DTE) - X) 
						LEFT JOIN PKUB.PDXX_PHN_HST PDXX
							ON PDXX.DF_PRS_ID = PDXX.DF_PRS_ID
							AND PDXX.DI_PHN_VLD_HST = 'N'
							AND DAYS(PDXX.DD_PHN_VER_HST) BETWEEN (DAYS(&DTE) - X) AND (DAYS(&DTE) - X) 
					WHERE 
						(PDXX.DF_PRS_ID IS NOT NULL OR PDXX.DF_PRS_ID IS NOT NULL)
				)
;

	CREATE TABLE &TITLEB AS
					SELECT DISTINCT
						A.DF_SPE_ACC_ID AS BTH
					FROM
						&TITLEA A
						INNER JOIN &TITLEP B
							ON A.DF_SPE_ACC_ID = B.DF_SPE_ACC_ID
;

	CREATE TABLE &TITLEAX AS
		SELECT DISTINCT
			COUNT(A.DF_SPE_ACC_ID) AS CNT_ADR
		FROM
			&TITLEA A
				LEFT JOIN &TITLEB B
					ON A.DF_SPE_ACC_ID = B.BTH
		WHERE 
			B.BTH IS NULL
;

	CREATE TABLE &TITLEPX AS
		SELECT DISTINCT
			COUNT(A.DF_SPE_ACC_ID) AS CNT_PHN
		FROM
			&TITLEP A
				LEFT JOIN &TITLEB B
					ON A.DF_SPE_ACC_ID = B.BTH
		WHERE 
			B.BTH IS NULL
;

	CREATE TABLE &TITLEBX AS
		SELECT
			COUNT(A.BTH) AS CNT_BTH
		FROM &TITLEB A
;

	DISCONNECT FROM DBX;

	/*%PUT  SQLXRC= >>> &SQLXRC <<< ||| SQLXMSG= >>> &SQLXMSG >>> ;  ** INCLUDES ERROR MESSAGES TO SAS LOG  ;*/
	/*%SQLCHECK;*/
QUIT;

DATA &TITLE_ALL;
	SET &TITLEAX &TITLEPX &TITLEBX;
RUN;

%MEND DTE;
%DTE ('XX/X/XXXX',OCTXA,OCTXP,OCTXB,OCTXAX,OCTXPX,OCTXBX,OCTXALL);
%DTE ('XX/XX/XXXX',OCTXXA,OCTXXP,OCTXXB,OCTXXAX,OCTXXPX,OCTXXBX,OCTXXALL);
%DTE ('XX/XX/XXXX',OCTXXA,OCTXXP,OCTXXB,OCTXXAX,OCTXXPX,OCTXXBX,OCTXXALL);
%DTE ('XX/XX/XXXX',OCTXXA,OCTXXP,OCTXXB,OCTXXAX,OCTXXPX,OCTXXBX,OCTXXALL);
%DTE ('XX/X/XXXX',NOVXA,NOVXP,NOVXB,NOVXAX,NOVXPX,NOVXBX,NOVXALL);
%DTE ('XX/X/XXXX',NOVXA,NOVXP,NOVXB,NOVXAX,NOVXPX,NOVXBX,NOVXALL);
%DTE ('XX/XX/XXXX',NOVXXA,NOVXXP,NOVXXB,NOVXXAX,NOVXXPX,NOVXXBX,NOVXXALL);
%DTE ('XX/XX/XXXX',NOVXXA,NOVXXP,NOVXXB,NOVXXAX,NOVXXPX,NOVXXBX,NOVXXALL);
%DTE ('XX/XX/XXXX',NOVXXA,NOVXXP,NOVXXB,NOVXXAX,NOVXXPX,NOVXXBX,NOVXXALL);
%DTE ('XX/X/XXXX',DECXA,DECXP,DECXB,DECXAX,DECXPX,DECXBX,DECXALL);
%DTE ('XX/XX/XXXX',DECXXA,DECXXP,DECXXB,DECXXAX,DECXXPX,DECXXBX,DECXXALL);
%DTE ('XX/XX/XXXX',DECXXA,DECXXP,DECXXB,DECXXAX,DECXXPX,DECXXBX,DECXXALL);
%DTE ('XX/XX/XXXX',DECXXA,DECXXP,DECXXB,DECXXAX,DECXXPX,DECXXBX,DECXXALL);
%DTE ('X/X/XXXX',JANXA,JANXP,JANXB,JANXAX,JANXPX,JANXBX,JANXALL);
%DTE ('X/XX/XXXX',JANXXA,JANXXP,JANXXB,JANXXAX,JANXXPX,JANXXBX,JANXXALL);
%DTE ('X/XX/XXXX',JANXXA,JANXXP,JANXXB,JANXXAX,JANXXPX,JANXXBX,JANXXALL);
%DTE ('X/XX/XXXX',JANXXA,JANXXP,JANXXB,JANXXAX,JANXXPX,JANXXBX,JANXXALL);
%DTE ('X/XX/XXXX',JANXXA,JANXXP,JANXXB,JANXXAX,JANXXPX,JANXXBX,JANXXALL);
%DTE ('X/X/XXXX',FEBXA,FEBXP,FEBXB,FEBXAX,FEBXPX,FEBXBX,FEBXALL);
%DTE ('X/XX/XXXX',FEBXXA,FEBXXP,FEBXXB,FEBXXAX,FEBXXPX,FEBXXBX,FEBXXALL);
%DTE ('X/XX/XXXX',FEBXXA,FEBXXP,FEBXXB,FEBXXAX,FEBXXPX,FEBXXBX,FEBXXALL);
%DTE ('X/XX/XXXX',FEBXXA,FEBXXP,FEBXXB,FEBXXAX,FEBXXPX,FEBXXBX,FEBXXALL);
%DTE ('X/X/XXXX',MARXA,MARXP,MARXB,MARXAX,MARXPX,MARXBX,MARXALL);
%DTE ('X/XX/XXXX',MARXXA,MARXXP,MARXXB,MARXXAX,MARXXPX,MARXXBX,MARXXALL);
%DTE ('X/XX/XXXX',MARXXA,MARXXP,MARXXB,MARXXAX,MARXXPX,MARXXBX,MARXXALL);
%DTE ('X/XX/XXXX',MARXXA,MARXXP,MARXXB,MARXXAX,MARXXPX,MARXXBX,MARXXALL);

ENDRSUBMIT;

%MACRO PULL_DOWN(TITLE);
DATA &TITLE; SET LEGEND.&TITLE; RUN;

/*export to Excel spreadsheet*/
PROC EXPORT DATA = WORK.&TITLE
            OUTFILE = "T:\SAS\&TITLE &SYSDATE..xlsx" 
            DBMS = EXCEL
			REPLACE;
     SHEET="A"; 
RUN;

%MEND PULL_DOWN;
%PULL_DOWN(OCTXALL);
%PULL_DOWN(OCTXXALL);
%PULL_DOWN(OCTXXALL);
%PULL_DOWN(OCTXXALL);
%PULL_DOWN(NOVXALL);
%PULL_DOWN(NOVXALL);
%PULL_DOWN(NOVXXALL);
%PULL_DOWN(NOVXXALL);
%PULL_DOWN(NOVXXALL);
%PULL_DOWN(DECXALL);
%PULL_DOWN(DECXXALL);
%PULL_DOWN(DECXXALL);
%PULL_DOWN(DECXXALL);
%PULL_DOWN(JANXALL);
%PULL_DOWN(JANXXALL);
%PULL_DOWN(JANXXALL);
%PULL_DOWN(JANXXALL);
%PULL_DOWN(JANXXALL);
%PULL_DOWN(FEBXALL);
%PULL_DOWN(FEBXXALL);
%PULL_DOWN(FEBXXALL);
%PULL_DOWN(FEBXXALL);
%PULL_DOWN(MARXALL);
%PULL_DOWN(MARXXALL);
%PULL_DOWN(MARXXALL);
%PULL_DOWN(MARXXALL);
