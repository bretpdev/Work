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
CREATE TABLE ADXXDUMP AS
	SELECT *
	FROM CONNECTION TO DBX 
		(
		SELECT 
			*
		FROM 
			PKUB.ADXX_PCV_ATY_ADJ ADXX
		WHERE
			BF_SSN IN 
				(
				SELECT
					DF_PRS_ID
				FROM
					PKUB.PDXX_PRS_NME PDXX
				WHERE
					DF_SPE_ACC_ID IN
					(
						 'XXXXXXXXXX'
						,'XXXXXXXXXX'
						,'XXXXXXXXXX'
						,'XXXXXXXXXX'
					)
				)
		)
;
CREATE TABLE LNXXDUMP AS
	SELECT *
	FROM CONNECTION TO DBX 
		(
		SELECT 
			*
		FROM 
			PKUB.LNXX_INT_RTE_HST LNXX
		WHERE
			BF_SSN IN 
				(
				SELECT
					DF_PRS_ID
				FROM
					PKUB.PDXX_PRS_NME PDXX
				WHERE
					DF_SPE_ACC_ID IN
					(
						 'XXXXXXXXXX'
						,'XXXXXXXXXX'
						,'XXXXXXXXXX'
						,'XXXXXXXXXX'
					)
				)
		)
;
DISCONNECT FROM DBX;
%PUT  SQLXRC= >>> &SQLXRC <<< ||| SQLXMSG= >>> &SQLXMSG >>> ;  

QUIT;

ENDRSUBMIT;

DATA ADXXDUMP;
	SET LEGEND.ADXXDUMP;
RUN;

DATA LNXXDUMP;
	SET LEGEND.LNXXDUMP;
RUN;

PROC EXPORT
		DATA=ADXXDUMP
		OUTFILE="&RPTLIB\CNH XXXX.xlsx"
		DBMS = EXCEL
		REPLACE;
		SHEET="_ADXX";
RUN;

PROC EXPORT
		DATA=LNXXDUMP
		OUTFILE="&RPTLIB\CNH XXXX.xlsx"
		DBMS = EXCEL
		REPLACE;
		SHEET="_LNXX";
RUN;
