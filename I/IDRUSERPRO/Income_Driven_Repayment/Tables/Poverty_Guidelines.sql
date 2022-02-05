CREATE TABLE [dbo].[Poverty_Guidelines]
(
    [year] SMALLINT NOT NULL PRIMARY KEY, 
    [continental_income] DECIMAL(9, 2) NOT NULL, 
    [alaska_income] DECIMAL(9, 2) NOT NULL, 
    [hawaii_income] DECIMAL(9, 2) NOT NULL,
	[continental_increment] DECIMAL(9, 2) NOT NULL, 
    [alaska_increment] DECIMAL(9, 2) NOT NULL, 
    [hawaii_increment] DECIMAL(9, 2) NOT NULL
)
