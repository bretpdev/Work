CREATE PROCEDURE [pridrcrp].[InsertToReviewQueue]
	@SSN VARCHAR(9),
	@ArcAddProcessingId BIGINT,
	@ExceptionLog VARCHAR(MAX)
AS
BEGIN

INSERT INTO [pridrcrp].[ReviewQueue]
           ([SSN]
		   ,[ArcAddProcessingId]
           ,[ExceptionLog]
           ,[CreatedAt]
           ,[ReviewDate]
           ,[Reviewer]
           ,[ReviewComment]
           ,[DeletedAt]
           ,[DeletedBy])
     VALUES
           (@SSN
		   ,@ArcAddProcessingId
           ,@ExceptionLog
           ,GETDATE()
           ,NULL
           ,NULL
           ,NULL
           ,NULL
           ,NULL)

END