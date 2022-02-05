CREATE TABLE [dbo].[PRNT_REF_BUsXArcsAndQueues] (
    [BU]         NVARCHAR (50) NOT NULL,
    [Class]      VARCHAR (50)  NOT NULL,
    [ArcOrQueue] VARCHAR (10)  NOT NULL,
    CONSTRAINT [PK_PRNT_REF_BUsXArcsAndQueues] PRIMARY KEY CLUSTERED ([BU] ASC, [Class] ASC),
    CONSTRAINT [FK_PRNT_REF_BUsXArcsAndQueues_GENR_LST_BusinessUnits] FOREIGN KEY ([BU]) REFERENCES [dbo].[GENR_LST_BusinessUnits] ([BusinessUnit]) ON UPDATE CASCADE
);

