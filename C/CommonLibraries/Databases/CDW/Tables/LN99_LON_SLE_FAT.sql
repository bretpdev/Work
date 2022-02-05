CREATE TABLE [dbo].[LN99_LON_SLE_FAT] (
    [BF_SSN]             CHAR (9)        NOT NULL,
    [LN_SEQ]             SMALLINT        NOT NULL,
    [LN_FAT_SEQ]         SMALLINT        NOT NULL,
    [IF_LON_SLE]         VARCHAR (7)     NOT NULL,
    [LF_LST_DTS_LN99]    DATETIME2 (7)   NOT NULL,
    [IF_SLL_OWN_SLD]     VARCHAR (8)     NULL,
    [IF_BUY_OWN_SLD]     VARCHAR (8)     NULL,
    [LA_STD_STD_ISL_DCV] NUMERIC (12, 2) NULL
);



