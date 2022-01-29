CREATE TABLE [dbo].[LTDB_DAT_CentralPrintingDocData] (
    [DocSeqNo]        NUMERIC (19) NOT NULL,
    [ID]              VARCHAR (10) NULL,
    [Pages]           NUMERIC (18) CONSTRAINT [DF_LTDB_DAT_CentralPrintingDocData_Pages] DEFAULT ((0)) NOT NULL,
    [Instructions]    TEXT         NULL,
    [Duplex]          BIT          CONSTRAINT [DF_LTDB_DAT_CentralPrintingDocData_Duplex] DEFAULT ((0)) NOT NULL,
    [UHEAACostCenter] VARCHAR (50) NULL,
    [Path]            VARCHAR (50) NULL,
    [ResendMail]      BIT          CONSTRAINT [DF_LTDB_DAT_CentralPrintingDocData_ResendMail] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_LTDB_DAT_CentralPrintingDocData] PRIMARY KEY CLUSTERED ([DocSeqNo] ASC) WITH (FILLFACTOR = 90)
);

