CREATE TABLE [dbo].[AC11_ACT_REQ_LTR] (
    [PF_REQ_ACT]         CHAR (5)      NOT NULL,
    [PF_LTR]             CHAR (10)     NOT NULL,
    [PF_LST_DTS_AC11]    DATETIME2 (7) NOT NULL,
    [PI_BKR_EDS_SND_LTR] CHAR (1)      NOT NULL,
    [PI_BKR_ATN_SND_LTR] CHAR (1)      NOT NULL,
    [PI_BKR_LTR_OVR]     CHAR (1)      NOT NULL
);

