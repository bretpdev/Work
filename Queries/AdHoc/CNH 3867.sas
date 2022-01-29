/*%LET RPTLIB = %SYSGET(reportdir);*/
%LET RPTLIB = T:\SAS;
FILENAME REPORTZ "&RPTLIB/UNWSXX.NWSXXRZ";
FILENAME REPORTX "&RPTLIB/Suspense Refunds.xls";
FILENAME REPORTX "&RPTLIB/Overpayment Refund Cancellations.xls";

LIBNAME  LEGEND  REMOTE  SERVER=LEGEND SLIBREF=work  ;
RSUBMIT LEGEND;
/*%let DB = DNFPRQUT;  *This is test;*/
%let DB = DNFPUTDL;  *This is live;

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

PROC SQL ;
	CONNECT TO DBX (DATABASE=&DB);

	CREATE TABLE SUSP AS
		SELECT
			*
		FROM	
			CONNECTION TO DBX 
				(
					SELECT	
						COALESCE(D.DF_SPE_ACC_ID,'XXXXXXXXXX') AS DF_SPE_ACC_ID
						,A.LA_SPS_RFD
						,A.LF_SPS_CAN_SCH_NUM
						,A.LD_SPS_CAN_SCH
					FROM	
						PKUB.RMXX_RMT_SPS_RFD A
						LEFT OUTER JOIN PKUB.RMXX_BR_RMT_PST B
							ON A.LN_RMT_BCH_SEQ = B.LN_RMT_BCH_SEQ
							AND A.LC_RMT_BCH_SRC_IPT = B.LC_RMT_BCH_SRC_IPT
							AND A.LD_RMT_BCH_INI = B.LD_RMT_BCH_INI
							AND A.LN_RMT_SEQ_PST = B.LN_RMT_SEQ_PST
							AND A.LN_RMT_ITM_PST = B.LN_RMT_ITM_PST
							AND A.LN_RMT_ITM_SEQ_PST = B.LN_RMT_ITM_SEQ_PST
						LEFT OUTER JOIN PKUB.PDXX_PRS_NME D
							ON B.BF_SSN = D.DF_PRS_ID
					WHERE	
						A.LD_SPS_CAN_SCH BETWEEN 'XX/XX/XXXX' AND 'XX/XX/XXXX'
						AND A.LC_STA_REMTXX = 'C'

					FOR READ ONLY WITH UR
				)
	;

	CREATE TABLE OVER AS
		SELECT
			*
		FROM	
			CONNECTION TO DBX 
				(
					SELECT	
						D.DF_SPE_ACC_ID
						,A.LA_RFD_TO_RCP
						,A.LF_RFD_CAN_SCH_NUM
						,A.LD_RFD_CAN_SCH
					FROM	
						PKUB.LNXX_LON_RFD A
						INNER JOIN PKUB.PDXX_PRS_NME D
							ON A.BF_SSN = D.DF_PRS_ID
					WHERE	
						A.LD_RFD_CAN_SCH BETWEEN 'XX/XX/XXXX' AND 'XX/XX/XXXX'

					FOR READ ONLY WITH UR
				)
	;

	DISCONNECT FROM DBX;

QUIT;

ENDRSUBMIT;
DATA SUSP; SET LEGEND.SUSP; RUN;
DATA OVER; SET LEGEND.OVER; RUN;


/*export to Excel spreadsheet*/
PROC EXPORT DATA= WORK.SUSP 
            OUTFILE= REPORTX
            DBMS=EXCEL REPLACE;
     SHEET="A"; 
RUN;

PROC EXPORT DATA= WORK.OVER
            OUTFILE= REPORTX
            DBMS=EXCEL REPLACE;
     SHEET="A"; 
RUN;
