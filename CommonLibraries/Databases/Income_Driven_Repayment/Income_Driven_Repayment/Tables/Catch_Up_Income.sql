CREATE TABLE [dbo].[Catch_Up_Income]
(
	[Catch_Up_Income_Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [application_id] INT NOT NULL, 
    [Repaye_End_Date] DATETIME NOT NULL, 
    [Create_Date] DATETIME NOT NULL DEFAULT getdate(), 
    CONSTRAINT [FK_Catch_Up_Income_ToApplications] FOREIGN KEY (application_id) REFERENCES Applications(application_id)
)
