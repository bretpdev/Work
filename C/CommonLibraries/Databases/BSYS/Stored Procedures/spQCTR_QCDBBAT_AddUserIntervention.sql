
-- add a record to the user intervention table
CREATE PROCEDURE [dbo].[spQCTR_QCDBBAT_AddUserIntervention]
	@ReportName		varchar(20),
	@UserID			varchar(15),
	@ActivityDate	datetime,
	@Description	varchar(8000),
	@RequiredDays	int,
	@BusinessUnit	varchar(50)

AS
BEGIN
	SET NOCOUNT ON;

	IF ((SELECT     COUNT(ReportName) AS Expr1
	FROM         QCTR_DAT_UserIntervention
	WHERE     (ReportName = @ReportName) AND (UserID = @UserID) AND (ActivityDate = @ActivityDate) AND (BusinessUnit = @BusinessUnit) AND (Description = @Description)) < 1)
	BEGIN
		INSERT INTO dbo.QCTR_DAT_UserIntervention (ReportName,UserID,ActivityDate,Description,RequiredDays,BusinessUnit) VALUES (@ReportName,@UserID,@ActivityDate,@Description,@RequiredDays,@BusinessUnit)
	END
END