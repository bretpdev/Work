/*%LET RPTLIB = %SYSGET(reportdir);*/
%LET RPTLIB = T:\SAS;

DATA _NULL_;
RUNDAY = TODAY();
/*RUNDAY = '11SEP2001'D;*/
CALL SYMPUT('BEGIN',INTNX('MONTH',RUNDAY,-1,'B'));
CALL SYMPUT('FINISH',INTNX('MONTH',RUNDAY,-1,'E'));
RUN;

%SYSLPUT BEGIN = &BEGIN;
%SYSLPUT FINISH= &FINISH;

FILENAME REPORTZ "&RPTLIB/UNWS61.NWS61RZ";
FILENAME REPORT2 "&RPTLIB/UNWS61.NWS61R2";
FILENAME REPORT3 "&RPTLIB/UNWS61.NWS61R3";
FILENAME REPORT4 "&RPTLIB/UNWS61.NWS61R4";
LIBNAME  LEGEND  REMOTE  SERVER=LEGEND SLIBREF=work  ;
RSUBMIT;
LIBNAME PKUB DB2 DATABASE=DNFPRQUT OWNER=PKUB;
/*LIBNAME PKUB DB2 DATABASE=DNFPUTDL OWNER=PKUB;*/

PROC SQL;
	CREATE TABLE LCT AS
		SELECT 
			DISTINCT 
			C.*
			,SUBSTR(C.LX_ATY,3,3) AS LOC_TYP
			,A.LD_ATY_REQ_RCV
			,A.LF_USR_REQ_ATY
			,A.LF_ATY_RGD_TO
			,D.DF_SPE_ACC_ID	
		FROM 
			PKUB.AY10_BR_LON_ATY A
			LEFT OUTER JOIN PKUB.AY15_ATY_CMT B
				on a.bf_ssn = b.bf_ssn
				and a.ln_aty_seq = b.ln_aty_seq
			LEFT OUTER JOIN PKUB.AY20_ATY_TXT C
				on b.bf_ssn = c.bf_ssn
				and b.ln_aty_seq = c.ln_aty_seq
				and b.ln_aty_cmt_seq = c.ln_aty_cmt_seq
			INNER JOIN PKUB.PD10_PRS_NME D
				ON A.BF_SSN = D.DF_PRS_ID
		WHERE 
			B.LC_STA_AY15 = 'A'
			AND A.LD_ATY_REQ_RCV BETWEEN &BEGIN AND &FINISH
			AND A.PF_REQ_ACT = 'SFNDM'
	;
QUIT;

ENDRSUBMIT;
DATA LCT; SET LEGEND.LCT; RUN;

DATA LCT;
	SET LCT;

	IF LOC_TYP = '3RD' THEN LOC_TYPE = 'In Bound Call Not Borrower, Reference, or Endorser';
	ELSE IF LOC_TYP = 'ADD' THEN LOC_TYPE = 'Address Hygiene';
	ELSE IF LOC_TYP = 'ALT' THEN LOC_TYPE = 'Alternate Demographics';
	ELSE IF LOC_TYP = 'ATT' THEN LOC_TYPE = 'Attorney';
	ELSE IF LOC_TYP = 'BRC' THEN LOC_TYPE = 'Borrower Contact';
	ELSE IF LOC_TYP = 'EML' THEN LOC_TYPE = 'Email';
	ELSE IF LOC_TYP = 'EMP' THEN LOC_TYPE = 'Employer Call';
	ELSE IF LOC_TYP = 'END' THEN LOC_TYPE = 'Endorser Contact';
	ELSE IF LOC_TYP = 'LDC' THEN LOC_TYPE = 'Legal Document';
	ELSE IF LOC_TYP = 'POO' THEN LOC_TYPE = 'Post Office';
	ELSE IF LOC_TYP = 'PRS' THEN LOC_TYPE = 'Prison';
	ELSE IF LOC_TYP = 'RFC' THEN LOC_TYPE = 'Reference Contact';
	ELSE LOC_TYPE = 'Unknown';
RUN;

PROC SQL;
	CREATE TABLE TYPE_CNT AS
		SELECT
			LOC_TYPE
			,COUNT(BF_SSN) AS CNT
		FROM
			LCT
		GROUP BY
			LOC_TYPE
	;

	CREATE TABLE AGENT_CNT AS
		SELECT
			LF_USR_REQ_ATY
			,COUNT(BF_SSN) AS CNT
		FROM
			LCT
		GROUP BY
			LF_USR_REQ_ATY
	;
QUIT;

/*create printed report*/
PROC PRINTTO PRINT=REPORT2 NEW; RUN;
OPTIONS ORIENTATION = LANDSCAPE;
OPTIONS PS=39 LS=127;
TITLE 		'Skip Locates by Type- Last Month';
FOOTNOTE1  	"THIS DOCUMENT MAY CONTAIN BORROWERS' SENSITIVE INFORMATION THAT UHEAA HAS PLEDGED TO PROTECT.";
FOOTNOTE2	'PLEASE TAKE APPROPRIATE PRECAUTIONS TO SAFEGUARD THIS INFORMATION.';
FOOTNOTE3	;
FOOTNOTE4   'JOB = UTNWS61  	 REPORT = UNWS61.NWS61R2';

PROC PRINT NOOBS SPLIT='/' DATA=TYPE_CNT WIDTH=UNIFORM WIDTH=MIN LABEL;
	SUM
		CNT
	;
	
	VAR 	
		LOC_TYPE
		CNT
	;

	LABEL
		LOC_TYPE = 'Locate Type Description'
		CNT = 'Count'
	;
RUN;

PROC PRINTTO;
RUN;

PROC PRINTTO PRINT=REPORT3 NEW; RUN;
OPTIONS ORIENTATION = LANDSCAPE;
OPTIONS PS=39 LS=127;
TITLE 		'Skip Locates by User ID – Last Month';
FOOTNOTE1  	"THIS DOCUMENT MAY CONTAIN BORROWERS' SENSITIVE INFORMATION THAT UHEAA HAS PLEDGED TO PROTECT.";
FOOTNOTE2	'PLEASE TAKE APPROPRIATE PRECAUTIONS TO SAFEGUARD THIS INFORMATION.';
FOOTNOTE3	;
FOOTNOTE4   'JOB = UTNWS61  	 REPORT = UNWS61.NWS61R3';

PROC PRINT NOOBS SPLIT='/' DATA=AGENT_CNT WIDTH=UNIFORM WIDTH=MIN LABEL;
	SUM
		CNT
	;

	VAR 	
		LF_USR_REQ_ATY
		CNT
	;

	LABEL
		LF_USR_REQ_ATY = 'User ID'
		CNT = 'Count'
	;
RUN;

PROC PRINTTO;
RUN;

PROC PRINTTO PRINT=REPORT4 NEW; RUN;
OPTIONS ORIENTATION = LANDSCAPE;
OPTIONS PS=39 LS=127;
TITLE 		'Skip Locates Detail- Last Month';
FOOTNOTE1  	"THIS DOCUMENT MAY CONTAIN BORROWERS' SENSITIVE INFORMATION THAT UHEAA HAS PLEDGED TO PROTECT.";
FOOTNOTE2	'PLEASE TAKE APPROPRIATE PRECAUTIONS TO SAFEGUARD THIS INFORMATION.';
FOOTNOTE3	;
FOOTNOTE4   'JOB = UTNWS61  	 REPORT = UNWS61.NWS61R4';

PROC PRINT NOOBS SPLIT='/' DATA=LCT WIDTH=UNIFORM WIDTH=MIN LABEL;
	VAR 	
		DF_SPE_ACC_ID
		LD_ATY_REQ_RCV
		LOC_TYP
		LF_USR_REQ_ATY
	;

	LABEL
		DF_SPE_ACC_ID = 'Account #'
		LD_ATY_REQ_RCV = 'Activity Date'
		LOC_TYP = 'Locate Type'
		LF_USR_REQ_ATY = 'User ID'
	;
RUN;

PROC PRINTTO;
RUN;

