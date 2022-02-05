CREATE TABLE [complaints].[ComplaintTypes] (
    [ComplaintTypeId] INT           IDENTITY (1, 1) NOT NULL,
    [TypeName]        VARCHAR (50)  NULL,
    [AddedOn]         DATETIME      DEFAULT (getdate()) NOT NULL,
    [AddedBy]         NVARCHAR (50) DEFAULT (suser_sname()) NOT NULL,
    [DeletedOn]       DATETIME      NULL,
    [DeletedBy]       NVARCHAR (50) NULL,
    PRIMARY KEY CLUSTERED ([ComplaintTypeId] ASC)
);

