USE UDW
GO

--SELECT TOP 1
--	*
--INTO 
--	dbo.SC10_SCH_DMO
--FROM
--	OPENQUERY
--	(
--		DUSTER,
--		'
--			SELECT
--				*
--			FROM
--				OLWHRM1.SC10_SCH_DMO SC10
--		'
--	) 

DECLARE @LoopCount TINYINT = 0

RefreshStart:

DECLARE @SQLStatement VARCHAR(MAX) = 
'
	MERGE 
		dbo.SC10_SCH_DMO SC10
	USING
		(
			SELECT
				*
			FROM
				OPENQUERY
				(
					DUSTER,
					''
						SELECT
							SC10.*
						FROM
							OLWHRM1.SC10_SCH_DMO SC10
					''
				) 
		) D ON 
			SC10.IF_DOE_SCL = D.IF_DOE_SCL 
	WHEN MATCHED THEN 
		UPDATE SET
			SC10.IM_SCL_SHO = D.IM_SCL_SHO,
			SC10.IM_SCL_FUL = D.IM_SCL_FUL,
			SC10.IC_TYP_MEX_RCC_EFT = D.IC_TYP_MEX_RCC_EFT,
			SC10.IF_ACC_EFT_SCL = D.IF_ACC_EFT_SCL,
			SC10.IF_ABA_EFT_SCL = D.IF_ABA_EFT_SCL,
			SC10.IC_TYP_SCL = D.IC_TYP_SCL,
			SC10.IR_COH_DFL = D.IR_COH_DFL,
			SC10.ID_PRV_SCL_STA = D.ID_PRV_SCL_STA,
			SC10.IC_PRV_SCL_STA = D.IC_PRV_SCL_STA,
			SC10.ID_CUR_SCL_STA = D.ID_CUR_SCL_STA,
			SC10.IC_CUR_SCL_STA = D.IC_CUR_SCL_STA,
			SC10.IF_LST_USR_SC10 = D.IF_LST_USR_SC10,
			SC10.IF_LST_DTS_SC10 = D.IF_LST_DTS_SC10,
			SC10.II_SCL_CHS_PTC = D.II_SCL_CHS_PTC,
			SC10.IM_SCL_VRU_TXT = D.IM_SCL_VRU_TXT,
			SC10.II_SCL_BR_MPN_CNF = D.II_SCL_BR_MPN_CNF,
			SC10.II_SCL_SFD_MPN_CNF = D.II_SCL_SFD_MPN_CNF,
			SC10.II_SCL_USF_MPN_CNF = D.II_SCL_USF_MPN_CNF,
			SC10.II_SCL_BR_MPN_OVR = D.II_SCL_BR_MPN_OVR,
			SC10.IF_SCL_MPN_CNF_LTR = D.IF_SCL_MPN_CNF_LTR,
			SC10.IF_SCL_MPN_NTF_LTR = D.IF_SCL_MPN_NTF_LTR,
			SC10.IC_SCL_USB_LMT = D.IC_SCL_USB_LMT,
			SC10.IC_SCL_SKP_CNC_TYP = D.IC_SCL_SKP_CNC_TYP,
			SC10.II_SCL_SGS_DSB_MON = D.II_SCL_SGS_DSB_MON,
			SC10.II_SCL_SGS_DSB_TUE = D.II_SCL_SGS_DSB_TUE,
			SC10.II_SCL_SGS_DSB_WED = D.II_SCL_SGS_DSB_WED,
			SC10.II_SCL_SGS_DSB_THU = D.II_SCL_SGS_DSB_THU,
			SC10.II_SCL_SGS_DSB_FRI = D.II_SCL_SGS_DSB_FRI,
			SC10.II_SCL_HOL_NXT_PRC = D.II_SCL_HOL_NXT_PRC,
			SC10.II_MLT_DSB_ROS_DAY = D.II_MLT_DSB_ROS_DAY,
			SC10.IF_CDF_RTE_YR = D.IF_CDF_RTE_YR,
			SC10.IR_CDF_PRV_1 = D.IR_CDF_PRV_1,
			SC10.IF_CDF_RTE_PRV_YR1 = D.IF_CDF_RTE_PRV_YR1,
			SC10.IR_CDF_PRV_2 = D.IR_CDF_PRV_2,
			SC10.IF_CDF_RTE_PRV_YR2 = D.IF_CDF_RTE_PRV_YR2,
			SC10.IF_CDA = D.IF_CDA,
			SC10.II_SCL_MNF_DSB_RSN = D.II_SCL_MNF_DSB_RSN,
			SC10.IC_SND_CTF_REQ = D.IC_SND_CTF_REQ,
			SC10.IC_SCL_AUT_DBT_PTC = D.IC_SCL_AUT_DBT_PTC,
			SC10.IC_LEN_LNG_PGM_STY = D.IC_LEN_LNG_PGM_STY,
			SC10.IC_SCL_OWN_CTL_TYP = D.IC_SCL_OWN_CTL_TYP,
			SC10.IF_PEPS_DL_SCL = D.IF_PEPS_DL_SCL,
			SC10.II_SCL_IDP_RPT = D.II_SCL_IDP_RPT,
			SC10.IF_MRG_SCL = D.IF_MRG_SCL,
			SC10.ID_MRG_SCL_EFF = D.ID_MRG_SCL_EFF

	WHEN NOT MATCHED THEN
		INSERT 
		(
			IF_DOE_SCL,
			IM_SCL_SHO,
			IM_SCL_FUL,
			IC_TYP_MEX_RCC_EFT,
			IF_ACC_EFT_SCL,
			IF_ABA_EFT_SCL,
			IC_TYP_SCL,
			IR_COH_DFL,
			ID_PRV_SCL_STA,
			IC_PRV_SCL_STA,
			ID_CUR_SCL_STA,
			IC_CUR_SCL_STA,
			IF_LST_USR_SC10,
			IF_LST_DTS_SC10,
			II_SCL_CHS_PTC,
			IM_SCL_VRU_TXT,
			II_SCL_BR_MPN_CNF,
			II_SCL_SFD_MPN_CNF,
			II_SCL_USF_MPN_CNF,
			II_SCL_BR_MPN_OVR,
			IF_SCL_MPN_CNF_LTR,
			IF_SCL_MPN_NTF_LTR,
			IC_SCL_USB_LMT,
			IC_SCL_SKP_CNC_TYP,
			II_SCL_SGS_DSB_MON,
			II_SCL_SGS_DSB_TUE,
			II_SCL_SGS_DSB_WED,
			II_SCL_SGS_DSB_THU,
			II_SCL_SGS_DSB_FRI,
			II_SCL_HOL_NXT_PRC,
			II_MLT_DSB_ROS_DAY,
			IF_CDF_RTE_YR,
			IR_CDF_PRV_1,
			IF_CDF_RTE_PRV_YR1,
			IR_CDF_PRV_2,
			IF_CDF_RTE_PRV_YR2,
			IF_CDA,
			II_SCL_MNF_DSB_RSN,
			IC_SND_CTF_REQ,
			IC_SCL_AUT_DBT_PTC,
			IC_LEN_LNG_PGM_STY,
			IC_SCL_OWN_CTL_TYP,
			IF_PEPS_DL_SCL,
			II_SCL_IDP_RPT,
			IF_MRG_SCL,
			ID_MRG_SCL_EFF
		)
		VALUES 
		(
			D.IF_DOE_SCL,
			D.IM_SCL_SHO,
			D.IM_SCL_FUL,
			D.IC_TYP_MEX_RCC_EFT,
			D.IF_ACC_EFT_SCL,
			D.IF_ABA_EFT_SCL,
			D.IC_TYP_SCL,
			D.IR_COH_DFL,
			D.ID_PRV_SCL_STA,
			D.IC_PRV_SCL_STA,
			D.ID_CUR_SCL_STA,
			D.IC_CUR_SCL_STA,
			D.IF_LST_USR_SC10,
			D.IF_LST_DTS_SC10,
			D.II_SCL_CHS_PTC,
			D.IM_SCL_VRU_TXT,
			D.II_SCL_BR_MPN_CNF,
			D.II_SCL_SFD_MPN_CNF,
			D.II_SCL_USF_MPN_CNF,
			D.II_SCL_BR_MPN_OVR,
			D.IF_SCL_MPN_CNF_LTR,
			D.IF_SCL_MPN_NTF_LTR,
			D.IC_SCL_USB_LMT,
			D.IC_SCL_SKP_CNC_TYP,
			D.II_SCL_SGS_DSB_MON,
			D.II_SCL_SGS_DSB_TUE,
			D.II_SCL_SGS_DSB_WED,
			D.II_SCL_SGS_DSB_THU,
			D.II_SCL_SGS_DSB_FRI,
			D.II_SCL_HOL_NXT_PRC,
			D.II_MLT_DSB_ROS_DAY,
			D.IF_CDF_RTE_YR,
			D.IR_CDF_PRV_1,
			D.IF_CDF_RTE_PRV_YR1,
			D.IR_CDF_PRV_2,
			D.IF_CDF_RTE_PRV_YR2,
			D.IF_CDA,
			D.II_SCL_MNF_DSB_RSN,
			D.IC_SND_CTF_REQ,
			D.IC_SCL_AUT_DBT_PTC,
			D.IC_LEN_LNG_PGM_STY,
			D.IC_SCL_OWN_CTL_TYP,
			D.IF_PEPS_DL_SCL,
			D.II_SCL_IDP_RPT,
			D.IF_MRG_SCL,
			D.ID_MRG_SCL_EFF
		)
	-- !!! uncomment lines below ONLY when doing a full table refresh 
		WHEN NOT MATCHED BY SOURCE THEN
			DELETE
	;
'

PRINT @SQLStatement
EXEC (@SQLStatement)


-- ###### VALIDATION
DECLARE 
	@CountDifference INT

SELECT
	@CountDifference = L.LocalCount - R.RemoteCount
FROM
	OPENQUERY
	(
		DUSTER,
		'
			SELECT
				COUNT(*) AS "RemoteCount"
			FROM
				OLWHRM1.SC10_SCH_DMO SC10
		'	
	) R
	FULL OUTER JOIN
	(
		SELECT
			COUNT(*) [LocalCount]
		FROM
			UDW..SC10_SCH_DMO SC10
	) L ON 1 = 1

IF @CountDifference != 0 AND @LoopCount > 0
	BEGIN
		RAISERROR('SC10_SCH_DMO - The remote and local record counts do not match.  The local count is off by %i records.  A full refresh of the table is required.', 16, 11, @CountDifference)
	END
ELSE IF @CountDifference != 0 AND @LoopCount = 0
	BEGIN

		SET @LoopCount = @LoopCount + 1
		
		-- Local and remote record counts did not match.  Run refresh again.
		GOTO RefreshStart;
	END

