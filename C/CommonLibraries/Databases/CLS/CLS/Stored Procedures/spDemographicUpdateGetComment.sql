CREATE PROCEDURE spDemographicUpdateGetComment
	-- Add the parameters for the stored procedure here
	@RejectReason		VARCHAR(100)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT Comment
	FROM DemographicUpdateRejectReason
	WHERE RejectReason = @RejectReason
END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[spDemographicUpdateGetComment] TO [db_executor]
    AS [dbo];



