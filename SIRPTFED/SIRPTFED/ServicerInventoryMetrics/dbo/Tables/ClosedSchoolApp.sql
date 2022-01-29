CREATE TABLE [dbo].[ClosedSchoolApp] (
    [ClosedSchoolAppId] INT       IDENTITY (1, 1) NOT NULL,
    [BF_SSN]            CHAR (9)  NOT NULL,
    [WF_QUE]            CHAR (2)  NOT NULL,
    [WF_SUB_QUE]        CHAR (2)  NOT NULL,
    [WN_CTL_TSK]        CHAR (18) NOT NULL,
    [PF_REQ_ACT]        CHAR (5)  NOT NULL,
    [WD_ACT_REQ]        DATETIME  NOT NULL,
    [WC_STA_WQUE20]     CHAR (1)  NOT NULL,
    [WF_LST_DTS_WQ20]   DATETIME  NOT NULL,
    PRIMARY KEY CLUSTERED ([ClosedSchoolAppId] ASC)
);

