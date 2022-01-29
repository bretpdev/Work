/*%LET RPTLIB = %SYSGET(reportdir);*/
%LET RPTLIB = T:\SAS;
FILENAME REPORTZ "&RPTLIB/ULWAB1.LWAB1RZ";
FILENAME REPORT2 "&RPTLIB/ULWAB1.LWAB1R2";
LIBNAME  DUSTER  REMOTE  SERVER=DUSTER  SLIBREF=WORK;
RSUBMIT;
LIBNAME OLWHRM1 DB2 DATABASE=DLGSUTWH OWNER=OLWHRM1;

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
%MACRO PROCESS(DATASET, LF_LON_CUR_OWN);
PROC SQL OUTOBS = 5;
	CREATE TABLE &DATASET AS
		SELECT
			PD10.DF_SPE_ACC_ID,
			&LF_LON_CUR_OWN AS LF_LON_CUR_OWN
		FROM	
			OLWHRM1.PD10_PRS_NME PD10
			INNER JOIN OLWHRM1.LN10_LON LN10
				ON LN10.BF_SSN = PD10.DF_PRS_ID
		WHERE	
			LN10.LC_STA_LON10 = 'R' 
			AND LA_CUR_PRI = 0
			AND LF_LON_CUR_OWN = &LF_LON_CUR_OWN
;
QUIT;
%MEND;

%PROCESS(ONE, '830248');
%PROCESS(TWO, '826717');
%PROCESS(THREE, '834265');

ENDRSUBMIT;
DATA FINAL;
	SET DUSTER.ONE DUSTER.TWO DUSTER.THREE;
RUN;

PROC EXPORT DATA = WORK.FINAL 
            OUTFILE = "T:\NH 24534.xlsx" 
            DBMS = EXCEL
			REPLACE;
     SHEET="Sheet1"; 
RUN;
