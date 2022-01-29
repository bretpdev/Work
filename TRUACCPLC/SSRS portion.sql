SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
SELECT
	AccountNumber AS [Account Number],
	AccountNamespace AS [Account Namespace],
	AccountRelationship AS [Account Relationship],
	CONVERT(VARCHAR,AccountOpenDate,101) AS [Account Open Date],
	BrandId AS [Brand ID],
	ProductType AS [Product Type],
	CONVERT(VARCHAR,DateAssigned,101) AS [Date Assigned],
	CONVERT(VARCHAR,ExpectedRetractionDate,101) AS [Expected Retraction Date],
	CONVERT(VARCHAR,TransactionDate,101) AS [Transaction Date],
	FirstName AS [First Name],
	LastName AS [Last Name],
	MiddleName AS [Middle Name],
	Prefix,
	Suffix,
	EmailAddress1 AS [Email Address 1],
	EmailAddress2 AS [Email Address 2],
	EmailAddress3 AS [Email Address 3],
	Telephone1 AS [Telephone 1],
	Telephone2 AS [Telephone 2],
	Telephone3 AS [Telephone 3],
	AddressLine1 AS [Address Line-1],
	AddressLine2 AS [Address Line-2],
	AddressLine3 AS [Address Line-3],
	AddressType AS [Address Type],
	City,
	[State],
	ZipCode AS [Zip/Postal Code],
	TotalAmountDue AS [Total Amount Due],
	CurrentAmountDue AS [Current Amount Due],
	CurrentBalance AS [Current Balance],
	TotalDelinquentAmount AS [Total Delinquent Amount],
	CONVERT(VARCHAR,DelinquencyDate,101) AS [Delinquency Date],
	CyclesDelinquent AS [Cycles Delinquent],
	LastPaymentAmount AS [Last Payment Amount],
	CONVERT(VARCHAR,LastPaymentDate,101) AS [Last Payment Date],
	MonthToDateFeesPaid AS [Month to Date Fees Paid],
	MonthToDateInterestPaid AS [Month to Date Interest Paid],
	MonthToDatePrincipalPaid AS [Month to Date Principal Paid],
	PlacementNumber AS [Placement Number],
	CreatedAt,
	RetractedAt,
	RetractedBy,
	DeletedAt,
	DeletedBy
FROM
	OLS.trueaccord.Placements P
WHERE
	DeletedAt IS NULL
	AND RetractedAt IS NULL
	AND CAST(CreatedAt AS DATE) = CAST(GETDATE() AS DATE)