CREATE TABLE [scra].[DataComparison] (
    [DataComparisonId] INT      IDENTITY (1, 1) NOT NULL,
    [ActiveRow]        BIT      NULL,
    [Loan]             SMALLINT NULL,
    [StatusDate]       DATE     NULL,
    [BorrSSN]          CHAR (9) NULL,
    [BorrActive]       BIT      NULL,
    [EndrSSN]          CHAR (9) NULL,
    [EndrActive]       BIT      NULL,
    [BeginBrwr]        DATE     NULL,
    [EIDB]             BIT      NULL,
    [BeginEndr]        DATE     NULL,
    [EIDE]             BIT      NULL,
    [EndBrwr]          DATE     NULL,
    [EndEndr]          DATE     NULL,
    [BorrIsReservist]  BIT      NULL,
    [EndrIsReservist]  BIT      NULL,
    [CreatedAt]        DATETIME NULL,
    [ServiceComponent] CHAR(2) NULL, 
    PRIMARY KEY CLUSTERED ([DataComparisonId] ASC) WITH (FILLFACTOR = 95)
);

