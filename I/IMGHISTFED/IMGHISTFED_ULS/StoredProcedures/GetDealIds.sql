CREATE PROCEDURE [imghistfed].[GetDealIds]
AS
	SELECT
		DealId,
		SaleDate
	FROM
		imghistfed.DealIds
