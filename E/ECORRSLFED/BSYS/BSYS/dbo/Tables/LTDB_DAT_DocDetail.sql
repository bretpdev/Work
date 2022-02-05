CREATE TABLE [dbo].[LTDB_DAT_DocDetail] (
    [DocDetailId]    INT           IDENTITY (1, 1) NOT NULL,
    [DocName]        NVARCHAR (50) NOT NULL,
    [DocSeqNo]       INT           NULL,
    [DocTyp]         NVARCHAR (50) NULL,
    [Status]         NVARCHAR (50) CONSTRAINT [DF_LTDB_DAT_DocDetail_Status] DEFAULT (N'Development') NULL,
    [ID]             NVARCHAR (10) NULL,
    [Code]           NVARCHAR (4)  NULL,
    [Description]    NTEXT         NULL,
    [Letterhead]     NVARCHAR (50) NULL,
    [Model]          NVARCHAR (50) NULL,
    [ActCd]          NVARCHAR (50) NULL,
    [ARC]            NVARCHAR (50) NULL,
    [DocID]          NVARCHAR (50) NULL,
    [CostCd]         NVARCHAR (50) CONSTRAINT [DF_LTDB_DAT_DocDetail_CostCd] DEFAULT (N'000') NULL,
    [Addressee]      NVARCHAR (50) NULL,
    [Regarding]      NVARCHAR (50) NULL,
    [Recip]          NVARCHAR (50) CONSTRAINT [DF_LTDB_DAT_DocDetail_Recip] DEFAULT (N'Mail Room') NULL,
    [AltRecip]       NVARCHAR (50) CONSTRAINT [DF_LTDB_DAT_DocDetail_AltRecip] DEFAULT (N'Mail Room') NULL,
    [Unit]           NVARCHAR (50) NULL,
    [ACSParticipant] NVARCHAR (50) NULL,
    [LPD]            NVARCHAR (50) NULL,
    [Addresse]       NVARCHAR (50) NULL,
    [OtherAddressee] NVARCHAR (50) NULL,
    [Citation]       NVARCHAR (50) NULL,
    [ReqLang]        NTEXT         NULL,
    [Path_old]       NVARCHAR (50) CONSTRAINT [DF_LTDB_DAT_DocDetail_Path_old] DEFAULT (N'X:\PADD\') NULL,
    [BCPCriticality] INT           NULL,
    [Compliance]     BIT           CONSTRAINT [DF_LTDB_DAT_DocDetail_Compliance] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_LTDB_DAT_DocDetail] PRIMARY KEY CLUSTERED ([DocName] ASC) WITH (FILLFACTOR = 90)
);


GO
CREATE NONCLUSTERED INDEX [IX_LTDB_DAT_DocDetail_ID]
    ON [dbo].[LTDB_DAT_DocDetail]([ID] ASC) WITH (FILLFACTOR = 85, DATA_COMPRESSION = PAGE);

