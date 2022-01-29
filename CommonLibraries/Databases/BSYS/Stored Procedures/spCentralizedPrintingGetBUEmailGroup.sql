CREATE PROCEDURE [dbo].[spCentralizedPrintingGetBUEmailGroup]
	@BusinessUnit varchar(50)
AS
	SELECT AssociatedEmailAddr FROM GENR_LST_BusinessUnitEmailAddrs WHERE BusinessUnit = @BusinessUnit;
RETURN 0
