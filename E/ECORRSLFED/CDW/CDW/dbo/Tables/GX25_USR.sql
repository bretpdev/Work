CREATE TABLE [dbo].[GX25_USR] (
    [XF_USR]          CHAR (8)      NOT NULL,
    [XF_LST_DTS_GX25] DATETIME2 (7) NOT NULL,
    [XC_GRP]          CHAR (4)      NOT NULL,
    [XC_SUB_GRP]      CHAR (4)      NOT NULL,
    [XC_USR_TYP]      CHAR (1)      NOT NULL,
    [XM_USR_LST]      CHAR (35)     NOT NULL,
    [XM_USR_1]        CHAR (12)     NOT NULL,
    [XM_USR_MID]      CHAR (1)      NOT NULL,
    CONSTRAINT [PK_GX25_USR] PRIMARY KEY CLUSTERED ([XF_USR] ASC) WITH (FILLFACTOR = 95)
);

