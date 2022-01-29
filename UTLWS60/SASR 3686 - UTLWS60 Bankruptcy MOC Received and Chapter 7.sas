/*%LET RPTLIB = %SYSGET(reportdir);*/
%LET RPTLIB = T:\SAS;
FILENAME REPORTZ "&RPTLIB/ULWS60.LWS60RZ";
FILENAME REPORT2 "&RPTLIB/ULWS60.LWS60R2";
LIBNAME  DUSTER  REMOTE  SERVER=DUSTER  SLIBREF=WORK;

DATA _NULL_;
	CALL SYMPUT('BKDATE',"'"||PUT(INTNX('WEEKDAY',TODAY(),-5,'beginning'), MMDDYYD10.)||"'");
RUN;

%SYSLPUT BKDATE = &BKDATE;
RSUBMIT;
/*LIBNAME OLWHRM1 DB2 DATABASE=DLGSUTWH OWNER=OLWHRM1;*/

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
	CONNECT TO DB2 (DATABASE=DLGSUTWH);

	CREATE TABLE S60 AS
		SELECT *
		FROM 
			CONNECTION TO DB2 
				(
					SELECT
						DISTINCT
						PD01.DF_SPE_ACC_ID
					FROM	
						OLWHRM1.LN10_LON LN10
						INNER JOIN OLWHRM1.PD24_PRS_BKR PD24
							ON LN10.BF_SSN = PD24.DF_PRS_ID
							AND LN10.LC_STA_LON10 = 'R'
							AND LN10.LA_CUR_PRI > 0
							AND PD24.DC_BKR_TYP = '07'
							AND PD24.DC_BKR_STA = '06'
						INNER JOIN OLWHRM1.AY01_BR_ATY AY01
							ON LN10.BF_SSN = AY01.DF_PRS_ID
							AND AY01.PF_ACT = 'MDCID'
							AND SUBSTR(AY01.BX_CMT, 65, 5) = 'ASMOC'
							AND DAYS(AY01.BF_LST_DTS_AY01) = DAYS(&BKDATE)
							AND DAYS(AY01.BF_LST_DTS_AY01) > DAYS(PD24.DD_BKR_STA)
						INNER JOIN OLWHRM1.AY01_BR_ATY AY01A
							ON LN10.BF_SSN = AY01A.DF_PRS_ID
							AND AY01A.PF_ACT = 'DBKRW'
							AND AY01A.BF_LST_DTS_AY01 > AY01.BF_LST_DTS_AY01
						INNER JOIN OLWHRM1.PD01_PDM_INF PD01
							ON LN10.BF_SSN = PD01.DF_PRS_ID
						INNER JOIN OLWHRM1.DW01_DW_CLC_CLU DW01
							ON LN10.BF_SSN = DW01.BF_SSN
							AND LN10.LN_SEQ = DW01.LN_SEQ
							AND DW01.WC_DW_LON_STA = '21'

					FOR READ ONLY WITH UR
				)
	;

	DISCONNECT FROM DB2;

	/*%PUT  SQLXRC= >>> &SQLXRC <<< ||| SQLXMSG= >>> &SQLXMSG >>> ;  ** INCLUDES ERROR MESSAGES TO SAS LOG  ;*/
	/*%SQLCHECK;*/
	/*QUIT;*/
QUIT;

ENDRSUBMIT;
DATA S60; SET DUSTER.S60; RUN;

DATA _NULL_;
	SET  WORK.S60; 
	FILE REPORT2 DELIMITER=',' DSD DROPOVER LRECL=32767;

	ARC_NAME = 'BKMOC';
	FROM_DATE = '';
	TO_DATE = '';
	NEEDED_BY_DATE = '';
	RECIPIENT_ID = '';
	REGARDS_TO_CODE = '';
	REGARDS_TO_ID = '';
	LOAN_SEQ = 'ALL';
	FORMAT COMMENTS $200. ;
	COMMENTS = 'Review MOC and Pacer for Chapter 13';

	DO;
		PUT DF_SPE_ACC_ID $ @;
		PUT ARC_NAME $ @;
		PUT FROM_DATE @;
		PUT TO_DATE @;
		PUT NEEDED_BY_DATE @;
		PUT RECIPIENT_ID @;
		PUT REGARDS_TO_CODE @;
		PUT REGARDS_TO_ID @;
		PUT LOAN_SEQ @;
		PUT COMMENTS $ ;
	END;
RUN;
