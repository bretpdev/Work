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

	CREATE TABLE EMP AS
		SELECT 
			*
		FROM 
			CONNECTION TO DB2 
				(
					SELECT
						PD01.DF_PRS_ID
						,PD01.DM_PRS_LST
						,IN01.IM_IST_FUL
					FROM	
						OLWHRM1.IN01_LGS_IDM_MST IN01
						JOIN OLWHRM1.BR02_BR_EMP BR02
							ON IN01.IF_IST = BR02.BF_EMP_ID_1
							OR IN01.IF_IST = BR02.BF_EMP_ID_2
							OR IN01.IF_IST = BR02.BF_EMP_ID_3
							OR IN01.IF_IST = BR02.BF_EMP_ID_4
							OR IN01.IF_IST = BR02.BF_EMP_ID_5
							OR IN01.IF_IST = BR02.BF_EMP_ID_6
							OR IN01.IF_IST = BR02.BF_EMP_ID_7
							OR IN01.IF_IST = BR02.BF_EMP_ID_8
							OR IN01.IF_IST = BR02.BF_EMP_ID_9
							OR IN01.IF_IST = BR02.BF_EMP_ID_10
							OR IN01.IF_IST = BR02.BF_EMP_ID_11
							OR IN01.IF_IST = BR02.BF_EMP_ID_12
						JOIN OLWHRM1.PD01_PDM_INF PD01
							ON BR02.BF_SSN = PD01.DF_PRS_ID
					WHERE	
						IN01.IC_IST_TYP = '006'
						AND
						IN01.IF_IST IN ('GN000489','GN001888','GN002736','GN000677','GN000679')


					FOR READ ONLY WITH UR
				)
	;

	DISCONNECT FROM DB2;

	/*%PUT  SQLXRC= >>> &SQLXRC <<< ||| SQLXMSG= >>> &SQLXMSG >>> ;  ** INCLUDES ERROR MESSAGES TO SAS LOG  ;*/
	/*%SQLCHECK;*/
QUIT;

ENDRSUBMIT;
DATA EMP;
	SET DUSTER.EMP;
RUN;

/*export to Excel spreadsheet*/
PROC EXPORT DATA = WORK.EMP 
            OUTFILE = "T:\SAS\NH 27056.xlsx" 
            DBMS = EXCEL
			REPLACE;
     SHEET="A"; 
RUN;
