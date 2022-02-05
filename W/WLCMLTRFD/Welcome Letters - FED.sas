%let indate = '02/09/2012';

%LET RPTLIB = T:\SAS;
%LET REPORT2 = &RPTLIB/Welcome Letter.R2;
%LET REPORT3 = &RPTLIB/Welcome Letter.R3;
%LET REPORT4 = &RPTLIB/Welcome Letter.R4;
%LET REPORT5 = &RPTLIB/Welcome Letter.R5;
%LET REPORT6 = &RPTLIB/Welcome Letter.R6;
FILENAME REPORT7 "&RPTLIB/Welcome Letter.R7";
FILENAME REPORT8 "&RPTLIB/Welcome Letter.R8";
FILENAME REPORT9 "&RPTLIB/Welcome Letter.R9";
FILENAME REPORT10 "&RPTLIB/Welcome Letter.R10";
FILENAME REPORT11 "&RPTLIB/Welcome Letter.R11";
FILENAME REPORT12 "&RPTLIB/Welcome Letter.R12";
%LET REPORT13 = &RPTLIB/Welcome Letter.R13;
FILENAME REPORT14 "&RPTLIB/Welcome Letter.R14";
FILENAME REPORT99 "&RPTLIB/Welcome Letter - Over 50 Loans.csv";

%syslput indate = &indate;
LIBNAME  LEGEND  REMOTE  SERVER=LEGEND SLIBREF=work  ;
RSUBMIT;
/*%let DB = DNFPRQUT; *TEST;*/
%LET DB = DNFPUTDL; *PROD;

/*LIBNAME pkub DB2 DATABASE=DNFPRQUT OWNER=pkub;*/
DATA _NULL_;
call symput('minus60',"'" || put(today()-60,mmddyy10.) || "'");
RUN;

PROC SQL;
CONNECT TO DB2 (DATABASE=&DB);
CREATE TABLE DEMO AS
SELECT *
FROM CONNECTION TO DB2 (
SELECT distinct 
	d.df_spe_acc_id
	,D.DF_PRS_ID
	,A.LN_SEQ
	,d.df_prs_id
	,d.dm_prs_1
	,d.dm_prs_lst
	,b.dx_str_adr_1
	,b.dx_str_adr_2
	,b.dm_ct
	,b.dc_dom_st
	,b.df_zip_cde
	,b.dm_fgn_cny
	,b.dm_fgn_st
	,b.DC_ADR
	,CASE WHEN LC_STA_LN83 = 'A' THEN LC_STA_LN83
		ELSE 'I'
	END AS LC_STA_LN83
	,CASE 
		WHEN LD_DLQ_OCC <= &minus60 THEN 'A'
		WHEN LC_FOR_TYP= '17' THEN 'B'
		WHEN WC_DW_LON_STA = '03' THEN 'C'
		ELSE 'D'
	END AS CODIN
	,G.DX_ADR_EML
	,B.DI_VLD_ADR
	,G.DI_VLD_ADR_EML
	,c.wc_dw_lon_sta
	,A.IC_LON_PGM
	,A.LA_CUR_PRI
	,LN80.LD_BIL_DU_LON
	,LN80.LC_BIL_TYP_LON
	,RS10.LD_RPS_1_PAY_DU
FROM pkub.PD10_PRS_NME d
LEFT OUTER join pkub.PD30_PRS_ADR b
	on d.df_prs_id = b.df_prs_id
LEFT OUTER JOIN PKUB.PD32_PRS_ADR_EML G
	ON D.DF_PRS_ID = G.DF_PRS_ID
	AND G.DC_STA_PD32 = 'A'
LEFT OUTER join pkub.LN10_LON A
	on d.df_prs_id = a.bf_ssn
LEFT OUTER JOIN PKUB.DW01_DW_CLC_CLU C
	ON A.BF_SSN = C.BF_SSN
	AND A.LN_SEQ = C.LN_SEQ
LEFT OUTER JOIN PKUB.LN83_EFT_TO_LON E
	ON A.BF_SSN = E.BF_SSN
	AND A.LN_SEQ = E.LN_SEQ
LEFT OUTER JOIN PKUB.LN16_LON_DLQ_HST F
	ON A.BF_SSN = F.BF_SSN
	AND A.LN_SEQ = F.LN_SEQ
	and f.lc_sta_lon16 = '1'
LEFT OUTER JOIN (select H.bf_ssn
					,lc_for_typ
				from PKUB.LN60_BR_FOR_APV H
				inner join PKUB.FB10_BR_FOR_REQ I
					ON H.BF_SSN = I.BF_SSN
					AND H.LF_FOR_CTL_NUM = I.LF_FOR_CTL_NUM
				where DAYS(H.LD_FOR_END) >= DAYS(&INDATE) - 1
					and i.lc_for_typ = '17') H
	ON A.BF_SSN = H.BF_SSN
LEFT OUTER JOIN PKUB.RS10_BR_RPD RS10
	ON A.BF_SSN = RS10.BF_SSN
	AND RS10.LC_STA_RPST10 = 'A'
LEFT OUTER JOIN PKUB.LN80_LON_BIL_CRF LN80
	ON A.BF_SSN = LN80.BF_SSN
	AND A.LN_SEQ = LN80.LN_SEQ
	AND LN80.LC_STA_LON80 = 'A'

WHERE	LD_LON_ACL_ADD = &INDATE
		AND LN80.LD_BIL_DU_LON = (
								  SELECT	MAX(CRF.LD_BIL_DU_LON) AS MAX_DU_LON
								  FROM		PKUB.LN80_LON_BIL_CRF CRF
								  WHERE		A.BF_SSN = CRF.BF_SSN
											AND A.LN_SEQ = CRF.LN_SEQ
											AND CRF.LC_STA_LON80 = 'A'
								  )
		AND RS10.LD_RPS_1_PAY_DU = (
									SELECT	MAX(DU.LD_RPS_1_PAY_DU)
									FROM	PKUB.RS10_BR_RPD DU
									WHERE	A.BF_SSN = DU.BF_SSN
									)
									
FOR READ ONLY WITH UR
);


/*	IF THE BORROWER HAS BOTH ACTIVE AND INACTIVE RECORDS, KEEP THE ACTIVE RECORD*/
	CREATE TABLE DEMO_A AS
		SELECT	
			A.*
		FROM
			DEMO A
		WHERE
			(
			 A.LC_STA_LN83 = 'I'
			 AND NOT EXISTS (SELECT	B.*
			 				 FROM	DEMO B
							 WHERE	A.df_spe_acc_id = B.df_spe_acc_id
									AND A.LN_SEQ = B.LN_SEQ
									AND B.LC_STA_LN83 = 'A')
			)
			OR
			A.LC_STA_LN83 = 'A'
	;

/*	GET CO-BORROWERS FOR THE LOANS RETURNED BY THE MAIN QUERY ABOVE*/
	CREATE TABLE COBRWS AS
	SELECT DISTINCT
		CO.df_spe_acc_id
		,D.LN_SEQ
		,D.df_prs_id
		,CO.dm_prs_1
		,CO.dm_prs_lst
		,CO.dx_str_adr_1
		,CO.dx_str_adr_2
		,CO.dm_ct
		,CO.dc_dom_st
		,CO.df_zip_cde
		,CO.dm_fgn_cny
		,CO.dm_fgn_st
		,CO.DC_ADR
		,D.LC_STA_LN83
		,D.CODIN
		,CO.DX_ADR_EML
		,CO.DI_VLD_ADR
		,CO.DI_VLD_ADR_EML
		,D.wc_dw_lon_sta
		,D.IC_LON_PGM
		,D.LA_CUR_PRI
		,D.LD_BIL_DU_LON
		,D.LD_RPS_1_PAY_DU
	FROM
		DEMO_A D
		INNER JOIN 
			CONNECTION TO DB2
			(
				SELECT DISTINCT
					LN20.BF_SSN AS BRW_SSN
					,LN20.LN_SEQ
					,PD10.df_spe_acc_id
					,PD10.dm_prs_1
					,PD10.dm_prs_lst
					,PD30.dx_str_adr_1
					,PD30.dx_str_adr_2
					,PD30.dm_ct
					,PD30.dc_dom_st
					,PD30.df_zip_cde
					,PD30.dm_fgn_cny
					,PD30.dm_fgn_st
					,PD30.DC_ADR
					,PD30.DI_VLD_ADR
					,PD32.DX_ADR_EML
					,PD32.DI_VLD_ADR_EML
				FROM
					PKUB.LN20_EDS LN20
					INNER JOIN PKUB.PD10_PRS_NME PD10
						ON LN20.LF_EDS = PD10.DF_PRS_ID
					LEFT OUTER JOIN PKUB.PD30_PRS_ADR PD30
						ON LN20.LF_EDS = PD30.DF_PRS_ID
					LEFT OUTER JOIN PKUB.PD32_PRS_ADR_EML PD32
						ON LN20.LF_EDS = PD32.DF_PRS_ID
				WHERE
					LN20.LC_EDS_TYP = 'M'
			) CO
			ON D.DF_PRS_ID = CO.BRW_SSN
			AND D.LN_SEQ = CO.LN_SEQ	
	ORDER BY D.df_prs_id, D.LN_SEQ
;
DISCONNECT FROM DB2;
QUIT;

ENDRSUBMIT;
DATA DEMO_A; 	SET LEGEND.DEMO_A; RUN;
DATA COBRWS; 	SET LEGEND.COBRWS; RUN;


/*ADD COBORROWERS AND BORROWERS TO CREATE ONE DATASET WHICH INCLUDES EVERYONE*/
DATA DEMO;
	SET DEMO_A COBRWS;
RUN;

/*PROC SORT DATA = DEMO;*/
/*	BY DF_SPE_ACC_ID LN_SEQ;*/
/*RUN;*/

/*GET THE MIN VALUE FOR CODIN FOR EACH BORROWER AND ADD THAT TO THE DATA SO IT CAN BE USED TO PUT ALL OF THE*/
/*BORROWER'S LOANS IN THE SAME OUT FILE USING CODEIN VALUES AS A HIERARCHY*/
PROC SQL;
	CREATE TABLE MINCODE AS
	SELECT
		DF_SPE_ACC_ID
		,MIN(CODIN) AS MINCODIN
	FROM
		DEMO
	GROUP BY
		DF_SPE_ACC_ID
	;

	CREATE TABLE DEMO_M AS
	SELECT
		D.*
		,M.MINCODIN
	FROM
		DEMO D
		INNER JOIN MINCODE M
			ON D.DF_SPE_ACC_ID = M.DF_SPE_ACC_ID
	;
QUIT;


/*DETERMINE THE NEXT_PAY_DUE*/
DATA DEMO;
	SET DEMO_M;

	FORMAT LD_RPS_1_PAY_DU_ $25.;
	FORMAT LD_BIL_DU_LON_ $25.;

	IF DAY(LD_RPS_1_PAY_DU) > DAY(TODAY()) THEN
		LD_RPS_1_PAY_DU_ = PUT(INPUT(STRIP(MONTH(TODAY())) || '/' || STRIP(DAY(LD_RPS_1_PAY_DU)) || '/' || STRIP(YEAR(TODAY())),MMDDYY10.), MMDDYY10.);
	ELSE IF DAY(LD_RPS_1_PAY_DU) < DAY(TODAY()) THEN
		LD_RPS_1_PAY_DU_ = PUT(INPUT(STRIP(MONTH(INTNX('MONTH',TODAY(),1))) || '/' || STRIP(DAY(LD_RPS_1_PAY_DU)) || '/' || STRIP(YEAR(INTNX('MONTH',TODAY(),1))),MMDDYY10.), MMDDYY10.);
	ELSE
		LD_RPS_1_PAY_DU_ = 'Contact Customer Service';

	IF LD_BIL_DU_LON <> . AND LD_BIL_DU_LON > TODAY() AND LC_BIL_TYP_LON = 'P' THEN 
		LD_BIL_DU_LON_ = PUT(LD_BIL_DU_LON, MMDDYY10.);
	ELSE
		LD_BIL_DU_LON_ = LD_RPS_1_PAY_DU_;
RUN;


DATA DEMO (DROP = KEYSSN MODAY KEYLINE CHKDIG DIG I 
	CHKDIG CHK1 CHK2 CHK3 CHKDIGIT CHECK DF_PRS_ID DC_ADR);
SET DEMO;
IF _N_ = 1 THEN DO;
	length R_ADD R_EML PRIORITY $2.;
	LENGTH NEXT_DUE $25.;
	LENGTH LOAN_STA $65.;
	
	declare hash tran();
	tran.definekey("LC_STA_LN83", 'MINCODIN'); *MINCODIN USED TO KEEP ALL LOANS IN THE SAME FILE;
	tran.definedata("R_ADD","R_EML","PRIORITY");
	tran.definedone();
	call missing(LOAN_STA,R_ADD,R_EML,PRIORITY);
	tran.add(key: 'I',KEY: 'C', DATA: '2', DATA: '7', DATA: '5');
	tran.add(key: 'I',KEY: 'B', DATA: '3', DATA: '9', DATA: '4');
	tran.add(key: 'A',KEY: 'C', DATA: '4', DATA: '8', DATA: '2');
	tran.add(key: 'A',KEY: 'A', DATA: '5', DATA: '10', DATA: '1');
	tran.add(key: 'I',KEY: 'A', DATA: '5', DATA: '10', DATA: '1');
	tran.add(key: 'A',KEY: 'B', DATA: '6', DATA: '11', DATA: '3');
/*	These cases are borrowers not in repayment*/
	tran.add(key: 'A',KEY: 'D', DATA: '13', DATA: '14', DATA: '6');
	tran.add(key: 'I',KEY: 'D', DATA: '13', DATA: '14', DATA: '6');
END;
KEYSSN = TRANSLATE(DF_PRS_ID,'MYLAUGHTER','0987654321');
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
tran.find();
if di_vld_adr ^= 'Y' THEN R_EML = '12';

IF R_ADD = '2' OR R_ADD = '3' OR R_ADD = '4' OR R_ADD = '6' THEN
	LOAN_STA = 'REPAYMENT';
ELSE IF R_ADD = '5' THEN
	LOAN_STA = 'DELINQUENT';
ELSE IF R_ADD = '13' THEN
	LOAN_STA = WC_DW_LON_STA;

IF R_ADD = '2' OR R_ADD = '3' OR R_ADD = '5'  THEN
	NEXT_DUE = LD_BIL_DU_LON_;
ELSE IF R_ADD = '4' OR R_ADD = '6' THEN
	NEXT_DUE = LD_RPS_1_PAY_DU_;
ELSE IF R_ADD = '13' THEN
	NEXT_DUE = 'Contact Customer Service';

RUN;

proc sort data=DEMO; by df_spe_acc_id LN_SEQ; run;

/*ENDRSUBMIT;*/
/*DATA DEMO; 	SET LEGEND.DEMO; RUN;*/


/*REMOVE E-MAIL DATA BECAUSE IT MAY CAUSE DUPLICATE LOANS IN THE DATA SET FOR THE PRINTED LETTER IF THE BORROWER HAS MORE THAN ONE E-MAIL ADDRESS*/
DATA DEMO_PRN (DROP = DI_VLD_ADR_EML DX_ADR_EML);
	SET DEMO;
RUN;

/*REMOVE DUPLICATE LOANS CAUSED BY MULTIPLE E-MAIL ADDRESSES AND SORT FOR PRINTING*/
PROC SORT DATA = DEMO_PRN NODUPKEY; BY DF_SPE_ACC_ID LN_SEQ; RUN;

/*GET BORROWERS WITH MORE THAN 50 LOANS AS ONLY 50 LOANS WILL FIT ON THE LETTER*/
PROC SQL;
	CREATE TABLE DEMO_PRN_GT50 AS
	SELECT DISTINCT
		PRN.R_ADD
		,PRN.DF_SPE_ACC_ID 
		,PRN.DM_PRS_1 
		,PRN.DM_PRS_LST 
		,PRN.DX_STR_ADR_1 
		,PRN.DX_STR_ADR_2 
		,PRN.DM_CT 
		,PRN.DC_DOM_ST 
		,PRN.DF_ZIP_CDE 
		,PRN.DM_FGN_CNY 
		,PRN.DM_FGN_ST
		,PRN.ACSKEY
	FROM
		DEMO_PRN PRN
		INNER JOIN
		(
			SELECT
				DF_SPE_ACC_ID
				,COUNT(LN_SEQ) AS CNT
			FROM 
				DEMO_PRN
			GROUP BY
				DF_SPE_ACC_ID
		) CNT
			ON PRN.DF_SPE_ACC_ID = CNT.DF_SPE_ACC_ID
			AND CNT.CNT > 50
	;
QUIT;

/*REMOVE BORROWERS WITH MORE THAN 50 LOANS FROM THE PRINT POPULATION*/
PROC SQL;
	CREATE TABLE DEMO_PRN_LE50 AS
	SELECT
		PRN.*
	FROM
		DEMO_PRN PRN
	WHERE
		NOT EXISTS (
					SELECT
						GT.DF_SPE_ACC_ID
					FROM
						DEMO_PRN_GT50 GT
					WHERE 
						GT.DF_SPE_ACC_ID = PRN.DF_SPE_ACC_ID
					)
	;
QUIT;


/*CREATE A NEW DATASET FOR E-MAILS WITH ONLY ONE RECORD FOR EACH BORROWER/E-MAIL ADDRESS */
PROC SQL;
	CREATE TABLE DEMO_EML AS
	SELECT	DISTINCT
			DF_SPE_ACC_ID
			,DM_PRS_1
			,DM_PRS_LST
			,DX_ADR_EML
			,R_EML
			,DI_VLD_ADR_EML
	FROM DEMO;
QUIT;

PROC FORMAT;
	VALUE $LONPGM
		'DLSCST' = 'Direct Special Consol Stafford'
		'DLSCUN' = 'Direct Special Consol Unsubsidized Stafford'
		'DLSCPL' = 'Direct Special Consol Parent PLUS'
		'DLSCPG' = 'Direct Special Consol Grad PLUS'
		'DLSCSL' = 'Direct Special Consol SLS'
		'DLSCSC' = 'Direct Special Consol Subsidized Consol'
		'DLSCUC' = 'Direct Special Consol Unsubsidized Consol'
		'DLSCCN' = 'Direct Special Consol Consolidation Loan'
		'CNSLDN' = 'Consolidation Loan'
		'DLPCNS' = 'Direct Parent PLUS Consolidation Loan'
		'DLPLGB' = 'Direct Student plus loan'
		'DLPLUS' = 'Direct Parent PLUS Loan'
		'DLSCNS' = 'Direct Subsidized Consolidation Loan'
		'DLSPCN' = 'Direct Spousal Consolidation Loan'
		'DLSSPL' = 'Direct Sub Spousal Consolidation Loan'
		'DLSTFD' = 'Direct Subsidized Stafford Loan'
		'DLUCNS' = 'Direct Unsubsidized Consolidation Loan'
		'DLUNST' = 'Direct Unsubsidized Stafford Loan'
		'DLUSPL' = 'Direct Unsub Spousal Consolidation Loan'
		'PLUS' = 'Federal PLUS Loan'
		'PLUSGB' = 'Graduate PLUS'
		'SLS' = 'Federal SLS Loan'
		'STFFRD' = 'Federal Stafford Loan'
		'SUBCNS' = 'Subsidized Consolidation Loan'
		'SUBSPC' = 'Subsidized Spousal Consolidation Loan'
		'TEACH' = 'Direct TEACH Loan'
		'UNCNS' = 'Unsubsidized Consolidation Loan'
		'UNSPC' = 'Unsubsidized Spousal Consolidation Loan'
		'UNSTFD' = 'Federal Unsubsidized Stafford Loan';

	VALUE $LONSTA		
		'01' = 'IN GRACE'			
		'02' = 'IN SCHOOL'
		'03' = 'IN REPAYMENT'	
		'04' = 'IN DEFERMENT'	
		'05' = 'IN FORBEARANCE'	
		'06' = 'IN CURE'	
		'07' = 'CLAIM PENDING'	
		'08' = 'CLAIM SUBMITTED'	
		'09' = 'CLAIM CANCELLED'
		'10' = 'CLAIM REJECTED'	
		'11' = 'CLAIM RETURNED'	
		'12' = 'CLAIM PAID'	
		'13' = 'PRE-CLAIM PENDING'
		'14' = 'PRE-CLAIM SUBMITTED'	
		'15' = 'PRE-CLAIM CANCELLED'	
		'16' = 'DEATH ALLEGED'	
		'17' = 'DEATH VERIFIED'	
		'18' = 'DISABILITY ALLEGED'	
		'19' = 'DISABILITY VERIFIED'	
		'20' = 'BANKRUPTCY ALLEGED'
		'21' = 'BANKRUPTCY VERIFIED'		
		'22' = 'PAID IN FULL'	
		'23' = 'NOT FULLY ORIGINATED'
		'88' = 'PROCESSING ERROR'	
		'98' = 'UNKNOWN'	;
RUN;


/*TODO: THE CODE TO SPLIT THE FILES DOESN'T WORK, IT WILL HAVE TO BE FIXED WITH A FUTURE ENHANCEMENT*/

/*CREATE FILES FOR DOMESTIC ADDRESSES*/
%macro rep(num);
PROC SQL NOPRINT;
SELECT CEIL(COUNT(*) / 1000) INTO: NUMFILES
FROM DEMO_PRN_LE50
WHERE DI_VLD_ADR = 'Y'
	AND R_ADD = "&NUM";
QUIT;

%DO I = 1 %TO &numfiles;
	DATA _NULL_;
		SET DEMO_PRN_LE50 ;
		WHERE DI_VLD_ADR = 'Y'
			AND R_ADD = "&NUM"
			AND DC_DOM_ST <> '';
		BY df_spe_acc_id;

		FORMAT LOAN_STA $LONSTA.;
		FORMAT IC_LON_PGM $LONPGM.;

		FILE "&&REPORT&num..FILE&i" DELIMITER=',' DSD DROPOVER LRECL=32767;

		RETAIN B 
		RETAIN A 0;
		IF _N_ = 1 THEN
			DO;
				PUT "DF_SPE_ACC_ID,DM_PRS_1,DM_PRS_LST,DX_STR_ADR_1,DX_STR_ADR_2,DM_CT,DC_DOM_ST,DF_ZIP_CDE,DM_FGN_CNY,DM_FGN_ST"
				%DO I = 1 %TO 50; 
					",LOAN_STA&i,LOAN_TYPE&i,LA_CUR_PRI&i,NEXT_DUE&i"
				%END;	
				",ACSKEY" ;	
			END;

/*		IF ceil(_N_/1000) = &i then PUT DF_SPE_ACC_ID DM_PRS_1 DM_PRS_LST DX_STR_ADR_1 DX_STR_ADR_2 DM_CT DC_DOM_ST DF_ZIP_CDE DM_FGN_CNY DM_FGN_ST ACSKEY ;*/

		IF FIRST.DF_SPE_ACC_ID THEN 
			DO;
				A = 0;
				PUT DF_SPE_ACC_ID DM_PRS_1 DM_PRS_LST DX_STR_ADR_1 DX_STR_ADR_2 DM_CT DC_DOM_ST DF_ZIP_CDE DM_FGN_CNY DM_FGN_ST @;
			END;

		DO;
			A + 1;
			IF LAST.DF_SPE_ACC_ID THEN
				do;
					PUT LOAN_STA IC_LON_PGM LA_CUR_PRI NEXT_DUE @;
					do while (A <= 49);
						put 4*',' @;
						A + 1;
					end;		
					PUT ACSKEY $;
				end;
			ELSE PUT LOAN_STA IC_LON_PGM LA_CUR_PRI NEXT_DUE @;
		END;
	RUN;
%end;
%mend;

/*CREATE FILES FOR FOREIGN ADDRESSES*/
%macro repfrgn(num);
PROC SQL NOPRINT;
SELECT CEIL(COUNT(*) / 1000) INTO: NUMFILES
FROM DEMO_PRN_LE50
WHERE DI_VLD_ADR = 'Y'
	AND R_ADD = "&NUM";
QUIT;

%DO I = 1 %TO &numfiles;
	DATA _NULL_;
		SET DEMO_PRN_LE50 ;
		WHERE DI_VLD_ADR = 'Y'
			AND R_ADD = "&NUM"
			AND DC_DOM_ST = '';
		BY df_spe_acc_id;

		FORMAT LOAN_STA $LONSTA.;
		FORMAT IC_LON_PGM $LONPGM.;

		FILE "&&REPORT&num..FILEFRGN&i" DELIMITER=',' DSD DROPOVER LRECL=32767;

		RETAIN A 0;
		IF _N_ = 1 THEN
			DO;
				PUT "DF_SPE_ACC_ID,DM_PRS_1,DM_PRS_LST,DX_STR_ADR_1,DX_STR_ADR_2,DM_CT,DC_DOM_ST,DF_ZIP_CDE,DM_FGN_CNY,DM_FGN_ST"
				%DO I = 1 %TO 50; 
					",LOAN_STA&i,LOAN_TYPE&i,LA_CUR_PRI&i,NEXT_DUE&i"
				%END;	
				",ACSKEY" ;	
			END;

/*		IF ceil(_N_/1000) = &i then PUT DF_SPE_ACC_ID DM_PRS_1 DM_PRS_LST DX_STR_ADR_1 DX_STR_ADR_2 DM_CT DC_DOM_ST DF_ZIP_CDE DM_FGN_CNY DM_FGN_ST ACSKEY ;*/

		IF FIRST.DF_SPE_ACC_ID THEN 
			DO;
				A = 0;
				PUT DF_SPE_ACC_ID DM_PRS_1 DM_PRS_LST DX_STR_ADR_1 DX_STR_ADR_2 DM_CT DC_DOM_ST DF_ZIP_CDE DM_FGN_CNY DM_FGN_ST @;
			END;

		DO;
			A + 1;
			IF LAST.DF_SPE_ACC_ID THEN
				do;
					PUT LOAN_STA IC_LON_PGM LA_CUR_PRI NEXT_DUE @;
					do while (A <= 49);
						put 4*',' @;
						A + 1;
					end;		
					PUT ACSKEY $;
				end;
			ELSE PUT LOAN_STA IC_LON_PGM LA_CUR_PRI NEXT_DUE @;
		END;
	RUN;
%end;

%mend;

%macro eml(num);
DATA _NULL_;
SET DEMO_EML ;

WHERE R_EML = "&NUM"
	and (DI_VLD_ADR_EML = 'Y'
		OR R_EML = '12');
FILE REPORT&num DELIMITER=',' DSD DROPOVER LRECL=32767;
IF _N_ = 1 THEN PUT "DF_SPE_ACC_ID,DM_PRS_1,DM_PRS_LST,DX_ADR_EML";
PUT DF_SPE_ACC_ID DM_PRS_1 DM_PRS_LST DX_ADR_EML ;
RUN;
%mend;

%REP(2);
%REP(3);
%REP(4);
%REP(5);
%REP(6);
%EML(7);
%EML(8);
%EML(9);
%EML(10);
%EML(11);
%EML(12);
%REP(13);
%EML(14);

%REPFRGN(2);
%REPFRGN(3);
%REPFRGN(4);
%REPFRGN(5);
%REPFRGN(6);
%REPFRGN(13);

/*PRINT ERROR REPORT*/
PROC EXPORT DATA= WORK.DEMO_PRN_GT50 
            OUTFILE= REPORT99 
            DBMS=CSV REPLACE;
     PUTNAMES=YES;
RUN;


/*ODS LISTING CLOSE;*/
/*ODS RTF FILE=REPORT99;*/
/*OPTIONS ORIENTATION = LANDSCAPE;*/
/*OPTIONS PS=39 LS=127;*/
/*TITLE 		'Welcome Email  –  Over 50 Loans';*/
/*TITLE2		"RUNDATE &SYSDATE9";*/
/*FOOTNOTE1  	"THIS DOCUMENT MAY CONTAIN BORROWERS' SENSITIVE INFORMATION THAT UHEAA HAS PLEDGED TO PROTECT.";*/
/*FOOTNOTE2	'PLEASE TAKE APPROPRIATE PRECAUTIONS TO SAFEGUARD THIS INFORMATION.';*/
/*FOOTNOTE3	;*/
/*FOOTNOTE4   'JOB = Welcome Letters - FED  	 REPORT = Welcome Email  –  Over 50 Loans';*/
/**/
/*PROC PRINT NOOBS SPLIT='/' DATA=DEMO_PRN_GT50 WIDTH=UNIFORM WIDTH=MIN LABEL;*/
/*VAR 	R_ADD*/
/*		DF_SPE_ACC_ID */
/*		DM_PRS_1 */
/*		DM_PRS_LST */
/*		DX_STR_ADR_1 */
/*		DX_STR_ADR_2 */
/*		DM_CT */
/*		DC_DOM_ST */
/*		DF_ZIP_CDE */
/*		DM_FGN_CNY */
/*		DM_FGN_ST*/
/*		ACSKEY;*/
/*RUN;*/
/**/
/*ODS RTF CLOSE;*/
/*ODS LISTING;*/
