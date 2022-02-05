/*LIBNAME DLGSUTWH DB2 DATABASE=DLGSUTWH OWNER=OLWHRM1;*/
/*%LET RPTLIB = %SYSGET(reportdir);*/
%LET RPTLIB = T:/SAS;
FILENAME REPORT2 "&RPTLIB/ULWD28.LWD28R2";
/*FILENAME REPORTZ "&RPTLIB/ULWD28.LWD28RZ";*/

LIBNAME  WORKLOCL  REMOTE  SERVER=CYPRUS  SLIBREF=WORK;
RSUBMIT;

%macro sqlcheck ;
  %if  &sqlxrc ne 0  %then  %do  ;
    data _null_  ;
            file reportz notitles  ;
            put @01 " ********************************************************************* "
              / @01 " ****  The SQL code above has experienced an error.               **** "
              / @01 " ****  The SAS should be reviewed.                                **** "       
              / @01 " ********************************************************************* "
              / @01 " ****  The SQL error code is  &sqlxrc  and the SQL error message  **** "
              / @01 " ****  &sqlxmsg   **** "
              / @01 " ********************************************************************* "
            ;
         run  ;
  %end  ;
%mend  ;

PROC SQL;
CONNECT TO DB2 (DATABASE=DLGSUTWH);
	CREATE TABLE DEATHNDIS AS
		SELECT *
		FROM CONNECTION TO DB2 (
			SELECT	DISTINCT A.DF_SPE_ACC_ID
				,A.DD_DTH AS DEATH
				,A.DD_PMN_DSA AS DISABILITY
				,DATE(A.DF_LST_DTS_PD01) AS ACT_DT 
				,C.AC_LON_STA_TYP
				,C.AC_LON_STA_REA
				,C.AC_STA_GA14
			FROM	OLWHRM1.PD01_PDM_INF A
				INNER JOIN OLWHRM1.GA01_APP B
					ON B.DF_PRS_ID_BR = A.DF_PRS_ID
				INNER JOIN OLWHRM1.GA14_LON_STA C
					ON C.AF_APL_ID = B.AF_APL_ID 
					AND C.AC_STA_GA14 = 'A'

WHERE (A.DD_DTH IS NOT NULL 
		AND C.AF_APL_ID NOT IN (SELECT ZZ.AF_APL_ID FROM OLWHRM1.GA14_LON_STA ZZ
								WHERE ZZ.AC_STA_GA14 = 'A'
								AND ZZ.AC_LON_STA_REA = 'DE'
								AND (ZZ.AC_LON_STA_TYP = 'CR' OR ZZ.AC_LON_STA_TYP = 'CP')
								)
		)/*CR/DE, CP/DE */
AND (A.DD_PMN_DSA IS NOT NULL 
		AND C.AF_APL_ID NOT IN (SELECT ZZ.AF_APL_ID FROM OLWHRM1.GA14_LON_STA ZZ
								WHERE ZZ.AC_STA_GA14 = 'A'
								AND ZZ.AC_LON_STA_REA = 'DI'
								AND (ZZ.AC_LON_STA_TYP = 'CR' OR ZZ.AC_LON_STA_TYP = 'CP')
								)
		)/*CR/DI, CP/DI*/
AND C.AC_LON_STA_TYP NOT IN ('CA', 'DN', 'PC', 'PF', 'PM', 'PN')



FOR READ ONLY WITH UR


);
DISCONNECT FROM DB2;

/*%put  sqlxrc= >>> &sqlxrc <<< ||| sqlxmsg= >>> &sqlxmsg >>> ;  ** includes error messages to SAS log  ;*/
/*%sqlcheck;*/
/*quit;*/

ENDRSUBMIT;
DATA DEATHNDIS; 
	SET WORKLOCL.DEATHNDIS; 
RUN;

PROC SORT DATA=DEATHNDIS;
BY DF_SPE_ACC_ID;
RUN;

DATA _NULL_;
SET DEATHNDIS ;
LENGTH DESCRIPTION $600.;
USER = ' ';
ACT_DT = ACT_DT ;
DESCRIPTION = CATX(',',
		DF_SPE_ACC_ID,
		PUT(DEATH, MMDDYY10.),
		PUT(DISABILITY, MMDDYY10.),
		AC_LON_STA_TYP, 
		AC_LON_STA_REA
);
FILE REPORT2 DELIMITER=',' DSD DROPOVER LRECL=32767;
FORMAT USER $10. ;
FORMAT ACT_DT MMDDYY10. ;
FORMAT DESCRIPTION $600. ;
IF _N_ = 1 THEN DO;
	PUT "USER,ACT_DT,DESCRIPTION";
END;
DO;
   PUT USER $ @;
   PUT ACT_DT @;
   PUT DESCRIPTION $ ;
END;
RUN;

