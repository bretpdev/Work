CREATE TABLE [dbo].[LT20_LTR_REQ_PRC] (
    [DF_SPE_ACC_ID]                 CHAR (10)     NOT NULL,
    [RM_APL_PGM_PRC]                VARCHAR (8)   NOT NULL,
    [RT_RUN_SRT_DTS_PRC]            DATETIME      NOT NULL,
    [RN_SEQ_LTR_CRT_PRC]            INT           NOT NULL,
    [RN_SEQ_REC_PRC]                INT           NOT NULL,
    [RM_DSC_LTR_PRC]                VARCHAR (10)  NOT NULL,
    [RC_TYP_SBJ_PRC]                CHAR (1)      NOT NULL,
    [RF_SBJ_PRC]                    CHAR (9)      NOT NULL,
    [RN_ENT_REQ_PRC]                NUMERIC (3)   NOT NULL,
    [RN_ATY_SEQ_PRC]                INT           NOT NULL,
    [RI_REC_PRC]                    CHAR (1)      NOT NULL,
    [RX_REQ_ARA_1_PRC]              VARCHAR (250) NOT NULL,
    [RX_REQ_ARA_2_PRC]              VARCHAR (250) NOT NULL,
    [RX_REQ_ARA_3_PRC]              VARCHAR (250) NOT NULL,
    [RX_REQ_ARA_4_PRC]              VARCHAR (250) NOT NULL,
    [RX_REQ_ARA_5_PRC]              VARCHAR (250) NOT NULL,
    [RX_REQ_ARA_6_PRC]              VARCHAR (250) NOT NULL,
    [RI_LTR_REQ_DEL_PRC]            CHAR (1)      NOT NULL,
    [RC_LTR_REQ_SRC_PRC]            CHAR (1)      NOT NULL,
    [RI_PRV_RUN_ERR_PRC]            CHAR (1)      NOT NULL,
    [RF_LST_DTS_LT20]               DATETIME      NOT NULL,
    [RF_COR_DOC_PRC]                VARCHAR (17)  NOT NULL,
    [RX_REQ_ARA_16_PRC]             VARCHAR (210) NOT NULL,
    [RX_REQ_ARA_15_PRC]             VARCHAR (250) NOT NULL,
    [RX_REQ_ARA_14_PRC]             VARCHAR (250) NOT NULL,
    [RX_REQ_ARA_13_PRC]             VARCHAR (250) NOT NULL,
    [RX_REQ_ARA_12_PRC]             VARCHAR (250) NOT NULL,
    [RX_REQ_ARA_11_PRC]             VARCHAR (250) NOT NULL,
    [RX_REQ_ARA_10_PRC]             VARCHAR (250) NOT NULL,
    [RX_REQ_ARA_9_PRC]              VARCHAR (250) NOT NULL,
    [RX_REQ_ARA_8_PRC]              VARCHAR (250) NOT NULL,
    [RX_REQ_ARA_7_PRC]              VARCHAR (250) NOT NULL,
    [RI_LTR_OPT_ENC_PRC]            CHAR (1)      NOT NULL,
    [RN_ARR_PAR_PRC]                SMALLINT      NULL,
    [OnEcorr]                       BIT           NOT NULL,
    [PrintedAt]                     DATETIME      NULL,
    [EcorrDocumentCreatedAt]        DATETIME      NULL,
    [InactivatedAt]                 DATETIME      NULL,
    [SystemLetterExclusionReasonId] INT           NULL,
    [CreatedAt]                     DATETIME      CONSTRAINT [DF_LT20_LTR_REQ_PRC_CreatedAt] DEFAULT (getdate()) NULL,
    [ProcessingAttempts]            INT           DEFAULT ((0)) NULL
);




GO



GO
CREATE NONCLUSTERED INDEX [IX_OnEcorr_InactivatedAt_PrintedAt_CreatedAt]
    ON [dbo].[LT20_LTR_REQ_PRC]([OnEcorr] ASC, [InactivatedAt] ASC, [PrintedAt] ASC, [CreatedAt] ASC)
    INCLUDE([RM_DSC_LTR_PRC], [RF_SBJ_PRC]);


GO



GO



GO
CREATE NONCLUSTERED INDEX [IX_RILTRREQDELPRC_InactivatedAt_Full]
    ON [dbo].[LT20_LTR_REQ_PRC]([RI_LTR_REQ_DEL_PRC] ASC, [InactivatedAt] ASC)
    INCLUDE([DF_SPE_ACC_ID], [RM_APL_PGM_PRC], [RT_RUN_SRT_DTS_PRC], [RN_SEQ_LTR_CRT_PRC], [RN_SEQ_REC_PRC], [RM_DSC_LTR_PRC], [RF_SBJ_PRC], [OnEcorr], [PrintedAt], [EcorrDocumentCreatedAt], [CreatedAt]);


GO
ALTER INDEX [IX_RILTRREQDELPRC_InactivatedAt_Full]
    ON [dbo].[LT20_LTR_REQ_PRC] DISABLE;


GO
CREATE NONCLUSTERED INDEX [IX_LT20_LTR_REQ_PRC_InactivatedAt_RM_DSC_LTR_PRC]
    ON [dbo].[LT20_LTR_REQ_PRC]([InactivatedAt] ASC)
    INCLUDE([RT_RUN_SRT_DTS_PRC], [RM_DSC_LTR_PRC], [OnEcorr], [PrintedAt], [EcorrDocumentCreatedAt], [CreatedAt]) WITH (FILLFACTOR = 85, DATA_COMPRESSION = PAGE);


GO
CREATE NONCLUSTERED INDEX [IX_LT20_LTR_REQ_PRC_InactivatedAt]
    ON [dbo].[LT20_LTR_REQ_PRC]([InactivatedAt] ASC)
    INCLUDE([RT_RUN_SRT_DTS_PRC], [OnEcorr], [PrintedAt], [EcorrDocumentCreatedAt], [CreatedAt]) WITH (FILLFACTOR = 85);


GO
ALTER INDEX [IX_LT20_LTR_REQ_PRC_InactivatedAt]
    ON [dbo].[LT20_LTR_REQ_PRC] DISABLE;


GO
CREATE NONCLUSTERED INDEX [IX_LT20_LTR_REQ_PRC_DFSPEACCID_RNATYSEQPRC]
    ON [dbo].[LT20_LTR_REQ_PRC]([DF_SPE_ACC_ID] ASC, [RM_APL_PGM_PRC] ASC, [RT_RUN_SRT_DTS_PRC] ASC, [RN_SEQ_LTR_CRT_PRC] ASC)
    INCLUDE([RN_SEQ_REC_PRC], [RN_ATY_SEQ_PRC]) WITH (FILLFACTOR = 85, DATA_COMPRESSION = PAGE);

