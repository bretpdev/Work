CREATE TABLE [dbo].[DCR_03031_LN60] (
    [bad_deferments_forbearances_id] INT          NULL,
    [BF_SSN]                         CHAR (9)     NULL,
    [LN_SEQ]                         INT          NULL,
    [LD_FOR_BEG]                     DATE         NULL,
    [LD_FOR_END]                     DATE         NULL,
    [LD_STA_LON60]                   VARCHAR (12) NOT NULL,
    [LC_STA_LON60]                   VARCHAR (1)  NOT NULL,
    [LD_FOR_APL]                     DATE         NULL,
    [LF_LST_DTS_LN60]                VARCHAR (12) NOT NULL,
    [LC_LON_LEV_FOR_CAP]             CHAR (1)     NULL
);

