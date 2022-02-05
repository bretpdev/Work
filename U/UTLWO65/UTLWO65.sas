/*TILP SOE DATA FILES*/

/*LIBNAME OLWHRM1 DB2 DATABASE=DLGSUTWH OWNER=OLWHRM1;*/
/*%LET RPTLIB = %SYSGET(reportdir);*/
%LET RPTLIB = T:\SAS;
FILENAME REPORT2 "&RPTLIB/ULWO65.LWO65R2";
FILENAME REPORTZ "&RPTLIB/ULWO65.LWO65RZ";

DATA _NULL_;
/*Run as if today's date was:*/
RUN_DAY = TODAY();
/*RUN_DAY = '01DEC1941'D;*/
     CALL SYMPUT('SCHOOL_YEAR_START',"'07/01/" ||PUT(YEAR(INTNX('MONTH',RUN_DAY,-6,'B'))-1,4.)||"'");	
     CALL SYMPUT('SCHOOL_YEAR_END',"'06/30/"||PUT(YEAR(INTNX('MONTH',RUN_DAY,-6,'B')),4.)||"'");		
RUN;

%SYSLPUT SCHOOL_YEAR_START = &SCHOOL_YEAR_START;
%SYSLPUT SCHOOL_YEAR_END = &SCHOOL_YEAR_END;

LIBNAME  DUSTER  REMOTE  SERVER=DUSTER  SLIBREF=WORK;
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
	SELECT DISTINCT
		PD10.DF_PRS_ID
		,PD10.DF_SPE_ACC_ID
		,PD10.DM_PRS_1
		,PD10.DM_PRS_LST
		,AY20.LX_ATY AS COMMENT
		,AY10.PF_REQ_ACT AS ARC
	FROM	
		OLWHRM1.LN10_LON LN10
		INNER JOIN OLWHRM1.DF10_BR_DFR_REQ DF10
			ON LN10.BF_SSN = DF10.BF_SSN
		INNER JOIN OLWHRM1.LN50_BR_DFR_APV LN50
			ON DF10.BF_SSN = LN50.BF_SSN
			AND DF10.LF_DFR_CTL_NUM = LN50.LF_DFR_CTL_NUM
		INNER JOIN OLWHRM1.PD10_PRS_NME PD10
			ON LN10.BF_SSN = PD10.DF_PRS_ID
		LEFT OUTER JOIN OLWHRM1.AY10_BR_LON_ATY AY10
			ON LN10.BF_SSN = AY10.BF_SSN
			AND AY10.LC_STA_ACTY10 = 'A'
			AND AY10.PF_REQ_ACT = 'CACTS'
		LEFT OUTER JOIN OLWHRM1.AY20_ATY_TXT AY20
			ON AY10.BF_SSN = AY20.BF_SSN
			AND AY10.LN_ATY_SEQ = AY20.LN_ATY_SEQ
	WHERE 
		LN10.IC_LON_PGM = 'TILP'
		AND LN10.LA_CUR_PRI > 0
		AND DF10.LC_DFR_TYP = '06'
		AND DAYS(LN50.LD_DFR_END) >= DAYS(&SCHOOL_YEAR_START) 
		AND DAYS(LN50.LD_DFR_BEG) <= DAYS(&SCHOOL_YEAR_END)
		AND LN50.LC_STA_LON50 = 'A'
		AND LN10.LC_STA_LON10 = 'R'
FOR READ ONLY WITH UR
);

/*%put  sqlxrc= >>> &sqlxrc <<< ||| sqlxmsg= >>> &sqlxmsg >>> ;  ** includes error messages to SAS log  ;*/
/*%sqlcheck;*/
quit;

ENDRSUBMIT;

DATA DEMO; SET DUSTER.DEMO; RUN;

PROC SORT DATA=DEMO; BY DF_PRS_ID COMMENT; RUN;


DATA DEMO(keep=CACTUS_ID DM_PRS_1 DM_PRS_LST DF_PRS_ID);
SET DEMO;
FORMAT CACTUS_ID $15.;
IF INDEX(COMMENT,' ') > 0 THEN CACTUS_ID = SUBSTR(COMMENT,1,INDEX(COMMENT,' '));
IF ARC = '' THEN CACTUS_ID = DF_SPE_ACC_ID;
RUN;

DATA DEMO;
SET DEMO;
WHERE CACTUS_ID <> '';
RUN;

DATA DEMO (DROP=DF_PRS_ID);
SET DEMO;
BY DF_PRS_ID;
if first.df_prs_id;
FORMAT CACTUS_ID $15.;
RUN;

PROC SORT DATA=DEMO;
BY DM_PRS_LST;
RUN;

DATA _NULL_;
SET  WORK.DEMO;
FILE REPORT2 DELIMITER=',' DSD DROPOVER LRECL=32767;
IF _N_ = 1 THEN DO;
	PUT 'CACTUS_ID,FIRST_NAME,LAST_NAME,SCHOOL_NUMBER,CONTRACT_HOURS,CONTRACT_DAYS,BEGIN_DATE,NESS,TITLE1,DISTRICT_NUMBER,INTER_FLAG';
END;
DO;
	PUT CACTUS_ID DM_PRS_1 DM_PRS_LST ',,,,,,,';
END;
run;
