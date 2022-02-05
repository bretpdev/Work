/*%LET RPTLIB = %SYSGET(reportdir);*/
%LET RPTLIB = T:\SAS;
FILENAME REPORTZ "&RPTLIB/UNWR08.NWR08RZ";
FILENAME REPORT2 "&RPTLIB/UNWR08.NWR08R2";

LIBNAME  LEGEND  REMOTE  SERVER=LEGEND SLIBREF=work  ;

DATA _NULL_;
	RUN_DAY = TODAY();
	CALL SYMPUT('MON_AGO_1_BEG',"'"||PUT(INTNX('MONTH',RUN_DAY,-1,'B'), MMDDYYD10.)||"'");
	CALL SYMPUT('MON_AGO_1_END',"'"||PUT(INTNX('MONTH',RUN_DAY,-1,'E'), MMDDYYD10.)||"'");
RUN;

%SYSLPUT MON_AGO_1_BEG = &MON_AGO_1_BEG;
%SYSLPUT MON_AGO_1_END = &MON_AGO_1_END;

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
CREATE TABLE ALL_DETAIL AS
	SELECT	*
	FROM	CONNECTION TO DB2 (
				SELECT  DISTINCT
						A.BF_SSN
						,A.LN_SEQ
						,A.LD_FAT_PST
						,A.PC_FAT_TYP
						,A.PC_FAT_SUB_TYP
						,A.LC_CSH_ADV
						,COALESCE(A.LA_FAT_CUR_PRI,0) AS LA_FAT_CUR_PRI
						,COALESCE(A.LA_FAT_NSI,0) AS LA_FAT_NSI
						,COALESCE(A.LA_FAT_MSC_FEE,0) + COALESCE(LA_FAT_LTE_FEE,0) AS FEES
						,A.LC_FAT_REV_REA
						,CASE
							WHEN B.LC_FED_PGM_YR IN ('DLO', 'LNC', 'TPL') THEN 'DL'
							ELSE 'NON-DL'
						 END AS DL_SCN
				FROM	PKUB.LN90_FIN_ATY A
						INNER JOIN PKUB.LN10_LON B
							ON A.BF_SSN = B.BF_SSN
							AND A.LN_SEQ = B.LN_SEQ
				WHERE	A.LD_FAT_PST BETWEEN &MON_AGO_1_BEG AND &MON_AGO_1_END
						AND A.LC_STA_LON90 = 'A'
				FOR READ ONLY WITH UR
			);
DISCONNECT FROM DB2;

/*This next part is a result of the requirement that principal, interest, and fee amounts for individual Compass transactions*/
/*be reported under different FMS transaction codes for some type 10 and type 80 payments.  To accomplish this, three tables*/
/*are created (PRI_DETAIL, INT_DETAIL, and FEE_DETAIL) each with a corresponding amount (principal, interest, and fees) and*/
/*FMS transaction code (ADJPRI, INTADJ, and ADJFEE).  The three tables are then unioned and the amounts summed, grouped by the*/
/*Compass transaction codes and other values.  Note, as a result of this, each Compass transaction code may correlate to*/
/*multiple FMS transaction codes.*/
/**/
/*The FMS_TYPE CASE statement in each of the following three CREATE TABLE clauses is the same except */
/*WHEN PC_FAT_TYP = '10' AND LC_CSH_ADV <> 'C' and WHEN PC_FAT_TYP = '80' AND PC_FAT_SUB_TYP = '03'.*/

CREATE TABLE PRI_DETAIL AS
	SELECT 	 BF_SSN
			,LN_SEQ
			,LD_FAT_PST
			,PC_FAT_TYP
			,PC_FAT_SUB_TYP
			,CASE
				WHEN PC_FAT_TYP = '01' THEN 'DISBUR'
				WHEN PC_FAT_TYP = '02' AND PC_FAT_SUB_TYP = '90' THEN 'DISBUR'
				WHEN PC_FAT_TYP = '02' AND PC_FAT_SUB_TYP = '91' THEN 'TROSSV'
				WHEN PC_FAT_TYP = '04' AND PC_FAT_SUB_TYP = '95' THEN 'PUTRET'
				WHEN PC_FAT_TYP = '04' AND PC_FAT_SUB_TYP = '96' THEN 'TRSVOS'
				WHEN PC_FAT_TYP = '04' AND PC_FAT_SUB_TYP = '97' THEN 'TRSVOS'
				WHEN PC_FAT_TYP = '04' AND PC_FAT_SUB_TYP = '98' THEN 'TRSVOS'
				WHEN PC_FAT_TYP = '07' THEN 'DSBADJ'
				WHEN PC_FAT_TYP = '10' AND PC_FAT_SUB_TYP = '33' AND LC_CSH_ADV = 'C' THEN 'COLLFD'
				WHEN PC_FAT_TYP = '10' AND PC_FAT_SUB_TYP IN ('40', '41', '45') AND DL_SCN = 'DL' AND LC_CSH_ADV = 'C' THEN 'DSBADC'
				WHEN PC_FAT_TYP = '10' AND PC_FAT_SUB_TYP IN ('40', '41', '45') AND DL_SCN = 'DL' AND LC_CSH_ADV <> 'C' THEN 'DSBADJ'
				WHEN PC_FAT_TYP = '10' AND PC_FAT_SUB_TYP = '73' AND LC_CSH_ADV = 'C' THEN 'COLLCC'
				WHEN PC_FAT_TYP = '10' AND PC_FAT_SUB_TYP = '81' THEN 'COLCON'
				WHEN PC_FAT_TYP = '10' AND LC_CSH_ADV = 'C' THEN 'COLLEC'
				WHEN PC_FAT_TYP = '10' AND LC_CSH_ADV <> 'C' THEN 'ADJPRI'
				WHEN PC_FAT_TYP = '14' AND PC_FAT_SUB_TYP = '01' AND DL_SCN = 'NON-DL' THEN 'ADJPRI'
				WHEN PC_FAT_TYP = '14' AND PC_FAT_SUB_TYP = '01' AND DL_SCN = 'DL' THEN 'DSBADJ'
				WHEN PC_FAT_TYP = '14' AND PC_FAT_SUB_TYP = '85' THEN 'DSBADJ'
				WHEN PC_FAT_TYP = '14' AND PC_FAT_SUB_TYP = '86' THEN 'DSBADJ'
				WHEN PC_FAT_TYP = '16' THEN 'ADJPRI'
				WHEN PC_FAT_TYP = '18' AND PC_FAT_SUB_TYP = '01' THEN 'REBYR1'
				WHEN PC_FAT_TYP = '18' AND PC_FAT_SUB_TYP = '06' THEN 'ADJPRI'
				WHEN PC_FAT_TYP = '20' THEN 'COLLEC'
				WHEN PC_FAT_TYP = '26' THEN 'FEELTE'
				WHEN PC_FAT_TYP = '36' AND LC_CSH_ADV = 'C' THEN 'DSBADC'
				WHEN PC_FAT_TYP = '36' AND LC_CSH_ADV <> 'C' THEN 'DSBADJ'
				WHEN PC_FAT_TYP = '37' AND LC_FAT_REV_REA IS NULL THEN 'REBLST'
				WHEN PC_FAT_TYP = '37' AND LC_FAT_REV_REA IS NOT NULL THEN 'REBRES'
				WHEN PC_FAT_TYP = '38' THEN 'DSBADJ'
				WHEN PC_FAT_TYP = '39' THEN 'INTADJ'
				WHEN PC_FAT_TYP = '41' AND LC_FAT_REV_REA IS NULL THEN 'REBLST'
				WHEN PC_FAT_TYP = '41' AND LC_FAT_REV_REA IS NOT NULL THEN 'REBRES'
				WHEN PC_FAT_TYP = '43' THEN 'REBCON'
				WHEN PC_FAT_TYP = '50' AND PC_FAT_SUB_TYP = '01' THEN 'INTACC'
				WHEN PC_FAT_TYP = '50' AND PC_FAT_SUB_TYP = '02' AND ABS(LA_FAT_CUR_PRI + LA_FAT_NSI) < 25 THEN 'WOSMBL'
				WHEN PC_FAT_TYP = '50' AND PC_FAT_SUB_TYP = '02' AND ABS(LA_FAT_CUR_PRI + LA_FAT_NSI) >= 25 THEN 'WRTOFF'
				WHEN PC_FAT_TYP = '53' THEN 'COLCON'
				WHEN PC_FAT_TYP = '54' THEN 'COLCON'
				WHEN PC_FAT_TYP = '60' AND PC_FAT_SUB_TYP = '01' THEN 'DISBUR'
				WHEN PC_FAT_TYP = '60' AND PC_FAT_SUB_TYP = '02' AND ABS(LA_FAT_CUR_PRI + LA_FAT_NSI) < 5 THEN 'WOSMBL'
				WHEN PC_FAT_TYP = '60' AND PC_FAT_SUB_TYP = '02' AND ABS(LA_FAT_CUR_PRI + LA_FAT_NSI) >= 5 THEN 'WRTOFF'
				WHEN PC_FAT_TYP = '60' AND PC_FAT_SUB_TYP = '03' THEN 'WOSMBL'
				WHEN PC_FAT_TYP = '70' THEN 'INTCAP'
				WHEN PC_FAT_TYP = '80' AND PC_FAT_SUB_TYP = '03' THEN 'ADJPRI'
				ELSE 'UNKNOWN'
			END AS FMS_TYPE
			,CASE 
				WHEN LC_CSH_ADV = 'C' THEN 'C'
				ELSE 'N'
			 END AS LC_CSH_ADV
			,LA_FAT_CUR_PRI
			,0 AS LA_FAT_NSI
			,0 AS FEES
			,LA_FAT_CUR_PRI AS TOTAL
	FROM 	ALL_DETAIL
;

CREATE TABLE INT_DETAIL AS
	SELECT 	 BF_SSN
			,LN_SEQ
			,LD_FAT_PST
			,PC_FAT_TYP
			,PC_FAT_SUB_TYP
			,CASE
				WHEN PC_FAT_TYP = '01' THEN 'DISBUR'
				WHEN PC_FAT_TYP = '02' AND PC_FAT_SUB_TYP = '90' THEN 'DISBUR'
				WHEN PC_FAT_TYP = '02' AND PC_FAT_SUB_TYP = '91' THEN 'TROSSV'
				WHEN PC_FAT_TYP = '04' AND PC_FAT_SUB_TYP = '95' THEN 'PUTRET'
				WHEN PC_FAT_TYP = '04' AND PC_FAT_SUB_TYP = '96' THEN 'TRSVOS'
				WHEN PC_FAT_TYP = '04' AND PC_FAT_SUB_TYP = '97' THEN 'TRSVOS'
				WHEN PC_FAT_TYP = '04' AND PC_FAT_SUB_TYP = '98' THEN 'TRSVOS'
				WHEN PC_FAT_TYP = '07' THEN 'DSBADJ'
				WHEN PC_FAT_TYP = '10' AND PC_FAT_SUB_TYP = '33' AND LC_CSH_ADV = 'C' THEN 'COLLFD'
				WHEN PC_FAT_TYP = '10' AND PC_FAT_SUB_TYP IN ('40', '41', '45') AND DL_SCN = 'DL' AND LC_CSH_ADV = 'C' THEN 'DSBADC'
				WHEN PC_FAT_TYP = '10' AND PC_FAT_SUB_TYP IN ('40', '41', '45') AND DL_SCN = 'DL' AND LC_CSH_ADV <> 'C' THEN 'DSBADJ'
				WHEN PC_FAT_TYP = '10' AND PC_FAT_SUB_TYP = '73' AND LC_CSH_ADV = 'C' THEN 'COLLCC'
				WHEN PC_FAT_TYP = '10' AND PC_FAT_SUB_TYP = '81' THEN 'COLCON'
				WHEN PC_FAT_TYP = '10' AND LC_CSH_ADV = 'C' THEN 'COLLEC'
				WHEN PC_FAT_TYP = '10' AND LC_CSH_ADV <> 'C' THEN 'INTADJ'
				WHEN PC_FAT_TYP = '14' AND PC_FAT_SUB_TYP = '01' AND DL_SCN = 'NON-DL' THEN 'ADJPRI'
				WHEN PC_FAT_TYP = '14' AND PC_FAT_SUB_TYP = '01' AND DL_SCN = 'DL' THEN 'DSBADJ'
				WHEN PC_FAT_TYP = '14' AND PC_FAT_SUB_TYP = '85' THEN 'DSBADJ'
				WHEN PC_FAT_TYP = '14' AND PC_FAT_SUB_TYP = '86' THEN 'DSBADJ'
				WHEN PC_FAT_TYP = '16' THEN 'ADJPRI'
				WHEN PC_FAT_TYP = '18' AND PC_FAT_SUB_TYP = '01' THEN 'REBYR1'
				WHEN PC_FAT_TYP = '18' AND PC_FAT_SUB_TYP = '06' THEN 'ADJPRI'
				WHEN PC_FAT_TYP = '20' THEN 'COLLEC'
				WHEN PC_FAT_TYP = '26' THEN 'FEELTE'
				WHEN PC_FAT_TYP = '36' AND LC_CSH_ADV = 'C' THEN 'DSBADC'
				WHEN PC_FAT_TYP = '36' AND LC_CSH_ADV <> 'C' THEN 'DSBADJ'
				WHEN PC_FAT_TYP = '37' AND LC_FAT_REV_REA IS NULL THEN 'REBLST'
				WHEN PC_FAT_TYP = '37' AND LC_FAT_REV_REA IS NOT NULL THEN 'REBRES'
				WHEN PC_FAT_TYP = '38' THEN 'DSBADJ'
				WHEN PC_FAT_TYP = '39' THEN 'INTADJ'
				WHEN PC_FAT_TYP = '41' AND LC_FAT_REV_REA IS NULL THEN 'REBLST'
				WHEN PC_FAT_TYP = '41' AND LC_FAT_REV_REA IS NOT NULL THEN 'REBRES'
				WHEN PC_FAT_TYP = '43' THEN 'REBCON'
				WHEN PC_FAT_TYP = '50' AND PC_FAT_SUB_TYP = '01' THEN 'INTACC'
				WHEN PC_FAT_TYP = '50' AND PC_FAT_SUB_TYP = '02' AND ABS(LA_FAT_CUR_PRI + LA_FAT_NSI) < 25 THEN 'WOSMBL'
				WHEN PC_FAT_TYP = '50' AND PC_FAT_SUB_TYP = '02' AND ABS(LA_FAT_CUR_PRI + LA_FAT_NSI) >= 25 THEN 'WRTOFF'
				WHEN PC_FAT_TYP = '53' THEN 'COLCON'
				WHEN PC_FAT_TYP = '54' THEN 'COLCON'
				WHEN PC_FAT_TYP = '60' AND PC_FAT_SUB_TYP = '01' THEN 'DISBUR'
				WHEN PC_FAT_TYP = '60' AND PC_FAT_SUB_TYP = '02' AND ABS(LA_FAT_CUR_PRI + LA_FAT_NSI) < 5 THEN 'WOSMBL'
				WHEN PC_FAT_TYP = '60' AND PC_FAT_SUB_TYP = '02' AND ABS(LA_FAT_CUR_PRI + LA_FAT_NSI) >= 5 THEN 'WRTOFF'
				WHEN PC_FAT_TYP = '60' AND PC_FAT_SUB_TYP = '03' THEN 'WOSMBL'
				WHEN PC_FAT_TYP = '70' THEN 'INTCAP'
				WHEN PC_FAT_TYP = '80' AND PC_FAT_SUB_TYP = '03' THEN 'INTADJ'
				ELSE 'UNKNOWN'
			END AS FMS_TYPE
			,CASE 
				WHEN LC_CSH_ADV = 'C' THEN 'C'
				ELSE 'N'
			 END AS LC_CSH_ADV
			,0 AS LA_FAT_CUR_PRI
			,LA_FAT_NSI
			,0 AS FEES
			,LA_FAT_NSI AS TOTAL
	FROM 	ALL_DETAIL
;

CREATE TABLE FEE_DETAIL AS
	SELECT 	 BF_SSN
			,LN_SEQ
			,LD_FAT_PST
			,PC_FAT_TYP
			,PC_FAT_SUB_TYP
			,CASE
				WHEN PC_FAT_TYP = '01' THEN 'DISBUR'
				WHEN PC_FAT_TYP = '02' AND PC_FAT_SUB_TYP = '90' THEN 'DISBUR'
				WHEN PC_FAT_TYP = '02' AND PC_FAT_SUB_TYP = '91' THEN 'TROSSV'
				WHEN PC_FAT_TYP = '04' AND PC_FAT_SUB_TYP = '95' THEN 'PUTRET'
				WHEN PC_FAT_TYP = '04' AND PC_FAT_SUB_TYP = '96' THEN 'TRSVOS'
				WHEN PC_FAT_TYP = '04' AND PC_FAT_SUB_TYP = '97' THEN 'TRSVOS'
				WHEN PC_FAT_TYP = '04' AND PC_FAT_SUB_TYP = '98' THEN 'TRSVOS'
				WHEN PC_FAT_TYP = '07' THEN 'DSBADJ'
				WHEN PC_FAT_TYP = '10' AND PC_FAT_SUB_TYP = '33' AND LC_CSH_ADV = 'C' THEN 'COLLFD'
				WHEN PC_FAT_TYP = '10' AND PC_FAT_SUB_TYP IN ('40', '41', '45') AND DL_SCN = 'DL' AND LC_CSH_ADV = 'C' THEN 'DSBADC'
				WHEN PC_FAT_TYP = '10' AND PC_FAT_SUB_TYP IN ('40', '41', '45') AND DL_SCN = 'DL' AND LC_CSH_ADV <> 'C' THEN 'DSBADJ'
				WHEN PC_FAT_TYP = '10' AND PC_FAT_SUB_TYP = '73' AND LC_CSH_ADV = 'C' THEN 'COLLCC'
				WHEN PC_FAT_TYP = '10' AND PC_FAT_SUB_TYP = '81' THEN 'COLCON'
				WHEN PC_FAT_TYP = '10' AND LC_CSH_ADV = 'C' THEN 'COLLEC'
				WHEN PC_FAT_TYP = '10' AND LC_CSH_ADV <> 'C' THEN 'ADJFEE'
				WHEN PC_FAT_TYP = '14' AND PC_FAT_SUB_TYP = '01' AND DL_SCN = 'NON-DL' THEN 'ADJPRI'
				WHEN PC_FAT_TYP = '14' AND PC_FAT_SUB_TYP = '01' AND DL_SCN = 'DL' THEN 'DSBADJ'
				WHEN PC_FAT_TYP = '14' AND PC_FAT_SUB_TYP = '85' THEN 'DSBADJ'
				WHEN PC_FAT_TYP = '14' AND PC_FAT_SUB_TYP = '86' THEN 'DSBADJ'
				WHEN PC_FAT_TYP = '16' THEN 'ADJPRI'
				WHEN PC_FAT_TYP = '18' AND PC_FAT_SUB_TYP = '01' THEN 'REBYR1'
				WHEN PC_FAT_TYP = '18' AND PC_FAT_SUB_TYP = '06' THEN 'ADJPRI'
				WHEN PC_FAT_TYP = '20' THEN 'COLLEC'
				WHEN PC_FAT_TYP = '26' THEN 'FEELTE'
				WHEN PC_FAT_TYP = '36' AND LC_CSH_ADV = 'C' THEN 'DSBADC'
				WHEN PC_FAT_TYP = '36' AND LC_CSH_ADV <> 'C' THEN 'DSBADJ'
				WHEN PC_FAT_TYP = '37' AND LC_FAT_REV_REA IS NULL THEN 'REBLST'
				WHEN PC_FAT_TYP = '37' AND LC_FAT_REV_REA IS NOT NULL THEN 'REBRES'
				WHEN PC_FAT_TYP = '38' THEN 'DSBADJ'
				WHEN PC_FAT_TYP = '39' THEN 'INTADJ'
				WHEN PC_FAT_TYP = '41' AND LC_FAT_REV_REA IS NULL THEN 'REBLST'
				WHEN PC_FAT_TYP = '41' AND LC_FAT_REV_REA IS NOT NULL THEN 'REBRES'
				WHEN PC_FAT_TYP = '43' THEN 'REBCON'
				WHEN PC_FAT_TYP = '50' AND PC_FAT_SUB_TYP = '01' THEN 'INTACC'
				WHEN PC_FAT_TYP = '50' AND PC_FAT_SUB_TYP = '02' AND ABS(LA_FAT_CUR_PRI + LA_FAT_NSI) < 25 THEN 'WOSMBL'
				WHEN PC_FAT_TYP = '50' AND PC_FAT_SUB_TYP = '02' AND ABS(LA_FAT_CUR_PRI + LA_FAT_NSI) >= 25 THEN 'WRTOFF'
				WHEN PC_FAT_TYP = '53' THEN 'COLCON'
				WHEN PC_FAT_TYP = '54' THEN 'COLCON'
				WHEN PC_FAT_TYP = '60' AND PC_FAT_SUB_TYP = '01' THEN 'DISBUR'
				WHEN PC_FAT_TYP = '60' AND PC_FAT_SUB_TYP = '02' AND ABS(LA_FAT_CUR_PRI + LA_FAT_NSI) < 5 THEN 'WOSMBL'
				WHEN PC_FAT_TYP = '60' AND PC_FAT_SUB_TYP = '02' AND ABS(LA_FAT_CUR_PRI + LA_FAT_NSI) >= 5 THEN 'WRTOFF'
				WHEN PC_FAT_TYP = '60' AND PC_FAT_SUB_TYP = '03' THEN 'WOSMBL'
				WHEN PC_FAT_TYP = '70' THEN 'INTCAP'
				WHEN PC_FAT_TYP = '80' AND PC_FAT_SUB_TYP = '03' THEN 'ADJFEE'
				ELSE 'UNKNOWN'
			END AS FMS_TYPE
			,CASE 
				WHEN LC_CSH_ADV = 'C' THEN 'C'
				ELSE 'N'
			 END AS LC_CSH_ADV
			,0 AS LA_FAT_CUR_PRI
			,0 AS LA_FAT_NSI
			,FEES
			,FEES AS TOTAL
	FROM 	ALL_DETAIL
;

CREATE TABLE FIN_ACTIVITY AS
	SELECT	PC_FAT_TYP
			,PC_FAT_SUB_TYP
			,FMS_TYPE
			,LC_CSH_ADV
			,SUM(LA_FAT_CUR_PRI) AS LA_FAT_CUR_PRI
			,SUM(LA_FAT_NSI) AS LA_FAT_NSI
			,SUM(FEES) AS FEES
			,SUM(TOTAL) AS TOTAL
	FROM	(SELECT	* FROM PRI_DETAIL
			 UNION ALL 
			 SELECT * FROM INT_DETAIL 
			 UNION ALL
			 SELECT * FROM FEE_DETAIL)
	GROUP BY PC_FAT_TYP
			,PC_FAT_SUB_TYP
			,FMS_TYPE
			,LC_CSH_ADV
;


/*%PUT  SQLXRC= >>> &SQLXRC <<< ||| SQLXMSG= >>> &SQLXMSG >>> ;  ** INCLUDES ERROR MESSAGES TO SAS LOG  ;*/
/*%SQLCHECK;*/
QUIT;

ENDRSUBMIT;
DATA FIN_ACTIVITY; SET LEGEND.FIN_ACTIVITY; RUN;
/*DATA ALL_DETAIL; SET LEGEND.ALL_DETAIL; RUN;*/
/*DATA PRI_DETAIL; SET LEGEND.PRI_DETAIL; RUN;*/
/*DATA INT_DETAIL; SET LEGEND.INT_DETAIL; RUN;*/
/*DATA FEE_DETAIL; SET LEGEND.FEE_DETAIL; RUN;*/


/*create printed report*/
PROC PRINTTO PRINT=REPORT2 NEW; RUN;
OPTIONS ORIENTATION = LANDSCAPE;
OPTIONS PS=39 LS=127;
TITLE 		'Financial Activity by Servicer Tran Code';
TITLE2		"Date: For Month Ending &MON_AGO_1_END";
FOOTNOTE1  	"THIS DOCUMENT MAY CONTAIN BORROWERS' SENSITIVE INFORMATION THAT UHEAA HAS PLEDGED TO PROTECT.";
FOOTNOTE2	'PLEASE TAKE APPROPRIATE PRECAUTIONS TO SAFEGUARD THIS INFORMATION.';
FOOTNOTE3	;
FOOTNOTE4   'JOB = UTNWR08  	 REPORT = UNWR08.NWR08R2';

PROC PRINT NOOBS SPLIT='/' DATA=FIN_ACTIVITY WIDTH=UNIFORM WIDTH=MIN LABEL;
SUM		LA_FAT_CUR_PRI
		LA_FAT_NSI
		FEES
		TOTAL;
VAR 	PC_FAT_TYP
		PC_FAT_SUB_TYP
		FMS_TYPE
		LC_CSH_ADV
		LA_FAT_CUR_PRI
		LA_FAT_NSI
		FEES
		TOTAL;
LABEL	PC_FAT_TYP = 'Type'
		PC_FAT_SUB_TYP = 'Sub-Type'
		FMS_TYPE = 'FMS Tran Type'
		LC_CSH_ADV = 'Cash/Non-Cash'
		LA_FAT_CUR_PRI = 'Principal'
		LA_FAT_NSI = 'Interest'
		FEES = 'Fees'
		TOTAL = 'Total Amount';
FORMAT	LA_FAT_CUR_PRI COMMA15.2
		LA_FAT_NSI COMMA15.2
		FEES COMMA15.2
		TOTAL COMMA15.2;
RUN;

PROC PRINTTO;
RUN;


/*sum data to the FMS_TYPE level for testing*/
PROC SQL;
CREATE TABLE RECONCILE AS
	SELECT	FMS_TYPE
			,SUM(LA_FAT_CUR_PRI) AS LA_FAT_CUR_PRI
			,SUM(LA_FAT_NSI) AS LA_FAT_NSI
			,SUM(FEES) AS FEES
	FROM 	FIN_ACTIVITY
	GROUP BY FMS_TYPE
	;
QUIT;