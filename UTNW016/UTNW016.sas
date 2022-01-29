/*%LET RPTLIB = %SYSGET(reportdir);*/
%LET RPTLIB = T:\SAS;
FILENAME REPORTZ "&RPTLIB/UNWO16.NWO16RZ";
FILENAME REPORT2 "&RPTLIB/UNWO16.NWO16R2";

LIBNAME  LEGEND  REMOTE  SERVER=LEGEND SLIBREF=work  ;
RSUBMIT LEGEND;
/*%let DB = DNFPRQUT;  *This is test;*/
/*%let DB = DNFPRUUT;  *This is VUK3 test;*/
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

	CREATE TABLE DEMO AS
		SELECT	
			*
		FROM	
			CONNECTION TO DB2 
				(
					SELECT	
						DISTINCT
						PD10.DF_SPE_ACC_ID,
						PD23.DD_DSA_STA
						
					FROM
						PKUB.PD10_PRS_NME PD10
					INNER JOIN PKUB.LN10_LON LN10
						ON PD10.DF_PRS_ID = LN10.BF_SSN
					INNER JOIN PKUB.DW01_DW_CLC_CLU DW01
						ON DW01.BF_SSN = LN10.BF_SSN
						AND DW01.LN_SEQ = LN10.LN_SEQ
					INNER JOIN PKUB.PD23_GTR_DSA PD23
						ON PD23.DF_PRS_ID = LN10.BF_SSN
					WHERE
						LN10.LA_CUR_PRI > 0
						AND LN10.LC_STA_LON10 = 'R'
						AND DW01.WC_DW_LON_STA = '19'
					ORDER BY
						DD_DSA_STA

					FOR READ ONLY WITH UR
				)
	;

	DISCONNECT FROM DB2;

	/*%PUT  SQLXRC= >>> &SQLXRC <<< ||| SQLXMSG= >>> &SQLXMSG >>> ;  ** INCLUDES ERROR MESSAGES TO SAS LOG  ;*/
	/*%SQLCHECK;*/
QUIT;

ENDRSUBMIT;

DATA DEMO;
SET LEGEND.DEMO;
RUN;

/*write to comma delimited file*/
DATA _NULL_;
	SET		WORK.DEMO;
	FILE
		REPORT2
		DELIMITER = ','
		DSD
		DROPOVER
		LRECL = 32767
	;

	FORMAT
		DF_SPE_ACC_ID $10.
		DD_DSA_STA MMDDYY10.
	;

	/* write column names, remove this to create a file without a header row */
	IF _N_ = 1 THEN
		DO;
			PUT	
				'ACCOUNT_NUMBER'
				','
				'VERIFIED_DISABILITY_STATUS_DATE'
			;
		END;

	/* write data*/	
	DO;
		PUT DF_SPE_ACC_ID $ @;
		PUT DD_DSA_STA
		;
	END;
RUN;

