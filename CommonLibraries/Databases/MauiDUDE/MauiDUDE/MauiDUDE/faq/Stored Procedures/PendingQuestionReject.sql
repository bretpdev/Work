﻿CREATE PROCEDURE [faq].[PendingQuestionReject]
	@PendingQuestionId int
AS
	update faq.PendingQuestions
	   set ProcessedOn = getdate(), RejectedBy = SYSTEM_USER
	 where PendingQuestionId = @PendingQuestionId
RETURN 0
grant execute on [faq].[PendingQuestionReject] to [db_executor]