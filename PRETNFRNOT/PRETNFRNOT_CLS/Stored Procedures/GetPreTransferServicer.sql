CREATE PROCEDURE [pretnfrnot].[GetPreTransferServicer]
	@RegionDeconversion char(3)
AS

	SELECT
		PreTransferServicerId,
		RegionDeconversion,
		ServicerName,
		Website,
		Phone,
		PaymentAddress,
		CorrespondenceAddress
	FROM
		PreTransferServicer
	WHERE
		RegionDeconversion = @RegionDeconversion

RETURN 0
