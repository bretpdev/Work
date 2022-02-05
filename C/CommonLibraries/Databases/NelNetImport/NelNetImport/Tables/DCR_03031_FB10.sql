CREATE TABLE [dbo].[DCR_03031_FB10] (
    [bad_deferments_forbearances_id] INT          NULL,
    [BF_SSN]                         CHAR (9)     NULL,
    [LF_FOR_CTL_NUM]                 BIGINT       NULL,
    [LC_FOR_TYP]                     CHAR (2)     NULL,
    [LC_FOR_SUB_TYP]                 VARCHAR (1)  NOT NULL,
    [LD_FOR_REQ_BEG]                 DATE         NULL,
    [LD_FOR_REQ_END]                 DATE         NULL,
    [LD_CRT_REQ_FOR]                 VARCHAR (12) NOT NULL,
    [LI_CAP_FOR_INT_REQ]             CHAR (1)     NULL,
    [LC_FOR_STA]                     VARCHAR (1)  NOT NULL,
    [LC_STA_FOR10]                   VARCHAR (1)  NOT NULL,
    [LF_LST_DTS_FB10]                VARCHAR (12) NOT NULL
);

