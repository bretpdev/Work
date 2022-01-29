/*%LET RPTLIB = %SYSGET(reportdir);*/
%LET RPTLIB = T:\SAS;
FILENAME REPORTZ "&RPTLIB/UNWS96.NWS96RZ";
FILENAME REPORT2 "&RPTLIB/UNWS96.NWS96R2";

LIBNAME  LEGEND  REMOTE  SERVER=LEGEND SLIBREF=work  ;
RSUBMIT LEGEND;
/*%let DB = DNFPRQUT;  *This is test;*/
/*%let DB = DNFPRUUT;  *This is VUK3 test;*/
%let DB = DNFPUTDL;  *This is live;

LIBNAME PKUB DB2 DATABASE=&DB OWNER=PKUB;

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
	CREATE TABLE POP AS
		SELECT DISTINCT
			AY10.BF_SSN,
			AY10.LN_ATY_SEQ,
			COALESCE(SC10.IM_SCL_FUL, '') || LN10.LF_DOE_SCL_ORG AS SCHOOL,
			AY10.PF_REQ_ACT 
        FROM 
			PKUB.AY10_BR_LON_ATY AY10
	        LEFT JOIN  /*GETS THE NEWEST DISB DATE FOR ALL LOANS*/
			( 
	        	SELECT 
					BF_SSN,
	                MAX(LD_LON_1_DSB) AS LD_LON_1_DSB
	        	FROM 
					PKUB.LN10_LON 
	            WHERE 
					LA_CUR_PRI > 0
	            GROUP BY 
					BF_SSN
	        ) LNSEQ
	        	ON AY10.BF_SSN = LNSEQ.BF_SSN
	        INNER JOIN PKUB.LN10_LON LN10
	        	ON AY10.BF_SSN = LN10.BF_SSN
	        	AND LNSEQ.LD_LON_1_DSB = LN10.LD_LON_1_DSB
	        LEFT JOIN PKUB.SC10_SCH_DMO SC10
	        	ON LN10.LF_DOE_SCL_ORG = SC10.IF_DOE_SCL
	        WHERE 
	              AY10.PF_REQ_ACT IN('BDCLS', 'CCISC')
				  AND AY10.LD_ATY_REQ_RCV BETWEEN (TODAY() - 7) AND TODAY() 
	              AND LNSEQ.BF_SSN IS NOT NULL     
    ;

      /*%PUT  SQLXRC= >>> &SQLXRC <<< ||| SQLXMSG= >>> &SQLXMSG >>> ;  ** INCLUDES ERROR MESSAGES TO SAS LOG  ;*/
      /*%SQLCHECK;*/
QUIT;

ENDRSUBMIT;

DATA POP; SET LEGEND.POP; RUN;
PROC SQL;
    CREATE TABLE FINAL AS

        SELECT
        	'NUMBER OF CCISC ARCS:' AS TITLE,
        	COUNT (LN_ATY_SEQ) AS COUNT
        FROM 
			POP
		WHERE
			PF_REQ_ACT = 'CCISC'

		UNION ALL

        SELECT
        	SCHOOL AS TITLE,
        	COUNT (LN_ATY_SEQ) AS COUNT
        FROM 
			POP
		WHERE
			PF_REQ_ACT = 'BDCLS'
		GROUP BY 
			SCHOOL	 
;
QUIT;

PROC PRINTTO PRINT=REPORT2 NEW; RUN;

OPTIONS ORIENTATION=LANDSCAPE PS=39 LS=127;
TITLE 		'Defense Inquiries By School - FED';
TITLE2		"RUNDATE &SYSDATE9";
FOOTNOTE1  	"THIS DOCUMENT MAY CONTAIN BORROWERS' SENSITIVE INFORMATION THAT UHEAA HAS PLEDGED TO PROTECT.";
FOOTNOTE2	'PLEASE TAKE APPROPRIATE PRECAUTIONS TO SAFEGUARD THIS INFORMATION.';
FOOTNOTE3	;
FOOTNOTE4   'JOB = UTNWS96  	 REPORT = UNWS96.NWS96R2';

PROC PRINT 
		NOOBS SPLIT = '/' 
		DATA = FINAL 
		WIDTH = UNIFORM 
		WIDTH = MIN 
		LABEL;


	VAR 
		TITLE	
		COUNT
	;

	LABEL
		TITLE = 'TITLE'
		COUNT = 'COUNT'
	;
RUN;

PROC PRINTTO; RUN;
