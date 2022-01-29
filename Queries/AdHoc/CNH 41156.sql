DECLARE @Inbound TABLE(rowid int, call_type int, listid varchar(XX), appl varchar(X), fillerX varchar(XX), ani_code varchar(XX), ani_phone varchar(XXX), addi_status varchar(X), [status] varchar(X), tsr varchar(X), call_date datetime, call_time varchar(XXX), vox_file_name varchar(XX), time_connect int, TimeACW int, TimeHold int, AgentHold int, FillerX varchar(XX), FillerX int, FillerX int, d_record_id int)
DECLARE @Outbound TABLE(CDW_DF_SPE_ACC_ID varchar(XX), UDW_DF_SPE_ACC_ID varchar(XX), CDW_CoborrowerAccountNumber VARCHAR(XX), UDW_CoborrowerAccountNumber VARCHAR(XX), rowid int, call_type int, listid varchar(XX), appl varchar(X), lm_fillerX varchar(XX), areacode varchar(XX), phone varchar(XXX), addi_status varchar(X), [status] varchar(X), tsr varchar(X), act_date datetime, act_time varchar(XXX), vox_file_name varchar(XX), time_connect int, TimeACW int, TimeHold int, AgentHold int, FillerX varchar(XX), FillerX int, FillerX int, d_record_id int)

DECLARE @QUERYXXXXXXIn AS VARCHAR(MAX) = 
'
	SELECT 
		* 
	FROM OPENQUERY(UHEAARAS, 
		''SELECT 
			IB.rowid, 
			IB.call_type, 
			IB.listid, 
			IB.appl,
			IB.fillerX, 
			IB.ani_acode, 
			IB.ani_phone, 
			IB.addi_status, 
			IB.status, 
			IB.tsr, 
			IB.call_date, 
			IB.call_time, 
			RP.vox_file_name, 
			IB.time_connect,
			IB.time_acwork AS TimeACW,
			IB.time_holding AS TimeHold,
			CHH.time_hold AS AgentHold,
			IB.fillerX AS FillerX,
			IB.fillerX AS FillerX,
			IB.fillerX AS FillerX,
			IB.d_record_id
		FROM 
			inboundloghsXXXXXX IB 
			LEFT JOIN callhistholdXXXXXX CHH
				ON IB.d_record_id = CHH.d_record_id
				AND IB.tsr = CHH.tsr
			LEFT JOIN rec_playinthXXXXXX RP 
				ON IB.call_date = RP.call_date 
				AND IB.appl = RP.appl 
				AND IB.d_record_id = RP.d_record_id
				AND IB.tsr = RP.tsr
		WHERE
			IB.status IS NOT NULL
			AND IB.ani_acode IS NOT NULL
			AND IB.ani_phone IS NOT NULL
	'')
'
INSERT INTO @Inbound
EXEC(@QUERYXXXXXXIn)

DECLARE @QUERYXXXXXXIn AS VARCHAR(MAX) = 
'
	SELECT 
		* 
	FROM OPENQUERY(UHEAARAS, 
		''SELECT 
			IB.rowid, 
			IB.call_type, 
			IB.listid, 
			IB.appl,
			IB.fillerX, 
			IB.ani_acode, 
			IB.ani_phone, 
			IB.addi_status, 
			IB.status, 
			IB.tsr, 
			IB.call_date, 
			IB.call_time, 
			RP.vox_file_name, 
			IB.time_connect,
			IB.time_acwork AS TimeACW,
			IB.time_holding AS TimeHold,
			X AS AgentHold,
			IB.fillerX AS FillerX,
			IB.fillerX AS FillerX,
			IB.fillerX AS FillerX,
			IB.d_record_id
		FROM 
			inboundloghsXXXXXX IB 
			LEFT JOIN rec_playinthXXXXXX RP 
				ON IB.call_date = RP.call_date 
				AND IB.appl = RP.appl 
				AND IB.d_record_id = RP.d_record_id
				AND IB.tsr = RP.tsr
		WHERE
			IB.status IS NOT NULL
			AND IB.ani_acode IS NOT NULL
			AND IB.ani_phone IS NOT NULL
	'')
'
INSERT INTO @Inbound
EXEC(@QUERYXXXXXXIn)

DECLARE @QUERYXXXXXXIn AS VARCHAR(MAX) = 
'
	SELECT 
		* 
	FROM OPENQUERY(UHEAARAS, 
		''SELECT 
			IB.rowid, 
			IB.call_type, 
			IB.listid, 
			IB.appl,
			IB.fillerX, 
			IB.ani_acode, 
			IB.ani_phone, 
			IB.addi_status, 
			IB.status, 
			IB.tsr, 
			IB.call_date, 
			IB.call_time, 
			RP.vox_file_name, 
			IB.time_connect,
			IB.time_acwork AS TimeACW,
			IB.time_holding AS TimeHold,
			CHH.time_hold AS AgentHold,
			IB.fillerX AS FillerX,
			IB.fillerX AS FillerX,
			IB.fillerX AS FillerX,
			IB.d_record_id
		FROM 
			inboundloghsXXXXXX IB 
			LEFT JOIN callhistholdXXXXXX CHH
				ON IB.d_record_id = CHH.d_record_id
				AND IB.tsr = CHH.tsr
			LEFT JOIN rec_playinthXXXXXX RP 
				ON IB.call_date = RP.call_date 
				AND IB.appl = RP.appl 
				AND IB.d_record_id = RP.d_record_id
				AND IB.tsr = RP.tsr
		WHERE
			IB.status IS NOT NULL
			AND IB.ani_acode IS NOT NULL
			AND IB.ani_phone IS NOT NULL
	'')
'
INSERT INTO @Inbound
EXEC(@QUERYXXXXXXIn)

DECLARE @QUERYXXXXXXIn AS VARCHAR(MAX) = 
'
	SELECT 
		* 
	FROM OPENQUERY(UHEAARAS, 
		''SELECT 
			IB.rowid, 
			IB.call_type, 
			IB.listid, 
			IB.appl,
			IB.fillerX, 
			IB.ani_acode, 
			IB.ani_phone, 
			IB.addi_status, 
			IB.status, 
			IB.tsr, 
			IB.call_date, 
			IB.call_time, 
			RP.vox_file_name, 
			IB.time_connect,
			IB.time_acwork AS TimeACW,
			IB.time_holding AS TimeHold,
			CHH.time_hold AS AgentHold,
			IB.fillerX AS FillerX,
			IB.fillerX AS FillerX,
			IB.fillerX AS FillerX,
			IB.d_record_id
		FROM 
			inboundloghsXXXXXX IB 
			LEFT JOIN callhistholdXXXXXX CHH
				ON IB.d_record_id = CHH.d_record_id
				AND IB.tsr = CHH.tsr
			LEFT JOIN rec_playinthXXXXXX RP 
				ON IB.call_date = RP.call_date 
				AND IB.appl = RP.appl 
				AND IB.d_record_id = RP.d_record_id
				AND IB.tsr = RP.tsr
		WHERE
			IB.status IS NOT NULL
			AND IB.ani_acode IS NOT NULL
			AND IB.ani_phone IS NOT NULL
	'')
'
INSERT INTO @Inbound
EXEC(@QUERYXXXXXXIn)

DECLARE @QUERYXXXXXXIn AS VARCHAR(MAX) = 
'
	SELECT 
		* 
	FROM OPENQUERY(UHEAARAS, 
		''SELECT 
			IB.rowid, 
			IB.call_type, 
			IB.listid, 
			IB.appl,
			IB.fillerX, 
			IB.ani_acode, 
			IB.ani_phone, 
			IB.addi_status, 
			IB.status, 
			IB.tsr, 
			IB.call_date, 
			IB.call_time, 
			RP.vox_file_name, 
			IB.time_connect,
			IB.time_acwork AS TimeACW,
			IB.time_holding AS TimeHold,
			CHH.time_hold AS AgentHold,
			IB.fillerX AS FillerX,
			IB.fillerX AS FillerX,
			IB.fillerX AS FillerX,
			IB.d_record_id
		FROM 
			inboundloghsXXXXXX IB 
			LEFT JOIN callhistholdXXXXXX CHH
				ON IB.d_record_id = CHH.d_record_id
				AND IB.tsr = CHH.tsr
			LEFT JOIN rec_playinthXXXXXX RP 
				ON IB.call_date = RP.call_date 
				AND IB.appl = RP.appl 
				AND IB.d_record_id = RP.d_record_id
				AND IB.tsr = RP.tsr
		WHERE
			IB.status IS NOT NULL
			AND IB.ani_acode IS NOT NULL
			AND IB.ani_phone IS NOT NULL
	'')
'
INSERT INTO @Inbound
EXEC(@QUERYXXXXXXIn)

/*Out Begin*/
DECLARE @QUERYXXXXXXOut AS VARCHAR(MAX) = 
'
	SELECT DISTINCT
		CPDXX.DF_SPE_ACC_ID AS CDW_DF_SPE_ACC_ID,
		UPDXX.DF_SPE_ACC_ID AS UDW_DF_SPE_ACC_ID,
		NULL AS CDW_CoborrowerAccountNumber,
		NULL AS UDW_CoborrowerAccountNumber,
		OQ.* 
	FROM OPENQUERY(UHEAARAS, 
		''SELECT 
			CH.rowid, 
			CH.call_type, 
			CH.listid, 
			CH.appl, 
			CH.lm_fillerX, 
			CH.areacode, 
			CH.phone, 
			CH.addi_status, 
			CH.status, 
			CH.tsr,
			CH.act_date, 
			CH.act_time, 
			RP.vox_file_name, 
			CH.time_connect,
			CH.time_acw AS TimeACW,
			CH.time_hold AS TimeHold,
			CHH.time_hold AS AgentHold,
			CH.lm_fillerX AS FillerX,
			CH.lm_fillerX AS FillerX,
			CH.lm_fillerX AS FillerX,
			CH.d_record_id
		FROM 
			call_historyXXXXXX CH 
			LEFT JOIN callhistholdXXXXXX CHH
				ON CH.d_record_id = CHH.d_record_id
				AND CH.tsr = CHH.tsr
			LEFT JOIN rec_playinthXXXXXX RP 
				ON CH.d_record_id = RP.d_record_id 
				AND CH.act_date = RP.call_date 
				AND CH.appl = RP.appl
				AND CH.tsr = RP.tsr 
		WHERE 
			CH.call_type != X 
			AND CH.lm_fillerX != ''''''''
			AND CH.status IS NOT NULL
			AND CH.areacode IS NOT NULL
			AND CH.phone IS NOT NULL
	'') OQ
		LEFT JOIN 
		(
			SELECT DISTINCT
				CPDXX.DF_PRS_ID,
				CPDXX.DF_SPE_ACC_ID
			FROM
				CDW..PDXX_PRS_NME CPDXX
				INNER JOIN CDW..LNXX_LON CLNXX
					ON CLNXX.BF_SSN = CPDXX.DF_PRS_ID
		) CPDXX
			ON OQ.LM_FILLERX = CPDXX.DF_PRS_ID
		LEFT JOIN 
		(
			SELECT DISTINCT
				UPDXX.DF_PRS_ID,
				UPDXX.DF_SPE_ACC_ID
			FROM
				UDW..PDXX_PRS_NME UPDXX
				INNER JOIN UDW..LNXX_LON ULNXX
					ON ULNXX.BF_SSN = UPDXX.DF_PRS_ID
		) UPDXX
			ON OQ.LM_FILLERX = UPDXX.DF_PRS_ID
	WHERE
		CPDXX.DF_PRS_ID IS NOT NULL
		OR UPDXX.DF_PRS_ID IS NOT NULL

	UNION

	SELECT DISTINCT
		CENDS.DF_SPE_ACC_ID AS CDW_DF_SPE_ACC_ID,
		UENDS.DF_SPE_ACC_ID AS UDW_DF_SPE_ACC_ID,
		CASE WHEN CENDS.DF_SPE_ACC_ID IS NOT NULL THEN CPDXX.DF_SPE_ACC_ID ELSE NULL END AS CDW_CoborrowerAccountNumber,
		CASE WHEN UENDS.DF_SPE_ACC_ID IS NOT NULL THEN UPDXX.DF_SPE_ACC_ID ELSE NULL END AS UDW_CoborrowerAccountNumber,
		OQ.* 
	FROM OPENQUERY(UHEAARAS, 
		''SELECT 
			CH.rowid, 
			CH.call_type, 
			CH.listid, 
			CH.appl, 
			CH.lm_fillerX, 
			CH.areacode, 
			CH.phone, 
			CH.addi_status, 
			CH.status, 
			CH.tsr, 
			CH.act_date, 
			CH.act_time, 
			RP.vox_file_name, 
			CH.time_connect,
			CH.time_acw AS TimeACW,
			CH.time_hold AS TimeHold,
			CHH.time_hold AS AgentHold,
			CH.lm_fillerX AS FillerX,
			CH.lm_fillerX AS FillerX,
			CH.lm_fillerX AS FillerX,
			CH.d_record_id
		FROM 
			call_historyXXXXXX CH
			LEFT JOIN callhistholdXXXXXX CHH
				ON CH.d_record_id = CHH.d_record_id
				AND CH.tsr = CHH.tsr
			LEFT JOIN rec_playinthXXXXXX RP 
				ON CH.d_record_id = RP.d_record_id 
				AND CH.act_date = RP.call_date 
				AND CH.appl = RP.appl
				AND CH.tsr = RP.tsr 
		WHERE 
			CH.call_type != X 
			AND CH.lm_fillerX != ''''''''
			AND CH.status IS NOT NULL
			AND CH.areacode IS NOT NULL
			AND CH.phone IS NOT NULL
	'') OQ
		LEFT JOIN CDW..PDXX_PRS_NME CPDXX
			ON OQ.LM_FILLERX = CPDXX.DF_PRS_ID
		LEFT JOIN
		(
			SELECT DISTINCT
				LNXX.LF_EDS,
				PDXX.DF_SPE_ACC_ID
			FROM
				CDW..LNXX_LON LNXX
				INNER JOIN CDW..PDXX_PRS_NME PDXX
					ON PDXX.DF_PRS_ID = LNXX.BF_SSN
				INNER JOIN CDW..LNXX_EDS LNXX
					ON LNXX.BF_SSN = LNXX.BF_SSN
					AND LNXX.LN_SEQ = LNXX.LN_SEQ
					AND LNXX.LC_STA_LONXX = ''A''
				INNER JOIN CDW..LNXX_LON_DLQ_HST LNXX
					ON LNXX.BF_SSN = LNXX.BF_SSN
					AND LNXX.LN_SEQ = LNXX.LN_SEQ
					AND LNXX.LC_STA_LONXX = ''X''
					AND CAST(LD_DLQ_MAX AS DATE) >= CAST(DATEADD(DAY,-X,GETDATE()) AS DATE)
			WHERE
				LNXX.LA_CUR_PRI > X.XX
				AND LNXX.LC_STA_LONXX = ''R''
		) CENDS
			ON CENDS.LF_EDS = CPDXX.DF_PRS_ID
		LEFT JOIN UDW..PDXX_PRS_NME UPDXX
			ON OQ.LM_FILLERX = UPDXX.DF_PRS_ID
		LEFT JOIN
		(
			SELECT DISTINCT
				LNXX.LF_EDS,
				PDXX.DF_SPE_ACC_ID
			FROM
				UDW..LNXX_LON LNXX
				INNER JOIN UDW..PDXX_PRS_NME PDXX
					ON PDXX.DF_PRS_ID = LNXX.BF_SSN
				INNER JOIN UDW..LNXX_EDS LNXX
					ON LNXX.BF_SSN = LNXX.BF_SSN
					AND LNXX.LN_SEQ = LNXX.LN_SEQ
					AND LNXX.LC_STA_LONXX = ''A''
				INNER JOIN UDW..LNXX_LON_DLQ_HST LNXX
					ON LNXX.BF_SSN = LNXX.BF_SSN
					AND LNXX.LN_SEQ = LNXX.LN_SEQ
					AND LNXX.LC_STA_LONXX = ''X''
					AND CAST(LD_DLQ_MAX AS DATE) >= CAST(DATEADD(DAY,-X,GETDATE()) AS DATE)
			WHERE
				LNXX.LA_CUR_PRI > X.XX
				AND LNXX.LC_STA_LONXX = ''R''
		) UENDS
			ON UENDS.LF_EDS = UPDXX.DF_PRS_ID
	WHERE
		CENDS.LF_EDS IS NOT NULL
		OR UENDS.LF_EDS IS NOT NULL

	UNION

	SELECT DISTINCT
		CPDXX.DF_SPE_ACC_ID AS CDW_DF_SPE_ACC_ID,
		UPDXX.DF_SPE_ACC_ID AS UDW_DF_SPE_ACC_ID,
		NULL AS CDW_CoborrowerAccountNumber,
		NULL AS UDW_CoborrowerAccountNumber,
		OQ.* 
	FROM OPENQUERY(UHEAARAS, 
		''SELECT 
			CH.rowid, 
			CH.call_type, 
			CH.listid, 
			CH.appl, 
			CH.lm_fillerX, 
			CH.areacode, 
			CH.phone, 
			CH.addi_status, 
			CH.status, 
			CH.tsr,
			CH.act_date, 
			CH.act_time, 
			RP.vox_file_name, 
			CH.time_connect,
			CH.time_acw AS TimeACW,
			CH.time_hold AS TimeHold,
			CHH.time_hold AS AgentHold,
			CH.lm_fillerX AS FillerX,
			CH.lm_fillerX AS FillerX,
			CH.lm_fillerX AS FillerX,
			CH.d_record_id
		FROM 
			call_historyXXXXXX CH 
			LEFT JOIN callhistholdXXXXXX CHH
				ON CH.d_record_id = CHH.d_record_id
				AND CH.tsr = CHH.tsr
			LEFT JOIN rec_playinthXXXXXX RP 
				ON CH.d_record_id = RP.d_record_id 
				AND CH.act_date = RP.call_date 
				AND CH.appl = RP.appl
				AND CH.tsr = RP.tsr 
		WHERE 
			CH.call_type != X 
			AND CH.lm_fillerX != ''''''''
			AND CH.status IS NOT NULL
			AND CH.areacode IS NOT NULL
			AND CH.phone IS NOT NULL
	'') OQ
		LEFT JOIN 
		(
			SELECT DISTINCT
				CPDXX.DF_PRS_ID,
				CPDXX.DF_SPE_ACC_ID
			FROM
				CDW..PDXX_PRS_NME CPDXX
				INNER JOIN CDW..LNXX_LON CLNXX
					ON CLNXX.BF_SSN = CPDXX.DF_PRS_ID
		) CPDXX
			ON OQ.LM_FILLERX = CPDXX.DF_SPE_ACC_ID
		LEFT JOIN 
		(
			SELECT DISTINCT
				UPDXX.DF_PRS_ID,
				UPDXX.DF_SPE_ACC_ID
			FROM
				UDW..PDXX_PRS_NME UPDXX
				INNER JOIN UDW..LNXX_LON ULNXX
					ON ULNXX.BF_SSN = UPDXX.DF_PRS_ID
		) UPDXX
			ON OQ.LM_FILLERX = UPDXX.DF_SPE_ACC_ID
	WHERE
		CPDXX.DF_SPE_ACC_ID IS NOT NULL
		OR UPDXX.DF_SPE_ACC_ID IS NOT NULL

	UNION

	SELECT DISTINCT
		CENDS.DF_SPE_ACC_ID AS CDW_DF_SPE_ACC_ID,
		UENDS.DF_SPE_ACC_ID AS UDW_DF_SPE_ACC_ID,
		CASE WHEN CENDS.DF_SPE_ACC_ID IS NOT NULL THEN CPDXX.DF_SPE_ACC_ID ELSE NULL END AS CDW_CoborrowerAccountNumber,
		CASE WHEN UENDS.DF_SPE_ACC_ID IS NOT NULL THEN UPDXX.DF_SPE_ACC_ID ELSE NULL END AS UDW_CoborrowerAccountNumber,
		OQ.* 
	FROM OPENQUERY(UHEAARAS, 
		''SELECT 
			CH.rowid, 
			CH.call_type, 
			CH.listid, 
			CH.appl, 
			CH.lm_fillerX, 
			CH.areacode, 
			CH.phone, 
			CH.addi_status, 
			CH.status, 
			CH.tsr, 
			CH.act_date, 
			CH.act_time, 
			RP.vox_file_name, 
			CH.time_connect,
			CH.time_acw AS TimeACW,
			CH.time_hold AS TimeHold,
			CHH.time_hold AS AgentHold,
			CH.lm_fillerX AS FillerX,
			CH.lm_fillerX AS FillerX,
			CH.lm_fillerX AS FillerX,
			CH.d_record_id
		FROM 
			call_historyXXXXXX CH
			LEFT JOIN callhistholdXXXXXX CHH
				ON CH.d_record_id = CHH.d_record_id
				AND CH.tsr = CHH.tsr
			LEFT JOIN rec_playinthXXXXXX RP 
				ON CH.d_record_id = RP.d_record_id 
				AND CH.act_date = RP.call_date 
				AND CH.appl = RP.appl
				AND CH.tsr = RP.tsr 
		WHERE 
			CH.call_type != X 
			AND CH.lm_fillerX != ''''''''
			AND CH.status IS NOT NULL
			AND CH.areacode IS NOT NULL
			AND CH.phone IS NOT NULL
	'') OQ
		LEFT JOIN CDW..PDXX_PRS_NME CPDXX
			ON OQ.LM_FILLERX = CPDXX.DF_SPE_ACC_ID
		LEFT JOIN
		(
			SELECT DISTINCT
				LNXX.LF_EDS,
				PDXX.DF_SPE_ACC_ID
			FROM
				CDW..LNXX_LON LNXX
				INNER JOIN CDW..PDXX_PRS_NME PDXX
					ON PDXX.DF_PRS_ID = LNXX.BF_SSN
				INNER JOIN CDW..LNXX_EDS LNXX
					ON LNXX.BF_SSN = LNXX.BF_SSN
					AND LNXX.LN_SEQ = LNXX.LN_SEQ
					AND LNXX.LC_STA_LONXX = ''A''
				INNER JOIN CDW..LNXX_LON_DLQ_HST LNXX
					ON LNXX.BF_SSN = LNXX.BF_SSN
					AND LNXX.LN_SEQ = LNXX.LN_SEQ
					AND LNXX.LC_STA_LONXX = ''X''
					AND CAST(LD_DLQ_MAX AS DATE) >= CAST(DATEADD(DAY,-X,GETDATE()) AS DATE)
			WHERE
				LNXX.LA_CUR_PRI > X.XX
				AND LNXX.LC_STA_LONXX = ''R''
		) CENDS
			ON CENDS.LF_EDS = CPDXX.DF_PRS_ID
		LEFT JOIN UDW..PDXX_PRS_NME UPDXX
			ON OQ.LM_FILLERX = UPDXX.DF_SPE_ACC_ID
		LEFT JOIN
		(
			SELECT DISTINCT
				LNXX.LF_EDS,
				PDXX.DF_SPE_ACC_ID
			FROM
				UDW..LNXX_LON LNXX
				INNER JOIN UDW..PDXX_PRS_NME PDXX
					ON PDXX.DF_PRS_ID = LNXX.BF_SSN
				INNER JOIN UDW..LNXX_EDS LNXX
					ON LNXX.BF_SSN = LNXX.BF_SSN
					AND LNXX.LN_SEQ = LNXX.LN_SEQ
					AND LNXX.LC_STA_LONXX = ''A''
				INNER JOIN UDW..LNXX_LON_DLQ_HST LNXX
					ON LNXX.BF_SSN = LNXX.BF_SSN
					AND LNXX.LN_SEQ = LNXX.LN_SEQ
					AND LNXX.LC_STA_LONXX = ''X''
					AND CAST(LD_DLQ_MAX AS DATE) >= CAST(DATEADD(DAY,-X,GETDATE()) AS DATE)
			WHERE
				LNXX.LA_CUR_PRI > X.XX
				AND LNXX.LC_STA_LONXX = ''R''
		) UENDS
			ON UENDS.LF_EDS = UPDXX.DF_PRS_ID
	WHERE
		CENDS.LF_EDS IS NOT NULL
		OR UENDS.LF_EDS IS NOT NULL
'
INSERT INTO @Outbound
EXEC(@QUERYXXXXXXOut)

DECLARE @QUERYXXXXXXOut AS VARCHAR(MAX) = 
'
	SELECT DISTINCT
		CPDXX.DF_SPE_ACC_ID AS CDW_DF_SPE_ACC_ID,
		UPDXX.DF_SPE_ACC_ID AS UDW_DF_SPE_ACC_ID,
		NULL AS CDW_CoborrowerAccountNumber,
		NULL AS UDW_CoborrowerAccountNumber,
		OQ.* 
	FROM OPENQUERY(UHEAARAS, 
		''SELECT 
			CH.rowid, 
			CH.call_type, 
			CH.listid, 
			CH.appl, 
			CH.lm_fillerX, 
			CH.areacode, 
			CH.phone, 
			CH.addi_status, 
			CH.status, 
			CH.tsr,
			CH.act_date, 
			CH.act_time, 
			RP.vox_file_name, 
			CH.time_connect,
			CH.time_acw AS TimeACW,
			CH.time_hold AS TimeHold,
			X AS AgentHold,
			CH.lm_fillerX AS FillerX,
			CH.lm_fillerX AS FillerX,
			CH.lm_fillerX AS FillerX,
			CH.d_record_id
		FROM 
			call_historyXXXXXX CH 
			LEFT JOIN rec_playinthXXXXXX RP 
				ON CH.d_record_id = RP.d_record_id 
				AND CH.act_date = RP.call_date 
				AND CH.appl = RP.appl
				AND CH.tsr = RP.tsr 
		WHERE 
			CH.call_type != X 
			AND CH.lm_fillerX != ''''''''
			AND CH.status IS NOT NULL
			AND CH.areacode IS NOT NULL
			AND CH.phone IS NOT NULL
	'') OQ
		LEFT JOIN 
		(
			SELECT DISTINCT
				CPDXX.DF_PRS_ID,
				CPDXX.DF_SPE_ACC_ID
			FROM
				CDW..PDXX_PRS_NME CPDXX
				INNER JOIN CDW..LNXX_LON CLNXX
					ON CLNXX.BF_SSN = CPDXX.DF_PRS_ID
		) CPDXX
			ON OQ.LM_FILLERX = CPDXX.DF_PRS_ID
		LEFT JOIN 
		(
			SELECT DISTINCT
				UPDXX.DF_PRS_ID,
				UPDXX.DF_SPE_ACC_ID
			FROM
				UDW..PDXX_PRS_NME UPDXX
				INNER JOIN UDW..LNXX_LON ULNXX
					ON ULNXX.BF_SSN = UPDXX.DF_PRS_ID
		) UPDXX
			ON OQ.LM_FILLERX = UPDXX.DF_PRS_ID
	WHERE
		CPDXX.DF_PRS_ID IS NOT NULL
		OR UPDXX.DF_PRS_ID IS NOT NULL

	UNION

	SELECT DISTINCT
		CENDS.DF_SPE_ACC_ID AS CDW_DF_SPE_ACC_ID,
		UENDS.DF_SPE_ACC_ID AS UDW_DF_SPE_ACC_ID,
		CASE WHEN CENDS.DF_SPE_ACC_ID IS NOT NULL THEN CPDXX.DF_SPE_ACC_ID ELSE NULL END AS CDW_CoborrowerAccountNumber,
		CASE WHEN UENDS.DF_SPE_ACC_ID IS NOT NULL THEN UPDXX.DF_SPE_ACC_ID ELSE NULL END AS UDW_CoborrowerAccountNumber,
		OQ.* 
	FROM OPENQUERY(UHEAARAS, 
		''SELECT 
			CH.rowid, 
			CH.call_type, 
			CH.listid, 
			CH.appl, 
			CH.lm_fillerX, 
			CH.areacode, 
			CH.phone, 
			CH.addi_status, 
			CH.status, 
			CH.tsr, 
			CH.act_date, 
			CH.act_time, 
			RP.vox_file_name, 
			CH.time_connect,
			CH.time_acw AS TimeACW,
			CH.time_hold AS TimeHold,
			X AS AgentHold,
			CH.lm_fillerX AS FillerX,
			CH.lm_fillerX AS FillerX,
			CH.lm_fillerX AS FillerX,
			CH.d_record_id
		FROM 
			call_historyXXXXXX CH
			LEFT JOIN rec_playinthXXXXXX RP 
				ON CH.d_record_id = RP.d_record_id 
				AND CH.act_date = RP.call_date 
				AND CH.appl = RP.appl
				AND CH.tsr = RP.tsr 
		WHERE 
			CH.call_type != X 
			AND CH.lm_fillerX != ''''''''
			AND CH.status IS NOT NULL
			AND CH.areacode IS NOT NULL
			AND CH.phone IS NOT NULL
	'') OQ
		LEFT JOIN CDW..PDXX_PRS_NME CPDXX
			ON OQ.LM_FILLERX = CPDXX.DF_PRS_ID
		LEFT JOIN
		(
			SELECT DISTINCT
				LNXX.LF_EDS,
				PDXX.DF_SPE_ACC_ID
			FROM
				CDW..LNXX_LON LNXX
				INNER JOIN CDW..PDXX_PRS_NME PDXX
					ON PDXX.DF_PRS_ID = LNXX.BF_SSN
				INNER JOIN CDW..LNXX_EDS LNXX
					ON LNXX.BF_SSN = LNXX.BF_SSN
					AND LNXX.LN_SEQ = LNXX.LN_SEQ
					AND LNXX.LC_STA_LONXX = ''A''
				INNER JOIN CDW..LNXX_LON_DLQ_HST LNXX
					ON LNXX.BF_SSN = LNXX.BF_SSN
					AND LNXX.LN_SEQ = LNXX.LN_SEQ
					AND LNXX.LC_STA_LONXX = ''X''
					AND CAST(LD_DLQ_MAX AS DATE) >= CAST(DATEADD(DAY,-X,GETDATE()) AS DATE)
			WHERE
				LNXX.LA_CUR_PRI > X.XX
				AND LNXX.LC_STA_LONXX = ''R''
		) CENDS
			ON CENDS.LF_EDS = CPDXX.DF_PRS_ID
		LEFT JOIN UDW..PDXX_PRS_NME UPDXX
			ON OQ.LM_FILLERX = UPDXX.DF_PRS_ID
		LEFT JOIN
		(
			SELECT DISTINCT
				LNXX.LF_EDS,
				PDXX.DF_SPE_ACC_ID
			FROM
				UDW..LNXX_LON LNXX
				INNER JOIN UDW..PDXX_PRS_NME PDXX
					ON PDXX.DF_PRS_ID = LNXX.BF_SSN
				INNER JOIN UDW..LNXX_EDS LNXX
					ON LNXX.BF_SSN = LNXX.BF_SSN
					AND LNXX.LN_SEQ = LNXX.LN_SEQ
					AND LNXX.LC_STA_LONXX = ''A''
				INNER JOIN UDW..LNXX_LON_DLQ_HST LNXX
					ON LNXX.BF_SSN = LNXX.BF_SSN
					AND LNXX.LN_SEQ = LNXX.LN_SEQ
					AND LNXX.LC_STA_LONXX = ''X''
					AND CAST(LD_DLQ_MAX AS DATE) >= CAST(DATEADD(DAY,-X,GETDATE()) AS DATE)
			WHERE
				LNXX.LA_CUR_PRI > X.XX
				AND LNXX.LC_STA_LONXX = ''R''
		) UENDS
			ON UENDS.LF_EDS = UPDXX.DF_PRS_ID
	WHERE
		CENDS.LF_EDS IS NOT NULL
		OR UENDS.LF_EDS IS NOT NULL

	UNION

	SELECT DISTINCT
		CPDXX.DF_SPE_ACC_ID AS CDW_DF_SPE_ACC_ID,
		UPDXX.DF_SPE_ACC_ID AS UDW_DF_SPE_ACC_ID,
		NULL AS CDW_CoborrowerAccountNumber,
		NULL AS UDW_CoborrowerAccountNumber,
		OQ.* 
	FROM OPENQUERY(UHEAARAS, 
		''SELECT 
			CH.rowid, 
			CH.call_type, 
			CH.listid, 
			CH.appl, 
			CH.lm_fillerX, 
			CH.areacode, 
			CH.phone, 
			CH.addi_status, 
			CH.status, 
			CH.tsr,
			CH.act_date, 
			CH.act_time, 
			RP.vox_file_name, 
			CH.time_connect,
			CH.time_acw AS TimeACW,
			CH.time_hold AS TimeHold,
			X AS AgentHold,
			CH.lm_fillerX AS FillerX,
			CH.lm_fillerX AS FillerX,
			CH.lm_fillerX AS FillerX,
			CH.d_record_id
		FROM 
			call_historyXXXXXX CH 
			LEFT JOIN rec_playinthXXXXXX RP 
				ON CH.d_record_id = RP.d_record_id 
				AND CH.act_date = RP.call_date 
				AND CH.appl = RP.appl
				AND CH.tsr = RP.tsr 
		WHERE 
			CH.call_type != X 
			AND CH.lm_fillerX != ''''''''
			AND CH.status IS NOT NULL
			AND CH.areacode IS NOT NULL
			AND CH.phone IS NOT NULL
	'') OQ
		LEFT JOIN 
		(
			SELECT DISTINCT
				CPDXX.DF_PRS_ID,
				CPDXX.DF_SPE_ACC_ID
			FROM
				CDW..PDXX_PRS_NME CPDXX
				INNER JOIN CDW..LNXX_LON CLNXX
					ON CLNXX.BF_SSN = CPDXX.DF_PRS_ID
		) CPDXX
			ON OQ.LM_FILLERX = CPDXX.DF_SPE_ACC_ID
		LEFT JOIN 
		(
			SELECT DISTINCT
				UPDXX.DF_PRS_ID,
				UPDXX.DF_SPE_ACC_ID
			FROM
				UDW..PDXX_PRS_NME UPDXX
				INNER JOIN UDW..LNXX_LON ULNXX
					ON ULNXX.BF_SSN = UPDXX.DF_PRS_ID
		) UPDXX
			ON OQ.LM_FILLERX = UPDXX.DF_SPE_ACC_ID
	WHERE
		CPDXX.DF_SPE_ACC_ID IS NOT NULL
		OR UPDXX.DF_SPE_ACC_ID IS NOT NULL

	UNION

	SELECT DISTINCT
		CENDS.DF_SPE_ACC_ID AS CDW_DF_SPE_ACC_ID,
		UENDS.DF_SPE_ACC_ID AS UDW_DF_SPE_ACC_ID,
		CASE WHEN CENDS.DF_SPE_ACC_ID IS NOT NULL THEN CPDXX.DF_SPE_ACC_ID ELSE NULL END AS CDW_CoborrowerAccountNumber,
		CASE WHEN UENDS.DF_SPE_ACC_ID IS NOT NULL THEN UPDXX.DF_SPE_ACC_ID ELSE NULL END AS UDW_CoborrowerAccountNumber,
		OQ.* 
	FROM OPENQUERY(UHEAARAS, 
		''SELECT 
			CH.rowid, 
			CH.call_type, 
			CH.listid, 
			CH.appl, 
			CH.lm_fillerX, 
			CH.areacode, 
			CH.phone, 
			CH.addi_status, 
			CH.status, 
			CH.tsr, 
			CH.act_date, 
			CH.act_time, 
			RP.vox_file_name, 
			CH.time_connect,
			CH.time_acw AS TimeACW,
			CH.time_hold AS TimeHold,
			X AS AgentHold,
			CH.lm_fillerX AS FillerX,
			CH.lm_fillerX AS FillerX,
			CH.lm_fillerX AS FillerX,
			CH.d_record_id
		FROM 
			call_historyXXXXXX CH
			LEFT JOIN rec_playinthXXXXXX RP 
				ON CH.d_record_id = RP.d_record_id 
				AND CH.act_date = RP.call_date 
				AND CH.appl = RP.appl
				AND CH.tsr = RP.tsr 
		WHERE 
			CH.call_type != X 
			AND CH.lm_fillerX != ''''''''
			AND CH.status IS NOT NULL
			AND CH.areacode IS NOT NULL
			AND CH.phone IS NOT NULL
	'') OQ
		LEFT JOIN CDW..PDXX_PRS_NME CPDXX
			ON OQ.LM_FILLERX = CPDXX.DF_SPE_ACC_ID
		LEFT JOIN
		(
			SELECT DISTINCT
				LNXX.LF_EDS,
				PDXX.DF_SPE_ACC_ID
			FROM
				CDW..LNXX_LON LNXX
				INNER JOIN CDW..PDXX_PRS_NME PDXX
					ON PDXX.DF_PRS_ID = LNXX.BF_SSN
				INNER JOIN CDW..LNXX_EDS LNXX
					ON LNXX.BF_SSN = LNXX.BF_SSN
					AND LNXX.LN_SEQ = LNXX.LN_SEQ
					AND LNXX.LC_STA_LONXX = ''A''
				INNER JOIN CDW..LNXX_LON_DLQ_HST LNXX
					ON LNXX.BF_SSN = LNXX.BF_SSN
					AND LNXX.LN_SEQ = LNXX.LN_SEQ
					AND LNXX.LC_STA_LONXX = ''X''
					AND CAST(LD_DLQ_MAX AS DATE) >= CAST(DATEADD(DAY,-X,GETDATE()) AS DATE)
			WHERE
				LNXX.LA_CUR_PRI > X.XX
				AND LNXX.LC_STA_LONXX = ''R''
		) CENDS
			ON CENDS.LF_EDS = CPDXX.DF_PRS_ID
		LEFT JOIN UDW..PDXX_PRS_NME UPDXX
			ON OQ.LM_FILLERX = UPDXX.DF_SPE_ACC_ID
		LEFT JOIN
		(
			SELECT DISTINCT
				LNXX.LF_EDS,
				PDXX.DF_SPE_ACC_ID
			FROM
				UDW..LNXX_LON LNXX
				INNER JOIN UDW..PDXX_PRS_NME PDXX
					ON PDXX.DF_PRS_ID = LNXX.BF_SSN
				INNER JOIN UDW..LNXX_EDS LNXX
					ON LNXX.BF_SSN = LNXX.BF_SSN
					AND LNXX.LN_SEQ = LNXX.LN_SEQ
					AND LNXX.LC_STA_LONXX = ''A''
				INNER JOIN UDW..LNXX_LON_DLQ_HST LNXX
					ON LNXX.BF_SSN = LNXX.BF_SSN
					AND LNXX.LN_SEQ = LNXX.LN_SEQ
					AND LNXX.LC_STA_LONXX = ''X''
					AND CAST(LD_DLQ_MAX AS DATE) >= CAST(DATEADD(DAY,-X,GETDATE()) AS DATE)
			WHERE
				LNXX.LA_CUR_PRI > X.XX
				AND LNXX.LC_STA_LONXX = ''R''
		) UENDS
			ON UENDS.LF_EDS = UPDXX.DF_PRS_ID
	WHERE
		CENDS.LF_EDS IS NOT NULL
		OR UENDS.LF_EDS IS NOT NULL
'
INSERT INTO @Outbound
EXEC(@QUERYXXXXXXOut)

DECLARE @QUERYXXXXXXOut AS VARCHAR(MAX) = 
'
	SELECT DISTINCT
		CPDXX.DF_SPE_ACC_ID AS CDW_DF_SPE_ACC_ID,
		UPDXX.DF_SPE_ACC_ID AS UDW_DF_SPE_ACC_ID,
		NULL AS CDW_CoborrowerAccountNumber,
		NULL AS UDW_CoborrowerAccountNumber,
		OQ.* 
	FROM OPENQUERY(UHEAARAS, 
		''SELECT 
			CH.rowid, 
			CH.call_type, 
			CH.listid, 
			CH.appl, 
			CH.lm_fillerX, 
			CH.areacode, 
			CH.phone, 
			CH.addi_status, 
			CH.status, 
			CH.tsr,
			CH.act_date, 
			CH.act_time, 
			RP.vox_file_name, 
			CH.time_connect,
			CH.time_acw AS TimeACW,
			CH.time_hold AS TimeHold,
			CHH.time_hold AS AgentHold,
			CH.lm_fillerX AS FillerX,
			CH.lm_fillerX AS FillerX,
			CH.lm_fillerX AS FillerX,
			CH.d_record_id
		FROM 
			call_historyXXXXXX CH 
			LEFT JOIN callhistholdXXXXXX CHH
				ON CH.d_record_id = CHH.d_record_id
				AND CH.tsr = CHH.tsr
			LEFT JOIN rec_playinthXXXXXX RP 
				ON CH.d_record_id = RP.d_record_id 
				AND CH.act_date = RP.call_date 
				AND CH.appl = RP.appl
				AND CH.tsr = RP.tsr 
		WHERE 
			CH.call_type != X 
			AND CH.lm_fillerX != ''''''''
			AND CH.status IS NOT NULL
			AND CH.areacode IS NOT NULL
			AND CH.phone IS NOT NULL
	'') OQ
		LEFT JOIN 
		(
			SELECT DISTINCT
				CPDXX.DF_PRS_ID,
				CPDXX.DF_SPE_ACC_ID
			FROM
				CDW..PDXX_PRS_NME CPDXX
				INNER JOIN CDW..LNXX_LON CLNXX
					ON CLNXX.BF_SSN = CPDXX.DF_PRS_ID
		) CPDXX
			ON OQ.LM_FILLERX = CPDXX.DF_PRS_ID
		LEFT JOIN 
		(
			SELECT DISTINCT
				UPDXX.DF_PRS_ID,
				UPDXX.DF_SPE_ACC_ID
			FROM
				UDW..PDXX_PRS_NME UPDXX
				INNER JOIN UDW..LNXX_LON ULNXX
					ON ULNXX.BF_SSN = UPDXX.DF_PRS_ID
		) UPDXX
			ON OQ.LM_FILLERX = UPDXX.DF_PRS_ID
	WHERE
		CPDXX.DF_PRS_ID IS NOT NULL
		OR UPDXX.DF_PRS_ID IS NOT NULL

	UNION

	SELECT DISTINCT
		CENDS.DF_SPE_ACC_ID AS CDW_DF_SPE_ACC_ID,
		UENDS.DF_SPE_ACC_ID AS UDW_DF_SPE_ACC_ID,
		CASE WHEN CENDS.DF_SPE_ACC_ID IS NOT NULL THEN CPDXX.DF_SPE_ACC_ID ELSE NULL END AS CDW_CoborrowerAccountNumber,
		CASE WHEN UENDS.DF_SPE_ACC_ID IS NOT NULL THEN UPDXX.DF_SPE_ACC_ID ELSE NULL END AS UDW_CoborrowerAccountNumber,
		OQ.* 
	FROM OPENQUERY(UHEAARAS, 
		''SELECT 
			CH.rowid, 
			CH.call_type, 
			CH.listid, 
			CH.appl, 
			CH.lm_fillerX, 
			CH.areacode, 
			CH.phone, 
			CH.addi_status, 
			CH.status, 
			CH.tsr, 
			CH.act_date, 
			CH.act_time, 
			RP.vox_file_name, 
			CH.time_connect,
			CH.time_acw AS TimeACW,
			CH.time_hold AS TimeHold,
			CHH.time_hold AS AgentHold,
			CH.lm_fillerX AS FillerX,
			CH.lm_fillerX AS FillerX,
			CH.lm_fillerX AS FillerX,
			CH.d_record_id
		FROM 
			call_historyXXXXXX CH
			LEFT JOIN callhistholdXXXXXX CHH
				ON CH.d_record_id = CHH.d_record_id
				AND CH.tsr = CHH.tsr
			LEFT JOIN rec_playinthXXXXXX RP 
				ON CH.d_record_id = RP.d_record_id 
				AND CH.act_date = RP.call_date 
				AND CH.appl = RP.appl
				AND CH.tsr = RP.tsr 
		WHERE 
			CH.call_type != X 
			AND CH.lm_fillerX != ''''''''
			AND CH.status IS NOT NULL
			AND CH.areacode IS NOT NULL
			AND CH.phone IS NOT NULL
	'') OQ
		LEFT JOIN CDW..PDXX_PRS_NME CPDXX
			ON OQ.LM_FILLERX = CPDXX.DF_PRS_ID
		LEFT JOIN
		(
			SELECT DISTINCT
				LNXX.LF_EDS,
				PDXX.DF_SPE_ACC_ID
			FROM
				CDW..LNXX_LON LNXX
				INNER JOIN CDW..PDXX_PRS_NME PDXX
					ON PDXX.DF_PRS_ID = LNXX.BF_SSN
				INNER JOIN CDW..LNXX_EDS LNXX
					ON LNXX.BF_SSN = LNXX.BF_SSN
					AND LNXX.LN_SEQ = LNXX.LN_SEQ
					AND LNXX.LC_STA_LONXX = ''A''
				INNER JOIN CDW..LNXX_LON_DLQ_HST LNXX
					ON LNXX.BF_SSN = LNXX.BF_SSN
					AND LNXX.LN_SEQ = LNXX.LN_SEQ
					AND LNXX.LC_STA_LONXX = ''X''
					AND CAST(LD_DLQ_MAX AS DATE) >= CAST(DATEADD(DAY,-X,GETDATE()) AS DATE)
			WHERE
				LNXX.LA_CUR_PRI > X.XX
				AND LNXX.LC_STA_LONXX = ''R''
		) CENDS
			ON CENDS.LF_EDS = CPDXX.DF_PRS_ID
		LEFT JOIN UDW..PDXX_PRS_NME UPDXX
			ON OQ.LM_FILLERX = UPDXX.DF_PRS_ID
		LEFT JOIN
		(
			SELECT DISTINCT
				LNXX.LF_EDS,
				PDXX.DF_SPE_ACC_ID
			FROM
				UDW..LNXX_LON LNXX
				INNER JOIN UDW..PDXX_PRS_NME PDXX
					ON PDXX.DF_PRS_ID = LNXX.BF_SSN
				INNER JOIN UDW..LNXX_EDS LNXX
					ON LNXX.BF_SSN = LNXX.BF_SSN
					AND LNXX.LN_SEQ = LNXX.LN_SEQ
					AND LNXX.LC_STA_LONXX = ''A''
				INNER JOIN UDW..LNXX_LON_DLQ_HST LNXX
					ON LNXX.BF_SSN = LNXX.BF_SSN
					AND LNXX.LN_SEQ = LNXX.LN_SEQ
					AND LNXX.LC_STA_LONXX = ''X''
					AND CAST(LD_DLQ_MAX AS DATE) >= CAST(DATEADD(DAY,-X,GETDATE()) AS DATE)
			WHERE
				LNXX.LA_CUR_PRI > X.XX
				AND LNXX.LC_STA_LONXX = ''R''
		) UENDS
			ON UENDS.LF_EDS = UPDXX.DF_PRS_ID
	WHERE
		CENDS.LF_EDS IS NOT NULL
		OR UENDS.LF_EDS IS NOT NULL

	UNION

	SELECT DISTINCT
		CPDXX.DF_SPE_ACC_ID AS CDW_DF_SPE_ACC_ID,
		UPDXX.DF_SPE_ACC_ID AS UDW_DF_SPE_ACC_ID,
		NULL AS CDW_CoborrowerAccountNumber,
		NULL AS UDW_CoborrowerAccountNumber,
		OQ.* 
	FROM OPENQUERY(UHEAARAS, 
		''SELECT 
			CH.rowid, 
			CH.call_type, 
			CH.listid, 
			CH.appl, 
			CH.lm_fillerX, 
			CH.areacode, 
			CH.phone, 
			CH.addi_status, 
			CH.status, 
			CH.tsr,
			CH.act_date, 
			CH.act_time, 
			RP.vox_file_name, 
			CH.time_connect,
			CH.time_acw AS TimeACW,
			CH.time_hold AS TimeHold,
			CHH.time_hold AS AgentHold,
			CH.lm_fillerX AS FillerX,
			CH.lm_fillerX AS FillerX,
			CH.lm_fillerX AS FillerX,
			CH.d_record_id
		FROM 
			call_historyXXXXXX CH 
			LEFT JOIN callhistholdXXXXXX CHH
				ON CH.d_record_id = CHH.d_record_id
				AND CH.tsr = CHH.tsr
			LEFT JOIN rec_playinthXXXXXX RP 
				ON CH.d_record_id = RP.d_record_id 
				AND CH.act_date = RP.call_date 
				AND CH.appl = RP.appl
				AND CH.tsr = RP.tsr 
		WHERE 
			CH.call_type != X 
			AND CH.lm_fillerX != ''''''''
			AND CH.status IS NOT NULL
			AND CH.areacode IS NOT NULL
			AND CH.phone IS NOT NULL
	'') OQ
		LEFT JOIN 
		(
			SELECT DISTINCT
				CPDXX.DF_PRS_ID,
				CPDXX.DF_SPE_ACC_ID
			FROM
				CDW..PDXX_PRS_NME CPDXX
				INNER JOIN CDW..LNXX_LON CLNXX
					ON CLNXX.BF_SSN = CPDXX.DF_PRS_ID
		) CPDXX
			ON OQ.LM_FILLERX = CPDXX.DF_SPE_ACC_ID
		LEFT JOIN 
		(
			SELECT DISTINCT
				UPDXX.DF_PRS_ID,
				UPDXX.DF_SPE_ACC_ID
			FROM
				UDW..PDXX_PRS_NME UPDXX
				INNER JOIN UDW..LNXX_LON ULNXX
					ON ULNXX.BF_SSN = UPDXX.DF_PRS_ID
		) UPDXX
			ON OQ.LM_FILLERX = UPDXX.DF_SPE_ACC_ID
	WHERE
		CPDXX.DF_SPE_ACC_ID IS NOT NULL
		OR UPDXX.DF_SPE_ACC_ID IS NOT NULL

	UNION

	SELECT DISTINCT
		CENDS.DF_SPE_ACC_ID AS CDW_DF_SPE_ACC_ID,
		UENDS.DF_SPE_ACC_ID AS UDW_DF_SPE_ACC_ID,
		CASE WHEN CENDS.DF_SPE_ACC_ID IS NOT NULL THEN CPDXX.DF_SPE_ACC_ID ELSE NULL END AS CDW_CoborrowerAccountNumber,
		CASE WHEN UENDS.DF_SPE_ACC_ID IS NOT NULL THEN UPDXX.DF_SPE_ACC_ID ELSE NULL END AS UDW_CoborrowerAccountNumber,
		OQ.* 
	FROM OPENQUERY(UHEAARAS, 
		''SELECT 
			CH.rowid, 
			CH.call_type, 
			CH.listid, 
			CH.appl, 
			CH.lm_fillerX, 
			CH.areacode, 
			CH.phone, 
			CH.addi_status, 
			CH.status, 
			CH.tsr, 
			CH.act_date, 
			CH.act_time, 
			RP.vox_file_name, 
			CH.time_connect,
			CH.time_acw AS TimeACW,
			CH.time_hold AS TimeHold,
			CHH.time_hold AS AgentHold,
			CH.lm_fillerX AS FillerX,
			CH.lm_fillerX AS FillerX,
			CH.lm_fillerX AS FillerX,
			CH.d_record_id
		FROM 
			call_historyXXXXXX CH
			LEFT JOIN callhistholdXXXXXX CHH
				ON CH.d_record_id = CHH.d_record_id
				AND CH.tsr = CHH.tsr
			LEFT JOIN rec_playinthXXXXXX RP 
				ON CH.d_record_id = RP.d_record_id 
				AND CH.act_date = RP.call_date 
				AND CH.appl = RP.appl
				AND CH.tsr = RP.tsr 
		WHERE 
			CH.call_type != X 
			AND CH.lm_fillerX != ''''''''
			AND CH.status IS NOT NULL
			AND CH.areacode IS NOT NULL
			AND CH.phone IS NOT NULL
	'') OQ
		LEFT JOIN CDW..PDXX_PRS_NME CPDXX
			ON OQ.LM_FILLERX = CPDXX.DF_SPE_ACC_ID
		LEFT JOIN
		(
			SELECT DISTINCT
				LNXX.LF_EDS,
				PDXX.DF_SPE_ACC_ID
			FROM
				CDW..LNXX_LON LNXX
				INNER JOIN CDW..PDXX_PRS_NME PDXX
					ON PDXX.DF_PRS_ID = LNXX.BF_SSN
				INNER JOIN CDW..LNXX_EDS LNXX
					ON LNXX.BF_SSN = LNXX.BF_SSN
					AND LNXX.LN_SEQ = LNXX.LN_SEQ
					AND LNXX.LC_STA_LONXX = ''A''
				INNER JOIN CDW..LNXX_LON_DLQ_HST LNXX
					ON LNXX.BF_SSN = LNXX.BF_SSN
					AND LNXX.LN_SEQ = LNXX.LN_SEQ
					AND LNXX.LC_STA_LONXX = ''X''
					AND CAST(LD_DLQ_MAX AS DATE) >= CAST(DATEADD(DAY,-X,GETDATE()) AS DATE)
			WHERE
				LNXX.LA_CUR_PRI > X.XX
				AND LNXX.LC_STA_LONXX = ''R''
		) CENDS
			ON CENDS.LF_EDS = CPDXX.DF_PRS_ID
		LEFT JOIN UDW..PDXX_PRS_NME UPDXX
			ON OQ.LM_FILLERX = UPDXX.DF_SPE_ACC_ID
		LEFT JOIN
		(
			SELECT DISTINCT
				LNXX.LF_EDS,
				PDXX.DF_SPE_ACC_ID
			FROM
				UDW..LNXX_LON LNXX
				INNER JOIN UDW..PDXX_PRS_NME PDXX
					ON PDXX.DF_PRS_ID = LNXX.BF_SSN
				INNER JOIN UDW..LNXX_EDS LNXX
					ON LNXX.BF_SSN = LNXX.BF_SSN
					AND LNXX.LN_SEQ = LNXX.LN_SEQ
					AND LNXX.LC_STA_LONXX = ''A''
				INNER JOIN UDW..LNXX_LON_DLQ_HST LNXX
					ON LNXX.BF_SSN = LNXX.BF_SSN
					AND LNXX.LN_SEQ = LNXX.LN_SEQ
					AND LNXX.LC_STA_LONXX = ''X''
					AND CAST(LD_DLQ_MAX AS DATE) >= CAST(DATEADD(DAY,-X,GETDATE()) AS DATE)
			WHERE
				LNXX.LA_CUR_PRI > X.XX
				AND LNXX.LC_STA_LONXX = ''R''
		) UENDS
			ON UENDS.LF_EDS = UPDXX.DF_PRS_ID
	WHERE
		CENDS.LF_EDS IS NOT NULL
		OR UENDS.LF_EDS IS NOT NULL
'
INSERT INTO @Outbound
EXEC(@QUERYXXXXXXOut)

DECLARE @QUERYXXXXXXOut AS VARCHAR(MAX) = 
'
	SELECT DISTINCT
		CPDXX.DF_SPE_ACC_ID AS CDW_DF_SPE_ACC_ID,
		UPDXX.DF_SPE_ACC_ID AS UDW_DF_SPE_ACC_ID,
		NULL AS CDW_CoborrowerAccountNumber,
		NULL AS UDW_CoborrowerAccountNumber,
		OQ.* 
	FROM OPENQUERY(UHEAARAS, 
		''SELECT 
			CH.rowid, 
			CH.call_type, 
			CH.listid, 
			CH.appl, 
			CH.lm_fillerX, 
			CH.areacode, 
			CH.phone, 
			CH.addi_status, 
			CH.status, 
			CH.tsr,
			CH.act_date, 
			CH.act_time, 
			RP.vox_file_name, 
			CH.time_connect,
			CH.time_acw AS TimeACW,
			CH.time_hold AS TimeHold,
			CHH.time_hold AS AgentHold,
			CH.lm_fillerX AS FillerX,
			CH.lm_fillerX AS FillerX,
			CH.lm_fillerX AS FillerX,
			CH.d_record_id
		FROM 
			call_historyXXXXXX CH 
			LEFT JOIN callhistholdXXXXXX CHH
				ON CH.d_record_id = CHH.d_record_id
				AND CH.tsr = CHH.tsr
			LEFT JOIN rec_playinthXXXXXX RP 
				ON CH.d_record_id = RP.d_record_id 
				AND CH.act_date = RP.call_date 
				AND CH.appl = RP.appl
				AND CH.tsr = RP.tsr 
		WHERE 
			CH.call_type != X 
			AND CH.lm_fillerX != ''''''''
			AND CH.status IS NOT NULL
			AND CH.areacode IS NOT NULL
			AND CH.phone IS NOT NULL
	'') OQ
		LEFT JOIN 
		(
			SELECT DISTINCT
				CPDXX.DF_PRS_ID,
				CPDXX.DF_SPE_ACC_ID
			FROM
				CDW..PDXX_PRS_NME CPDXX
				INNER JOIN CDW..LNXX_LON CLNXX
					ON CLNXX.BF_SSN = CPDXX.DF_PRS_ID
		) CPDXX
			ON OQ.LM_FILLERX = CPDXX.DF_PRS_ID
		LEFT JOIN 
		(
			SELECT DISTINCT
				UPDXX.DF_PRS_ID,
				UPDXX.DF_SPE_ACC_ID
			FROM
				UDW..PDXX_PRS_NME UPDXX
				INNER JOIN UDW..LNXX_LON ULNXX
					ON ULNXX.BF_SSN = UPDXX.DF_PRS_ID
		) UPDXX
			ON OQ.LM_FILLERX = UPDXX.DF_PRS_ID
	WHERE
		CPDXX.DF_PRS_ID IS NOT NULL
		OR UPDXX.DF_PRS_ID IS NOT NULL

	UNION

	SELECT DISTINCT
		CENDS.DF_SPE_ACC_ID AS CDW_DF_SPE_ACC_ID,
		UENDS.DF_SPE_ACC_ID AS UDW_DF_SPE_ACC_ID,
		CASE WHEN CENDS.DF_SPE_ACC_ID IS NOT NULL THEN CPDXX.DF_SPE_ACC_ID ELSE NULL END AS CDW_CoborrowerAccountNumber,
		CASE WHEN UENDS.DF_SPE_ACC_ID IS NOT NULL THEN UPDXX.DF_SPE_ACC_ID ELSE NULL END AS UDW_CoborrowerAccountNumber,
		OQ.* 
	FROM OPENQUERY(UHEAARAS, 
		''SELECT 
			CH.rowid, 
			CH.call_type, 
			CH.listid, 
			CH.appl, 
			CH.lm_fillerX, 
			CH.areacode, 
			CH.phone, 
			CH.addi_status, 
			CH.status, 
			CH.tsr, 
			CH.act_date, 
			CH.act_time, 
			RP.vox_file_name, 
			CH.time_connect,
			CH.time_acw AS TimeACW,
			CH.time_hold AS TimeHold,
			CHH.time_hold AS AgentHold,
			CH.lm_fillerX AS FillerX,
			CH.lm_fillerX AS FillerX,
			CH.lm_fillerX AS FillerX,
			CH.d_record_id
		FROM 
			call_historyXXXXXX CH
			LEFT JOIN callhistholdXXXXXX CHH
				ON CH.d_record_id = CHH.d_record_id
				AND CH.tsr = CHH.tsr
			LEFT JOIN rec_playinthXXXXXX RP 
				ON CH.d_record_id = RP.d_record_id 
				AND CH.act_date = RP.call_date 
				AND CH.appl = RP.appl
				AND CH.tsr = RP.tsr 
		WHERE 
			CH.call_type != X 
			AND CH.lm_fillerX != ''''''''
			AND CH.status IS NOT NULL
			AND CH.areacode IS NOT NULL
			AND CH.phone IS NOT NULL
	'') OQ
		LEFT JOIN CDW..PDXX_PRS_NME CPDXX
			ON OQ.LM_FILLERX = CPDXX.DF_PRS_ID
		LEFT JOIN
		(
			SELECT DISTINCT
				LNXX.LF_EDS,
				PDXX.DF_SPE_ACC_ID
			FROM
				CDW..LNXX_LON LNXX
				INNER JOIN CDW..PDXX_PRS_NME PDXX
					ON PDXX.DF_PRS_ID = LNXX.BF_SSN
				INNER JOIN CDW..LNXX_EDS LNXX
					ON LNXX.BF_SSN = LNXX.BF_SSN
					AND LNXX.LN_SEQ = LNXX.LN_SEQ
					AND LNXX.LC_STA_LONXX = ''A''
				INNER JOIN CDW..LNXX_LON_DLQ_HST LNXX
					ON LNXX.BF_SSN = LNXX.BF_SSN
					AND LNXX.LN_SEQ = LNXX.LN_SEQ
					AND LNXX.LC_STA_LONXX = ''X''
					AND CAST(LD_DLQ_MAX AS DATE) >= CAST(DATEADD(DAY,-X,GETDATE()) AS DATE)
			WHERE
				LNXX.LA_CUR_PRI > X.XX
				AND LNXX.LC_STA_LONXX = ''R''
		) CENDS
			ON CENDS.LF_EDS = CPDXX.DF_PRS_ID
		LEFT JOIN UDW..PDXX_PRS_NME UPDXX
			ON OQ.LM_FILLERX = UPDXX.DF_PRS_ID
		LEFT JOIN
		(
			SELECT DISTINCT
				LNXX.LF_EDS,
				PDXX.DF_SPE_ACC_ID
			FROM
				UDW..LNXX_LON LNXX
				INNER JOIN UDW..PDXX_PRS_NME PDXX
					ON PDXX.DF_PRS_ID = LNXX.BF_SSN
				INNER JOIN UDW..LNXX_EDS LNXX
					ON LNXX.BF_SSN = LNXX.BF_SSN
					AND LNXX.LN_SEQ = LNXX.LN_SEQ
					AND LNXX.LC_STA_LONXX = ''A''
				INNER JOIN UDW..LNXX_LON_DLQ_HST LNXX
					ON LNXX.BF_SSN = LNXX.BF_SSN
					AND LNXX.LN_SEQ = LNXX.LN_SEQ
					AND LNXX.LC_STA_LONXX = ''X''
					AND CAST(LD_DLQ_MAX AS DATE) >= CAST(DATEADD(DAY,-X,GETDATE()) AS DATE)
			WHERE
				LNXX.LA_CUR_PRI > X.XX
				AND LNXX.LC_STA_LONXX = ''R''
		) UENDS
			ON UENDS.LF_EDS = UPDXX.DF_PRS_ID
	WHERE
		CENDS.LF_EDS IS NOT NULL
		OR UENDS.LF_EDS IS NOT NULL

	UNION

	SELECT DISTINCT
		CPDXX.DF_SPE_ACC_ID AS CDW_DF_SPE_ACC_ID,
		UPDXX.DF_SPE_ACC_ID AS UDW_DF_SPE_ACC_ID,
		NULL AS CDW_CoborrowerAccountNumber,
		NULL AS UDW_CoborrowerAccountNumber,
		OQ.* 
	FROM OPENQUERY(UHEAARAS, 
		''SELECT 
			CH.rowid, 
			CH.call_type, 
			CH.listid, 
			CH.appl, 
			CH.lm_fillerX, 
			CH.areacode, 
			CH.phone, 
			CH.addi_status, 
			CH.status, 
			CH.tsr,
			CH.act_date, 
			CH.act_time, 
			RP.vox_file_name, 
			CH.time_connect,
			CH.time_acw AS TimeACW,
			CH.time_hold AS TimeHold,
			CHH.time_hold AS AgentHold,
			CH.lm_fillerX AS FillerX,
			CH.lm_fillerX AS FillerX,
			CH.lm_fillerX AS FillerX,
			CH.d_record_id
		FROM 
			call_historyXXXXXX CH 
			LEFT JOIN callhistholdXXXXXX CHH
				ON CH.d_record_id = CHH.d_record_id
				AND CH.tsr = CHH.tsr
			LEFT JOIN rec_playinthXXXXXX RP 
				ON CH.d_record_id = RP.d_record_id 
				AND CH.act_date = RP.call_date 
				AND CH.appl = RP.appl
				AND CH.tsr = RP.tsr 
		WHERE 
			CH.call_type != X 
			AND CH.lm_fillerX != ''''''''
			AND CH.status IS NOT NULL
			AND CH.areacode IS NOT NULL
			AND CH.phone IS NOT NULL
	'') OQ
		LEFT JOIN 
		(
			SELECT DISTINCT
				CPDXX.DF_PRS_ID,
				CPDXX.DF_SPE_ACC_ID
			FROM
				CDW..PDXX_PRS_NME CPDXX
				INNER JOIN CDW..LNXX_LON CLNXX
					ON CLNXX.BF_SSN = CPDXX.DF_PRS_ID
		) CPDXX
			ON OQ.LM_FILLERX = CPDXX.DF_SPE_ACC_ID
		LEFT JOIN 
		(
			SELECT DISTINCT
				UPDXX.DF_PRS_ID,
				UPDXX.DF_SPE_ACC_ID
			FROM
				UDW..PDXX_PRS_NME UPDXX
				INNER JOIN UDW..LNXX_LON ULNXX
					ON ULNXX.BF_SSN = UPDXX.DF_PRS_ID
		) UPDXX
			ON OQ.LM_FILLERX = UPDXX.DF_SPE_ACC_ID
	WHERE
		CPDXX.DF_SPE_ACC_ID IS NOT NULL
		OR UPDXX.DF_SPE_ACC_ID IS NOT NULL

	UNION

	SELECT DISTINCT
		CENDS.DF_SPE_ACC_ID AS CDW_DF_SPE_ACC_ID,
		UENDS.DF_SPE_ACC_ID AS UDW_DF_SPE_ACC_ID,
		CASE WHEN CENDS.DF_SPE_ACC_ID IS NOT NULL THEN CPDXX.DF_SPE_ACC_ID ELSE NULL END AS CDW_CoborrowerAccountNumber,
		CASE WHEN UENDS.DF_SPE_ACC_ID IS NOT NULL THEN UPDXX.DF_SPE_ACC_ID ELSE NULL END AS UDW_CoborrowerAccountNumber,
		OQ.* 
	FROM OPENQUERY(UHEAARAS, 
		''SELECT 
			CH.rowid, 
			CH.call_type, 
			CH.listid, 
			CH.appl, 
			CH.lm_fillerX, 
			CH.areacode, 
			CH.phone, 
			CH.addi_status, 
			CH.status, 
			CH.tsr, 
			CH.act_date, 
			CH.act_time, 
			RP.vox_file_name, 
			CH.time_connect,
			CH.time_acw AS TimeACW,
			CH.time_hold AS TimeHold,
			CHH.time_hold AS AgentHold,
			CH.lm_fillerX AS FillerX,
			CH.lm_fillerX AS FillerX,
			CH.lm_fillerX AS FillerX,
			CH.d_record_id
		FROM 
			call_historyXXXXXX CH
			LEFT JOIN callhistholdXXXXXX CHH
				ON CH.d_record_id = CHH.d_record_id
				AND CH.tsr = CHH.tsr
			LEFT JOIN rec_playinthXXXXXX RP 
				ON CH.d_record_id = RP.d_record_id 
				AND CH.act_date = RP.call_date 
				AND CH.appl = RP.appl
				AND CH.tsr = RP.tsr 
		WHERE 
			CH.call_type != X 
			AND CH.lm_fillerX != ''''''''
			AND CH.status IS NOT NULL
			AND CH.areacode IS NOT NULL
			AND CH.phone IS NOT NULL
	'') OQ
		LEFT JOIN CDW..PDXX_PRS_NME CPDXX
			ON OQ.LM_FILLERX = CPDXX.DF_SPE_ACC_ID
		LEFT JOIN
		(
			SELECT DISTINCT
				LNXX.LF_EDS,
				PDXX.DF_SPE_ACC_ID
			FROM
				CDW..LNXX_LON LNXX
				INNER JOIN CDW..PDXX_PRS_NME PDXX
					ON PDXX.DF_PRS_ID = LNXX.BF_SSN
				INNER JOIN CDW..LNXX_EDS LNXX
					ON LNXX.BF_SSN = LNXX.BF_SSN
					AND LNXX.LN_SEQ = LNXX.LN_SEQ
					AND LNXX.LC_STA_LONXX = ''A''
				INNER JOIN CDW..LNXX_LON_DLQ_HST LNXX
					ON LNXX.BF_SSN = LNXX.BF_SSN
					AND LNXX.LN_SEQ = LNXX.LN_SEQ
					AND LNXX.LC_STA_LONXX = ''X''
					AND CAST(LD_DLQ_MAX AS DATE) >= CAST(DATEADD(DAY,-X,GETDATE()) AS DATE)
			WHERE
				LNXX.LA_CUR_PRI > X.XX
				AND LNXX.LC_STA_LONXX = ''R''
		) CENDS
			ON CENDS.LF_EDS = CPDXX.DF_PRS_ID
		LEFT JOIN UDW..PDXX_PRS_NME UPDXX
			ON OQ.LM_FILLERX = UPDXX.DF_SPE_ACC_ID
		LEFT JOIN
		(
			SELECT DISTINCT
				LNXX.LF_EDS,
				PDXX.DF_SPE_ACC_ID
			FROM
				UDW..LNXX_LON LNXX
				INNER JOIN UDW..PDXX_PRS_NME PDXX
					ON PDXX.DF_PRS_ID = LNXX.BF_SSN
				INNER JOIN UDW..LNXX_EDS LNXX
					ON LNXX.BF_SSN = LNXX.BF_SSN
					AND LNXX.LN_SEQ = LNXX.LN_SEQ
					AND LNXX.LC_STA_LONXX = ''A''
				INNER JOIN UDW..LNXX_LON_DLQ_HST LNXX
					ON LNXX.BF_SSN = LNXX.BF_SSN
					AND LNXX.LN_SEQ = LNXX.LN_SEQ
					AND LNXX.LC_STA_LONXX = ''X''
					AND CAST(LD_DLQ_MAX AS DATE) >= CAST(DATEADD(DAY,-X,GETDATE()) AS DATE)
			WHERE
				LNXX.LA_CUR_PRI > X.XX
				AND LNXX.LC_STA_LONXX = ''R''
		) UENDS
			ON UENDS.LF_EDS = UPDXX.DF_PRS_ID
	WHERE
		CENDS.LF_EDS IS NOT NULL
		OR UENDS.LF_EDS IS NOT NULL
'
INSERT INTO @Outbound
EXEC(@QUERYXXXXXXOut)

DECLARE @QUERYXXXXXXOut AS VARCHAR(MAX) = 
'
	SELECT DISTINCT
		CPDXX.DF_SPE_ACC_ID AS CDW_DF_SPE_ACC_ID,
		UPDXX.DF_SPE_ACC_ID AS UDW_DF_SPE_ACC_ID,
		NULL AS CDW_CoborrowerAccountNumber,
		NULL AS UDW_CoborrowerAccountNumber,
		OQ.* 
	FROM OPENQUERY(UHEAARAS, 
		''SELECT 
			CH.rowid, 
			CH.call_type, 
			CH.listid, 
			CH.appl, 
			CH.lm_fillerX, 
			CH.areacode, 
			CH.phone, 
			CH.addi_status, 
			CH.status, 
			CH.tsr,
			CH.act_date, 
			CH.act_time, 
			RP.vox_file_name, 
			CH.time_connect,
			CH.time_acw AS TimeACW,
			CH.time_hold AS TimeHold,
			CHH.time_hold AS AgentHold,
			CH.lm_fillerX AS FillerX,
			CH.lm_fillerX AS FillerX,
			CH.lm_fillerX AS FillerX,
			CH.d_record_id
		FROM 
			call_historyXXXXXX CH 
			LEFT JOIN callhistholdXXXXXX CHH
				ON CH.d_record_id = CHH.d_record_id
				AND CH.tsr = CHH.tsr
			LEFT JOIN rec_playinthXXXXXX RP 
				ON CH.d_record_id = RP.d_record_id 
				AND CH.act_date = RP.call_date 
				AND CH.appl = RP.appl
				AND CH.tsr = RP.tsr 
		WHERE 
			CH.call_type != X 
			AND CH.lm_fillerX != ''''''''
			AND CH.status IS NOT NULL
			AND CH.areacode IS NOT NULL
			AND CH.phone IS NOT NULL
	'') OQ
		LEFT JOIN 
		(
			SELECT DISTINCT
				CPDXX.DF_PRS_ID,
				CPDXX.DF_SPE_ACC_ID
			FROM
				CDW..PDXX_PRS_NME CPDXX
				INNER JOIN CDW..LNXX_LON CLNXX
					ON CLNXX.BF_SSN = CPDXX.DF_PRS_ID
		) CPDXX
			ON OQ.LM_FILLERX = CPDXX.DF_PRS_ID
		LEFT JOIN 
		(
			SELECT DISTINCT
				UPDXX.DF_PRS_ID,
				UPDXX.DF_SPE_ACC_ID
			FROM
				UDW..PDXX_PRS_NME UPDXX
				INNER JOIN UDW..LNXX_LON ULNXX
					ON ULNXX.BF_SSN = UPDXX.DF_PRS_ID
		) UPDXX
			ON OQ.LM_FILLERX = UPDXX.DF_PRS_ID
	WHERE
		CPDXX.DF_PRS_ID IS NOT NULL
		OR UPDXX.DF_PRS_ID IS NOT NULL

	UNION

	SELECT DISTINCT
		CENDS.DF_SPE_ACC_ID AS CDW_DF_SPE_ACC_ID,
		UENDS.DF_SPE_ACC_ID AS UDW_DF_SPE_ACC_ID,
		CASE WHEN CENDS.DF_SPE_ACC_ID IS NOT NULL THEN CPDXX.DF_SPE_ACC_ID ELSE NULL END AS CDW_CoborrowerAccountNumber,
		CASE WHEN UENDS.DF_SPE_ACC_ID IS NOT NULL THEN UPDXX.DF_SPE_ACC_ID ELSE NULL END AS UDW_CoborrowerAccountNumber,
		OQ.* 
	FROM OPENQUERY(UHEAARAS, 
		''SELECT 
			CH.rowid, 
			CH.call_type, 
			CH.listid, 
			CH.appl, 
			CH.lm_fillerX, 
			CH.areacode, 
			CH.phone, 
			CH.addi_status, 
			CH.status, 
			CH.tsr, 
			CH.act_date, 
			CH.act_time, 
			RP.vox_file_name, 
			CH.time_connect,
			CH.time_acw AS TimeACW,
			CH.time_hold AS TimeHold,
			CHH.time_hold AS AgentHold,
			CH.lm_fillerX AS FillerX,
			CH.lm_fillerX AS FillerX,
			CH.lm_fillerX AS FillerX,
			CH.d_record_id
		FROM 
			call_historyXXXXXX CH
			LEFT JOIN callhistholdXXXXXX CHH
				ON CH.d_record_id = CHH.d_record_id
				AND CH.tsr = CHH.tsr
			LEFT JOIN rec_playinthXXXXXX RP 
				ON CH.d_record_id = RP.d_record_id 
				AND CH.act_date = RP.call_date 
				AND CH.appl = RP.appl
				AND CH.tsr = RP.tsr 
		WHERE 
			CH.call_type != X 
			AND CH.lm_fillerX != ''''''''
			AND CH.status IS NOT NULL
			AND CH.areacode IS NOT NULL
			AND CH.phone IS NOT NULL
	'') OQ
		LEFT JOIN CDW..PDXX_PRS_NME CPDXX
			ON OQ.LM_FILLERX = CPDXX.DF_PRS_ID
		LEFT JOIN
		(
			SELECT DISTINCT
				LNXX.LF_EDS,
				PDXX.DF_SPE_ACC_ID
			FROM
				CDW..LNXX_LON LNXX
				INNER JOIN CDW..PDXX_PRS_NME PDXX
					ON PDXX.DF_PRS_ID = LNXX.BF_SSN
				INNER JOIN CDW..LNXX_EDS LNXX
					ON LNXX.BF_SSN = LNXX.BF_SSN
					AND LNXX.LN_SEQ = LNXX.LN_SEQ
					AND LNXX.LC_STA_LONXX = ''A''
				INNER JOIN CDW..LNXX_LON_DLQ_HST LNXX
					ON LNXX.BF_SSN = LNXX.BF_SSN
					AND LNXX.LN_SEQ = LNXX.LN_SEQ
					AND LNXX.LC_STA_LONXX = ''X''
					AND CAST(LD_DLQ_MAX AS DATE) >= CAST(DATEADD(DAY,-X,GETDATE()) AS DATE)
			WHERE
				LNXX.LA_CUR_PRI > X.XX
				AND LNXX.LC_STA_LONXX = ''R''
		) CENDS
			ON CENDS.LF_EDS = CPDXX.DF_PRS_ID
		LEFT JOIN UDW..PDXX_PRS_NME UPDXX
			ON OQ.LM_FILLERX = UPDXX.DF_PRS_ID
		LEFT JOIN
		(
			SELECT DISTINCT
				LNXX.LF_EDS,
				PDXX.DF_SPE_ACC_ID
			FROM
				UDW..LNXX_LON LNXX
				INNER JOIN UDW..PDXX_PRS_NME PDXX
					ON PDXX.DF_PRS_ID = LNXX.BF_SSN
				INNER JOIN UDW..LNXX_EDS LNXX
					ON LNXX.BF_SSN = LNXX.BF_SSN
					AND LNXX.LN_SEQ = LNXX.LN_SEQ
					AND LNXX.LC_STA_LONXX = ''A''
				INNER JOIN UDW..LNXX_LON_DLQ_HST LNXX
					ON LNXX.BF_SSN = LNXX.BF_SSN
					AND LNXX.LN_SEQ = LNXX.LN_SEQ
					AND LNXX.LC_STA_LONXX = ''X''
					AND CAST(LD_DLQ_MAX AS DATE) >= CAST(DATEADD(DAY,-X,GETDATE()) AS DATE)
			WHERE
				LNXX.LA_CUR_PRI > X.XX
				AND LNXX.LC_STA_LONXX = ''R''
		) UENDS
			ON UENDS.LF_EDS = UPDXX.DF_PRS_ID
	WHERE
		CENDS.LF_EDS IS NOT NULL
		OR UENDS.LF_EDS IS NOT NULL

	UNION

	SELECT DISTINCT
		CPDXX.DF_SPE_ACC_ID AS CDW_DF_SPE_ACC_ID,
		UPDXX.DF_SPE_ACC_ID AS UDW_DF_SPE_ACC_ID,
		NULL AS CDW_CoborrowerAccountNumber,
		NULL AS UDW_CoborrowerAccountNumber,
		OQ.* 
	FROM OPENQUERY(UHEAARAS, 
		''SELECT 
			CH.rowid, 
			CH.call_type, 
			CH.listid, 
			CH.appl, 
			CH.lm_fillerX, 
			CH.areacode, 
			CH.phone, 
			CH.addi_status, 
			CH.status, 
			CH.tsr,
			CH.act_date, 
			CH.act_time, 
			RP.vox_file_name, 
			CH.time_connect,
			CH.time_acw AS TimeACW,
			CH.time_hold AS TimeHold,
			CHH.time_hold AS AgentHold,
			CH.lm_fillerX AS FillerX,
			CH.lm_fillerX AS FillerX,
			CH.lm_fillerX AS FillerX,
			CH.d_record_id
		FROM 
			call_historyXXXXXX CH 
			LEFT JOIN callhistholdXXXXXX CHH
				ON CH.d_record_id = CHH.d_record_id
				AND CH.tsr = CHH.tsr
			LEFT JOIN rec_playinthXXXXXX RP 
				ON CH.d_record_id = RP.d_record_id 
				AND CH.act_date = RP.call_date 
				AND CH.appl = RP.appl
				AND CH.tsr = RP.tsr 
		WHERE 
			CH.call_type != X 
			AND CH.lm_fillerX != ''''''''
			AND CH.status IS NOT NULL
			AND CH.areacode IS NOT NULL
			AND CH.phone IS NOT NULL
	'') OQ
		LEFT JOIN 
		(
			SELECT DISTINCT
				CPDXX.DF_PRS_ID,
				CPDXX.DF_SPE_ACC_ID
			FROM
				CDW..PDXX_PRS_NME CPDXX
				INNER JOIN CDW..LNXX_LON CLNXX
					ON CLNXX.BF_SSN = CPDXX.DF_PRS_ID
		) CPDXX
			ON OQ.LM_FILLERX = CPDXX.DF_SPE_ACC_ID
		LEFT JOIN 
		(
			SELECT DISTINCT
				UPDXX.DF_PRS_ID,
				UPDXX.DF_SPE_ACC_ID
			FROM
				UDW..PDXX_PRS_NME UPDXX
				INNER JOIN UDW..LNXX_LON ULNXX
					ON ULNXX.BF_SSN = UPDXX.DF_PRS_ID
		) UPDXX
			ON OQ.LM_FILLERX = UPDXX.DF_SPE_ACC_ID
	WHERE
		CPDXX.DF_SPE_ACC_ID IS NOT NULL
		OR UPDXX.DF_SPE_ACC_ID IS NOT NULL

	UNION

	SELECT DISTINCT
		CENDS.DF_SPE_ACC_ID AS CDW_DF_SPE_ACC_ID,
		UENDS.DF_SPE_ACC_ID AS UDW_DF_SPE_ACC_ID,
		CASE WHEN CENDS.DF_SPE_ACC_ID IS NOT NULL THEN CPDXX.DF_SPE_ACC_ID ELSE NULL END AS CDW_CoborrowerAccountNumber,
		CASE WHEN UENDS.DF_SPE_ACC_ID IS NOT NULL THEN UPDXX.DF_SPE_ACC_ID ELSE NULL END AS UDW_CoborrowerAccountNumber,
		OQ.* 
	FROM OPENQUERY(UHEAARAS, 
		''SELECT 
			CH.rowid, 
			CH.call_type, 
			CH.listid, 
			CH.appl, 
			CH.lm_fillerX, 
			CH.areacode, 
			CH.phone, 
			CH.addi_status, 
			CH.status, 
			CH.tsr, 
			CH.act_date, 
			CH.act_time, 
			RP.vox_file_name, 
			CH.time_connect,
			CH.time_acw AS TimeACW,
			CH.time_hold AS TimeHold,
			CHH.time_hold AS AgentHold,
			CH.lm_fillerX AS FillerX,
			CH.lm_fillerX AS FillerX,
			CH.lm_fillerX AS FillerX,
			CH.d_record_id
		FROM 
			call_historyXXXXXX CH
			LEFT JOIN callhistholdXXXXXX CHH
				ON CH.d_record_id = CHH.d_record_id
				AND CH.tsr = CHH.tsr
			LEFT JOIN rec_playinthXXXXXX RP 
				ON CH.d_record_id = RP.d_record_id 
				AND CH.act_date = RP.call_date 
				AND CH.appl = RP.appl
				AND CH.tsr = RP.tsr 
		WHERE 
			CH.call_type != X 
			AND CH.lm_fillerX != ''''''''
			AND CH.status IS NOT NULL
			AND CH.areacode IS NOT NULL
			AND CH.phone IS NOT NULL
	'') OQ
		LEFT JOIN CDW..PDXX_PRS_NME CPDXX
			ON OQ.LM_FILLERX = CPDXX.DF_SPE_ACC_ID
		LEFT JOIN
		(
			SELECT DISTINCT
				LNXX.LF_EDS,
				PDXX.DF_SPE_ACC_ID
			FROM
				CDW..LNXX_LON LNXX
				INNER JOIN CDW..PDXX_PRS_NME PDXX
					ON PDXX.DF_PRS_ID = LNXX.BF_SSN
				INNER JOIN CDW..LNXX_EDS LNXX
					ON LNXX.BF_SSN = LNXX.BF_SSN
					AND LNXX.LN_SEQ = LNXX.LN_SEQ
					AND LNXX.LC_STA_LONXX = ''A''
				INNER JOIN CDW..LNXX_LON_DLQ_HST LNXX
					ON LNXX.BF_SSN = LNXX.BF_SSN
					AND LNXX.LN_SEQ = LNXX.LN_SEQ
					AND LNXX.LC_STA_LONXX = ''X''
					AND CAST(LD_DLQ_MAX AS DATE) >= CAST(DATEADD(DAY,-X,GETDATE()) AS DATE)
			WHERE
				LNXX.LA_CUR_PRI > X.XX
				AND LNXX.LC_STA_LONXX = ''R''
		) CENDS
			ON CENDS.LF_EDS = CPDXX.DF_PRS_ID
		LEFT JOIN UDW..PDXX_PRS_NME UPDXX
			ON OQ.LM_FILLERX = UPDXX.DF_SPE_ACC_ID
		LEFT JOIN
		(
			SELECT DISTINCT
				LNXX.LF_EDS,
				PDXX.DF_SPE_ACC_ID
			FROM
				UDW..LNXX_LON LNXX
				INNER JOIN UDW..PDXX_PRS_NME PDXX
					ON PDXX.DF_PRS_ID = LNXX.BF_SSN
				INNER JOIN UDW..LNXX_EDS LNXX
					ON LNXX.BF_SSN = LNXX.BF_SSN
					AND LNXX.LN_SEQ = LNXX.LN_SEQ
					AND LNXX.LC_STA_LONXX = ''A''
				INNER JOIN UDW..LNXX_LON_DLQ_HST LNXX
					ON LNXX.BF_SSN = LNXX.BF_SSN
					AND LNXX.LN_SEQ = LNXX.LN_SEQ
					AND LNXX.LC_STA_LONXX = ''X''
					AND CAST(LD_DLQ_MAX AS DATE) >= CAST(DATEADD(DAY,-X,GETDATE()) AS DATE)
			WHERE
				LNXX.LA_CUR_PRI > X.XX
				AND LNXX.LC_STA_LONXX = ''R''
		) UENDS
			ON UENDS.LF_EDS = UPDXX.DF_PRS_ID
	WHERE
		CENDS.LF_EDS IS NOT NULL
		OR UENDS.LF_EDS IS NOT NULL
'
INSERT INTO @Outbound
EXEC(@QUERYXXXXXXOut)

UPDATE 
	@Inbound 
SET 
	fillerX = LTRIM(RTRIM(fillerX)),
	ani_code = LTRIM(RTRIM(ani_code)),
	ani_phone = LTRIM(RTRIM(ani_phone)),
	vox_file_name = LTRIM(RTRIM(ISNULL(vox_file_name,''))),
	addi_status = ISNULL(addi_status,'')

UPDATE
	@Outbound
SET
	lm_fillerX = LTRIM(RTRIM(lm_fillerX)),
	CDW_CoborrowerAccountNumber = LTRIM(RTRIM(CDW_CoborrowerAccountNumber)),
	UDW_CoborrowerAccountNumber = LTRIM(RTRIM(UDW_CoborrowerAccountNumber)),
	tsr = ISNULL(tsr,''),
	areacode = LTRIM(RTRIM(areacode)),
	phone = LTRIM(RTRIM(phone)),
	vox_file_name = LTRIM(RTRIM(ISNULL(vox_file_name,''))),
	addi_status = ISNULL(addi_status,'')

INSERT INTO NobleCalls..NobleCallHistory(NobleRowId, AccountIdentifier, CallType, ListId, CallCampaign, ActivityDate, PhoneNumber, AgentId, DispositionCode, AdditionalDispositionCode, RegionId, VoxFileId, IsInbound, CallLength, TimeACW, TimeHold, AgentHold, FillerX, FillerX, FillerX, d_record_id)
SELECT
	D.rowid, 
	CASE
		WHEN ISNULL(D.fillerX, '') = '' AND CC.RegionId IN (X,X) THEN UPDXX.DF_SPE_ACC_ID -- Uheaa and OneLink 
		WHEN ISNULL(D.fillerX, '') = '' AND CC.RegionId = X THEN CPDXX.DF_SPE_ACC_ID -- CornerStone
	ELSE D.fillerX END [AccountIdentifier], 
	D.call_type,
	D.listid,
	D.appl,
	D.call_date, 
	D.ani_code + D.ani_phone, 
	D.tsr,
	D.[status],
	D.addi_status,
	CC.RegionId,
	D.vox_file_name,
	X,
	D.time_connect,
	D.TimeACW,
	D.TimeHold,
	D.AgentHold,
	D.FillerX,
	D.FillerX,
	D.FillerX,
	D.d_record_id
FROM
	@Inbound D
	LEFT JOIN NobleCalls..NobleCallHistory NCH
		ON D.rowid = NCH.NobleRowId 
		AND	D.call_type = NCH.CallType
		AND	D.appl = NCH.CallCampaign
		AND	D.call_date = NCH.ActivityDate
		AND	D.ani_code + D.ani_phone = NCH.PhoneNumber
		AND NCH.CreatedAt > CAST(DATEADD(DAY,-X,GETDATE()) AS DATE)
	LEFT JOIN NobleCalls..CallCampaigns CC
		ON CC.CallCampaign = D.appl
	LEFT JOIN 
	(
		SELECT DISTINCT
			PDXX.DF_SPE_ACC_ID,
			PDXX.DN_DOM_PHN_ARA + PDXX.DN_DOM_PHN_XCH + PDXX.DN_DOM_PHN_LCL [PhoneNumber],
			ROW_NUMBER() OVER (PARTITION BY PDXX.DN_DOM_PHN_ARA + PDXX.DN_DOM_PHN_XCH + PDXX.DN_DOM_PHN_LCL ORDER BY PDXX.DD_PHN_VER DESC, PDXX.DI_PHN_VLD DESC) [VerifyOrder]
		FROM 
			CDW..PDXX_PRS_PHN PDXX
			INNER JOIN CDW..PDXX_PRS_NME PDXX
				ON PDXX.DF_PRS_ID = PDXX.DF_PRS_ID
		WHERE
			PDXX.DN_DOM_PHN_ARA + PDXX.DN_DOM_PHN_XCH + PDXX.DN_DOM_PHN_LCL != ''
	) CPDXX ON CPDXX.PhoneNumber = (D.ani_code + D.ani_phone) AND CPDXX.VerifyOrder = X
	LEFT JOIN 
	( 
		SELECT DISTINCT
			PDXX.DF_SPE_ACC_ID,
			PDXX.DN_DOM_PHN_ARA + PDXX.DN_DOM_PHN_XCH + PDXX.DN_DOM_PHN_LCL [PhoneNumber],
			ROW_NUMBER() OVER (PARTITION BY PDXX.DN_DOM_PHN_ARA + PDXX.DN_DOM_PHN_XCH + PDXX.DN_DOM_PHN_LCL ORDER BY PDXX.DD_PHN_VER DESC, PDXX.DI_PHN_VLD DESC) [VerifyOrder]
		FROM 
			UDW..PDXX_PRS_PHN PDXX
			INNER JOIN UDW..PDXX_PRS_NME PDXX
				ON PDXX.DF_PRS_ID = PDXX.DF_PRS_ID
		WHERE
			PDXX.DN_DOM_PHN_ARA + PDXX.DN_DOM_PHN_XCH + PDXX.DN_DOM_PHN_LCL != ''
	) UPDXX ON UPDXX.PhoneNumber = (D.ani_code + D.ani_phone) AND UPDXX.VerifyOrder = X
			
WHERE
	NCH.NobleRowId IS NULL;


INSERT INTO NobleCalls..NobleCallHistory(NobleRowId, AccountIdentifier, CallType, ListId, CallCampaign, ActivityDate, PhoneNumber, AgentId, DispositionCode, AdditionalDispositionCode, RegionId, VoxFileId, IsInbound, CallLength, Deleted, DeletedAt, DeletedBy, TimeACW, TimeHold, AgentHold, FillerX, FillerX, FillerX, d_record_id, CoborrowerAccountNumber)
SELECT
	D.rowid, 
	CASE
		WHEN CC.RegionId IN (X,X) AND ISNULL(D.UDW_DF_SPE_ACC_ID,'') != '' THEN D.UDW_DF_SPE_ACC_ID -- Uheaa and OneLink 
		WHEN  CC.RegionId = X  AND ISNULL(D.CDW_DF_SPE_ACC_ID,'') != '' THEN D.CDW_DF_SPE_ACC_ID -- CornerStone
	ELSE ISNULL(D.lm_fillerX,'') END [AccountIdentifier], 
	D.call_type,
	D.listid,
	D.appl,
	D.act_date, 
	D.areacode + D.phone, 
	D.tsr,
	D.[status],
	D.addi_status,
	CC.RegionId,
	D.vox_file_name,
	X,
	D.time_connect,
	CASE 
		WHEN 
			CASE
				WHEN CC.RegionId IN (X,X) AND ISNULL(D.UDW_DF_SPE_ACC_ID,'') != '' THEN D.UDW_DF_SPE_ACC_ID -- Uheaa and OneLink 
				WHEN  CC.RegionId = X  AND ISNULL(D.CDW_DF_SPE_ACC_ID,'') != '' THEN D.CDW_DF_SPE_ACC_ID -- CornerStone
			ELSE ISNULL(D.lm_fillerX,'') END = '' THEN X
		ELSE X
	END [Deleted],
	CASE 
		WHEN 
			CASE
				WHEN CC.RegionId IN (X,X) AND ISNULL(D.UDW_DF_SPE_ACC_ID,'') != '' THEN D.UDW_DF_SPE_ACC_ID -- Uheaa and OneLink 
				WHEN  CC.RegionId = X  AND ISNULL(D.CDW_DF_SPE_ACC_ID,'') != '' THEN D.CDW_DF_SPE_ACC_ID -- CornerStone
			ELSE ISNULL(D.lm_fillerX,'') END = '' THEN GETDATE()
		ELSE NULL
	END [DeletedAt],
	CASE 
		WHEN 
			CASE
				WHEN CC.RegionId IN (X,X) AND ISNULL(D.UDW_DF_SPE_ACC_ID,'') != '' THEN D.UDW_DF_SPE_ACC_ID -- Uheaa and OneLink 
				WHEN  CC.RegionId = X  AND ISNULL(D.CDW_DF_SPE_ACC_ID,'') != '' THEN D.CDW_DF_SPE_ACC_ID -- CornerStone
			ELSE ISNULL(D.lm_fillerX,'') END = '' THEN 'InsertCallHistory - No Account#'
		ELSE NULL
	END  [DeletedBy],
	D.TimeACW,
	D.TimeHold,
	D.AgentHold,
	D.FillerX,
	D.FillerX,
	D.FillerX,
	D.d_record_id,
	CASE
		WHEN CC.RegionId IN (X,X) AND ISNULL(D.UDW_CoborrowerAccountNumber,'') != '' THEN D.UDW_CoborrowerAccountNumber -- Uheaa and OneLink 
		WHEN CC.RegionId = X  AND ISNULL(D.CDW_CoborrowerAccountNumber,'') != '' THEN D.CDW_CoborrowerAccountNumber -- CornerStone
	ELSE NULL END
FROM
	@Outbound D
	LEFT JOIN NobleCalls..NobleCallHistory NCH
		ON D.rowid = NCH.NobleRowId
		AND	D.call_type = NCH.CallType
		AND	D.appl = NCH.CallCampaign
		AND	D.act_date = NCH.ActivityDate
		AND (D.areacode + D.phone) = NCH.PhoneNumber
		AND D.addi_status = NCH.AdditionalDispositionCode
		AND D.[status] = NCH.DispositionCode
		AND CAST(NCH.CreatedAt AS DATE) >= CAST(DATEADD(DAY,-X,GETDATE()) AS DATE)
		AND 
		(
			(NCH.CoborrowerAccountNumber IS NULL AND D.CDW_CoborrowerAccountNumber IS NULL AND D.UDW_CoborrowerAccountNumber IS NULL)
			OR NCH.CoborrowerAccountNumber IN(D.CDW_CoborrowerAccountNumber, D.UDW_CoborrowerAccountNumber)
		)
		AND NCH.AccountIdentifier IN (ISNULL(D.lm_fillerX,''), D.CDW_DF_SPE_ACC_ID, D.UDW_DF_SPE_ACC_ID)
	LEFT JOIN NobleCalls..CallCampaigns CC
		ON CC.CallCampaign = D.appl
WHERE
	NCH.NobleRowId IS NULL;