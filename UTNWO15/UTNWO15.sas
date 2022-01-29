/*%LET RPTLIB = %SYSGET(reportdir);*/
%LET RPTLIB = T:\SAS;
FILENAME REPORTZ "&RPTLIB/UNWO15.NWO15RZ";
FILENAME REPORT2 "&RPTLIB/UNWO15.NWO15R2";

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

	CREATE TABLE INITPOP AS
		SELECT	
			*
		FROM	
			CONNECTION TO DB2 
				(
					SELECT DISTINCT
						LN16.LN_DLQ_MAX,
						PD10.DF_SPE_ACC_ID,
						LN10.LN_SEQ,
						CASE
/*							fridays*/
							WHEN DAYOFWEEK(CURRENT_DATE) = 6 AND LN16.LN_DLQ_MAX >= 351 THEN 'Y'
/*							holidays*/
							WHEN DAYS(CURRENT_DATE) = DAYS('2013-11-27') AND LN16.LN_DLQ_MAX >= 350 THEN 'Y'
							WHEN MONTH(CURRENT_DATE) = 12 AND DAY(CURRENT_DATE) = 24 AND LN16.LN_DLQ_MAX >= 350 THEN 'Y'
							WHEN MONTH(CURRENT_DATE) = 12 AND DAY(CURRENT_DATE) = 31 AND LN16.LN_DLQ_MAX >= 350 THEN 'Y'
							WHEN DAYS(CURRENT_DATE) = DAYS('2014-01-17') AND LN16.LN_DLQ_MAX >= 350 THEN 'Y'
							WHEN DAYS(CURRENT_DATE) = DAYS('2014-02-14') AND LN16.LN_DLQ_MAX >= 350 THEN 'Y'
							WHEN DAYS(CURRENT_DATE) = DAYS('2014-05-23') AND LN16.LN_DLQ_MAX >= 350 THEN 'Y'
							WHEN MONTH(CURRENT_DATE) = 7 AND DAY(CURRENT_DATE) = 3 AND LN16.LN_DLQ_MAX >= 350 THEN 'Y'
							WHEN DAYS(CURRENT_DATE) = DAYS('2014-08-29') AND LN16.LN_DLQ_MAX >= 350 THEN 'Y'
							WHEN DAYS(CURRENT_DATE) = DAYS('2014-11-26') AND LN16.LN_DLQ_MAX >= 350 THEN 'Y'
/*							every day delinquency*/
							WHEN LN16.LN_DLQ_MAX >= 354 THEN 'Y'
/*							not delinquent*/
							ELSE 'N'
						END AS IS_DELQ
					FROM
						PKUB.PD10_PRS_NME PD10
						JOIN PKUB.LN10_LON LN10
							ON PD10.DF_PRS_ID = LN10.BF_SSN
						JOIN PKUB.LN16_LON_DLQ_HST LN16
							ON LN10.BF_SSN = LN16.BF_SSN
							AND LN10.LN_SEQ = LN16.LN_SEQ
						JOIN PKUB.DW01_DW_CLC_CLU DW01
							ON LN10.BF_SSN = DW01.BF_SSN
							AND LN10.LN_SEQ = DW01.LN_SEQ
						JOIN PKUB.LN09_RPD_PIO_CVN LN09
							ON LN10.BF_SSN = LN09.BF_SSN
							AND LN10.LN_SEQ = LN09.LN_SEQ

					WHERE
						LN10.LC_STA_LON10 = 'R'
						AND LN10.LA_CUR_PRI + DW01.WA_TOT_BRI_OTS > 0.00
						AND LN16.LN_DLQ_MAX >= 350
						AND LN16.LC_STA_LON16 = '1'
						AND LN09.IF_LON_SRV_DFL_LON <> ''

					FOR READ ONLY WITH UR
				)
	;

	CREATE TABLE LIT AS
		SELECT DISTINCT
			DF_SPE_ACC_ID,
			LN_SEQ
		FROM
			INITPOP
		WHERE
			IS_DELQ = 'Y'
	;

	DISCONNECT FROM DB2;

	/*%PUT  SQLXRC= >>> &SQLXRC <<< ||| SQLXMSG= >>> &SQLXMSG >>> ;  ** INCLUDES ERROR MESSAGES TO SAS LOG  ;*/
	/*%SQLCHECK;*/
QUIT;

ENDRSUBMIT;
DATA INITPOP; SET LEGEND.INITPOP; RUN;
DATA LIT; SET LEGEND.LIT; RUN;

/*write to comma delimited file*/
DATA _NULL_;
	SET		LIT;
	FILE	REPORT2 delimiter=',' DSD DROPOVER lrecl=32767;
	DO;
		PUT DF_SPE_ACC_ID $ @;
		PUT LN_SEQ $;
		;
	END;
RUN;
