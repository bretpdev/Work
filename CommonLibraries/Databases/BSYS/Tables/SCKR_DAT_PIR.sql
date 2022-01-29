CREATE TABLE [dbo].[SCKR_DAT_PIR] (
    [Request]           INT           NOT NULL,
    [Class]             NVARCHAR (3)  NOT NULL,
    [ReviewNo]          INT           NOT NULL,
    [Reviewer]          NVARCHAR (50) NULL,
    [Comments]          NTEXT         NULL,
    [ReturnReason]      NVARCHAR (50) NULL,
    [ReturnDescription] NTEXT         NULL,
    [Status]            NVARCHAR (50) NULL,
    [StatusDate]        SMALLDATETIME NULL,
    [Update]            NTEXT         NULL,
    [History]           NTEXT         NULL,
    [CurrentStatus]     NVARCHAR (50) NULL,
    [CurrentStatusDate] SMALLDATETIME CONSTRAINT [DF_SCKR_DAT_PIR_CurrentStatusDate] DEFAULT (convert(datetime,floor(convert(real,getdate())))) NULL,
    [Court]             NVARCHAR (50) NULL,
    [CourtDate]         SMALLDATETIME CONSTRAINT [DF_SCKR_DAT_PIR_CourtDate] DEFAULT (convert(datetime,floor(convert(real,getdate())))) NULL,
    [PreviousStatus]    NVARCHAR (50) NULL,
    CONSTRAINT [PK_datPIR] PRIMARY KEY CLUSTERED ([Request] ASC, [Class] ASC, [ReviewNo] ASC) WITH (FILLFACTOR = 90)
);

