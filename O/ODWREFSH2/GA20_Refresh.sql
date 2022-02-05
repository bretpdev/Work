USE ODW
GO

TRUNCATE TABLE dbo.GA20_CNL_DAT_STAGE
GO

MERGE 
	dbo.GA20_CNL_DAT_STAGE AS [LOCAL]
USING
(
	SELECT
		*
	FROM
		OPENQUERY
		(
			DUSTER,
			'
				SELECT
					REMOTE.*
				FROM
					OLWHRM1.GA20_CNL_DAT REMOTE
			'
		) 
) AS R 
	ON [LOCAL].AF_APL_ID = R.AF_APL_ID
WHEN NOT MATCHED THEN
	INSERT 
	(
		[AF_APL_ID],
		[AC_MPN_SRL_LON],
		[AC_CNL_PRC_TYP],
		[AC_SCL_DSG_BC_DIV],
		[AX_SCL_USE_ARA],
		[AX_LDR_USE_ARA],
		[AC_DSB_HLD_RLS_1],
		[AC_DSB_HLD_RLS_2],
		[AC_DSB_HLD_RLS_3],
		[AC_DSB_HLD_RLS_4],
		[AI_BR_ATH_CRD_CHK],
		[AC_CNL_ERR_MSG_1],
		[AC_CNL_ERR_MSG_2],
		[AC_CNL_ERR_MSG_3],
		[AC_CNL_ERR_MSG_4],
		[AC_CNL_ERR_MSG_5],
		[AI_PNT_XST_IMG_SYS],
		[AC_CNL_FUL_RSP],
		[IF_CNL_SND_BCH],
		[AX_SCL_USE_ARA_USB],
		[AX_LDR_USE_ARA_USB],
		[AC_DIR_DSB_BR_1],
		[AC_DIR_DSB_BR_2],
		[AC_DIR_DSB_BR_3],
		[AC_DIR_DSB_BR_4]
	)
	VALUES 
	(
		R.[AF_APL_ID],
		R.[AC_MPN_SRL_LON],
		R.[AC_CNL_PRC_TYP],
		R.[AC_SCL_DSG_BC_DIV],
		R.[AX_SCL_USE_ARA],
		R.[AX_LDR_USE_ARA],
		R.[AC_DSB_HLD_RLS_1],
		R.[AC_DSB_HLD_RLS_2],
		R.[AC_DSB_HLD_RLS_3],
		R.[AC_DSB_HLD_RLS_4],
		R.[AI_BR_ATH_CRD_CHK],
		R.[AC_CNL_ERR_MSG_1],
		R.[AC_CNL_ERR_MSG_2],
		R.[AC_CNL_ERR_MSG_3],
		R.[AC_CNL_ERR_MSG_4],
		R.[AC_CNL_ERR_MSG_5],
		R.[AI_PNT_XST_IMG_SYS],
		R.[AC_CNL_FUL_RSP],
		R.[IF_CNL_SND_BCH],
		R.[AX_SCL_USE_ARA_USB],
		R.[AX_LDR_USE_ARA_USB],
		R.[AC_DIR_DSB_BR_1],
		R.[AC_DIR_DSB_BR_2],
		R.[AC_DIR_DSB_BR_3],
		R.[AC_DIR_DSB_BR_4]
	)
	-- !!! uncomment lines below ONLY when doing a full table refresh 
	--WHEN NOT MATCHED BY SOURCE THEN
	--    DELETE
	;

GO
TRUNCATE TABLE dbo.GA20_CNL_DAT
GO

MERGE 
	dbo.GA20_CNL_DAT AS [LOCAL]
USING
(
	SELECT
		[AF_APL_ID],
		[AC_MPN_SRL_LON],
		[AC_CNL_PRC_TYP],
		[AC_SCL_DSG_BC_DIV],
		[AX_SCL_USE_ARA],
		[AX_LDR_USE_ARA],
		[AC_DSB_HLD_RLS_1],
		[AC_DSB_HLD_RLS_2],
		[AC_DSB_HLD_RLS_3],
		[AC_DSB_HLD_RLS_4],
		[AI_BR_ATH_CRD_CHK],
		[AC_CNL_ERR_MSG_1],
		[AC_CNL_ERR_MSG_2],
		[AC_CNL_ERR_MSG_3],
		[AC_CNL_ERR_MSG_4],
		[AC_CNL_ERR_MSG_5],
		[AI_PNT_XST_IMG_SYS],
		[AC_CNL_FUL_RSP],
		[IF_CNL_SND_BCH],
		[AX_SCL_USE_ARA_USB],
		[AX_LDR_USE_ARA_USB],
		[AC_DIR_DSB_BR_1],
		[AC_DIR_DSB_BR_2],
		[AC_DIR_DSB_BR_3],
		[AC_DIR_DSB_BR_4]
	FROM
		GA20_CNL_DAT_STAGE
) AS R 
	ON [LOCAL].AF_APL_ID = R.AF_APL_ID
WHEN NOT MATCHED THEN
	INSERT 
	(
		[AF_APL_ID],
		[AC_MPN_SRL_LON],
		[AC_CNL_PRC_TYP],
		[AC_SCL_DSG_BC_DIV],
		[AX_SCL_USE_ARA],
		[AX_LDR_USE_ARA],
		[AC_DSB_HLD_RLS_1],
		[AC_DSB_HLD_RLS_2],
		[AC_DSB_HLD_RLS_3],
		[AC_DSB_HLD_RLS_4],
		[AI_BR_ATH_CRD_CHK],
		[AC_CNL_ERR_MSG_1],
		[AC_CNL_ERR_MSG_2],
		[AC_CNL_ERR_MSG_3],
		[AC_CNL_ERR_MSG_4],
		[AC_CNL_ERR_MSG_5],
		[AI_PNT_XST_IMG_SYS],
		[AC_CNL_FUL_RSP],
		[IF_CNL_SND_BCH],
		[AX_SCL_USE_ARA_USB],
		[AX_LDR_USE_ARA_USB],
		[AC_DIR_DSB_BR_1],
		[AC_DIR_DSB_BR_2],
		[AC_DIR_DSB_BR_3],
		[AC_DIR_DSB_BR_4]
	)
	VALUES 
	(
		R.[AF_APL_ID],
		R.[AC_MPN_SRL_LON],
		R.[AC_CNL_PRC_TYP],
		R.[AC_SCL_DSG_BC_DIV],
		R.[AX_SCL_USE_ARA],
		R.[AX_LDR_USE_ARA],
		R.[AC_DSB_HLD_RLS_1],
		R.[AC_DSB_HLD_RLS_2],
		R.[AC_DSB_HLD_RLS_3],
		R.[AC_DSB_HLD_RLS_4],
		R.[AI_BR_ATH_CRD_CHK],
		R.[AC_CNL_ERR_MSG_1],
		R.[AC_CNL_ERR_MSG_2],
		R.[AC_CNL_ERR_MSG_3],
		R.[AC_CNL_ERR_MSG_4],
		R.[AC_CNL_ERR_MSG_5],
		R.[AI_PNT_XST_IMG_SYS],
		R.[AC_CNL_FUL_RSP],
		R.[IF_CNL_SND_BCH],
		R.[AX_SCL_USE_ARA_USB],
		R.[AX_LDR_USE_ARA_USB],
		R.[AC_DIR_DSB_BR_1],
		R.[AC_DIR_DSB_BR_2],
		R.[AC_DIR_DSB_BR_3],
		R.[AC_DIR_DSB_BR_4]
	)