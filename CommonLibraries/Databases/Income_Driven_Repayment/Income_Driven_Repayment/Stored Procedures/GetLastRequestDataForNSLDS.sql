﻿CREATE PROCEDURE [dbo].[GetLastRequestDataForNSLDS]
		@SSN CHAR(9),
	@TESTMODE BIT
AS
	DECLARE @QUERY VARCHAR(MAX), @TSQL VARCHAR(MAX)
	IF @TESTMODE = 0
		SELECT @TSQL = 'SELECT DATEDIFF(DAY, P.LF_CRT_DTS_GRSP, GETDATE()) AS DAY_DIFF FROM OPENQUERY(LEGEND_TEST_VUK1,'
	ELSE
		SELECT @TSQL = 'SELECT DATEDIFF(DAY, P.LF_CRT_DTS_GRSP, GETDATE()) AS DAY_DIFF FROM OPENQUERY(LEGEND,'
	  SELECT  @QUERY = @TSQL + 
	  '''
		SELECT 
			MAX(LF_CRT_DTS_GRSP) AS LF_CRT_DTS_GRSP
		FROM 
			PKUB.GRSP_NDS_LON_RSP GRSP 	
		WHERE 
			GRSP.DF_PRS_ID = ''''' + @SSN + '''''
	   '') P'
      EXEC (@QUERY)
RETURN 0