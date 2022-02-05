-- =============================================
-- Author:		JAROM RYAN
-- Create date: 06/18/2013
-- Description:	WILL GET THE ARC AND COMMENT FOR A GIVEN APPLICATION ID
-- =============================================
CREATE PROCEDURE [dbo].[spGetArcAndComment]

@AppId INT



AS
BEGIN

	SET NOCOUNT ON;

	SELECT 
		HIS.repayment_plan_type_status_history_id AS HisId,
		ALD.arc As Arc,
		ALD.system_comment As Comment,
		ALD.letter_id As LetterId,
		ALD.letter_merge_comment As LetterMergeText
	FROM 
		Repayment_Plan_Selected RPS
	INNER JOIN Repayment_Type RT
		ON RT.repayment_type_id = RPS.repayment_type_id
	INNER JOIN Repayment_Plan_Type_Status_History HIS
		ON HIS.repayment_plan_type_id = RPS.repayment_plan_type_id
	INNER JOIN Repayment_Plan_Type_Substatus SUBSTA
		ON SUBSTA.repayment_plan_type_substatus_id = HIS.repayment_plan_type_status_mapping_id
	INNER JOIN Repayment_Type_Status RTS
		ON RTS.repayment_type_status_id = SUBSTA.repayment_type_status_id
	LEFT JOIN Arc_Letter_Mapping ALM
		ON ALM.repayment_type_id = RT.repayment_type_id
		AND ALM.repayment_type_status_id = RTS.repayment_type_status_id
	LEFT JOIN Arc_Letter_Data ALD
		ON ALD.mapping_id = ALM.mapping_id
	WHERE RPS.application_id = @AppId
END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[spGetArcAndComment] TO [db_executor]
    AS [dbo];

