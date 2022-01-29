-- =============================================
-- Author:		Jarom Ryan
-- Create date: 03/06/2012
-- Description:	This sp will get the data in the CompassPifBwrLevel table
-- =============================================
CREATE PROCEDURE [dbo].[spCompassPifGetBwrInfo]

AS
BEGIN

	SET NOCOUNT ON;

	SELECT AccountNumber, 
			FirstName, 
			LastName, 
			Address1, 
			Address2, 
			City, 
			State, 
			Zip, 
			Country, 
			EffectiveDate,
			ConsolPif,
			CostCenterCode
			
	FROM CompassPifBwrLevel
	ORDER BY Country DESC
END
