-- DROP TABLE IF EXISTS #IDR_DATA 

--SELECT DISTINCT
--	PL.ProcessLogId,
--	PL.StartedOn,
--	PL.RunBy,
--	PLM.LogMessage,
--	CASE 
--		WHEN CHARINDEX('ACCOUNT NUMBER:', PLM.LogMessage) != X THEN SUBSTRING(PLM.LogMessage, CHARINDEX('ACCOUNT NUMBER:', PLM.LogMessage) + XX, XX)
--		ELSE NULL
--	END AS AN
--INTO #IDR_DATA
--FROM
--	ProcessLogs..ProcessLogs PL
--	INNER JOIN ProcessLogs..ProcessNotifications PN
--		ON PN.ProcessLogId = PL.ProcessLogId
--	INNER JOIN CLS.[log].ProcessLogMessages PLM
--		ON PLM.ProcessNotificationId = PN.ProcessNotificationId
--WHERE
--	ScriptId = 'IDRUSERPRO'
--	AND CAST(StartedOn AS DATE) = 'XX/XX/XXXX'
--	AND CHARINDEX('ACCOUNT NUMBER:', PLM.LogMessage) != X
--ORDER BY
--	PL.ProcessLogId

SELECT
	P.AN AS ACCOUNT_NUMBER,
	STA.repayment_plan_type_substatus AS APPLICATION_STATUS,
	P.application_id AS APPLICATION_ID,
	P.created_at AS APP_CREATION_DATE,
	P.updated_at AS APP_UPDATED_DATE,
	P.updated_by LAST_USER_TO_UPDATE,
	P.repayment_type_description AS PLAN_SELECTED,
	RS.LN_SEQ AS LOAN_SEQ,
	RS.LC_TYP_SCH_DIS AS CURRENT_SCHEDULE_TYPE,
	lkXX.PX_DSC_MDM AS SYSTEM_TRANSLATION
FROM
	(
		SELECT DISTINCT
			MAX(repayment_plan_type_status_history_id) OVER (PARTITION BY RPH.repayment_plan_type_id) MAX_ID,
			RPH.repayment_plan_type_status_history_id,
			RPH.repayment_plan_type_status_mapping_id,
			A.application_id,
			A.updated_at,
			A.updated_by,
			a.created_at,
			RT.repayment_type_description,
			
			D.StartedOn,
			D.AN
		FROM
			Income_Driven_Repayment..Borrowers B
			INNER JOIN #IDR_DATA D
				ON B.account_number = D.AN
			INNER JOIN Income_Driven_Repayment..Loans L	
				ON L.borrower_id = B.borrower_id
			INNER JOIN Income_Driven_Repayment..Applications A 
				ON A.application_id = L.application_id
			INNER JOIN Income_Driven_Repayment..Repayment_Plan_Selected RPS
				ON RPS.application_id = A.application_id
			INNER JOIN Income_Driven_Repayment..Repayment_Plan_Type_Status_History RPH
				ON RPH.repayment_plan_type_id = RPS.repayment_plan_type_id
			INNER JOIN Income_Driven_Repayment..Repayment_Type RT
				ON RT.repayment_type_id = RPS.repayment_type_id
		WHERE
			(A.created_at >= 'XX/XX/XXXX'
			OR A.updated_at >= 'XX/XX/XXXX')

	) P
	INNER JOIN Income_Driven_Repayment..Repayment_Plan_Type_Substatus STA
		ON STA.repayment_plan_type_substatus_id = P.repayment_plan_type_status_mapping_id
	INNER JOIN CDW..PDXX_PRS_NME PDXX
		ON PDXX.DF_SPE_ACC_ID = P.AN
	LEFT JOIN CDW.calc.RepaymentSchedules RS
		ON RS.BF_SSN = PDXX.DF_PRS_ID
		AND RS.CurrentGradation = X
	LEFT JOIN CDW..LKXX_LS_CDE_LKP LKXX
		ON LKXX.PM_ATR = 'LC-TYP-SCH-DIS'
		AND LKXX.PX_ATR_VAL = RS.LC_TYP_SCH_DIS
	WHERE 
		P.MAX_ID = P.repayment_plan_type_status_history_id
		AND STA.repayment_plan_type_substatus 
		IN (
			'New Income Driven Application Approved on Tax Documentation',
			'New Income Driven Application Approved on ADOI',
			'New Income Driven Application Approved on Self-Certification',
			'Recertification Application Approved on Tax Documentation',
			'Recertification Application Approved on ADOI',
			'Recalculation Approved Lower Income',
			'Recalculation Approved Higher Income or No Change',
			'New Income Driven Application Approved on Tax Documentation',
			'New Income Driven Application Approved on ADOI',
			'New Income Driven Application Approved on Self-Certification'
		   )
