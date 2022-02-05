CREATE TABLE [dbo].[LoginType] (
    [LoginTypeId] INT           IDENTITY (1, 1) NOT NULL,
    [LoginType]   VARCHAR (150) NOT NULL,
    [MaxInUse]    INT           DEFAULT ((0)) NOT NULL,
    PRIMARY KEY CLUSTERED ([LoginTypeId] ASC),
    CONSTRAINT [AK_LoginType_LoginType] UNIQUE NONCLUSTERED ([LoginType] ASC)
);


