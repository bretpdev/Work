CREATE PROCEDURE dbo.spNDHP_StatusUpdate 

@Ticket     		bigint,
@Status		varchar(50),
@Agent			nvarchar(50)

AS

/* end last status */
UPDATE dbo.NDHP_REF_Statuses
SET EndDate = GETDATE()
WHERE Ticket = @Ticket 
	AND EndDate = ''


/* begin new status */
INSERT 
INTO dbo.NDHP_REF_Statuses (Ticket, Status, Court) 
VALUES (@Ticket, @Status, @Agent)