CREATE TABLE [dbo].[Catch_Up_Income_Data]
(
	[Catch_Up_Income_Data_Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [Catch_Up_Income_Id] INT NOT NULL, 
    [Year] INT NOT NULL, 
    [AGI] DECIMAL(18, 2) NOT NULL, 
    [State] CHAR(2) NOT NULL, 
    [Catch_Up_Family_Size] INT NOT NULL, 
    [Source_Code] VARCHAR(3) NOT NULL, 
    [External_Debt] DECIMAL(18, 2) NOT NULL, 
    CONSTRAINT [FK_Catch_Up_Income_Data_ToCatch_Up_Income] FOREIGN KEY (Catch_Up_Income_Id) REFERENCES Catch_Up_Income(Catch_Up_Income_Id)
)
