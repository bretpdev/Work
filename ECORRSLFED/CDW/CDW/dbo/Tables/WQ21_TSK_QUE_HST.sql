﻿CREATE TABLE [dbo].[WQ21_TSK_QUE_HST] (
    [WF_QUE]            CHAR (2)      NOT NULL,
    [WF_SUB_QUE]        CHAR (2)      NOT NULL,
    [WN_CTL_TSK]        CHAR (18)     NOT NULL,
    [PF_REQ_ACT]        CHAR (5)      NOT NULL,
    [WF_CRT_DTS_WQ21]   DATETIME2 (7) NOT NULL,
    [WD_ACT_REQ]        DATE          NOT NULL,
    [WD_ACT_RQR]        DATE          NULL,
    [WC_CND_CTC]        CHAR (1)      NOT NULL,
    [WD_INI_TSK]        DATE          NULL,
    [WT_INI_TSK]        TIME (0)      NULL,
    [WF_USR_ASN_TSK]    CHAR (8)      NOT NULL,
    [WF_USR_ASN_BY_TSK] CHAR (8)      NOT NULL,
    [WX_MSG_1_TSK]      CHAR (77)     NOT NULL,
    [WX_MSG_2_TSK]      CHAR (77)     NOT NULL,
    [WC_STA_WQUE20]     CHAR (1)      NOT NULL,
    [WF_LST_DTS_WQ20]   DATETIME2 (7) NOT NULL,
    [BF_SSN]            CHAR (9)      NULL,
    [LN_ATY_SEQ]        INT           NULL,
    [WF_CRT_DTS_WQ20]   DATETIME2 (7) NOT NULL,
    CONSTRAINT [PK_WQ21_TSK_QUE_HST] PRIMARY KEY CLUSTERED ([WF_QUE] ASC, [WF_SUB_QUE] ASC, [WN_CTL_TSK] ASC, [PF_REQ_ACT] ASC, [WF_CRT_DTS_WQ21] ASC) WITH (FILLFACTOR = 95)
);




GO
CREATE NONCLUSTERED INDEX [IX_WFUSRASNTSK_WCSTAWQUE20]
    ON [dbo].[WQ21_TSK_QUE_HST]([WF_USR_ASN_TSK] ASC, [WC_STA_WQUE20] ASC)
    INCLUDE([WF_QUE], [WF_SUB_QUE], [WN_CTL_TSK], [PF_REQ_ACT], [WF_LST_DTS_WQ20], [WF_CRT_DTS_WQ20]);


GO
CREATE NONCLUSTERED INDEX [IX_WFQUE_WDINITSK_WCSTAWQUE20]
    ON [dbo].[WQ21_TSK_QUE_HST]([WF_QUE] ASC, [WD_INI_TSK] ASC, [WC_STA_WQUE20] ASC)
    INCLUDE([WF_SUB_QUE], [WD_ACT_REQ], [WF_USR_ASN_TSK], [WF_LST_DTS_WQ20], [BF_SSN]);


GO
CREATE NONCLUSTERED INDEX [IX_WCSTAWQUE20]
    ON [dbo].[WQ21_TSK_QUE_HST]([WC_STA_WQUE20] ASC)
    INCLUDE([WF_QUE], [WF_SUB_QUE], [WN_CTL_TSK], [PF_REQ_ACT], [WF_USR_ASN_TSK], [WF_LST_DTS_WQ20], [WF_CRT_DTS_WQ20]);

