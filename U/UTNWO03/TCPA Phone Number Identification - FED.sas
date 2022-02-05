/*%LET RPTLIB = %SYSGET(reportdir);*/
%LET RPTLIB = T:\SAS;
FILENAME REPORTZ "&RPTLIB/UNWO03.NWO03RZ";
FILENAME REPORT2 "&RPTLIB/UNWO03.NWO03R2";
FILENAME REPORT3 "&RPTLIB/UNWO03.NWO03R3";
FILENAME REPORT4 "&RPTLIB/UNWO03.NWO03R4";

LIBNAME  LEGEND  REMOTE  SERVER=LEGEND SLIBREF=work  ;
RSUBMIT LEGEND;
/*%let DB = DNFPRQUT;  *This is test;*/
%let DB = DNFPRUUT;  *New test region;
/*%let DB = DNFPUTDL;  *This is live;*/

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
CONNECT TO DB2 (DATABASE=&DB);
CREATE TABLE DEMO AS
	SELECT	*
	FROM	CONNECTION TO DB2 (
				SELECT DISTINCT	CASE
						WHEN LN10.PRS_TYP = 'R' AND PD10.DF_PRS_ID LIKE 'P%' THEN PD10.DF_PRS_ID
						ELSE PD10.DF_SPE_ACC_ID END AS DF_SPE_ACC_ID,
						LN10.PRS_TYP,
						PD40.DC_PHN,
						RTRIM(PD40.DN_DOM_PHN_ARA) || '' || RTRIM(PD40.DN_DOM_PHN_XCH) || '' || PD40.DN_DOM_PHN_LCL		AS PHN_NUM

				FROM	PKUB.PD10_PRS_NME PD10
						INNER JOIN PKUB.PD40_PRS_PHN PD40
							ON PD10.DF_PRS_ID = PD40.DF_PRS_ID
						INNER JOIN (
								SELECT BF_SSN 
									,BF_SSN AS DF_PRS_ID
									,'B' AS PRS_TYP
								FROM PKUB.LN10_LON 
							UNION
								SELECT BF_SSN
									,LF_STU_SSN AS DF_PRS_ID
									,'S' AS PRS_TYP
								FROM PKUB.LN10_LON 
							UNION
								SELECT LON.BF_SSN 
									,END.LF_EDS AS DF_PRS_ID
									,'E' AS PRS_TYP
								FROM PKUB.LN10_LON LON
									INNER JOIN PKUB.LN20_EDS END
									ON LON.BF_SSN = END.BF_SSN
									AND END.LC_STA_LON20 = 'A'
					   		UNION
								SELECT L.BF_SSN
									,RFR.BF_RFR AS DF_PRS_ID
									,'R' AS PRS_TYP
								FROM PKUB.PD10_PRS_NME PD10
									INNER JOIN PKUB.RF10_RFR RFR
										ON PD10.DF_PRS_ID = RFR.BF_RFR
									INNER JOIN PKUB.LN10_LON L
									ON RFR.BF_SSN = L.BF_SSN
								WHERE RFR.BC_STA_REFR10 = 'A'
							) LN10
							ON LN10.DF_PRS_ID = PD10.DF_PRS_ID
						INNER JOIN PKUB.LN10_LON LN10_FIL
							ON LN10.BF_SSN = LN10_FIL.BF_SSN
				
				WHERE	PD40.DI_PHN_VLD = 'Y'
					AND LN10_FIL.LA_CUR_PRI > 0 
					AND LN10_FIL.LC_STA_LON10 = 'R'
				FOR READ ONLY WITH UR
			);
DISCONNECT FROM DB2;

PROC SQL;
CREATE TABLE PHN AS
	SELECT DISTINCT 	
			A.DF_SPE_ACC_ID,
			A.PHN_NUM
	FROM	DEMO A
	WHERE 	A.PHN_NUM IS NOT NULL;
QUIT;

ENDRSUBMIT;
DATA PHN; 
	SET LEGEND.PHN ; 
RUN;

DATA PHN;
	SET PHN;
	IF SUBSTR(DF_SPE_ACC_ID,1,1) = 'P' THEN RNUM = 'R3';
	ELSE RNUM = 'R2';
	RUN_DATE = TODAY();
RUN;

DATA PHN;
	SET PHN;
	FORMAT DF_SPE_ACC_ID_EP $20.;
	IF SUBSTR(DF_SPE_ACC_ID,1,1) IN ('P','R') THEN DF_SPE_ACC_ID_EP = DF_SPE_ACC_ID;
	ELSE DF_SPE_ACC_ID_EP = STRIP(PUT(JULDATE(TODAY()) * INPUT(SUBSTR(DF_SPE_ACC_ID,ANYDIGIT(DF_SPE_ACC_ID)),BEST12.), NUMX20.));
	FORMAT RUN_DATE MMDDYY10.;
	RUN_DATE = TODAY();

RUN;

PROC SQL;
CREATE TABLE COUNT AS
	SELECT	CASE RNUM
				WHEN 'R2' THEN 1 
			ELSE 0 END AS R2,
			CASE RNUM
				WHEN 'R3' THEN 1 
			ELSE 0 END AS R3
	FROM 	PHN A
		;
QUIT;

PROC SQL;
CREATE TABLE COUNT2 AS
	SELECT	SUM(R2) AS OTH_PHN,
			SUM(R3) AS REF_PHN,
			COUNT(*) AS TOT_PHN
	FROM	COUNT A
	;
QUIT;

%MACRO CRT_FILE(RNO);
DATA _NULL_;
	SET  WORK.PHN;
	WHERE RNUM = "R&RNO";
	FILE REPORT&RNO DELIMITER=',' DSD DROPOVER LRECL=32767;
		FORMAT PHN_NUM $12. ;
		FORMAT DF_SPE_ACC_ID_EP $20. ;
		FORMAT RUN_DATE MMDDYY10.;
	IF _N_ = 1 THEN        
	DO;
	   PUT
	      "DF_SPE_ACC_ID_EP,PHN_NUM,RUN_DATE";
	END;
	DO;
		PUT DF_SPE_ACC_ID_EP $ @;	
		PUT PHN_NUM $ @;
		PUT RUN_DATE;
	END;
RUN;
%MEND CRT_FILE;
%CRT_FILE(2);
%CRT_FILE(3);

DATA _NULL_;
SET		WORK.COUNT2;
FILE	REPORT4 delimiter=',' DSD DROPOVER lrecl=32767;
/* write column names, remove this to create a file without a header row */
IF _N_ = 1 THEN
	DO;
		PUT	'Total Non-Reference Phone Numbers'
			','
			'Total Reference Phone Numbers'
			','
			'Total Phone Numbers';
	END;
/* write data*/	
DO;
	PUT OTH_PHN $ @;
	PUT REF_PHN $ @;
	PUT TOT_PHN $;
	;
END;
RUN;
