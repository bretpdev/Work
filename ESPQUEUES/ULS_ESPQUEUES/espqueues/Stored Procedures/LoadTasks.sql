CREATE PROCEDURE [espqueues].[LoadTasks] --Retrieves unworked tasks from the DB and inserts them into the processing table for the ESPQUEUES script

AS
	
	DECLARE @TargetGaurantor CHAR(6) = '000749' 

	INSERT INTO ULS.espqueues.ProcessingQueue (BorrowerSsn, [Queue], SubQueue, TaskControlNumber, RequestArc, RequestArcCreatedAt, HasOtherGuarantor)
	SELECT DISTINCT
		EspPop.BorrowerSsn,
		EspPop.[Queue],
		EspPop.SubQueue,
		EspPop.TaskControlNumber,
		EspPop.RequestArc,
		EspPop.RequestArcCreatedAt,
		CASE 
			WHEN (LN10_NON_TGT_GTR.BF_SSN IS NOT NULL) THEN 1 
			ELSE 0 
		END AS HasOtherGuarantor
	FROM
		(
			SELECT DISTINCT 
				LN10.BF_SSN [BorrowerSsn],
				WQ20.WF_QUE [Queue],
				WQ20.WF_SUB_QUE [SubQueue], 
				WQ20.WN_CTL_TSK [TaskControlNumber],
				WQ20.PF_REQ_ACT [RequestArc],
				WQ20.WF_CRT_DTS_WQ20 [RequestArcCreatedAt]
			FROM
				UDW..LN10_LON LN10
				INNER JOIN UDW..WQ20_TSK_QUE WQ20
					ON WQ20.BF_SSN = LN10.BF_SSN
					AND WQ20.WF_QUE = 'RB'
					AND WQ20.WC_STA_WQUE20 = 'U'
			WHERE
				IF_GTR = @TargetGaurantor
				AND LC_STA_LON10 = 'R'
				AND LA_CUR_PRI > 0
		) EspPop
		LEFT JOIN UDW..LN10_LON LN10_NON_TGT_GTR
			ON LN10_NON_TGT_GTR.BF_SSN = EspPop.BorrowerSsn
			AND LN10_NON_TGT_GTR.LC_STA_LON10 = 'R'
			AND LN10_NON_TGT_GTR.LA_CUR_PRI > 0
			AND ISNULL(LN10_NON_TGT_GTR.IF_GTR, '000000') != @TargetGaurantor
		LEFT JOIN ULS.espqueues.ProcessingQueue PQ
			ON PQ.TaskControlNumber = EspPop.TaskControlNumber
			AND PQ.RequestArc = EspPop.RequestArc
			AND PQ.RequestArcCreatedAt = EspPop.RequestArcCreatedAt
			AND PQ.[Queue] = EspPop.[Queue]
			AND PQ.SubQueue = EspPop.SubQueue
			AND PQ.DeletedAt IS NULL
			AND (PQ.ReassignedAt IS NULL OR CAST(PQ.ReassignedAt AS DATE) = CAST(GETDATE() AS DATE))
			AND (PQ.ProcessedAt IS NULL OR CAST(PQ.ProcessedAt AS DATE) = CAST(GETDATE() AS DATE))
	WHERE
		PQ.BorrowerSsn IS NULL
			

RETURN 0
