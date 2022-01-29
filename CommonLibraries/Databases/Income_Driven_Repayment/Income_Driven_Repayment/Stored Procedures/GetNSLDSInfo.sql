﻿CREATE PROCEDURE [dbo].[GetNSLDSInfo]
	@SSN CHAR(9),
	@SPOUSE BIT,
	@TESTMODE BIT
AS
	DECLARE @QUERY VARCHAR(MAX), @TSQL VARCHAR(MAX)
IF @TESTMODE = 0
		SELECT @TSQL = 'SELECT * FROM OPENQUERY(LEGEND_TEST_VUK1,'
	ELSE
		SELECT @TSQL = 'SELECT * FROM OPENQUERY(LEGEND,'
	  SELECT  @QUERY = @TSQL + 
	  '''
		SELECT 
			'''''  + cast(@SPOUSE as char(1)) + ''''' AS SpouseIndicator,
			IC_LON_PGM AS LoanType,
			LF_CUR_SER AS OwnerLender,
			(COALESCE(LA_CUR_PRI,0) + COALESCE(WA_TOT_BRI_OTS,0)) AS OutstandingBalance,
			LR_ITR AS InterestRate
		FROM 
			PKUB.GRSP_NDS_LON_RSP GRSP 	
			INNER JOIN
			(
				SELECT
					DF_PRS_ID,
					MAX(LF_CRT_DTS_GRSP) AS LF_CRT_DTS_GRSP
				FROM
					PKUB.GRSP_NDS_LON_RSP
				GROUP BY
					DF_PRS_ID
			)POP
				ON POP.DF_PRS_ID = GRSP.DF_PRS_ID
				AND POP.LF_CRT_DTS_GRSP = GRSP.LF_CRT_DTS_GRSP
		WHERE 
			GRSP.DF_PRS_ID = ''''' + @SSN + '''''
	   '')'
      EXEC (@QUERY)
RETURN 0