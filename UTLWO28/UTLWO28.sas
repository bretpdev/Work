/*LIBNAME DLGSUTWH DB2 DATABASE=DLGSUTWH OWNER=OLWHRM1;*/
/*%LET RPTLIB = %SYSGET(reportdir);*/
%LET RPTLIB = T:\SAS;
FILENAME REPORT2 "&RPTLIB/ULWO28.LWO28R2";
FILENAME REPORT3 "&RPTLIB/ULWO28.LWO28R3";
FILENAME REPORT4 "&RPTLIB/ULWO28.LWO28R4";
FILENAME REPORTZ "&RPTLIB/ULWO28.LWO28RZ";

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
CREATE TABLE DDB AS
SELECT *
FROM CONNECTION TO DB2 (
SELECT DISTINCT 
	B.DF_SPE_ACC_ID
	,A.WC_DW_LON_STA AS STATUS
	,B.DM_PRS_1
	,B.DM_PRS_LST 
	,C.STA_DATE
	,D.LF_USR_CRT_REQ_FOR
FROM OLWHRM1.DW01_DW_CLC_CLU A
INNER JOIN OLWHRM1.PD10_PRS_NME B 
	ON A.BF_SSN = B.DF_PRS_ID
INNER JOIN (SELECT DF_PRS_ID
				,DD_DTH_STA	AS STA_DATE
		FROM OLWHRM1.PD21_GTR_DTH
	UNION SELECT DF_PRS_ID
				,DD_DSA_STA	AS STA_DATE
		FROM OLWHRM1.PD23_GTR_DSA
	UNION SELECT DF_PRS_ID
				,DD_BKR_STA	AS STA_DATE
		FROM OLWHRM1.PD24_PRS_BKR
) C
	ON C.DF_PRS_ID = A.BF_SSN
INNER JOIN OLWHRM1.FB10_BR_FOR_REQ D
	ON A.BF_SSN = D.BF_SSN
LEFT OUTER JOIN OLWHRM1.AY10_BR_LON_ATY E
	ON A.BF_SSN = E.BF_SSN
	AND PF_REQ_ACT = 'DBBOR'
	AND LD_ATY_REQ_RCV >= C.STA_DATE 
WHERE A.WC_DW_LON_STA IN ('16','18','20')
	AND D.LC_FOR_TYP IN ('10','13','14')
	AND D.LF_USR_CRT_REQ_FOR != ''
	AND E.BF_SSN IS NULL
);
DISCONNECT FROM DB2;
/*%put  sqlxrc= >>> &sqlxrc <<< ||| sqlxmsg= >>> &sqlxmsg >>> ;  ** includes error messages to SAS log  ;*/
/*%sqlcheck;*/
quit;
ENDRSUBMIT;
DATA DDB;SET WORKLOCL.DDB;RUN;
PROC SORT DATA=DDB;
BY DF_SPE_ACC_ID;
RUN;
%MACRO DDB(R,ALLEGED);
DATA _NULL_;
SET DDB ;
WHERE STATUS = &ALLEGED;
LENGTH DESCRIPTION $600.;
USER = LF_USR_CRT_REQ_FOR;
ACT_DT = STA_DATE;
DESCRIPTION = CATX(',',
	'ACCOUNT NUMBER = ' || DF_SPE_ACC_ID,
	'FIRST NAME = '||DM_PRS_1,
	'LAST NAME = '||DM_PRS_LST,
	'# DAYS IN STATUS = '||LEFT((TODAY() - STA_DATE))
);
FILE REPORT&R DELIMITER=',' DSD DROPOVER LRECL=32767;
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
%MEND;
%DDB(2,'16');
%DDB(3,'18');
%DDB(4,'20');
