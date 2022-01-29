﻿
CREATE PROCEDURE [dbo].[LT_US06BIBR45_FormFields]
	@BF_SSN  CHAR(9)
AS

	SELECT 
		CONVERT(VARCHAR(10),CAST(RS05.BD_ANV_QLF_IBR - 30 AS DATE),101) AS PZSFTDDLNE,
		RS05.BA_PMN_STD_TOT_PAY AS PZPERMSTND,
		CONVERT(VARCHAR(10),CAST(RS05.BD_ANV_QLF_IBR AS DATE),101) AS PZANTDTPAY
	FROM
		[dbo].[RS05_IBR_RPS] RS05
	WHERE
		RS05.BF_SSN = @BF_SSN
		AND RS05.BC_STA_RS05 = 'A'

RETURN 0