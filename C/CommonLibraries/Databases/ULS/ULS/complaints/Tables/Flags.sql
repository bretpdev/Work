CREATE TABLE [complaints].[Flags] (
    [FlagId]                   INT           IDENTITY (1, 1) NOT NULL,
    [FlagName]                 NVARCHAR (50) NOT NULL,
    [EnablesControlMailFields] BIT           DEFAULT ((0)) NOT NULL,
    [AddedOn]                  DATETIME      DEFAULT (getdate()) NOT NULL,
    [AddedBy]                  NVARCHAR (50) DEFAULT (suser_sname()) NOT NULL,
    [DeletedOn]                DATETIME      NULL,
    [DeletedBy]                NVARCHAR (50) NULL,
    PRIMARY KEY CLUSTERED ([FlagId] ASC)
);

