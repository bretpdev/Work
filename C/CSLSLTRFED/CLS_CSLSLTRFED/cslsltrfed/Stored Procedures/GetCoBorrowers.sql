CREATE PROCEDURE [cslsltrfed].[GetCoBorrowers]
	@BF_SSN VARCHAR(9)
AS
	SELECT
		LF_EDS,
		LN_SEQ
	FROM
		CDW..LN20_EDS
	WHERE
		BF_SSN = @BF_SSN
		AND LC_STA_LON20 = 'A'
		AND LC_EDS_TYP = 'M'
RETURN 0

GRANT EXECUTE ON [cslsltrfed].[GetCoBorrowers] TO db_executor