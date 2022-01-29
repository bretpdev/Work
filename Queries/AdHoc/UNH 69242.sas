/*TODO:  */
/*1. delete non-data lines (leave header row) from top and bottom of Excel file provided*/
/*2. replace spaces in column headers with an underscore*/
/*3. rename the data sheet "SHEET1"*/
/*4. save the file*/
/*5. update the name of the file on line 9 below*/
/*6. import the csv created into excel*/

PROC IMPORT OUT= WORK.DATA
            DATAFILE= "T:\UNH 69242 IN.xlsx" 
            DBMS=EXCEL REPLACE;
     RANGE="Sheet1$"; 
     GETNAMES=YES;
     MIXED=NO;
     SCANTEXT=YES;
     USEDATE=YES;
     SCANTIME=YES;
RUN;

DATA DATA;
SET DATA;
AF_APL_ID = substr(Unique_ID,1,17);
AF_APL_ID_SFX = substr(Unique_ID,18,2);
RUN;


LIBNAME  DUSTER  REMOTE  SERVER=DUSTER SLIBREF=WORK;
DATA DUSTER.DATA; *Send data to Duster;
SET DATA;
RUN;

RSUBMIT DUSTER;
LIBNAME OLWHRM1 DB2 DATABASE=DLGSUTWH OWNER=OLWHRM1;
PROC SQL;
	CREATE TABLE POP AS
		SELECT DISTINCT
			D.*,
			GA11.AF_APL_ID AS A,
			GA10.AC_LON_TYP,
			GA11.AD_DSB_ADJ,
			GA01.AC_ACA_GDE_LEV
		FROM
			 DATA D
			 LEFT JOIN OLWHRM1.GA10_LON_APP GA10
			 	ON GA10.AF_APL_ID = D.AF_APL_ID
				AND GA10.AF_APL_ID_SFX = D.AF_APL_ID_SFX 
			 LEFT JOIN 
			 (
			 	SELECT
					AF_APL_ID,
					AF_APL_ID_SFX,
					MIN(AD_DSB_ADJ) AS AD_DSB_ADJ
				FROM
					OLWHRM1.GA11_LON_DSB_ATY GA11
				WHERE
					GA11.AN_DSB_SEQ = 1
					AND GA11.AC_DSB_ADJ = 'A'
				GROUP BY
					AF_APL_ID
			 )GA11
			 	ON GA11.AF_APL_ID = D.AF_APL_ID
				AND GA11.AF_APL_ID_SFX = D.AF_APL_ID_SFX
			LEFT JOIN OLWHRM1.GA01_APP GA01
				ON GA01.AF_APL_ID = D.AF_APL_ID
		
;
QUIT;
ENDRSUBMIT;

DATA POP;
SET DUSTER.POP;
RUN;

PROC SQL;
CREATE TABLE OUTPUT AS 
SELECT
	GA_Code,
	SSN,
	Unique_ID,
	CASE 
		WHEN AD_DSB_ADJ < INPUT('10/01/1991', MMDDYY10.) THEN '1000'
		WHEN AD_DSB_ADJ BETWEEN INPUT('10/01/1991', MMDDYY10.) AND INPUT('09/30/1992', MMDDYY10.)  THEN '1992'
		WHEN AD_DSB_ADJ BETWEEN INPUT('10/01/1992', MMDDYY10.) AND INPUT('09/30/1993', MMDDYY10.)  THEN '1993'
		WHEN AD_DSB_ADJ BETWEEN INPUT('10/01/1993', MMDDYY10.) AND INPUT('09/30/1994', MMDDYY10.)  THEN '1994'
		WHEN AD_DSB_ADJ BETWEEN INPUT('10/01/1994', MMDDYY10.) AND INPUT('09/30/1995', MMDDYY10.)  THEN '1995'
		WHEN AD_DSB_ADJ BETWEEN INPUT('10/01/1995', MMDDYY10.) AND INPUT('09/30/1996', MMDDYY10.)  THEN '1996'
		WHEN AD_DSB_ADJ BETWEEN INPUT('10/01/1996', MMDDYY10.) AND INPUT('09/30/1997', MMDDYY10.)  THEN '1997'
		WHEN AD_DSB_ADJ BETWEEN INPUT('10/01/1997', MMDDYY10.) AND INPUT('09/30/1998', MMDDYY10.)  THEN '1998'
		WHEN AD_DSB_ADJ BETWEEN INPUT('10/01/1998', MMDDYY10.) AND INPUT('09/30/1999', MMDDYY10.)  THEN '1999'
		WHEN AD_DSB_ADJ BETWEEN INPUT('10/01/1999', MMDDYY10.) AND INPUT('09/30/2000', MMDDYY10.)  THEN '2000'
		WHEN AD_DSB_ADJ BETWEEN INPUT('10/01/2000', MMDDYY10.) AND INPUT('09/30/2001', MMDDYY10.)  THEN '2001'
		WHEN AD_DSB_ADJ BETWEEN INPUT('10/01/2001', MMDDYY10.) AND INPUT('09/30/2002', MMDDYY10.)  THEN '2002'
		WHEN AD_DSB_ADJ BETWEEN INPUT('10/01/2002', MMDDYY10.) AND INPUT('09/30/2003', MMDDYY10.)  THEN '2003'
		WHEN AD_DSB_ADJ BETWEEN INPUT('10/01/2003', MMDDYY10.) AND INPUT('09/30/2004', MMDDYY10.)  THEN '2004'
		WHEN AD_DSB_ADJ BETWEEN INPUT('10/01/2004', MMDDYY10.) AND INPUT('09/30/2005', MMDDYY10.)  THEN '2005'
		WHEN AD_DSB_ADJ BETWEEN INPUT('10/01/2005', MMDDYY10.) AND INPUT('09/30/2006', MMDDYY10.)  THEN '2006'
		WHEN AD_DSB_ADJ BETWEEN INPUT('10/01/2006', MMDDYY10.) AND INPUT('09/30/2007', MMDDYY10.)  THEN '2007'
		WHEN AD_DSB_ADJ BETWEEN INPUT('10/01/2007', MMDDYY10.) AND INPUT('09/30/2008', MMDDYY10.)  THEN '2008'
		WHEN AD_DSB_ADJ BETWEEN INPUT('10/01/2008', MMDDYY10.) AND INPUT('09/30/2009', MMDDYY10.)  THEN '2009'
		WHEN AD_DSB_ADJ BETWEEN INPUT('10/01/2009', MMDDYY10.) AND INPUT('09/30/2010', MMDDYY10.)  THEN '2010'
		ELSE 'N/A' 
	END AS Cohort_Year_Dist,
	CASE
		WHEN AC_LON_TYP = 'CL' THEN '6'
		WHEN AC_LON_TYP = 'GB' THEN '4'
		WHEN AC_LON_TYP = 'PL' THEN '4'
		WHEN AC_LON_TYP = 'SF' THEN '1'
		WHEN AC_LON_TYP = 'SL' THEN '3'
		WHEN AC_LON_TYP = 'SU' THEN '2'
		ELSE 'N/A'
	END AS Loan_Type,
	CASE
		WHEN AC_LON_TYP = 'SF' AND AC_ACA_GDE_LEV IN ('1','2') THEN '1'
		WHEN AC_LON_TYP = 'SF' AND AC_ACA_GDE_LEV IN ('3','4', '5','A', 'B', 'C', 'D') THEN '3' 
		WHEN AC_LON_TYP = 'SU' THEN '5'
		WHEN AC_LON_TYP = 'CL' THEN '6'
		WHEN AC_LON_TYP = 'GB' THEN '6'
		WHEN AC_LON_TYP = 'PL' THEN '6'
		WHEN AC_LON_TYP = 'SL' THEN '6'
		ELSE 'N/A'
	END AS Risk_Category,
	Principal,
	Interest,
	Collection_Cost_Other_Chgs
FROM
	POP
WHERE
	GA_Code ^= ''
;
QUIT;

PROC SQL;
CREATE TABLE MISSING_DATA AS 
SELECT
	*
FROM
	OUTPUT
WHERE
	Risk_Category = 'N/A' OR Loan_Type = 'N/A' OR Cohort_Year_Dist = 'N/A'
;
QUIT;

PROC SQL;
CREATE TABLE MISSING AS 
SELECT
	*
FROM
	POP
WHERE
	A = ''
;
QUIT;

/* export to CSV */
PROC EXPORT 
	DATA= WORK.OUTPUT
	OUTFILE= "T:\SAS\FSA MR-32 Data Request.xlsx" 
	DBMS=xlsx; 
RUN;



