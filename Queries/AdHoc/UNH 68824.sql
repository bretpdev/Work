USE UDW
GO

DECLARE @DATA TABLE (ST VARCHAR(2), TIME_ZONE VARCHAR(100))
INSERT INTO @DATA VALUES
('AK','Alaska Time Zone'),
('AL','Central Time Zone'),
('AZ','Mountain Time Zone'),
('AR','Central Time Zone'),
('CA','Pacific Time Zone'),
('CO','Mountain Time Zone'),
('CT','Eastern Time Zone'),
('DE','Eastern Time Zone'),
('FL','Eastern Time Zone'),
('GA','Eastern Time Zone'),
('HI','Hawaii-Aleutian Time Zone'),
('ID','Mountain Time Zone'),
('IL','Central Time Zone'),
('IN','Eastern Time Zone'),
('IA','Central Time Zone'),
('KS','Central Time Zone'),
('KY','Eastern Time Zone'),
('LA','Central Time Zone'),
('ME','Eastern Time Zone'),
('MD','Eastern Time Zone'),
('MA','Eastern Time Zone'),
('MI','Eastern Time Zone'),
('MN','Central Time Zone'),
('MS','Central Time Zone'),
('MO','Central Time Zone'),
('MT','Mountain Time Zone'),
('NE','Central Time Zone'),
('NV','Pacific Time Zone'),
('NH','Eastern Time Zone'),
('NJ','Eastern Time Zone'),
('NM','Mountain Time Zone'),
('NY','Eastern Time Zone'),
('NC','Eastern Time Zone'),
('ND','Central Time Zone'),
('OH','Eastern Time Zone'),
('OK','Central Time Zone'),
('OR','Pacific Time Zone'),
('PA','Eastern Time Zone'),
('RI','Eastern Time Zone'),
('SC','Eastern Time Zone'),
('SD','Central Time Zone'),
('TN','Eastern Time ZonE'),
('TX','Central Time Zone'),
('UT','Mountain Time Zone'),
('VT','Eastern Time Zone'),
('VA','Eastern Time Zone'),
('WA','Pacific Time Zone'),
('WV','Eastern Time Zone'),
('WI','Central Time Zone'),
('WY','Mountain Time Zone'),
('DC','Eastern Time Zone')


SELECT
	P.TIME_ZONE,
	COUNT(*) AS BORROWER_COUNT
FROM
(
SELECT DISTINCT
	PD30.DF_PRS_ID, 
	ISNULL(D.TIME_ZONE, 'FOREIGN COUNTRY') AS TIME_ZONE
FROM
	UDW..LN10_LON LN10
	INNER JOIN UDW..PD30_PRS_ADR PD30
		ON PD30.DF_PRS_ID = LN10.BF_SSN
		AND PD30.DC_ADR = 'L'
	LEFT JOIN @DATA D
		ON D.ST = PD30.DC_DOM_ST
WHERE
	LN10.LA_CUR_PRI > 0
	AND LN10.LC_STA_LON10 = 'R'
) P
GROUP BY 
	P.TIME_ZONE
ORDER BY 
	TIME_ZONE



