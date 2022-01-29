CREATE TABLE [dbo].[Income_Percentage_Factors]
(
	[income_percentage_factor_id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [married_or_head_of_household] BIT NOT NULL, 
    [income] DECIMAL(9, 2) NOT NULL, 
    [factor] DECIMAL(5, 4) NOT NULL,
	[start_date] DATETIME,
	[end_date] DATETIME
)
