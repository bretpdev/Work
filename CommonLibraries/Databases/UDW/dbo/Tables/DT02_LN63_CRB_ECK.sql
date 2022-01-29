﻿CREATE TABLE [dbo].[DT02_LN63_CRB_ECK] (
    [BF_SSN]             CHAR (9)      NOT NULL,
    [LN_SEQ]             SMALLINT      NOT NULL,
    [LD_RPT_CRB]         DATE          NOT NULL,
    [LN_CRB_ECK_SEQ]     SMALLINT      NOT NULL,
    [LN_CRB_ECK_CHG]     SMALLINT      NOT NULL,
    [LF_CRT_DTS_DT02]    DATETIME2 (7) NOT NULL,
    [LF_CRT_USR_DT02]    VARCHAR (12)  NOT NULL,
    [LC_CRB_ECK_ECOA]    CHAR (1)      NOT NULL,
    [LC_ECOA_UPD_TYP]    CHAR (1)      NOT NULL,
    [LC_ECK_CSM_INF_IND] VARCHAR (2)   NOT NULL,
    [LC_CSM_IND_UPD_TYP] CHAR (1)      NOT NULL,
    [LC_ECK_ADR_SGM_TYP] CHAR (1)      NOT NULL,
    CONSTRAINT [PK_DT02_LN63_CRB_ECK] PRIMARY KEY CLUSTERED ([BF_SSN] ASC, [LN_SEQ] ASC) WITH (FILLFACTOR = 95)
);

