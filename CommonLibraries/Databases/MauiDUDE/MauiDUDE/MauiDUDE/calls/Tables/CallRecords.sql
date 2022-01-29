CREATE TABLE [calls].[CallRecords] (
    [CallRecordId]  INT           IDENTITY (1, 1) NOT NULL,
    [ReasonId]      INT           NOT NULL,
    [Comments]      NVARCHAR (30) NULL,
    [LetterID]      VARCHAR (10)  NULL,
    [IsCornerstone] BIT           NOT NULL,
    [IsOutbound]    BIT           NOT NULL,
    [RecordedOn]    DATETIME      DEFAULT (getdate()) NOT NULL,
    [RecordedBy]    NVARCHAR (50) DEFAULT (suser_sname()) NOT NULL,
    PRIMARY KEY CLUSTERED ([CallRecordId] ASC),
    CONSTRAINT [FK_CallRecords_Reasons] FOREIGN KEY ([ReasonId]) REFERENCES [calls].[Reasons] ([ReasonId])
);

