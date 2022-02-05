CREATE PROCEDURE [aesxmtdial].[GetCallHistory]
	@CreatedAt DATETIME
AS
	SELECT 
		NCH.DialerField1 [TargetID],
		CT30.IC_REC_TYP [QueueRegion],
		CT30.IF_WRK_GRP [Queue],
		CT30.DF_PRS_ID_BR [BorrowerSSN],
		NCH.AgentId [UserID],
		NCH.Disposition [OnelinkDisposition],
		CASE
			WHEN NCH.PhoneNumber = NCH.DialerField14 THEN NCH.DialerField13
			WHEN NCH.PhoneNumber = NCH.DialerField16 THEN NCH.DialerField15
			WHEN NCH.PhoneNumber = NCH.DialerField18 THEN NCH.DialerField17
			ELSE 'D'
		END [PhoneIndicator],
		NCH.PhoneNumber [PhoneNumber],
		CONVERT(CHAR(8), FirstActivity, 112) [DateDialed],
		REPLACE(CONVERT(CHAR(5), FirstActivity, 108), ':', '') [TimeDialed]
	FROM
		ODW..CT30_CALL_QUE CT30
		INNER JOIN ODW..PD01_PDM_INF PD01
			ON PD01.DF_PRS_ID = CT30.DF_PRS_ID_BR
		INNER JOIN
		(
			SELECT
				Response.DialerField1,
				Response.DialerField2,
				Response.DialerField7,
				Response.ActivityDate,
				Response.AgentId,
				Response.PhoneNumber,
				Response.DialerField13,
				Response.DialerField14,
				Response.DialerField15,
				Response.DialerField16,
				Response.DialerField17,
				Response.DialerField18,
				Response.Disposition,
				MIN(ActivityDate) OVER (PARTITION BY Response.DialerField1, Response.DialerField2 ORDER BY Response.ResponseCodeWeight DESC, Response.ActivityDate) AS FirstActivity,
				ROW_NUMBER() OVER (PARTITION BY Response.DialerField1, Response.DialerField2 ORDER BY Response.ResponseCodeWeight DESC, Response.ActivityDate) AS RankedCall 
			FROM
			(
				SELECT
					NCH.DialerField1,
					NCH.dialerfield2,
					NCH.DialerField7,
					NCH.ActivityDate,
					NCH.AgentId,
					NCH.PhoneNumber,
					NCH.DialerField13,
					NCH.DialerField14,
					NCH.DialerField15,
					NCH.DialerField16,
					NCH.DialerField17,
					NCH.DialerField18,
					ISNULL(DCM.OnelinkDisposition, '') AS Disposition,
					CASE WHEN RC.ResponseCode = 'CNTCT' THEN 1 ELSE 0 END AS ResponseCodeWeight
				FROM
					NobleCalls..NobleCallHistory NCH
					INNER JOIN NobleCalls..DispositionCodeMapping DCM
						ON DCM.DispositionCode = NCH.DispositionCode
					INNER JOIN NobleCalls..ResponseCodes RC
						ON RC.ResponseCodeId = DCM.ResponseCodeId
				WHERE
					NCH.IsInbound = 0 --outbound only
					AND NCH.RegionId = '1' --Onelink
					AND COALESCE(LTRIM(RTRIM(NCH.DialerField1)),'') != ''
					AND CAST(NCH.CreatedAt AS DATE) = CAST(@CreatedAt AS DATE)
					AND NCH.DeletedAt IS NULL
			) Response
		) NCH
			ON NCH.DialerField7 = PD01.DF_SPE_ACC_ID
			AND NCH.DialerField1 = CT30.DF_PRS_ID_TGT
			AND NCH.DialerField2 = CT30.IC_REC_TYP
			AND NCH.PhoneNumber IN (CT30.DN_TGT_PHN, CT30.DN_TGT_ALT_PHN, CT30.DN_TGT_OTH_PHN)
			AND NCH.RankedCall = 1
		INNER JOIN NobleCalls.aesxmtdial.OnelinkDialerQueues Q
			ON Q.[Queue] = CT30.IF_WRK_GRP
			AND Q.Region = CT30.IC_REC_TYP
		INNER JOIN ODW..CT20_CAS_REC CT20
			ON CT20.IF_COL = Q.[Queue]
			AND CT20.IC_COL_TYP = Q.Region
		INNER JOIN ODW..CT10_COL_MST CT10
			ON CT10.IF_COL = Q.[Queue]
			AND CT10.IC_COL_TYP = Q.Region
	WHERE
		CT20.IC_QUE_TYP IN ('C','S')
		AND CT10.ID_COL_INA_STA IS NULL
		AND CT30.IC_TSK_STA IN ('A','W')
	ORDER BY
		PhoneIndicator,
		NCH.DialerField1
