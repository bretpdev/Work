SELECT
	QuestionID [question_number],
	CAST(Revised as DATE) [revision_date],
	RTRIM(Question) [question],
	RTRIM(Answer) [answer]
FROM
	MauiDUDE.[dbo].[QuestionTB] 
ORDER BY
	question_number