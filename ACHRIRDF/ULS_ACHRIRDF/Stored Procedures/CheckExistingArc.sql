CREATE PROCEDURE [achrirdf].[CheckExistingArc]
(
	@AccountNumber VARCHAR(10),
	@Arc VARCHAR(5),
	@Comment VARCHAR(MAX)
)
AS

SELECT
	 COALESCE(MAX(AA.ArcAddProcessingId), -1)
FROM 
	(
		SELECT 
			AAP.[ArcAddProcessingId] AS ArcAddProcessingId
		FROM
			[ULS].[dbo].[ArcAddProcessing] AAP
		WHERE
			AAP.AccountNumber = @AccountNumber
			AND AAP.ARC = @Arc
			AND AAP.Comment = @Comment
			AND 
			(
				AAP.ProcessedAt IS NULL
			)
		UNION
		--Check if there is an existing queue, since the arc add processing id is not used in this case the value doesnt matter only that it returns a number.
		SELECT
			CAST(0 AS BIGINT) AS ArcAddProcessingId
		FROM
			[UDW].[dbo].PD10_PRS_NME PD10
			INNER JOIN [UDW].[dbo].[WQ20_TSK_QUE] WQ20
				ON PD10.DF_PRS_ID = WQ20.BF_SSN
				AND WQ20.WF_QUE = 'X5'
				AND CONCAT(WQ20.WX_MSG_1_TSK,WQ20.WX_MSG_2_TSK) = CONCAT(@Comment, '.  { ACHRIRDF   }')
		WHERE
			PD10.DF_SPE_ACC_ID = @AccountNumber
	) AA