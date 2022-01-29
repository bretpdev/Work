﻿CREATE TABLE [dbo].[WQ20_TSK_QUE] (
    [WF_QUE]            VARCHAR (2)   NOT NULL,
    [WF_SUB_QUE]        VARCHAR (2)   NOT NULL,
    [WN_CTL_TSK]        VARCHAR (18)  NOT NULL,
    [PF_REQ_ACT]        VARCHAR (5)   NOT NULL,
    [WD_ACT_REQ]        DATE          NOT NULL,
    [WD_ACT_RQR]        DATE          NULL,
    [WC_CND_CTC]        CHAR (1)      NOT NULL,
    [WD_INI_TSK]        DATE          NULL,
    [WT_INI_TSK]        TIME (0)      NULL,
    [WF_USR_ASN_TSK]    VARCHAR (8)   NOT NULL,
    [WF_USR_ASN_BY_TSK] VARCHAR (8)   NOT NULL,
    [WX_MSG_1_TSK]      VARCHAR (77)  NOT NULL,
    [WX_MSG_2_TSK]      VARCHAR (77)  NOT NULL,
    [WC_STA_WQUE20]     CHAR (1)      NOT NULL,
    [WX_VAL_SRH_2]      VARCHAR (18)  NOT NULL,
    [WX_VAL_SRH_1]      VARCHAR (18)  NOT NULL,
    [WF_LST_DTS_WQ20]   DATETIME2 (7) NOT NULL,
    [BF_SSN]            CHAR (9)      NULL,
    [LN_ATY_SEQ]        INT           NULL,
    [WF_CRT_DTS_WQ20]   DATETIME2 (7) NOT NULL,
    CONSTRAINT [PK_WQ20_TSK_QUE] PRIMARY KEY CLUSTERED ([WF_QUE] ASC, [WF_SUB_QUE] ASC, [WN_CTL_TSK] ASC, [PF_REQ_ACT] ASC) WITH (FILLFACTOR = 95)
);

