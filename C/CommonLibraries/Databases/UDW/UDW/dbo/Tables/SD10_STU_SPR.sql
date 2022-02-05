CREATE TABLE [dbo].[SD10_STU_SPR] (
    [LF_STU_SSN]         CHAR (9)      NOT NULL,
    [LN_STU_SPR_SEQ]     SMALLINT      NOT NULL,
    [LD_STA_STU10]       DATE          NULL,
    [LC_STA_STU10]       CHAR (1)      NOT NULL,
    [LD_NTF_SCL_SPR]     DATE          NULL,
    [LC_SCR_SCL_SPR]     VARCHAR (2)   NOT NULL,
    [LC_REA_SCL_SPR]     VARCHAR (2)   NOT NULL,
    [LD_SCL_SPR]         DATE          NULL,
    [LF_DOE_SCL_ENR_CUR] VARCHAR (8)   NOT NULL,
    [LF_LST_DTS_SD10]    DATETIME2 (7) NOT NULL,
    [IF_HSP]             CHAR (6)      NULL,
    [LD_SCL_CER_STU_STA] DATE          NULL,
    [LD_ENR_STA_EFF_CAM] DATE          NULL,
    [LC_SEP_DTE_TRT_SRC] CHAR (2)      NOT NULL,
    [LF_LST_USR_SD10]    VARCHAR (8)   NOT NULL,
    [LX_GTR_ID_CHG_SRC]  VARCHAR (3)   NOT NULL
);

