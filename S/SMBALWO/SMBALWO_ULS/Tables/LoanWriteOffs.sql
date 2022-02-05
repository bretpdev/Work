CREATE TABLE [smbalwo].[LoanWriteOffs] (
    [LoanWriteOffId]            INT             IDENTITY (1, 1) NOT NULL,
    [DF_SPE_ACC_ID]             VARCHAR (10)    NOT NULL,
    [LN_SEQ]                    INT             NOT NULL,
    [LoanBalance]               DECIMAL (18, 2) NOT NULL, 
    [EffectiveDate] DATETIME NULL, 
    [ActualPrincipalWrittenOff] DECIMAL (18, 2) NULL,
    [ActualInterestWrittenOff]  DECIMAL (18, 2) NULL,
    [ProcessedAt]               DATETIME        NULL,
    [HadError]                  BIT             DEFAULT ((0)) NOT NULL,
	[IsTILP]					BIT				DEFAULT 0 NOT NULL,
    [AddedAt]                   DATETIME        DEFAULT (GETDATE()) NOT NULL,
    [AddedBy] VARCHAR(50) NOT NULL DEFAULT SUSER_SNAME(),
    [DeletedAt]                 DATETIME        NULL,
    [DeletedBy]                 VARCHAR (100)   NULL,
    PRIMARY KEY CLUSTERED ([LoanWriteOffId] ASC) WITH (FILLFACTOR = 95)
);

