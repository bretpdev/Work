CREATE TABLE [dbo].[LN20_EDS] (
    [BF_SSN]             CHAR (9)      NOT NULL,
    [LN_EDS_SEQ]         SMALLINT      NOT NULL,
    [LC_STA_LON20]       CHAR (1)      NOT NULL,
    [LC_REL_TO_BR]       VARCHAR (2)   NOT NULL,
    [LC_EDS_TYP]         CHAR (1)      NOT NULL,
    [LF_EDS]             CHAR (9)      NOT NULL,
    [LF_LST_DTS_LN20]    DATETIME2 (7) NOT NULL,
    [LN_SEQ]             SMALLINT      NULL,
    [AN_SEQ]             SMALLINT      NULL,
    [IC_LON_PGM]         VARCHAR (6)   NULL,
    [AN_SEQ_WK79]        SMALLINT      NULL,
    [LD_EFF_EDS_HST]     DATE          NULL,
    [LC_REA_EDS_HST]     CHAR (1)      NOT NULL,
    [LF_LST_USR_HST_EDS] VARCHAR (8)   NOT NULL,
    [AF_APL_ID]          VARCHAR (9)   NULL,
    [AN_LC_APL_SEQ]      SMALLINT      NULL
);

