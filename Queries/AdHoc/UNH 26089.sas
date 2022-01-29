/*%LET RPTLIB = %SYSGET(reportdir);*/
%LET RPTLIB = T:\SAS;
FILENAME REPORTZ "&RPTLIB/ULWAB1.LWAB1RZ";
FILENAME REPORT2 "&RPTLIB/ULWAB1.LWAB1R2";
LIBNAME  DUSTER  REMOTE  SERVER=DUSTER  SLIBREF=WORK;
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

	CREATE TABLE DEMO AS
		SELECT 
			*
		FROM 
			CONNECTION TO DB2 
				(
					SELECT DISTINCT
						LN10.BF_SSN
						,DF10.LF_DFR_CTL_NUM
						,DF10.LC_DFR_TYP AS LC_DFR_TYP_OLD
						,'45' AS LC_DFR_TYP_NEW
						,DF10.LF_DOE_SCL_DFR AS LF_DUE_SCL_DFR_OLD
						,'99999900' AS LF_DUE_SCL_DFR_NEW
						,DF10.LF_STU_SSN AS LF_STU_SSN_OLD
						,LN10.LF_STU_SSN AS LF_STU_SSN_NEW
					FROM	
						OLWHRM1.LN10_LON LN10
						JOIN OLWHRM1.LN50_BR_DFR_APV LN50
							ON LN10.BF_SSN = LN50.BF_SSN
							AND LN10.LN_SEQ = LN50.LN_SEQ
						JOIN OLWHRM1.DF10_BR_DFR_REQ DF10
							ON LN10.BF_SSN = DF10.BF_SSN
							AND LN50.LF_DFR_CTL_NUM = DF10.LF_DFR_CTL_NUM
					WHERE	
						LN10.LC_STA_LON10 = 'P'
						AND
						LN10.IC_LON_PGM IN ('PLUS')
						AND
						LN10.LF_LON_CUR_OWN = '829769'
						AND
						LN50.LC_STA_LON50 = 'A'
						AND
						DF10.LC_DFR_TYP IN ('17','34','07')
						AND
						DF10.LC_DFR_STA = 'A'
						AND 
						DF10.LC_STA_DFR10 = 'A'

					FOR READ ONLY WITH UR
				)
	;

	DISCONNECT FROM DB2;

	/*%PUT  SQLXRC= >>> &SQLXRC <<< ||| SQLXMSG= >>> &SQLXMSG >>> ;  ** INCLUDES ERROR MESSAGES TO SAS LOG  ;*/
	/*%SQLCHECK;*/
QUIT;

ENDRSUBMIT;
DATA DEMO;
	SET DUSTER.DEMO;
RUN;

/*export to Excel spreadsheet*/
PROC EXPORT DATA = WORK.DEMO 
            OUTFILE = "T:\SAS\UNH 26089.xlsx" 
            DBMS = EXCEL
			REPLACE;
     SHEET="A"; 
RUN;
