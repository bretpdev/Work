/********* HISTORIC *********/

SELECT DISTINCT
	 TT.Sr
	,SR.Script
	,RU.Unit
FROM
	BSYS..SCKR_REF_Unit RU
	INNER JOIN BSYS..SCKR_DAT_ScriptRequests SR
		ON SR.Script = RU.Program
	INNER JOIN CSYS..COST_DAT_TimeTracking TT
		ON TT.Sr = SR.Request
	LEFT JOIN CSYS..GENR_LST_BusinessUnits BU
		ON BU.Name = RU.Unit
	LEFT JOIN CSYS..COST_DAT_BusinessUnitCostCenters CC
		ON CC.BusinessUnitId = BU.ID
WHERE
	BU.ID IS NULL
ORDER BY
	SR.Script
;

SELECT DISTINCT
	 TT.Sasr
	,SASR.Job
	,RU.Unit
FROM
	BSYS..SCKR_REF_UnitSAS RU
	INNER JOIN BSYS..SCKR_DAT_SASRequests SASR
		ON SASR.Job = RU.Program
	INNER JOIN CSYS..COST_DAT_TimeTracking TT
		ON TT.Sasr = SASR.Request
	LEFT JOIN CSYS..GENR_LST_BusinessUnits BU
		ON BU.Name = RU.Unit
	LEFT JOIN CSYS..COST_DAT_BusinessUnitCostCenters CC
		ON CC.BusinessUnitId = BU.ID
WHERE
	BU.ID IS NULL
ORDER BY
	TT.SASR
;

SELECT DISTINCT
	 TT.Lts
	,LR.Title
	,RU.Unit
FROM
	BSYS..LTDB_REF_Unit RU
	INNER JOIN BSYS..LTDB_DAT_Requests LR
		ON LR.DocName = RU.DocName
	INNER JOIN CSYS..COST_DAT_TimeTracking TT
		ON TT.Lts = LR.Request
	LEFT JOIN CSYS..GENR_LST_BusinessUnits BU
		ON BU.Name = RU.Unit
	LEFT JOIN CSYS..COST_DAT_BusinessUnitCostCenters CC
		ON CC.BusinessUnitId = BU.ID
WHERE
	BU.ID IS NULL
ORDER BY
	TT.Lts
;

/****** CURRENT ******/

SELECT DISTINCT
	 TT.Sr
	,SR.Script
	,RU.Unit
FROM
	BSYS..SCKR_REF_Unit RU
	INNER JOIN BSYS..SCKR_DAT_ScriptRequests SR
		ON SR.Script = RU.Program
	INNER JOIN CSYS..COST_DAT_TimesheetProcessing TT
		ON TT.Sr = SR.Request
	LEFT JOIN CSYS..GENR_LST_BusinessUnits BU
		ON BU.Name = RU.Unit
	LEFT JOIN CSYS..COST_DAT_BusinessUnitCostCenters CC
		ON CC.BusinessUnitId = BU.ID
WHERE
	BU.ID IS NULL
ORDER BY
	SR.Script
;

SELECT DISTINCT
	 TT.Sasr
	,SASR.Job
	,RU.Unit
FROM
	BSYS..SCKR_REF_UnitSAS RU
	INNER JOIN BSYS..SCKR_DAT_SASRequests SASR
		ON SASR.Job = RU.Program
	INNER JOIN CSYS..COST_DAT_TimesheetProcessing TT
		ON TT.Sasr = SASR.Request
	LEFT JOIN CSYS..GENR_LST_BusinessUnits BU
		ON BU.Name = RU.Unit
	LEFT JOIN CSYS..COST_DAT_BusinessUnitCostCenters CC
		ON CC.BusinessUnitId = BU.ID
WHERE
	BU.ID IS NULL
ORDER BY
	TT.SASR
;

SELECT DISTINCT
	 TT.Lts
	,LR.Title
	,RU.Unit
FROM
	BSYS..LTDB_REF_Unit RU
	INNER JOIN BSYS..LTDB_DAT_Requests LR
		ON LR.DocName = RU.DocName
	INNER JOIN CSYS..COST_DAT_TimesheetProcessing TT
		ON TT.Lts = LR.Request
	LEFT JOIN CSYS..GENR_LST_BusinessUnits BU
		ON BU.Name = RU.Unit
	LEFT JOIN CSYS..COST_DAT_BusinessUnitCostCenters CC
		ON CC.BusinessUnitId = BU.ID
WHERE
	BU.ID IS NULL
ORDER BY
	TT.Lts
;

/****** STAND-ALONE SSRS ******/
/****** CURRENT per user ******/

--Business Unit in Sacker (SR) is not used any more
WITH POP AS
(
	SELECT
		TP.SourceFile,
		'' AS RowNumber,
		TP.TaskDate,
		CONCAT('Defunct Business Unit in Sacker (SR_',TP.Sr,')') AS ColumnName,
		RU.Unit AS InvalidValue
		--,SR.Script
		--,RU.Unit
	FROM
		BSYS..SCKR_REF_Unit RU
		INNER JOIN BSYS..SCKR_DAT_ScriptRequests SR
			ON SR.Script = RU.Program
		INNER JOIN CSYS..COST_DAT_TimesheetProcessing TP
			ON TP.Sr = SR.Request
		LEFT JOIN CSYS..GENR_LST_BusinessUnits BU
			ON BU.Name = RU.Unit
		LEFT JOIN CSYS..COST_DAT_BusinessUnitCostCenters CC
			ON CC.BusinessUnitId = BU.ID
	WHERE
		BU.ID IS NULL
)
SELECT
	A.SourceFile,
	A.RowNumber,
	B_min.TaskDate,
	A.ColumnName,
	A.InvalidValue
FROM
	POP A
	INNER JOIN
	(--gets earliest instance of ticket since only 1 person needs to fix it
		SELECT
			MIN(TaskDate) AS TaskDate
		FROM
			POP
	) B_min
		ON A.TaskDate = B_min.TaskDate
;

--Business Unit in Sacker (SASR) is not used any more
WITH POP AS
(
	SELECT DISTINCT
		TP.SourceFile,
		'' AS RowNumber,
		TP.TaskDate,
		CONCAT('Defunct Business Unit in Sacker (SASR_',TP.Sasr,')') AS ColumnName,
		RU.Unit AS InvalidValue
		--,SASR.Job
		--,RU.Unit
	FROM
		BSYS..SCKR_REF_UnitSAS RU
		INNER JOIN BSYS..SCKR_DAT_SASRequests SASR
			ON SASR.Job = RU.Program
		INNER JOIN CSYS..COST_DAT_TimesheetProcessing TP
			ON TP.Sasr = SASR.Request
		LEFT JOIN CSYS..GENR_LST_BusinessUnits BU
			ON BU.Name = RU.Unit
		LEFT JOIN CSYS..COST_DAT_BusinessUnitCostCenters CC
			ON CC.BusinessUnitId = BU.ID
	WHERE
		BU.ID IS NULL
)
SELECT
	A.SourceFile,
	A.RowNumber,
	B_min.TaskDate,
	A.ColumnName,
	A.InvalidValue
FROM
	POP A
	INNER JOIN
	(--gets earliest instance of ticket since only 1 person needs to fix it
		SELECT
			MIN(TaskDate) AS TaskDate
		FROM
			POP
	) B_min
		ON A.TaskDate = B_min.TaskDate
;

--Business Unit in Letter Tracking Database is not used any more
WITH POP AS
(
	SELECT DISTINCT
		TP.SourceFile,
		'' AS RowNumber,
		TP.TaskDate,
		CONCAT('Defunct Business Unit in Letter Tracking Database (LR_',TP.Lts,')') AS ColumnName,
		RU.Unit AS InvalidValue
		--,LR.Title
		--,RU.Unit
	FROM
		BSYS..LTDB_REF_Unit RU
		INNER JOIN BSYS..LTDB_DAT_Requests LR
			ON LR.DocName = RU.DocName
		INNER JOIN CSYS..COST_DAT_TimesheetProcessing TP
			ON TP.Lts = LR.Request
		LEFT JOIN CSYS..GENR_LST_BusinessUnits BU
			ON BU.Name = RU.Unit
		LEFT JOIN CSYS..COST_DAT_BusinessUnitCostCenters CC
			ON CC.BusinessUnitId = BU.ID
	WHERE
		BU.ID IS NULL
)
SELECT
	A.SourceFile,
	A.RowNumber,
	B_min.TaskDate,
	A.ColumnName,
	A.InvalidValue
FROM
	POP A
	INNER JOIN
	(--gets earliest instance of ticket since only 1 person needs to fix it
		SELECT
			MIN(TaskDate) AS TaskDate
		FROM
			POP
	) B_min
		ON A.TaskDate = B_min.TaskDate
;


/****** integrated with timetracking SSRS weekly error report ******/
/****** CURRENT per user ******/

--Business Unit in Sacker (SR) is not used any more
SELECT
	A.SourceFile,
	A.RowNumber,
	B_min.TaskDate,
	A.ColumnName,
	A.InvalidValue
FROM
	(--population
		SELECT
			TP.SourceFile,
			'' AS RowNumber,
			TP.TaskDate,
			CONCAT('Defunct Business Unit in Sacker (SR_',TP.Sr,')') AS ColumnName,
			RU.Unit AS InvalidValue
			--,SR.Script
			--,RU.Unit
		FROM
			BSYS..SCKR_REF_Unit RU
			INNER JOIN BSYS..SCKR_DAT_ScriptRequests SR
				ON SR.Script = RU.Program
			INNER JOIN CSYS..COST_DAT_TimesheetProcessing TP
				ON TP.Sr = SR.Request
			LEFT JOIN CSYS..GENR_LST_BusinessUnits BU
				ON BU.Name = RU.Unit
			LEFT JOIN CSYS..COST_DAT_BusinessUnitCostCenters CC
				ON CC.BusinessUnitId = BU.ID
		WHERE
			BU.ID IS NULL
	) A
	INNER JOIN
	(--gets earliest instance of ticket since only 1 person needs to fix it
		SELECT
			MIN(TaskDate) AS TaskDate
		FROM
			(--population
				SELECT
					TP.SourceFile,
					'' AS RowNumber,
					TP.TaskDate,
					CONCAT('Defunct Business Unit in Sacker (SR_',TP.Sr,')') AS ColumnName,
					RU.Unit AS InvalidValue
					--,SR.Script
					--,RU.Unit
				FROM
					BSYS..SCKR_REF_Unit RU
					INNER JOIN BSYS..SCKR_DAT_ScriptRequests SR
						ON SR.Script = RU.Program
					INNER JOIN CSYS..COST_DAT_TimesheetProcessing TP
						ON TP.Sr = SR.Request
					LEFT JOIN CSYS..GENR_LST_BusinessUnits BU
						ON BU.Name = RU.Unit
					LEFT JOIN CSYS..COST_DAT_BusinessUnitCostCenters CC
						ON CC.BusinessUnitId = BU.ID
				WHERE
					BU.ID IS NULL
			) POP
	) B_min
		ON A.TaskDate = B_min.TaskDate
;

--Business Unit in Sacker (SASR) is not used any more
SELECT
	A.SourceFile,
	A.RowNumber,
	B_min.TaskDate,
	A.ColumnName,
	A.InvalidValue
FROM
	(--population
		SELECT DISTINCT
			TP.SourceFile,
			'' AS RowNumber,
			TP.TaskDate,
			CONCAT('Defunct Business Unit in Sacker (SASR_',TP.Sasr,')') AS ColumnName,
			RU.Unit AS InvalidValue
			--,SASR.Job
			--,RU.Unit
		FROM
			BSYS..SCKR_REF_UnitSAS RU
			INNER JOIN BSYS..SCKR_DAT_SASRequests SASR
				ON SASR.Job = RU.Program
			INNER JOIN CSYS..COST_DAT_TimesheetProcessing TP
				ON TP.Sasr = SASR.Request
			LEFT JOIN CSYS..GENR_LST_BusinessUnits BU
				ON BU.Name = RU.Unit
			LEFT JOIN CSYS..COST_DAT_BusinessUnitCostCenters CC
				ON CC.BusinessUnitId = BU.ID
		WHERE
			BU.ID IS NULL
	) A
	INNER JOIN
	(--gets earliest instance of ticket since only 1 person needs to fix it
		SELECT
			MIN(TaskDate) AS TaskDate
		FROM
			(--population
				SELECT DISTINCT
					TP.SourceFile,
					'' AS RowNumber,
					TP.TaskDate,
					CONCAT('Defunct Business Unit in Sacker (SASR_',TP.Sasr,')') AS ColumnName,
					RU.Unit AS InvalidValue
					--,SASR.Job
					--,RU.Unit
				FROM
					BSYS..SCKR_REF_UnitSAS RU
					INNER JOIN BSYS..SCKR_DAT_SASRequests SASR
						ON SASR.Job = RU.Program
					INNER JOIN CSYS..COST_DAT_TimesheetProcessing TP
						ON TP.Sasr = SASR.Request
					LEFT JOIN CSYS..GENR_LST_BusinessUnits BU
						ON BU.Name = RU.Unit
					LEFT JOIN CSYS..COST_DAT_BusinessUnitCostCenters CC
						ON CC.BusinessUnitId = BU.ID
				WHERE
					BU.ID IS NULL
			) POP
	) B_min
		ON A.TaskDate = B_min.TaskDate
;

--Business Unit in Letter Tracking Database is not used any more
SELECT
	A.SourceFile,
	A.RowNumber,
	B_min.TaskDate,
	A.ColumnName,
	A.InvalidValue
FROM
	(--population
		SELECT DISTINCT
			TP.SourceFile,
			'' AS RowNumber,
			TP.TaskDate,
			CONCAT('Defunct Business Unit in Letter Tracking Database (LR_',TP.Lts,')') AS ColumnName,
			RU.Unit AS InvalidValue
			--,LR.Title
			--,RU.Unit
		FROM
			BSYS..LTDB_REF_Unit RU
			INNER JOIN BSYS..LTDB_DAT_Requests LR
				ON LR.DocName = RU.DocName
			INNER JOIN CSYS..COST_DAT_TimesheetProcessing TP
				ON TP.Lts = LR.Request
			LEFT JOIN CSYS..GENR_LST_BusinessUnits BU
				ON BU.Name = RU.Unit
			LEFT JOIN CSYS..COST_DAT_BusinessUnitCostCenters CC
				ON CC.BusinessUnitId = BU.ID
		WHERE
			BU.ID IS NULL	
	) A
	INNER JOIN
	(--gets earliest instance of ticket since only 1 person needs to fix it
		SELECT
			MIN(TaskDate) AS TaskDate
		FROM
			(--population
				SELECT DISTINCT
					TP.SourceFile,
					'' AS RowNumber,
					TP.TaskDate,
					CONCAT('Defunct Business Unit in Letter Tracking Database (LR_',TP.Lts,')') AS ColumnName,
					RU.Unit AS InvalidValue
					--,LR.Title
					--,RU.Unit
				FROM
					BSYS..LTDB_REF_Unit RU
					INNER JOIN BSYS..LTDB_DAT_Requests LR
						ON LR.DocName = RU.DocName
					INNER JOIN CSYS..COST_DAT_TimesheetProcessing TP
						ON TP.Lts = LR.Request
					LEFT JOIN CSYS..GENR_LST_BusinessUnits BU
						ON BU.Name = RU.Unit
					LEFT JOIN CSYS..COST_DAT_BusinessUnitCostCenters CC
						ON CC.BusinessUnitId = BU.ID
				WHERE
					BU.ID IS NULL
			) POP
	) B_min
		ON A.TaskDate = B_min.TaskDate
;
