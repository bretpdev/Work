CREATE PROCEDURE [hrbridge].[GetDestinations]
AS
	SELECT
		DestinationSource,
		DestinationValue,
		Destination
	FROM
		Destinations
RETURN 0
