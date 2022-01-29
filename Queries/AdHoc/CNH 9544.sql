DECLARE @Populations TABLE
(
	MATCHING_PERIOD VARCHAR(XXX)
	,ADS_DATE VARCHAR(X)
)

INSERT INTO
	@Populations
(
	MATCHING_PERIOD
	,ADS_DATE
)
VALUES
('the XXX days prior to an Active Duty Request Date of July X, XXXX','XXXXXXXX'),
('the XXX days prior to an Active Duty Request Date of July X, XXXX','XXXXXXXX'),
('the XXX days prior to an Active Duty Request Date of July X, XXXX','XXXXXXXX'),
('the XXX days prior to an Active Duty Request Date of July X, XXXX','XXXXXXXX'),
('from August XX, XXXX thru an Active Duty Request Date of July X, XXXX','XXXXXXXX')

SELECT * 
FROM @Populations A
FULL OUTER JOIN [CLS].[scra].[Data_CRXXXX] B
ON A.ADS_DATE = B.Active_Duty_Status_Date


SELECT 
	CONVERT(NUMERIC(XX),Number_of_Borrowers_Reviewed) AS Number_of_Borrowers_Reviewed
	,Number_of_Active_Duty_Borrowers
	,Active_Duty_Status_Date
	,CreatedAt
	,CASE
		WHEN Active_Duty_Status_Date = 'XXXXXXXX'
		THEN 'the XXX days prior to an Active Duty Request Date of July X, XXXX'
		WHEN Active_Duty_Status_Date = 'XXXXXXXX'
		THEN 'the XXX days prior to an Active Duty Request Date of July X, XXXX'
		WHEN Active_Duty_Status_Date = 'XXXXXXXX'
		THEN 'the XXX days prior to an Active Duty Request Date of July X, XXXX'
		WHEN Active_Duty_Status_Date = 'XXXXXXXX'
		THEN 'the XXX days prior to an Active Duty Request Date of July X, XXXX'
		WHEN Active_Duty_Status_Date = 'XXXXXXXX'
		THEN 'from August XX, XXXX thru an Active Duty Request Date of July X, XXXX'
	ELSE ''
	END AS MATCHING_PERIOD	
FROM [CLS].[scra].[Data_CRXXXX]
ORDER BY Active_Duty_Status_Date DESC

