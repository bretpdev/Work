/*UTLWR11 - PLUSGB Endorsement Field Setting Cleanup*/
/*LIBNAME DLGSUTWH DB2 DATABASE=DLGSUTWH OWNER=OLWHRM1;*/
/*%LET RPTLIB = %SYSGET(reportdir);*/
%LET RPTLIB = T:\SAS;
FILENAME REPORTZ "&RPTLIB/ULWR11.LWR11RZ";
FILENAME REPORT2 "&RPTLIB/ULWR11.LWR11R2";
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
CREATE TABLE QUERY AS
SELECT *
FROM CONNECTION TO DB2 (
SELECT DISTINCT
	A.IF_DOE_SCL,
	B.IM_SCL_FUL,
	A.IC_LON_PGM,
	A.IC_CHK_EDS_TYP,
	A.IC_COP_TYP,
	A.IF_LST_USR_SC20,
	A.IC_STA_SCL_GTR,
	DATE(A.IF_LST_DTS_SC20) AS IF_LST_DTS_SC20,
	B.IC_CUR_SCL_STA,
	C.IC_SLG_DSB_PRC AS SC23_DSB_PRC,
	D.IC_SCL_DSB_PRC AS SC11_DSB_PRC
	
FROM OLWHRM1.SC20_SCH_GTR A
LEFT OUTER JOIN OLWHRM1.SC10_SCH_DMO B
	ON A.IF_DOE_SCL = B.IF_DOE_SCL
LEFT OUTER JOIN OLWHRM1.SC23_SLG_LO_PAR C
	ON C.IF_DOE_SCL = A.IF_DOE_SCL
LEFT OUTER JOIN OLWHRM1.SC11_SCL_LO_PAR D
	ON D.IF_DOE_SCL = A.IF_DOE_SCL
WHERE A.IC_LON_PGM IN ('PLUS','PLUSGB','STFFRD','UNSTFD')
	AND A.IC_CHK_EDS_TYP <> 'M'
	AND A.IC_COP_TYP = 'S'
	AND A.IF_GTR = '000749'
FOR READ ONLY WITH UR
);
DISCONNECT FROM DB2;
/*%put  sqlxrc= >>> &sqlxrc <<< ||| sqlxmsg= >>> &sqlxmsg >>> ;  ** includes error messages to SAS log  ;*/
/*%sqlcheck;*/
/*quit;*/
ENDRSUBMIT;
DATA QUERY;
	SET WORKLOCL.QUERY;
RUN;



PROC SORT DATA=QUERY(WHERE=(
	IC_STA_SCL_GTR IN ('D','I') OR IC_CUR_SCL_STA = 'C' OR SC23_DSB_PRC = '' OR SC11_DSB_PRC = ''
	)) OUT=Q NODUPKEY;
BY IF_DOE_SCL;
RUN;
PROC SQL;
	CREATE TABLE QRES AS
		SELECT DISTINCT A.IF_DOE_SCL
			,A.IM_SCL_FUL
			,A.IC_LON_PGM
			,A.IC_CHK_EDS_TYP
			,A.IF_LST_USR_SC20
			,A.IF_LST_DTS_SC20

		FROM QUERY A
		WHERE NOT EXISTS 
			(
				SELECT 1
				FROM Q
				WHERE Q.IF_DOE_SCL = A.IF_DOE_SCL
			)
		ORDER BY IF_DOE_SCL, 
			IC_LON_PGM
		;
QUIT;

DATA _NULL_;
SET QRES ;
LENGTH DESCRIPTION $600.;
USER = IF_LST_USR_SC20;
ACT_DT = IF_LST_DTS_SC20;
DESCRIPTION = CATX(',',
	IF_DOE_SCL,
	IM_SCL_FUL,
	IC_LON_PGM,
	IC_CHK_EDS_TYP
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
