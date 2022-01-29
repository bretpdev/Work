/*UTLWS11 BORROWER SERVICES FORBEARANCE, DEFERMENT, INCOME SENSITIVE LETTERS*/
/*%LET RPTLIB = %SYSGET(reportdir);*/
%LET RPTLIB = T:\SAS;
FILENAME REPORTZ "&RPTLIB/ULWS11.LWS11RZ";
FILENAME REPORT2 "&RPTLIB/ULWS11.LWS11R2";
FILENAME REPORT3 "&RPTLIB/ULWS11.LWS11R3";
FILENAME REPORT4 "&RPTLIB/ULWS11.LWS11R4";
FILENAME REPORT5 "&RPTLIB/ULWS11.LWS11R5";
FILENAME REPORT6 "&RPTLIB/ULWS11.LWS11R6";
FILENAME REPORT7 "&RPTLIB/ULWS11.LWS11R7";
FILENAME REPORT8 "&RPTLIB/ULWS11.LWS11R8";
FILENAME REPORT9 "&RPTLIB/ULWS11.LWS11R9";
FILENAME REPORT10 "&RPTLIB/ULWS11.LWS11R10";
FILENAME REPORT11 "&RPTLIB/ULWS11.LWS11R11";
FILENAME REPORT12 "&RPTLIB/ULWS11.LWS11R12";
FILENAME REPORT13 "&RPTLIB/ULWS11.LWS11R13";
FILENAME REPORT14 "&RPTLIB/ULWS11.LWS11R14";
FILENAME REPORT15 "&RPTLIB/ULWS11.LWS11R15";
FILENAME REPORT16 "&RPTLIB/ULWS11.LWS11R16";
FILENAME REPORT17 "&RPTLIB/ULWS11.LWS11R17";
FILENAME REPORT18 "&RPTLIB/ULWS11.LWS11R18";
FILENAME REPORT19 "&RPTLIB/ULWS11.LWS11R19";
FILENAME REPORT25 "&RPTLIB/ULWS11.LWS11R25";
FILENAME REPORT26 "&RPTLIB/ULWS11.LWS11R26";
FILENAME REPORT27 "&RPTLIB/ULWS11.LWS11R27";
FILENAME REPORT28 "&RPTLIB/ULWS11.LWS11R28";
FILENAME REPORT29 "&RPTLIB/ULWS11.LWS11R29";

DATA _NULL_;
	IF WEEKDAY(TODAY()) = 2 THEN DO;
		CALL SYMPUT('CDATE',"'"||PUT(INTNX('DAY',TODAY(),-3,'BEGINNING'), MMDDYYD10.)||"'");
	END;
	ELSE DO;
		CALL SYMPUT('CDATE',"'"||PUT(INTNX('DAY',TODAY(),-2,'BEGINNING'), MMDDYYD10.)||"'");
	END;
RUN;
%SYSLPUT CDATE = &CDATE;

/*TEST*/
/*LIBNAME  DUSTER  REMOTE  SERVER=QADBD004 SLIBREF=WORK  ;*/

/*LIVE*/
LIBNAME  DUSTER  REMOTE  SERVER=DUSTER  SLIBREF=WORK;
RSUBMIT;  

/*%let DB = DLGSWQUT;  *This is test;*/
%let DB = DLGSUTWH;  *This is live;

LIBNAME OLWHRM1 DB2 DATABASE=&DB OWNER=OLWHRM1; 
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
CONNECT TO DB2 (DATABASE=&DB);
CREATE TABLE DEMO AS
SELECT *
FROM CONNECTION TO DB2 (
SELECT DISTINCT PD10.DF_PRS_ID AS BF_SSN
	,AY10.PF_REQ_ACT
	,PD10.DF_SPE_ACC_ID
	,PD10.DM_PRS_MID
	,PD10.DM_PRS_1
	,PD10.DM_PRS_LST
	,PD30.DX_STR_ADR_1
	,PD30.DX_STR_ADR_2
	,PD30.DX_STR_ADR_3
	,PD30.DM_CT
	,PD30.DC_DOM_ST
	,PD30.DF_ZIP_CDE
	,PD30.DM_FGN_CNY
	,PD30.DM_FGN_ST
	,PD30.DC_ADR
	,AY10.LD_ATY_REQ_RCV
	,SUBSTR(AY20.LX_ATY, 1, 9) AS PMT_AMT
	,'MA2324' AS COST_CENTER_CODE
	,CASE
		WHEN PD30.DC_DOM_ST IN ('FC','') THEN 1
		ELSE 2
	 END AS SVAR	
FROM OLWHRM1.AY10_BR_LON_ATY AY10
INNER JOIN OLWHRM1.PD10_PRS_NME PD10
	ON AY10.BF_SSN = PD10.DF_PRS_ID
INNER JOIN OLWHRM1.PD30_PRS_ADR PD30
	ON PD10.DF_PRS_ID = PD30.DF_PRS_ID
LEFT OUTER JOIN OLWHRM1.AY15_ATY_CMT AY15
	ON AY10.BF_SSN = AY15.BF_SSN
	AND AY10.LN_ATY_SEQ = AY15.LN_ATY_SEQ
LEFT OUTER JOIN OLWHRM1.AY20_ATY_TXT AY20
	ON AY15.BF_SSN = AY20.BF_SSN
	AND AY15.LN_ATY_SEQ = AY20.LN_ATY_SEQ
	AND AY15.LN_ATY_CMT_SEQ = AY20.LN_ATY_CMT_SEQ

WHERE AY10.PF_REQ_ACT IN (
		'G7077','G708C','H742A','LS261','DRNDP',
		'DMISH','PHNPL','ALSCH','TPUNL','TPECL',
		'TPMIS','TPISD','TPMDL','TPBBD','TPBBA',
		'APBDN','THFSC','NAMCH','MILDF','TPDCL','ALSCH',
		'IBRCL','RQ003','IVRM1'
		)
	AND AY10.LD_ATY_REQ_RCV = &CDATE
	AND PD30.DC_ADR = 'L'

	
FOR READ ONLY WITH UR
);
DISCONNECT FROM DB2;

PROC SQL;
	CREATE TABLE INTEREST AS 
		SELECT DISTINCT
			LN10.BF_SSN,
			ln10.ln_seq,
			(LN72.LR_ITR / 100) as calc1,
			LN10.LA_CUR_PRI  as calc2
		FROM
			OLWHRM1.LN72_INT_RTE_HST LN72
			INNER JOIN OLWHRM1.LN10_LON LN10
				ON LN10.BF_SSN = LN72.BF_SSN
				AND LN10.LN_SEQ = LN72.LN_SEQ
			INNER JOIN DEMO D
				ON D.BF_SSN = LN72.BF_SSN
		WHERE
			LC_STA_LON72 = 'A'
			AND TODAY() BETWEEN LD_ITR_EFF_BEG AND LD_ITR_EFF_END
			AND LN10.LC_STA_LON10 = 'R'
			AND LN10.LA_CUR_PRI > 0
		order by 
			ln10.bf_ssn
;
QUIT;

PROC SQL;
	CREATE TABLE TEST AS 
		SELECT DISTINCT
			BF_SSN,
			SUM(CALC1 * CALC2)  AS CALC3
		FROM
			INTEREST
		GROUP BY 
			BF_SSN
;

	CREATE TABLE FINAL AS 
		SELECT DISTINCT
			D.*,
			(T.CALC3 / 365 * 31 + 5) AS INTEREST
		FROM
			DEMO D
			INNER JOIN TEST T
				ON D.BF_SSN = T.BF_SSN
;
QUIT;


/*%put  sqlxrc= >>> &sqlxrc <<< ||| sqlxmsg= >>> &sqlxmsg >>> ;  ** includes error messages to SAS log  ;*/
/*%sqlcheck;*/
/*quit;*/
ENDRSUBMIT;
DATA DEMO;
	SET DUSTER.FINAL;
/*	INTEREST = SUM(CALC1 * CALC2);*/
RUN;
DATA DEMO (DROP=PMT_AMT);
	SET DEMO;
	IF PF_REQ_ACT IN ('PHNPL') THEN 
		IPMT_AMT = INPUT(SUBSTR(PMT_AMT,2,8),BEST12.);
	ELSE IF PF_REQ_ACT = 'IBAPV' THEN
		PMT = INTEREST;
RUN;

*CALCULATE KEYLINE;
DATA DEMO1 (DROP = KEYSSN MODAY KEYLINE CHKDIG DIG I 
	CHKDIG CHK1 CHK2 CHK3 CHKDIGIT CHECK);
SET DEMO;
KEYSSN = TRANSLATE(BF_SSN,'MYLAUGHTER','0987654321');
MODAY = PUT(DATE(),MMDDYYN4.);
KEYLINE = "P"||KEYSSN||MODAY||DC_ADR;
CHKDIG = 0;
LENGTH DIG $2.;
DO I = 1 TO LENGTH(KEYLINE);
	IF I/2 NE ROUND(I/2,1) 
		THEN DIG = PUT(INPUT(SUBSTR(KEYLINE,I,1),BITS4.4) * 2, 2.);
	ELSE DIG = PUT(INPUT(SUBSTR(KEYLINE,I,1),BITS4.4), 2.);
	IF SUBSTR(DIG,1,1) = " " 
		THEN CHKDIG = CHKDIG + INPUT(SUBSTR(DIG,2,1),1.);
		ELSE DO;
			CHK1 = INPUT(SUBSTR(DIG,1,1),1.);
			CHK2 = INPUT(SUBSTR(DIG,2,1),1.);
			IF CHK1 + CHK2 >= 10
				THEN DO;
					CHK3 = PUT(CHK1 + CHK2,2.);
					CHK1 = INPUT(SUBSTR(CHK3,1,1),1.);
					CHK2 = INPUT(SUBSTR(CHK3,2,1),1.);
				END;
			CHKDIG = CHKDIG + CHK1 + CHK2;
		END;
END;
CHKDIGIT = 10 - INPUT(SUBSTR((RIGHT(PUT(CHKDIG,3.))),3,1),3.);
IF CHKDIGIT = 10 THEN CHKDIGIT = 0;
CHECK = PUT(CHKDIGIT,1.);
ACSKEY = "#"||KEYLINE||CHECK||"#";
RUN;

%MACRO GENRPT(ARC,REPNO,NEWDS);
DATA &NEWDS;
	SET DEMO1;
	WHERE PF_REQ_ACT = &ARC;
RUN;
PROC SORT DATA=&NEWDS;
	BY SVAR DC_DOM_ST;
RUN;
DATA _NULL_;
	SET  WORK.&NEWDS;
	FILE REPORT&REPNO DELIMITER=',' DSD DROPOVER LRECL=32767;
		FORMAT BF_SSN $9. ;
		FORMAT PF_REQ_ACT $5. ;
		FORMAT DF_SPE_ACC_ID $10. ;
		FORMAT DM_PRS_MID $13. ;
		FORMAT DM_PRS_1 $13. ;
		FORMAT DM_PRS_LST $23. ;
		FORMAT DX_STR_ADR_1 $30. ;
		FORMAT DX_STR_ADR_2 $30. ;
		FORMAT DX_STR_ADR_3 $30. ;
		FORMAT DM_CT $20. ;
		FORMAT DC_DOM_ST $2. ;
		FORMAT DF_ZIP_CDE $17. ;
		FORMAT DM_FGN_CNY $25. ;
		FORMAT DM_FGN_ST $15. ;
		FORMAT ACSKEY $18. ;
		FORMAT LD_ATY_REQ_RCV MMDDYY10. ;
		FORMAT IPMT_AMT DOLLAR12.2 ;
		FORMAT PMT DOLLAR12.2 ;
		FORMAT COST_CENTER_CODE $6.;
	DO;
		IF _N_ = 1 THEN DO;
			PUT 'SSN,Arc,AN,FirstName,MI,LastName,Address1,Address2,Address3,City,State,Zip,Country,Foreign State,KeyLine,Arc Date,Payment,State_Ind,COST_CENTER_CODE,INTEREST';
		END;
		PUT BF_SSN $ @;
		PUT PF_REQ_ACT $ @;
		PUT DF_SPE_ACC_ID $ @;
		PUT DM_PRS_1 $ @;
		PUT DM_PRS_MID $ @;
		PUT DM_PRS_LST $ @;
		PUT DX_STR_ADR_1 $ @;
		PUT DX_STR_ADR_2 $ @;
		PUT DX_STR_ADR_3 $ @;
		PUT DM_CT $ @;
		PUT DC_DOM_ST $ @;
		PUT DF_ZIP_CDE $ @;
		PUT DM_FGN_CNY $ @;
		PUT DM_FGN_ST $ @;
		PUT ACSKEY $ @;
		PUT LD_ATY_REQ_RCV $ @;
		PUT IPMT_AMT  @;
		PUT DC_DOM_ST $ @;
		PUT COST_CENTER_CODE $ @;
		PUT PMT  $;
	END;
RUN;
%MEND GENRPT;

%GENRPT('G7077',2,G7077);
%GENRPT('G708C',3,G708C);
%GENRPT('H742A',4,H742A);
%GENRPT('DRNDP',5,DRNDP);
%GENRPT('DMISH',6,DMISH);
%GENRPT('LS261',7,LS261);
%GENRPT('PHNPL',8,PHNPL);
%GENRPT('ALSCH',9,ALSCH);
%GENRPT('TPUNL',10,TPUNL);
%GENRPT('TPECL',11,TPECL);
%GENRPT('TPMIS',12,TPMIS);
%GENRPT('TPISD',13,TPISD);
%GENRPT('TPMDL',14,TPMDL);
%GENRPT('TPBBD',15,TPBBD);
%GENRPT('TPBBA',16,TPBBA);
%GENRPT('APBDN',17,APBDN);
%GENRPT('THFSC',18,THFSC);
%GENRPT('NAMCH',19,NAMCH);
%GENRPT('MILDF',25,MILDF);
%GENRPT('TPDCL',26,TPDCL);
%GENRPT('IBRCL',27,IBRCL);
%GENRPT('RQ003',28,RQ003);
%GENRPT('IVRM1',29,IVRM1);

