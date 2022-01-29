CREATE TABLE [dbo].[CompassPifBwrLevel] (
    [AccountNumber]  VARCHAR (10)  NOT NULL,
    [FirstName]      VARCHAR (100) NOT NULL,
    [LastName]       VARCHAR (100) NOT NULL,
    [Address1]       VARCHAR (200) NOT NULL,
    [Address2]       VARCHAR (200) NULL,
    [City]           VARCHAR (50)  NOT NULL,
    [State]          VARCHAR (15)  NULL,
    [Zip]            VARCHAR (25)  NOT NULL,
    [Country]        VARCHAR (150) NULL,
    [EffectiveDate]  VARCHAR (10)  NOT NULL,
    [ConsolPif]      BIT           NOT NULL,
    [CostCenterCode] VARCHAR (10)  NOT NULL
);

