CREATE TABLE [dbo].[CL10_CLM_PCL] (
    [BF_SSN]             CHAR (9)      NOT NULL,
    [LN_SEQ_CLM_PCL]     SMALLINT      NOT NULL,
    [LC_REA_CLM_PCL]     VARCHAR (2)   NOT NULL,
    [LC_TYP_REC_CLM_PCL] CHAR (1)      NOT NULL,
    [LF_USR_ASN_CLM_PCL] VARCHAR (8)   NOT NULL,
    [LI_CLM_GTR_RCV]     CHAR (1)      NOT NULL,
    [LD_CLM_RQR]         DATE          NULL,
    [LF_CLM_BCH]         VARCHAR (7)   NULL,
    [LF_LST_DTS_CL10]    DATETIME2 (7) NOT NULL,
    [LI_CLM_QA]          CHAR (1)      NOT NULL,
    [LD_GTR_CLM_RCI]     DATE          NULL,
    [LC_GTR_CLM_ACK]     CHAR (1)      NOT NULL,
    [LC_CAN_STA_CCI]     CHAR (1)      NOT NULL,
    [LI_CLM_CLL_RCV]     CHAR (1)      NOT NULL,
    [LC_XCP_PRF]         CHAR (1)      NOT NULL,
    CONSTRAINT [PK_CL10_CLM_PCL] PRIMARY KEY CLUSTERED ([BF_SSN] ASC, [LN_SEQ_CLM_PCL] ASC)
);

