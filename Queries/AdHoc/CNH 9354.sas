%LET NHTIX = CNH_XXXX;

%LET RPTLIB = T:\SAS;
LIBNAME  LEGEND  REMOTE  SERVER=LEGEND SLIBREF=WORK;
RSUBMIT;
%LET DB = DNFPUTDL;
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
			LNXX.BF_SSN
			,LNXX.LN_SEQ
			,LNXX.LN_RPS_SEQ
			,LNXX.LA_RPS_ISL_LEVELX
			,LNXX.LN_GRD_RPS_SEQ_LEVELX			
			,LNXX.LA_RPS_ISL_LEVELX
			,LNXX.LN_GRD_RPS_SEQ_LEVELX
		FROM 
			PKUB.LNXX_LON LNXX
			INNER JOIN PKUB.LNXX_LON_RPS LNXX
				ON LNXX.BF_SSN = LNXX.BF_SSN
				AND LNXX.LN_SEQ = LNXX.LN_SEQ
			INNER JOIN 
				(
					SELECT
						LNXXA.BF_SSN
						,LNXXA.LN_SEQ
						,LNXXA.LN_RPS_SEQ
						,LNXXA.LA_RPS_ISL		AS LA_RPS_ISL_LEVELX
						,LNXXA.LN_GRD_RPS_SEQ	AS LN_GRD_RPS_SEQ_LEVELX
						,LNXXB.LA_RPS_ISL 		AS LA_RPS_ISL_LEVELX
						,LNXXB.LN_GRD_RPS_SEQ	AS LN_GRD_RPS_SEQ_LEVELX

					FROM
						PKUB.LNXX_LON_RPS_SPF LNXXA
						INNER JOIN PKUB.LNXX_LON_RPS_SPF LNXXB
							ON LNXXA.BF_SSN = LNXXB.BF_SSN
							AND LNXXA.LN_SEQ = LNXXB.LN_SEQ
							AND LNXXA.LN_RPS_SEQ = LNXXB.LN_RPS_SEQ
					WHERE
						LNXXA.LA_RPS_ISL <= LNXXB.LA_RPS_ISL
						AND LNXXA.LN_GRD_RPS_SEQ = X
						AND LNXXB.LN_GRD_RPS_SEQ = X

				) LNXX
					ON LNXX.BF_SSN = LNXX.BF_SSN
					AND LNXX.LN_SEQ = LNXX.LN_SEQ
					AND LNXX.LN_RPS_SEQ = LNXX.LN_RPS_SEQ
		WHERE 
			LNXX.LC_STA_LONXX = 'A'
			AND LNXX.LC_TYP_SCH_DIS = 'CX'
			AND LNXX.LC_STA_LONXX = 'R'
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
	OUTFILE="&RPTLIB\&NHTIX..xlsx"
	DBMS = EXCEL
	REPLACE;
	SHEET="ICR_borrowers";
RUN;
