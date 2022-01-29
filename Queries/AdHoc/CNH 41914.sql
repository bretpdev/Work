USE CDW
GO

DECLARE @StartDate DATE = 'XX/XX/XXXX'
DECLARE @EndDate DATE = 'XX/XX/XXXX'

-- RMXX, RMXX not local; use OPENQUERY
DECLARE @SuspenseRefundCancelations VARCHAR(XXXX) = 
'
	SELECT
		*
	FROM
		OPENQUERY
		(
			LEGEND,
			''
				SELECT DISTINCT
					COALESCE(PDXX.DF_SPE_ACC_ID, ''''XXXXXXXXXX'''') AS DF_SPE_ACC_ID,
					PDXX.DF_PRS_ID,
					RMXX.LA_SPS_RFD,
					RMXX.LF_SPS_CAN_SCH_NUM,
					RMXX.LD_SPS_CAN_SCH
				FROM	
					PKUB.RMXX_RMT_SPS_RFD RMXX
					LEFT JOIN PKUB.RMXX_BR_RMT_PST RMXX
						ON RMXX.LN_RMT_BCH_SEQ = RMXX.LN_RMT_BCH_SEQ
						AND RMXX.LC_RMT_BCH_SRC_IPT = RMXX.LC_RMT_BCH_SRC_IPT
						AND RMXX.LD_RMT_BCH_INI = RMXX.LD_RMT_BCH_INI
						AND RMXX.LN_RMT_SEQ_PST = RMXX.LN_RMT_SEQ_PST
						AND RMXX.LN_RMT_ITM_PST = RMXX.LN_RMT_ITM_PST
						AND RMXX.LN_RMT_ITM_SEQ_PST = RMXX.LN_RMT_ITM_SEQ_PST
					LEFT OUTER JOIN PKUB.PDXX_PRS_NME PDXX ON RMXX.BF_SSN = PDXX.DF_PRS_ID
				WHERE	
					RMXX.LD_SPS_CAN_SCH BETWEEN ''''' + CONVERT(VARCHAR(XX),  @StartDate, XXX) + ''''' AND ''''' + CONVERT(VARCHAR(XX),  @EndDate, XXX) + '''''
					AND 
					RMXX.LC_STA_REMTXX = ''''C''''
			''
		);
'
EXEC (@SuspenseRefundCancelations)



-- LNXX not local; use OPENQUERY
DECLARE @OverpaymentRefundCancelations VARCHAR(XXXX) = 
'
	SELECT
		*
	FROM
		OPENQUERY
		(
			LEGEND,
			''
				
				SELECT	
					PDXX.DF_SPE_ACC_ID,
					PDXX.DF_PRS_ID,
					LNXX.LA_RFD_TO_RCP,
					LNXX.LF_RFD_CAN_SCH_NUM,
					LNXX.LD_RFD_CAN_SCH
				FROM	
					PKUB.LNXX_LON_RFD LNXX
					INNER JOIN PKUB.PDXX_PRS_NME PDXX ON LNXX.BF_SSN = PDXX.DF_PRS_ID
				WHERE	
					LNXX.LD_RFD_CAN_SCH BETWEEN ''''' + CONVERT(VARCHAR(XX),  @StartDate, XXX) + ''''' AND ''''' + CONVERT(VARCHAR(XX),  @EndDate, XXX) + '''''
			''
		);
'
EXEC (@OverpaymentRefundCancelations)