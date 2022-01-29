CREATE PROCEDURE [complaints].[ComplaintAddHistory]
	@ComplaintId int,
	@HistoryDetail nvarchar(4000),
	@UpdateResolvesComplaint bit
AS
	insert into [complaints].[ComplaintHistory] (ComplaintId, HistoryDetail)
	values (@ComplaintId, @HistoryDetail)

	declare @Id int = cast(SCOPE_IDENTITY() as int)

	if @UpdateResolvesComplaint = 1
	begin
		update [complaints].Complaints set ResolutionComplaintHistoryId = @Id where ComplaintId = @ComplaintId
	end

	select @Id

RETURN 0