CREATE PROCEDURE [dbo].[GetCountries]
AS

	select CountryCode, CountryName from Countries

RETURN 0
