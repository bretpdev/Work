CREATE PROCEDURE spDemographicUpdateGetArc
	-- Add the parameters for the stored procedure here
	@Queue				CHAR(4),
	@DemographicType	VARCHAR(10),
	@RejectReason		VARCHAR(100)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT Arc
	FROM DemographicUpdateArc
	WHERE [Queue] = @Queue
		AND DemographicType = @DemographicType
		AND RejectReason = @RejectReason
END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[spDemographicUpdateGetArc] TO [db_executor]
    AS [dbo];



