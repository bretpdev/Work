﻿CREATE PROCEDURE [subscrprt].[LoadNewData]
AS
	INSERT INTO ULS.subscrprt.PrintData(BF_SSN, CLUID, DM_PRS_LST)
	SELECT DISTINCT
		DC01.BF_SSN,
		RTRIM(DC01.AF_APL_ID) + DC01.AF_APL_ID_SFX [CLUID],
		PD01.DM_PRS_LST
	FROM
		ODW..DC01_LON_CLM_INF DC01
		INNER JOIN ODW..GA20_CNL_DAT GA20
			ON RTRIM(DC01.AF_APL_ID)  = RTRIM(GA20.AF_APL_ID)
		INNER JOIN ODW..GA10_LON_APP GA10
			ON RTRIM(DC01.AF_APL_ID) = RTRIM(GA10.AF_APL_ID)
		INNER JOIN ODW..PD01_PDM_INF PD01
			ON DC01.BF_SSN = PD01.DF_PRS_ID
		LEFT JOIN
		(
			SELECT DISTINCT
				CLUID
			FROM
				ULS.subscrprt.PrintData
			WHERE
				YEAR(AddedAt) = YEAR(GETDATE())
		) ThisYear
			ON ThisYear.CLUID = RTRIM(DC01.AF_APL_ID) + DC01.AF_APL_ID_SFX
	WHERE
		DC01.LC_REA_CLM_ASN_DOE = '07'
		AND DC01.LD_CLM_ASN_DOE IS NULL
		AND DC01.LC_STA_DC10 NOT IN ('04','02')
		AND ThisYear.CLUID IS NULL
		AND GA20.AC_MPN_SRL_LON IN ('N','S')
		AND GA10.AA_CUR_PRI > 0
	ORDER BY
		PD01.DM_PRS_LST
RETURN 0

GRANT EXECUTE ON [subscrprt].[LoadNewData] TO db_executor