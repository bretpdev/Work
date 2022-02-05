CREATE TABLE [dbo].[PD24_PRS_BKR] (
    [DF_PRS_ID]          VARCHAR (9)    NOT NULL,
    [DD_BKR_NTF]         DATE           NOT NULL,
    [DD_BKR_FIL]         DATE           NULL,
    [DC_BKR_TYP]         VARCHAR (2)    NOT NULL,
    [DF_ATT]             VARCHAR (9)    NULL,
    [DF_COU_DKT]         VARCHAR (12)   NOT NULL,
    [DD_BKR_VER]         DATE           NULL,
    [DC_BKR_DCH_NDC]     CHAR (1)       NOT NULL,
    [DM_BKR_CT]          VARCHAR (20)   NOT NULL,
    [DC_BKR_ST]          VARCHAR (2)    NOT NULL,
    [DD_BKR_COR_1_RCV]   DATE           NULL,
    [DA_BKR_DCH]         NUMERIC (9, 2) NULL,
    [DD_BKR_STA]         DATE           NOT NULL,
    [DD_BKR_POO_ACK]     DATE           NULL,
    [DD_BKR_POO]         DATE           NULL,
    [DD_BKR_DCH_RCV]     DATE           NULL,
    [DD_BKR_CDR_RCV]     DATE           NULL,
    [DD_BKR_ADS_RCV]     DATE           NULL,
    [DN_BKR_ADS]         VARCHAR (10)   NOT NULL,
    [DC_BKR_STA]         VARCHAR (2)    NOT NULL,
    [DF_LST_DTS_PD24]    DATETIME2 (7)  NOT NULL,
    [IF_IST]             VARCHAR (8)    NULL,
    [DM_FGN_CNY_BKR_FIL] VARCHAR (15)   NOT NULL,
    [DM_FGN_ST_BKR_FIL]  VARCHAR (15)   NOT NULL,
    [DD_BKR_RAF]         DATE           NULL,
    [DD_COU_LST_CNC]     DATE           NULL,
    [DD_BKR_CAE_CLO]     DATE           NULL,
    [DD_BKR_CHP_CVN]     DATE           NULL,
    [DD_BKR_COR_LST_RCV] DATE           NULL,
    [DC_PRS_BKR_RSU_REA] CHAR (1)       NOT NULL,
    [DF_RSU_REA_LST_USR] VARCHAR (12)   NOT NULL,
    CONSTRAINT [PK_PD24_PRS_BKR] PRIMARY KEY CLUSTERED ([DF_PRS_ID] ASC, [DD_BKR_NTF] ASC)
);


GO
CREATE NONCLUSTERED INDEX [IX_PD24_PRS_BKR]
    ON [dbo].[PD24_PRS_BKR]([DD_BKR_FIL] ASC)
    INCLUDE([DF_PRS_ID], [DF_COU_DKT]);

