LIBNAME DLGSUTWH DB2 DATABASE=DLGSUTWH OWNER=OLWHRM1;
%LET RPTLIB = %SYSGET(reportdir);
/*%LET RPTLIB = C:\WINDOWS\TEMP;*/
FILENAME REPORT2 "&RPTLIB/ULWM18.LWM18R2";
FILENAME REPORTZ "&RPTLIB/ULWM18.LWM18RZ";


/*LIBNAME  WORKLOCL  REMOTE  SERVER=CYPRUS  SLIBREF=WORK;*/
/*RSUBMIT;*/

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

SELECT DISTINCT B.DF_PRS_ID
,'EXITCALL' AS QNAME
,'' as INST_ID
,'' as INST_TYPE
,'' as DUE_DATE
,'' AS DUE_TIME 
,'' AS TEXT 


FROM OLWHRM1.PD42_PRS_PHN B
INNER JOIN ( SELECT XX.LF_STU_SSN,MAX(XX.LD_SCL_SPR) as MAX_SEP
			FROM OLWHRM1.LN13_LON_STU_OSD DD
			INNER JOIN OLWHRM1.LN10_LON AA
				ON DD.BF_SSN = AA.BF_SSN 
				AND DD.LN_SEQ = AA.LN_SEQ
				AND AA.LA_CUR_PRI > 0
				AND DAYS(AA.LD_END_GRC_PRD) > DAYS(CURRENT DATE)
			INNER JOIN OLWHRM1.SD10_STU_SPR XX
				ON DD.BF_SSN = XX.LF_STU_SSN 
				AND DD.LN_STU_SPR_SEQ = XX.LN_STU_SPR_SEQ
				AND XX.LC_REA_SCL_SPR IN ('01','08')
				AND XX.LC_STA_STU10 = 'A'
			WHERE DD.LC_STA_LON13 = 'A'
			GROUP BY XX.LF_STU_SSN
			) C
	ON B.DF_PRS_ID = C.LF_STU_SSN 
INNER JOIN OLWHRM1.GA01_APP D 
	ON D.DF_PRS_ID_BR = C.LF_STU_SSN
INNER JOIN OLWHRM1.GA10_LON_APP E
	ON D.AF_APL_ID = E.AF_APL_ID
	AND E.AC_LON_TYP IN ('GB','SF','SL','SU')
	AND E.AC_PRC_STA = 'A'

WHERE B.DC_PHN = 'H'
AND B.DI_PHN_VLD = 'Y'
AND DAYS(CURRENT DATE) > DAYS(C.MAX_SEP) + 60

AND B.DF_PRS_ID NOT IN (SELECT DISTINCT AA.DF_PRS_ID 
					FROM OLWHRM1.AY01_BR_ATY AA
					WHERE AA.PF_ACT = 'AUCON'
					AND DAYS(AA.BD_ATY_PRF) > DAYS(CURRENT DATE)-15)
AND B.DF_PRS_ID NOT IN (SELECT DISTINCT AA.DF_PRS_ID 
					FROM OLWHRM1.AY01_BR_ATY AA
					WHERE AA.PF_ACT = 'ASCON'
					AND DAYS(AA.BD_ATY_PRF) > DAYS(CURRENT DATE)-30)


AND B.DF_PRS_ID NOT IN (SELECT DISTINCT AA.DF_PRS_ID_BR
					FROM OLWHRM1.CT30_CALL_QUE AA
					WHERE AA.IF_WRK_GRP = 'EXITCALL')




FOR READ ONLY WITH UR

);
DISCONNECT FROM DB2;

%put  sqlxrc= >>> &sqlxrc <<< ||| sqlxmsg= >>> &sqlxmsg >>> ;  ** includes error messages to SAS log  ;
%sqlcheck;
quit;

/*ENDRSUBMIT;*/
/**/
/*DATA DEMO; SET WORKLOCL.DEMO; RUN;*/

data _null_;
set  WORK.Demo                                    end=EFIEOD;

file REPORT2 delimiter=',' DSD DROPOVER lrecl=32767;
   format DF_PRS_ID $9. ;
   format QNAME $8. ;
   format INST_ID $1. ;
   format INST_TYPE $1. ;
   format DUE_DATE $1. ;
   format DUE_TIME $1. ;
   format TEXT $1. ;

 do;
   EFIOUT + 1;
   put DF_PRS_ID $ @;
   put QNAME $ @;
   put INST_ID $ @;
   put INST_TYPE $ @;
   put DUE_DATE @;
   put DUE_TIME @;
   put TEXT $ ;
   ;
 end;

run;
