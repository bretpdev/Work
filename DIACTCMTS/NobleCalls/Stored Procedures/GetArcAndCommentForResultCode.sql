CREATE PROCEDURE [dbo].[GetArcAndCommentForResultCode]
(
	@DispositionCode VARCHAR(50)
)

AS
	SELECT 
		A.Arc,
		C.Comment,
		RC.ResponseCode
	FROM
		DispositionCodeMapping DCM
		INNER JOIN Arcs A
			ON A.ArcId = DCM.ArcId
		INNER JOIN Comments C
			ON C.CommentId = DCM.CommentId
		INNER JOIN ResponseCodes RC
			ON RC.ResponseCodeId = DCM.ResponseCodeId
	WHERE 
		DispositionCode = @DispositionCode

