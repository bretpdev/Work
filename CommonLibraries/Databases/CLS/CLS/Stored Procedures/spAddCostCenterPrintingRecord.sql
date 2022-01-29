-- =============================================
-- Author:		Daren Beattie
-- Create date: November 3, 2011
-- =============================================
CREATE PROCEDURE spAddCostCenterPrintingRecord
	-- Add the parameters for the stored procedure here
	@LetterId		VARCHAR(10),
	@ForeignCount	INT = 0,
	@DomesticCount	INT = 0,
	@CostCenterCode	CHAR(6) = 'MA4481'
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	INSERT INTO CostCenterPrinting (
		PrintDateTime,
		LetterId,
		ForeignCount,
		DomesticCount,
		CostCenterCode
	)
	VALUES (
		GETDATE(),
		@LetterId,
		@ForeignCount,
		@DomesticCount,
		@CostCenterCode
	)
END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[spAddCostCenterPrintingRecord] TO [UHEAA\CornerStoneUsers]
    AS [dbo];




GO
GRANT EXECUTE
    ON OBJECT::[dbo].[spAddCostCenterPrintingRecord] TO [UHEAA\SystemAnalysts]
    AS [dbo];




GO
GRANT EXECUTE
    ON OBJECT::[dbo].[spAddCostCenterPrintingRecord] TO [db_executor]
    AS [dbo];



