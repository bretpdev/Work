-- =============================================
-- Author:		Daren Beattie
-- Create date: August 23, 2011
-- Description:	Gets the names of business units that apply to Commercial Incident Reporting.
-- =============================================
CREATE PROCEDURE [dbo].[spGetBusinessUnits]
	-- Add the parameters for the stored procedure here
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT BU.Name, BU.ID
	FROM CSYS.dbo.GENR_LST_BusinessUnits BU
	INNER JOIN CSYS.dbo.GENR_REF_BusinessUnitVisibleToSystem VIS
		ON BU.ID = VIS.BusinessUnitId
	WHERE VIS.System = 'Incident Reporting Commercial'
END