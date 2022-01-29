/*CRI Audit Files - Current Lender*/

%LET RPTLIB = T:\SAS;
LIBNAME PROGREVW 'Q:\Support Services\Test Files\SAS\PROGREVW';

/*BANA LENDER IDS & PURCHASE DATE ADDED TO LOCAL PERMANENT DATA SET FOR USE IN SCRIPT LINES 688-699 BELOW*/
/*DATA PROGREVW.LOAN_PURCHASE_DATE;	*/
/*INPUT LOAN_PURCHASE_DATE_ID @4 LENDER_ID $ @13 PURCHASE_DATE ;*/
/*	FORMAT LENDER_ID $8. PURCHASE_DATE DATE9.;*/
/*	INFORMAT LENDER_ID $8. PURCHASE_DATE DATE9.;*/
/*DATALINES;*/
/*0  abcdefg  01JAN2000*/
/*1  830248   27FEB2014*/
/*2  826717   27FEB2014*/
/*3  829769   23FEB2015*/
/*4  82976901 23FEB2015*/
/*5  82976902 23FEB2015*/
/*6  82976903 23FEB2015*/
/*7  82976904 23FEB2015*/
/*8  82976905 23FEB2015*/
/*9  82976906 23FEB2015*/
/*10 82976907 23FEB2015*/
/*11 82976908 23FEB2015*/
/*;*/

FILENAME REPORT2 "&RPTLIB/CRI_Audit_Files_-_Current_Lender.csv";

OPTIONS SYMBOLGEN NOCENTER NODATE NONUMBER LS=132;

/*Also modify the list of lender ID's for each run.  The requester will*/
/*tell you which lender ID's they need.*/
DATA _NULL_;            
      CALL SYMPUT('BEGIN', "'01/01/2015'" );
      CALL SYMPUT('END',"'10/07/2016'");
RUN;

%LET LENDERS = '829769';

%SYSLPUT BEGIN = &BEGIN;
%SYSLPUT END = &END;
%SYSLPUT LENDERS = &LENDERS;
LIBNAME  WORKLOCL  REMOTE  SERVER=DUSTER  SLIBREF=WORK;
RSUBMIT;

PROC SQL;
	CONNECT TO DB2 (DATABASE=DLGSUTWH);
	CREATE TABLE DEMO_LON AS
		SELECT *
		FROM CONNECTION TO DB2 
			(
				SELECT DISTINCT
					LN10.BF_SSN
					,LN10.LF_LON_CUR_OWN
					,LOAN_SOLD.LD_FAT_EFF
				FROM 
					OLWHRM1.PD10_PRS_NME PD10
					INNER JOIN OLWHRM1.LN10_LON LN10
						ON PD10.DF_PRS_ID = LN10.BF_SSN
						AND SUBSTR(LN10.LF_LON_CUR_OWN,1,6) = &LENDERS
/*						AND LN10.LF_LON_CUR_OWN LIKE '829769%'*/
						AND LN10.LC_STA_LON10 = 'R'
					LEFT JOIN
						(
							SELECT DISTINCT
								LN90.BF_SSN
								,LN90.LN_SEQ
								,LN90.LD_FAT_EFF
							FROM 
								OLWHRM1.LN90_FIN_ATY LN90
							WHERE 
								LN90.PC_FAT_TYP = '03'
								AND LN90.PC_FAT_SUB_TYP = '95'
						) LOAN_SOLD
						ON LN10.BF_SSN = LOAN_SOLD.BF_SSN
						AND LN10.LN_SEQ = LOAN_SOLD.LN_SEQ		
						AND LN10.LF_LON_CUR_OWN IN ('826717','830248')

				FETCH FIRST 10 ROWS ONLY
			)
	;
QUIT;
ENDRSUBMIT;

DATA _DEMO; 
	SET DUSTER.DEMO_LON; 
RUN;

PROC SORT DATA=_DEMO; 
	BY BF_SSN LN_SEQ;
RUN;

/*GET LOAN PURCHASE DATE FROM LOCAL PERMANENT DATA SET BASED ON LENDER ID*/
PROC SQL;
	CREATE TABLE DEMO AS
		SELECT
			_DEMO.*
			,COALESCE(LP.PURCHASE_DATE,'31DEC9999'D) AS LOAN_PURCHASE_DATE
		FROM
			_DEMO
			LEFT JOIN PROGREVW.LOAN_PURCHASE_DATE LP
				ON _DEMO.LF_LON_CUR_OWN = LP.LENDER_ID
	;
QUIT;
