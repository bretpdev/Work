CREATE TABLE [pridrcrp].[MonetaryHistoryToBorrowerInformation]
(
	MonetaryHistoryId INT,
	BorrowerInformationId INT,
	CONSTRAINT PK_MonetaryHistoryToBorrowerInformation PRIMARY KEY (MonetaryHistoryId, BorrowerInformationId)
)