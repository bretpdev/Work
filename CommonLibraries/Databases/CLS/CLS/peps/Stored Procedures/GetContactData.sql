CREATE PROCEDURE [peps].[GetContactData]
	AS
	SELECT
		[CONTACT_ID] AS RecordId,
		[RecordType],
		[OpeId],
		[ChangeIndicator],
		[ContType],
		[ContStreet1],
		[ContStreet2],
		[ContCity],
		[ContState],
		[Zip],
		[ContProvince],
		[ContCountry],
		[ContSaluataion],
		[ContFirstName],
		[ContMI],
		[ContLastName],
		[ContSuffix],
		[ContAreaCode],
		[ContExchange],
		[ContExt],
		[ContExt2],
		[ContForeignPhone],
		[ContFax],
		[ContInternetAdd],
		[ContEffectDte],
		[ContEndDte],
		[SchoolsJobTitle],
		[ContSysId],
		[Filler]
	FROM 
		[CLS].[peps].[CONTACT]
	WHERE
		ProcessedAt IS NULL 
		AND
		DeletedAt IS NULL
RETURN 0
