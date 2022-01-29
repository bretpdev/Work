/*%LET RPTLIB = %SYSGET(reportdir);*/
%LET RPTLIB = T:\SAS;
FILENAME REPORTZ "&RPTLIB/ULWD41.LWD41RZ";
FILENAME REPORT2 "&RPTLIB/ULWD41.LWD41R2";
LIBNAME  DUSTER  REMOTE  SERVER=DUSTER  SLIBREF=WORK;

RSUBMIT;
/*LIBNAME OLWHRM1 DB2 DATABASE=DLGSUTWH OWNER=OLWHRM1;*/

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

	CREATE TABLE EVC AS
		SELECT 
			*
		FROM 
			CONNECTION TO DB2 
				(
					SELECT DISTINCT
						PD01.DF_SPE_ACC_ID,
						PD01.DM_PRS_1,
						PD01.DM_PRS_LST,
						SUBSTR(AY01.BX_CMT,1,LOCATE('CLAIM',AY01.BX_CMT)-1) AS SERVICER,
						AY01.BD_ATY_PRF
					FROM	
						OLWHRM1.PD01_PDM_INF PD01
						JOIN OLWHRM1.DC01_LON_CLM_INF DC01
							ON PD01.DF_PRS_ID = DC01.BF_SSN
						JOIN OLWHRM1.AY01_BR_ATY AY01
							ON PD01.DF_PRS_ID = AY01.DF_PRS_ID
					WHERE	
						DC01.LC_STA_DC10 = '03'
						AND DC01.LA_TOT_ITL_CLM_PD - DC01.LA_PRI_COL > 0
						AND AY01.PF_ACT = 'DBEVC'
						AND DC01.LC_AUX_STA NOT IN ('02','03','06','07','08','09')
						AND DC01.LC_REA_CLM_ASN_DOE = ''
					ORDER BY
						AY01.BD_ATY_PRF,
						PD01.DF_SPE_ACC_ID

					FOR READ ONLY WITH UR
				)
	;

	DISCONNECT FROM DB2;

	/*%PUT  SQLXRC= >>> &SQLXRC <<< ||| SQLXMSG= >>> &SQLXMSG >>> ;  ** INCLUDES ERROR MESSAGES TO SAS LOG  ;*/
	/*%SQLCHECK;*/
QUIT;

ENDRSUBMIT;

DATA EVC; SET DUSTER.EVC; RUN;

PROC PRINTTO PRINT=REPORT2 NEW;
RUN;

OPTIONS ORIENTATION = PORTRAIT;
OPTIONS PS=52 LS=96;
OPTIONS PAGENO = 1;
TITLE 'Completed EVCs';
FOOTNOTE1  	"THIS DOCUMENT MAY CONTAIN BORROWERS' SENSITIVE INFORMATION THAT UHEAA HAS PLEDGED TO PROTECT.";
FOOTNOTE2	'PLEASE TAKE APPROPRIATE PRECAUTIONS TO SAFEGUARD THIS INFORMATION.';
FOOTNOTE3	;
FOOTNOTE4   'JOB = UTLWD41  	 REPORT = ULWD41.LWD41R2';

PROC CONTENTS DATA=EVC OUT=EMPTYSET NOPRINT;
DATA _NULL_;
	SET EMPTYSET;
	FILE PRINT;

	IF  NOBS=0 AND _N_ =1 THEN 
		DO;
			PUT // 132*'-';
			PUT      //////
				@51 '**** NO OBSERVATIONS FOUND ****';
			PUT //////
				@57 '-- END OF REPORT --';
			PUT //////////////
				@46 "JOB = UTLWD41  	 REPORT = ULWD41.LWD41R2";
		END;
	RETURN;
RUN;

PROC PRINT NOOBS SPLIT='/' DATA=EVC WIDTH=UNIFORM WIDTH=MIN LABEL;
	FORMAT
		BD_ATY_PRF MMDDYY10.;
	VAR 
		DF_SPE_ACC_ID
		DM_PRS_1
		DM_PRS_LST
		SERVICER
		BD_ATY_PRF;
	LABEL
		DF_SPE_ACC_ID = 'Account Number'
		DM_PRS_1 = 'First Name'
		DM_PRS_LST = 'Last Name'
		SERVICER = 'Servicer'
		BD_ATY_PRF = 'Date Completed';
RUN;

PROC PRINTTO;
RUN;