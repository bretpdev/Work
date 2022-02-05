

CREATE PROCEDURE [peps].[GetOtherAddressData]

AS
	SELECT 
		[OTHERADD_ID] AS RecordId,
		[RecordType],
		[OpeId],
		[ChangeIndicator],
		[AddressType],
		[Line1Adr],
		[Line2Adr],
		[City],
		[Country],
		[State],
		[Zip],
		[ForeignProvinceName],
		[OtherAreaCode],
		[OtherExchange],
		[OtherExt],
		[OtherExt2],
		[OtherForeignPhone],
		[OtherFax],
		[OtherInternetAdr],
		[FscLocName],
		[FscContFirstName],
		[FscContLastName],
		[Filler]
	FROM 
		[CLS].[peps].[OTHERADD]
	WHERE
		ProcessedAt IS NULL 
		AND
		DeletedAt IS NULL
RETURN 0
