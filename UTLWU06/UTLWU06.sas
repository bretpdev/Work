/*LIBNAME DLGSUTWH DB2 DATABASE=DLGSUTWH OWNER=OLWHRM1;*/
/*%LET RPTLIB = %SYSGET(reportdir);*/
%LET RPTLIB = T:\SAS;
FILENAME REPORTZ "&RPTLIB/ULWU06.LWU06RZ";
FILENAME REPORT2 "&RPTLIB/ULWU06.LWU06R2";
LIBNAME  WORKLOCL  REMOTE  SERVER=CYPRUS  SLIBREF=WORK;

DATA _NULL_;
	CALL SYMPUT('PRV_DAY',INTNX('DAY',TODAY(),-1,'BEGINNING'));
RUN; 
%SYSLPUT PRV_DAY = &PRV_DAY;
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
CREATE TABLE IABRQCR AS
SELECT *
FROM CONNECTION TO DB2 (
SELECT X.PERSONID,
	X.ADDR1,
	X.ADDR2,
	X.ST,
	X.CITY, 
	X.ZIP,
	Q.BX_CMT AS COMMENT,
	X.UPDATBY
FROM OLWHRM1.AY01_BR_ATY Q
INNER JOIN (
		SELECT A.DF_SPE_ACC_ID AS PERSONID,
			B.DX_STR_ADR_1 AS ADDR1,
			B.DX_STR_ADR_2 AS ADDR2,
			B.DC_DOM_ST AS ST,
			B.DM_CT AS CITY,
			B.DF_ZIP AS ZIP, 
			B.DF_PRS_ID AS PERSONIDLINK,
			B.DF_USR_UPD_ADR AS UPDATBY
		FROM OLWHRM1.PD01_PDM_INF A
		INNER JOIN OLWHRM1.PD03_PRS_ADR_PHN B
			ON A.DF_PRS_ID = B.DF_PRS_ID
		WHERE DATE(B.DD_LST_UPD_ADR) = DATE(&PRV_DAY)
			AND B.DC_ADR = 'L'
	UNION
		SELECT C.DF_PRS_ID_RFR AS PERSONID,
			C.BX_RFR_STR_ADR_1 AS ADDR1,
			C.BX_RFR_STR_ADR_2 AS ADDR2,
			C.BC_RFR_ST AS ST,
			C.BM_RFR_CT AS CITY,
			C.BF_RFR_ZIP AS ZIP,
			C.DF_PRS_ID_BR AS PERSONIDLINK,
			C.BF_LST_USR_BR03 AS UPDATBY
		FROM OLWHRM1.BR03_BR_REF C
		WHERE DATE(C.BD_ADR_DAT_EFF) = DATE(&PRV_DAY)
	) X
	ON X.PERSONIDLINK = Q.DF_PRS_ID
INNER JOIN (
	SELECT DF_PRS_ID
		,MAX(BD_ATY_PRF) AS MAX_KDT
	FROM OLWHRM1.AY01_BR_ATY
	WHERE PF_ACT = 'K0ADD'
	GROUP BY DF_PRS_ID
	) MAX_K0ADD
	ON Q.DF_PRS_ID = MAX_K0ADD.DF_PRS_ID
LEFT OUTER JOIN (
	SELECT DF_PRS_ID
		,MAX(BD_ATY_PRF) AS MAX_QDT
	FROM OLWHRM1.AY01_BR_ATY
	WHERE PF_ACT = 'QMRVD'
	GROUP BY DF_PRS_ID
	) MAX_QMRVD
	ON Q.DF_PRS_ID = MAX_QMRVD.DF_PRS_ID
WHERE Q.PF_ACT = 'K0ADD'
	AND DAYS(MAX_K0ADD.MAX_KDT) > COALESCE(DAYS(MAX_QMRVD.MAX_QDT),DAYS(MAX_K0ADD.MAX_KDT)-1) 
FOR READ ONLY WITH UR
);
DISCONNECT FROM DB2;
/*%put  sqlxrc= >>> &sqlxrc <<< ||| sqlxmsg= >>> &sqlxmsg >>> ;  ** includes error messages to SAS log  ;*/
/*%sqlcheck;*/
/*quit;*/
ENDRSUBMIT;
DATA IABRQCR;
SET WORKLOCL.IABRQCR;
RUN;
PROC SORT DATA=IABRQCR;BY PERSONID;RUN;

DATA _NULL_;
SET IABRQCR ;
LENGTH DESCRIPTION $600.;
USER = UPDATBY; 
ACT_DT = &PRV_DAY;
DESCRIPTION = CATX(',',
		PERSONID,
		TRIM(ADDR1)||', '||TRIM(ADDR2)||', '||TRIM(CITY)||', '||TRIM(ST)||' '||ZIP,
		COMMENT
/*Activity Comment – Activity comment from the K0ADD action code.  */
/*If more than 1 exist, include a row for each occurrence*/
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
