CREATE TABLE [complaints].[ComplaintGroups] (
    [ComplaintGroupId] INT           IDENTITY (1, 1) NOT NULL,
    [GroupName]        NVARCHAR (50) NOT NULL,
    [AddedOn]          DATETIME      DEFAULT (getdate()) NOT NULL,
    [AddedBy]          NVARCHAR (50) DEFAULT (suser_sname()) NOT NULL,
    [DeletedOn]        DATETIME      NULL,
    [DeletedBy]        NVARCHAR (50) NULL,
    PRIMARY KEY CLUSTERED ([ComplaintGroupId] ASC)
);

