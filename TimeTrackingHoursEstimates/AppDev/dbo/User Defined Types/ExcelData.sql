CREATE TYPE [dbo].[ExcelData] AS TABLE (
    [Date]           DATE            NULL,
    [Hours]          DECIMAL (18, 2) NULL,
    [SR]             VARCHAR (500)   NULL,
    [SASR]           VARCHAR (500)   NULL,
    [LTR]            VARCHAR (500)   NULL,
    [PROJ]           VARCHAR (500)   NULL,
    [GenericMeeting] VARCHAR (500)   NULL,
    [FSACR]          VARCHAR (500)   NULL,
    [CostCenter]     VARCHAR (500)   NULL,
    [Agent]          VARCHAR (500)   NULL,
    [FileName]       VARCHAR (500)   NULL);

