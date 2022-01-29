/*%LET RPTLIB = %SYSGET(reportdir);*/
%LET RPTLIB = T:\SAS;
FILENAME REPORTZ "&RPTLIB/UNWC11.NWC11RZ";
FILENAME REPORT2 "&RPTLIB/UNWC11.NWC11R2";
LIBNAME  LEGEND  REMOTE  SERVER=LEGEND SLIBREF=WORK  ;
RSUBMIT;

DATA _NULL_;
RUN_MON = -1;
	CALL SYMPUT('BEGIN',PUT(INTNX('MONTH',TODAY(),RUN_MON,'B'),5.));
	CALL SYMPUT('FINISH',PUT(INTNX('MONTH',TODAY(),RUN_MON,'E'),5.));
RUN;

/*%let DB = DNFPRQUT;  *This is test;*/
%let DB = DNFPUTDL;  *This is live;

LIBNAME PKUB DB2 DATABASE=&DB OWNER=PKUB;
LIBNAME PKUR DB2 DATABASE=&DB OWNER=PKUR;
LIBNAME FRDWODS DB2 DATABASE=&DB OWNER=FRDWODS;


PROC SQL;
CREATE TABLE PRV_SER AS
select DISTINCT PF_SSN
	,PM_SER_PRV
	,PD_LON_ACL_ADD
from PKUB.BT35_MNR_BCH_BR E
inner join PKUB.BT20_MAJ_BCH D
	ON E.PF_MAJ_BCH = D.PF_MAJ_BCH;

CREATE TABLE DEAL_ID AS
SELECT DISTINCT Z.BF_SSN
	,Z.WF_DEA
	,Y.WD_LON_SLE
FROM FRDWODS.ZC16_FIL_TRF_DTL Z
INNER JOIN FRDWODS.ZC15_FIL_TRF_HDR Y
	ON Z.WF_DEA = Y.WF_DEA
WHERE Y.WC_LON_TYP = 'LNC'
;

CREATE TABLE DEMO AS
SELECT C.LD_FAT_APL
	,e.PM_SER_PRV
	,A.LC_FED_PGM_YR
	,B.WF_DEA
	,COUNT(DISTINCT C.BF_SSN) AS BORROWER
	,COUNT(*) AS LOAN
	,SUM(COALESCE(C.LA_FAT_CUR_PRI,0)) AS PBO
	,SUM(COALESCE(C.LA_FAT_NSI,0)) AS IRB
	,CALCULATED PBO + CALCULATED IRB AS TOT
/*The transaction table is being used because we need the conversion amount, not an end of month or current dollar amount*/
FROM pkub.LN90_FIN_ATY C
INNER JOIN pkub.LN10_LON A
	ON A.BF_SSN = C.BF_SSN
	AND A.LN_SEQ = C.LN_SEQ
	AND C.LD_FAT_APL = A.LD_LON_ACL_ADD
LEFT OUTER JOIN PRV_SER e
	ON C.BF_SSN = E.PF_SSN
	AND C.LD_FAT_APL = E.PD_LON_ACL_ADD
LEFT OUTER JOIN DEAL_ID B
	ON A.BF_SSN = B.BF_SSN
	AND C.LD_FAT_EFF = B.WD_LON_SLE

WHERE A.IC_LON_PGM IN ('DLUCNS', 'DLSCNS', 'CNSLDN', 'DSCON', 'DLUSPL', 'DUCON', 'SPCNSL', 
						'SUBCNS', 'SUBSPC', 'UNCNS', 'DLSPCN', 'DLSSPL', 'DLCNSL', 'DLPCNS', 
						'UNSPC', 'DLSCST', 'DLSCUN', 'DLSCPL', 'DLSCPG', 'DLSCSL', 'DLSCSC', 'DLSCUC', 'DLSCCN')
	AND C.PC_FAT_TYP in ('02','03')
	AND C.PC_FAT_SUB_TYP in ('90','91')
	AND C.LD_FAT_APL BETWEEN &BEGIN AND &FINISH
	AND C.LC_STA_LON90 = 'A'
	AND C.LC_FAT_REV_REA = ''
GROUP BY C.LD_FAT_APL
	,PM_SER_PRV
	,LC_FED_PGM_YR
	,WF_DEA
;
QUIT;

PROC SORT DATA=DEMO ; BY LD_FAT_APL PM_SER_PRV LC_FED_PGM_YR ; RUN;

ENDRSUBMIT;
DATA DEMO; SET LEGEND.DEMO; RUN;

DATA _NULL_;
SET DEMO END=LAST;
FILE REPORT2 DELIMITER=',' DSD DROPOVER LRECL=32767;
FORMAT LD_FAT_APL MMDDYY10. PBO IRB TOT C D E DOLLAR18.2;
if _n_ = 1 then do;
	put "DATE LOAN ADDED,SOURCE,LOAN PROGRAM,DEAL ID,TOTAL BORROWERS,TOTAL LOANS,TOTAL PRINCIPAL,TOTAL INTEREST,TOTAL TRANSFERRED";
	ARRAY TOTALS{5} A B C D E (0 0 0 0 0);
	ARRAY SOURCE{5} BORROWER LOAN PBO IRB TOT;
end;

DO I = 1 TO 5;
	TOTALS(I) = TOTALS(I) + SOURCE(I);
END;

PUT LD_FAT_APL PM_SER_PRV LC_FED_PGM_YR WF_DEA BORROWER	LOAN PBO IRB TOT ;

IF LAST THEN DO;
   PUT ',,,,' A B C D E @;
END;
RUN;