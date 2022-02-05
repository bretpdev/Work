/*This report picks up FSA FISCAL YEAR that begins in OCTOBER 1st and ends in SEPTEMBER 30th*/
DATA _NULL_;
	/*prior fiscal year relative to Sept.30th*/
/*	CALL SYMPUT('BEGIN',"'"||PUT(intnx('year.10',today(),-1,'beginning'), MMDDYYD10.)||"'");*/
/*	CALL SYMPUT('FINISH',"'"||PUT(intnx('year.10',today(),-1,'end'), MMDDYYD10.)||"'"); */
	/*current fiscal year relative to Sept.30th*/
	CALL SYMPUT('BEGIN',"'"||PUT(intnx('year.10',today(),0,'beginning'), MMDDYYD10.)||"'");
	CALL SYMPUT('FINISH',"'"||PUT(intnx('year.10',today(),0,'end'), MMDDYYD10.)||"'");
RUN;

/*view begin & end date in SAS log*/
%PUT BEGIN = &BEGIN;
%PUT FINISH = &FINISH;

%LET RPTLIB = Q:\FSA Monthly Reports\Discharge Report\Incoming Report Data; *LIVE;
/*%LET RPTLIB = T:\SAS; *TEST;*/

FILENAME REPORTZ "&RPTLIB/UNWC13.NWC13RZ";
FILENAME REPORT2 "&RPTLIB/UNWC13.NWC13R2";
FILENAME DETAIL "&RPTLIB/UNWC13.NWC13R2_detail.csv";
%SYSLPUT BEGIN = &BEGIN;
%SYSLPUT FINISH = &FINISH;
LIBNAME  LEGEND  REMOTE  SERVER=LEGEND SLIBREF=WORK  ;
RSUBMIT LEGEND;
%LET DB = DNFPUTDL;

PROC SQL;
CONNECT TO DB2 (DATABASE=&DB);
	CREATE TABLE DEMO AS
		SELECT 
			*
		FROM CONNECTION TO DB2 
		(
			SELECT DISTINCT
				COUNT(DISTINCT AY10.PF_REQ_ACT) AS OCCUR
				,AY10.PF_REQ_ACT
				,LN10.BF_SSN
				,AY10.LN_ATY_SEQ
				,MONTH(AY10.LD_ATY_REQ_RCV) AS ACT_MONTH
				,LN10.TYP
			FROM 
				PKUB.AY10_BR_LON_ATY AY10
				INNER JOIN 
				(
					SELECT 
						LN10.BF_SSN
						,LN10.LN_SEQ
						,CASE 
							WHEN LN10.IC_LON_PGM IN 
							(
								'DLPCNS','DLPLGB','DLPLUS','DLSCNS','DLSPCN','DLSSPL',
								'DLSTFD','DLUCNS','DLUNST','DLUSPL','DLSCST','DSLCUN',
								'DSLCPL','DLSCSC','DLSCUC','DSLCCN','TEACH','FISL'
							) 
							THEN 'DL'
							ELSE 'FFELP'
						END AS TYP
						,LN10.LC_WOF_WUP_REA
					FROM 
						PKUB.LN10_LON LN10
				) LN10
					ON AY10.BF_SSN = LN10.BF_SSN
				LEFT JOIN PKUB.PD10_PRS_NME PD10
					ON AY10.BF_SSN = PD10.DF_PRS_ID
			WHERE
				AY10.PF_REQ_ACT IN ('DIDTH','DITPD','WOBNK','DICSK','DIFCR','DIUPR','DITLF','DIPSF','DIS11','DIDAS')
				AND AY10.LD_ATY_REQ_RCV BETWEEN &BEGIN AND &FINISH
			GROUP BY 
				AY10.PF_REQ_ACT
				,LN10.BF_SSN
				,AY10.LN_ATY_SEQ
				,MONTH(AY10.LD_ATY_REQ_RCV)
				,LN10.TYP

			FOR READ ONLY WITH UR
		);

	CREATE TABLE DEMO_DETAIL AS
		SELECT 
			*
		FROM CONNECTION TO DB2 
		(
			SELECT DISTINCT
				AY10.PF_REQ_ACT				
				,LN10.BF_SSN
				,AY10.LN_ATY_SEQ
				,AY10.LD_ATY_REQ_RCV
				,LN10.TYP
			FROM
				PKUB.AY10_BR_LON_ATY AY10
				INNER JOIN 
				(
					SELECT 
						LN10.BF_SSN
						,LN10.LN_SEQ
						,CASE 
							WHEN LN10.IC_LON_PGM IN 
							(
								'DLPCNS','DLPLGB','DLPLUS','DLSCNS','DLSPCN','DLSSPL'
								,'DLSTFD','DLUCNS','DLUNST','DLUSPL','DLSCST','DSLCUN'
								,'DSLCPL','DLSCSC','DLSCUC','DSLCCN','TEACH','FISL'
							) 
							THEN 'DL'
							ELSE 'FFELP'
						END AS TYP
						,LN10.LC_WOF_WUP_REA
					FROM 
						PKUB.LN10_LON LN10
				) LN10
					ON AY10.BF_SSN = LN10.BF_SSN
				LEFT JOIN PKUB.PD10_PRS_NME PD10
					ON AY10.BF_SSN = PD10.DF_PRS_ID
			WHERE 
				AY10.PF_REQ_ACT IN ('DIDTH','DITPD','WOBNK','DICSK','DIFCR','DIUPR','DITLF','DIPSF','DIS11','DIDAS')
				AND AY10.LD_ATY_REQ_RCV BETWEEN &BEGIN AND &FINISH

			FOR READ ONLY WITH UR
		);
DISCONNECT FROM DB2;
QUIT;
ENDRSUBMIT;

DATA DEMO;
	SET LEGEND.DEMO;
RUN; 
DATA DEMO_DETAIL;
	SET LEGEND.DEMO_DETAIL;
RUN;

DATA ALLARCS;
	PF_REQ_ACT = '     ';
	TYP = '      ';
	ENGLISH_DESC = '                                                 ';
RUN;

PROC SQL;
	INSERT INTO ALLARCS
		VALUES('DIDTH','FFELP','Death')
		VALUES('DITPD','FFELP','TPD')
		VALUES('WOBNK','FFELP','Bankruptcy')
		VALUES('DICSK','FFELP','Closed School')
		VALUES('DIFCR','FFELP','False Cert')
		VALUES('DIUPR','FFELP','Unpaid Refunds')
		VALUES('DITLF','FFELP','Teacher Loan Forgiveness')
		VALUES('DIPSF','FFELP','Public School Loan Forgiveness')
		VALUES('DIS11','FFELP','Sept 11')
		VALUES('DIDAS','FFELP','Disaster')
		VALUES('DIDTH','DL','Death')
		VALUES('DITPD','DL','TPD')
		VALUES('WOBNK','DL','Bankruptcy')
		VALUES('DICSK','DL','Closed School')
		VALUES('DIFCR','DL','False Cert')
		VALUES('DIUPR','DL','Unpaid Refunds')
		VALUES('DITLF','DL','Teacher Loan Forgiveness')
		VALUES('DIPSF','DL','Public School Loan Forgiveness')
		VALUES('DIS11','DL','Sept 11')
		VALUES('DIDAS','DL','Disaster')
	;
QUIT;

PROC SQL;
	CREATE TABLE ALLARCS_2 AS 
		SELECT
			*
		FROM
			ALLARCS
		WHERE
			PF_REQ_ACT ^= '      '
	;
QUIT;

PROC SORT DATA=ALLARCS_2; 
	BY PF_REQ_ACT TYP; 
RUN;

PROC SQL;
	CREATE TABLE _DEMO AS
		SELECT 
			PF_REQ_ACT
			,ACT_MONTH
			,TYP
			,SUM(OCCUR) AS OCCUR
		FROM
			DEMO
		GROUP BY
			PF_REQ_ACT
			,ACT_MONTH
			,TYP
	;
QUIT;

PROC SORT DATA=_DEMO; 
	BY TYP PF_REQ_ACT ACT_MONTH; 
RUN;

DATA DEMO(DROP=ACT_MONTH OCCUR I);
	SET _DEMO;
	BY TYP PF_REQ_ACT ACT_MONTH; 
	RETAIN JAN FEB MAR APR MAY JUN JUL AUG SEP OCT NOV DEC TOT 0;
	ARRAY MON{12} JAN FEB MAR APR MAY JUN JUL AUG SEP OCT NOV DEC; 
	DO I = 1 TO 12;
		IF FIRST.PF_REQ_ACT 
			THEN MON(I) = 0;
		IF ACT_MONTH = I AND OCCUR > 0 
			THEN MON(I) = OCCUR ;
	END;
		IF LAST.PF_REQ_ACT THEN
			DO;
				TOT = SUM(OF MON{*});
				OUTPUT;
			END;
RUN;

PROC SORT DATA=DEMO; 
	BY PF_REQ_ACT TYP; 
RUN;

DATA DEMO;
	MERGE ALLARCS_2 DEMO;
	BY PF_REQ_ACT TYP;
RUN;

PROC SQL;
	CREATE TABLE DEMO_2 AS 
		SELECT DISTINCT
			*
		FROM
			DEMO
	;
QUIT;

DATA _NULL_;
	SET DEMO_2 ;
	FILE REPORT2 DELIMITER=',' DSD DROPOVER LRECL=32767;
	ARRAY OUT{12} JAN FEB MAR APR MAY JUN JUL AUG SEP OCT NOV DEC;
	DO I = 1 TO 12;
		IF OUT(I) = . 
		THEN OUT(I) = 0;
	END;
	IF _N_ = 1 THEN 
		DO;
			PUT "DISCHARGE TYPE,JAN,FEB,MAR,APR,MAY,JUN,JUL,AUG,SEP,OCT,NOV,DEC,DL/FFELP INDICATOR";
		END;
	DO;
	   PUT ENGLISH_DESC @;
		   DO I = 1 TO 12;
				PUT OUT(I) @;
		   END;
	   PUT TYP $ ;
	END;
RUN;

DATA _NULL_;
	SET DEMO_DETAIL ;
	FILE DETAIL DELIMITER=',' DSD DROPOVER LRECL=32767;
	FORMAT LD_ATY_REQ_RCV MMDDYY10.;
	IF _N_ = 1 THEN 
		DO;
			PUT "DISCHARGE TYPE
				,SSN
				,LN_ATY_SEQ
				,DATE
				,IC_LON_PGM"
				;
		END;
	PUT PF_REQ_ACT
		BF_SSN 
		LN_ATY_SEQ 
		LD_ATY_REQ_RCV 
		TYP
		;
RUN;