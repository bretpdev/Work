CREATE TYPE [dbo].[BatchLettersData] AS TABLE (
    [BatchLettersId]          INT            NULL,
    [LetterId]                VARCHAR (10)   NULL,
    [SasFilePattern]          VARCHAR (50)   NULL,
    [StateFieldCodeName]      VARCHAR (25)   NULL,
    [AccountNumberFieldName]  VARCHAR (25)   NULL,
    [CostCenterFieldCodeName] VARCHAR (25)   NULL,
    [IsDuplex]                BIT            NULL,
    [OkIfMissing]             BIT            NULL,
    [ProcessAllFiles]         BIT            NULL,
    [Arc]                     VARCHAR (5)    NULL,
    [Comment]                 VARCHAR (1200) NULL,
    [CreatedAt]               DATETIME       NULL,
    [CreatedBy]               VARCHAR (250)  NULL,
    [UpdatedAt]               DATETIME       NULL,
    [UpdatedBy]               VARCHAR (250)  NULL,
    [Active]                  BIT            NULL);

