CREATE PROCEDURE [accurint].[AddNewWork]
	@RunId INT
AS
	
BEGIN TRANSACTION
	
	DECLARE @ERROR INT = 0

	--Get UH tasks to work
	INSERT INTO accurint.DemosProcessingQueue_UH (AccountNumber, EndorserSsn, [Queue], SubQueue, TaskControlNumber, TaskRequestArc, TaskCreatedAt, RunId)
	SELECT DISTINCT
		PD10.DF_SPE_ACC_ID AS AccountNumber,
		CASE 
			WHEN AY10.LC_ATY_RCP = 'E' THEN AY10.LF_ATY_RCP
			ELSE NULL
		END AS EndorserSsn,
		WQ20.WF_QUE AS [Queue],
		WQ20.WF_SUB_QUE AS SubQueue,
		WQ20.WN_CTL_TSK AS TaskControlNumber,
		WQ20.PF_REQ_ACT AS TaskRequestArc,
		WQ20.WF_CRT_DTS_WQ20 AS TaskCreatedAt,
		@RunId AS RunId
	FROM
		UDW..WQ20_TSK_QUE WQ20
		INNER JOIN UDW..PD10_PRS_NME PD10
			ON PD10.DF_PRS_ID = WQ20.BF_SSN
		LEFT JOIN ULS.accurint.DemosProcessingQueue_UH DPQ
			ON DPQ.AccountNumber = PD10.DF_SPE_ACC_ID
			AND DPQ.[Queue] = WQ20.WF_QUE
			AND DPQ.SubQueue = WQ20.WF_SUB_QUE
			AND DPQ.TaskControlNumber = WQ20.WN_CTL_TSK
			AND DPQ.TaskRequestArc = WQ20.PF_REQ_ACT
			AND DPQ.TaskCreatedAt = WQ20.WF_CRT_DTS_WQ20
			AND DPQ.DeletedAt IS NULL
			AND 
			(
				DPQ.AddedToRequestFileAt IS NULL
				OR DPQ.TaskCompletedAt IS NULL
				OR DPQ.RequestArcId IS NULL
				OR 
				(	
					DPQ.ResponseAddressArcId IS NULL
					AND DPQ.ResponsePhoneArcId IS NULL
				)
			)
		LEFT JOIN UDW..AY10_BR_LON_ATY AY10
			ON AY10.BF_SSN = PD10.DF_PRS_ID
			AND AY10.PF_REQ_ACT = WQ20.PF_REQ_ACT
			AND AY10.LN_ATY_SEQ = WQ20.LN_ATY_SEQ 
	WHERE
		WQ20.WF_QUE IN ('IC', 'IM', 'IO')
		AND WQ20.WC_STA_WQUE20 IN ('U')
		AND DPQ.AccountNumber IS NULL

	SELECT @ERROR = @@ERROR;

	--Now Get OneLINK tasks to work
	INSERT INTO accurint.DemosProcessingQueue_OL (AccountNumber, WorkGroup, Department, TaskCreatedAt, SendToAccurint, RunId) 
	SELECT DISTINCT
		PD01.DF_SPE_ACC_ID AS AccountNumber,
		CT30.IF_WRK_GRP AS WorkGroup,
		CT30.IC_REC_TYP AS Department,
		CT30.IF_CRT_DTS_CT30 AS TaskCreatedAt,
		CASE --We send to Accurint if the new skip status post-dates the last skip activity for a specific task, others we send regardless
			WHEN CT30.IF_WRK_GRP != 'ACURINTS' THEN CAST(1 AS BIT)
			WHEN CT30.IF_WRK_GRP = 'ACURINTS' AND PD01.DC_SKP_TRC_STA = 'S' AND CAST(ISNULL(LastSkipAction_OL.SkipActionDate, '1900-01-01') AS DATE) < PD01.DD_SKP_TRC_EFF THEN CAST(1 AS BIT)
			ELSE CAST(0 AS BIT)
		END AS SendToAccurint,
		@RunId AS RunId
	FROM
		ODW..CT30_CALL_QUE CT30
		INNER JOIN ODW..PD01_PDM_INF PD01
			ON PD01.DF_PRS_ID = CT30.DF_PRS_ID_BR
		LEFT JOIN
		(
			SELECT
				MAX(AY01.BF_CRT_DTS_AY01) AS SkipActionDate,
				AY01.DF_PRS_ID
			FROM
				ODW..AY01_BR_ATY AY01
			WHERE
				PF_ACT = 'KUBSS'
			GROUP BY
				AY01.DF_PRS_ID
		) LastSkipAction_OL
			ON LastSkipAction_OL.DF_PRS_ID = PD01.DF_PRS_ID
		LEFT JOIN accurint.DemosProcessingQueue_OL DPQ
			ON DPQ.AccountNumber = PD01.DF_SPE_ACC_ID
			AND DPQ.WorkGroup = CT30.IF_WRK_GRP
			AND DPQ.Department = CT30.IC_REC_TYP
			AND DPQ.TaskCreatedAt = CT30.IF_CRT_DTS_CT30
			AND DPQ.DeletedAt IS NULL
			AND 
			(
				(
					DPQ.RequestCommentAdded IS NULL --No comment on account. All records should have a comment.
					OR
					(
						DPQ.TaskCompletedAt IS NULL --Task not completed. All non-special request records should have their request task completed.
						AND DPQ.WorkGroup IS NULL --If WorkGroup is null, it means this was submitted by a special request file, so there is no corresponding task to close
					)
				)
				OR
				(
					(
						DPQ.AddedToRequestFileAt IS NULL --Haven't put record in request file.
						OR 
						(
							DPQ.AddressTaskQueueId IS NULL --Haven't created task off of response file record.
							AND DPQ.PhoneTaskQueueId IS NULL
						)
					)
					AND
					(
						DPQ.SendToAccurint IS NULL --Haven't determined whether we need to send the record.
						OR DPQ.SendToAccurint = 1 --Determined we need to send the record in the request file.
					)
				)
			)
	WHERE
		CT30.IF_WRK_GRP IN ('ACURINTS','ACURINT2')
		AND IC_TSK_STA = 'A' --Available
		AND DPQ.AccountNumber IS NULL

	SELECT @ERROR = @ERROR + @@ERROR;


IF @ERROR = 0
	BEGIN
		COMMIT TRANSACTION
		SELECT CAST(1 AS BIT) AS WasSuccessful; --Succeeded
	END
ELSE
	BEGIN
		ROLLBACK TRANSACTION
		SELECT CAST(0 AS BIT) AS WasSuccessful; --Failed
	END

RETURN 0;
