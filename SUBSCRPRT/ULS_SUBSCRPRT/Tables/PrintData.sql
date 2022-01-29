CREATE TABLE [subscrprt].[PrintData] (
    [PrintDataId] INT          IDENTITY (1, 1) NOT NULL,
    [BF_SSN]      CHAR (9)     NOT NULL,
    [CLUID]       CHAR (19)    NOT NULL,
	[DM_PRS_LST]  VARCHAR (35) NOT NULL,
    [ProcessedAt] DATETIME     NULL,
    [AddedAt]     DATETIME     DEFAULT (getdate()) NOT NULL,
    [AddedBy]     VARCHAR (50) DEFAULT (suser_sname()) NOT NULL,
    [DeletedAt]   DATETIME     NULL,
    [DeletedBy]   VARCHAR (50) NULL,
    PRIMARY KEY CLUSTERED ([PrintDataId] ASC) WITH (FILLFACTOR = 95)
);

