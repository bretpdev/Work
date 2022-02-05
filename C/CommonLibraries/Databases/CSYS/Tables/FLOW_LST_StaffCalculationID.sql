CREATE TABLE [dbo].[FLOW_LST_StaffCalculationID] (
    [CalculationID] VARCHAR (100) NOT NULL,
    [System]        VARCHAR (30)  NOT NULL,
    CONSTRAINT [PK_FLOW_LST_StaffCalculationID] PRIMARY KEY CLUSTERED ([CalculationID] ASC),
    CONSTRAINT [FK_FLOW_LST_StaffCalculationID_GENR_LST_System] FOREIGN KEY ([System]) REFERENCES [dbo].[GENR_LST_System] ([System])
);

