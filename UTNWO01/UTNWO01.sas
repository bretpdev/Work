/*%LET RPTLIB = %SYSGET(reportdir);*/
%LET RPTLIB = T:\SAS;
FILENAME REPORTZ "&RPTLIB/UNWO01.NWO01RZ";
FILENAME REPORT2 "&RPTLIB/UNWO01.NWO01R2";

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
CREATE TABLE DEMO AS
SELECT *
FROM CONNECTION TO DB2 (
SELECT	DISTINCT
		PD10.DF_SPE_ACC_ID
		,FS10.LD_DL_CON_INQ
FROM	PKUB.LN10_LON LN10
		INNER JOIN PKUB.FS10_DL_LON FS10
			ON LN10.BF_SSN = FS10.BF_SSN
		INNER JOIN PKUB.PD10_PRS_NME PD10
			ON LN10.BF_SSN = PD10.DF_PRS_ID
WHERE	LN10.LA_CUR_PRI > 0
		AND LN10.LC_STA_LON10 = 'R'
		AND FS10.LD_DL_CON_INQ IS NOT NULL
		AND NOT EXISTS (SELECT *
						FROM 	PKUB.AY10_BR_LON_ATY AY10
								INNER JOIN PKUB.FS10_DL_LON FS10
									ON AY10.BF_SSN = FS10.BF_SSN
						WHERE	AY10.PF_REQ_ACT = 'DLCON'
								AND AY10.LD_ATY_REQ_RCV > FS10.LD_DL_CON_INQ
								AND LN10.BF_SSN = AY10.BF_SSN
						)
FOR READ ONLY WITH UR
);
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
SET  WORK.DEMO;
FILE REPORT2 delimiter=',' DSD DROPOVER lrecl=32767;
FORMAT DF_SPE_ACC_ID $10. ;
FORMAT LD_DL_CON_INQ $MMDDYY10.;
DO;
PUT DF_SPE_ACC_ID $ @;
PUT 'DLCON' @;
PUT ',,,,,,,' @;
PUT 'ALL,' @;
PUT 'borrower has started the direct consolidation process as of ' @;
PUT LD_DL_CON_INQ $;

;
END;
RUN;
