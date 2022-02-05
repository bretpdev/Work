CREATE TABLE [dbo].[QCTR_DAT_Approved] (
    [ID]      INT           IDENTITY (1, 1) NOT NULL,
    [IssueID] INT           NOT NULL,
    [BU]      NVARCHAR (50) NOT NULL,
    CONSTRAINT [PK_QCTR_DAT_Approved] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_QCTR_DAT_Approved_GENR_LST_BusinessUnits] FOREIGN KEY ([BU]) REFERENCES [dbo].[GENR_LST_BusinessUnits] ([BusinessUnit]) ON UPDATE CASCADE
);

