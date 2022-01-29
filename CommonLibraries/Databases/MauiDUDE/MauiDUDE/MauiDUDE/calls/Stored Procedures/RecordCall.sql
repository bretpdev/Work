CREATE PROCEDURE [calls].[RecordCall]
	@ReasonId int,
	@Comments nvarchar(30),
	@LetterId nvarchar(10),
	@IsCornerstone bit,
	@IsOutbound bit
AS
	insert into [calls].CallRecords (ReasonId, Comments, LetterId, IsCornerstone, IsOutbound)
	values (@ReasonId, @Comments, @LetterId, @IsCornerstone, @IsOutbound)
RETURN 0
