﻿CREATE PROCEDURE [dbo].[CheckIfBorrowerHasBeenOnRepaye]
	@SSN CHAR(9),
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
			COUNT(*)
		FROM 
			PKUB.LN65_LON_RPS LN65 	
		WHERE 
			LN65.BF_SSN = ''''' + @SSN + '''''
			AND LN65.LC_TYP_SCH_DIS = ''''I5''''
	   '')'
      EXEC (@QUERY)
RETURN 0
