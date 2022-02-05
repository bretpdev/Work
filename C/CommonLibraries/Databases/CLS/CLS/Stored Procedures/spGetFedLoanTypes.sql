
CREATE PROCEDURE [dbo].[spGetFedLoanTypes] @queryARC varchar(5) AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	SELECT COUNT(*) AS ARCCount
	FROM ArcAdd 
	WHERE ARC = @queryARC

END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[spGetFedLoanTypes] TO [db_executor]
    AS [dbo];



