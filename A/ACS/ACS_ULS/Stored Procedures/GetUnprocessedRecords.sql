CREATE PROCEDURE [acs].[GetUnprocessedRecords]

AS

SELECT
	UheaaDemographicsId,
	PersonType,
	SSN,
	PD10.DF_SPE_ACC_ID AS AccountNumber,
	AddrDate,
	AddrType,
	FullName,
	Address1,
	Address2,
	City,
	[State],
	Zip,
	NewAddressFull,
	OldAddressFull,
	FileId
FROM
	ULS.acs.UheaaDemographics UD
	INNER JOIN UDW..PD10_PRS_NME PD10
		ON PD10.DF_PRS_ID = UD.SSN
WHERE
	DeletedAt IS NULL
	AND
	(
		ProcessedAt IS NULL
		OR ArcAddProcessingId IS NULL
	)
