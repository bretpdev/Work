LIBNAME  LEGEND  REMOTE  SERVER=LEGEND SLIBREF=work  ;
RSUBMIT LEGEND;
LIBNAME PKUB DBX DATABASE=DNFPUTDL OWNER=PKUB;

%LET BEGIN = 'XXJULXXXX'D;
%LET END = 'XXOCTXXXX'D;

PROC SQL ;


	CREATE TABLE SUSP AS
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
			A.LD_SPS_CAN_SCH BETWEEN &BEGIN AND &END
			AND A.LC_STA_REMTXX = 'C'
	;

	CREATE TABLE OVER AS
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
			A.LD_RFD_CAN_SCH BETWEEN &BEGIN AND &END
	;
QUIT;

ENDRSUBMIT;
DATA SUSP; SET LEGEND.SUSP; RUN;
DATA OVER; SET LEGEND.OVER; RUN;


/*export to Excel spreadsheet*/
PROC EXPORT DATA= WORK.SUSP 
            OUTFILE= 'T:\SAS\SSAE XX Refund and Cancellation.xlsx'
            DBMS=EXCEL REPLACE;
RUN;

PROC EXPORT DATA= WORK.OVER
            OUTFILE= 'T:\SAS\SSAE XX Refund and Cancellation.xlsx'
            DBMS=EXCEL REPLACE;
RUN;
