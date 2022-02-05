CREATE TABLE [peps].[OTHERADD] (
    [OTHERADD_ID]         BIGINT        IDENTITY (1, 1) NOT NULL,
    [RecordType]          VARCHAR (2)   NULL,
    [OpeId]               VARCHAR (8)   NULL,
    [ChangeIndicator]     VARCHAR (1)   NULL,
    [AddressType]         VARCHAR (2)   NULL,
    [Line1Adr]            VARCHAR (35)  NULL,
    [Line2Adr]            VARCHAR (35)  NULL,
    [City]                VARCHAR (25)  NULL,
    [State]               VARCHAR (2)   NULL,
    [Country]             VARCHAR (25)  NULL,
    [Zip]                 VARCHAR (14)  NULL,
    [ForeignProvinceName] VARCHAR (25)  NULL,
    [OtherAreaCode]       VARCHAR (3)   NULL,
    [OtherExchange]       VARCHAR (3)   NULL,
    [OtherExt]            VARCHAR (4)   NULL,
    [OtherExt2]           VARCHAR (5)   NULL,
    [OtherForeignPhone]   VARCHAR (14)  NULL,
    [OtherFax]            VARCHAR (14)  NULL,
    [OtherInternetAdr]    VARCHAR (50)  NULL,
    [FscLocName]          VARCHAR (70)  NULL,
    [FscContFirstName]    VARCHAR (15)  NULL,
    [FscContLastName]     VARCHAR (30)  NULL,
    [Filler]              VARCHAR (256) NULL,
    [ProcessedAt]         DATETIME      NULL,
    [DeletedAt]           DATETIME      NULL,
    PRIMARY KEY CLUSTERED ([OTHERADD_ID] ASC) WITH (FILLFACTOR = 95)
);



