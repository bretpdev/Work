%LET RPTLIB = %SYSGET(reportdir);
/*%LET RPTLIB = T:\SAS;*/

FILENAME REPORTZ "&RPTLIB/UNWS98.NWS98RZ";
FILENAME REPORT2 "&RPTLIB/UNWS98.NWS98R2";

/*LIBNAME  LEGEND  REMOTE  SERVER=LEGEND SLIBREF=work  ;*/
%LET BegMonth = Current Date + 1 Days - Day(Current Date) days;
%LET EndMonth = Last_Day(Current Date);
%SYSLPUT _all_;
/*RSUBMIT LEGEND;*/
/*%let DB = DNFPRQUT;  *This is test;*/
/*%let DB = DNFPRUUT;  *This is VUK3 test;*/
%LET DB = DNFPUTDL;  *This is live;

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
						SUM(CreatedCount) AS CreatedOnlineAccount,
						SUM(AccessedCount) AS AccessedOnlineAccount,
						SUM(AccessedEcorrCount) AS AccessedEcorrInbox,
						SUM(MadeOnlinePaymentCount) AS MadeOnlinePayment,
						SUM(RequestedEBillCount) AS RequestedEBill,
						Current Date - 1 Month AS ReportingDate
					FROM
						PKUB.PD10_PRS_NME PD10
						LEFT JOIN 
							(
								SELECT DISTINCT
									CASE WHEN DF_CRT_DTS_WB24 >= &BegMonth AND DF_CRT_DTS_WB24 <= &EndMonth THEN 1 ELSE 0 END AS CreatedCount,
									CASE WHEN DF_USR_LST_ATH_DTS >= &BegMonth AND DF_USR_LST_ATH_DTS <= &EndMonth THEN 1 ELSE 0 END AS AccessedCount,
									CASE WHEN DF_USR_LST_IBX_ACS >= &BegMonth AND DF_USR_LST_IBX_ACS <= &EndMonth THEN 1 ELSE 0 END AS AccessedEcorrCount,
									DF_USR_SSN
								FROM
									WEBFLS1.WB24_CSM_USR_ACC
							) WB24 ON PD10.DF_PRS_ID = WB24.DF_USR_SSN
						LEFT JOIN
							(
								SELECT DISTINCT
									CASE WHEN BF_SSN IS NULL THEN 0 ELSE 1 END AS MadeOnlinePaymentCount,
									BF_SSN
								FROM
									WEBFLS1.RM03_ONL_PAY
								WHERE
									NF_ONL_PAY_DTS >= &BegMonth
									AND NF_ONL_PAY_DTS <= &EndMonth
									AND NF_IPH <> '192.84.171.174' /*Local IP to exclude when agents access borrowers online accounts*/
							) RM03 ON PD10.DF_PRS_ID = RM03.BF_SSN 
						LEFT JOIN
							(
								SELECT DISTINCT
									CASE WHEN BF_OPS_SSN IS NULL THEN 0 ELSE 1 END AS RequestedEBillCount,
									BF_OPS_SSN
								FROM
									WEBFLS1.RM12_OPS_BIL_MTD
								WHERE
									LF_OPS_DTS_SBM >= &BegMonth
									AND LF_OPS_DTS_SBM <= &EndMonth
									AND LC_OPS_CUS_BIL_PFC = 'E'
							) RM12 ON PD10.DF_PRS_ID = RM12.BF_OPS_SSN

					FOR READ ONLY WITH UR
				)
	;

	DISCONNECT FROM DB2;
	/*%PUT  SQLXRC= >>> &SQLXRC <<< ||| SQLXMSG= >>> &SQLXMSG >>> ;  ** INCLUDES ERROR MESSAGES TO SAS LOG  ;*/
	/*%SQLCHECK;*/
QUIT;

/*ENDRSUBMIT;*/

/*DATA DEMO; SET LEGEND.DEMO; RUN;*/

/*create printed report*/
PROC PRINTTO PRINT=REPORT2 NEW; RUN;

OPTIONS ORIENTATION=LANDSCAPE PS=39 LS=127;
TITLE 		'Web Portal Stats - FED';
TITLE2		"RUNDATE &SYSDATE9";
FOOTNOTE1  	"THIS DOCUMENT MAY CONTAIN BORROWERS' SENSITIVE INFORMATION THAT UHEAA HAS PLEDGED TO PROTECT.";
FOOTNOTE2	'PLEASE TAKE APPROPRIATE PRECAUTIONS TO SAFEGUARD THIS INFORMATION.';
FOOTNOTE3	;
FOOTNOTE4   'JOB = UTNWS98  	 REPORT = UNWS98.UNWS98R2';

PROC PRINT 
		NOOBS SPLIT = '/' 
		DATA = DEMO 
		WIDTH = UNIFORM 
		WIDTH = MIN 
		LABEL;

	FORMAT
		CreatedOnlineAccount BEST12.
		AccessedOnlineAccount BEST12.
		AccessedEcorrInbox BEST12.
		MadeOnlinePayment BEST12.
		RequestedEBill BEST12.;

	VAR 
		CreatedOnlineAccount 
		AccessedOnlineAccount
		AccessedEcorrInbox
		MadeOnlinePayment
		RequestedEBill;

	LABEL
		CreatedOnlineAccount = 'Created Online Account'
		AccessedOnlineAccount = 'Accessed Online Account'
		AccessedEcorrInbox = 'Accessed Ecorr Inbox'
		MadeOnlinePayment = 'Made Online Payment'
		RequestedEBill = 'Requested Electronic Bill';
	;
RUN;

PROC PRINTTO; RUN;
