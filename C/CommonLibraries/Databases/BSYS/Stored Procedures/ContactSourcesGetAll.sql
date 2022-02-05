CREATE PROCEDURE [dbo].[ContactSourcesGetAll]
AS
	SELECT [Source], ActivityType, ContactType FROM INCA_LST_ContactSources
RETURN 0
