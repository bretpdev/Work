EXEC CDW..RefreshTableWithValidation 'PD10_PRS_NME','DF_LST_DTS_PD10','PD10_PRS_NME','DF_PRS_ID'


-- ###### ADDITIONAL VALIDATION
 --the DF_LST_DTS_PD10 date is unreliable when a person's SSN has been changed.
 --sum all SSNs in order to dentify missing/change/added SSNs
DECLARE 
	@SumDifference INT

SELECT
	@SumDifference = L.LocalSum - R.RemoteSum
FROM
	OPENQUERY
	(
		LEGEND,
		'
			SELECT
				SUM(CAST(DF_PRS_ID AS BIGINT)) AS "RemoteSum"
			FROM
				PKUB.PD10_PRS_NME PD10
			WHERE
				PD10.DF_PRS_ID NOT LIKE ''P%''
		'
	) R
	FULL OUTER JOIN
	(
		SELECT
			SUM(CAST(DF_PRS_ID AS BIGINT)) [LocalSum]
		FROM
			CDW..PD10_PRS_NME_1 PD10
		WHERE
			PD10.DF_PRS_ID NOT LIKE 'P%'
	) L 
		ON 1 = 1


IF @SumDifference != 0
	BEGIN
		RAISERROR('PD10_PRS_NME - The remote and local SSN sums do not match.  A full refresh of the table may be required.', 16, 11, @SumDifference)
	END