CREATE PROCEDURE [dbo].[GetNTISReturnInfo]

AS
	SELECT 
		ReturnName,
		AddressLine1,
		AddressLine2,
		AddressLine3,
		City,
		[State],
		ReturnZip,
		ReturnZipAddOn
	FROM
		NTISReturnContactInfo
RETURN 0
