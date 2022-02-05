/*%LET RPTLIB = %SYSGET(reportdir);*/
%LET RPTLIB = T:\SAS;
FILENAME REPORTZ "&RPTLIB/UNWQ03.NWQ03RZ";
FILENAME REPORT2 "&RPTLIB/UNWQ03.NWQ03R2";
FILENAME REPORT3 "&RPTLIB/UNWQ03.NWQ03R3";

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
CREATE TABLE NREF AS
	SELECT	*
	FROM	CONNECTION TO DB2 (
		SELECT DISTINCT A.DF_SPE_ACC_ID
	FROM PKUB.PD10_PRS_NME A
		INNER JOIN PKUB.PD40_PRS_PHN B
			ON A.DF_PRS_ID = B.DF_PRS_ID
		INNER JOIN PKUB.LN10_LON C
			ON A.DF_PRS_ID = C.BF_SSN
	WHERE B.DI_PHN_VLD = 'Y'
		AND B.DC_ALW_ADL_PHN IN ('P','X')
		AND B.DD_PHN_VER < '04/01/2012'
		AND C.LA_CUR_PRI > 0
		AND C.LC_STA_LON10 = 'R'
			FOR READ ONLY WITH UR
		);
			
CREATE TABLE REF AS
	SELECT *
	FROM	CONNECTION TO DB2(
		SELECT DISTINCT	RF10.BF_RFR
		FROM	PKUB.LN10_LON LN10
			INNER JOIN PKUB.RF10_RFR RF10
			ON	LN10.BF_SSN = RF10.BF_SSN
			INNER JOIN PKUB.PD40_PRS_PHN PD40
			ON	RF10.BF_RFR = PD40.DF_PRS_ID
		WHERE	LN10.LA_CUR_PRI > 0
		AND		LN10.LC_STA_LON10 = 'R'
		AND		PD40.DI_PHN_VLD = 'Y'
		AND 	PD40.DC_ALW_ADL_PHN IN ('P','X')
		AND		PD40.DD_PHN_VER < '04/01/2012'
				FOR READ ONLY WITH UR
			);
DISCONNECT FROM DB2;
/*%PUT  SQLXRC= >>> &SQLXRC <<< ||| SQLXMSG= >>> &SQLXMSG >>> ;  ** INCLUDES ERROR MESSAGES TO SAS LOG  ;*/
/*%SQLCHECK;*/
QUIT;

ENDRSUBMIT;

DATA NREF; SET LEGEND.NREF; RUN;
DATA REF; SET LEGEND.REF; RUN;

 data _null_;
 	set  WORK.NREF;
 	format DF_SPE_ACC_ID $10. ;
 	file Report2 delimiter=',' DSD DROPOVER lrecl=32767;
 	if _n_ = 1 then do;		
		put "DF_SPE_ACC_ID";
	end;
	do;
		put DF_SPE_ACC_ID $ ;
	end;
run;

 data _null_;
 	set  WORK.REF;
 	format BF_RFR $9. ;
 	file Report3 delimiter=',' DSD DROPOVER lrecl=32767;
 	if _n_ = 1 then do;		
		put "BF_RFR";
	end;
	do;
		put BF_RFR $ ;
	end;
run;
