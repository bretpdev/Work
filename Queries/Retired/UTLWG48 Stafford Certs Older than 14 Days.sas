/*UTLWG48 STAFFORD CERTS GT 14 DAYS*/
/*LIBNAME DLGSUTWH DB2 DATABASE=DLGSUTWH OWNER=OLWHRM1;*/
/*%LET RPTLIB = %SYSGET(reportdir);*/
%LET RPTLIB = T:\SAS;
FILENAME REPORT2 "&RPTLIB/ULWG48.LWG48R2";
FILENAME REPORT3 "&RPTLIB/ULWG48.LWG48R3";
FILENAME REPORT4 "&RPTLIB/ULWG48.LWG48R4";
FILENAME REPORT5 "&RPTLIB/ULWG48.LWG48R5";
FILENAME REPORT6 "&RPTLIB/ULWG48.LWG48R6";
FILENAME REPORT7 "&RPTLIB/ULWG48.LWG48R7";
FILENAME REPORT8 "&RPTLIB/ULWG48.LWG48R8";
FILENAME REPORT9 "&RPTLIB/ULWG48.LWG48R9";
FILENAME REPORT10 "&RPTLIB/ULWG48.LWG48R10";
FILENAME REPORT11 "&RPTLIB/ULWG48.LWG48R11";
FILENAME REPORT12 "&RPTLIB/ULWG48.LWG48R12";
FILENAME REPORT13 "&RPTLIB/ULWG48.LWG48R13";
FILENAME REPORT14 "&RPTLIB/ULWG48.LWG48R14";
FILENAME REPORT15 "&RPTLIB/ULWG48.LWG48R15";
FILENAME REPORT16 "&RPTLIB/ULWG48.LWG48R16";
FILENAME REPORT17 "&RPTLIB/ULWG48.LWG48R17";
FILENAME REPORT18 "&RPTLIB/ULWG48.LWG48R18";
FILENAME REPORT19 "&RPTLIB/ULWG48.LWG48R19";
FILENAME REPORT20 "&RPTLIB/ULWG48.LWG48R20";
FILENAME REPORT21 "&RPTLIB/ULWG48.LWG48R21";
FILENAME REPORT22 "&RPTLIB/ULWG48.LWG48R22";
FILENAME REPORT23 "&RPTLIB/ULWG48.LWG48R23";
FILENAME REPORT24 "&RPTLIB/ULWG48.LWG48R24";
FILENAME REPORT25 "&RPTLIB/ULWG48.LWG48R25";
FILENAME REPORT26 "&RPTLIB/ULWG48.LWG48R26";
FILENAME REPORT27 "&RPTLIB/ULWG48.LWG48R27";
FILENAME REPORT28 "&RPTLIB/ULWG48.LWG48R28";
FILENAME REPORT29 "&RPTLIB/ULWG48.LWG48R29";
FILENAME REPORT30 "&RPTLIB/ULWG48.LWG48R30";
FILENAME REPORT31 "&RPTLIB/ULWG48.LWG48R31";
FILENAME REPORT32 "&RPTLIB/ULWG48.LWG48R32";
FILENAME REPORT33 "&RPTLIB/ULWG48.LWG48R33";
FILENAME REPORT34 "&RPTLIB/ULWG48.LWG48R34";
FILENAME REPORT35 "&RPTLIB/ULWG48.LWG48R35";
FILENAME REPORT36 "&RPTLIB/ULWG48.LWG48R36";
FILENAME REPORT37 "&RPTLIB/ULWG48.LWG48R37";
FILENAME REPORT38 "&RPTLIB/ULWG48.LWG48R38";
FILENAME REPORT39 "&RPTLIB/ULWG48.LWG48R39";
FILENAME REPORT40 "&RPTLIB/ULWG48.LWG48R40";
FILENAME REPORT41 "&RPTLIB/ULWG48.LWG48R41";
FILENAME REPORT42 "&RPTLIB/ULWG48.LWG48R42";
FILENAME REPORT43 "&RPTLIB/ULWG48.LWG48R43";
FILENAME REPORT44 "&RPTLIB/ULWG48.LWG48R44";
FILENAME REPORT45 "&RPTLIB/ULWG48.LWG48R45";
FILENAME REPORT46 "&RPTLIB/ULWG48.LWG48R46";
FILENAME REPORT47 "&RPTLIB/ULWG48.LWG48R47";
FILENAME REPORT48 "&RPTLIB/ULWG48.LWG48R48";
FILENAME REPORT49 "&RPTLIB/ULWG48.LWG48R49";
FILENAME REPORT50 "&RPTLIB/ULWG48.LWG48R50";
FILENAME REPORT51 "&RPTLIB/ULWG48.LWG48R51";
FILENAME REPORT52 "&RPTLIB/ULWG48.LWG48R52";
FILENAME REPORT53 "&RPTLIB/ULWG48.LWG48R53";
FILENAME REPORT54 "&RPTLIB/ULWG48.LWG48R54";
FILENAME REPORT55 "&RPTLIB/ULWG48.LWG48R55";
FILENAME REPORT56 "&RPTLIB/ULWG48.LWG48R56";
FILENAME REPORT57 "&RPTLIB/ULWG48.LWG48R57";
FILENAME REPORT58 "&RPTLIB/ULWG48.LWG48R58";
FILENAME REPORT59 "&RPTLIB/ULWG48.LWG48R59";
FILENAME REPORT60 "&RPTLIB/ULWG48.LWG48R60";
FILENAME REPORT61 "&RPTLIB/ULWG48.LWG48R61";
FILENAME REPORT63 "&RPTLIB/ULWG48.LWG48R63";
FILENAME REPORT64 "&RPTLIB/ULWG48.LWG48R64";
FILENAME REPORT65 "&RPTLIB/ULWG48.LWG48R65";
FILENAME REPORT66 "&RPTLIB/ULWG48.LWG48R66";
FILENAME REPORT67 "&RPTLIB/ULWG48.LWG48R67";
FILENAME REPORT68 "&RPTLIB/ULWG48.LWG48R68";
FILENAME REPORT69 "&RPTLIB/ULWG48.LWG48R69";
FILENAME REPORT70 "&RPTLIB/ULWG48.LWG48R70";
FILENAME REPORT71 "&RPTLIB/ULWG48.LWG48R71";
FILENAME REPORT72 "&RPTLIB/ULWG48.LWG48R72";
FILENAME REPORT73 "&RPTLIB/ULWG48.LWG48R73";
FILENAME REPORTZ "&RPTLIB/ULWG48.LWG48RZ";

LIBNAME  WORKLOCL  REMOTE  SERVER=CYPRUS  SLIBREF=WORK;
RSUBMIT;
%MACRO SQLCHECK (SQLRPT= );
%IF &SQLXRC NE 0 %THEN %DO;
	DATA _NULL_;
    FILE REPORTZ NOTITLES;
    PUT @01 " ********************************************************************* "
      / @01 " ****  THE SQL CODE ABOVE HAS EXPERIENCED AN ERROR.               **** "
      / @01 " ****  THE SAS LOG IN &SQLRPT SHOULD BE REVIEWED.          **** "       
      / @01 " ********************************************************************* "
      / @01 " ****  THE SQL ERROR CODE IS  &SQLXRC  AND THE SQL ERROR MESSAGE  **** "
      / @01 " ****  &SQLXMSG   **** "
      / @01 " ********************************************************************* ";
	RUN;
%END;
%MEND;
PROC SQL;
CONNECT TO DB2 (DATABASE=DLGSUTWH);
CREATE TABLE CERTS AS
SELECT *
FROM CONNECTION TO DB2 (
SELECT DISTINCT
	CASE 
		WHEN A.AF_APL_OPS_SCL = '01009801' THEN '01009800'
		ELSE A.AF_APL_OPS_SCL
	 END AS AF_APL_OPS_SCL
	,D.DM_PRS_LST
	,D.DM_PRS_1
	,A.DF_PRS_ID_BR
	,A.AD_APL_RCV
	,A.AX_SCL_SUB_CER_IAA
	,A.AX_SCL_USB_CER_IAA
	,A.AD_SCL_SIG
	,CASE 
		WHEN B.AC_PRC_STA = 'H' THEN 'App Held '||B.AC_APL_SPS_REA_1
		WHEN B.AC_PRC_STA = 'P' THEN 'Provisional'
		WHEN B.AC_PRC_STA = 'I' THEN 'Incomplete'
		WHEN B.AC_PRC_STA = 'M' THEN 'Acc Held '||D.DC_PRS_HLD_REA_1
		WHEN B.AC_PRC_STA = 'N' THEN 'Not Processed'
		WHEN B.AC_PRC_STA = 'Z' THEN 'App Sent'
		WHEN B.AC_PRC_STA = 'S' THEN 'Staff Review'
	 END							AS AC_PRC_STA
	,'('||SUBSTR(D.DN_PHN,1,3)||') '||SUBSTR(D.DN_PHN,4,3)||'-'||SUBSTR(D.DN_PHN,7,4) AS DN_PHN
FROM	OLWHRM1.GA01_APP A
		INNER JOIN OLWHRM1.GA10_LON_APP B ON
			A.AF_APL_ID = B.AF_APL_ID
			AND DAYS(A.AD_SCL_SIG) < DAYS(CURRENT DATE) - 14
			AND B.AC_LON_TYP IN ('SF','SU')
			AND B.AC_PRC_STA NOT IN ('A','R','X')
		INNER JOIN OLWHRM1.PD01_PDM_INF D ON
			A.DF_PRS_ID_BR = D.DF_PRS_ID
ORDER BY D.DM_PRS_LST
);
DISCONNECT FROM DB2;
/*%PUT  SQLXRC= >>> &SQLXRC <<< ||| SQLXMSG= >>> &SQLXMSG >>>  ;  * INCLUDES ERROR MESSAGES TO SAS LOG  ;*/
/*%SQLCHECK (SQLRPT=ULWG48.LWG48RZ);*/
/*QUIT;*/
ENDRSUBMIT;

DATA CERTS;
SET WORKLOCL.CERTS;
RUN;

DATA CERTS;
SET CERTS;
FORMAT SUB_AMT USB_AMT DOLLAR11.2 ;
SUB_AMT = AX_SCL_SUB_CER_IAA;
USB_AMT = AX_SCL_USB_CER_IAA;
RUN;

DATA _NULL_;
CALL SYMPUT('RUNDATE',PUT(INTNX('DAY',TODAY(),0,'beginning'), MMDDYY10.));
RUN;

OPTIONS CENTER NODATE ORIENTATION=LANDSCAPE ;
OPTIONS LS=127 PS=39;

%MACRO PRINT(SCH=, SCHNAME=, RPNO=);
PROC PRINTTO PRINT=REPORT&RPNO NEW;
RUN;
OPTIONS PAGENO=1 ;

TITLE1	'STAFFORD CERTS OLDER THAN 14 DAYS';
TITLE2	"&SCHNAME (&SCH)";
TITLE3	"&RUNDATE";
FOOTNOTE "JOB = UTLWG48     REPORT = ULWG48.LWG48R&RPNO";

DATA CERTSS;
SET CERTS;
WHERE AF_APL_OPS_SCL = "&SCH";
RUN;

PROC CONTENTS DATA=CERTSS OUT=EMPTYSET NOPRINT;
DATA _NULL_;
SET EMPTYSET;
FILE PRINT;
IF  NOBS=0 AND _N_ =1 THEN DO;
   PUT // 126*'-';
   PUT      ////////
       @53 '**** NO RECORDS FOUND ****';
   PUT ////////
       @56 '-- END OF REPORT --';
   PUT ///////
   		@44 "JOB = UTLWG48     REPORT = ULWG48.LWG48R&RPNO";
   END;
RETURN;

PROC PRINT NOOBS SPLIT='/' DATA=CERTSS WIDTH=UNIFORM WIDTH=MIN;
VAR 	DM_PRS_LST
		DM_PRS_1
		DF_PRS_ID_BR
		AD_APL_RCV
		SUB_AMT
		USB_AMT
		AD_SCL_SIG
		AC_PRC_STA
		DN_PHN;
LABEL	DM_PRS_LST = 'Borr LN'
		DM_PRS_1 = 'Borr FN'
		DF_PRS_ID_BR = 'Borr SSN'
		AD_APL_RCV = 'App Recd Dt'
		SUB_AMT = 'Sub Cert'
		USB_AMT = 'Unsub Cert'
		AD_SCL_SIG = 'Cert Date'
		AC_PRC_STA = 'Loan Status'
		DN_PHN = 'Borr Phn';
FORMAT	AD_APL_RCV AD_SCL_SIG MMDDYY10.;
RUN;
PROC PRINTTO;
RUN;
%MEND PRINT;

%PRINT(SCH=02270800,SCHNAME=AMERICAN INSTITUTE OF MED & DENT - PROVO,RPNO=2);
%PRINT(SCH=02270801,SCHNAME=AMERICAN INSTITUTE OF MED & DENT - ST GEORGE,RPNO=3);
%PRINT(SCH=00160600,SCHNAME=BRIGHAM YOUNG UNIVERSITY - HAWAII,RPNO=4);
%PRINT(SCH=00162500,SCHNAME=BRIGHAM YOUNG UNIVERSITY - IDAHO,RPNO=5);
%PRINT(SCH=00367000,SCHNAME=BRIGHAM YOUNG UNIVERSITY - PROVO,RPNO=6);
%PRINT(SCH=00367600,SCHNAME=COLLEGE OF EASTERN UTAH,RPNO=7);
%PRINT(SCH=00367100,SCHNAME=DIXIE STATE COLLEGE OF UTAH,RPNO=8);
%PRINT(SCH=02178500,SCHNAME=EAGLE GATE COLLEGE,RPNO=9);
%PRINT(SCH=02178501,SCHNAME=EAGLE GATE COLLEGE - DAVIS WEBER,RPNO=10);
%PRINT(SCH=00367200,SCHNAME=LATTER DAY SAINTS BUSINESS COLLEGE,RPNO=11);
%PRINT(SCH=02298500,SCHNAME=MOUNTAIN WEST COLLEGE,RPNO=12);
%PRINT(SCH=03082100,SCHNAME=MYOTHERAPY COLLEGE OF UTAH,RPNO=13);
%PRINT(SCH=01009800,SCHNAME=NEUMONT UNIVERSITY,RPNO=14);
%PRINT(SCH=02531801,SCHNAME=PAUL MITCHELL - COSTA MESA,RPNO=15);
%PRINT(SCH=02531803,SCHNAME=PAUL MITCHELL - ORLANDO,RPNO=16);
%PRINT(SCH=02531800,SCHNAME=PAUL MITCHELL - PROVO,RPNO=17);
%PRINT(SCH=02531802,SCHNAME=PAUL MITCHELL - RHODE ISLAND,RPNO=18);
%PRINT(SCH=02360800,SCHNAME=PROVO COLLEGE,RPNO=19); 
%PRINT(SCH=00522000,SCHNAME=SALT LAKE COMMUNITY COLLEGE,RPNO=20);
%PRINT(SCH=00367405,SCHNAME=STEVENS HENAGER COLLEGE - LOGAN,RPNO=21);
%PRINT(SCH=00367403,SCHNAME=STEVENS HENAGER COLLEGE - MURRAY,RPNO=22);
%PRINT(SCH=00367400,SCHNAME=STEVENS HENAGER COLLEGE - OGDEN,RPNO=23);
%PRINT(SCH=00367401,SCHNAME=STEVENS HENAGER COLLEGE - PROVO,RPNO=24);
%PRINT(SCH=00367404,SCHNAME=STEVENS HENAGER COLLEGE - WOODS CROSS,RPNO=25);
%PRINT(SCH=03829500,SCHNAME=SKINWORKS SCHOOL OF ADVANCED SKINCARE,RPNO=26);
%PRINT(SCH=00367900,SCHNAME=SNOW COLLEGE,RPNO=27);
%PRINT(SCH=00367800,SCHNAME=SOUTHERN UTAH UNIVERSITY,RPNO=28); 
%PRINT(SCH=03030607,SCHNAME=UTAH COLLEGE OF MASSAGE THERAPY - AURORA,RPNO=29);
%PRINT(SCH=03030606,SCHNAME=UTAH COLLEGE OF MASSAGE THERAPY - DENVER,RPNO=30); 
%PRINT(SCH=03030602,SCHNAME=UTAH COLLEGE OF MASSAGE THERAPY - LAYTON,RPNO=31);
%PRINT(SCH=03030603,SCHNAME=UTAH COLLEGE OF MASSAGE THERAPY - LAS VEGAS,RPNO=32);
%PRINT(SCH=03030605,SCHNAME=UTAH COLLEGE OF MASSAGE THERAPY - PHOENIX,RPNO=33);
%PRINT(SCH=03030600,SCHNAME=UTAH COLLEGE OF MASSAGE THERAPY - SALT LAKE CITY,RPNO=34);
%PRINT(SCH=03030604,SCHNAME=UTAH COLLEGE OF MASSAGE THERAPY - TEMPE,RPNO=35); 
%PRINT(SCH=03030601,SCHNAME=UTAH COLLEGE OF MASSAGE THERAPY - UTAH VALLEY,RPNO=36);
%PRINT(SCH=00367500,SCHNAME=UNIVERSITY OF UTAH,RPNO=37);
%PRINT(SCH=00367700,SCHNAME=UTAH STATE UNIVERSITY,RPNO=38);
%PRINT(SCH=00402700,SCHNAME=UTAH VALLEY UNIVERSITY,RPNO=39); 
%PRINT(SCH=00368000,SCHNAME=WEBER STATE UNIVERSITY,RPNO=40);
%PRINT(SCH=00368100,SCHNAME=WESTMINSTER COLLEGE,RPNO=41);
%PRINT(SCH=03838300,SCHNAME=OGDEN INSTITUTE OF MASSAGE THERAPY,RPNO=42);
%PRINT(SCH=02178502,SCHNAME=EAGLE GATE COLLEGE - SLC,RPNO=43);
%PRINT(SCH=02531804,SCHNAME=PAUL MITCHELL THE SCHOOL - SAN DIEGO CALIFORNIA,RPNO=44);
%PRINT(SCH=03909300,SCHNAME=MAXIMUM STYLE TECH,RPNO=45);
%PRINT(SCH=02556800,SCHNAME=PAUL MITCHELL THE SCHOOL - SLC,RPNO=46);
%PRINT(SCH=02270802,SCHNAME=AMERICAN INSTITUTE OF MED & DENT - DRAPER,RPNO=47);
%PRINT(SCH=00161900,SCHNAME=COLLEGE OF SOUTHERN IDAHO,RPNO=48);
%PRINT(SCH=02531805,SCHNAME=PAUL MITCHELL THE SCHOOL - TAMPA,RPNO=49);
%PRINT(SCH=02153100,SCHNAME=PAUL MITCHELL THE SCHOOL - NASHVILLE,RPNO=50);
%PRINT(SCH=02149900,SCHNAME=PAUL MITCHELL THE SCHOOL - JACKSONVILLE,RPNO=51);
%PRINT(SCH=01116600,SCHNAME=UTAH CAREER COLLEGE,RPNO=52);
%PRINT(SCH=01116601,SCHNAME=UTAH CAREER COLLEGE - LAYTON,RPNO=53);
%PRINT(SCH=03463300,SCHNAME=CAREERS UNLIMITED,RPNO=54);
%PRINT(SCH=02357700,SCHNAME=PAUL MITCHELL THE SCHOOL - HOUSTON,RPNO=55);
%PRINT(SCH=03598300,SCHNAME=PAUL MITCHELL THE SCHOOL - SANTA BARBARA,RPNO=56);
%PRINT(SCH=03907300,SCHNAME=GREAT LAKES ACADEMY OF HAIR DESIGN,RPNO=57);
%PRINT(SCH=04072300,SCHNAME=DALLAS ROBERTS ACADEMY OF HAIR DESIGN AND AESTHETICS,RPNO=58);
%PRINT(SCH=03084602,SCHNAME=THE ART INSTITUTE OF SALT LAKE CITY,RPNO=59);
%PRINT(SCH=02531806,SCHNAME=PAUL MITCHELL THE SCHOOL - STERLING HEIGHTS,RPNO=60);
%PRINT(SCH=03490300,SCHNAME=UP ACADEMY OF HAIR DESIGN ,RPNO=61);
%PRINT(SCH=02351700,SCHNAME=DONTA SCHOOL OF BEAUTY CULTURE,RPNO=63);
%PRINT(SCH=02149901,SCHNAME=PAUL MITCHELL THE SCHOOL - VIRGINIA,RPNO=64);
%PRINT(SCH=02179935,SCHNAME=ARGOSY UNIVERSITY,RPNO=65);
%PRINT(SCH=02360802,SCHNAME=PROVO COLLEGE - AMERICAN FORK,RPNO=66);
%PRINT(SCH=01179900,SCHNAME=PAUL MITCHELL THE SCHOOL - FAYETTEVILLE,RPNO=67);
%PRINT(SCH=01116602,SCHNAME=UTAH CAREER COLLEGE - OREM,RPNO=68);
%PRINT(SCH=04145500,SCHNAME=SKIN SCIENCE INSTITUTE,RPNO=69);
%PRINT(SCH=02556801,SCHNAME=PAUL MITCHELL THE SCHOOL ST GEORGE ,RPNO=70);
%PRINT(SCH=03490301,SCHNAME=PAUL MITCHELL THE SCHOOL - CHICAGO,RPNO=71);
%PRINT(SCH=04158200,SCHNAME=PAUL MITCHELL THE SCHOOL SHERMAN OAKS,RPNO=72);
%PRINT(SCH=02612200,SCHNAME=MARINELLO SCHOOL OF BEAUTY,RPNO=73);

PROC PRINTTO;
RUN;

/*JOB DETAIL FILE */
/*PROC EXPORT DATA=CERTS*/
/*            OUTFILE= "T:\SAS\UTLWG48_DETAIL.xls" */
/*            DBMS=EXCEL2000 REPLACE;*/
/*RUN;*/
