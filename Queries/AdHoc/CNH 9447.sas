/****enter date range in this format: mm/dd/yyyy ****/

%LET BEGINDATE = 'XX/XX/XXXX';
%LET ENDDATE = 'XX/XX/XXXX';

/****enter active duty status date in this format: yyyymmdd ****/

%LET ADSDATE = XXXXXXXX;

/*********************************************/

/*%LET RPTLIB = %SYSGET(reportdir);*/
%LET RPTLIB = T:\SAS;
LIBNAME  LEGEND  REMOTE  SERVER=LEGEND SLIBREF=WORK;
%SYSLPUT BEGINDATE = &BEGINDATE;
%SYSLPUT ENDDATE = &ENDDATE;

RSUBMIT LEGEND;
/*%let DB = DNFPRQUT;  *This is test;*/
/*%let DB = DNFPRUUT;  *This is VUKX test;*/
%LET DB = DNFPUTDL;  *This is live;
LIBNAME PKUB DBX DATABASE=&DB OWNER=PKUB;

%MACRO SQLCHECK ;
  %IF  &SQLXRC NE X  %THEN  %DO  ;
    DATA _NULL_  ;
            FILE REPORTZ NOTITLES  ;
            PUT @XX " ********************************************************************* "
              / @XX " ****  THE SQL CODE ABOVE HAS EXPERIENCED AN ERROR.               **** "
              / @XX " ****  THE SAS SHOULD BE REVIEWED.                                **** "       
              / @XX " ********************************************************************* "
              / @XX " ****  THE SQL ERROR CODE IS  &SQLXRC  AND THE SQL ERROR MESSAGE  **** "
              / @XX " ****  &SQLXMSG   **** "
              / @XX " ********************************************************************* "
            ;
         RUN  ;
  %END  ;
%MEND  ;

/*Get base population of borrowers*/
PROC SQL;
	CONNECT TO DBX (DATABASE=&DB);

	CREATE TABLE BOR AS
		SELECT	
			*
		FROM	
			CONNECTION TO DBX 
				(
					SELECT DISTINCT
						LNXX.BF_SSN
						,PDXX.DD_BRT
						,PDXX.DM_PRS_LST
						,PDXX.DM_PRS_X
						,PDXX.DF_SPE_ACC_ID
						,PDXX.DM_PRS_MID
					FROM
						PKUB.PDXX_PRS_NME PDXX
						INNER JOIN PKUB.LNXX_LON LNXX
							ON PDXX.DF_PRS_ID = LNXX.BF_SSN
						INNER JOIN PKUB.LNXX_FIN_ATY LNXX
							ON LNXX.BF_SSN = LNXX.BF_SSN
							AND LNXX.LN_SEQ = LNXX.LN_SEQ
					WHERE
						LNXX.LD_LON_ACL_ADD BETWEEN &BEGINDATE AND &ENDDATE
						OR (
								LNXX.LD_LON_ACL_ADD <= &BEGINDATE
								AND LNXX.LA_CUR_PRI > X.XX
							)
						OR LNXX.LD_PIF_RPT BETWEEN &BEGINDATE AND &ENDDATE
						OR (
								LNXX.PC_FAT_TYP = 'XX'
								AND LNXX.PC_FAT_SUB_TYP = 'XX'
								AND LNXX.LD_FAT_EFF BETWEEN &BEGINDATE AND &ENDDATE
							)

					FOR READ ONLY WITH UR
				)
	;

/*Pull Endorsers and Co-Borrowers*/
CREATE TABLE EDS AS
	SELECT DISTINCT
		LNXX.LF_EDS AS BF_SSN
		,PDXX.DD_BRT FORMAT = YYMMDDNX.
		,PDXX.DM_PRS_LST
		,PDXX.DM_PRS_X
		,PDXX.DF_SPE_ACC_ID
		,PDXX.DM_PRS_MID
	FROM 
		BOR
		INNER JOIN PKUB.LNXX_EDS LNXX
			ON BOR.BF_SSN = LNXX.BF_SSN
		INNER JOIN PKUB.PDXX_PRS_NME PDXX
			ON LNXX.LF_EDS = PDXX.DF_PRS_ID
	WHERE 
		LNXX.LC_STA_LONXX = 'A'
;

	DISCONNECT FROM DBX;

	/*%PUT  SQLXRC= >>> &SQLXRC <<< ||| SQLXMSG= >>> &SQLXMSG >>> ;  ** INCLUDES ERROR MESSAGES TO SAS LOG  ;*/
	/*%SQLCHECK;*/
QUIT;

ENDRSUBMIT;

DATA BOR; SET LEGEND.BOR; RUN;
DATA EDS; SET LEGEND.EDS; RUN;

DATA BOR;
	SET BOR;
	FORMAT DD_BRT YYMMDDNX.;
RUN;

DATA JOINT;
	SET BOR EDS END=END;
	ACTIVE_DUTY_STATUS_DATE = "&ADSDATE";
	/*	END =;*/
RUN;

PROC SQL NOPRINT;
  SELECT COUNT(*) INTO: NOBS
  FROM JOINT
  ;
QUIT;

DATA REP;
  PUT "&NOBS";
  NREP = INT(&NOBS/XXXXXX) + X;
RUN;

PROC SQL;
      SELECT NREP INTO :NREP FROM REP;
QUIT;

%PUT &NREP;

DATA _NULL_;
    IF X THEN SET JOINT NOBS=N;
    DO I=X TO CEIL(N/XXXXXX);
        CALL EXECUTE (CATS("DATA OUTPT",I)||";");
        CALL EXECUTE ("SET JOINT(FIRSTOBS="||(I-X)*XXXXXX+X||" OBS="||I*XXXXXX||");");
        CALL EXECUTE ("RUN;");
    END;
RUN;

%MACRO CREX;
      %DO I = X %TO &NREP;
            DATA _NULL_;
				SET OUTPT&I END=EOF;
				FILE "&RPTLIB/IDENTIFYING ACTIVE DUTY MILITARY BORROWERS--FED--NEW_&I" DROPOVER LRECL=XXXXX;
				DO;
					PUT @X BF_SSN   			
						@XX DD_BRT
						@XX DM_PRS_LST
						@XX DM_PRS_X
						@XX DF_SPE_ACC_ID
						@XX ACTIVE_DUTY_STATUS_DATE
						@XXX DM_PRS_MID;
					END;
				IF EOF THEN DO;
					PUT @X 'EOF';
					END;
				RUN;
      %END;
%MEND;
%CREX;
