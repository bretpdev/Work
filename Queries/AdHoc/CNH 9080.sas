%LET RPTLIB = T:\SAS;
LIBNAME  LEGEND  REMOTE  SERVER=LEGEND SLIBREF=WORK;
RSUBMIT;

%LET DB = DNFPUTDL; /*live*/
/*%LET DB = DLGSWQUT; /*test*/
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
CREATE TABLE BORROWER AS
	SELECT *
	FROM CONNECTION TO DBX 
		(
		SELECT DISTINCT
			PDXX.DF_SPE_ACC_ID AS ACCOUNT_NUMBER
			,CASE 
				WHEN LNXX.LC_STA_LONXX = 'X' 
					THEN MAX(LNXX.LN_DLQ_MAX)+X
				WHEN LNXX.LC_STA_LONXX ^= 'X' THEN 'X'
				ELSE 'X'
			END AS DAYS_DELINQUENT
		FROM 
			PKUB.WQXX_TSK_QUE WQXX
			INNER JOIN PKUB.PDXX_PRS_NME PDXX
				ON WQXX.BF_SSN = PDXX.DF_PRS_ID
			INNER JOIN PKUB.LNXX_LON_DLQ_HST LNXX
				ON WQXX.BF_SSN = LNXX.BF_SSN
		WHERE 
			WQXX.WF_QUE = 'XA'
			AND WQXX.WC_STA_WQUEXX ^= 'C'
		GROUP BY
			PDXX.DF_SPE_ACC_ID
			,LNXX.LC_STA_LONXX
		)
;
DISCONNECT FROM DBX;
%PUT  SQLXRC= >>> &SQLXRC <<< ||| SQLXMSG= >>> &SQLXMSG >>> ;  

QUIT;

ENDRSUBMIT;

DATA BORROWER;
	SET LEGEND.BORROWER;
RUN;

PROC EXPORT
		DATA=BORROWER
		OUTFILE="&RPTLIB\CNH XXXX.xlsx"
		DBMS = EXCEL
		REPLACE;
		SHEET="QXA";
RUN;
