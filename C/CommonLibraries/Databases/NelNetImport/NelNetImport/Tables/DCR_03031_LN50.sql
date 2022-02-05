CREATE TABLE [dbo].[DCR_03031_LN50] (
    [bad_deferments_forbearances_id] INT          NULL,
    [BF_SSN]                         CHAR (9)     NULL,
    [LN_SEQ]                         INT          NULL,
    [LD_DFR_BEG]                     DATE         NULL,
    [LD_DFR_END]                     DATE         NULL,
    [LF_LST_DTS_LN50]                VARCHAR (12) NOT NULL,
    [LC_STA_LON50]                   VARCHAR (1)  NOT NULL,
    [LD_STA_LON50]                   VARCHAR (12) NOT NULL,
    [LD_DFR_APL]                     DATE         NULL,
    [LC_LON_LEV_FOR_CAP]             CHAR (1)     NULL
);

