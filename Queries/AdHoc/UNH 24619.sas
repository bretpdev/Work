/*%LET RPTLIB = %SYSGET(reportdir);*/
%LET RPTLIB = T:\SAS;
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
						PD10.DF_SPE_ACC_ID AS AwardedAccount
						,YEAR(LN15.LD_DSB) 
						,MONTH(LN15.LD_DSB)
					FROM	
						OLWHRM1.LN10_LON LN10
						INNER JOIN OLWHRM1.LN15_DSB LN15
							ON LN10.BF_SSN = LN15.BF_SSN
							AND LN10.LN_SEQ = LN15.LN_SEQ
							AND LD_DSB BETWEEN '07/01/2004' AND '06/30/2015'
						LEFT JOIN OLWHRM1.SC10_SCH_DMO SC10
							ON LN10.LF_DOE_SCL_ORG = SC10.IF_DOE_SCL
						INNER JOIN OLWHRM1.PD10_PRS_NME PD10
							ON LN10.BF_SSN = PD10.DF_PRS_ID
					WHERE	
						LN10.IC_LON_PGM = 'TILP'
						AND (LN15.LA_DSB - COALESCE(LN15.LA_DSB_CAN,0)) != 0

					FOR READ ONLY WITH UR
				)
	;
/*After receiving this info, you need to group the output by FISCAL YEAR and then remove duplicate account numbers and count the remaining rows*/
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
            OUTFILE = "T:\NH 24619.xlsx" 
            DBMS = EXCEL
			REPLACE;
     SHEET="Sheet1"; 
RUN;
