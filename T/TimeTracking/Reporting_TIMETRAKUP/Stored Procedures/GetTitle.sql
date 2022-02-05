CREATE PROCEDURE [dbo].[GetTitle]
	@System varchar(25),
	@RequestNumber int
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
	IF @System = 'Need Help'
		BEGIN
			SELECT
				[Subject]
			FROM
				NeedHelpUheaa.dbo.DAT_Ticket
			WHERE
				Ticket = @RequestNumber
		END