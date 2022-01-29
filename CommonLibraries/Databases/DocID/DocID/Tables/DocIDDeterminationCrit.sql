CREATE TABLE [dbo].[DocIDDeterminationCrit] (
    [RecNum]       INT           IDENTITY (1, 1) NOT NULL,
    [ResultOrder]  INT           NOT NULL,
    [DocID]        VARCHAR (50)  NOT NULL,
    [DocumentType] VARCHAR (500) NOT NULL,
    [LoanStatus]   VARCHAR (50)  NULL,
    [ReasonCode]   NVARCHAR (50) NULL,
    [Servicer]     NVARCHAR (50) NULL,
    CONSTRAINT [PK_DocIDDeterminationCrit] PRIMARY KEY CLUSTERED ([RecNum] ASC)
);

