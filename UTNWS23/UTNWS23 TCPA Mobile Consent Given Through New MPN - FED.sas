/*%LET RPTLIB = %SYSGET(reportdir);*/
%LET RPTLIB = T:\SAS;
FILENAME REPORTZ "&RPTLIB/UNWS23.NWS23RZ";
FILENAME REPORT2 "&RPTLIB/UNWS23.NWS23R2";

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
	SELECT	*
	FROM	CONNECTION TO DB2 (
		SELECT DISTINCT A.DF_SPE_ACC_ID
	FROM PKUB.PD10_PRS_NME A
		INNER JOIN PKUB.LN10_LON B
			ON A.DF_PRS_ID = B.BF_SSN
		INNER JOIN PKUB.PD40_PRS_PHN C
			ON A.DF_PRS_ID = C.DF_PRS_ID
		LEFT OUTER JOIN (
			SELECT PKUB.LN10_LON.BF_SSN
			FROM PKUB.LN10_LON
			WHERE PKUB.LN10_LON.LD_LON_1_DSB < '10/21/2009')D
			ON A.DF_PRS_ID = D.BF_SSN
	WHERE B.LD_LON_1_DSB > '10/20/2009'
		AND B.LA_CUR_PRI > 0
		AND B.LC_STA_LON10 = 'R'
		AND C.DI_PHN_VLD = 'Y'
		AND C.DC_ALW_ADL_PHN NOT IN ('L','P','X')
		AND D.BF_SSN IS NULL
				FOR READ ONLY WITH UR
			);
DISCONNECT FROM DB2;

/*%PUT  SQLXRC= >>> &SQLXRC <<< ||| SQLXMSG= >>> &SQLXMSG >>> ;  ** INCLUDES ERROR MESSAGES TO SAS LOG  ;*/
/*%SQLCHECK;*/
QUIT;

ENDRSUBMIT;
DATA DEMO; SET LEGEND.DEMO; RUN;

 data _null_;
 	set  WORK.Demo;
 	format DF_SPE_ACC_ID $10. ;
 	file Report2 delimiter=',' DSD DROPOVER lrecl=32767;
 	if _n_ = 1 then do;		
		put "DF_SPE_ACC_ID";
	end;
	do;
		put DF_SPE_ACC_ID $ ;
	end;
run;
