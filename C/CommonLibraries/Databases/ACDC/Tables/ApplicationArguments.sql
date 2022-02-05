CREATE TABLE [dbo].[ApplicationArguments]
(
	[ApplicationArgumentId] INT NOT NULL PRIMARY KEY IDENTITY, 
    [ApplicationId] INT NOT NULL, 
    [ArgumentId] INT NOT NULL, 
    [ArgumentOrder] INT NOT NULL, 
    CONSTRAINT [FK_ApplicationArguments_Applications] FOREIGN KEY ([ApplicationId]) REFERENCES [Applications]([ApplicationId]), 
    CONSTRAINT [FK_ApplicationArguments_Arguments] FOREIGN KEY ([ArgumentId]) REFERENCES [Arguments]([ArgumentId])
)
