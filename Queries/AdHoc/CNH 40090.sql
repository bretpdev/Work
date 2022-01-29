USE CDW
GO

SELECT DISTINCT
	PDXX.DF_SPE_ACC_ID AS AccountNumber,
	COMBINED_COMMENT.LX_ATY AS [AYXXText],
	--rpts.repayment_plan_type_status,
	--rptss.repayment_plan_type_substatus_description,
	WQXX.WN_CTL_TSK,
	WQXX.CreateDate,
	WQXX.WorkDate,
	WQXX.WorkTime,
	WQXX.WorkUser,
	WQXX.CompleteDate,
	WQXX.CompleteUser	
FROM
	PDXX_PRS_NME PDXX
	--AYXX Combined Comment
	INNER JOIN
	(
		SELECT DISTINCT
			AYXX.BF_SSN,
			AYXX.LN_ATY_SEQ,
			AYXX.LD_ATY_REQ_RCV,
			AYXX.PF_REQ_ACT,
			AYXX.LC_STA_ACTYXX,
			STUFF(
			(
				SELECT 
						' ' + SUB.LX_ATY AS [text()]
				FROM 
					AYXX_ATY_TXT SUB
				WHERE
					SUB.BF_SSN = AYXX.BF_SSN
					AND SUB.LN_ATY_SEQ = AYXX.LN_ATY_SEQ
			FOR XML PATH('')
			)
			,X,X, '') AS LX_ATY	
		FROM	
			AYXX_BR_LON_ATY AYXX
			INNER JOIN AYXX_ATY_CMT AYXX
				ON AYXX.BF_SSN = AYXX.BF_SSN
				AND AYXX.LN_ATY_SEQ = AYXX.LN_ATY_SEQ
				AND AYXX.LC_STA_AYXX = 'A'
			INNER JOIN AYXX_ATY_TXT AYXX
				ON AYXX.BF_SSN = AYXX.BF_SSN
				AND AYXX.LN_ATY_SEQ = AYXX.LN_ATY_SEQ
				AND AYXX.LN_ATY_CMT_SEQ = AYXX.LN_ATY_CMT_SEQ
		WHERE
			AYXX.PF_REQ_ACT = 'IDRPN'
			AND AYXX.LC_STA_ACTYXX = 'A'
	) COMBINED_COMMENT
		ON COMBINED_COMMENT.BF_SSN = PDXX.DF_PRS_ID
	INNER JOIN
	(
		SELECT 
			Unassign.BF_SSN,
			Unassign.LN_ATY_SEQ,
			Unassign.WN_CTL_TSK,
			Unassign.WF_CRT_DTS_WQXX AS CreateDate,
			Work.WD_INI_TSK AS WorkDate,
			Work.WT_INI_TSK AS WorkTime,
			Work.WF_USR_ASN_TSK AS WorkUser,
			Complete.WF_CRT_DTS_WQXX AS CompleteDate,
			Complete.WF_USR_ASN_TSK AS CompleteUser
		FROM 
			CDW..WQXX_TSK_QUE_HST Unassign
			INNER JOIN CDW..WQXX_TSK_QUE_HST Work
				ON Work.BF_SSN = Unassign.BF_SSN
				AND Work.LN_ATY_SEQ = Unassign.LN_ATY_SEQ
				AND Work.WF_QUE = Unassign.WF_QUE
				AND Work.WF_SUB_QUE = Unassign.WF_SUB_QUE
				AND Work.WC_STA_WQUEXX = 'W'
				AND Work.WF_CRT_DTS_WQXX > Unassign.WF_CRT_DTS_WQXX
			INNER JOIN CDW..WQXX_TSK_QUE_HST Complete
				ON Complete.BF_SSN = Work.BF_SSN
				AND Complete.LN_ATY_SEQ = Work.LN_ATY_SEQ
				AND Complete.WF_QUE = Work.WF_QUE
				AND Complete.WF_SUB_QUE = Work.WF_SUB_QUE
				AND Complete.WC_STA_WQUEXX IN('C','X')
				AND Complete.WF_CRT_DTS_WQXX > Work.WF_CRT_DTS_WQXX
		WHERE
			Unassign.WF_QUE = 'XP'
			AND Unassign.WF_SUB_QUE = 'XX'
			AND Unassign.WC_STA_WQUEXX = 'U'
			AND CAST(Complete.WF_LST_DTS_WQXX AS DATE) BETWEEN 'XXXX-XX-XX' AND 'XXXX-XX-XX'
	) WQXX
		ON WQXX.BF_SSN = COMBINED_COMMENT.BF_SSN
		AND WQXX.LN_ATY_SEQ = COMBINED_COMMENT.LN_ATY_SEQ
	--INNER JOIN Income_Driven_Repayment.dbo.Borrowers B
	--	ON PDXX.DF_SPE_ACC_ID = B.account_number
	--INNER JOIN Income_Driven_Repayment.dbo.Loans L
	--	ON B.borrower_id = L.borrower_id
	--INNER JOIN Income_Driven_Repayment.dbo.Applications A
	--	ON A.Active = X
	--	AND L.application_id = A.application_id
	--INNER JOIN Income_Driven_Repayment..Repayment_Plan_Selected rps
	--	ON rps.application_id = A.application_id
 --   INNER JOIN Income_Driven_Repayment..Repayment_Plan_Type_Status_History rptsh
	--	ON rptsh.repayment_plan_type_id = rps.repayment_plan_type_id
 --   INNER JOIN Income_Driven_Repayment..Repayment_Plan_Type_Substatus rptss
	--	ON rptss.repayment_plan_type_substatus_id = rptsh.repayment_plan_type_status_mapping_id
	--INNER JOIN Income_Driven_Repayment..Repayment_Plan_Type_Status rpts
	--	ON rpts.repayment_plan_type_status_id = rptss.repayment_plan_type_status_id
WHERE
	COMBINED_COMMENT.PF_REQ_ACT='IDRPN'
	--AND rpts.repayment_plan_type_substatus_id = XX
	--AND rpts.repayment_plan_type_status_id = X
	--AND rpts.repayment_plan_type_substatus = 'Application Pending - Other'
	AND COMBINED_COMMENT.LC_STA_ACTYXX = 'A'
ORDER BY
	PDXX.DF_SPE_ACC_ID,
	WQXX.WN_CTL_TSK,
	WQXX.CreateDate,
	WQXX.WorkDate,
	WQXX.WorkTime




