CREATE TABLE [dbo].[DCR_03031_DF10] (
    [bad_deferments_forbearances_id] INT          NULL,
    [BF_SSN]                         CHAR (9)     NULL,
    [LF_DFR_CTL_NUM]                 BIGINT       NULL,
    [LC_DFR_TYP]                     CHAR (2)     NULL,
    [LD_DFR_REQ_BEG]                 DATE         NULL,
    [LD_DFR_REQ_END]                 DATE         NULL,
    [LD_CRT_REQ_DFR]                 VARCHAR (12) NOT NULL,
    [LI_CAP_DFR_INT_REQ]             CHAR (1)     NULL,
    [LC_DFR_STA]                     VARCHAR (1)  NOT NULL,
    [LD_STA_DFR10]                   VARCHAR (12) NOT NULL,
    [LC_STA_DFR10]                   VARCHAR (1)  NOT NULL
);

