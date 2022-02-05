/*%LET RPTLIB = %SYSGET(reportdir);*/
%LET RPTLIB = T:\SAS;
FILENAME REPORTZ "&RPTLIB/ULWS68.LWS68RZ";
FILENAME REPORT2 "&RPTLIB/ULWS68.LWS68R2";
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

	CREATE TABLE NSFS AS
		SELECT 
			*
		FROM 
			CONNECTION TO DB2 
				(
					SELECT DISTINCT
						PD01.DF_SPE_ACC_ID,
						LN10.BF_SSN,
						LN90.LD_FAT_EFF,
						LN90.LC_FAT_REV_REA
					FROM	
						OLWHRM1.PD01_PDM_INF PD01
						JOIN OLWHRM1.LN10_LON LN10
							ON PD01.DF_PRS_ID = LN10.BF_SSN
							AND LN10.LF_LON_CUR_OWN = '826717'
							AND LN10.LA_CUR_PRI > 0
							AND LN10.LC_STA_LON10 =  'R'
						JOIN OLWHRM1.LN90_FIN_ATY LN90
							ON LN10.BF_SSN = LN90.BF_SSN
							AND LN10.LN_SEQ = LN90.LN_SEQ
							AND LN90.LC_STA_LON90 = 'A'
							AND LN90.PC_FAT_TYP = '10'
							AND LN90.PC_FAT_SUB_TYP = '10'
						JOIN 
/*							count of NSFs*/
							(
								SELECT 
									BF_SSN,
									LN_SEQ,
									COUNT(DISTINCT LD_FAT_EFF) AS CNT
								FROM
									OLWHRM1.LN90_FIN_ATY
								WHERE														
									LC_FAT_REV_REA = '1'
									AND LC_STA_LON90 = 'A'
									AND PC_FAT_TYP = '10'
									AND PC_FAT_SUB_TYP = '10'
								GROUP BY
									BF_SSN,
									LN_SEQ
							) NSF
							ON LN10.BF_SSN = NSF.BF_SSN
							AND LN10.LN_SEQ = NSF.LN_SEQ
							AND NSF.CNT > 1 /*only include borrower with more than one NSF*/
					ORDER BY
						LN10.BF_SSN,
						LN90.LD_FAT_EFF					

					FOR READ ONLY WITH UR
				)
	;

	DISCONNECT FROM DB2;

	/*%PUT  SQLXRC= >>> &SQLXRC <<< ||| SQLXMSG= >>> &SQLXMSG >>> ;  ** INCLUDES ERROR MESSAGES TO SAS LOG  ;*/
	/*%SQLCHECK;*/
QUIT;

ENDRSUBMIT;
DATA NSFS; SET DUSTER.NSFS; RUN;

/*find consecutive NSFs*/
DATA CONSECUTIVE;
	SET NSFS;
	BY BF_SSN;

	RETAIN PREV_LD_FAT_EFF;
	RETAIN PREV_REV_REA;

	FORMAT PREV_LD_FAT_EFF DATE9.;

/*	for each SSN, reset values of variables that retain values of previous record*/
	IF FIRST.BF_SSN THEN
		DO;
			PREV_LD_FAT_EFF = LD_FAT_EFF;
			PREV_REV_REA = LC_FAT_REV_REA;
		END;
/*	output the data if the criteria is met*/
	ELSE IF PREV_REV_REA = '1' AND LC_FAT_REV_REA = '1' AND LD_FAT_EFF ^= PREV_LD_FAT_EFF AND DATDIF(PREV_LD_FAT_EFF,LD_FAT_EFF,'ACT/ACT') <=30 THEN OUTPUT;
/*	set variables which retain values of previous record*/
	ELSE
		DO;
			PREV_LD_FAT_EFF = LD_FAT_EFF;
			PREV_REV_REA = LC_FAT_REV_REA;
		END;
RUN;


DATA _NULL_;
	SET CONSECUTIVE ;
	FILE REPORT2 DELIMITER=',' DSD DROPOVER LRECL=32767;
	
	COMMENT = CAT("Borrower has received two consecutive NSF’s due to insufficient funds on ",PUT(PREV_LD_FAT_EFF,MMDDYY10.),' and ',PUT(LD_FAT_EFF,MMDDYY10.));

	PUT BF_SSN 'AGNSF,,,,,,,ALL,' COMMENT;
RUN;

/*only needed for testing*/
/*PROC EXPORT*/
/*		DATA=NSFS*/
/*		OUTFILE='T:\SAS\NSFS.XLSX'*/
/*		REPLACE;*/
/*RUN;*/
