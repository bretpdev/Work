/*UTLWU18 - 1029 Transaction Non-PLUS Loan - QC Report*/
/*LIBNAME DLGSUTWH DB2 DATABASE=DLGSUTWH OWNER=OLWHRM1;*/
/*%LET RPTLIB = %SYSGET(reportdir);*/
%LET RPTLIB = T:\SAS;
FILENAME REPORTZ "&RPTLIB/ULWU18.LWU18RZ";
FILENAME REPORT2 "&RPTLIB/ULWU18.LWU18R2";
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
CREATE TABLE DEMO AS
SELECT *
FROM CONNECTION TO DB2 (
SELECT B.DF_SPE_ACC_ID,
	A.LN_SEQ,
	B.DM_PRS_LST,
	C.LD_FAT_EFF,
	ABS(C.LA_FAT_CUR_PRI)	AS LA_FAT_CUR_PRI,
	A.LF_LON_CUR_OWN,
	AY10.PF_REQ_ACT,
	AY10.LD_ATY_REQ_RCV,
	AY10.CMT_TXT
FROM OLWHRM1.LN10_LON A
INNER JOIN OLWHRM1.PD10_PRS_NME B
	ON A.BF_SSN = B.DF_PRS_ID
INNER JOIN OLWHRM1.LN90_FIN_ATY C
	ON A.BF_SSN = C.BF_SSN
	AND A.LN_SEQ = C.LN_SEQ
LEFT OUTER JOIN (
	SELECT A10.BF_SSN
		,A10.PF_REQ_ACT
		,A10.LD_ATY_REQ_RCV
		,SUBSTR(A20.LX_ATY,41,6) AS CMT_TXT
	FROM OLWHRM1.AY10_BR_LON_ATY A10
	INNER JOIN OLWHRM1.LN85_LON_ATY L85
		ON A10.BF_SSN = L85.BF_SSN
		AND A10.LN_ATY_SEQ = L85.LN_ATY_SEQ
	INNER JOIN OLWHRM1.AY15_ATY_CMT A15
		ON A10.BF_SSN = A15.BF_SSN
		AND A10.LN_ATY_SEQ = A15.LN_ATY_SEQ
	INNER JOIN OLWHRM1.AY20_ATY_TXT A20
		ON A15.BF_SSN = A20.BF_SSN
		AND A15.LN_ATY_SEQ = A20.LN_ATY_SEQ
		AND A15.LN_ATY_CMT_SEQ = A20.LN_ATY_CMT_SEQ
	WHERE A10.PF_REQ_ACT = 'U1029'
		AND A10.LC_STA_ACTY10 = 'A'
	) AY10
	ON A.BF_SSN = AY10.BF_SSN
WHERE A.LA_CUR_PRI > 0
	AND A.LC_STA_LON10 = 'R'
	AND A.IC_LON_PGM <> 'PLUS'
	AND C.PC_FAT_TYP = '10'
	AND C.PC_FAT_SUB_TYP = '29'
	AND C.LC_STA_LON90 = 'A'
	AND C.LC_FAT_REV_REA = ''
FOR READ ONLY WITH UR
);
DISCONNECT FROM DB2;
/*%put  sqlxrc= >>> &sqlxrc <<< ||| sqlxmsg= >>> &sqlxmsg >>> ;  ** includes error messages to SAS log  ;*/
/*%sqlcheck;*/
/*quit;*/
ENDRSUBMIT;
DATA DEMO;
SET WORKLOCL.DEMO;
RUN;
PROC SORT DATA=DEMO OUT=XOUT (
	WHERE=(
		CMT_TXT = 'NELNET' | LD_FAT_EFF = LD_ATY_REQ_RCV
		)
	);
	BY DF_SPE_ACC_ID LN_SEQ;
RUN;
PROC SQL;
	CREATE TABLE TDEMO AS
		SELECT DISTINCT 
			A.DF_SPE_ACC_ID,
			A.LN_SEQ,
			A.DM_PRS_LST,
			A.LD_FAT_EFF,
			A.LA_FAT_CUR_PRI,
			A.LF_LON_CUR_OWN
		FROM DEMO A
		WHERE NOT EXISTS 
		(
			SELECT 1
			FROM XOUT X
			WHERE X.DF_SPE_ACC_ID = A.DF_SPE_ACC_ID
				 AND X.LN_SEQ = A.LN_SEQ
		)
	;
QUIT;
PROC SORT DATA=TDEMO OUT=DEMO;
BY DF_SPE_ACC_ID;
RUN;

DATA _NULL_;
SET DEMO ;
LENGTH DESCRIPTION $600.;
USER = '';
ACT_DT = LD_FAT_EFF;
DESCRIPTION = CATX(',',
	'BORROWER ACCOUNT NUMBER = '||DF_SPE_ACC_ID,
	'LOAN SEQUENCE NUMBER = '||LN_SEQ,
	'BORROWER LAST NAME = '||DM_PRS_LST,   
	'EFFECTIVE DATE OF 1029 = '||(PUT(LD_FAT_EFF,MMDDYY10.)),
	'AMOUNT	OF 1029 = '|| LA_FAT_CUR_PRI,
	'CURRENT OWNER = '|| LF_LON_CUR_OWN
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