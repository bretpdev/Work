LIBNAME DLGSUTWH DB2 DATABASE=DLGSUTWH OWNER=OLWHRM1;
%LET RPTLIB = %SYSGET(reportdir);
/*%LET RPTLIB = T:\SAS;*/
FILENAME REPORTZ "&RPTLIB/ULWK38.LWK38RZ";
FILENAME REPORT2 "&RPTLIB/ULWK38.LWK38R2";
%LET TBLLIB = /sas/whse/progrevw;
/*%LET TBLLIB = Q:\Process Automation\TabSAS;*/

/*INPUT LOAN TYPES FOR PRIVATE AND FFEL LOANS*/
DATA LOAN_TYPES;
	FORMAT LN_TYP LN_PGM $50.;
	INFILE "&TBLLIB/GENR_REF_LoanTypes.txt" DLM=',' MISSOVER DSD;
	INFORMAT LN_TYP LN_PGM $50.;
	INPUT LN_TYP LN_PGM ;
	LN_PGM = UPCASE(LN_PGM);
RUN;
/*CREATE MACRO VARIALBE LISTS OF LOAN PROGRAMS(PRIVATE LOANS)*/
PROC SQL NOPRINT;
	SELECT "'"||TRIM(LN_TYP)||"'" 
		INTO :PRIVATE_LIST SEPARATED BY "," /*PRIVATE LOAN LIST*/
	FROM LOAN_TYPES
	WHERE LN_PGM ^= 'FFEL';
QUIT;
/*%SYSLPUT PRIVATE_LIST = &PRIVATE_LIST;*/
/*LIBNAME  WORKLOCL  REMOTE  SERVER=CYPRUS  SLIBREF=WORK;*/
/*RSUBMIT; */
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
CONNECT TO DB2 (DATABASE=DLGSUTWH);
CREATE TABLE SKPEMP AS
SELECT *
FROM CONNECTION TO DB2 (
SELECT DISTINCT A.BF_SSN
	,A.LN_SEQ
	,A.LA_CUR_PRI
FROM OLWHRM1.LN10_LON A
INNER JOIN OLWHRM1.PD30_PRS_ADR B
	ON A.BF_SSN = B.DF_PRS_ID
INNER JOIN OLWHRM1.PD42_PRS_PHN C
	ON A.BF_SSN = C.DF_PRS_ID
LEFT OUTER JOIN (SELECT MAX(LD_ATY_REQ_RCV) AS LD_ATY_REQ_RCV
					,BF_SSN
				FROM OLWHRM1.AY10_BR_LON_ATY AY10
				WHERE PF_REQ_ACT = 'K1APP'
				GROUP BY BF_SSN ) Z
	ON A.BF_SSN = Z.BF_SSN
LEFT OUTER JOIN (SELECT BF_SSN
				,AD_LC_APL_RCV
			FROM OLWHRM1.LC01_LC_APL_MST) Y
	ON A.BF_SSN = Y.BF_SSN
LEFT OUTER JOIN (SELECT BF_SSN
				,MAX(LD_DSB) AS LD_DSB
			FROM OLWHRM1.LN15_DSB
			GROUP BY BF_SSN) X
	ON A.BF_SSN = X.BF_SSN
WHERE A.LA_CUR_PRI > 0
	AND A.LC_STA_LON10 = 'R'
	AND A.LI_CON_PAY_STP_PUR != 'Y'
	AND ((B.DC_ADR = 'L' AND B.DI_VLD_ADR = 'N')
		OR (C.DI_PHN_VLD = 'N' AND C.DC_PHN = 'H'))
	AND A.BF_SSN NOT IN (SELECT BF_SSN
				FROM OLWHRM1.WQ20_TSK_QUE
				WHERE WF_QUE = 'KB'
					AND WF_SUB_QUE = '01')
	AND A.BF_SSN NOT IN (SELECT BF_SSN
				FROM OLWHRM1.DW01_DW_CLC_CLU 
				WHERE WC_DW_LON_STA IN ('16','17','18','19','20','21'))
	AND (A.LD_LON_1_DSB > COALESCE(Z.LD_ATY_REQ_RCV,'01/01/1950'))
	AND ((A.IC_LON_PGM != 'TILP' AND A.IC_LON_PGM NOT IN (&PRIVATE_LIST))
		OR (Y.AD_LC_APL_RCV > X.LD_DSB 
			AND (A.LD_LON_1_DSB > COALESCE(Z.LD_ATY_REQ_RCV,'01/01/1950') 
			AND COALESCE(Z.LD_ATY_REQ_RCV,'01/01/1950') < '06/01/2001'
			AND (B.DC_ADR = 'L' OR B.DI_VLD_ADR = 'N' 
				OR (C.DI_PHN_VLD = 'N' AND C.DC_PHN = 'H')))))
FOR READ ONLY WITH UR
);
DISCONNECT FROM DB2;

%PUT  SQLXRC= >>> &SQLXRC <<< ||| SQLXMSG= >>> &SQLXMSG >>> ;  ** INCLUDES ERROR MESSAGES TO SAS LOG  ;
%SQLCHECK;
QUIT;

/*ENDRSUBMIT;*/
/*DATA SKPEMP;*/
/*	SET WORKLOCL.SKPEMP;*/
/*RUN;*/
PROC SORT DATA=SKPEMP;
	BY BF_SSN LN_SEQ;
RUN;
DATA SKPEMP (DROP=LN_SEQ SEQLIST SEQLIST2);
SET SKPEMP;
LENGTH SEQLIST SEQLIST2 $200. ;
BY BF_SSN;
RETAIN SEQLIST SEQLIST2;
IF FIRST.BF_SSN THEN DO;
	SEQLIST = PUT(LN_SEQ,3.);
	SEQLIST2 = SEQLIST;
END;
ELSE DO;
	SEQLIST2 = TRIM(SEQLIST2);
	SEQLIST = PUT(LN_SEQ,3.);
	SEQLIST2 = TRIM(SEQLIST2)||','||TRIM(SEQLIST);
END;
IF LAST.BF_SSN THEN DO;
	SEQ_LIST = LEFT(TRIM(SEQLIST2));
	OUTPUT ;
END;
RUN;
DATA _NULL_;
SET  SKPEMP;
FILE REPORT2 DELIMITER=',' DSD DROPOVER LRECL=32767;
ARC = 'EPRVW';
FROM_DATE = '';
TO_DATE = '';
NEED_BY_DATE = '';
RECIPIENT_ID = '';
REG_TO_CODE = '';
REG_TO_ID = '';
LOAN_SEQ_NUM = SEQ_LIST ;
COMMENTS = 'REVIEW BORROWER APPLICATION FOR ANY POSSIBLE NEW EMPLOYER RECORDS.';
DO;
   PUT BF_SSN $ @;
   PUT ARC @ ;
   PUT FROM_DATE @;
   PUT TO_DATE @;
   PUT NEED_BY_DATE @;
   PUT RECIPIENT_ID @;
   PUT REG_TO_CODE @;
   PUT REG_TO_ID @;
   PUT LOAN_SEQ_NUM @;
   PUT COMMENTS $;
END;
RUN;
