CREATE TABLE [dbo].[Catch_Up_Income] (
    [Catch_Up_Income_Id] INT      IDENTITY (1, 1) NOT NULL,
    [application_id]     INT      NOT NULL,
    [Repaye_End_Date]    DATETIME NOT NULL,
    [Create_Date]        DATETIME DEFAULT (getdate()) NOT NULL,
    PRIMARY KEY CLUSTERED ([Catch_Up_Income_Id] ASC),
    CONSTRAINT [FK_Catch_Up_Income_ToApplications] FOREIGN KEY ([application_id]) REFERENCES [dbo].[Applications] ([application_id])
);

