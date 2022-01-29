/*%LET RPTLIB = %SYSGET(reportdir);*/
%LET RPTLIB = T:\SAS;
FILENAME REPORTZ "&RPTLIB/Military Outreach Tracking FEDRZ";
FILENAME REPORT2 "&RPTLIB/Military Outreach Tracking FEDR2";

DATA _NULL_;
     CALL SYMPUT('SIX',"'"||PUT(INTNX('MONTH',TODAY(),-6,'same'), MMDDYYD10.)||"'");
RUN;
%SYSLPUT SIX = &SIX;

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

PROC SQL ;
	CONNECT TO DB2 (DATABASE=&DB);

	CREATE TABLE MILT AS
		SELECT	
			*
		FROM	
			CONNECTION TO DB2 
				(
					SELECT DISTINCT
						LN10.BF_SSN
						,CASE
							WHEN MIDNY.BF_SSN IS NULL OR APRVD_DFR.LD_DFR_APL > MIDNY.LD_ATY_REQ_RCV THEN 'A'
							ELSE 'D'
						 END AS DFR_STA
					FROM
						PKUB.LN10_LON LN10
						INNER JOIN PKUB.AY10_BR_LON_ATY AY10
							ON LN10.BF_SSN = AY10.BF_SSN
							AND AY10.PF_REQ_ACT IN ('SCRAA','SCRAN','SCRAD')
							AND DAYS(AY10.LD_ATY_REQ_RCV) BETWEEN DAYS(&SIX) AND DAYS(CURRENT_DATE) /*Filtering to latest 6 months*/
						LEFT JOIN (
								SELECT DISTINCT
									LN50.BF_SSN
									,LN50.LD_DFR_APL
									,LN50.LD_DFR_END
								FROM PKUB.LN50_BR_DFR_APV LN50
									INNER JOIN PKUB.DF10_BR_DFR_REQ DF10
										ON LN50.BF_SSN = DF10.BF_SSN
										AND LN50.LF_DFR_CTL_NUM = DF10.LF_DFR_CTL_NUM
										AND DF10.LC_DFR_TYP = '38'
								WHERE LN50.LC_STA_LON50 = 'A'
									AND DF10.LC_DFR_STA = 'A'
									AND DF10.LC_STA_DFR10 = 'A'
									) APRVD_DFR
							ON LN10.BF_SSN = APRVD_DFR.BF_SSN
							AND DAYS(APRVD_DFR.LD_DFR_APL) BETWEEN DAYS(AY10.LD_ATY_REQ_RCV) AND DAYS(AY10.LD_ATY_REQ_RCV) + 60
							AND APRVD_DFR.LD_DFR_END > CURRENT_DATE
						LEFT JOIN (
								SELECT DISTINCT
									AY10.BF_SSN
									,AY10.LD_ATY_REQ_RCV
								FROM PKUB.AY10_BR_LON_ATY AY10
								WHERE AY10.PF_REQ_ACT = 'MIDNY'
									) MIDNY
							ON LN10.BF_SSN = MIDNY.BF_SSN
							AND DAYS(MIDNY.LD_ATY_REQ_RCV) BETWEEN DAYS(AY10.LD_ATY_REQ_RCV) AND DAYS(AY10.LD_ATY_REQ_RCV) + 60
					WHERE
						LN10.LA_CUR_PRI > 0
						AND LN10.LC_STA_LON10 = 'R'
						AND APRVD_DFR.BF_SSN IS NOT NULL OR MIDNY.BF_SSN IS NOT NULL


					FOR READ ONLY WITH UR
				)
	;

	DISCONNECT FROM DB2;

	/*%PUT  SQLXRC= >>> &SQLXRC <<< ||| SQLXMSG= >>> &SQLXMSG >>> ;  ** INCLUDES ERROR MESSAGES TO SAS LOG  ;*/
	/*%SQLCHECK;*/
QUIT;

ENDRSUBMIT;

DATA MILT; SET LEGEND.MILT; RUN;

PROC SQL;
CREATE TABLE OUTPT AS
	SELECT
		COUNT(DISTINCT M.BF_SSN) AS BOR_CNT
		,SUM(CASE WHEN DFR_STA = 'A' THEN 1 ELSE 0 END) AS APPROVED_CNT
		,SUM(CASE WHEN DFR_STA = 'D' THEN 1 ELSE 0 END) AS DENIED_CNT
	FROM MILT M
;
QUIT;


/*write to comma delimited file*/
DATA _NULL_;
	SET		WORK.OUTPT;
	FILE
		'T:\SAS\Military Outreach Tracking FED.txt'
		DELIMITER = ','
		DSD
		DROPOVER
		LRECL = 32767
	;

	FORMAT
		BOR_CNT BEST12.
		APPROVED_CNT BEST12.
		DENIED_CNT BEST12.
	;

	/* write column names, remove this to create a file without a header row */
	IF _N_ = 1 THEN
		DO;
			PUT	
				'Borrower Count'
				','
				'Approved Count'
				','
				'Denied Count'
			;
		END;

	/* write data*/	
	DO;
		PUT BOR_CNT @;
		PUT APPROVED_CNT @;
		PUT DENIED_CNT;
		;
	END;
RUN;
