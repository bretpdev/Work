/*%LET RPTLIB = %SYSGET(reportdir);*/
%LET RPTLIB = T:\SAS;

FILENAME REPORTZ "&RPTLIB/NHXXXXX.RZ";
FILENAME REPORTX "&RPTLIB/NHXXXXX.RX";

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
CREATE TABLE FSAXxDLI AS
	SELECT *
	FROM CONNECTION TO DBX 
		(
		SELECT 
			PDXX.DF_SPE_ACC_ID AS Acct_no,
			LNXX.LN_SEQ AS Loan_Seq,
			LNXX.LC_STA_LONXX AS LNXX_Status,
			DWXX.WC_DW_LON_STA AS Loan_Status, /*data warehouse loan status*/
			LNXX.LD_PIF_RPT AS PIF_Date /*paid in full*/
		FROM 
			PKUB.PDXX_PRS_NME PDXX
			INNER JOIN PKUB.LNXX_LON LNXX
				ON LNXX.BF_SSN = PDXX.DF_PRS_ID
			INNER JOIN PKUB.DWXX_A DWXX
				ON DWXX.BF_SSN = LNXX.BF_SSN
				AND DWXX.LN_SEQ = LNXX.LN_SEQ
			INNER JOIN PKUB.LNXX_LON_BBS LNXX
				ON LNXX.BF_SSN = LNXX.BF_SSN
				AND LNXX.LN_SEQ = LNXX.LN_SEQ
			INNER JOIN 
				(
				SELECT 
					LNXXA.BF_SSN,
					LNXXA.LN_SEQ,
					COUNT(LNXXA.LC_INT_RDC_PGM)
				FROM
					PKUB.LNXX_INT_RTE_HST LNXXA
					INNER JOIN PKUB.LNXX_INT_RTE_HST LNXXB
						ON LNXXA.BF_SSN = LNXXB.BF_SSN
						AND LNXXA.LN_SEQ = LNXXB.LN_SEQ
				WHERE
					LNXXA.LC_INT_RDC_PGM = 'R' /*interest rate reduction program*/
				GROUP BY
					LNXXA.BF_SSN,
					LNXXA.LN_SEQ
				HAVING
					COUNT(LNXXA.LC_INT_RDC_PGM)>X
				)LNXX 
					ON LNXX.BF_SSN = LNXX.BF_SSN
					AND LNXX.LN_SEQ = LNXX.LN_SEQ
		WHERE 
			LNXX.PM_BBS_PGM = 'DLI' /*borrower benefit program*/
		)
;
DISCONNECT FROM DBX;
%PUT  SQLXRC= >>> &SQLXRC <<< ||| SQLXMSG= >>> &SQLXMSG >>> ;  

QUIT;

ENDRSUBMIT;

DATA FSAXxDLI;
	SET LEGEND.FSAXxDLI;
RUN;

PROC PRINT DATA=FSAXxDLI;
RUN;

PROC EXPORT
		DATA=FSAXxDLI
		OUTFILE="&RPTLIB\CNH XXXX.xlsx"
		DBMS = EXCEL
		REPLACE;
		SHEET="FSAXxDLI";
RUN;
