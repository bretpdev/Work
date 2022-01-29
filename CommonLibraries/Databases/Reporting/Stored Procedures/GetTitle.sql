CREATE PROCEDURE [dbo].[GetTitle]
	@System varchar(25),
	@RequestNumber int,
	@CornerstoneRegion bit
AS
	
	IF @System = 'Sacker Script'
		BEGIN
			SELECT
				Script
			FROM
				BSYS.dbo.SCKR_DAT_ScriptRequests
			WHERE
				Request = @RequestNumber
		END
	IF @System = 'Sacker SAS'
		BEGIN
			SELECT
				Job
			FROM
				BSYS.dbo.SCKR_DAT_SasRequests
			WHERE
				Request = @RequestNumber
		END
	IF @System = 'Letter Tracking'
		BEGIN
			SELECT
				DocName
			FROM
				BSYS.dbo.LTDB_DAT_Requests
			WHERE
				Request = @RequestNumber
		END
	IF @System = 'Need Help' AND @CornerstoneRegion = 1
		BEGIN
			SELECT
				[Subject]
			FROM
				NeedHelpCornerStone.dbo.DAT_Ticket
			WHERE
				Ticket = @RequestNumber
		END
	IF @System = 'Need Help' AND @CornerstoneRegion = 0
		BEGIN
			SELECT
				[Subject]
			FROM
				NeedHelpUheaa.dbo.DAT_Ticket
			WHERE
				Ticket = @RequestNumber
		END

RETURN 0

GRANT EXECUTE ON [dbo].[GetTitle] TO db_executor
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetTitle] TO [db_executor]
    AS [dbo];

