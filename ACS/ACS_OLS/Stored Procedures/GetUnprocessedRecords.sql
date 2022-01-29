CREATE PROCEDURE [acs].[GetUnprocessedRecords]

AS

SELECT
	OneLinkDemographicsId,
	PersonType,
	SSN,
	PD01.DF_SPE_ACC_ID AS AccountNumber,
	AddrDate,
	AddrType,
	FirstFourName,
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
	acs.OneLinkDemographics OLD
	INNER JOIN ODW..PD01_PDM_INF PD01
		ON PD01.DF_PRS_ID = OLD.SSN
WHERE
	DeletedAt IS NULL
	AND
	(
		ProcessedAt IS NULL
		OR ArcAddProcessingId IS NULL
	)
