CREATE TABLE [dbo].[Projects] (
    [ProjectId] INT            IDENTITY (1, 1) NOT NULL,
    [Name]      NVARCHAR (50)  NOT NULL,
    [Notes]     NVARCHAR (200) NULL,
    [IsFederal] BIT NOT NULL DEFAULT 0, 
	[Retired] BIT NOT NULL DEFAULT 0,
    CONSTRAINT [PK_Projects] PRIMARY KEY CLUSTERED ([ProjectId] ASC)
);

